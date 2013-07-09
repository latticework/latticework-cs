using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Lw.Reflection.CompilerServices
{
    public class MemberData
    {
        public string FileName { get; set; }
        public int LineNumber { get; set; }
        public string Name { get; set; }
        public Type RuntimeType { get; set; }

        //public MemberInfo DeclaredMember { get; set; }

        // http://simoncropp.com/callermembernameandruntimecode
        // http://www.kunal-chowdhury.com/2012/07/whats-new-in-c-50-learn-about.html
        // http://www.kunal-chowdhury.com/2012/07/whats-new-in-csharp-5-callerfilepath.html
        // http://www.kunal-chowdhury.com/2012/07/whats-new-in-csharp-50-callerlinenumber.html
        public static MemberData GetCurrent(
            object currentObject,
            [CallerMemberName] string name = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Contract.Requires<ArgumentNullException>(currentObject != null, "currentObject");

            return new MemberData
            {
                FileName = filePath,
                LineNumber = lineNumber,
                Name = name,
                RuntimeType = currentObject.GetType(),
            };

            //var typeInfo = currentObject.GetType().GetTypeInfo();

            //MemberInfo declaredMember;

            //if (name.EqualsOrdinal(".ctor"))
            //{
            //    declaredMember = typeInfo.GetConstructor(
            //}
            
            //declaredMember = 
            //       (MemberInfo)typeInfo.GetDeclaredEvent(name)
            //    ?? (MemberInfo)typeInfo.GetDeclaredField(name)
            //    ?? (MemberInfo)typeInfo.GetDeclaredMethod(name)
            //    ?? (MemberInfo)typeInfo.GetDeclaredProperty(name);


            //MemberInfo runtimeMember =
            //       (MemberInfo)typeInfo.GetRuntimeEvent(name)
            //    ?? (MemberInfo)typeInfo.GetRuntimeField(name)
            //    ?? (MemberInfo)typeInfo.GetRuntimeMethod(name, )
            //    ?? (MemberInfo)typeInfo.GetRuntimeProperty(name);

        }
    }
}
