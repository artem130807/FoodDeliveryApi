using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RestaurantService.Contracts;
using RestaurantService.DadataApiModels;
using RestaurantService.Records;

namespace RestaurantService.Service
{
    public class AddressValidationService : IAddressValidationService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly string _secretKey;
        public AddressValidationService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _apiKey = config["Dadata:ApiKey"];
            _secretKey = config["Dadata:SecretKey"];
        }
        public async Task<RestaurantAddressValidation> ValidateAddressAsync(string city, string street, string houseNumber)
        {
            var address = $"{city}, {street}, {houseNumber}";
            var requestData = new List<string> { address };
            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://cleaner.dadata.ru/api/v1/clean/address")
            {
                Content = content
            };
            requestMessage.Headers.Add("Authorization", $"Token {_apiKey}");
            requestMessage.Headers.Add("X-Secret", _secretKey);
            var response = await _client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                return HandleErrorResponse(response.StatusCode);
            }
            var responceContent = await response.Content.ReadAsStringAsync();
            return ProcessDadataResponse(responceContent);
        }
        private RestaurantAddressValidation ProcessDadataResponse(string responseContent)
        {
            try
            {
                var dadataResponse = JsonSerializer.Deserialize<List<DadataResponse>>(responseContent);

                if (dadataResponse == null || dadataResponse.Count == 0)
                {
                    return RestaurantAddressValidation.Invalid("Адрес не найден");
                }

                var result = dadataResponse[0];

                
                if (result.QualityCheck > 2)
                {
                    return RestaurantAddressValidation.Invalid("Адрес указан с ошибками");
                }

                
                var hasValidCity = !string.IsNullOrEmpty(result.City) || !string.IsNullOrEmpty(result.Region);
                var hasValidStreet = !string.IsNullOrEmpty(result.Street);
                var hasValidHouse = !string.IsNullOrEmpty(result.House);

                if (!hasValidCity || !hasValidStreet || !hasValidHouse)
                {
                    return RestaurantAddressValidation.Invalid("Адрес указан неполно");
                }

                return RestaurantAddressValidation.Valid("Адрес подтверждён");
            }

            catch (JsonException)
            {
                return RestaurantAddressValidation.Invalid("Ошибка обработки ответа от сервиса адресов");
            }
        }
        private RestaurantAddressValidation HandleErrorResponse(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
            HttpStatusCode.Forbidden => 
            RestaurantAddressValidation.Invalid("Неверный API ключ или закончился лимит запросов"),
            HttpStatusCode.Unauthorized => 
            RestaurantAddressValidation.Invalid("Ошибка авторизации в сервисе проверки адресов"),
            _ => RestaurantAddressValidation.Invalid("Сервис проверки адресов временно недоступен")
            };
        }
    }
}