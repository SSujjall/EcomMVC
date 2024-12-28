using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Core.IServices
{
    public interface IAdminService
    {
        public Task<User?> LoginAdminUser(LoginDTO model);
        public Task<IEnumerable<User>> GetAdminUsers();
        public Task<bool> DeleteAdminUser(int id);
    }
}
