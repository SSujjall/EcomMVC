using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomSiteMVC.Data.Repositories
{
    public class UserRepository : RepositoryBase<UserProfile>, IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserProfile> GetUserProfileByUserIdAsync(int userId)
        {
            return await FindByConditionAsync(up => up.UserId == userId);
        }
    }
}