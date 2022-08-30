using ElSaiys.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElSaiys.Repositories
{
    public interface IAdminRepository
    {
        Task<List<UserResult>> AllUsers();
        void DeleteUser(string userId);
        void DisableUser(string userId);
        Task<List<UserResult>> TrashUsers();
        void UnDisableUser(string userId);
    }
}