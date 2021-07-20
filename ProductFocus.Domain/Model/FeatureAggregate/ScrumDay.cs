using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model.FeatureAggregate
{
    public class ScrumDay : Entity<long>
    {
        public DateTime ScrumDate { get; private set; }
        public int? WorkCompletionPercentage { get; private set; }
        public virtual string Comment { get; private set; }
        public virtual long FeatureId { get; private set; }
        public virtual Feature Feature { get; private set; }

        protected ScrumDay()
        {

        }

        public ScrumDay(DateTime scrumDate, string comment, Feature feature)
        {
            ScrumDate = scrumDate;
            Comment = comment;
            FeatureId = feature.Id;
            Feature = feature;
        }

        public ScrumDay(DateTime scrumDate, int workCompletionPercentage, Feature feature)
        {
            ScrumDate = scrumDate;
            WorkCompletionPercentage = workCompletionPercentage;
            Feature = feature;
        }

        public void UpdateComment(string comment)
        {
            this.Comment = comment;
        }

        internal void UpdateCompletionPercentage(int workCompletionPercentage)
        {
            this.WorkCompletionPercentage = workCompletionPercentage;
        }
    }
}
