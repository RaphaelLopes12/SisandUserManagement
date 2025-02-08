using Moq;
using SisandUserManagement.Application.Interfaces.Repositories;
using SisandUserManagement.Application.Services;
using SisandUserManagement.Domain.Entities;

namespace SisandUserManagement.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAuthenticatedUserAsync_ShouldReturnUser_WhenValidId()
    {
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
                           .ReturnsAsync(user);

        var result = await _userService.GetAuthenticatedUserAsync(userId.ToString());

        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public async Task GetAuthenticatedUserAsync_ShouldReturnNull_WhenInvalidId()
    {
        var result = await _userService.GetAuthenticatedUserAsync("invalid-guid");

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync((User)null);

        var updatedUser = new User { Id = Guid.NewGuid(), Name = "Updated Name" };

        var result = await _userService.UpdateUserAsync(updatedUser.Id, updatedUser);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateUser_WhenValid()
    {
        var userId = Guid.NewGuid();
        var existingUser = new User { Id = userId, Name = "Old Name" };

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
                           .ReturnsAsync(existingUser);

        var updatedUser = new User { Id = userId, Name = "Updated Name" };

        _userRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
                           .Returns(Task.CompletedTask);

        var result = await _userService.UpdateUserAsync(userId, updatedUser);

        Assert.True(result);
        Assert.Equal("Updated Name", existingUser.Name);
    }
}
