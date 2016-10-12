using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace Tutorial_Linq_To_XML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Section 1: Read XML and Traverse the XML Document using LINQ To XML

        /// <summary>
        /// 1 - Read XML using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo1_Click(object sender, EventArgs e)
        {
            XElement xelement = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            IEnumerable<XElement> employees = xelement.Elements();
            // Read the entire XML
            foreach (var employee in employees)
            {
                Console.WriteLine(employee);
            }
            
            XDocument xdocument = XDocument.Load("..\\..\\..\\..\\exemplo.xml");
            IEnumerable<XElement> employees2 = xdocument.Elements();
            foreach (var employee in employees2)
            {
                Console.WriteLine(employee);
            }
        }

        /// <summary>
        /// 2 - Access a Single Element using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo2_Click(object sender, EventArgs e)
        {
            XElement xelement = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            IEnumerable<XElement> employees = xelement.Elements();
            Console.WriteLine("List of all Employee Names :");
            foreach (var employee in employees)
            {
                Console.WriteLine(employee.Element("Name").Value);
            }
        }

        /// <summary>
        /// 3 - Access Multiple Elements using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo3_Click(object sender, EventArgs e)
        {
            XElement xelement = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            IEnumerable<XElement> employees = xelement.Elements();
            Console.WriteLine("List of all Employee Names along with their ID:");
            foreach (var employee in employees)
            {
                Console.WriteLine("{0} has Employee ID {1}",
                    employee.Element("Name").Value,
                    employee.Element("EmpId").Value);
            }
        }

        /// <summary>
        /// 4 - Access all Elements having a Specific Attribute using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo4_Click(object sender, EventArgs e)
        {
            XElement xelement = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            var name = from nm in xelement.Elements("Employee")
                       where (string)nm.Element("Sex") == "Female"
                       select nm;
            Console.WriteLine("Details of Female Employees:");
            foreach (XElement xEle in name)
                Console.WriteLine(xEle);
        }

        /// <summary>
        /// 5 - Access Specific Element having a Specific Attribute using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo5_Click(object sender, EventArgs e)
        {
            XElement xelement = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            var homePhone = from phoneno in xelement.Elements("Employee")
                            where (string)phoneno.Element("Sex") == "Female" 
                            where (string)phoneno.Element("Phone").Attribute("Type") == "Home"
                            select phoneno;
            Console.WriteLine("List HomePhone Nos.");
            foreach (XElement xEle in homePhone)
            {
                Console.WriteLine(xEle.Element("Phone").Value);
            }
        }

        /// <summary>
        /// 6 - Find an Element within another Element using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo6_Click(object sender, EventArgs e)
        {
            XElement xelement = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            var addresses = from address in xelement.Elements("Employee")
                            where (string)address.Element("Address").Element("City") == "Alta"
                            select address;
            Console.WriteLine("Details of Employees living in Alta City");
            foreach (XElement xEle in addresses)
                Console.WriteLine(xEle);
        }

        /// <summary>
        /// 7 - Find Nested Elements (using Descendants Axis) using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo7_Click(object sender, EventArgs e)
        {
            XElement xelement = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            Console.WriteLine("List of all Zip Codes");
            foreach (XElement xEle in xelement.Descendants("Zip"))
            {
                Console.WriteLine((string)xEle);
            }
        }

        /// <summary>
        /// 8 - apply Sorting on Elements using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo8_Click(object sender, EventArgs e)
        {
            XElement xelement = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            IEnumerable<string> codes = from code in xelement.Elements("Employee")
                                        let zip = (string)code.Element("Address").Element("Zip")
                                        orderby zip
                                        select zip;
            Console.WriteLine("List and Sort all Zip Codes");

            foreach (string zp in codes)
                Console.WriteLine(zp);
        }

        #endregion

        
        #region Section 2: Manipulate XML content and Persist the changes using LINQ To XML

        /// <summary>
        /// Create an XML Document with Xml Declaration/Namespace/Comments using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo9_Click(object sender, EventArgs e)
        {
            XNamespace empNM = "urn:lst-emp:emp";

            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "UTF-16", null),
                new XElement(empNM + "Employees",
                    new XElement("Employee",
                        new XComment("Only 3 elements for demo purposes"),
                        new XElement("EmpId", "5"),
                        new XElement("Name", "Kimmy"),
                        new XElement("Sex", "Female")
                    )));

            StringWriter sw = new StringWriter();
            xDoc.Save(sw);
            Console.WriteLine(sw);
        }

        #endregion
    }
}
