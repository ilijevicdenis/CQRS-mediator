using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Students;
using Logic.Utils;
using MediatR;

namespace Logic.AppServices.CommandHandlers
{
    [AuditLog]
    public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly SessionFactory _sessionFactory;

        public RegisterCommandHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        private Result Execute(RegisterCommand command)
        {
            var unitOfWork = new UnitOfWork(_sessionFactory);
            var courseRepository = new CourseRepository(unitOfWork);
            var studentRepository = new StudentRepository(unitOfWork);
            var student = new Student(command.Name, command.Email);

            if (command.Course1 != null && command.Course1Grade != null)
            {
                Course course = courseRepository.GetByName(command.Course1);
                student.Enroll(course, Enum.Parse<Grade>(command.Course1Grade));
            }

            if (command.Course2 != null && command.Course2Grade != null)
            {
                Course course = courseRepository.GetByName(command.Course2);
                student.Enroll(course, Enum.Parse<Grade>(command.Course2Grade));
            }

            studentRepository.Save(student);
            unitOfWork.Commit();

            return Result.Ok();
        }

        public Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(request));
        }
    }
}
