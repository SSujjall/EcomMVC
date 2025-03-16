using EcomSiteMVC.Core.Enums;

namespace EcomSiteMVC.Utilities.ExternalServices.CloudinaryService.Service
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file, FolderName folderName);
        Task<List<string>> UploadMultipleImageAsync(List<IFormFile> files, FolderName folderName);
        public Task DeleteImageAsync(string publicId);
    }
}