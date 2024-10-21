using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Data.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<IEnumerable<User>> GetAdminUsers()
        {
            var users = await _adminRepository.GetAllAsync();
            var adminUsers = users.Where(u => u.Role == Models.Enums.Role.Admin);
            return adminUsers;
        }
    }
}