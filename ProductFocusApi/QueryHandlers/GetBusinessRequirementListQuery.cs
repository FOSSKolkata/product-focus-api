using Dapper;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using ProductFocus.Domain;
using ProductFocusApi.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocusApi.QueryHandlers
{
    public sealed class GetBusinessRequirementListQuery : IQuery<List<GetBusinessRequirementDto>>
    {
        public long ProductId { get; set; }
        public List<long> TagIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public GetBusinessRequirementListQuery(long productId, List<long> tagIds, DateTime? startDate, DateTime? endDate)
        {
            ProductId = productId;
            TagIds = tagIds;
            StartDate = startDate;
            EndDate = endDate;
        }
        internal sealed class GetBusinessRequirementListQueryHandler : IQueryHandler<GetBusinessRequirementListQuery, List<GetBusinessRequirementDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            public GetBusinessRequirementListQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<List<GetBusinessRequirementDto>> Handle(GetBusinessRequirementListQuery query)
            {
                List<GetBusinessRequirementDto> businessRequirements = new();
                var builder = new SqlBuilder();
                var selector = builder.AddTemplate("SELECT br.id, br.title, br.receivedOn, br.productId, brt.TagId FROM BusinessRequirements br/**innerjoin**/ /**where**/ /**orderby**/");
                builder.InnerJoin("BusinessRequirementTags brt ON br.Id = brt.BusinessRequirementId");
                builder.Where("br.ProductId = @ProductId");
                if (query.TagIds != null && query.TagIds.Count > 0)
                    builder.Where("brt.TagId IN @TagIds");
                if (query.StartDate != null)
                    builder.Where("br.ReceivedOn >= @StartDate");
                if (query.EndDate != null)
                    builder.Where("br.ReceivedOn <= @EndDate");
                builder.OrderBy("br.Id");
                string sql = selector.RawSql;

                string sql1 = @"SELECT t.Id, t.Name, brt.BusinessRequirementId FROM BusinessRequirementTags brt
                                INNER JOIN Tags t ON brt.TagId = t.Id WHERE t.ProductId = @ProductId";

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    businessRequirements = (await con.QueryAsync<GetBusinessRequirementDto>(sql, new
                    {
                        query.ProductId,
                        query.TagIds,
                        query.StartDate,
                        query.EndDate
                    })).ToList();

                    var tags = (await con.QueryAsync<BusinessRequirementTagDto>(sql1, new
                    {
                        query.ProductId,
                        query.TagIds
                    })).ToList();

                    foreach(var businessRequirement in businessRequirements)
                    {
                        businessRequirement.Tags = tags.Where(tag => tag.BusinessRequirementId == businessRequirement.Id).ToList();
                    }
                }
                return businessRequirements;
            }
        }
    }
}
