using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace webkom.Models
{
    public class Entitet : BazniEntitet
    {
        [JsonIgnore]
        public virtual DateTime? DateCreated { get; set; }
        [JsonIgnore]
        public virtual DateTime? DateModified { get; set; }
        public virtual string UserCreated { get; set; }
        public virtual string UserModified { get; set; }
        public virtual bool Obrisan { get; set; }
        [JsonIgnore]
        public virtual bool Zakljucan { get; set; }
        public virtual bool Edit { get; set; }
    }
}
