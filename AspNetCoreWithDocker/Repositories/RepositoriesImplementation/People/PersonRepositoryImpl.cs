using AspNetCoreWithDocker.Models.Contexts;
using AspNetCoreWithDocker.Models.DataBaseModel.People;
using AspNetCoreWithDocker.Repositories.Generic.Implementation;
using AspNetCoreWithDocker.Repositories.Repository.People;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreWithDocker.Repositories.RepositoriesImplementation.People
{
    public class PersonRepositoryImpl : GenericRepositoryImpl<Person>, IPersonRepository 
    {
        public PersonRepositoryImpl(MySqlContext context) : base(context) { }
        
        public List<Person> FindByName(string firstName)
        {
            bool isPersonFinded = false;
            List<Person> personFinded = null;

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                personFinded = _context.People.Where(person => person.FirstName.Equals(firstName)).ToList();
                if (personFinded != null && personFinded.Count > 0)
                {
                    isPersonFinded = true;
                    return personFinded;
                }
            }

            if (!isPersonFinded)
                return _context.People.Where(person => person.FirstName.Contains(firstName)).ToList();
            
            return _context.People.ToList();
        }
    }
}
