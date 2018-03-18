using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace webkom.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public  string Ime { get; set; }
        public  string Prezime { get; set; }
        public  string Uloga { get; set; }
        public  bool Aktivan { get; set; }
        public string PanteonUserName {get;set;}
    }
}
