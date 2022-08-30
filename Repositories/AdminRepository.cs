using AutoMapper;
using ElSaiys.Constants;
using ElSaiys.DTOs;
using ElSaiys.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _map;

        public AdminRepository(ApplicationDbContext context, IMapper map)
        {
            _context = context;
            _map = map;
        }

        public async Task<List<UserResult>> AllUsers()
        {
            var userRoleId = await _context.Roles.Where(e => e.Name == Roles.User).Select(e => e.Id).FirstAsync();
            var test = await _context.Users.Where(e => e.RoleId == userRoleId && e.IsDelete == false).Select(e => _map.Map(e, new UserResult())).ToListAsync();
            return test;
        }

        public void DisableUser(string userId)
        {
            var user = _context.Users.Find(userId);

            user.IsDelete = true;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void UnDisableUser(string userId)
        {
            var user = _context.Users.Find(userId);

            user.IsDelete = false;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public async Task<List<UserResult>> TrashUsers()
        {
            var userRoleId = await _context.Roles.Where(e => e.Name == Roles.User).Select(e => e.Id).FirstAsync();
            var test = await _context.Users.Where(e => e.RoleId == userRoleId && e.IsDelete == true).Select(e => _map.Map(e, new UserResult())).ToListAsync();
            return test;
        }

        public void DeleteUser(string userId)
        {
            var user = _context.Users.Find(userId);

            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
