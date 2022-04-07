using ProductTests.Domain.Common;
using System;

namespace ProductTests.Domain.Model.TestCaseAggregate
{
    public class TestStep : Entity<long>, ISoftDeletable
    {
        public virtual long TestCaseId { get; private set; }
        public virtual TestCase TestCase { get; private set; }
        public virtual long StepNo { get; private set; }
        public virtual string Action { get; private set; }
        public virtual string ExpectedResult { get; private set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime DeletedOn { get; set; }
        public virtual string DeletedBy { get; set; }
        protected TestStep()
        {

        }

        private TestStep(long stepNo, string action, string expectedResult)
        {
            StepNo = stepNo;
            Action = action;
            ExpectedResult = expectedResult;
        }

        private TestStep(long id, long stepNo, string action, string expectedResult)
        {
            Id = id;
            StepNo = stepNo;
            Action = action;
            ExpectedResult = expectedResult;
        }
        internal void Delete(string userId)
        {
            DeletedBy = userId;
            DeletedOn = DateTime.Now;
            IsDeleted = true;
        }
        internal void Update(long stepNo, string action, string expectedResult)
        {
            StepNo = stepNo;
            Action = action;
            ExpectedResult = expectedResult;
        }
        public static TestStep CreateInstance(long stepNo, string action, string expectedResult)
        {
            TestStep testStep = new(stepNo, action, expectedResult);
            return testStep;
        }
        public static TestStep CreateInstance(long id, long stepNo, string action, string expectedResult)
        {
            TestStep testStep = new(id, stepNo, action, expectedResult);
            return testStep;
        }
    }
}
