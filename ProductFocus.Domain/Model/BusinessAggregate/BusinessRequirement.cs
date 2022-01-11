using Common;
using CSharpFunctionalExtensions;
using ProductFocus.Domain.Model.BusinessAggregate;
using System;
using System.Collections.Generic;

namespace ProductFocus.Domain.Model
{
    public class BusinessRequirement : AggregateRoot<long>
    {
        public virtual Product Product { get; private set; }
        public virtual long ProductId { get; private set; }
        public virtual string Title { get; private set; }
        public virtual DateTime ReceivedOn { get; private set; }
        public virtual BusinessRequirementSourceEnum SourceEnum { get; private set; }
        public virtual string SourceInformation { get; private set; }
        public virtual string Description { get; private set; }
        protected BusinessRequirement()
        {
            // this protected constructor is for lazy loading to work
        }
        private BusinessRequirement(string title, DateTime receivedOn,
            BusinessRequirementSourceEnum sourceEnum, string sourceInformation,
            string description, Product product)
        {
            Title = title;
            ReceivedOn = receivedOn;
            SourceEnum = sourceEnum;
            SourceInformation = sourceInformation;
            Description = description;
            ProductId = product.Id;
            Product = product;
        }
        public static Result<BusinessRequirement> CreateInstance(string title, Product product,
            BusinessRequirementSourceEnum sourceEnum, string sourceInformation, string description, DateTime receivedOn)
        {
            BusinessRequirement businessRequirement = new(title, receivedOn, sourceEnum, sourceInformation, description, product);
            return businessRequirement;
        }
    }

    public enum BusinessRequirementSourceEnum
    {
        Email = 1,
        Meeting = 2
    }
}
