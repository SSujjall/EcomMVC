using EcomSiteMVC.Data.Repositories;
using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Interfaces.IRepositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserProfileByUserIdAsync(int userId);
    }
}
