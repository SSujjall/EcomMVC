using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Utilities.Helpers;

namespace EcomSiteMVC.Infrastructure.Services
{
    public class OtpService(IUserRepository userRepository) : IOtpService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<string> GeneratePasswordChangeOtp(int userId)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null) return null;

            var otp = OtpHelper.GeneratePasswordChangeOTP();
            user.PasswordChangeOTP = otp.EncryptParameter();
            await _userRepository.Update(user);

            return otp;
        }

        public async Task<bool> VerifyPasswordChangeOtp(int userId, string otp)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null) return false;

            return OtpHelper.VerifyPasswordChangeOTP(otp, user.PasswordChangeOTP?.DecryptParameter());
        }
    }
}
