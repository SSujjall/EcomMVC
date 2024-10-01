using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
