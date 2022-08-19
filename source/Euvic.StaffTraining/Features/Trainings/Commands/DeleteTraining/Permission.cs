using System.Security.Principal;
using Euvic.Cqrs.PipelineBehaviors.Permissions;
using Euvic.StaffTraining.Common;
using Euvic.StaffTraining.Infrastructure;
using Euvic.StaffTraining.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Contract = Euvic.StaffTraining.Contracts.Trainings.Commands.DeleteTraining;

namespace Euvic.StaffTraining.Features.Trainings.Commands.DeleteTraining
{
    internal class Permission : IPermission<Contract.Command>
    {
        private readonly StaffTrainingContext _context;
        private readonly IPrincipal _principal;

        public Permission(StaffTrainingContext context, IPrincipal principal)
        {
            _context = context;
            _principal = principal;
        }

        public async Task<bool> IsPermittedAsync(Contract.Command request, HashSet<string> userPermissions)
        {
            var trainingLecturerId = await _context.Trainings.Where(x => x.Id == request.TrainingId).Select(x => x.LecturerId).FirstOrDefaultAsync();
            var userLecturerId = _principal.GetLecturerId();
            var canUserDeleteAnyTraining = userPermissions.Contains(Permissions.CanDeleteTraining);

            return trainingLecturerId == userLecturerId || canUserDeleteAnyTraining;
        }
    }
}
