using TaskManager.Core.Entities;

namespace TaskManager.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(UserEntity user);
}