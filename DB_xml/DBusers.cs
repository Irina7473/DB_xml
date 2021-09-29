using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace DB_xml
{ 
    public class DBusers
    {
        private string _connectionString;
        private SqliteConnection _connection;
        private SqliteCommand _query;               

        public DBusers(string patch)
        {
            _connectionString = $"Data Source={patch};Mode=ReadWrite;";
            try {
                _connection = new SqliteConnection(_connectionString);
                _query = new SqliteCommand { Connection = _connection };
            }
            catch (Exception)
            {
                throw new Exception("Путь к базе данных не найден");
            }
        }

        public void Open()
        {
            try
            {
                _connection.Open();
                Console.WriteLine("Успешное подключение к базе данных");
            }
            catch (InvalidOperationException)
            {
                throw new Exception("Ошибка открытия базы данных");
            }
            catch (SqliteException)
            {
                throw new Exception("Подключаемся к уже открытой базе данных");
            }
            catch (Exception)
            {
                throw new Exception("Путь к базе данных не найден");
            }
        }

        public void Close()
        {
            _connection.Close();
        }

        public void AddUsers(string name, string company, int age)
        {
            Open();
            _query.CommandText = $"INSERT INTO table_users (name, company, age) VALUES('{name}','{company}','{age}');";           
            _query.ExecuteNonQuery();
            Close();
        }

        public void ShowAll()
        {
            Open();
            _query.CommandText = "SELECT * FROM table_users;";
            var result = _query.ExecuteReader();

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return;
            }

            var users = new List<User>();
            do 
            {
                while (result.Read())
                {
                    var user = new User
                    {
                        Id = result.GetInt32(0),
                        Name = result.GetString(1),
                        Company = result.GetString(2),
                        Age = result.GetInt32(3),
                    };

                    users.Add(user);
                } 
            } while (result.NextResult());

            Console.WriteLine(" ----------------------------------------------");
            Console.WriteLine(" № |       Имя       | Место работы | Возраст ");
            Console.WriteLine(" ----------------------------------------------");
            foreach (var user in users)
                Console.WriteLine($" {user.Id} |   {user.Name}   |   {user.Company}   | {user.Age} ");
            Console.WriteLine(" --------------------------------------------");

            if (result != null) result.Close();
            Close();
        }              

    }
}