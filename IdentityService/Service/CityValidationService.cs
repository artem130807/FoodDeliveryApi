using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityService.Contracts;
using IdentityService.DadataApiModels;
using IdentityService.Records;

namespace IdentityService.Service
{
    public class CityValidationService : ICityValidationService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly string _secretKey;
        public CityValidationService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _apiKey = config["Dadata:ApiKey"];
            _secretKey = config["Dadata:SecretKey"];
        }
        public async Task<CityValidationResult> ValidCityAsync(string City)
        {
            var city = $"{City}";
            var requestData = new List<string> { city };
            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://cleaner.dadata.ru/api/v1/clean/address")
            {
                Content = content
            };
            requestMessage.Headers.Add("Authorization", $"Token {_apiKey}");
            requestMessage.Headers.Add("X-Secret", _secretKey);
            var responce = await _client.SendAsync(requestMessage);
            if (!responce.IsSuccessStatusCode)
            {
                return HandleErrorResponse(responce.StatusCode);
            }
            var responceContent = await responce.Content.ReadAsStringAsync();
            return ProcessDadataResponse(responceContent);
        }
        private CityValidationResult ProcessDadataResponse(string responseContent)
        {
            try
            {
                var dadataResponse = JsonSerializer.Deserialize<List<DadataResponce>>(responseContent);

                if (dadataResponse == null || dadataResponse.Count == 0)
                {
                    return CityValidationResult.Invalid("Город не найден");
                }
                var result = dadataResponse[0];               
                if (result.QualityCheck > 0)
                {
                    return CityValidationResult.Invalid("Город указан с ошибками");
                }
                bool IsValidRegion = ValidateRegion(result.Region);
                if (!IsValidRegion)
                {
                    return CityValidationResult.Invalid("Вы неправильно указали регион");
                }                          
                var hasValidCity = !string.IsNullOrEmpty(result.City) || !string.IsNullOrEmpty(result.Region);

                if (!hasValidCity)
                {
                    return CityValidationResult.Invalid("Вы не указали город");
                }
                
                return CityValidationResult.Valid("Успешно");

            }catch(JsonException)
            {
                return CityValidationResult.Invalid("Ошибка обработки ответа от сервиса адресов");
            }
        }
        private CityValidationResult HandleErrorResponse(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.Forbidden =>
                CityValidationResult.Invalid("Неверный API ключ или закончился лимит запросов"),
                HttpStatusCode.Unauthorized =>
                CityValidationResult.Invalid("Ошибка авторизации в сервисе проверки адресов"),
                _ => CityValidationResult.Invalid("Сервис проверки адресоs временно недоступен")
            };
        }
        private bool ValidateRegion(string region)
        {
            string[] russiaRegion = {"Russia", "Россия"};
            return russiaRegion.Any(cuisine => cuisine.Equals(region, StringComparison.OrdinalIgnoreCase));
        }
    }
}