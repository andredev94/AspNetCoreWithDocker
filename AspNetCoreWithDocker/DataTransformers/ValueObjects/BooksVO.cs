using System;
using System.Runtime.Serialization;

namespace AspNetCoreWithDocker.DataTransformers.ValueObjects
{
    [DataContract]
    public class BooksVO
    {
        public long? Codigo { get; set; }

        [DataMember(IsRequired = true)]
        public string Autor { get; set; }

        public DateTime DataLancamento { get; set; }

        [DataMember(IsRequired = true)]
        public decimal Preco { get; set; }

        [DataMember(IsRequired = true)]
        public string Titulo { get; set; }
    }
}
