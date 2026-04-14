using booksy.API.Models.DTOs;

namespace booksy.API.Services.Interfaces
{
    public interface IAISearchService
    {
        Task<IEnumerable<int>> SearchHardwareIdsAsync(string query, IEnumerable<HardwareDto> hardwareList);
    }
}
