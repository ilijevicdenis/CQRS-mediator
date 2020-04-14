using CSharpFunctionalExtensions;
using MediatR;

namespace Logic.AppServices
{
    public sealed class EnrollCommand : IRequest<Result>
    {
        public long Id { get; }
        public string Course { get; }
        public string Grade { get; }
        public EnrollCommand(long id, string course, string grade)
        {
            Id = id;
            Course = course;
            Grade = grade;
        }
    }
}