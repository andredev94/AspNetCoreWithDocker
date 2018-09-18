using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Tapioca.HATEOAS;

namespace AspNetCoreWithDocker.DataTransformers.ValueObjects
{
    [DataContract]
    public class PersonVO : ISupportsHyperMedia
    {
        public PersonVO()
        {
            Links = new List<HyperMediaLink>();
        }

        public long? Codigo { get; set; }

        [DataMember(IsRequired = true)]
        public string PrimeiroNome { get; set; }

        [DataMember(IsRequired = true)]
        public string Sobrenome { get; set; }

        public string SobrenomeSecundario { get; set; }

        [DataMember(IsRequired = true)]
        public string Genero { get; set; }

        [DataMember(IsRequired = true)]
        public int Idade { get; set; }

        [JsonProperty]
        public List<HyperMediaLink> Links { get; set; }
    }
}
