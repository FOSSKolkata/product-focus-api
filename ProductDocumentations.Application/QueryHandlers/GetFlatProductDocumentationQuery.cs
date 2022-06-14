using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductDocumentations.Application.QueryHandlers
{
    public sealed class GetFlatProductDocumentationQuery : IRequest<List<GetFlatProductDocumentationDto>>
    {
        public long ProductId { get; private set; }
        public GetFlatProductDocumentationQuery(long productId)
        {
            ProductId = productId;
        }
        internal sealed class GetFlatProductDocumentationQueryHandler : IRequestHandler<GetFlatProductDocumentationQuery, List<GetFlatProductDocumentationDto>>
        {
            private readonly string _queriesConnectionString;
            public GetFlatProductDocumentationQueryHandler(IConfiguration configuration)
            {
                _queriesConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file");
            }
            public async Task<List<GetFlatProductDocumentationDto>> Handle(GetFlatProductDocumentationQuery request, CancellationToken cancellationToken)
            {
                List<GetFlatProductDocumentationDto> productDocumentations;

                string sql = @"SELECT Id, ParentId, Title, Description FROM [ProductDocumentation].[ProductDocumentations] WHERE ProductId = @ProductId";
                
                using(IDbConnection con = new SqlConnection(_queriesConnectionString))
                {
                    productDocumentations = (await con.QueryAsync<GetFlatProductDocumentationDto>(sql, new
                    {
                        request.ProductId
                    })).ToList();
                }
                return productDocumentations;
            }
        }
    }
}
