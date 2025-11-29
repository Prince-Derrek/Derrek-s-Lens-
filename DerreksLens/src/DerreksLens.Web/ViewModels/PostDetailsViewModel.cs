using System;
using System.Collections.Generic;
using DerreksLens.Application.DTOs.Comments;

namespace DerreksLens.Web.ViewModels
{
    public class PostDetailsViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;

        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}