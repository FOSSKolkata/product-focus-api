using ProductTests.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductTests.Domain.Model.PublishedTestCaseAggregate
{
    public class PublishedTestStep : AggregateRoot<long>
    {
        public virtual long PublishedTestCaseId { get; private set; }
        public virtual PublishedTestCase TestCase { get; private set; }
        public virtual long StepNo { get; private set; }
        public virtual string Action { get; private set; }
        public virtual string ExpectedResult { get; private set; }

        protected PublishedTestStep()
        {

        }
        private PublishedTestStep(long stepNo, string action, string expectedResult)
        {
            StepNo = stepNo;
            Action = action;
            ExpectedResult = expectedResult;
        }
        public static PublishedTestStep CreateInstance(long stepNo, string action, string expectedResult)
        {
            return new PublishedTestStep(stepNo, action, expectedResult);
        }
    }
}
