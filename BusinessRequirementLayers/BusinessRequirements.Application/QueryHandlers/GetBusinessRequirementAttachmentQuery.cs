using Azure.Storage.Blobs.Models;
using BusinessRequirements.Domain.Services;
using BusinessRequirements.QueryHandlers.Dtos;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BusinessRequirements.QueryHandlers
{
    public sealed class GetBusinessRequirementAttachmentQuery : IRequest<List<GetBusinessRequirementAttachmentDto>>
    {
        public long BusinessRequirementId { get; private set; }
        public GetBusinessRequirementAttachmentQuery(long businessReqirementId)
        {
            BusinessRequirementId = businessReqirementId;
        }
        internal class GetBusinessRequirementAttachmentQueryHandler : IRequestHandler<GetBusinessRequirementAttachmentQuery, List<GetBusinessRequirementAttachmentDto>>
        {
            private readonly string _queriesConnectionString;
            private readonly IBlobStorageService _blobStorageService;

            public GetBusinessRequirementAttachmentQueryHandler(
                IConfiguration configuration,
                IBlobStorageService blobStorageService)
            {
                _queriesConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file");
                _blobStorageService = blobStorageService;
            }
            public async Task<List<GetBusinessRequirementAttachmentDto>> Handle(GetBusinessRequirementAttachmentQuery query, CancellationToken cancellationToken)
            {
                List<GetBusinessRequirementAttachmentDto> attachments = new();
                string sql = @"SELECT productId FROM BusinessRequirements WHERE Id = @BusinessRequirementId";
                string sql1 = @"SELECT organizationId FROM Products WHERE Id = @ProductId";
                string sql2 = @"SELECT id, name,fileName, uri FROM BusinessRequirementAttachments
                                WHERE businessRequirementId = @BusinessRequirementId";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString))
                {
                    long productId = (await con.QueryAsync<long>(sql, new 
                    {
                        query.BusinessRequirementId
                    })).First();

                    long organizationId = (await con.QueryAsync<long>(sql1, new
                    {
                        ProductId = productId
                    })).First();

                    var attachmentList = await con.QueryAsync<GetBusinessRequirementAttachmentDto>(sql2, new
                    {
                        query.BusinessRequirementId
                    });

                    List<KeyValuePair<BlobItem,BlobDownloadResult>> blobItems = await _blobStorageService.GetAllAsync(BlobStorageFileTypeEnum.BusinessRequirementAttachments,
                        organizationId, productId, query.BusinessRequirementId);

                    foreach(var attachment in attachmentList)
                    {
                        var item = blobItems.Where(blob => blob.Key.Name == attachment.Name).FirstOrDefault();
                        if (item.Key is null || item.Value is null)
                            continue;

                        byte[] byteArr = item.Value.Content.ToArray();
                        string mimeType = item.Value.Details.ContentType;
                        var attachedFile = new FileContentResult(byteArr, mimeType)
                        {
                            FileDownloadName = attachment.Name[(attachment.Name.LastIndexOf('/') + 1)..],
                            LastModified = item.Value.Details.LastModified
                        };
                        attachment.Contents = attachedFile.FileContents;
                        attachment.ContentType = attachedFile.ContentType;
                        attachment.Name = attachedFile.FileDownloadName;
                        attachment.LastModified = attachedFile.LastModified;
                        attachments.Add(attachment);
                    }
                    /*await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(prefix: folderPath))
                    {
                        BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);
                        BlobDownloadResult file = await blobClient.DownloadContentAsync();

                        byte[] byteArr = file.Content.ToArray();
                        string mimeType = file.Details.ContentType;
                        var attachment = new FileContentResult(byteArr, mimeType)
                        {
                            FileDownloadName = blobItem.Name[(blobItem.Name.LastIndexOf('/') + 1)..],
                            LastModified = file.Details.LastModified
                        };
                        attachments.Add(attachment);
                    }*/
                    attachments.Sort((a, b) => a.LastModified < b.LastModified ? -1 : 1);
                }
                return attachments;
            }
        }
    }
}
