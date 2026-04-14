using AutoMapper;
using booksy.API.Data;
using booksy.API.Models.DTOs;
using booksy.API.Models.Entities;
using booksy.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace booksy.API.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _context.Users
                .Where(u => u.DateDeleted == null)
                .ToListAsync();
            
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.DateDeleted == null);

            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> CreateAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            // set UserName to Email for Identity
            user.UserName = dto.Email;

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return null;

            await _userManager.AddToRoleAsync(user, dto.Role.ToString());

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null || user.DateDeleted is not null) return false;

            if (dto.Email is not null)
            {
                await _userManager.SetEmailAsync(user, dto.Email);
                await _userManager.SetUserNameAsync(user, dto.Email);
            }

            if (dto.Password is not null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, dto.Password);
            }

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.DateDeleted == null);

            if (user == null) return false;

            user.DateDeleted = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
