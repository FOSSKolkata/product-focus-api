using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace BusinessRequirements.Domain.Services
{
    public interface IBlobStorageService
    {
        public Task<BlobClient> AddAsync(BlobStorageFileTypeEnum blobStorageFileTypeEnum, long organizationId, long productId, long attachmentId, IFormFile file);
        public Task<Response> DeleteAsync(BlobStorageFileTypeEnum blobStorageFileTypeEnum, string attachmentName);
        public Task<List<KeyValuePair<BlobItem, BlobDownloadResult>>> GetAllAsync(BlobStorageFileTypeEnum blobStorageFileTypeEnum, long organizationId, long productId, long attachmentId);
    }

    public enum BlobStorageFileTypeEnum
    {
        BusinessRequirementAttachments = 1
    }
}
