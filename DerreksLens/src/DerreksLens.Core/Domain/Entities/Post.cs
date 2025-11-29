using DerreksLens.Core.Common;
using DerreksLens.Core.Domain.Enums;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DerreksLens.Core.Domain.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty; // e.g. "my-first-post"
        public string Summary { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; // Stores raw HTML or Markdown
        public string? CoverImageUrl { get; set; }

        public PostStatus Status { get; set; } = PostStatus.Draft;
        public int ViewCount { get; set; } = 0;

        // Foreign Keys & Navigations
        public int AuthorId { get; set; }
        public User Author { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}