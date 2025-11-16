using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantService.Contracts;

namespace RestaurantService.ValidationService
{
    public class RestaurantStatusService : IRestaurantStatusService
    {
        public bool IsActiveRestaurant(TimeSpan TimeOpen, TimeSpan TimeClose)
        {
            TimeSpan Time = DateTime.Now.TimeOfDay;
            bool result = Time >= TimeOpen && Time <= TimeClose;
            return result;
        }
    }
}