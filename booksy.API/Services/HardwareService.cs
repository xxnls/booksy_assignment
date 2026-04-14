using AutoMapper;
using booksy.API.Data;
using booksy.API.Models.DTOs;
using booksy.API.Models.Entities;
using booksy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace booksy.API.Services
{
    public class HardwareService : IHardwareService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public HardwareService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HardwareDto>> GetAllAsync()
        {
            var hardware = await _context.Hardwares
                .Where(h => h.DateDeleted == null)
                .ToListAsync();

            return _mapper.Map<IEnumerable<HardwareDto>>(hardware);
        }

        public async Task<HardwareDto?> GetByIdAsync(int id)
        {
            var hardware = await _context.Hardwares
                .FirstOrDefaultAsync(h => h.Id == id && h.DateDeleted == null);

            if (hardware == null) return null;

            return _mapper.Map<HardwareDto>(hardware);
        }

        public async Task<HardwareDto> CreateAsync(CreateHardwareDto hardwareDto)
        {
            var hardware = _mapper.Map<Hardware>(hardwareDto);

            _context.Hardwares.Add(hardware);
            await _context.SaveChangesAsync();

            return _mapper.Map<HardwareDto>(hardware);
        }

        public async Task<bool> UpdateAsync(int id, UpdateHardwareDto hardwareDto)
        {
            var hardware = await _context.Hardwares
                .FirstOrDefaultAsync(h => h.Id == id && h.DateDeleted == null);

            if (hardware == null) return false;

            _mapper.Map(hardwareDto, hardware);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var hardware = await _context.Hardwares
                .FirstOrDefaultAsync(h => h.Id == id && h.DateDeleted == null);

            if (hardware == null) return false;

            _context.Hardwares.Remove(hardware);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
