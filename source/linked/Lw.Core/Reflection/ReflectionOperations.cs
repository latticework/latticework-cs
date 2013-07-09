using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Lw.Reflection
{
    public static class ReflectionOperations
    {
#if (!NETFX_CORE)
        public static IEnumerable<Type> GetAllTypes()
        {
            HashSet<Assembly> assemblyCatalog = new HashSet<Assembly>();
            Queue<Assembly> assemblyQueue = new Queue<Assembly>();

            var rootAssembly = Assembly.GetEntryAssembly();
            assemblyCatalog.Add(rootAssembly);
            assemblyQueue.Enqueue(rootAssembly);

            while (assemblyQueue.Any())
            {
                var assembly = assemblyQueue.Dequeue();
                foreach (var assemblyName in assembly.GetReferencedAssemblies())
                {
                    var childAssembly = Assembly.Load(assemblyName);

                    if (!assemblyCatalog.Contains(childAssembly))
                    {
                        assemblyCatalog.Add(childAssembly);
                        assemblyQueue.Enqueue(childAssembly);
                    }
                }
                foreach (var type in assembly.GetTypes())
                {
                    yield return type;
                }
            }
        }
#else
        // http://forums.silverlight.net/t/228954.aspx/1
        // http://www.wintellect.com/CS/blogs/jlikness/archive/2011/03/24/clean-design-time-friendly-viewmodels-a-walkthrough.aspx
        // http://www.davidpoll.com/2010/02/01/on-demand-loading-of-assemblies-with-silverlight-navigation-revisited-for-silverlight-4-beta/
        // http://www.davidpoll.com/2009/12/07/opening-up-silverlight-4-navigation-event-based-and-error-handling-inavigationcontentloaders/

#endif
    }
}
