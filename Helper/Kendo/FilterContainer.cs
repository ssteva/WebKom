using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webkom.Helper.Kendo
{
    public class FilterContainer
    {
        public FilterContainer()
        {
            Filters=  new List<FilterDescription>();
        }
        public IList<FilterDescription> Filters { get; set; }
        public string Logic { get; set; }
        //public int page { get; set; }
        //public int skip { get; set; }
        //public int take { get; set; }
        //public int pageSize { get; set; }
    }
}
