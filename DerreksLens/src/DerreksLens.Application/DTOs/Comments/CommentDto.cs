using System;
using System.Collections.Generic;

namespace DerreksLens.Application.DTOs.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsAdmin { get; set; } // To highlight Author comments

        // Recursion
        public int? ParentId { get; set; }
        public List<CommentDto> Replies { get; set; } = new List<CommentDto>();
    }
}