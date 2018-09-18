using AspNetCoreWithDocker.Models.Base;
using AspNetCoreWithDocker.Models.Contexts;
using AspNetCoreWithDocker.Repositories.Generic.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreWithDocker.Repositories.Generic.Implementation
{
    public class GenericRepositoryImpl<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly MySqlContext _context;
        private DbSet<T> _dataSet;

        public GenericRepositoryImpl(MySqlContext context)
        {
            _context = context;
            _dataSet = _context.Set<T>();
        }

        public T Create(T entity)
        {
            try
            {
                _dataSet.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }

        public void Delete(long id)
        {
            try
            {
                var result = FindById(id);
                if (result != null)
                {
                    _dataSet.Remove(result);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Exist(long id)
        {
            return _dataSet.Any(entity => entity.Id.Equals(id));
        }

        public List<T> FindAll()
        {
            return _dataSet.ToList();
        }

        public T FindById(long id)
        {
            return _dataSet.SingleOrDefault(entity => entity.Id.Equals(id));
        }

        public List<T> PagedSearch(string query)
        {
            return _dataSet.FromSql(query).ToList();
        }

        public T Update(T entity)
        {
            if (!Exist(entity.Id.Value)) return null;

            try
            {
                
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }
    }
}
