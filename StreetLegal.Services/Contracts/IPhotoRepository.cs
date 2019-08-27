using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.Services.Contracts
{
    public interface IPhotoRepository
    {
        Task<string> UploadImageToStorage(IFormFile file);

    }
}
