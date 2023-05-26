using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Service;
using Backend.User.Application.GetUserImage;
using Backend.User.Infrastructure;
using NSubstitute;

namespace TestBackend.User.Application.GetUserImage;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    private readonly IUserImageRepository _userImageService = Substitute.For<IUserImageRepository>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_userImageService, _transactionFactory);
    }

    [Fact]
    public async Task Handle_ReturnsByteArray_WhenImageExists()
    {
        var query = _fixture.Create<Query>();
        var expected = _fixture.Create<byte[]>();

        _userImageService.ReadImageForUserAsync(query.userId, Arg.Any<DbReadOnlyTransaction>())
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

        _userImageService.ReadImageForUserAsync(query.userId, Arg.Any<DbReadOnlyTransaction>())
            .Returns(expected);

        var result = await _handler.Handle(query, CancellationToken.None);
        
        Assert.Null(result.UserImageDto.ImageData);
    }
}