using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.adminDashDtos.AdminGetUsersDtos
{
    public class PasswordChangeDto
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
