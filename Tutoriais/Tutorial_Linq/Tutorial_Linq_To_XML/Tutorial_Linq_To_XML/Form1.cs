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
using System.Xml;
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
        /// 9 - Create an XML Document with Xml Declaration/Namespace/Comments using LINQ to XML
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

        /// <summary>
        /// 10 - Save the XML Document to a XMLWriter or to the disk using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo10_Click(object sender, EventArgs e)
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
            
            // Save to XMLWriter
            StringWriter sw = new StringWriter();
            XmlWriter xWrite = XmlWriter.Create(sw);
            xDoc.Save(xWrite);
            xWrite.Close();

            // Save to Disk
            xDoc.Save("..\\..\\..\\..\\Created.xml");
            Console.WriteLine("Saved");
        }


        /// <summary>
        /// 11 - Load an XML Document using XML Reader using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo11_Click(object sender, EventArgs e)
        {
            XmlReader xRead = XmlReader.Create(@"..\\..\\..\\..\\exemplo.xml");
            XElement xEle = XElement.Load(xRead);
            Console.WriteLine(xEle);
            xRead.Close();
        }

        /// <summary>
        /// 12 - Find Element at a Specific Position using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            // Using XElement
            Console.WriteLine("Using XElement");
            XElement xEle = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            var emp1 = xEle.Descendants("Employee").ElementAt(0);
            Console.WriteLine(emp1);

            Console.WriteLine("------------");

            //// Using XDocument
            Console.WriteLine("Using XDocument");
            XDocument xDoc = XDocument.Load("..\\..\\..\\..\\exemplo.xml");
            var emp2 = xDoc.Descendants("Employee").ElementAt(1);
            Console.WriteLine(emp2);
        }


        /// <summary>
        /// 13 - List the First 2 Elements using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo13_Click(object sender, EventArgs e)
        {
            XElement xEle = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            var emps = xEle.Descendants("Employee").Take(2);
            foreach (var emp in emps)
                Console.WriteLine(emp);
        }

        /// <summary>
        /// 14 - List the 2nd and 3rd Element using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo14_Click(object sender, EventArgs e)
        {
            XElement xEle = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            var emps = xEle.Descendants("Employee").Skip(1).Take(2);
            foreach (var emp in emps)
                Console.WriteLine(emp);
        }

        /// <summary>
        /// 15 - List the Last 2 Elements using LINQ To XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo15_Click(object sender, EventArgs e)
        {
            XElement xEle = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            var emps = xEle.Descendants("Employee").Reverse().Take(2);
            foreach (var emp in emps)
                Console.WriteLine(emp.Element("EmpId") + "" + emp.Element("Name"));

            //To display only the values without the XML tags, use the ‘Value’ property

            XElement xEle1 = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            var emps1 = xEle.Descendants("Employee").Reverse().Take(2);
            foreach (var emp in emps1)
                Console.WriteLine(emp.Element("EmpId").Value + ". " + emp.Element("Name").Value);

            //If you notice, the results are not ordered i.e. the Employee 4 is printed before 3. 
            //To order the results, just add call Reverse() again while filtering as shown below:

            XElement xEle2 = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            var emps2 = xEle.Descendants("Employee").Reverse().Take(2).Reverse();
            foreach (var emp in emps2)
                Console.WriteLine(emp.Element("EmpId").Value + ". " + emp.Element("Name").Value);
        }


        /// <summary>
        /// 16 - Find the Element Count based on a condition using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo16_Click(object sender, EventArgs e)
        {
            XElement xelement = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            var stCnt = from address in xelement.Elements("Employee")
                        where (string)address.Element("Address").Element("State") == "CA"
                        select address;
            Console.WriteLine("No of Employees living in CA State are {0}", stCnt.Count());
        }


        /// <summary>
        /// 17 - Add a new Element at runtime using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo17_Click(object sender, EventArgs e)
        {
            XElement xEle = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            xEle.Add(new XElement("Employee",
                new XElement("EmpId", 5),
                new XElement("Name", "George")));

            Console.Write(xEle);
        }
        

        /// <summary>
        /// 18 - Add a new Element as the First Child using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo18_Click(object sender, EventArgs e)
        {
            XElement xEle = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            xEle.AddFirst(new XElement("Employee",
                new XElement("EmpId", 5),
                new XElement("Name", "George")));

            Console.Write(xEle);
        }


        /// <summary>
        /// 19 - Add an attribute to an Element using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo19_Click(object sender, EventArgs e)
        {
            XElement xEle = XElement.Load("..\\..\\..\\..\\exemplo.xml");
            xEle.Add(new XElement("Employee",
                new XElement("EmpId", 5),
                new XElement("Phone", "423-555-4224", new XAttribute("Type", "Home"))));

            Console.Write(xEle);
        }        

        /// <summary>
        /// 20 - Replace Contents of an Element/Elements using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button20_Click(object sender, EventArgs e)
        {
            XElement xEle = XElement.Load("..\\..\\..\\..\\exemplo.xml.xml");
            var countries = xEle.Elements("Employee").Elements("Address").Elements("Country").ToList();
            foreach (XElement cEle in countries)
                cEle.ReplaceNodes("United States Of America");

            Console.Write(xEle);
        }

        /// <summary>
        /// 21 - Remove an attribute from all the Elements using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo21_Click(object sender, EventArgs e)
        {
            XElement xEle = XElement.Load("..\\..\\Employees.xml");
            var phone = xEle.Elements("Employee").Elements("Phone").ToList();
            foreach (XElement pEle in phone)
                pEle.RemoveAttributes();

            Console.Write(xEle);

            /*
             * To remove attribute of one Element based on a condition, traverse to that Element and SetAttributeValue("Type", null); 
             * You can also use SetAttributeValue(XName,object) to update an attribute value.
             */
        }

        /// <summary>
        /// 22 - Delete an Element based on a condition using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo22_Click(object sender, EventArgs e)
        {
            XElement xEle = XElement.Load("..\\..\\Employees.xml");
            var addr = xEle.Elements("Employee").ToList();
            foreach (XElement addEle in addr)
                addEle.SetElementValue("Address", null);

            Console.Write(xEle);
        }

        /// <summary>
        /// 23 - Remove ‘n’ number of Elements using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo23_Click(object sender, EventArgs e)
        {
            XElement xEle = XElement.Load("..\\..\\Employees.xml");
            var emps = xEle.Descendants("Employee");
            emps.Reverse().Take(2).Remove();

            Console.Write(xEle);
        }

        /// <summary>
        /// 24 - Save/Persists Changes to the XML using LINQ to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exemplo24_Click(object sender, EventArgs e)
        {
            XElement xEle = XElement.Load("..\\..\\Employees.xml");
            xEle.Add(new XElement("Employee",
            new XElement("EmpId", 5),
            new XElement("Name", "George"),
            new XElement("Sex", "Male"),
            new XElement("Phone", "423-555-4224", new XAttribute("Type", "Home")),
            new XElement("Phone", "424-555-0545", new XAttribute("Type", "Work")),
            new XElement("Address",
                new XElement("Street", "Fred Park, East Bay"),
                new XElement("City", "Acampo"),
                new XElement("State", "CA"),
                new XElement("Zip", "95220"),
                new XElement("Country", "USA"))));

            xEle.Save("..\\..\\Employees.xml");
            Console.WriteLine(xEle);

            Console.ReadLine();
        }

        #endregion
    }
}
