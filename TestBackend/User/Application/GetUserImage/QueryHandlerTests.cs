using AutoFixture;
using Backend.Service;
using Backend.User.Application.GetUserImage;
using NSubstitute;

namespace TestBackend.User.Application.GetUserImage;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    private readonly IUserImageService _userImageService = Substitute.For<IUserImageService>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_userImageService);
    }

    [Fact]
    public async Task Handle_ReturnsByteArray_WhenImageExists()
    {
        var query = _fixture.Create<Query>();
        var expected = _fixture.Create<byte[]>();

        _userImageService.GetImageDataAsync(query.userId)
            .Returns(expected);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(expected, result.UserImageDto.ImageData);
    }

    [Fact]
    public async Task Handle_ReturnsNullObject_WhenImageNotFound()
    {
        var query = _fixture.Create<Query>();
        byte[]? expected = null;

        _userImageService.GetImageDataAsync(query.userId)
            .Returns(expected);

        var result = await _handler.Handle(query, CancellationToken.None);
        
        Assert.Null(result.UserImageDto.ImageData);
    }
}