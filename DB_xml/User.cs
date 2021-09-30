using System;

namespace DB_xml
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public int Age { get; set; }

        public User() { }
        public User(string name, string company, int age)
        {            
            Name = name;
            Company = company;
            Age = age;
        }
        public User(int id, string name, string company, int age)
        {
            Id = id;
            Name = name;
            Company = company;
            Age = age;
        }
    }
}