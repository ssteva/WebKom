using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace webkom.Models
{
  public class Mesto : Entitet
  {
      public virtual int Id { get; set; }
      public virtual string Ptt { get; set; }
      public virtual string Naziv { get; set; }
      public virtual string Region { get; set; }

  }
}


