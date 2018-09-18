using AspNetCoreWithDocker.DataTransformers.ValueObjects;
using System.Collections.Generic;
using Tapioca.HATEOAS.Utils;

namespace AspNetCoreWithDocker.Business.Rules.People
{
    public interface IPersonBusiness
    {
        PersonVO Create(PersonVO person);
        PersonVO FindById(long id);
        List<PersonVO> FindByName(string firstName);
        List<PersonVO> FindAll();
        PersonVO Update(PersonVO person);
        void Delete(long id);
        PagedSearchDTO<PersonVO> PagedSearch(string name, string sortDirection, int pageSize, int page);
    }
}
