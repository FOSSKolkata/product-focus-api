using Common;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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

        private Invitation(Organization organization, string email)
        {
            Email = email;
            InvitedOn = DateTime.UtcNow;
            Status = InvitationStatus.New;
            Organization = organization;
        }

        public static Invitation CreateInstance(Organization organization, string email)
        {
            var invitation = new Invitation(organization, email);
            return invitation;
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
