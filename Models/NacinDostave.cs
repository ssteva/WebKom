using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace webkom.Models
{
    public class NacinDostave : Entitet
    {
        public virtual string Id { get; set; }
        public virtual string Naziv  { get; set; }
        
    }
}


