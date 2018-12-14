using AspNetCoreWithDocker.Models.DataBaseModel.Book;
using AspNetCoreWithDocker.Models.DataBaseModel.People;
using AspNetCoreWithDocker.Models.DataBaseModel.User;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWithDocker.Models.Contexts
{
    public class MySqlContext : DbContext
    {
        public MySqlContext() {}

        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
