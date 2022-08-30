using ElSaiys.DTOs;
using System.Threading.Tasks;

namespace ElSaiys.Repositories
{
    public interface IUserRepository
    {
        Task<UserResult> CarOwner(string slug);
        Task<UserResult> CheckVerificationCode(string email, int verificationCode);
        int GenrateVerificationCode(UserResult user);
        void UpdatePassword(string email, string password);
        Task<UserResult> UserExist(string email);
    }
}