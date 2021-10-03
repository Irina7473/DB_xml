using System;
using System.Xml;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace DB_xml
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("  *** База данных пользователей ***");
            Console.WriteLine();

            var DB = new DBusers(@"DB_USERS.sqlite");                     
            List<User> usersList = new List<User>();

            var xml = new XmlDocument();
            xml.Load(@"XMLusers.xml");
            XmlElement root = xml.DocumentElement;

            //Вариант с XmlNodeList и XPath
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Вариант с XmlNodeList и XPath");
            Console.ResetColor();
                        
            XmlNodeList users = root?.SelectNodes("user");
           
            foreach (XmlNode user in users)
            {
                var name = user.SelectSingleNode("@name")?.Value;
                var company= user.SelectSingleNode("company")?.InnerText;
                var age = Convert.ToInt32(user.SelectSingleNode("age")?.InnerText);               
                DB.AddUsers(name, company, age);
                usersList.Add(new User(name, company, age));
            }

            Console.WriteLine("Вывод из списка");
            foreach (User u in usersList)
                Console.WriteLine($"{u.Name} ({u.Company}) - {u.Age}");
            Console.WriteLine("Вывод из базы данных");            
            DB.ShowAll();
            Console.WriteLine();
            SerializeJSON(usersList);

            // Вариант без XmlNodeList и XPath
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Вариант без XmlNodeList и XPath");
            Console.ResetColor();

            foreach (XmlElement xnode in root)
            {
                User user = new User();
                XmlNode attr = xnode.Attributes.GetNamedItem("name");
                if (attr != null)
                    user.Name = attr.Value;

                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "company")
                        user.Company = childnode.InnerText;

                    if (childnode.Name == "age")
                        user.Age = Int32.Parse(childnode.InnerText);
                }
                usersList.Add(user);
            }

            Console.WriteLine("Вывод из списка");
            foreach (User u in usersList)
            {
                Console.WriteLine($"{u.Name} ({u.Company}) - {u.Age}");
                DB.AddUsers(u.Name, u.Company, u.Age);
            }
            Console.WriteLine("Вывод из базы данных");
            DB.ShowAll();
            SerializeJSON(usersList);

            void SerializeJSON(List<User> usersList)
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                File.WriteAllText(@"G:\STEP\TEMP\DB_xml\DB_xml\Jsonusers.json", "[\n");
                for (int i = 0; i < usersList.Count; i++)
                {
                    string jsonString = JsonSerializer.Serialize<User>(usersList[i], options);
                    if (i == usersList.Count - 1) jsonString = jsonString + "\n";
                    else jsonString = jsonString + "," + "\n";
                    File.AppendAllText(@"G:\STEP\TEMP\DB_xml\DB_xml\Jsonusers.json", jsonString);
                }
                File.AppendAllText(@"G:\STEP\TEMP\DB_xml\DB_xml\Jsonusers.json", "]");

                Console.WriteLine("Данные записаны в json");
                Console.WriteLine();
            }
        }        
    }
}