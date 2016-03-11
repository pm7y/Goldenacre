using System.IO;
using System.Text;
using System.Xml;

namespace Goldenacre.Core.Extensions
{
    public static class XmlDocumentExtensions
    {
        public static string Prettify(this XmlDocument @this)
        {
            string result;

            using (var ms = new MemoryStream())
            {
                var w = new XmlTextWriter(ms, Encoding.Unicode);
                w.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                @this.WriteContentTo(w);
                w.Flush();
                ms.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                ms.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                var sr = new StreamReader(ms);

                // Extract the text from the StreamReader.
                var formattedXml = sr.ReadToEnd();

                result = formattedXml;

                w.Close();
            }

            return result;
        }
    }
}