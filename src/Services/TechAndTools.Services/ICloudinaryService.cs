﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TechAndTools.Services
{
    public interface ICloudinaryService
    {
        Task<string> UploadPictureAsync(IFormFile pictureFile, string fileName);
    }
}
