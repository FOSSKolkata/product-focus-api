using ProductTests.Domain.Model.TestPlanAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductTests.Application.QueryHandler.GetTestRunQueries
{
    public class GetTestRunsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public TestTypeEnum TestType { get; set; }
        public long TotalTestSuites { get; set; }
        public long TotalTestCases { get; set; }
        public long TotalPassedCases { get; set; }
        public long TotalFailedCases { get; set; }

    }
}
