using System.ComponentModel.DataAnnotations;
using DerreksLens.Core.Domain.Enums;

namespace DerreksLens.Application.DTOs.Posts
{
    public class UpdatePostDto : CreatePostDto
    {
        public int Id { get; set; }
        public string CurrentSlug { get; set; } = string.Empty;
    }
}