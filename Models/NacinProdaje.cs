﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace webkom.Models
{
  public class NacinProdaje : Entitet
  {
      public virtual string Id { get; set; }
      public virtual string Naziv { get; set; }

  }
}


