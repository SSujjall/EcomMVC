namespace EcomSiteMVC.Interfaces.IServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadProfilePictureAsync(IFormFile file);
        Task<string> UploadImageAsync(IFormFile file);
        public Task DeleteImageAsync(string publicId);
    }
}