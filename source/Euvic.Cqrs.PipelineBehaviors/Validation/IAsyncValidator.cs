using FluentValidation.Results;

namespace Euvic.Cqrs.PipelineBehaviors.Validation
{
    public interface IAsyncValidator<in TRequest>
    {
        public Task<ValidationResult> ValidateAsync(TRequest request);
    }
}
