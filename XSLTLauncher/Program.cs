using System;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
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
                //2.1
                Transform(args[0], args[1]);
                //2.2
                AddEmployeeSalaryAmount("result.xml");
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

        public static void AddEmployeeSalaryAmount(string resultPath)
        {
            //load the Xml doc
            XDocument doc = XDocument.Load(resultPath);

            // Культура, в которой запятая – разделитель дробной части
            var ruCulture = new CultureInfo("ru-RU");

            foreach (var emp in doc.Root.Elements("Employee"))
            {
                // Все salary‑элементы текущего сотрудника
                var salaries = emp.Elements("salary");

                double sum = salaries
                    .Select(s =>
                    {
                    // Берём строку атрибута amount
                    string txt = s.Attribute("amount").Value;

                    // Заменяем точку на запятую (и наоборот) — делаем валидным для Parse
                    txt = txt.Replace('.', ',').Replace(',', System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);

                    // Парсим в double, учитывая локаль
                    return double.Parse(txt, NumberStyles.Any, ruCulture);
                    })
                    .Sum();

                // Добавляем атрибут totalSalary
                emp.SetAttributeValue("totalSalary", sum.ToString("F2", CultureInfo.InvariantCulture));
            }

            // Сохраняем в файл:
            doc.Save("employees_with_total.xml");
        }
    }
}
