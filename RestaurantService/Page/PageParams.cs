using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Page
{
    public class PageParams
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}