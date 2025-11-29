using DerreksLens.Core.Common;
using DerreksLens.Core.Domain.Enums;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DerreksLens.Core.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Manual Auth Security Columns
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.User;
        public string? Bio { get; set; }

        // Navigation Properties
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}