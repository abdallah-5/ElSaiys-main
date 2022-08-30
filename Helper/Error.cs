using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.Helper
{
    public class Error : IError
    {
        private readonly IConfiguration _config;

        public string ErrorCode { get; set; }
        public string ErrorProp { get; set; }
        public string ErrorMessage { get; set; }

        public Error(IConfiguration config)
        {
            _config = config;
        }

        public void LoadError(string errorCode)
        {
            ErrorCode = errorCode;

            var eSection = _config.GetSection("Errors");
            eSection.Bind(errorCode, this);
        }
    }
}
