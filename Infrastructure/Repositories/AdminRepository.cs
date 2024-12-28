using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Infrastructure.Data.Contexts;

namespace EcomSiteMVC.Infrastructure.Repositories
{
    public class AdminRepository : RepositoryBase<User>, IAdminRepository
    {
        public AdminRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
