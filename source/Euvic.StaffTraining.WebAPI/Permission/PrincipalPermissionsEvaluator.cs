using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Euvic.Cqrs.PipelineBehaviors.Permissions;
using Euvic.StaffTraining.Common;

namespace Euvic.StaffTraining.WebAPI.Permission
{
    public class PrincipalPermissionsEvaluator : IUserPermissionsEvaluator
    {
        private readonly IPrincipal _principal;

        public PrincipalPermissionsEvaluator(IPrincipal principal)
        {
            _principal = principal;
        }

        public Task<HashSet<string>> GetPermissionsAsync()
        {
            return Task.FromResult(_principal.GetPermissions()?.ToHashSet() ?? new HashSet<string>());
        }
    }
}
