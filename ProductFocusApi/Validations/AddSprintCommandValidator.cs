using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using ProductFocusApi.CommandHandlers;

namespace ProductFocusApi.Validations
{
    public class AddSprintCommandValidator : AbstractValidator<AddSprintCommand>
    {
        public AddSprintCommandValidator(ILogger<AddSprintCommandValidator> logger)
        {
            RuleFor(sprint => sprint.Name).NotEmpty().WithMessage("Sprint name cannot be empty");
            RuleFor(sprint => sprint.ProductId).NotEmpty();
            RuleFor(sprint => sprint.StartDate).NotEmpty();
            RuleFor(sprint => sprint.EndDate).NotEmpty();



            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
