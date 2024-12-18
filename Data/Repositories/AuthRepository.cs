using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomSiteMVC.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _dbContext;

        public AuthRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var result = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
            return result;
        }
        public async Task<User> AddUser(User user)
        {
            var result = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var result = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
            return result;
        }
    }
}
