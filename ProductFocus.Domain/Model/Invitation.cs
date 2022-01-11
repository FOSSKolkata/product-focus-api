using Common;
using System;

namespace ProductFocus.Domain.Model
{    
    public class Invitation : AggregateRoot<long>
    {        
        public virtual string Email { get; set; }
        public virtual InvitationStatus Status { get; set; }
        public virtual DateTime InvitedOn { get; set; }
        public virtual DateTime LastResentOn { get; set; }
        public virtual DateTime ActionedOn { get; set; }
        public virtual Organization Organization { get; set; }

        protected Invitation()
        {
            // this protected constructor is for lazy loading to work
        }

        private Invitation(Organization organization, string email, long createdById)
        {
            Email = email;
            InvitedOn = DateTime.UtcNow;
            Status = InvitationStatus.New;
            Organization = organization;
            CreatedById = createdById;
        }

        public static Invitation CreateInstance(Organization organization, string email, long createdById)
        {
            var invitation = new Invitation(organization, email, createdById);
            return invitation;
        }

        public virtual void UpdateInvitationAsAccepted()
        {
            Status = InvitationStatus.Accepted;
            ActionedOn = DateTime.UtcNow;
        }
        public virtual void UpdateInvitationAsRejected()
        {
            Status = InvitationStatus.Rejected;
            ActionedOn = DateTime.UtcNow;
        }
        public virtual void UpdateInvitationAsCancelled()
        {
            Status = InvitationStatus.Cancelled;
            ActionedOn = DateTime.UtcNow;
        }
    }

    public enum InvitationStatus
    {
        New = 1,
        Cancelled = 2,
        Rejected = 3,
        Accepted = 4,
        Resent = 5
    }
}
