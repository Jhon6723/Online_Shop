using TiendaOnline1.Application.Interfaces;
using TiendaOnline1.Core.Database;
using TiendaOnline1.Models;

namespace TiendaOnline1.Services;

/// <summary>
/// Service class for managing SuperUser operations.
/// </summary>
public class SuperUserService : ISuperUserService
{
    private readonly MysqlDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SuperUserService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context to interact with the database.</param>
    public SuperUserService(MysqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Creates another super user based on the provided user ID.
    /// </summary>
    /// <param name="id">The ID of the user to promote to super user.</param>
    /// <returns>A task representing the asynchronous operation. Returns true if successful.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the user is null or has an invalid role.</exception>
    public Task<bool> CreateAnotherSuperUserAsync(string id)
    {
        var userChangue = _dbContext.Users.FirstOrDefault(x => x.Id == id);
        if (userChangue != null && userChangue.Role != "user")
        {
            _dbContext.SuperUser.Add(new SuperUser {
                Id = Guid.NewGuid().ToString(),
                Name = userChangue.Name,
                Password = userChangue.Password,
                CreatedAt = DateTime.UtcNow,
                UserId = userChangue.Id
            });
        }
        throw new InvalidOperationException($"Failed to create a super user. User with ID '{id}' is either null or has an invalid role.");
    }

    /// <summary>
    /// Deletes a super user by their ID.
    /// </summary>
    /// <param name="id">The ID of the super user to delete.</param>
    /// <returns>A task representing the asynchronous operation. Returns true if successful.</returns>
    public Task<bool> DeleteSuperUserAsyncById(string id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Generates a token for a super user.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. Returns true if successful.</returns>
    public Task<bool> GenerateTokenSuperUserAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves all super users.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. Returns true if successful.</returns>
    public Task<bool> GetAllSuperUsersAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves a super user by their ID.
    /// </summary>
    /// <param name="id">The ID of the super user to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. Returns true if successful.</returns>
    public Task<bool> GetSuperUserByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Updates a super user by their ID.
    /// </summary>
    /// <param name="id">The ID of the super user to update.</param>
    /// <returns>A task representing the asynchronous operation. Returns true if successful.</returns>
    public Task<bool> UpdateSuperUserAsyncById(string id)
    {
        throw new NotImplementedException();
    }
}
