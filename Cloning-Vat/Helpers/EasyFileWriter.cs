using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cloning_Vat.Helpers
{
    public class EasyFileWriter
    {
        /// <summary>
        /// Writes out the specified text into a file in the execution directory and
        /// returns the file's path
        /// </summary>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        public string WriteFile(string filename, string fileContents)
        {
            string fullPath = $@"{Directory.GetCurrentDirectory()}\{filename}";

            // Create the directory if it doesn't exist
            string dir = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (FileStream stream = File.Open(fullPath, FileMode.OpenOrCreate))
            {
                // Clear the contents of the file if stuff is in there
                stream.SetLength(0);

                // Write out the xml in utf-16 format
                // We use utf-16 because c# strings are encoded that way and it likes it better
                using (StreamWriter writer = new StreamWriter(stream, Encoding.BigEndianUnicode))
                {
                    writer.Write(fileContents);
                }
            }
            //todo: Catch Exceptions

            return fullPath;
        }


    }
}
