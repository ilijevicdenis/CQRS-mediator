using CSharpFunctionalExtensions;
using MediatR;

namespace Logic.AppServices
{
    public sealed class EditPersonalInfoCommand : IRequest<Result>
    {
        public long Id { get; }
        public string Name { get; }
        public string Email { get; }
        public EditPersonalInfoCommand(long id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
