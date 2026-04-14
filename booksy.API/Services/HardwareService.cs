using AutoMapper;
using booksy.API.Data;
using booksy.API.Models.DTOs;
using booksy.API.Models.Entities;
using booksy.API.Models.Enums;
using booksy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace booksy.API.Services
{
    public class HardwareService : IHardwareService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IRentalRecordService _rentalRecordService;

        public HardwareService(AppDbContext context, IMapper mapper, IRentalRecordService rentalRecordService)
        {
            _context = context;
            _mapper = mapper;
            _rentalRecordService = rentalRecordService;
        }

        public async Task<IEnumerable<HardwareDto>> GetAllAsync()
        {
            var hardware = await _context.Hardware
                .Include(h => h.RentalRecords.Where(r => r.ReturnedAt == null))
                    .ThenInclude(r => r.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<HardwareDto>>(hardware);
        }


        public async Task<HardwareDto?> GetByIdAsync(int id)
        {
            var hardware = await _context.Hardware
                .Include(h => h.RentalRecords.Where(r => r.ReturnedAt == null))
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(h => h.Id == id);

            return hardware is null ? null : _mapper.Map<HardwareDto>(hardware);
        }
        public async Task<HardwareDto?> CreateAsync(CreateHardwareDto dto)
        {
            User? user = null;

            if (!string.IsNullOrWhiteSpace(dto.AssignedTo))
            {
                user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == dto.AssignedTo && u.DateDeleted == null);

                if (user is null) return null;
            }

            var hardware = _mapper.Map<Hardware>(dto);

            if (user is not null)
            {
                hardware.Status = HardwareStatus.InUse;
                hardware.RentalRecords.Add(new RentalRecord
                {
                    UserId = user.Id,
                    RentedAt = DateTime.UtcNow
                });
            }

            _context.Hardware.Add(hardware);
            await _context.SaveChangesAsync();

            return _mapper.Map<HardwareDto>(hardware);
        }

        public async Task<bool> UpdateAsync(int id, UpdateHardwareDto hardwareDto)
        {
            var hardware = await _context.Hardware
                .FirstOrDefaultAsync(h => h.Id == id && h.DateDeleted == null);

            if (hardware == null) return false;

            _mapper.Map(hardwareDto, hardware);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var hardware = await _context.Hardware
                .Include(h => h.RentalRecords)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hardware is null) return false;

            if (hardware.RentalRecords.Any(r => r.ReturnedAt is null))
                return false;

            hardware.DateDeleted = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
