using ElSaiys.DTOs;
using System.Threading.Tasks;

namespace ElSaiys.Repositories
{
    public interface IAuthRepository
    {
        Task<AuthResult> Registration(UserRegisterDTO userRegisterDTO);
        Task<AuthResult> Login(UserLoginDTO userLoginDTO);
    }
}