using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webkom.Models
{
    public class Parametar 
    {
        public virtual int Id { get; set; }
        public virtual string Vrsta { get; set; }
        public virtual string Klasa { get; set; }
        public virtual string Naziv { get; set; }
        public virtual int Vredpar1 { get; set; }
        public virtual int Vredpar2 { get; set; }
        public virtual decimal Vredpar3 { get; set; }
        public virtual decimal Vredpar4 { get; set; }
        public virtual string Vredpar5 { get; set; }
        public virtual string Vredpar6 { get; set; }
        public virtual string Vredpar7 { get; set; }
        public virtual string Vredpar8 { get; set; }
    }
}


