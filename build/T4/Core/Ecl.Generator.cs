﻿using System;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

public class CodeGenerationTools
{
    private readonly DynamicTextTransformation _textTransformation;
    private readonly CSharpCodeProvider _code;

    public CodeGenerationTools(object textTransformation)
    {
        if (textTransformation == null)
        {
            throw new ArgumentNullException("textTransformation");
        }

        _textTransformation = DynamicTextTransformation.Create(textTransformation);
        _code = new CSharpCodeProvider();

    }

    public string Escape(string name)
    {
        if (name == null)
        {
            return null;
        }

        return _code.CreateEscapedIdentifier(name);
    }

}

/// <summary>
/// Responsible for marking the various sections of the generation,
/// so they can be split up into separate files
/// </summary>
public class TemplateFileManager
{
    /// <summary>
    /// Creates the VsTemplateFileManager if VS is detected, otherwise
    /// creates the file system version.
    /// </summary>
    public static TemplateFileManager Create(object textTransformation)
    {
        DynamicTextTransformation transformation = DynamicTextTransformation.Create(textTransformation);
        IDynamicHost host = transformation.Host;

#if !PREPROCESSED_TEMPLATE
        var hostServiceProvider = host.AsIServiceProvider();

        if (hostServiceProvider != null)
        {
            EnvDTE.DTE dte = (EnvDTE.DTE)hostServiceProvider.GetService(typeof(EnvDTE.DTE));

            if (dte != null)
            {
                return new VsTemplateFileManager(transformation);
            }
        }
#endif
        return new TemplateFileManager(transformation);
    }

    private sealed class Block
    {
        public String Name;
        public int Start, Length;
    }

    private readonly List<Block> files = new List<Block>();
    private readonly Block footer = new Block();
    private readonly Block header = new Block();
    private readonly DynamicTextTransformation _textTransformation;

    // reference to the GenerationEnvironment StringBuilder on the
    // TextTransformation object
    private readonly StringBuilder _generationEnvironment;

    private Block currentBlock;

    /// <summary>
    /// Initializes an TemplateFileManager Instance  with the
    /// TextTransformation (T4 generated class) that is currently running
    /// </summary>
    private TemplateFileManager(object textTransformation)
    {
        if (textTransformation == null)
        {
            throw new ArgumentNullException("textTransformation");
        }

        _textTransformation = DynamicTextTransformation.Create(textTransformation);
        _generationEnvironment = _textTransformation.GenerationEnvironment;
    }

    /// <summary>
    /// Marks the end of the last file if there was one, and starts a new
    /// and marks this point in generation as a new file.
    /// </summary>
    public void StartNewFile(string name)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }

        CurrentBlock = new Block { Name = name };
    }

    public void StartFooter()
    {
        CurrentBlock = footer;
    }

    public void StartHeader()
    {
        CurrentBlock = header;
    }

    public void EndBlock()
    {
        if (CurrentBlock == null)
        {
            return;
        }

        CurrentBlock.Length = _generationEnvironment.Length - CurrentBlock.Start;

        if (CurrentBlock != header && CurrentBlock != footer)
        {
            files.Add(CurrentBlock);
        }

        currentBlock = null;
    }

    /// <summary>
    /// Produce the template output files.
    /// </summary>
    public virtual IEnumerable<string> Process(bool split = true)
    {
        var generatedFileNames = new List<string>();

        if (split)
        {
            EndBlock();

            var headerText = _generationEnvironment.ToString(header.Start, header.Length);
            var footerText = _generationEnvironment.ToString(footer.Start, footer.Length);
            var outputPath = Path.GetDirectoryName(_textTransformation.Host.TemplateFile);

            files.Reverse();

            foreach (var block in files)
            {
                var fileName = Path.Combine(outputPath, block.Name);
                var content = headerText + _generationEnvironment.ToString(block.Start, block.Length) + footerText;

                generatedFileNames.Add(fileName);
                CreateFile(fileName, content);
                _generationEnvironment.Remove(block.Start, block.Length);
            }
        }

        return generatedFileNames;
    }

    protected virtual void CreateFile(string fileName, string content)
    {
        if (IsFileContentDifferent(fileName, content))
        {
            File.WriteAllText(fileName, content);
        }
    }

    protected bool IsFileContentDifferent(String fileName, string newContent)
    {
        return !(File.Exists(fileName) && File.ReadAllText(fileName) == newContent);
    }

    private Block CurrentBlock
    {
        get { return currentBlock; }
        set
        {
            if (CurrentBlock != null)
            {
                EndBlock();
            }

            if (value != null)
            {
                value.Start = _generationEnvironment.Length;
            }

            currentBlock = value;
        }
    }

