using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using ProductFocus.Domain.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        public async Task<BlobClient> AddAsync(BlobStorageFileTypeEnum blobStorageFileTypeEnum, long organizationId, long productId, long attachmentId, IFormFile file)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(blobStorageFileTypeEnum.ToString().ToLower());
            if(!containerClient.Exists())
            {
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(blobStorageFileTypeEnum.ToString());
            }
            string folderPath = organizationId.ToString() + "/" + productId.ToString() + "/" + attachmentId.ToString() + "/";
            string fileName = Guid.NewGuid().ToString();
            BlobClient blobClient = containerClient.GetBlobClient(folderPath + fileName + Path.GetExtension(file.Name));
            await blobClient.UploadAsync(file.OpenReadStream(), new BlobHttpHeaders { ContentType = file.ContentType });
            return blobClient;
        }

        public async Task<Response> DeleteAsync(BlobStorageFileTypeEnum blobStorageFileTypeEnum, string attachmentName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(blobStorageFileTypeEnum.ToString().ToLower());
            var blobClient = containerClient.GetBlobClient(attachmentName);
            return await blobClient.DeleteAsync();
        }

        public async Task<List<KeyValuePair<BlobItem,BlobDownloadResult>>> GetAllAsync(BlobStorageFileTypeEnum blobStorageFileTypeEnum, long organizationId, long productId, long attachmentId)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(blobStorageFileTypeEnum.ToString().ToLower());
            string folderPath = organizationId.ToString() + "/" + productId.ToString() + "/" + attachmentId.ToString();
            List<KeyValuePair<BlobItem,BlobDownloadResult>> pair = new();
            var iterator = containerClient.GetBlobsAsync(prefix: folderPath);
            await foreach(BlobItem blobItem in iterator)
            {
                BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);
                BlobDownloadResult file = await blobClient.DownloadContentAsync();
                pair.Add(new KeyValuePair<BlobItem,BlobDownloadResult>(blobItem,file));
            }
            return pair;
        }
    }
}
