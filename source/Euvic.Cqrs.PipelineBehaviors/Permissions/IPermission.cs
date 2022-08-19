namespace Euvic.Cqrs.PipelineBehaviors.Permissions
{
    public interface IPermission<in TRequest>
    {
        Task<bool> IsPermittedAsync(TRequest request, HashSet<string> userPermissions);
    }
}
