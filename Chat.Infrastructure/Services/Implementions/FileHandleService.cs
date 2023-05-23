﻿using Chat.Application.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Infrastructure.Services.Implementions
{
    public class FileHandleService : IFileHandleService
    {
        private readonly IWebRootPathProvider _webRootPathProvider;

        public FileHandleService(IWebRootPathProvider webRootPathProvider)
        {
            _webRootPathProvider = webRootPathProvider;
        }

        public Task<byte[]> LoadAsync(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SaveAsync(IFormFile data)
        {
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(data.FileName)}";
            string webRootPath = _webRootPathProvider.GetWebRootPath();
            string imagePath = Path.Combine(webRootPath, "Images", "Avatars", imageName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                try
                {
                   await data.CopyToAsync(stream);
                }
                catch(Exception)
                {
                    throw;
                }
            }

            return imagePath;
        }
    }
}