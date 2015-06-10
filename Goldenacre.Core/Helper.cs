using System;
using System.Collections.Specialized;
using System.DirectoryServices;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace Goldenacre.Core
{
    public class Helper
    {
        public static Color GenerateRandomColour()
        {
            return Color.FromArgb(StaticRandom.Next(0, 255),
                StaticRandom.Next(0, 255),
                StaticRandom.Next(0, 255));
        }

        public static string GetHostNameFromIP(string ipAddress)
        {
            var strHostName = string.Empty;
            IPHostEntry objIPEntry = null;

            if ((ipAddress == null) || (ipAddress.Trim().Length <= 0))
            {
                throw new ArgumentNullException("ipAddress");
            }

            objIPEntry = Dns.GetHostEntry(ipAddress);

            strHostName = objIPEntry.HostName.ToString(CultureInfo.InvariantCulture);


            return strHostName;
        }

        public static String PrettyPrint(String XML)
        {
            String Result = "";

            MemoryStream MS = new MemoryStream();
            XmlTextWriter W = new XmlTextWriter(MS, Encoding.Unicode);
            XmlDocument D = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                D.LoadXml(XML);

                W.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                D.WriteContentTo(W);
                W.Flush();
                MS.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                MS.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader SR = new StreamReader(MS);

                // Extract the text from the StreamReader.
                String FormattedXML = SR.ReadToEnd();

                Result = FormattedXML;
            }
            catch (XmlException)
            {
            }

            MS.Close();
            W.Close();

            return Result;
        }

        public static StringCollection GetDomainList()
        {
            StringCollection domainList = new StringCollection();

            try
            {
                // instantiate the active directory entry object
                DirectoryEntry en = new DirectoryEntry("WinNT:");

                // check that there are child objects
                if (en.Children != null)
                {
                    // loop through the child objects
                    foreach (DirectoryEntry child in en.Children)
                    {
                        // make sure the child object is a domain
                        if ((child.SchemaClassName != null) && (child.SchemaClassName.ToUpper() == "DOMAIN"))
                        {
                            // add the domain to the collection
                            domainList.Add(child.Name.ToUpper());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return domainList;
        }

    }
}