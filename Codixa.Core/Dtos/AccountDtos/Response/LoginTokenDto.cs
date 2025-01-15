using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.AccountDtos.Response
{
    public class LoginTokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
