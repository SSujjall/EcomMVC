using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Infrastructure.Repositories;
using EcomSiteMVC.Core.DTOs;

namespace EcomSiteMVC.Core.IRepositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserAndProfileByUserIdAsync(int userId);
        Task<User> GetByUserEmailAsync(string email);
    }
}
