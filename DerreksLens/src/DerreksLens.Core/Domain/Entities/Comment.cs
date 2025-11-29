using System.Collections.Generic;
using DerreksLens.Core.Common;

namespace DerreksLens.Core.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = true; // For moderation

        // Relationships
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Self-Referencing for Nested Replies
        public int? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}