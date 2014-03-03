using System;
using System.Reflection;

namespace org.btg.Star.Rhapsody
{
    public static class ProviderLoader
    {
        public static IProvider Load(string location)
        {
            Type[] types = Assembly.LoadFrom(location).GetTypes();

            foreach (Type t in types)
            {
                if (t.GetInterface("IProvider") != null)
                {
                    return (IProvider) Activator.CreateInstance(t);
                }
            }

            return null;
        }
    }
}
