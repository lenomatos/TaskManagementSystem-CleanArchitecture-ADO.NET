using Moq;
using TaskManager.Application.DTOs.Auth;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces;

namespace TaskManager.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_ShouldThrow_WhenUsernameAlreadyExists()
    {
        var userRepository = new Mock<IUserRepository>();
        var jwtService = new Mock<IJwtTokenService>();
        var passwordHasher = new Mock<IPasswordHasher>();

        userRepository
            .Setup(x => x.GetByUsernameAsync("demo"))
            .ReturnsAsync(new UserEntity
            {
                Id = Guid.NewGuid(),
                Username = "demo",
                PasswordHash = "hash"
            });

        var service = new AuthService(
            userRepository.Object,
            jwtService.Object,
            passwordHasher.Object);

        var request = new RegisterRequest
        {
            Username = "demo",
            Password = "demo123"
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.RegisterAsync(request));
    }

    [Fact]
    public async Task RegisterAsync_ShouldCreateUser_WhenValid()
    {
        var userRepository = new Mock<IUserRepository>();
        var jwtService = new Mock<IJwtTokenService>();
        var passwordHasher = new Mock<IPasswordHasher>();

        userRepository
            .Setup(x => x.GetByUsernameAsync("newuser"))
            .ReturnsAsync((UserEntity?)null);

        passwordHasher
            .Setup(x => x.Hash("password123"))
            .Returns("hashed-password");

        var service = new AuthService(
            userRepository.Object,
            jwtService.Object,
            passwordHasher.Object);

        var request = new RegisterRequest
        {
            Username = "newuser",
            Password = "password123"
        };

        await service.RegisterAsync(request);

        userRepository.Verify(
            x => x.CreateAsync(It.Is<UserEntity>(
                u => u.Username == "newuser" &&
                     u.PasswordHash == "hashed-password")),
            Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenUserDoesNotExist()
    {
        var userRepository = new Mock<IUserRepository>();
        var jwtService = new Mock<IJwtTokenService>();
        var passwordHasher = new Mock<IPasswordHasher>();

        userRepository
            .Setup(x => x.GetByUsernameAsync("missing"))
            .ReturnsAsync((UserEntity?)null);

        var service = new AuthService(
            userRepository.Object,
            jwtService.Object,
            passwordHasher.Object);

        var request = new LoginRequest
        {
            Username = "missing",
            Password = "demo123"
        };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.LoginAsync(request));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenPasswordIsInvalid()
    {
        var userRepository = new Mock<IUserRepository>();
        var jwtService = new Mock<IJwtTokenService>();
        var passwordHasher = new Mock<IPasswordHasher>();

        userRepository
            .Setup(x => x.GetByUsernameAsync("demo"))
            .ReturnsAsync(new UserEntity
            {
                Id = Guid.NewGuid(),
                Username = "demo",
                PasswordHash = "hash"
            });

        passwordHasher
            .Setup(x => x.Verify("wrong", "hash"))
            .Returns(false);

        var service = new AuthService(
            userRepository.Object,
            jwtService.Object,
            passwordHasher.Object);

        var request = new LoginRequest
        {
            Username = "demo",
            Password = "wrong"
        };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.LoginAsync(request));
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Username = "demo",
            PasswordHash = "hash"
        };

        var userRepository = new Mock<IUserRepository>();
        var jwtService = new Mock<IJwtTokenService>();
        var passwordHasher = new Mock<IPasswordHasher>();

        userRepository
            .Setup(x => x.GetByUsernameAsync("demo"))
            .ReturnsAsync(user);

        passwordHasher
            .Setup(x => x.Verify("demo123", "hash"))
            .Returns(true);

        jwtService
            .Setup(x => x.GenerateToken(user))
            .Returns("fake-jwt-token");

        var service = new AuthService(
            userRepository.Object,
            jwtService.Object,
            passwordHasher.Object);

        var request = new LoginRequest
        {
            Username = "demo",
            Password = "demo123"
        };

        var result = await service.LoginAsync(request);

        Assert.Equal("fake-jwt-token", result.Token);
        Assert.Equal("demo", result.Username);
    }
}