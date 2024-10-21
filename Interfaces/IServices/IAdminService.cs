using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Interfaces.IServices
{
    public interface IAdminService
    {
        public Task<IEnumerable<User>> GetAdminUsers();
    }
}
