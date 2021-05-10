using ProductFocus.Domain;
using ProductFocus.Dtos;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using ProductFocus.ConnectionString;
using ProductFocus.Services;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class GetKanbanViewQuery : IQuery<List<GetKanbanViewDto>>
    {
        public long Id { get; }
        public GetKanbanViewQuery(long id)
        {
            Id = id;
        }

        internal sealed class GetKanbanViewQueryHandler : IQueryHandler<GetKanbanViewQuery, List<GetKanbanViewDto>>
        {
            private readonly QueriesConnectionString _queriesConnectionString;
            private readonly IEmailService _emailService;

            public GetKanbanViewQueryHandler(QueriesConnectionString queriesConnectionString, IEmailService emailService)
            {
                _queriesConnectionString = queriesConnectionString;
                _emailService = emailService;
            }
            public async Task<List<GetKanbanViewDto>> Handle(GetKanbanViewQuery query)
            {
                List<GetKanbanViewDto> kanbanViewList = new List<GetKanbanViewDto>();
                string sql = @"
                    SELECT id, name 
                    from [product-focus].[dbo].[Modules]
                    WHERE productid = @PrdId
                    ;
                    SELECT id, moduleid, title 
                    from [product-focus].[dbo].[Features]
                    WHERE moduleid in (
                        select id from Modules where productid = @PrdId)";
                using (IDbConnection con = new SqlConnection(_queriesConnectionString.Value))
                {
                    var result = await con.QueryMultipleAsync(sql, new
                    {
                        PrdId = query.Id
                    });

                    
                    var kanbanViews = await result.ReadAsync<GetKanbanViewDto>();
                    var featureDetails = await result.ReadAsync<FeatureDetail>();

                    foreach (var kanbanView in kanbanViews)
                    {
                        kanbanView.FeatureDetails = featureDetails.Where(a => a.ModuleId == kanbanView.Id).ToList();
                    }

                    kanbanViewList = kanbanViews.ToList();
                }

                _emailService.send();
                
                return kanbanViewList;
            }
        }
    }
}
