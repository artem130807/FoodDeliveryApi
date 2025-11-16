using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Contracts
{
    public interface IRestaurantStatusService
    {
        bool IsActiveRestaurant(TimeSpan TimeOpen, TimeSpan TimeClose);                         
    }
}