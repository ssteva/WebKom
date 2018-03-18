using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace webkom.Models
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
        public virtual Subjekt Narucila {get;set;}
        public virtual ICollection<PorudzbenicaStavka> Stavke { get; set; }

    }
}


