namespace EcomSiteMVC.Interfaces.IServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}