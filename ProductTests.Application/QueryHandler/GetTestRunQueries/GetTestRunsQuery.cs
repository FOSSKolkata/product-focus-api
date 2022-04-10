using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Application.QueryHandler.GetTestRunQueries
{
    public class GetTestRunsQuery : IRequest<List<GetTestRunsDto>>
    {
        public long ProductId { get; private set; }
        public GetTestRunsQuery(long productId)
        {
            ProductId = productId;
        }
        internal sealed class GetTestRunsQueryHandler : IRequestHandler<GetTestRunsQuery, List<GetTestRunsDto>>
        {
            private readonly string _queriesConnectionString;
            public GetTestRunsQueryHandler(IConfiguration configuration)
            {
                _queriesConnectionString = configuration["ConnectionStrings:QueriesConnectionString"] ?? throw new NullReferenceException("Connection String is missing from config file");
            }

            public Task<List<GetTestRunsDto>> Handle(GetTestRunsQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
