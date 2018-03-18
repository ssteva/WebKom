using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace webkom.Models
{
    public class PorudzbenicaStavka : Entitet
    {
        public virtual int Id { get; set; }
        public virtual Dokument Dokument { get; set; }
        public virtual int? Rbr { get; set; }
        public virtual int? Poruceno { get; set; }
        public virtual int? Odobreno { get; set; }
        public virtual int? Otpremljeno { get; set; }
        public virtual int? Primljeno { get; set; }
        public virtual int? Poslato { get; set; }
        public virtual int? Kolicina { get; set; }
        public virtual decimal? Cena { get; set; }
    }
}



