using Microsoft.AspNetCore.Http;
using SiwesApp.Dtos.All;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Interfaces
{
    public interface ICloudinaryRepository
    {
        public ToRespond UploadFilesToCloudinary(IFormFileCollection formFiles);
        public ToRespond UploadFileToCloudinary(IFormFile formFile);
        public ToRespond DeleteFilesFromCloudinary(List<string> attachedFilesPublicIds);
    }
}
