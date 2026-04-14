using AutoMapper;
using booksy.API.Data;
using booksy.API.Models.DTOs;
using booksy.API.Models.Entities;
using booksy.API.Models.Enums;
using booksy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace booksy.API.Services
{
    public class RentalRecordService : IRentalRecordService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RentalRecordService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RentalRecordDto>> GetAllAsync()
        {
            var rentals = await _context.RentalRecords
                .Where(r => r.DateDeleted == null)
                .Include(r => r.Hardware)
                .Include(r => r.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RentalRecordDto>>(rentals);
        }

        public async Task<RentalRecordDto?> GetByIdAsync(int id)
        {
            var r = await _context.RentalRecords
                .Include(r => r.Hardware)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id && r.DateDeleted == null);

            if (r == null) return null;

            return _mapper.Map<RentalRecordDto>(r);
        }

        public async Task<RentalRecordDto?> CreateAsync(CreateRentalRecordDto dto)
        {
            var hardware = await _context.Hardware
                .FirstOrDefaultAsync(h => h.Id == dto.HardwareId);

            if (hardware is null) return null;
            if (hardware.Status != HardwareStatus.Available) return null;

            var userExists = await _context.Users
                .AnyAsync(u => u.Id == dto.UserId && u.DateDeleted == null);

            if (!userExists) return null;

            var rental = new RentalRecord
            {
                HardwareId = dto.HardwareId,
                UserId = dto.UserId,
                RentedAt = DateTime.UtcNow
            };

            hardware.Status = HardwareStatus.InUse;
            _context.RentalRecords.Add(rental);
            await _context.SaveChangesAsync();

            await _context.Entry(rental).Reference(r => r.Hardware).LoadAsync();
            await _context.Entry(rental).Reference(r => r.User).LoadAsync();

            return _mapper.Map<RentalRecordDto>(rental);
        }

        public async Task<bool> ReturnAsync(int rentalId)
        {
            var rental = await _context.RentalRecords
                .Include(r => r.Hardware)
                .FirstOrDefaultAsync(r => r.Id == rentalId);

            if (rental is null || rental.ReturnedAt is not null) return false;

            rental.ReturnedAt = DateTime.UtcNow;
            rental.Hardware.Status = HardwareStatus.Available;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(int id, UpdateRentalRecordDto rentalDto)
        {
            var rental = await _context.RentalRecords
                .FirstOrDefaultAsync(r => r.Id == id && r.DateDeleted == null);

            if (rental == null) return false;

            _mapper.Map(rentalDto, rental);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rental = await _context.RentalRecords
                .FirstOrDefaultAsync(r => r.Id == id && r.DateDeleted == null);

            if (rental == null) return false;

            rental.DateDeleted = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
