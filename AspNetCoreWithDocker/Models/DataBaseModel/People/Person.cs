using AspNetCoreWithDocker.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreWithDocker.Models.DataBaseModel.People
{
    [Table("people")]
    public class Person : BaseEntity
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Genre { get; set; }
        public int Age { get; set; }
    }
}
