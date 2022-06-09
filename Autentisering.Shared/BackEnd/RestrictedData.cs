using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autentisering.Shared.BackEnd
{
    public class RestrictedData
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public string? Jti { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
