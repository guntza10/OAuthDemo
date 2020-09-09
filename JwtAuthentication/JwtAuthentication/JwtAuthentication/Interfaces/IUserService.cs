using JwtAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthentication.Services
{
    public interface IUserService
    {
        void CreateUser(User user);
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(string id);
        User GetByUsername(string userName);
    }
}
