using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductTests.Application.CommandHandler.TestRunCommands
{
    public sealed class MarkTestCaseVersionDto
    {
        public long Id { get; set; }
        public bool IsSelected { get; set; }
    }
}
