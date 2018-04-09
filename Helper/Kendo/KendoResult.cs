using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webkom.Helper.Kendo
{
    public class KendoResult<T>
    {
        public IEnumerable<T>  Data { get; set; }
        public int Total { get; set; }
    }
}
