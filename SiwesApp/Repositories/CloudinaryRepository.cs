using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SiwesApp.Dtos.All;
using SiwesApp.Interfaces;
using SiwesApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SiwesApp.Repositories
{
    public class CloudinaryRepository : ICloudinaryRepository
    {
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly Cloudinary _cloudinary;

        public CloudinaryRepository(IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;

            var acc = new Account(_cloudinaryConfig.Value.CloudName, _cloudinaryConfig.Value.ApiKey, _cloudinaryConfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
        }


        public ToRespond DeleteFilesFromCloudinary(List<string> attachedFilesPublicIds)
        {
            try
            {
                if (attachedFilesPublicIds == null || !attachedFilesPublicIds.Any())
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.ObjectNull,
                        StatusMessage = Helpers.StatusMessageObjectNull
                    };
                }
            }
            catch (ArgumentNullException ex)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = ex.Message
                };
            }

            var deleteResult = _cloudinary.DeleteResources(new DelResParams()
            {
                PublicIds = attachedFilesPublicIds
            });

            if (deleteResult.StatusCode != HttpStatusCode.OK)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.CloudinaryFileDeleteError,
                    StatusMessage = "Error dectected while trying to delete"
                };
            }

            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                ObjectValue = deleteResult,
                StatusMessage = Helpers.StatusMessageSuccess
            };
        }

        public ToRespond UploadFilesToCloudinary(IFormFileCollection formFiles)
        {
            try
            {
                if (formFiles == null || !formFiles.Any())
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.ObjectNull,
                        StatusMessage = Helpers.StatusMessageObjectNull
                    };
                }
            }
            catch (ArgumentNullException ex)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = ex.Message
                };
            }

            var uploadedFiles = new List<RawUploadResult>();
            foreach (var attachmentFile in formFiles)
            {
                var file = attachmentFile;
                var uploadResult = new RawUploadResult();
                if (file.Length > 0 && file.Length < (100 * 1024))  //MAXIMUM FILE SIZE - 100MB
                {
                    using (var stream = file.OpenReadStream())
                    {
                        uploadResult = _cloudinary.Upload("auto", null, new FileDescription(file.Name, stream));
                    }


                    uploadedFiles.Add(uploadResult);
                }
                else
                {
                    return new ToRespond()
                    {
                        StatusCode = Helpers.InvalidFileSize,
                        StatusMessage = "Size too big"
                    };
                }
            }

            return new ToRespond()
            {
                StatusCode = Helpers.Success,
                StatusMessage = Helpers.StatusMessageSuccess,
                ObjectValue = uploadedFiles
            };
        }

        public ToRespond UploadFileToCloudinary(IFormFile formFile)
        {
            if (formFile == null)
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.ObjectNull,
                    StatusMessage = Helpers.StatusMessageObjectNull
                };
            }

            var file = formFile;
            var uploadResult = new RawUploadResult();
            if (file.Length > 0 && file.Length < (100 * 1024 * 1024))  //MAXIMUM FILE SIZE - 100MB
            {
                using (var stream = file.OpenReadStream())
                {
                    uploadResult = _cloudinary.Upload("auto", null, new FileDescription(file.Name, stream));
                }

                return new ToRespond()
                {
                    StatusCode = Helpers.Success,
                    ObjectValue = uploadResult,
                    StatusMessage = Helpers.StatusMessageSuccess
                };
            }
            else
            {
                return new ToRespond()
                {
                    StatusCode = Helpers.InvalidFileSize,
                    StatusMessage = "File too big"
                };
            }
        }
    }
}
