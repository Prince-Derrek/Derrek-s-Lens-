using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DerreksLens.Application.DTOs.Comments;
using DerreksLens.Application.Interfaces.Repositories;
using DerreksLens.Core.Domain.Entities;
using DerreksLens.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DerreksLens.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public PostController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        [HttpGet("post/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var post = await _postRepository.GetBySlugAsync(slug);
            if (post == null) return NotFound();

            // 1. Fetch flat comments
            var comments = await _commentRepository.GetByPostIdAsync(post.Id);

            // 2. Build Tree (In-Memory)
            var commentDtos = BuildCommentTree(comments, null);

            var viewModel = new PostDetailsViewModel
            {
                PostId = post.Id,
                Title = post.Title,
                Slug = post.Slug,
                Content = post.Content,
                PublishedDate = post.CreatedAt,
                AuthorName = post.Author.Username,
                CategoryName = post.Category.Name,
                Comments = commentDtos
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize] // 🔒 Must be logged in
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CreateCommentDto input)
        {
            if (!ModelState.IsValid) return RedirectToAction("Details", new { slug = input.PostSlug });

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var comment = new Comment
            {
                PostId = input.PostId,
                UserId = userId,
                Content = input.Content,
                // FIX: Map DTO ParentId -> Entity ParentCommentId
                ParentCommentId = input.ParentId,
                CreatedAt = DateTime.UtcNow,
                IsApproved = true
            };

            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();

            return RedirectToAction("Details", new { slug = input.PostSlug });
        }

        // Recursive Tree Builder
        private List<CommentDto> BuildCommentTree(IEnumerable<Comment> allComments, int? parentId)
        {
            // FIX: Use c.ParentCommentId here
            return allComments
                .Where(c => c.ParentCommentId == parentId)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    Content = c.Content,
                    Username = c.User.Username,
                    CreatedAt = c.CreatedAt,
                    IsAdmin = c.User.Role == Core.Domain.Enums.UserRole.Admin,
                    // FIX: Pass c.Id (Current ID) as the Parent ID for the next recursion level
                    Replies = BuildCommentTree(allComments, c.Id)
                })
                .ToList();
        }
    }
}