using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace webkom.Models
{
  public class NacinPlacanja : Entitet
  {
      public virtual int Id { get; set; }
      public virtual string Naziv { get; set; }
      
  }
}


