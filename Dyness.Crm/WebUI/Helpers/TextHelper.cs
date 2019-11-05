using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebUI.Helpers
{
    public class TextHelper
    {
        public static string[] GetLines(string path)
        {
            string[] lines;
            var list = new List<string>();
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }

            lines = list.ToArray();

            return lines;
        }



    }
}