using Backend.Service;
using MediatR;

namespace Backend.User.Application.GetUserImage;

public record Query(string userId) : IRequest<UserImageResponse>;

public record UserImageResponse(UserImageDto UserImageDto);

public class UserImageDto
{
    public byte[]? ImageData { get; set; }
}

public class QueryHandler : IRequestHandler<Query, UserImageResponse>
{
    private readonly IUserImageService _userImageService;

    public QueryHandler(IUserImageService userImageService)
    {
        _userImageService = userImageService;
    }

    public async Task<UserImageResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var imageData = await _userImageService.GetImageData(request.userId);

        return new UserImageResponse(new UserImageDto(){ImageData = imageData});
    }

}