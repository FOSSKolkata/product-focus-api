using System;

namespace ProductFocus.Domain.Common
{
    public abstract class Entity<TId>: IEquatable<Entity<TId>>
    {
        public TId Id { get; protected set; }

        public DateTime CreatedOn { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }

        protected Entity(TId id)
        {
            if(object.Equals(id, default(TId)))
            {
                throw new ArgumentException("The ID cannot be the type's default value.", nameof(id));
            }

            this.Id = id;
        }

        // EF require an empty constructor
        protected Entity()
        {

        }

        public override bool Equals(object otherObj)
        {
            var entity = otherObj as Entity<TId>;
            if(entity != null)
            {
                return this.Equals(entity);
            }

            return base.Equals(otherObj);
        }
        
        public bool Equals(Entity<TId> other)
        {
            if(other == null)
            {
                return false;
            }
           
            return this.Id.Equals(other.Id);
        }

        public static bool operator ==(Entity<TId> a, Entity<TId> b)
        {
            if (a is null && b is null)
                return true;
            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity<TId> a, Entity<TId> b)
        {
            return !(a == b);
        }


        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
