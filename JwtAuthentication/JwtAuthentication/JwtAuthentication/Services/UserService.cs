using JwtAuthentication.Interfaces;
using JwtAuthentication.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace JwtAuthentication.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _UserCollection;

        public UserService(IAppSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DbName);
            _UserCollection = database.GetCollection<User>(settings.User);
        }
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            _UserCollection.InsertOne(user);
        }

        public IEnumerable<User> GetAll()
        {
            return _UserCollection.Find(it => true).ToEnumerable<User>();
        }

        public User GetById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
