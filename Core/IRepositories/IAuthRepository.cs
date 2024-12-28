using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Core.IRepositories
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByUsername(string username);
        Task<User> AddUser(User user);
        Task<User?> GetUserByEmail(string email);
    }
}
