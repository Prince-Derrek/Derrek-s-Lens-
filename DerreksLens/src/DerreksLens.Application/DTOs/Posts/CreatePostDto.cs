using System.ComponentModel.DataAnnotations;
using DerreksLens.Core.Domain.Enums;

namespace DerreksLens.Application.DTOs.Posts
{
    public class CreatePostDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string Summary { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? CoverImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public PostStatus Status { get; set; } = PostStatus.Draft;
    }
}