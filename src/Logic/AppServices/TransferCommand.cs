using CSharpFunctionalExtensions;
using MediatR;

namespace Logic.AppServices
{
    public sealed class TransferCommand : IRequest<Result>
    {
        public long Id { get; }
        public int EnrollmentNumber { get; }
        public string Course { get; }
        public string Grade { get; }
        public TransferCommand(long id, int enrollmentNumber, string course, string grade)
        {
            Id = id;
            EnrollmentNumber = enrollmentNumber;
            Course = course;
            Grade = grade;
        }
    }
}