using JwtAuthentication.Entity;
using JwtAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthentication.Services
{
    public interface IUserService
    {
        User CreateUser(User user, string password);
        AuthenticateResponse Authenticate(AuthenticateRequest model,string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
        IEnumerable<UserModel> GetAll();
        UserModel GetById(string id);
        UserModel GetByUsername(string userName);
    }
}
