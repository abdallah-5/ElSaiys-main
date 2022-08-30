using AutoMapper;
using ElSaiys.Constants;
using ElSaiys.DTOs;
using ElSaiys.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElSaiys.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _map;
        private readonly IConfiguration _config;

        public AuthRepository(ApplicationDbContext context, IMapper map, IConfiguration config)
        {
            _context = context;
            _map = map;
            _config = config;
        }

        public async Task<AuthResult> Registration(UserRegisterDTO userRegisterDTO)
        {
            if (_context.Users.Any(e => e.Username.Equals(userRegisterDTO.Username)))
                return new AuthResult { Success = false, ErrorCode = "Usr001" };

            if (_context.Users.Any(e => e.Email.Equals(userRegisterDTO.Email)))
                return new AuthResult { Success = false, ErrorCode = "Usr002" };

            var user = _map.Map<User>(userRegisterDTO);
            byte[] hash, salt;
            GenerateHash(userRegisterDTO.Password, out hash, out salt);
            user.Passwordhash = hash;
            user.Passwordsalt = salt;
            user.RoleId = userRegisterDTO.RoleId == null ? await _context.Roles.Where(e => e.Name == Roles.User).Select(e => e.Id).FirstAsync() : userRegisterDTO.RoleId;

            await _context.Users.AddAsync(user);
            _context.SaveChanges();

            return new AuthResult
            {
                Success = true,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                IsPrivate = user.IsPrivate,
                Slug = user.Slug,
                Role = await _context.Roles.Where(e => e.Id == user.RoleId).Select(e => e.Name).FirstAsync()
            };
        }

        public async Task<AuthResult> Login(UserLoginDTO userLoginDTO)
        {
            var isEmail = IsValidEmail(userLoginDTO.Username);
            User userExist = new User();

            if (isEmail)
                userExist = await _context.Users.FirstOrDefaultAsync(e => e.Email == userLoginDTO.Username);
            else
                userExist = await _context.Users.FirstOrDefaultAsync(e => e.Username == userLoginDTO.Username);

            if (userExist.IsDelete == true)
                return new AuthResult { Success = false, ErrorCode = "Usr006" };

            if (userExist == null)
                return new AuthResult { Success = false, ErrorCode = "Usr003" };

            if(!ValidateHash(userLoginDTO.Password, userExist.Passwordhash, userExist.Passwordsalt))
                return new AuthResult { Success = false, ErrorCode = "Usr004" };

            // Generate JWT Token
            var key = _config.GetValue<string>("JWTSecret");
            var keyByte = Encoding.ASCII.GetBytes(key);
            var desc = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyByte), SecurityAlgorithms.HmacSha512Signature),
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(JwtRegisteredClaimNames.Sub, userExist.Username),
                    new Claim(JwtRegisteredClaimNames.Email, userExist.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", userExist.Id)
                })
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(desc);

            return new AuthResult
            {
                Success = true,
                Id = userExist.Id,
                FirstName = userExist.FirstName,
                LastName = userExist.LastName,
                Username = userExist.Username,
                PhoneNumber = userExist.PhoneNumber,
                Email = userExist.Email,
                IsPrivate = userExist.IsPrivate,
                Slug = userExist.Slug,
                Role = await _context.Roles.Where(e => e.Id == userExist.RoleId).Select(e => e.Name).FirstAsync(),
                Token = handler.WriteToken(token)
            };
        }

        public void GenerateHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hash = new HMACSHA512())
            {
                passwordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordSalt = hash.Key;
            }
        }

        private bool ValidateHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hash = new HMACSHA512(passwordSalt))
            {
                var newPassword = hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < newPassword.Length; i++)
                    if (newPassword[i] != passwordHash[i])
                        return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
