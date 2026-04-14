using booksy.API.Models.DTOs;

namespace booksy.API.Services.Interfaces
{
    public interface IRentalRecordService
    {
        Task<IEnumerable<RentalRecordDto>> GetAllAsync();
        Task<RentalRecordDto?> GetByIdAsync(int id);
        Task<RentalRecordDto> CreateAsync(CreateRentalRecordDto rentalDto);
        Task<bool> ReturnAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateRentalRecordDto rentalDto);
        Task<bool> DeleteAsync(int id);
    }
}
