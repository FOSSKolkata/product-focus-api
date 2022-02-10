using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public class UpsertScrumCommentCommand : ICommand
    {
        public long FeatureId { get; private set; }
        public DateTime ScrumDate { get; private set; }
        public string ScrumComment { get; private set; }

        public UpsertScrumCommentCommand(long featureId, DateTime scrumDate, string scrumComment)
        {
            FeatureId = featureId;
            ScrumDate = scrumDate;
            ScrumComment = scrumComment;
        }

        internal sealed class UpsertScrumCommentCommandHandler : ICommandHandler<UpsertScrumCommentCommand>
        {
            private readonly IFeatureRepository _featureRepository;
            private readonly IUnitOfWork _unitOfWork;
            public UpsertScrumCommentCommandHandler(
                IFeatureRepository featureRepository,
                IUnitOfWork unitOfWork
                )
            {
                _featureRepository = featureRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(UpsertScrumCommentCommand command)
            {
                Feature feature = await _featureRepository.GetById(command.FeatureId);

                if (feature == null) return Result.Failure("Invalid feature id");

                if (command.ScrumDate.TimeOfDay.TotalSeconds != 0)
                    return Result.Failure("Invalid scrum date value");

                Result result = feature.UpsertScrumComment(command.ScrumDate, command.ScrumComment);

                if (result.IsFailure)
                    return result;

                await _unitOfWork.CompleteAsync();

                return Result.Success();
            }
        }
    }
}
