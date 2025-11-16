using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Contracts;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityService.Service
{
    public class EmailVerificationService : IEmailVerficationService
    {
        private readonly IdentityDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;
        private readonly IDnsEmailValidator _emailValidator;
        public EmailVerificationService(IdentityDbContext context, IMemoryCache cache, IEmailService emailService, IDnsEmailValidator emailValidator)
        {
            _context = context;
            _emailService = emailService;
            _cache = cache;
            _emailValidator = emailValidator;
        }
        private string GenerateCode() => new Random().Next(100000, 999999).ToString();
               
        public async Task SendVerificationAsync(string Email)
        {
            await _emailValidator.ValidateEmailAsync(Email);
            var emailverif = new EmailVerification
            {
                Id = Guid.NewGuid(),
                Email = Email,
                Code = GenerateCode(),
                CratedDate = DateTime.Now,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            };
            await _context.EmailVerifications.AddAsync(emailverif);
            var cacheKey = $"verification_{emailverif.Code}";
            _cache.Set(cacheKey, Email, TimeSpan.FromMinutes(30));
            await _emailService.SendVerificationService(emailverif.Email, emailverif.Code);
            await _context.SaveChangesAsync();
           
        }
        public async Task<bool> Verificate(string Code, string Email)
        {
            var result = await _context.EmailVerifications.Where(x => x.Code == Code && x.Email == Email && !x.IsUsed)
            .OrderByDescending(x => x.ExpiresAt)
            .FirstOrDefaultAsync();
            if (result == null || result.ExpiresAt < DateTime.UtcNow)
                return false;
            result.IsUsed = true;
            await _context.SaveChangesAsync();
            var cacheKey = $"email_verified_{Email}";
            _cache.Set(cacheKey, true, TimeSpan.FromMinutes(30));
            return true;
        }

        public async Task DeleteEmailVerificate(string email)
        {
            await _context.EmailVerifications.Where(x => x.Email == email).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }
    }
}