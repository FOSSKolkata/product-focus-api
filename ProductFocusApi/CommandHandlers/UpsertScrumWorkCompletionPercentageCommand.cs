using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading.Tasks;


namespace ProductFocusApi.CommandHandlers
{
    public class UpsertScrumWorkCompletionPercentageCommand : ICommand
    {
            public long FeatureId { get; private set; }
            public DateTime ScrumDate { get; private set; }
            public int WorkCompletionPercentage { get; private set; }

            public UpsertScrumWorkCompletionPercentageCommand(long featureId, DateTime scrumDate, int workCompletionPercentage)
            {
                FeatureId = featureId;
                ScrumDate = scrumDate;
            WorkCompletionPercentage = workCompletionPercentage;
            }

            internal sealed class UpsertScrumCommentCommandHandler : ICommandHandler<UpsertScrumWorkCompletionPercentageCommand>
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

                public async Task<Result> Handle(UpsertScrumWorkCompletionPercentageCommand command)
                {
                    Feature feature = await _featureRepository.GetById(command.FeatureId);

                    if (feature == null) return Result.Failure("Invalid feature id");

                    if (command.ScrumDate.TimeOfDay.TotalSeconds != 0)
                        return Result.Failure("Invalid scrum date value");

                    Result result = feature.UpsertWorkCompletionPercentage(command.ScrumDate, command.WorkCompletionPercentage);

                    if (result.IsFailure)
                        return result;

                    await _unitOfWork.CompleteAsync();

                    return Result.Success();
                }
            }
        }
    }
