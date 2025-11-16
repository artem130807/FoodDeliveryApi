using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using IdentityService.Contracts;
using IdentityService.Records;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Tls;

namespace IdentityService.Service
{
    public class PasswordValidatorService : IPasswordValidatorService
    {
        private static readonly Regex CyrillicRegex = new Regex(@"\p{IsCyrillic}", RegexOptions.Compiled);
        private static readonly Regex EnglishLettersRegex = new Regex(@"[a-zA-Z]", RegexOptions.Compiled); 
        private static readonly Regex InvalidPasswordCharsRegex = new Regex(@"[!$%^&*()+=|<>?{}\[\]~]", RegexOptions.Compiled);
        public async Task<PasswordValidationResult> ValidatePasswordAsync(string password)
        {
            if (password.Contains(" "))
            {
                return PasswordValidationResult.Invalid("Пароль не может содержать пробелы");
            }
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                return PasswordValidationResult.Invalid("Вы не указали пароль");
            }
            if (password.Length != 8)
            {
                return PasswordValidationResult.Invalid("Пароль должен содержать 6 символов");
            }
            bool CyrillicLetters = ContainsCyrillicLetters(password);
            bool PasswordChars = ContainsPasswordChars(password);
            if (CyrillicLetters)
            {
                return PasswordValidationResult.Invalid("Пароль не может содержать русские буквы");
            }
            if (PasswordChars)
            {
                return PasswordValidationResult.Invalid("Пароль не может содержать символы [!$%^&*()+=|<>?{}[]~] и т.д");
            }
            if (!Contains3letters(password))
            {
                return PasswordValidationResult.Invalid("Пароль должен содержать только 3 английские буквы");
            }
            if (!Nonrepeatingcharacters(password))
            {
                return PasswordValidationResult.Invalid("Символы в пароле не могут повторяться более 3 раз");
            }
            return PasswordValidationResult.Valid("Валидный пароль");
        }
        private bool ContainsPasswordChars(string password)
        {
            return InvalidPasswordCharsRegex.IsMatch(password);
        }
        private bool ContainsCyrillicLetters(string password)
        {
            return CyrillicRegex.IsMatch(password);
        }
        private bool Contains3letters(string password)
        {
            int count = 0;
            foreach (var pas in password)
            {
                if (EnglishLettersRegex.IsMatch(pas.ToString()))
                {
                    count++;     
                }
            }
            return count == 3;
        }
        private bool Nonrepeatingcharacters(string password)
        {
            return !password.GroupBy(x => x).Any(g => g.Count() > 3);
        }
    }
    
}