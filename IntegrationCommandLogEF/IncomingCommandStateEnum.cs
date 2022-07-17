using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationCommandLogEF
{
    public enum  IncomingCommandStateEnum
    {
        ProcessingInProgress = 1,
        Processed = 2,
        ProcessingFailed = 3
    }
}
