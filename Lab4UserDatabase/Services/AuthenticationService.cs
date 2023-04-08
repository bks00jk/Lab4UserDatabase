using System;
using System.Threading.Tasks;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Repositories;

namespace KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Services
{
    class AuthenticationService
    {
        private static FileRepository Repository = new FileRepository();

        public async Task<bool> AddPerson(Person person)
        {
            var addedPerson = await Repository.GetAsync(person.FirstName + person.LastName);
            if (addedPerson != null)
                throw new Exception("Person already exists");
            addedPerson = new Person(person.FirstName, person.LastName, person.Email, person.BirthDate);
            await Repository.AddOrUpdateAsync(addedPerson);
            return true;
        }

        public async Task<bool> DeletePerson(Person person)
        {
            var personToDelete = await Repository.GetAsync(person.FirstName + person.LastName);
            if (personToDelete == null)
                throw new Exception("Person does not exist");
            await Repository.DeleteAsync(personToDelete);
            return true;
        }

        public async Task<bool> EditPerson(Person newPerson, Person oldPerson)
        {
            var personToEdit = await Repository.GetAsync(oldPerson.FirstName + oldPerson.LastName);
            if (personToEdit == null)
                throw new Exception("Person does not exist");
            await Repository.EditAsync(newPerson, oldPerson.FirstName + oldPerson.LastName);
            return true;
        }
    }
}
