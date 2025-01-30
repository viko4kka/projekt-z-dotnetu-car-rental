using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class QueryObject
    {
        // public decimal? PricePerDay { get; set; } = null;
        // public string? Brand { get; set; } = null;

        public string? SortBy { get; set; } = null;
        public bool IsDecsending { get; set; } = false;
    }
}


//price & brand