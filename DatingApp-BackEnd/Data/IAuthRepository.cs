using System.Threading.Tasks;
using DatingApp_BackEnd.Models;

namespace DatingApp_BackEnd.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);

        Task<User> login (string username, string password);

        Task<bool> UserExists(string username);
        Task Login(string username, string password);
    }
}