using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestSuiteCommands
{
    public sealed class AddTestSuiteDto
    {
        public long TestPlanId { get; set; }
        public string Title { get; set; }
    }
}
