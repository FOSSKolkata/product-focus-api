using ProductFocus.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductFocus.Domain.Model
{
    [Table("UserToFeatureAssignments")]
    public class UserToFeatureAssignment : AggregateRoot<long>
    {
        public virtual Feature Feature { get; set; }
        public virtual User User { get; set; }
        
        protected UserToFeatureAssignment()
        {

        }

        private UserToFeatureAssignment(Feature feature, User user)
        {
            Feature = feature;
            User = user;
        }

        public static UserToFeatureAssignment CreateInstance (Feature feature, User user)
        {            
            var newInstance = new UserToFeatureAssignment(feature, user);
            return newInstance;
        }
    }
}
