using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

using webkom.Models;
{
    public class Dokument : Entitet
    {
        public Dokument()
        {
            Stavke  = new HashSet<PorudzbenicaStavka>();
        }
        public virtual int Id { get; set; }
        public virtual string Vrsta { get; set; }
        public virtual int Rbr { get; set; }
        public virtual string BrojDokumenta { get; set; }
        public virtual string BrojFakture { get; set; }
        public virtual int? BrojStavki { get; set; }
        public virtual DateTime? Datum { get; set; }
        public virtual int Status { get; set; }
        public virtual string Status2 { get; set; }
        public virtual int Poruceno { get; set; }
        public virtual int Odobreno { get; set; }
        public virtual int Otpremljeno { get; set; }
        public virtual int Poslato { get; set; }
        public virtual int? FiskalniRacun { get; set; }
        public virtual bool Gratis { get; set; }
        public virtual string Napomena { get; set; }
        public virtual ICollection<PorudzbenicaStavka> Stavke { get; set; }

    }
}


