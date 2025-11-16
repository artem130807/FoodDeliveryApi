using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantService.Dto.DtoRestaurant;
using RestaurantService.Models;
using RestaurantService.RecordsVerificate;

namespace RestaurantService.Contracts
{
    public interface IValidationRestaurantService
    {
        Task<ValidationRestaurant> ValidateCreateRestaurantAsync(DtoCreateRestaurant dtoCreateRestaurant);
        Task<ValidationRestaurant> ValidateUpdateRestaurantAsync(Guid Id, DtoUpdateRestaurant dtoUpdateRestaurant);
    }
}