using TiendaOnline1.Application.Dtos;

namespace TiendaOnline1.Application.Interfaces;

public interface IUserServiceDatabase
{
    Task<bool> CreateUserAsync(UserRequestRegisterDto userRequestRegisterDto);
    Task<string> GenerateToken(UserRequestLoginDto userRequestLoginDto);
    // ID should be a string, not an int
    Task<bool> GetUserByIdAsync(string id);
}
