using System.Reflection.Metadata.Ecma335;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Infrastructure.Data.Contexts;
using EcomSiteMVC.Core.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EcomSiteMVC.Infrastructure.Repositories
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