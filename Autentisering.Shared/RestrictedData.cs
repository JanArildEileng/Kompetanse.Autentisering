﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autentisering.Shared
{
    public class RestrictedData
    {
        public String Name { get; set; }
        public int Value { get; set; }
        public string? Jti { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
