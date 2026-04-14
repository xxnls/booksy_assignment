using AutoMapper;
using booksy.API.Data;
using booksy.API.Models.DTOs;
using booksy.API.Models.Entities;
using booksy.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace booksy.API.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserService(AppDbContext context, UserManager<User> userManager, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
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

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || user.DateDeleted != null) return null;

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid) return null;

            var userRoles = await _userManager.GetRolesAsync(user);
            var role = userRoles.FirstOrDefault() ?? "User";

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                User = _mapper.Map<UserDto>(user)
            };
        }
    }
}
