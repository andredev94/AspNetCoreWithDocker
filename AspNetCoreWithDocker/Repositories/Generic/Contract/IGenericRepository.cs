using AspNetCoreWithDocker.Models.Base;
using System.Collections.Generic;

namespace AspNetCoreWithDocker.Repositories.Generic.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        T Create(T entity);
        T FindById(long id);
        List<T> FindAll();
        T Update(T entity);
        void Delete(long id);
        bool Exist(long id);
        List<T> PagedSearch(string query);
    }
}
