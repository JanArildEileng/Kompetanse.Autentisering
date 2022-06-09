using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autentisering.Shared.Dto.IdentityAndAccess
{
    public class GetTokenResponse
    {
        public string IdToken { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public DateTime Expire { get; set; }

    }
}
