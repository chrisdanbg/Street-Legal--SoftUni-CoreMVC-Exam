using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using StreetLegal.Services.Contracts;
using System.Threading.Tasks;

namespace StreetLegal.Services
{
    public class PhotoRepository : IPhotoRepository
    {
        public Task<string> UploadImageToStorage(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            Account acc = new Account(
                "chrisdanbg",
                "144564433746877",
                "g8NzLV8lnZp-5ou8SuVo0rGkhwY"
            );

            Cloudinary cloudinary = new Cloudinary(acc);

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        //Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = cloudinary.Upload(uploadParams);
                }
            }

            return Task.FromResult(uploadResult.SecureUri.ToString());
        }
    }
}
