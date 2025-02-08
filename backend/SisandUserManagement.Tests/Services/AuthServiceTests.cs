using Microsoft.Extensions.Configuration;
using Moq;
using SisandUserManagement.Application.Interfaces.Repositories;
using SisandUserManagement.Application.Services;
using SisandUserManagement.Domain.Entities;

namespace SisandUserManagement.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _configurationMock = new Mock<IConfiguration>();

        _configurationMock.SetupGet(c => c["Jwt:Secret"]).Returns("supersecurekey12345678901234567890");
        _configurationMock.SetupGet(c => c["Jwt:ExpirationInMinutes"]).Returns("60");
        _configurationMock.SetupGet(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        _configurationMock.SetupGet(c => c["Jwt:Audience"]).Returns("TestAudience");

        _authService = new AuthService(_userRepositoryMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var existingUser = new User { Email = "test@email.com" };
        _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                           .ReturnsAsync(existingUser);

        var newUser = new User { Email = "test@email.com", Username = "newuser" };

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RegisterAsync(newUser));
        Assert.Equal("Este e-mail já está em uso.", exception.Message);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowException_WhenUsernameAlreadyExists()
    {
        var existingUser = new User { Username = "existingUser" };
        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(It.IsAny<string>()))
                           .ReturnsAsync(existingUser);

        var newUser = new User { Email = "new@email.com", Username = "existingUser" };

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RegisterAsync(newUser));
        Assert.Equal("Este nome de usuário já está em uso.", exception.Message);
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnToken_WhenValidCredentials()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Username = "validUser", PasswordHash = hashedPassword };

        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync("validUser"))
                           .ReturnsAsync(user);

        var token = await _authService.AuthenticateAsync("validUser", "password123");

        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnNull_WhenInvalidPassword()
    {
        var user = new User { Username = "validUser", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123") };

        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync("validUser"))
                           .ReturnsAsync(user);

        var token = await _authService.AuthenticateAsync("validUser", "wrongpassword");

        Assert.Null(token);
    }
}
