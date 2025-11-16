using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantService.Records;

namespace RestaurantService.Contracts
{
    public interface IAddressValidationService
    {
        Task<RestaurantAddressValidation> ValidateAddressAsync(string city, string street, string houseNumber);
    }
}