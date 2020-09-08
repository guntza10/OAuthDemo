using JwtAuthentication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthentication.Controllers
{
    public class AppSettings : IAppSettings
    {
        public string SecretKey { get; set; }
        public string ConnectionString { get; set; }
        public string DbName { get; set; }
        public string User { get; set; }
    }
}
