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
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<UserModel> GetAll();
        UserModel GetById(string id);
        UserModel GetByUsername(string userName);
    }
}
