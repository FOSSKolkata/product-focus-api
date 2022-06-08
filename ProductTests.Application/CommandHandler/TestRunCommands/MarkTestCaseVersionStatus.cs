using ProductTests.Domain.Model.TestRunAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestRunCommands
{
    public sealed class MarkTestCaseVersionStatus
    {
        public long Id { get; set; }
        public TestCaseResult ResultStatus { get; set; }
    }
}
