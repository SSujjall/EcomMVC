using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Data.Repositories
{
    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
