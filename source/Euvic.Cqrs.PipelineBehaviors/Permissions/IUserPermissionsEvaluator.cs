namespace Euvic.Cqrs.PipelineBehaviors.Permissions
{
    public interface IUserPermissionsEvaluator
    {
        Task<HashSet<string>> GetPermissionsAsync();
    }
}
