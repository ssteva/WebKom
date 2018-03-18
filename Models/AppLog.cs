using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace webkom.Models
{
    public class AppLog 
    {
        public virtual int Id { get; set; }
        public virtual string Entity { get; set; }
        public virtual string Property { get; set; }
        public virtual string EntityId { get; set; }
        public virtual string StaraVrednost { get; set; }
        public virtual string NovaVrednost { get; set; }
        public virtual string Akcija { get; set; }
        public virtual string Korisnik { get; set; }
        public virtual DateTime Azudat { get; set; }

    }
}
