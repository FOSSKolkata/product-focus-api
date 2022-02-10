using ProductFocus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductFocus.Domain.Model
{
    public class Organization : AggregateRoot<long>
    {
        public virtual string Name { get; private set; }
        
        private readonly IList<Member> _members = new List<Member>();
        public virtual IReadOnlyList<Member> Members => _members.ToList();

        private readonly IList<Product> _products = new List<Product>();
        public virtual IReadOnlyList<Product> Products => _products.ToList();

        protected Organization()
        {

        }
        private Organization(string name)
        {
            Name = name;
        }

        public static Organization CreateInstance(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new Exception("Organization name can't be null or empty");

            var organization = new Organization(name);
            return organization;
        }

        public virtual void AddMember(User user, bool isOwner)
        {
            var member = Member.CreateInstance(this, user, isOwner);
            _members.Add(member);
        }

        public virtual bool IfProductExists (string name)
        {
            var existingPoductWithSameName = Products.FirstOrDefault(x => x.Name == name);

            if (existingPoductWithSameName != null)
                return true;
            else
                return false;            
        }

        public virtual bool IfAlreadyMember (User user)
        {
            var existingMemberWithSameUser = Members.FirstOrDefault(x => x.User == user);

            if (existingMemberWithSameUser != null)
                return true;
            else
                return false;
        }
    }
}
