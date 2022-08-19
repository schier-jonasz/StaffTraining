using Euvic.Cqrs.Primitives;

namespace Euvic.StaffTraining.Contracts.Attendees.Commands
{
    public static class CreateDemoAttendees
    {
        public class Command : ICommand
        {
            public int AttendeesCount { get; set; }
        }

        public class IdentityAttendee
        {
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

        }

    }
}
