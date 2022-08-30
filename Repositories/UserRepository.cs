using AutoMapper;
using ElSaiys.DTOs;
using ElSaiys.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _map;
        private readonly IConfiguration _config;

        public UserRepository(ApplicationDbContext context, IMapper map, IConfiguration config)
        {
            _context = context;
            _map = map;
            _config = config;
        }

        public async Task<UserResult> CarOwner(string slug)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Slug == slug && e.IsDelete == false);

            if (user == null)
                return new UserResult { Success = false, ErrorCode = "Usr005" };

            return new UserResult
            {
                Success = true,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsPrivate = user.IsPrivate,
                Slug = user.Slug
            };
        }

        public async Task<UserResult> UserExist(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);

            if (user == null)
                return new UserResult { Success = false, ErrorCode = "Usr007" };

            return new UserResult
            {
                Success = true,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsPrivate = user.IsPrivate,
                Slug = user.Slug
            };
        }

        public int GenrateVerificationCode(UserResult userResult)
        {
            var user = _context.Users.Find(userResult.Id);
            Random random = new Random();
            user.VerificationCode = random.Next();

            _context.Users.Update(user);
            _context.SaveChanges();

            return user.VerificationCode;
        }

        public async Task<UserResult> CheckVerificationCode(string email, int verificationCode)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);

            if (user.VerificationCode != verificationCode)
                return new UserResult { Success = false, ErrorCode = "Usr008" };

            return new UserResult
            {
                Success = true,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsPrivate = user.IsPrivate,
                Slug = user.Slug
            };
        }

        public void UpdatePassword(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(e => e.Email == email);

            byte[] hash, salt;
            var authRepository = new AuthRepository(_context, _map, _config);
            authRepository.GenerateHash(password, out hash, out salt);
            user.Passwordhash = hash;
            user.Passwordsalt = salt;

            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
