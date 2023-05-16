using System.ComponentModel.DataAnnotations;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.CreateUser;

public record Command(string userId) : IRequest;

public class CommandHandler : IRequestHandler<Command>
{
    private readonly IUserRepository _repository;
    
    public CommandHandler(IUserRepository userRepository, IMediator mediator)
    {
        _repository = userRepository;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        await _repository.CreateUserAsync(request.userId);
    }
}