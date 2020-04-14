using CSharpFunctionalExtensions;
using MediatR;

namespace Logic.AppServices
{
    public sealed class UnregisterCommand : IRequest<Result>
    {
        public long Id { get; }
        public UnregisterCommand(long id)
        {
            Id = id;
        }
    }
}