#if !PREPROCESSED_TEMPLATE
    private sealed class VsTemplateFileManager : TemplateFileManager
    {
        private EnvDTE.ProjectItem templateProjectItem;
        private EnvDTE.DTE dte;
        private Action<string> checkOutAction;
        private Action<IEnumerable<string>> projectSyncAction;

        /// <summary>
        /// Creates an instance of the VsTemplateFileManager class with the IDynamicHost instance
        /// </summary>
        public VsTemplateFileManager(object textTemplating)
            : base(textTemplating)
        {
            var hostServiceProvider = _textTransformation.Host.AsIServiceProvider();
            if (hostServiceProvider == null)
            {
                throw new ArgumentNullException("Could not obtain hostServiceProvider");
            }

            dte = (EnvDTE.DTE)hostServiceProvider.GetService(typeof(EnvDTE.DTE));
            if (dte == null)
            {
                throw new ArgumentNullException("Could not obtain DTE from host");
            }

            templateProjectItem = dte.Solution.FindProjectItem(_textTransformation.Host.TemplateFile);

            checkOutAction = fileName => dte.SourceControl.CheckOutItem(fileName);
            projectSyncAction = keepFileNames => ProjectSync(templateProjectItem, keepFileNames);
        }

        public override IEnumerable<string> Process(bool split)
        {
            if (templateProjectItem.ProjectItems == null)
            {
                return new List<string>();
            }

            var generatedFileNames = base.Process(split);

            projectSyncAction.EndInvoke(projectSyncAction.BeginInvoke(generatedFileNames, null, null));

            return generatedFileNames;
        }

        protected override void CreateFile(string fileName, string content)
        {
            if (IsFileContentDifferent(fileName, content))
            {
                CheckoutFileIfRequired(fileName);
                File.WriteAllText(fileName, content);
            }
        }

        private static void ProjectSync(EnvDTE.ProjectItem templateProjectItem, IEnumerable<string> keepFileNames)
        {
            var keepFileNameSet = new HashSet<string>(keepFileNames);
            var projectFiles = new Dictionary<string, EnvDTE.ProjectItem>();
            var originalOutput = Path.GetFileNameWithoutExtension(templateProjectItem.FileNames[0]);

            foreach (EnvDTE.ProjectItem projectItem in templateProjectItem.ProjectItems)
            {
                projectFiles.Add(projectItem.FileNames[0], projectItem);
            }

            // Remove unused items from the project
            foreach (var pair in projectFiles)
            {
                if (!keepFileNames.Contains(pair.Key)
                    && !(Path.GetFileNameWithoutExtension(pair.Key) + ".").StartsWith(originalOutput + "."))
                {
                    pair.Value.Delete();
                }
            }

            // Add missing files to the project
            foreach (string fileName in keepFileNameSet)
            {
                if (!projectFiles.ContainsKey(fileName))
                {
                    templateProjectItem.ProjectItems.AddFromFile(fileName);
                }
            }
        }

        private void CheckoutFileIfRequired(string fileName)
        {
            if (dte.SourceControl == null
                || !dte.SourceControl.IsItemUnderSCC(fileName)
                    || dte.SourceControl.IsItemCheckedOut(fileName))
            {
                return;
            }

            // run on worker thread to prevent T4 calling back into VS
            checkOutAction.EndInvoke(checkOutAction.BeginInvoke(fileName, null, null));
        }
    }
#endif
}


/// <summary>
/// Responsible creating an instance that can be passed
/// to helper classes that need to access the TextTransformation
/// members.  It accesses member by name and signature rather than
/// by type.  This is necessary when the
/// template is being used in Preprocessed mode
/// and there is no common known type that can be
/// passed instead
/// </summary>
public class DynamicTextTransformation
{
    private object _instance;
    IDynamicHost _dynamicHost;

    private readonly MethodInfo _write;
    private readonly MethodInfo _writeLine;
    private readonly PropertyInfo _generationEnvironment;
    private readonly PropertyInfo _errors;
    private readonly PropertyInfo _host;

    /// <summary>
    /// Creates an instance of the DynamicTextTransformation class around the passed in
    /// TextTransformation shapped instance passed in, or if the passed in instance
    /// already is a DynamicTextTransformation, it casts it and sends it back.
    /// </summary>
    public static DynamicTextTransformation Create(object instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException("instance");
        }

        DynamicTextTransformation textTransformation = instance as DynamicTextTransformation;
        if (textTransformation != null)
        {
            return textTransformation;
        }

