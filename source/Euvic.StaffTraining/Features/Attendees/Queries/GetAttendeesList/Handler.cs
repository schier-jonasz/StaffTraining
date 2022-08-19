using Euvic.Cqrs.Primitives;
using Euvic.StaffTraining.Domain;
using Euvic.StaffTraining.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Contract = Euvic.StaffTraining.Contracts.Attendees.Queries.GetAttendeesList;

namespace Euvic.StaffTraining.Features.Attendees.Queries.GetAttendees
{
    internal class Handler : IQueryHandler<Contract.Query, IEnumerable<Contract.AttendeesListItem>>
    {
        private readonly StaffTrainingContext _context;

        public Handler(StaffTrainingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contract.AttendeesListItem>> Handle(Contract.Query request, CancellationToken cancellationToken)
        {
            var attendees = await GetAttendeesWithProjection();

            return attendees;
        }

        // The best perfomance
        private async Task<IEnumerable<Contract.AttendeesListItem>> GetAttendeesWithProjection()
        {
            return await _context.Attendees
              .Include(x => x.Trainings)
                  .ThenInclude(x => x.Training)
              .Select(x =>
              new Contract.AttendeesListItem()
              {
                  Id = x.Id,
                  AllowedHours = x.AllowedHours,
                  Firstname = x.Firstname,
                  Lastname = x.Lastname,
                  TotalHours = (double)x.Trainings.Sum(t => t.Training.Duration) / 60,
                  TotalConfirmedHours = (double)x.Trainings
                      .Where(x => x.StatusId == (int)TrainingAttendeeStatuses.Confirmed)
                      .Sum(t => t.Training.Duration) / 60,
              })
              .OrderBy(x => x.Lastname)
              .ThenBy(x => x.Firstname)
              .AsNoTracking()
              .ToListAsync();
        }

        // Worst performance
        private async Task<IEnumerable<Contract.AttendeesListItem>> GetAttendeesWithoutProjection()
        {
            var attendees = await _context.Attendees
              .Include(x => x.Trainings)
                  .ThenInclude(x => x.Training)
              .ToListAsync();

            return attendees.Select(x =>
              new Contract.AttendeesListItem()
              {
                  Id = x.Id,
                  AllowedHours = x.AllowedHours,
                  Firstname = x.Firstname,
                  Lastname = x.Lastname,
                  TotalHours = (double)x.Trainings.Sum(t => t.Training.Duration) / 60,
                  TotalConfirmedHours = (double)x.Trainings
                      .Where(x => x.StatusId == (int)TrainingAttendeeStatuses.Confirmed)
                      .Sum(t => t.Training.Duration) / 60,
              })
              .OrderBy(x => x.Lastname)
              .ThenBy(x => x.Firstname)
              .ToList();
        }

        // Average performance
        private async Task<IEnumerable<Contract.AttendeesListItem>> GetAttendeesWithoutProjectionAsNoTracking()
        {
            var attendees = await _context.Attendees
              .AsNoTracking()
              .Include(x => x.Trainings)
                  .ThenInclude(x => x.Training)
              .ToListAsync();

            return attendees.Select(x =>
              new Contract.AttendeesListItem()
              {
                  Id = x.Id,
                  AllowedHours = x.AllowedHours,
                  Firstname = x.Firstname,
                  Lastname = x.Lastname,
                  TotalHours = (double)x.Trainings.Sum(t => t.Training.Duration) / 60,
                  TotalConfirmedHours = (double)x.Trainings
                      .Where(x => x.StatusId == (int)TrainingAttendeeStatuses.Confirmed)
                      .Sum(t => t.Training.Duration) / 60,
              })
              .OrderBy(x => x.Lastname)
              .ThenBy(x => x.Firstname)
              .ToList();
        }
    }
}
