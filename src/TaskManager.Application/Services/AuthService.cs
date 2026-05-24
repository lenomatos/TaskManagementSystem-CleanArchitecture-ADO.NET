using TaskManager.Application.DTOs.Auth;
using TaskManager.Application.Interfaces;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces;

namespace TaskManager.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            throw new ArgumentException("Username is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password is required.");

        if (request.Password.Length < 6)
            throw new ArgumentException("Password must have at least 6 characters.");

        var existingUser =
            await _userRepository.GetByUsernameAsync(request.Username);

        if (existingUser is not null)
            throw new InvalidOperationException("Username already exists.");

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Username = request.Username.Trim(),
            PasswordHash = _passwordHasher.Hash(request.Password)
        };

        await _userRepository.CreateAsync(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user =
            await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null)
            throw new UnauthorizedAccessException("Invalid username or password.");

        var validPassword =
            _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!validPassword)
            throw new UnauthorizedAccessException("Invalid username or password.");

        return new AuthResponse
        {
            Token = _jwtTokenService.GenerateToken(user),
            Username = user.Username
        };
    }
}