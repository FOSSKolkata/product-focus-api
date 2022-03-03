using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading.Tasks;
using MediatR;
using System.Threading;


namespace ProductFocusApi.CommandHandlers
{
    public class UpsertScrumWorkCompletionPercentageCommand : IRequest<Result>
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

            internal sealed class UpsertScrumCommentCommandHandler : IRequestHandler<UpsertScrumWorkCompletionPercentageCommand, Result>
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

                public async Task<Result> Handle(UpsertScrumWorkCompletionPercentageCommand request, CancellationToken cancellationToken)
                {
                    Feature feature = await _featureRepository.GetById(request.FeatureId);

                    if (feature == null) return Result.Failure("Invalid feature id");

                    if (request.ScrumDate.TimeOfDay.TotalSeconds != 0)
                        return Result.Failure("Invalid scrum date value");

                    Result result = feature.UpsertWorkCompletionPercentage(request.ScrumDate, request.WorkCompletionPercentage);

                    if (result.IsFailure)
                        return result;

                    await _unitOfWork.CompleteAsync(cancellationToken);

                    return Result.Success();
                }
            }
        }
    }
