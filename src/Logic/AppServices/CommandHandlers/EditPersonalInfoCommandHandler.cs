using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Students;
using Logic.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.AppServices.CommandHandlers
{
    [AuditLog]
    [DatabaseRetry]
    public sealed class EditPersonalInfoCommandHandler : IRequestHandler<EditPersonalInfoCommand, Result>
    {
        private readonly SessionFactory _sessionFactory;

        public EditPersonalInfoCommandHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        private Result Execute(EditPersonalInfoCommand command)
        {
            var unitOfWork = new UnitOfWork(_sessionFactory);
            var repository = new StudentRepository(unitOfWork);
            Student student = repository.GetById(command.Id);

            if (student == null)
                return Result.Fail($"No student found for Id {command.Id}");

            student.Name = command.Name;
            student.Email = command.Email;

            unitOfWork.Commit();

            return Result.Ok();
        }

        public Task<Result> Handle(EditPersonalInfoCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(request));
        }
    }
}
