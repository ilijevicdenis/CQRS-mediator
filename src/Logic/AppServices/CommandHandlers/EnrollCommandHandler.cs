using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;
using MediatR;

namespace Logic.AppServices.CommandHandlers
{
    public sealed class EnrollCommandHandler : IRequestHandler<EnrollCommand, Result>
    {
        private readonly SessionFactory _sessionFactory;

        public EnrollCommandHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        private Result Execute(EnrollCommand command)
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

            student.Enroll(course, grade);

            unitOfWork.Commit();

            return Result.Ok();
        }

        public Task<Result> Handle(EnrollCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(request));
        }
    }
}