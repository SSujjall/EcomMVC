using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Interfaces.IServices
{
    public interface IAdminService
    {
        public Task<User?> LoginAdminUser(LoginDTO model);
        public Task<IEnumerable<User>> GetAdminUsers();
        public Task<bool> DeleteAdminUser(int id);
    }
}
