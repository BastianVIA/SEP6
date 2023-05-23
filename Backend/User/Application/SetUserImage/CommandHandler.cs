using Backend.Service;
using MediatR;

namespace Backend.User.Application.SetUserImage;

public record Command(string userId, byte[] data) : IRequest;

public class CommandHandler : IRequestHandler<Command>
{
    private readonly IUserImageService _userImageService;

    public CommandHandler(IUserImageService userImageService)
    {
        _userImageService = userImageService;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        await _userImageService.UploadImage(request.userId, request.data);
    }
}