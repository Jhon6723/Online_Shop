namespace TiendaOnline1.Application.Interfaces;

public interface ISuperUserService
{
    Task<bool> CreateAnotherSuperUserAsync(string id);
    Task<bool> GetSuperUserByIdAsync(string id);
    Task<bool> UpdateSuperUserAsyncById(string id);
    Task<bool> DeleteSuperUserAsyncById(string id);
    Task<bool> GetAllSuperUsersAsync();
}
