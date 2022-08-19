using MediatR;

namespace Euvic.Cqrs.PipelineBehaviors.Permissions
{
    public class PermissionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IPermission<TRequest>> _permissionRequirements;
        private readonly IUserPermissionsEvaluator _userPermissionsEvaluator;

        public PermissionBehavior(IEnumerable<IPermission<TRequest>> permissionRequiements, IUserPermissionsEvaluator userPermissionsEvaluator)
        {
            _permissionRequirements = permissionRequiements;
            _userPermissionsEvaluator = userPermissionsEvaluator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var userPermissions = await _userPermissionsEvaluator.GetPermissionsAsync();

            foreach (var permission in _permissionRequirements)
            {
                var isPermitted = await permission.IsPermittedAsync(request, userPermissions);

                if (!isPermitted)
                    throw new InsufficientPermissionsExceptions();
            }

            return await next();
        }
    }
}
