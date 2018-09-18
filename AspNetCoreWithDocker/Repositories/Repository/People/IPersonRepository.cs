using AspNetCoreWithDocker.Models.DataBaseModel.People;
using AspNetCoreWithDocker.Repositories.Generic.Contract;
using System.Collections.Generic;

namespace AspNetCoreWithDocker.Repositories.Repository.People
{
    public interface IPersonRepository: IGenericRepository<Person>
    {
        List<Person> FindByName(string firstName);
    }
}
