using AspNetCoreWithDocker.DataTransformers.Transformers;
using AspNetCoreWithDocker.DataTransformers.ValueObjects;
using AspNetCoreWithDocker.Models.DataBaseModel.People;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreWithDocker.DataTransformers.TransformersImplementation
{
    public class PersonTransformerImpl : Transformer<PersonVO, Person>, Transformer<Person, PersonVO>
    {
        public Person Transform(PersonVO source)
        {
            if (source == null) return new Person();

            return new Person {
                Id = source.Codigo,
                FirstName = source.PrimeiroNome,
                MiddleName = source.Sobrenome,
                LastName = source.SobrenomeSecundario,
                Age = source.Idade,
                Genre = source.Genero
            };
        }

        public PersonVO Transform(Person source)
        {
            if (source == null) return new PersonVO();

            return new PersonVO {
                Codigo = source.Id,
                PrimeiroNome = source.FirstName,
                Sobrenome = source.MiddleName,
                SobrenomeSecundario = source.LastName,
                Idade = source.Age,
                Genero = source.Genre
            };
        }

        public List<Person> TransformList(List<PersonVO> source)
        {
            if (source == null) return new List<Person>();

            return source.Select(element => this.Transform(element)).ToList();
        }

        public List<PersonVO> TransformList(List<Person> source)
        {
            if (source == null) return new List<PersonVO>();

            return source.Select(element => this.Transform(element)).ToList();
        }
    }
}
