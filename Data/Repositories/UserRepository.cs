using System.Reflection.Metadata.Ecma335;
using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomSiteMVC.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserAndProfileByUserIdAsync(int userId)
        {
            return await _dbContext.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}