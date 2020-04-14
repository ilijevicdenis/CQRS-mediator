
using CSharpFunctionalExtensions;
using MediatR;

namespace Logic.AppServices
{
    public sealed class DisenrollCommand : IRequest<Result>
    {
        public long Id { get; }
        public int EnrollmentNumber { get; }
        public string Comment { get; }

        public DisenrollCommand(long id, int enrollmentNumber, string comment)
        {
            Id = id;
            EnrollmentNumber = enrollmentNumber;
            Comment = comment;
        }
    }
}