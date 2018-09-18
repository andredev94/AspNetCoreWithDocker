using System.Collections.Generic;
using System.Linq;
using AspNetCoreWithDocker.DataTransformers.Transformers;
using AspNetCoreWithDocker.DataTransformers.ValueObjects;
using AspNetCoreWithDocker.Models.DataBaseModel.Book;

namespace AspNetCoreWithDocker.DataTransformers.TransformersImplementation
{
    public class BooksTransformerImpl : Transformer<BooksVO, Books>, Transformer<Books, BooksVO>
    {
        public Books Transform(BooksVO source)
        {
            if (source == null) return new Books();

            return new Books {
                Id = source.Codigo,
                Title = source.Titulo,
                Author = source.Autor,
                Price = source.Preco,
                LaunchDate = source.DataLancamento
            };
        }

        public BooksVO Transform(Books source)
        {
            if (source == null) return new BooksVO();

            return new BooksVO {
                Codigo = source.Id,
                Titulo = source.Title,
                Autor = source.Author,
                Preco = source.Price,
                DataLancamento = source.LaunchDate
            };
        }

        public List<Books> TransformList(List<BooksVO> source)
        {
            if (source == null) return new List<Books>();

            return source.Select(element => this.Transform(element)).ToList();
        }

        public List<BooksVO> TransformList(List<Books> source)
        {
            if (source == null) return new List<BooksVO>();

            return source.Select(element => this.Transform(element)).ToList();
        }
    }
}
