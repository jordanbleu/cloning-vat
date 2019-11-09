using System;
using System.IO;
using System.Reflection;

namespace Cloning_Vat.Helpers
{
    public class EmbeddedResourceReader
    {
        /// <summary>
        /// Reads an embedded resource and returns it as a string.  Path should be something like:
        /// 'TestXml.Expected.xml' or whatever.  Also, the xml needs to have a build action of "embedded resource"
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public string ReadFile(string resource)
        {
            Assembly ass = Assembly.GetCallingAssembly();
            string fullyQualifiedResourceName = $"{ass.GetName().Name}.{resource}";

            using (Stream stream = ass.GetManifestResourceStream(fullyQualifiedResourceName))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Unable to find an embedded resource at path '{fullyQualifiedResourceName}'");
                }
            }
        }
    }
}
