using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces;

public interface IUserRepository
{
    Task<UserEntity?> GetByUsernameAsync(
        string username);

    Task<UserEntity?> GetByIdAsync(
        Guid id);

    Task CreateAsync(
        UserEntity user);
}