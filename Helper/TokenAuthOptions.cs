
using System;
using System.Collections.Generic;
//using System.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.IdentityModel.Tokens;

namespace webkom.Helper
{
    public class TokenAuthOptions 
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public Microsoft.IdentityModel.Tokens.SymmetricSecurityKey Key { get; set; }
        public Microsoft.IdentityModel.Tokens.SigningCredentials SigningCredentials { get; set; }
        public int MinutaToken {get;set;}
        public int MinuntaRefreshToken {get;set;}

        //public static implicit operator TokenAuthOptions(TokenAuthOptions v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
