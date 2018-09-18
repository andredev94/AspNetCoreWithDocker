using AspNetCoreWithDocker.Models.DataBaseModel.User;

namespace AspNetCoreWithDocker.Repositories.Repository.User
{
    public interface IUserRepository
    {
        Users FindByLogin(string login);
    }
}
