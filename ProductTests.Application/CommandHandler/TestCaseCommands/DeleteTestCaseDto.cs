using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestCaseCommands
{
    public sealed class DeleteTestCaseDto
    {
        public long Id { get; set; }
        public long? TestPlanId { get; set; }
        public long? TestSuiteId { get; set; }
    }
}
