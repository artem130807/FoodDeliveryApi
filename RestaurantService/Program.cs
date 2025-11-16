using RestaurantService;
using RestaurantService.Cache;
using RestaurantService.Contracts;
using RestaurantService.Mapper;
using RestaurantService.Middleware;
using RestaurantService.Repositories;
using RestaurantService.Service;
using RestaurantService.ValidationService;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof (MapperProfile));
///База данных
builder.Services.AddDb(configuration);
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IAddressValidationService, AddressValidationService>(client =>
{
    client.BaseAddress = new Uri("https://cleaner.dadata.ru");
    client.DefaultRequestHeaders.Add("Authorization", $"Token {builder.Configuration["Dadata:ApiKey"]}");
    client.DefaultRequestHeaders.Add("X-Secret", builder.Configuration["Dadata:SecretKey"]);
});
///Рестораны
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IRestaurantServices, RestaurantServices>();
builder.Services.AddScoped<IValidationRestaurantService, ValidationRestaurantService>();
builder.Services.AddScoped<IAddressValidationService, AddressValidationService>();
builder.Services.AddScoped<IRestaurantStatusService, RestaurantStatusService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<CacheKeyManager>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();