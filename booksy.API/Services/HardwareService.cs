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
            private readonly IRentalRecordService _rentalRecordService;

            public HardwareService(AppDbContext context, IMapper mapper, IRentalRecordService rentalRecordService)
            {
                _context = context;
                _mapper = mapper;
                _rentalRecordService = rentalRecordService;
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
                User? user = null;

                if (!string.IsNullOrWhiteSpace(hardwareDto.AssignedTo))
                {
                    user = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email == hardwareDto.AssignedTo && u.DateDeleted == null);

                    if (user == null)
                    {
                        return null;
                    }
                }

                var hardware = _mapper.Map<Hardware>(hardwareDto);
                _context.Hardwares.Add(hardware);
                await _context.SaveChangesAsync();

                if (user != null)
                {
                    var rentalRecord = new CreateRentalRecordDto
                    {
                        UserId = user.Id,
                        HardwareId = hardware.Id,
                        RentedAt = DateTime.UtcNow
                    };

                    await _rentalRecordService.CreateAsync(rentalRecord);

                    hardware.Status = Models.Enums.HardwareStatus.InUse;
                    await _context.SaveChangesAsync();
                }

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
