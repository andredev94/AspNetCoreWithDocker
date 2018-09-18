using System.Collections.Generic;
using AspNetCoreWithDocker.Business.Rules.Book;
using AspNetCoreWithDocker.DataTransformers.TransformersImplementation;
using AspNetCoreWithDocker.DataTransformers.ValueObjects;
using AspNetCoreWithDocker.Models.DataBaseModel.Book;
using AspNetCoreWithDocker.Repositories.Generic.Contract;

namespace AspNetCoreWithDocker.Business.BusinessImplementation.Book
{
    public class BooksBusinessImpl : IBooksBusiness
    {
        private IGenericRepository<Books> _repository;
        private readonly BooksTransformerImpl _transformer;


        public BooksBusinessImpl(IGenericRepository<Books> repository)
        {
            _repository = repository;
            _transformer = new BooksTransformerImpl();
        }

        public BooksVO Create(BooksVO book)
        {
            var entity = _transformer.Transform(book);

            return _transformer.Transform(_repository.Create(entity));
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }

        public List<BooksVO> FindAll()
        {
            return _transformer.TransformList(_repository.FindAll());
        }

        public BooksVO FindById(long id)
        {
            return _transformer.Transform(_repository.FindById(id));
        }

        public BooksVO Update(BooksVO book)
        {
            var entity = _transformer.Transform(book);

            return _transformer.Transform(_repository.Update(entity));
        }
    }
}
