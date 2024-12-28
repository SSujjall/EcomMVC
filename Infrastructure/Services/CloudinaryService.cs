using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EcomSiteMVC.Core.Enums;
using EcomSiteMVC.Core.IServices;

namespace EcomSiteMVC.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private string folder = string.Empty;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(IFormFile file, FolderName folderName)
        {
            if (file.Length > 0)
            {
                string folder;
                Transformation transformation = null;

                switch (folderName)
                {
                    case FolderName.ProfilePictures:
                        folder = "ProfilePictures";
                        transformation = new Transformation().Width(500).Height(500).Crop("fill");
                        break;
                    case FolderName.Ecom:
                        folder = "Ecom";
                        break;
                    default:
                        folder = "Ecom";
                        break;
                }

                using var stream = file.OpenReadStream();
                var uploadParam = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder,
                    Transformation = transformation
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParam);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return uploadResult.SecureUrl.ToString();
                }
            }

            return null;
        }

        public async Task<List<string>> UploadMultipleImageAsync(List<IFormFile> files, FolderName folderName)
        {
            if (files != null || files.Count != 0)
            {
                var uploadedImageUrls = new List<string>();

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var uploadedUrl = await UploadImageAsync(file, folderName);
                        if (!string.IsNullOrEmpty(uploadedUrl))
                        {
                            uploadedImageUrls.Add(uploadedUrl);
                        }
                    }
                }

                return uploadedImageUrls;
            }
            return null;
        }

        public async Task DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
            {
                return; // No image to delete
            }

            var deletionResult = await _cloudinary.DeleteResourcesAsync(ResourceType.Image, publicId);

            if (deletionResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                // Handle deletion error if necessary
                Console.WriteLine($"Error deleting image: {deletionResult.Error.Message}");
            }
        }
    }
}
