using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using ProductFocus.Domain;
using ProductFocus.Domain.Services;
using ProductFocusApi.Dtos;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocusApi.QueryHandlers
{
    public sealed class GetBusinessRequirementAttachmentQuery : IQuery<List<GetBusinessRequirementAttachmentDto>>
    {
        public long BusinessRequirementId { get; private set; }
        public GetBusinessRequirementAttachmentQuery(long businessReqirementId)
        {
            BusinessRequirementId = businessReqirementId;
        }
        public sealed class GetBusinessRequirementAttachmentQueryHandler : IQueryHandler<GetBusinessRequirementAttachmentQuery, List<GetBusinessRequirementAttachmentDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            private readonly IBlobStorageService _blobStorageService;

            public GetBusinessRequirementAttachmentQueryHandler(
                QueriesConnectionString queriesConnectionString,
                IBlobStorageService blobStorageService)
            {
                _queriesConnectionString = queriesConnectionString;
                _blobStorageService = blobStorageService;
            }
            public async Task<List<GetBusinessRequirementAttachmentDto>> Handle(GetBusinessRequirementAttachmentQuery query)
            {
                List<GetBusinessRequirementAttachmentDto> attachments = new();
                string sql = @"SELECT productId FROM BusinessRequirements WHERE Id = @BusinessRequirementId";
                string sql1 = @"SELECT organizationId FROM Products WHERE Id = @ProductId";
                string sql2 = @"SELECT id, name, uri FROM BusinessRequirementAttachments
                                WHERE businessRequirementId = @BusinessRequirementId";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
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

                    List<KeyValuePair<BlobItem,BlobDownloadResult>> blobItems = await _blobStorageService.GetAllAsync(ProductFocus.Domain.Services.BlobStorageFileTypeEnum.BusinessRequirementAttachments,
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
