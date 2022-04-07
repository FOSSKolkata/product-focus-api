using ProductTests.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTests.Domain.Model.TestCaseAggregate
{
    public class TestCase : AggregateRoot<long>, ISoftDeletable
    {
        public virtual string Title { get; private set; }
        public virtual string Preconditions { get; private set; }

        private readonly IList<TestStep> _testSteps = new List<TestStep>();
        public virtual IReadOnlyList<TestStep> TestSteps => _testSteps.ToList();

        public virtual bool IsDeleted { get; set; }
        public virtual DateTime DeletedOn { get; set; }
        public virtual string DeletedBy { get; set; }

        protected TestCase()
        {

        }

        private TestCase(string title, string preconditions, List<TestStep> testSteps)
        {
            Title = title;
            Preconditions = preconditions;
            _testSteps = testSteps;
        }

        public static TestCase CreateInstance(string title, string precondition, List<TestStep> testSteps)
        {
            TestCase testCase = new(title, precondition, testSteps);
            return testCase;
        }
        public void UpdateTitle(string title)
        {
            Title = title;
        }
        public void UpdatePreconditions(string preconditions)
        {
            Preconditions = preconditions;
        }
        public void AddTestStep(long stepNo, string action, string expectedResult)
        {
            TestStep testStep = TestStep.CreateInstance(stepNo, action, expectedResult);
            _testSteps.Add(testStep);
        }
        public void UpdateTestStep(long id, long stepNo, string action, string expectedResult)
        {
            TestStep testStep = TestSteps.Where(x => x.Id == id).SingleOrDefault();
            testStep.Update(stepNo, action, expectedResult);
        }
        public void UpdateTestSteps(List<TestStep> newTestSteps, string userId)
        {
            var deletedTestSteps = TestSteps.Where(x => !newTestSteps.Select(y => y.Id).Contains(x.Id) && !x.IsDeleted).ToList();
            var addedTestSteps = newTestSteps.Where(x => x.Id == 0).ToList();
            foreach (TestStep deletedTestStep in deletedTestSteps)
            {
                // _testSteps.Remove(deletedTestStep); // Permanently delete
                deletedTestStep.Delete(userId); // Soft Delete

            }
            foreach(TestStep oldTestStep in TestSteps)
            {
                TestStep updatedTestStep = newTestSteps.Where(x => x.Id == oldTestStep.Id).SingleOrDefault();
                if(updatedTestStep != null)
                    oldTestStep.Update(updatedTestStep.StepNo, updatedTestStep.Action, updatedTestStep.ExpectedResult);
            }
            foreach(TestStep addedTestStep in addedTestSteps)
            {
                AddTestStep(addedTestStep.StepNo, addedTestStep.Action, addedTestStep.ExpectedResult);
            }
        }

        public void Delete(string userId)
        {
            DeletedBy = userId;
            DeletedOn = DateTime.Now;
            IsDeleted = true;
        }
    }
}
