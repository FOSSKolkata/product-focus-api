using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductDocumentations.Application.QueryHandlers
{
    public sealed class GetProductDocumentationsQuery : IRequest<List<GetProductDocumentationDto>>
    {
        public long ProductId { get; private set; }
        public GetProductDocumentationsQuery(long productId)
        {
            ProductId = productId;
        }
        internal sealed class GetProductDocumentationsQueryHandler : IRequestHandler<GetProductDocumentationsQuery, List<GetProductDocumentationDto>>
        {
            private readonly string _queriesConnectionString;
            public GetProductDocumentationsQueryHandler(IConfiguration configuration)
            {
                _queriesConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file"); ;
            }
            public async Task<List<GetProductDocumentationDto>> Handle(GetProductDocumentationsQuery request, CancellationToken cancellationToken)
            {
                List<GetProductDocumentationDto> productDocumentations = new();
                string sql = @"SELECT Id, Title, Description, ParentId, OrderNumber FROM productdocumentation.ProductDocumentations
                    WHERE productId = @ProductId AND IsDeleted = 'false'";
                using(IDbConnection con = new SqlConnection(_queriesConnectionString))
                {
                    var result = (await con.QueryAsync<ProductDocumentationsQueryResult>(sql, new
                    {
                        request.ProductId
                    })).ToList();
                    List<GetProductDocumentationDto> dtos = new();
                    foreach(var productDoc in result)
                    {
                        dtos.Add(productDoc.ToDto());
                    }
                    productDocumentations = GetProductDocumentationHierarchicalData(dtos);
                }
                return productDocumentations;
            }
            private List<GetProductDocumentationDto> GetProductDocumentationHierarchicalData(List<GetProductDocumentationDto> productDocList, long? parentId = null, string generatedIndex = "")
            {
                var records = productDocList.Where(x => x.ParentId == parentId).OrderBy(x => x.OrderNumber).ToList();
                if (records.Count == 0)
                    return new List<GetProductDocumentationDto>();
                int index = 1;
                foreach(var record in records)
                {
                    string currentIndex;
                    if (string.IsNullOrEmpty(generatedIndex))
                        currentIndex = $"{index}";
                    else
                        currentIndex = $"{generatedIndex}.{index}";
                    record.Index = currentIndex;
                    record.ChildDocumentations = GetProductDocumentationHierarchicalData(productDocList, record.Id, currentIndex);
                    index++;
                }
                return records;
            }
        }
    }
}
