using JwtAuthentication.Entity;
using JwtAuthentication.Exceptions;
using JwtAuthentication.Interfaces;
using JwtAuthentication.Models;
using Mapster;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password)) return null;

            var userExist = checkUserExist(model.Username);
            if (userExist == null) return null;

            if (!VerifyPasswordHash(model.Password, userExist.PasswordHash, userExist.PasswordSalt)) return null;

            var user = userExist.Adapt<UserModel>();
            var token = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress);

            userExist.RefreshTokens.Add(refreshToken);

            var def = Builders<User>.Update
            .Set(it => it.RefreshTokens, userExist.RefreshTokens);
            _UserCollection.UpdateOne(it => it.Id == userExist.Id, def);

            return new AuthenticateResponse(user, token, refreshToken.Token);
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var user = _UserCollection.Find(it => it.RefreshTokens.Any(i => i.Token == token)).FirstOrDefault();

            if (user == null) return null;

            var refreshToken = user.RefreshTokens.FirstOrDefault(it => it.Token == token);

            if (!refreshToken.IsActive) return null;

            user.RefreshTokens.Remove(refreshToken);

            var newRefreshToken = generateRefreshToken(ipAddress);

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            user.RefreshTokens.AddRange(new List<RefreshToken> { refreshToken, newRefreshToken });

            var def = Builders<User>.Update
            .Set(it => it.RefreshTokens, user.RefreshTokens);

            _UserCollection.UpdateOne(it => it.Id == user.Id, def);

            var userData = user.Adapt<UserModel>();

            var jwtToken = generateJwtToken(userData);

            return new AuthenticateResponse(userData, jwtToken, newRefreshToken.Token);
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var user = _UserCollection.Find(it => it.RefreshTokens.Any(i => i.Token == token)).FirstOrDefault();

            if (user == null) return false;

            var refreshToken = user.RefreshTokens.FirstOrDefault(it => it.Token == token);

            //if (!refreshToken.IsActive) return false;

            user.RefreshTokens.Remove(refreshToken);

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            user.RefreshTokens.Add(refreshToken);

            var def = Builders<User>.Update
            .Set(it => it.RefreshTokens, user.RefreshTokens);

            _UserCollection.UpdateOne(it => it.Id == user.Id, def);

            return true;
        }

        public User CreateUser(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password", "Password is required!");

            var userExist = checkUserExist(user.Username);
            if (userExist != null) throw new AppException($"Username {user.Username} is already existed!");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.RefreshTokens = new List<RefreshToken>();
            user.PasswordHash = Convert.ToBase64String(passwordHash);
            user.PasswordSalt = Convert.ToBase64String(passwordSalt);
            user.Id = Guid.NewGuid().ToString();
            _UserCollection.InsertOne(user);

            return user;
        }

        public IEnumerable<UserModel> GetAll()
        {
            var userEntity = _UserCollection.Find(it => true).ToEnumerable<User>();
            var userData = userEntity.Adapt<IEnumerable<UserModel>>();
            return userData;
        }

        public UserModel GetById(string id)
        {
            var userEntity = _UserCollection.Find(it => it.Id == id).FirstOrDefault();
            var userData = userEntity.Adapt<UserModel>();
            return userData;
        }

        public UserModel GetByUsername(string userName)
        {
            var userEntity = _UserCollection.Find(it => it.Username == userName).FirstOrDefault();
            var userData = userEntity.Adapt<UserModel>();
            return userData;
        }

        private User checkUserExist(string username)
        {
            var userData = _UserCollection.Find(it => it.Username == username).FirstOrDefault();
            return userData;
        }

        private string generateJwtToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_settings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username), // Registered Claim
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Exp,DateTime.UtcNow.AddMinutes(1).ToString()),
                    new Claim("FisrtName",user.FirstName), // Public Claim
                    new Claim("LastName",user.LastName),
                    //new Claim("Id",user.Id)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddMinutes(2),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password", "Password is required!");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("password cannot be empty or whitespace only string!", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password", "Password is required!");

            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password can not be null or empty only string!", "password");

            var PasswordHash = Convert.FromBase64String(passwordHash);
            var PasswordSalt = Convert.FromBase64String(passwordSalt);

            if (PasswordHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected)", "passwordHash");
            if (PasswordSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected)", "passwordSalt");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != PasswordHash[i]) return false;
                }
            }
            return true;
        }
    }
}
