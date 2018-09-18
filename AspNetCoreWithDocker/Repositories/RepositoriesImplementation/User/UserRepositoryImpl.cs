using AspNetCoreWithDocker.Models.Contexts;
using AspNetCoreWithDocker.Models.DataBaseModel.User;
using AspNetCoreWithDocker.Repositories.Repository.User;
using System.Linq;

namespace AspNetCoreWithDocker.Repositories.RepositoriesImplementation.User
{
    public class UserRepositoryImpl : IUserRepository
    {
        private readonly MySqlContext _context;

        public UserRepositoryImpl(MySqlContext context)
        {
            _context = context;
        }

        public Users FindByLogin(string login)
        {
            return _context.Users.SingleOrDefault(user => user.Login.Equals(login.ToUpper()));
        }
    }
}
