using System.ComponentModel.DataAnnotations;

namespace DerreksLens.Application.DTOs.Comments
{
    public class CreateCommentDto
    {
        [Required]
        public int PostId { get; set; }

        public int? ParentId { get; set; } // Optional (if null, it's a top-level comment)

        [Required, MinLength(2), MaxLength(1000)]
        public string Content { get; set; } = string.Empty;

        // Helper for redirection
        public string PostSlug { get; set; } = string.Empty;
    }
}