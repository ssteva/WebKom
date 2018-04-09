using System.Collections.Generic;

namespace webkom.Helper.Kendo
{
    public class KendoRequest
    {
        public KendoRequest()
        {
            Filter = new FilterContainer();
            Sort = new List<Sort>();
        }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<Sort> Sort { get; set; }
        public FilterContainer Filter { get; set; }
    }

    public class Sort
    {
        public string Compare { get; set; }
        public string Field { get; set; }
        public string Dir { get; set; }
    }

}
