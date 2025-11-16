using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityService.Contracts;
using IdentityService.Records;

namespace IdentityService.Service
{
    public class DnsEmailValidator : IDnsEmailValidator
    {
        
        public async Task<EmailValidationResult> ValidateEmailAsync(string Email)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Email))
            {
                return EmailValidationResult.Invalid("Email не может быть пустым");
            }
            if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(Email))
            {
            return EmailValidationResult.Invalid("Неверный формат email");
            }
            if (!Email.Contains("@"))
            {
                return EmailValidationResult.Invalid("Неверный формат email");
            }
            var domen = Email.Split("@")[1];
            try
            {
                var mxRecords = await Dns.GetHostAddressesAsync(domen);
                if(mxRecords.Length == 0)
                {
                    return EmailValidationResult.Invalid("Домент Email не существует");
                }
            }catch
            {
                return EmailValidationResult.Invalid("");
            }
            var disposibleDomains = new HashSet<string>
            {
                 "tempmail.com", "10minutemail.com", "guerrillamail.com",
                 "mailinator.com", "yopmail.com", "throwawaymail.com"
            };
            if (disposibleDomains.Contains(domen.ToLower()))
            {
                return EmailValidationResult.Invalid("Временные email не поддерживаются");
            }
            return EmailValidationResult.Valid("Валидный емейл");
        }
    }
}