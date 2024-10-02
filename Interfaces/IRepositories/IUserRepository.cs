using EcomSiteMVC.Data.Repositories;
using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Interfaces.IRepositories
{
    public interface IUserRepository : IRepositoryBase<UserProfile>
    {
        Task<UserProfile> GetUserProfileByUserIdAsync(int userId);
    }
}
