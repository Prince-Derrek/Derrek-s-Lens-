using System.Threading.Tasks;
using DerreksLens.Application.DTOs.Auth;
using DerreksLens.Application.Interfaces.Repositories;
using DerreksLens.Application.Interfaces.Services;
using DerreksLens.Core.Domain.Entities;
using DerreksLens.Core.Domain.Enums;

namespace DerreksLens.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> RegisterAsync(RegisterDto dto)
        {
            if (await _userRepository.EmailExistsAsync(dto.Email))
                throw new System.Exception("Email already taken.");

            if (await _userRepository.UsernameExistsAsync(dto.Username))
                throw new System.Exception("Username already taken.");

            // 1. Hash Password
            PasswordHasher.CreatePasswordHash(dto.Password, out string passwordHash, out string passwordSalt);

            // 2. Create Entity
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = UserRole.User, // Default to normal user
                CreatedAt = System.DateTime.UtcNow
            };

            // 3. Save
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return null;

            if (!PasswordHasher.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }
    }
}