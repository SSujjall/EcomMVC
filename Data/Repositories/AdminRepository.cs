using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Data.Repositories
{
    public class AdminRepository : RepositoryBase<User>, IAdminRepository
    {
        public AdminRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
