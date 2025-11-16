using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RestaurantService.Contracts;
using RestaurantService.Dto.DtoRestaurant;
using RestaurantService.RecordsVerificate;

namespace RestaurantService.ValidationService
{
    public class ValidationRestaurantService:IValidationRestaurantService
    {
        private readonly IRestaurantRepository _repository;
        public static string[] Cuisines = {"Итальянская", "Японская", "Русская", "Американская", "Китайская", "Грузинская", "Турецкая", "Индийская", "Мексиканская", "Узбекская", "Другая"};
        public ValidationRestaurantService(IRestaurantRepository repository)
        {
            _repository = repository;
        }

        public async Task<ValidationRestaurant> ValidateCreateRestaurantAsync(DtoCreateRestaurant dtoCreateRestaurant)
        {
            bool IsValidationCuisines = ValidationCuisines(dtoCreateRestaurant.CuisineType);
            var restaurant = await _repository.GetRestaurantByName(dtoCreateRestaurant.Name);
            if (restaurant != null)
            {
                return ValidationRestaurant.Invalid("Ресторан с таким названием уже существует");
            }
            if(dtoCreateRestaurant.Name == null)
            {
                return ValidationRestaurant.Invalid("Некорретное название ресторана.");
            }
            if (dtoCreateRestaurant.Name.Length < 3 || dtoCreateRestaurant.Name.Length > 10)
            {
                return ValidationRestaurant.Invalid("Некорретное название ресторана. Минимальная количество символов 3, максимальное 10");
            }
            if (dtoCreateRestaurant.Phone == null)
            {
                return ValidationRestaurant.Invalid("Номер телефона обязателен");
            }
            if (dtoCreateRestaurant.Phone.Length != 10)
            {
                return ValidationRestaurant.Invalid("Некорректный номер телефона, должно быть 10 цифр, после +7");
            }
            if (!dtoCreateRestaurant.Phone.StartsWith("+7"))
            {
                return ValidationRestaurant.Invalid("Некорректный номер телефона, должен начинаться с '+7'");
            }
            if (!IsValidationCuisines)
            {
                return ValidationRestaurant.Invalid("Вы указали неверный тип кухни, если в списке кухней нету той, которая вам нужна, то укажите 'Другая'");
            }
            if (dtoCreateRestaurant.DeliveryTime < 15 || dtoCreateRestaurant.DeliveryTime > 120)
            {
                return ValidationRestaurant.Invalid("Диапазон доставки должен быть 15-120 минут");
            }
            if (dtoCreateRestaurant.DeliveryFee < 0 || dtoCreateRestaurant.DeliveryFee > 100)
            {
                return ValidationRestaurant.Invalid("Диапазон цены доставки от 0 до 100 рублей");
            }          
            if (dtoCreateRestaurant.ImageUrl.Length > 2000)
            {
                return ValidationRestaurant.Invalid("Изображение не может быть больше 2000 символов");
            }
            if (dtoCreateRestaurant.ImageUrl == null || !dtoCreateRestaurant.ImageUrl.StartsWith("http"))
            {
                return ValidationRestaurant.Invalid("Некорректный формат изображения");
            }
            if (dtoCreateRestaurant.CuisineType == null)
            {
                return ValidationRestaurant.Invalid("Тип кухни обязателен");
            }    
            return ValidationRestaurant.Valid("Успешно");
        }

        public async Task<ValidationRestaurant> ValidateUpdateRestaurantAsync(Guid Id, DtoUpdateRestaurant dtoUpdateRestaurant)
        {
            bool IsValidationCuisines = ValidationCuisines(dtoUpdateRestaurant.CuisineType);
            var restaurant = await _repository.GetRestaurantById(Id);
            if (restaurant == null)
            {
                return ValidationRestaurant.Invalid("Такого ресторона не существует");
            }
            if (dtoUpdateRestaurant.Name == null)
            {
                return ValidationRestaurant.Invalid("Некорретное название ресторана.");
            }
            if (dtoUpdateRestaurant.Phone == null)
            {
                return ValidationRestaurant.Invalid("Номер телефона обязателен");
            }
            if (dtoUpdateRestaurant.Phone.Length != 13)
            {
                return ValidationRestaurant.Invalid("Некорректный номер телефона, должно быть 11 цифр после +7");
            }
            if (!dtoUpdateRestaurant.Phone.StartsWith("+"))
            {
                return ValidationRestaurant.Invalid("Некорректный номер телефона, должен содержать '+'");
            }
            if (!IsValidationCuisines)
            {
                return ValidationRestaurant.Invalid("Вы указали неверный тип кухни, если в списке кухней нету той, которая вам нужна, то укажите 'Другая'");
            }
            if (dtoUpdateRestaurant.DeliveryTime < 15 || dtoUpdateRestaurant.DeliveryTime > 120)
            {
                return ValidationRestaurant.Invalid("Диапазон доставки должен быть 15-120 минут");
            }
            if (dtoUpdateRestaurant.DeliveryFee < 0 || dtoUpdateRestaurant.DeliveryFee > 100)
            {
                return ValidationRestaurant.Invalid("Диапазон цены доставки от 0 до 100 рублей");
            }
            if (dtoUpdateRestaurant.ImageUrl.Length > 2000)
            {
                return ValidationRestaurant.Invalid("Изображение не может быть больше 2000 символов");
            }
            if (dtoUpdateRestaurant.ImageUrl == null || !dtoUpdateRestaurant.ImageUrl.StartsWith("http"))
            {
                return ValidationRestaurant.Invalid("Некорректный формат изображения");
            }
            if (dtoUpdateRestaurant.CuisineType == null)
            {
                return ValidationRestaurant.Invalid("Тип кухни обязателен");
            }
            
            return ValidationRestaurant.Valid("Успешно");
        }
        private bool ValidationCuisines(string cuisines) 
        {
           return Cuisines.Any(cuisine => cuisine.Equals(cuisines, StringComparison.OrdinalIgnoreCase));
        }
    }
}