using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;
using MediatR;

namespace Logic.AppServices.CommandHandlers
{
    public sealed class TransferCommandHandler : IRequestHandler<TransferCommand, Result>
    {
        private readonly SessionFactory _sessionFactory;

        public TransferCommandHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Task<Result> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(request));
        }

        private Result Execute(TransferCommand command)
        {
            var unitOfWork = new UnitOfWork(_sessionFactory);
            var courseRepository = new CourseRepository(unitOfWork);
            var studentRepository = new StudentRepository(unitOfWork);
            Student student = studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Fail($"No student found with Id '{command.Id}'");

            Course course = courseRepository.GetByName(command.Course);
            if (course == null)
                return Result.Fail($"Course is incorrect: '{command.Course}'");

            bool success = Enum.TryParse(command.Grade, out Grade grade);
            if (!success)
                return Result.Fail($"Grade is incorrect: '{command.Grade}'");

            Enrollment enrollment = student.GetEnrollment(command.EnrollmentNumber);
            if (enrollment == null)
                return Result.Fail($"No enrollment found with number '{command.EnrollmentNumber}'");

            enrollment.Update(course, grade);

            unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
