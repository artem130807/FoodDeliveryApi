using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.RecordsVerificate
{
    public record ValidationRestaurant
    {
        public bool IsValid { get; init; }
        public string Message { get; set; }
        public static ValidationRestaurant Invalid(string Message) => new() {IsValid = false, Message = Message};
        public static ValidationRestaurant Valid(string Message) => new() { IsValid = true, Message = Message };
    }
}