using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Records
{
    public record CityValidationResult
    {
        public bool IsValid { get; init; }
        public string Message { get; set; }
        public static CityValidationResult Invalid(string Message) => new() {IsValid = false, Message = Message};
        public static CityValidationResult Valid(string Message) => new() {IsValid = true,  Message = Message};
    }
}