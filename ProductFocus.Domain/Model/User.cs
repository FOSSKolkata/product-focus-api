using ProductFocus.Domain.Common;
using System;

namespace ProductFocus.Domain.Model
{
    public class User : AggregateRoot<long>
    {
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }

        public virtual string ObjectId { get; set; }
        protected User()
        {

        }

        private User(string name, string email, string objectid)
        {
            Name = name;
            Email = email;
            ObjectId = objectid;
        }

        public static User CreateInstance (string name, string email, string objectid)
        {
            if (String.IsNullOrEmpty(name))
                throw new Exception("Name cannot be null or empty");
            
            if (String.IsNullOrWhiteSpace(email))
                throw new Exception("Not a valid email address");

            var user = new User(name, email, objectid);
            return user;
        }
    }
}
