using System;

namespace DerreksLens.Application.DTOs.Posts
{
    // Used for the Homepage list
    public class PostListDto
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string? CoverImageUrl { get; set; }
    }
}