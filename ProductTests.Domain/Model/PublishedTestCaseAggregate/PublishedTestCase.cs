using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestPlanAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTests.Domain.Model.PublishedTestCaseAggregate
{
    public class PublishedTestCase : AggregateRoot<long>, ISoftDeletable
    {
        public virtual long TestSuiteId { get; private set; }
        public virtual TestSuite TestSuite { get; private set; }
        public virtual string Title { get; private set; }
        public virtual string Preconditions { get; private set; }

        private readonly IList<PublishedTestStep> _testSteps = new List<PublishedTestStep>();
        public virtual IReadOnlyList<PublishedTestStep> TestSteps => _testSteps.ToList();

        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        protected PublishedTestCase()
        {

        }
        private PublishedTestCase(long suiteId, string title, string preconditions, List<PublishedTestStep> testSteps)
        {
            Title = title;
            TestSuiteId = suiteId;
            Preconditions = preconditions;
            _testSteps = testSteps;
            CreatedOn = DateTime.Now;
        }
        public static PublishedTestCase CreateInstance(long suiteId, string title, string preconditions, List<PublishedTestStep> testSteps)
        {
            return new PublishedTestCase(suiteId, title, preconditions, testSteps);
        }
    }
}
