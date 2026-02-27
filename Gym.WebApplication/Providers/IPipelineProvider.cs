using Gym.WebApplication.Features.Admin.Shared.Services;
using Gym.WebApplication.ViewModels;
using Polly;
using Polly.Registry;

namespace Gym.WebApplication.Providers
{
    public interface IPipelineProvider
    {
        ResiliencePipeline<InstructorViewModel?> InstructorEventualConsistency { get; }
        ResiliencePipeline<TrainingViewModel?> TrainingEventualConsistency { get; }
    }

    public class PipelineProvider(ResiliencePipelineProvider<String> _resiliencePipelineProvider) : IPipelineProvider
    {
        public ResiliencePipeline<InstructorViewModel?> InstructorEventualConsistency 
            => _resiliencePipelineProvider.GetPipeline<InstructorViewModel?>(nameof(GetInstructorByIdService));

        public ResiliencePipeline<TrainingViewModel?> TrainingEventualConsistency 
            => _resiliencePipelineProvider.GetPipeline<TrainingViewModel?>(nameof(GetTrainingByIdService));
    }
}
