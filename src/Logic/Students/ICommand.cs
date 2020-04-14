using CSharpFunctionalExtensions;
using Logic.AppServices;

namespace Logic.Mediator
{
    public interface ICommand 
    {
    }

    public interface ICommandHandler<TCommand> 
        where TCommand : ICommand
    {
        Result Handle(TCommand command);
    }
}