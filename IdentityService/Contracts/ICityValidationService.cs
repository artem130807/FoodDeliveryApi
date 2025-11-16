using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Records;

namespace IdentityService.Contracts
{
    public interface ICityValidationService
    {
        Task<CityValidationResult> ValidCityAsync(string City);
    }
}