using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    public class Organization : AggregateRoot<long>
    {
        public virtual string Name { get; set; }
        
        private readonly IList<Member> _members = new List<Member>();
        public virtual IReadOnlyList<Member> Members => _members.ToList();

        private readonly IList<Product> _products = new List<Product>();
        public virtual IReadOnlyList<Product> Products => _products.ToList();
        protected Organization()
        {
            
        }
        public Organization(string name) : this()
        {
            Name = name;
        }
        public virtual void AddMember(User user, bool isOwner)
        {
            var member = new Member(this, user, isOwner);
            _members.Add(member);
        }
        public virtual void AddProduct(string name)
        {            
            var product = new Product(this, name);
            _products.Add(product);
        }
    }
}
