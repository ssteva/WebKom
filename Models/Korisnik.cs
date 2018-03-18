using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace webkom.Models
{
    public class Korisnik : Entitet
    {
        public virtual int Id { get; set; }
        public virtual int PanteonId { get; set; }
        public virtual string KorisnickoIme { get; set; }
        //public string KorisnickoIme { get; set; }
        public virtual string Ime { get; set; }
        public virtual string Prezime { get; set; }
        public virtual string Naziv { get; set; }
        [JsonIgnore]
        public virtual string Lozinka { get; set; }
        [JsonIgnore]
        public virtual Guid So { get; set; }
        public virtual string Email { get; set; }
        public virtual string Uloga { get; set; }
        public virtual bool Aktivan { get; set; }

        public virtual string[] Uloge
        {
            get
            {
                try
                {
                    return Uloga.Split(',');
                }
                catch
                {
                    return new string[0];
                }
            }
        }
    }
}


