using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Interfaces.IRepositories
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByUsername(string username);
        Task<User> AddUser(User user);
        Task<User?> GetUserByEmail(string email);
    }
}
