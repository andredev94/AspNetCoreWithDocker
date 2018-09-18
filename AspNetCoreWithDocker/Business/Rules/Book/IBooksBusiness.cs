using AspNetCoreWithDocker.DataTransformers.ValueObjects;
using System.Collections.Generic;

namespace AspNetCoreWithDocker.Business.Rules.Book
{
    public interface IBooksBusiness
    {
        BooksVO Create(BooksVO book);
        BooksVO FindById(long id);
        List<BooksVO> FindAll();
        BooksVO Update(BooksVO book);
        void Delete(long id);
    }
}
