using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using AspNetCoreWithDocker.Business.Rules.User;
using AspNetCoreWithDocker.Models.DataBaseModel.User;
using AspNetCoreWithDocker.Repositories.Repository.User;
using AspNetCoreWithDocker.Security.Configuration;

namespace AspNetCoreWithDocker.Business.BusinessImplementation.User
{
    public class UserBusinessImpl : IUserBusiness
    {
        private readonly IUserRepository _repository;
        private SigningConfiguration _signingConfiguration;
        private TokenConfiguration _tokenConfiguration;

        public UserBusinessImpl(IUserRepository repository, SigningConfiguration signingConfiguration, TokenConfiguration tokenConfiguration)
        {
            _repository = repository;
            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;
        }

        public object FindByLogin(Users user)
        {
            var credentialsIsValid = false;

            if (user != null && !string.IsNullOrWhiteSpace(user.Login))
            {
                var baseUser = _repository.FindByLogin(user.Login);
                credentialsIsValid = (baseUser != null && baseUser.Login == user.Login.ToUpper() && baseUser.AccesKey == user.AccesKey);
            }
            if (credentialsIsValid)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(user.Login, "Login"), new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.Login.ToString())
                        }
                    );
                DateTime createDate = DateTime.Now;
                DateTime expirationDate = (createDate + TimeSpan.FromSeconds(_tokenConfiguration.Seconds));

                var handler = new JwtSecurityTokenHandler();
                string token = CreateToken(identity, createDate, expirationDate, handler);

                return SucessObject(createDate,expirationDate,token);
            }
            else
            {
                return ExceptionObject();
            }
        }

        private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            var securitToken = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                SigningCredentials = _signingConfiguration.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });

            var token = handler.WriteToken(securitToken);

            if (!string.IsNullOrWhiteSpace(token)) return token;
            return string.Empty;
        }

        private object ExceptionObject()
        {
            return new
            {
                autenticated = false,
                message = "Failed to autheticate"
            };
        }

        private object SucessObject(DateTime createDate, DateTime expirationDate, string token)
        {
            return new
            {
                autenticated = true,
                created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token,
                message = "OK, right now you are logged in our api, enjoy!"
            };
        }
    }
}
