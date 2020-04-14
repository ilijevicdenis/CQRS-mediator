using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.AppServices
{
    public sealed class DisenrollCommandHandler : IRequestHandler<DisenrollCommand, Result>
    {
        private readonly SessionFactory _sessionFactory;

        public DisenrollCommandHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        
        private Result Execute(DisenrollCommand command)
        {
            var unitOfWork = new UnitOfWork(_sessionFactory);
            var studentRepository = new StudentRepository(unitOfWork);
            Student student = studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Fail($"No student found for Id {command.Id}");

            if (string.IsNullOrWhiteSpace(command.Comment))
                return Result.Fail("Disenrollment comment is required");

            Enrollment enrollment = student.GetEnrollment(command.EnrollmentNumber);
            if (enrollment == null)
                return Result.Fail($"No enrollment found with number '{command.EnrollmentNumber}'");

            student.RemoveEnrollment(enrollment, command.Comment);

            unitOfWork.Commit();

            return Result.Ok();
        }

        public Task<Result> Handle(DisenrollCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(request));
        }
    }
}