        return new DynamicTextTransformation(instance);
    }

    private DynamicTextTransformation(object instance)
    {
        _instance = instance;
        Type type = _instance.GetType();
        _write = type.GetMethod("Write", new Type[] { typeof(string) });
        _writeLine = type.GetMethod("WriteLine", new Type[] { typeof(string) });
        _generationEnvironment = type.GetProperty("GenerationEnvironment", BindingFlags.Instance | BindingFlags.NonPublic);
        _host = type.GetProperty("Host");
        _errors = type.GetProperty("Errors");
    }

    /// <summary>
    /// Gets the value of the wrapped TextTranformation instance's GenerationEnvironment property
    /// </summary>
    public StringBuilder GenerationEnvironment { get { return (StringBuilder)_generationEnvironment.GetValue(_instance, null); } }

    /// <summary>
    /// Gets the value of the wrapped TextTranformation instance's Errors property
    /// </summary>
    public System.CodeDom.Compiler.CompilerErrorCollection Errors { get { return (System.CodeDom.Compiler.CompilerErrorCollection)_errors.GetValue(_instance, null); } }

    /// <summary>
    /// Calls the wrapped TextTranformation instance's Write method.
    /// </summary>
    public void Write(string text)
    {
        _write.Invoke(_instance, new object[] { text });
    }

    /// <summary>
    /// Calls the wrapped TextTranformation instance's WriteLine method.
    /// </summary>
    public void WriteLine(string text)
    {
        _writeLine.Invoke(_instance, new object[] { text });
    }

    /// <summary>
    /// Gets the value of the wrapped TextTranformation instance's Host property
    /// if available (shows up when hostspecific is set to true in the template directive) and returns
    /// the appropriate implementation of IDynamicHost
    /// </summary>
    public IDynamicHost Host
    {
        get
        {
            if (_dynamicHost == null)
            {
                if (_host == null)
                {
                    _dynamicHost = new NullHost();
                }
                else
                {
                    _dynamicHost = new DynamicHost(_host.GetValue(_instance, null));
                }
            }
            return _dynamicHost;
        }
    }
}

/// <summary>
/// Reponsible for abstracting the use of Host between times
/// when it is available and not
/// </summary>
public interface IDynamicHost
{
    /// <summary>
    /// An abstracted call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolveParameterValue
    /// </summary>
    string ResolveParameterValue(string id, string name, string otherName);

    /// <summary>
    /// An abstracted call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolvePath
    /// </summary>
    string ResolvePath(string path);

    /// <summary>
    /// An abstracted call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost TemplateFile
    /// </summary>
    string TemplateFile { get; }

    /// <summary>
    /// Returns the Host instance cast as an IServiceProvider
    /// </summary>
    IServiceProvider AsIServiceProvider();
}

/// <summary>
/// Reponsible for implementing the IDynamicHost as a dynamic
/// shape wrapper over the Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost interface
/// rather than type dependent wrapper.  We don't use the
/// interface type so that the code can be run in preprocessed mode
/// on a .net framework only installed machine.
/// </summary>
public class DynamicHost : IDynamicHost
{
    private readonly object _instance;
    private readonly MethodInfo _resolveParameterValue;
    private readonly MethodInfo _resolvePath;
    private readonly PropertyInfo _templateFile;

    /// <summary>
    /// Creates an instance of the DynamicHost class around the passed in
    /// Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost shapped instance passed in.
    /// </summary>
    public DynamicHost(object instance)
    {
        _instance = instance;
        Type type = _instance.GetType();
        _resolveParameterValue = type.GetMethod("ResolveParameterValue", new Type[] { typeof(string), typeof(string), typeof(string) });
        _resolvePath = type.GetMethod("ResolvePath", new Type[] { typeof(string) });
        _templateFile = type.GetProperty("TemplateFile");

    }

    /// <summary>
    /// A call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolveParameterValue
    /// </summary>
    public string ResolveParameterValue(string id, string name, string otherName)
    {
        return (string)_resolveParameterValue.Invoke(_instance, new object[] { id, name, otherName });
    }

    /// <summary>
    /// A call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolvePath
    /// </summary>
    public string ResolvePath(string path)
    {
        return (string)_resolvePath.Invoke(_instance, new object[] { path });
    }

    /// <summary>
    /// A call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost TemplateFile
    /// </summary>
    public string TemplateFile
    {
        get
        {
            return (string)_templateFile.GetValue(_instance, null);
        }
    }

    /// <summary>
    /// Returns the Host instance cast as an IServiceProvider
    /// </summary>
    public IServiceProvider AsIServiceProvider()
    {
        return _instance as IServiceProvider;
    }
}

/// <summary>
/// Reponsible for implementing the IDynamicHost when the
/// Host property is not available on the TextTemplating type. The Host
/// property only exists when the hostspecific attribute of the template
/// directive is set to true.
/// </summary>
public class NullHost : IDynamicHost
{
    /// <summary>
    /// An abstraction of the call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolveParameterValue
    /// that simply retuns null.
    /// </summary>
    public string ResolveParameterValue(string id, string name, string otherName)
    {
        return null;
    }

    /// <summary>
    /// An abstraction of the call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolvePath
    /// that simply retuns the path passed in.
    /// </summary>
    public string ResolvePath(string path)
    {
        return path;
    }

    /// <summary>
    /// An abstraction of the call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost TemplateFile
    /// that returns null.
    /// </summary>
    public string TemplateFile
    {
        get
        {
            return null;
        }
    }

    /// <summary>
    /// Returns null.
    /// </summary>
    public IServiceProvider AsIServiceProvider()
    {
        return null;
    }
}
