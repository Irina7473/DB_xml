using System;
using System.Xml;
using System.Collections.Generic;

namespace DB_xml
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("  *** База данных пользователей ***");
            Console.WriteLine();
            var DB = new DBusers(@"DB_USERS.sqlite");

            DB.AddUsers("qw", "asd", 24);

            List<User> usersList = new List<User>();

            var xml = new XmlDocument();
            xml.Load("XMLusers.xml");

            var root = xml.DocumentElement;
            var users = root?.SelectNodes("user");
            foreach(XmlNode user in users)
            {
                var name = user.SelectSingleNode("@name")?.Value;
                Console.WriteLine(name);
                var company= user.SelectSingleNode("company")?.InnerText;
                var age = Convert.ToInt32(user.SelectSingleNode("age")?.InnerText);
                Console.WriteLine(name, company, age);
                DB.AddUsers(name, company, age);
                usersList.Add(new User(name, company, age));
            }

            DB.ShowAll();

            foreach (var us in usersList) Console.WriteLine(us.Name, us.Company, us.Age);

            for (int i = 0; i < usersList.Count; i++)
            {
                DB.AddUsers(usersList[i].Name, usersList[i].Company, usersList[i].Age);
            }
            DB.ShowAll();

            /*foreach (XmlAttribute attribute in root.Attributes) Console.WriteLine($"{attribute.Name} {attribute.Value}");
                       
            ShowChildNodes(root);

           // var usersName = root.GetElementsByTagName("user");
            //Console.WriteLine (usersName.Item(0).InnerText);

            static void ShowChildNodes(XmlNode node)
            {
                if (!node.HasChildNodes)
                {
                    Console.WriteLine($"{node.InnerText}");
                    return; 
                }

                foreach (XmlNode? element in node)
                {
                    Console.WriteLine($"{element.Name} : ");
                    ShowChildNodes(element);
                }
            }*/
        }
    }
}

/*
Дан XML-файл. Необходимо из этого файла данные поместить в БД.
БД разработать на основе xml-файла
*Написать конвертер из XML в JSON

<?xml version="1.0" encoding="utf-8" ?>
<users>
  <user name="Bill Gates">
    <company>Microsoft</company>
    <age>48</age>
  </user>
  <user name="Larry Page">
    <company>Google</company>
    <age>42</age>
  </user>
</users>
*/