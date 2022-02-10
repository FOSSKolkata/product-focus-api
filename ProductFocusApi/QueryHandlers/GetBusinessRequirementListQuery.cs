using Dapper;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using ProductFocus.Domain.Common;
using ProductFocusApi.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocusApi.QueryHandlers
{
    public sealed class GetBusinessRequirementListQuery : IQuery<GetBusinessRequirementsDto>
    {
        public long ProductId { get; set; }
        public List<long> TagIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long Offset { get; set; }
        public long Count { get; set; }

        public GetBusinessRequirementListQuery(long productId, List<long> tagIds, DateTime? startDate, DateTime? endDate, long offset, long count)
        {
            ProductId = productId;
            TagIds = tagIds;
            StartDate = startDate;
            EndDate = endDate;
            Offset = offset;
            Count = count;
        }
        internal sealed class GetBusinessRequirementListQueryHandler : IQueryHandler<GetBusinessRequirementListQuery, GetBusinessRequirementsDto>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            public GetBusinessRequirementListQueryHandler(QueriesConnectionString queriesConnectionString)
            {
                _queriesConnectionString = queriesConnectionString;
            }
            public async Task<GetBusinessRequirementsDto> Handle(GetBusinessRequirementListQuery query)
            {
                GetBusinessRequirementsDto getBusinessRequirements = new();
                var builder = new SqlBuilder();
                var selector = builder.AddTemplate("SELECT br.id, br.title, br.receivedOn, br.productId, brt.TagId FROM BusinessRequirements br/**innerjoin**/ /**where**/ /**orderby**/");
                builder.InnerJoin("BusinessRequirementTags brt ON br.Id = brt.BusinessRequirementId");
                builder.Where("br.ProductId = @ProductId AND br.IsDeleted = 'false'");
                if (query.TagIds != null && query.TagIds.Count > 0)
                    builder.Where("brt.TagId IN @TagIds");
                if (query.StartDate != null)
                    builder.Where("br.ReceivedOn >= @StartDate");
                if (query.EndDate != null)
                    builder.Where("br.ReceivedOn <= @EndDate");
                builder.OrderBy("br.Id desc offset @Offset rows fetch next @Count rows only");
                string sql = selector.RawSql;

                string sql1 = @"SELECT t.Id, t.Name, brt.BusinessRequirementId FROM BusinessRequirementTags brt
                                INNER JOIN Tags t ON brt.TagId = t.Id WHERE t.ProductId = @ProductId";

                var builderCount = new SqlBuilder();
                var selectorCount = builderCount.AddTemplate("SELECT COUNT(*) FROM BusinessRequirements br/**innerjoin**/ /**where**/ /**orderby**/");
                builderCount.InnerJoin("BusinessRequirementTags brt ON br.Id = brt.BusinessRequirementId");
                builderCount.Where("br.ProductId = @ProductId AND br.IsDeleted = 'false'");
                if (query.TagIds != null && query.TagIds.Count > 0)
                    builderCount.Where("brt.TagId IN @TagIds");
                if (query.StartDate != null)
                    builderCount.Where("br.ReceivedOn >= @StartDate");
                if (query.EndDate != null)
                    builderCount.Where("br.ReceivedOn <= @EndDate");

                string sqlCount = selectorCount.RawSql;

                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    List<GetBusinessRequirementDto> businessRequirementList = new();
                    businessRequirementList = (await con.QueryAsync<GetBusinessRequirementDto>(sql, new
                    {
                        query.ProductId,
                        query.TagIds,
                        query.StartDate,
                        query.EndDate,
                        query.Offset,
                        query.Count
                    })).ToList();

                    getBusinessRequirements.RecordCount = (await con.QueryAsync<long>(sqlCount, new
                    {
                        query.ProductId,
                        query.TagIds,
                        query.StartDate,
                        query.EndDate
                    })).First();

                    var tags = (await con.QueryAsync<BusinessRequirementTagDto>(sql1, new
                    {
                        query.ProductId,
                        query.TagIds
                    })).ToList();

                    foreach(var businessRequirement in businessRequirementList)
                    {
                        businessRequirement.Tags = tags.Where(tag => tag.BusinessRequirementId == businessRequirement.Id).ToList();
                    }
                    getBusinessRequirements.BusinessRequirements = businessRequirementList;
                }
                return getBusinessRequirements;
            }
        }
    }
}
