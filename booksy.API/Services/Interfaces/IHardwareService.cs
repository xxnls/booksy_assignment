using booksy.API.Models.DTOs;

namespace booksy.API.Services.Interfaces
{
    public interface IHardwareService
    {
        Task<IEnumerable<HardwareDto>> GetAllAsync();
        Task<HardwareDto?> GetByIdAsync(int id);
        Task<HardwareDto> CreateAsync(CreateHardwareDto hardwareDto);
        Task<bool> UpdateAsync(int id, UpdateHardwareDto hardwareDto);
        Task<bool> DeleteAsync(int id);
    }
}
