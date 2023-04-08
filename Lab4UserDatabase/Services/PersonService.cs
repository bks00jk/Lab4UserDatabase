using System;
using System.Collections.Generic;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Repositories;

namespace KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Services
{
    class PersonService
    {
        private static FileRepository Repository = new FileRepository();


        public List<Person> GetAllPersons()
        {
            var res = new List<Person>();
            foreach (var pers in Repository.GetAll())
            {
                res.Add(new Person(pers.FirstName, pers.LastName, pers.Email, pers.BirthDate));                    
            }
            return res;
        }

        public void FillPersons()
        {
            Random ran = new Random();
            string[] firstNames = { "Lewis", "Max", "Charles", "Valtteri", "Lando", "Sergio", "Daniel", "Carlos", "Fernando", "Sebastian", "Esteban", "Pierre", "George", "Kimi", "Antonio", "Nico", "Jules", "Romain", "Kevin", "Daniil", "Nikita", "Mick", "Robert", "Nicholas", "Giancarlo", "Felipe", "Rubens", "Jenson", "Nelson", "Heikki", "Kazuki", "Timo", "Vitaly", "Jarno", "Mark", "David", "Juan", "Jacques", "Mika", "Michael", "Eddie", "Jean", "Ayrton", "Niki", "Alain", "Keke", "Mario", "Gilles", "Emerson", "Kevin", "Sam" };
            string[] lastNames = { "Hamilton", "Verstappen", "Leclerc", "Bottas", "Norris", "Perez", "Ricciardo", "Sainz", "Alonso", "Vettel", "Ocon", "Gasly", "Russell", "Raikkonen", "Giovinazzi", "Hulkenberg", "Bianchi", "Grosjean", "Magnussen", "Kvyat", "Mazepin", "Schumacher", "Kubica", "Latifi", "Fisichella", "Massa", "Barrichello", "Button", "Piquet", "Kovalainen", "Nakajima", "Glock", "Petrov", "Trulli", "Webber", "Coulthard", "Montoya", "Villeneuve", "Hakkinen", "Schumacher", "Irving", "Alesi", "Prost", "Rosberg", "Piquet", "Senna", "Lauda", "Prost", "Rosberg", "Andretti", "Villeneuve", "Fittipaldi", "Smith", "Baron" };
            string[] emails = { "hotmail.com", "yahoo.com", "outlook.com", "aol.com", "gmail.com", "i.ua", "yandex.ru",  "mail.ru"};
            for (int i = 0; i < firstNames.Length; i++)
            {
                DateTime birthday = new DateTime(ran.Next(1890, 2023), ran.Next(1, 12), ran.Next(1, 28));
                var authService = new AuthenticationService();
                authService.AddPerson(new Person(firstNames[i], lastNames[i], $"{firstNames[i]}.{lastNames[i]}@{emails[ran.Next(emails.Length)]}", birthday));
                
              
            }
        }
    }
}
