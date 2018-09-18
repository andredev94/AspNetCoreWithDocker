using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreWithDocker.Models.DataBaseModel.User
{
    [Table("users")]
    public class Users
    {
        public long? Id { get; set; }
        public string Login { get; set; }
        public string AccesKey { get; set; }
    }
}
