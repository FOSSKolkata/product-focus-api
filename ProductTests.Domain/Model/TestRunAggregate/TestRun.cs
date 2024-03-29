﻿using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestPlanAggregate;
using System;

namespace ProductTests.Domain.Model.TestRunAggregate
{
    public class TestRun : AggregateRoot<long>
    {
        public virtual long ProductId { get; private set; }
        public virtual long? SprintId { get; private set; }
        public virtual long TestPlanId { get; private set; }
        public virtual Status RunningStatus { get; set; }
        public virtual long TestPlanVersionId { get; set; }
        public virtual TestPlanVersion TestPlanVersion { get; set; }
        protected TestRun()
        {

        }
        public void ChangeRunningStatus(Status runningStatus)
        {
            RunningStatus = runningStatus;
        }
        private TestRun(TestPlan testPlan, string userId)
        {
            ProductId = testPlan.ProductId;
            SprintId = testPlan.SprintId;
            TestPlanId = testPlan.Id;
            TestPlanVersion = TestPlanVersion.CreateInstance(testPlan);
            TestPlanVersionId = TestPlanVersion.Id;
            CreatedOn = DateTime.UtcNow;
            CreatedBy = userId;
        }
        public void IncludeTestCase(long testCaseId, bool isSelected)
        {
            TestPlanVersion.IncludeTestCase(testCaseId, isSelected);
        }
        public static TestRun CreateInstance(TestPlan testPlan, string userId)
        {
            return new TestRun(testPlan, userId);
        }

        public void UpdateResultStatus(long id, TestCaseResult resultStatus)
        {
            TestPlanVersion.UpdateResultStatus(id, resultStatus);
            RunningStatus = TestPlanVersion.IsTestPlanExecutionCompleted() ? Status.Completed : Status.Incompleted;
        }

        public void UpdateTestStep(long id, TestStepResult resultStatus)
        {
            TestPlanVersion.UpdateTestStep(id, resultStatus);
        }
    }
    public enum Status
    {
        Completed = 1,
        Incompleted = 2
    }
}
