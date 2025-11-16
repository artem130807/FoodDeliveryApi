using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Page
{
    public class PagedResult<T>
    {
        public List<T> Data { get; }
        public  int Count { get; }
        public PagedResult(List<T> data, int count)
        {
            Data = data;
            Count = count;
        }

    }
}
