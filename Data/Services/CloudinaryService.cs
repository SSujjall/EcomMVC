using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EcomSiteMVC.Interfaces.IServices;

namespace EcomSiteMVC.Data.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
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

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var uploadParam = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Transformation = new Transformation().Width(500).Height(500).Crop("fill")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParam);
            return uploadResult?.SecureUrl.ToString();
        }

        public async Task<string> UploadProfilePictureAsync(IFormFile file)
        {
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "ProfilePictures" // Folder to store images in Cloudinary
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return uploadResult.SecureUrl.ToString();
                }
            }

            return null;
        }
    }
}
