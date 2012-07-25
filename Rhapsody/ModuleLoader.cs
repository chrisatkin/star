using System;
using System.Reflection;

namespace org.btg.Star.Rhapsody
{
    public static class ModuleLoader
    {
        public static IProvider LoadProvider(string location)
        {
            foreach (Type t in Assembly.LoadFrom(location).GetTypes())
            {
                if (t.GetInterface("IProvider") != null)
                {
                    return (IProvider) Activator.CreateInstance(t);
                }
            }

            return null;
        }

        public static ILogger LoadLogger(string location)
        {
            foreach (Type t in Assembly.LoadFrom(location).GetTypes())
            {
                if (t.GetInterface("ILogger") != null)
                {
                    return (ILogger) Activator.CreateInstance(t);
                }
            }

            return null;
        }
    }
}