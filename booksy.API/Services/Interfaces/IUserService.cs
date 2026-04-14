using booksy.API.Models.DTOs;

namespace booksy.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(CreateUserDto userDto);
        Task<bool> UpdateAsync(int id, UpdateUserDto userDto);
        Task<bool> DeleteAsync(int id);
    }
}
