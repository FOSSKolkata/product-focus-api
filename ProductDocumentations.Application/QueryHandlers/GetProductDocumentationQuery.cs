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
    public sealed class GetProductDocumentationQuery : IRequest<List<GetProductDocumentationDetailsDto>>
    {
        public long Id { get; private set; }
        public int Index { get; private set; }
        public GetProductDocumentationQuery(long id, int index)
        {
            Id = id;
            Index = index;
        }
        internal sealed class GetProductDocumentationQueryHandler : IRequestHandler<GetProductDocumentationQuery, List<GetProductDocumentationDetailsDto>>
        {
            private readonly string _queriesConnectionString;
            public GetProductDocumentationQueryHandler(IConfiguration configuration)
            {
                _queriesConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file");
            }
            public async Task<List<GetProductDocumentationDetailsDto>> Handle(GetProductDocumentationQuery request, CancellationToken cancellationToken)
            {
                List<GetProductDocumentationDetailsDto> productDocumentations = new();
                string sql = @"SELECT id, title, description, parentId, orderNumber FROM productdocumentation.ProductDocumentations
                    WHERE id = @Id;

                    WITH CTE AS (
                        SELECT id, title, description, parentId, orderNumber FROM productdocumentation.ProductDocumentations
                        WHERE ParentId = @Id
		
	                    UNION ALL

                        SELECT t.id, t.title, t.description, t.ParentId, t.orderNumber
                        FROM productdocumentation.ProductDocumentations t
                        INNER JOIN CTE c ON t.ParentId = c.id
                    )
                    SELECT * FROM CTE ORDER BY orderNumber;";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString))
                {
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        request.Id
                    });
                    var currentDocumentation = (await result.ReadAsync<ProductDocumentationsQueryResult>()).FirstOrDefault();
                    var childDocumentations = (await result.ReadAsync<ProductDocumentationsQueryResult>()).ToList();
                    productDocumentations.Add(currentDocumentation.ToDetailsDto(1, request.Index.ToString())); // Level 1
                    ProductDocumentationLevelGenerator(childDocumentations, productDocumentations, 1, currentDocumentation.Id, request.Index.ToString());
                }
                return productDocumentations;
            }
            private  void ProductDocumentationLevelGenerator(List<ProductDocumentationsQueryResult> queryList, List<GetProductDocumentationDetailsDto> dtos, int level, long parentId, string generatedIndex)
            {
                var records = queryList.Where(x => x.ParentId == parentId).ToList();
                int index = 1;
                foreach(var record in records)
                {
                    dtos.Add(record.ToDetailsDto(level + 1, $"{generatedIndex}.{index}"));
                    ProductDocumentationLevelGenerator(queryList, dtos, level + 1, record.Id, $"{generatedIndex}.{index}");
                    index++;
                }
            }
        }
    }
}
