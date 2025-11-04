using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace XSLTLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                Transform(args[0], args[1]);
            }
            else
            {
                PrintUsage();
            }
        }

        public static void Transform(string sXmlPath, string sXslPath)
        {
            try
            {
                //load the Xml doc
                XPathDocument myXPathDoc = new XPathDocument(sXmlPath);

                XslCompiledTransform myXslTrans = new XslCompiledTransform();

                //load the Xsl 
                myXslTrans.Load(sXslPath);

                //create the output stream
                XmlTextWriter myWriter = new XmlTextWriter
                    ("result.xml", null);

                //do the actual transform of Xml
                myXslTrans.Transform(myXPathDoc, null, myWriter);

                myWriter.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
            }
        }

        public static void PrintUsage()
        {
            Console.WriteLine
            ("Usage: XmlTransformUtil.exe <xml path> <xsl path>");
        }
    }
}
