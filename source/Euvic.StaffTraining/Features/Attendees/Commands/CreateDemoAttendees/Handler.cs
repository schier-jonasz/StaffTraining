using Bogus;
using EFCore.BulkExtensions;
using Euvic.Cqrs.Primitives;
using Euvic.StaffTraining.Domain;
using Euvic.StaffTraining.Identity.Abstractions;
using Euvic.StaffTraining.Infrastructure.EntityFramework;
using MediatR;
using static Euvic.StaffTraining.Contracts.Attendees.Commands.CreateDemoAttendees;
using Contract = Euvic.StaffTraining.Contracts.Attendees.Commands.CreateDemoAttendees;

namespace Euvic.StaffTraining.Features.Attendees.Commands.CreateDemoAttendees
{
    internal class Handler : ICommandHandler<Contract.Command>
    {
        private readonly StaffTrainingContext _context;
        private readonly IIdentityProvider _identityProvider;

        public Handler(IIdentityProvider identityProvider, StaffTrainingContext context)
        {
            _identityProvider = identityProvider;
            _context = context;
        }

        public async Task<Unit> Handle(Contract.Command request, CancellationToken cancellationToken)
        {
            var existingEmails = _context.Attendees.Select(x => GetEmail(x.Firstname, x.Lastname)).ToHashSet();
            var attendeesWithIdentity = GenerateAttenddees(request.AttendeesCount, existingEmails);
            var attendees = attendeesWithIdentity
                .Select(x => new Attendee()
                {
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    AllowedHours = 8
                }).ToList();

            //Bulk insert
            await _context.BulkInsertAsync<Attendee>(attendees, b => b.SetOutputIdentity = true);

            var attendeesEmailMapping = attendees.ToDictionary(key => GetEmail(key.Firstname, key.Lastname), value => value.Id);


            // ==================================================> Explain why not TaskWhenAll() <=================================================
            await Parallel.ForEachAsync(attendeesWithIdentity, new ParallelOptions()
            {
                MaxDegreeOfParallelism = 20
            },
            async (attendee, _) =>
            {
                await _identityProvider.CreateAttendeeUserAsync(attendee.Email, "Euvic1234!", attendeesEmailMapping[attendee.Email]);
            });

            return Unit.Value;
        }

        private static string GetEmail(string firstname, string lastname) => $"{firstname.ToLower()}.{lastname.ToLower()}@euvic.pl";

        private IEnumerable<IdentityAttendee> GenerateAttenddees(int attendeesCount, HashSet<string> existingEmails)
        {
            var dataConfig = new Faker<Contract.IdentityAttendee>();
            dataConfig.RuleFor(x => x.Firstname, x => x.Name.FirstName().Replace("'", string.Empty));
            dataConfig.RuleFor(x => x.Lastname, x => x.Name.LastName().Replace("'", string.Empty));
            dataConfig.RuleFor(x => x.Email, (x, attendee) => $"{attendee.Firstname.ToLower()}.{attendee.Lastname.ToLower()}@euvic.pl");

            return dataConfig.Generate(attendeesCount)
                .DistinctBy(x => x.Email)
                .ToDictionary(key => key.Email, value => value)
                .Where(x => !existingEmails.Contains(x.Key))
                .Select(x => x.Value)
                .ToList();
        }
    }
}
