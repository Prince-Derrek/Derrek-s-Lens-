using System.Collections.Generic;
using DerreksLens.Core.Common;

namespace DerreksLens.Core.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty; // e.g. "software-engineering"
        public string? Description { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}