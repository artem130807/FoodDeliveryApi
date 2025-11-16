using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Records
{
    public record PasswordValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public static PasswordValidationResult Invalid(string Message) => new() { IsValid = false, Message = Message };  
        public static PasswordValidationResult Valid(string Message) => new() { IsValid = true, Message = Message};
    }
}