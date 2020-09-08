using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthentication.Interfaces
{
    public interface IAppSettings
    {
        string SecretKey { get; set; }
        string ConnectionString { get; set; }
        string DbName { get; set; }
        string User { get; set; }
    }
}
