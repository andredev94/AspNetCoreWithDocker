using AspNetCoreWithDocker.Models.DataBaseModel.User;

namespace AspNetCoreWithDocker.Business.Rules.User
{
    public interface IUserBusiness
    {
        object FindByLogin(Users user);
    }
}
