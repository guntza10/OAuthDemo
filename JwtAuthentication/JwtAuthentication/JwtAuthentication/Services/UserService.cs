using JwtAuthentication.Interfaces;
using JwtAuthentication.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthentication.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _UserCollection;
        private readonly IAppSettings _settings;

        public UserService(IAppSettings settings)
        {
            _settings = settings;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DbName);
            _UserCollection = database.GetCollection<User>(_settings.User);
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _UserCollection.Find(it => it.Username == model.Username && it.Password == model.Password).FirstOrDefault();

            if (user == null) return null;

            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public void CreateUser(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            _UserCollection.InsertOne(user);
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username), // Registered Claim
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Exp,DateTime.UtcNow.AddMinutes(10).ToString()),
                    new Claim("FisrtName",user.FirstName), // Public Claim
                    new Claim("LastName",user.LastName),
                    //new Claim("Id",user.Id)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public IEnumerable<User> GetAll()
        {
            return _UserCollection.Find(it => true).ToEnumerable<User>();
        }

        public User GetById(string id)
        {
            return _UserCollection.Find(it => it.Id == id).FirstOrDefault();
        }

        public User GetByUsername(string userName)
        {
            return _UserCollection.Find(it => it.Username == userName).FirstOrDefault();
        }
    }
}
