using System.Threading.Tasks;
using DerreksLens.Application.DTOs.Auth;
using DerreksLens.Core.Domain.Entities;

namespace DerreksLens.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterDto dto);
        Task<User?> ValidateUserAsync(string email, string password);
    }
}