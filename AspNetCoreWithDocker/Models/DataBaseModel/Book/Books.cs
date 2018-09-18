using AspNetCoreWithDocker.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace AspNetCoreWithDocker.Models.DataBaseModel.Book
{
    [Table("books")]
    public class Books : BaseEntity
    {
        public string  Author { get; set; }
        public DateTime LaunchDate { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
    }
}
