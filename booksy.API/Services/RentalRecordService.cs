using AutoMapper;
using booksy.API.Data;
using booksy.API.Models.DTOs;
using booksy.API.Models.Entities;
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

        public async Task<RentalRecordDto> CreateAsync(CreateRentalRecordDto rentalDto)
        {
            var rental = _mapper.Map<RentalRecord>(rentalDto);

            _context.RentalRecords.Add(rental);
            await _context.SaveChangesAsync();

            // Reload to get navigation properties for mapping
            await _context.Entry(rental).Reference(r => r.Hardware).LoadAsync();
            await _context.Entry(rental).Reference(r => r.User).LoadAsync();

            return _mapper.Map<RentalRecordDto>(rental);
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

            _context.RentalRecords.Remove(rental);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
