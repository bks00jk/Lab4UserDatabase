using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Services;

namespace KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Repositories
{
    class FileRepository
    {

        internal static readonly string BaseFolder = Path.Combine(Directory.GetCurrentDirectory(), "PersonStorage");
        public FileRepository()
        {
            if (!Directory.Exists(BaseFolder))
            {
                Directory.CreateDirectory(BaseFolder);
                var serv = new PersonService();
                serv.FillPersons();
            }

        }

        public async Task AddOrUpdateAsync(Person obj)
        {
            var stringObj = JsonSerializer.Serialize(obj);

            using (StreamWriter sw = new StreamWriter(Path.Combine(BaseFolder, obj.FirstName + obj.LastName), false))
            {
                await sw.WriteAsync(stringObj);
            }
        }

        public async Task<Person> GetAsync(string fullName)
        {
            string stringObj = null;
            string filePath = Path.Combine(BaseFolder, fullName);

            if (!File.Exists(filePath))
                return null;

            using (StreamReader sw = new StreamReader(filePath))
            {
                stringObj = await sw.ReadToEndAsync();
            }

            return JsonSerializer.Deserialize<Person>(stringObj);
        }

        public async Task<List<Person>> GetAllAsync()
        {
            var res = new List<Person>();
            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                string stringObj = null;

                using (StreamReader sw = new StreamReader(file))
                {
                    stringObj = await sw.ReadToEndAsync();
                }

                res.Add(JsonSerializer.Deserialize<Person>(stringObj));
            }

            return res;
        }

        public List<Person> GetAll()
        {
            var res = new List<Person>();
            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                string stringObj = null;

                using (StreamReader sw = new StreamReader(file))
                {
                    stringObj = sw.ReadToEnd();
                }

                res.Add(JsonSerializer.Deserialize<Person>(stringObj));
            }

            return res;
        }

        public async Task DeleteAsync(Person obj)
        {
            string filePath = Path.Combine(BaseFolder, obj.FirstName + obj.LastName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                await Task.CompletedTask;
            }
        }

        public async Task EditAsync(Person obj, string fullName)
        {
            string filePath = Path.Combine(BaseFolder, fullName);

            if (File.Exists(filePath))
            {
                if (!fullName.Equals(obj.FirstName + obj.LastName))
                {
                    File.Delete(filePath);
                }
                AddOrUpdateAsync(obj);
                    
            }
        }
    }

}
