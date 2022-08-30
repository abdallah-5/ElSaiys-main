using ElSaiys.Constants;
using ElSaiys.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElSaiys.Models
{
    public class SeedingData
    {
        private ApplicationDbContext _context;

        public SeedingData(ApplicationDbContext context)
        {
            _context = context;
        }

        public void GenerateHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hash = new HMACSHA512())
            {
                passwordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordSalt = hash.Key;
            }
        }

        public async void SeedAdminUser()
        {
            if (!_context.Roles.Any(e => e.Name == Roles.Admin))
            {
                await _context.Roles.AddAsync(new Role { Id = "3bed8e1c-ee57-4a18-89a8-17d708ebe14d", Name = Roles.Admin });
            }

            if (!_context.Roles.Any(e => e.Name == Roles.Manager))
            {
                await _context.Roles.AddAsync(new Role { Id = "7f1963e1-5a02-4495-b290-d00a82ae28ae", Name = Roles.Manager });
            }

            if (!_context.Roles.Any(e => e.Name == Roles.User))
            {
                await _context.Roles.AddAsync(new Role { Id = "bacbd190-db69-4a20-a657-485d7b7a3205", Name = Roles.User });
            }

            byte[] hash, salt;

            GenerateHash("Aa@123456789", out hash, out salt);

            var user = new User
            {
                Id = "86f344cd-5168-4c2f-8dfb-0eef5bebc743",
                FirstName = "Ali",
                LastName = "Nasser",
                Username = "AliEn",
                PhoneNumber = "01151088932",
                Email = "ali.nasser.9.1997@gmail.com",
                IsPrivate = false,
                Passwordhash = hash,
                Passwordsalt = salt,
                Slug = "f0232f0b-7c8b-4ea0-8c9e-f209ab5a40fc",
                RoleId = "3bed8e1c-ee57-4a18-89a8-17d708ebe14d"
            };

            if (!_context.Users.Any(e => e.Username == user.Username))
            {
                await _context.Users.AddAsync(user);
            }

            await _context.SaveChangesAsync();
        }
    }
}
