using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Records
{
    public record RestaurantAddressValidation
    {
        public bool IsValid { get; init; }
        public string Message { get; set; }
        public static RestaurantAddressValidation Invalid(string Message) => new() {IsValid = false, Message = Message};
        public static RestaurantAddressValidation Valid(string Message) => new() { IsValid = true, Message = Message };
    }
}