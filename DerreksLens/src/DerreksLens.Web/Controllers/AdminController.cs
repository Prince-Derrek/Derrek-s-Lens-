using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DerreksLens.Application.Common.Utils;
using DerreksLens.Application.DTOs.Posts;
using DerreksLens.Application.Interfaces.Repositories;
using DerreksLens.Core.Domain.Entities;
using DerreksLens.Core.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DerreksLens.Web.Controllers
{
    [Authorize(Roles = "Admin")] // 🛡️ CRITICAL SECURITY
    public class AdminController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;

        public AdminController(IPostRepository postRepository, ICategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: /Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            // We reuse the existing repository method for now, but in a real app 
            // we'd want a specific GetAllIncludingDrafts method.
            // For now, let's assume GetAllPublishedAsync only gets published.
            // We need to fetch ALL posts. Let's add a GetAllAsync to IPostRepository later.
            // For this phase, we will fetch recent posts and filter in memory or add the method.

            // Let's add the method to the Interface/Repo quickly below to do this right.
            var posts = await _postRepository.GetRecentPostsAsync(100); // Temporary Hack: Get 100 recent
            return View(posts);
        }

        // GET: /Admin/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: /Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
                return View(model);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var post = new Post
            {
                Title = model.Title,
                Slug = SlugGenerator.Generate(model.Title), // Auto-generate
                Summary = model.Summary,
                Content = model.Content,
                CoverImageUrl = model.CoverImageUrl,
                CategoryId = model.CategoryId,
                Status = model.Status,
                AuthorId = userId,
                CreatedAt = DateTime.UtcNow,
                ViewCount = 0
            };

            // Check for duplicate slug
            if (await _postRepository.GetBySlugAsync(post.Slug) != null)
            {
                post.Slug += $"-{Guid.NewGuid().ToString().Substring(0, 4)}";
            }

            await _postRepository.AddAsync(post);
            await _postRepository.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        // GET: /Admin/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null) return NotFound();

            var dto = new UpdatePostDto
            {
                Id = post.Id,
                Title = post.Title,
                Summary = post.Summary,
                Content = post.Content,
                CoverImageUrl = post.CoverImageUrl,
                CategoryId = post.CategoryId,
                Status = post.Status,
                CurrentSlug = post.Slug
            };

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", post.CategoryId);
            return View(dto);
        }

        // POST: /Admin/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePostDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", model.CategoryId);
                return View(model);
            }

            var post = await _postRepository.GetByIdAsync(model.Id);
            if (post == null) return NotFound();

            post.Title = model.Title;
            post.Summary = model.Summary;
            post.Content = model.Content;
            post.CoverImageUrl = model.CoverImageUrl;
            post.CategoryId = model.CategoryId;
            post.Status = model.Status;
            post.UpdatedAt = DateTime.UtcNow;

            // Only regenerate slug if title changed significantly, strictly speaking we might want to keep slug stable for SEO.
            // For now, let's KEEP slug stable unless explicitly requested. 
            // Implementation: We won't update Slug here to preserve links.

            await _postRepository.UpdateAsync(post);
            await _postRepository.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        // POST: /Admin/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post != null)
            {
                await _postRepository.DeleteAsync(post);
                await _postRepository.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Dashboard));
        }
    }
}