using AspNetCoreWithDocker.Business.Rules.People;
using AspNetCoreWithDocker.DataTransformers.TransformersImplementation;
using AspNetCoreWithDocker.DataTransformers.ValueObjects;
using AspNetCoreWithDocker.Repositories.Repository.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tapioca.HATEOAS.Utils;

namespace AspNetCoreWithDocker.Business.BusinessImplementation.People
{
    public class PersonBusinessImpl : IPersonBusiness
    {
        private IPersonRepository _repository;
        private readonly PersonTransformerImpl _transformer;

        public PersonBusinessImpl(IPersonRepository repository)
        {
            _repository = repository;
            _transformer = new PersonTransformerImpl();
        }

        public PersonVO Create(PersonVO person)
        {
            var entity = _transformer.Transform(person);
            return _transformer.Transform(_repository.Create(entity));
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }

        public List<PersonVO> FindAll()
        {
            return _transformer.TransformList(_repository.FindAll());
        }


        public PersonVO FindById(long id)
        {
            return _transformer.Transform(_repository.FindById(id));
        }

        public List<PersonVO> FindByName(string firstName)
        {
            return _transformer.TransformList(_repository.FindByName(firstName));
        }

        public PersonVO Update(PersonVO Person)
        {
            var entity = _transformer.Transform(Person);
            return _transformer.Transform(_repository.Update(entity));
        }

        public PagedSearchDTO<PersonVO> PagedSearch(string firstName, string sortDirection, int pageSize, int page)
        {
            int index = 1;
            StringBuilder query = new StringBuilder();
            query.Append(@"SELECT DISTINCT
                                *, COUNT(*) OVER() AS 'TotalCounted' 
                            FROM people AS P WHERE 1 = 1");
            if (!string.IsNullOrWhiteSpace(firstName)) query.Append($" AND  P.FirstName LIKE '%{firstName}%' GROUP BY P.FirstName");
            page = page > 0 ? page - index : 0;
            query.Append($" ORDER BY P.FirstName {sortDirection} LIMIT {pageSize} OFFSET {page}");

            var people = _repository.PagedSearch(query.ToString());

            return new PagedSearchDTO<PersonVO>
            {
                CurrentPage = page + index,
                PageSize = pageSize,
                SortDirections = sortDirection,
                TotalResults = Convert.ToInt32(people.FirstOrDefault(p => p.Id > 0).TotalCounted.Value),
                List = _transformer.TransformList(people)
            };
        }
    }
}
