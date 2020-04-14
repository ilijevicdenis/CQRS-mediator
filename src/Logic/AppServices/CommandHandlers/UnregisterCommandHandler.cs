using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.AppServices.CommandHandlers
{
    public sealed class UnregisterCommandHandler : IRequestHandler<UnregisterCommand, Result>
    {
        private readonly SessionFactory _sessionFactory;

        public UnregisterCommandHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        private Result Execute(UnregisterCommand command)
        {
            var unitOfWork = new UnitOfWork(_sessionFactory);
            var repository = new StudentRepository(unitOfWork);
            Student student = repository.GetById(command.Id);
            if (student == null)
                return Result.Fail($"No student found for Id {command.Id}");

            repository.Delete(student);
            unitOfWork.Commit();

            return Result.Ok();
        }

        public Task<Result> Handle(UnregisterCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(request));
        }
    }
}