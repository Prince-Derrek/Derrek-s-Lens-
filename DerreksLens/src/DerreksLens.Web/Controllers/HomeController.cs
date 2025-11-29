using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DerreksLens.Web.Models;
using DerreksLens.Application.Interfaces.Repositories;
using DerreksLens.Application.DTOs.Posts;
using DerreksLens.Core.Domain.Entities;

namespace DerreksLens.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPostRepository _postRepository;

    public HomeController(ILogger<HomeController> logger, IPostRepository postRepository)
    {
        _logger = logger;
        _postRepository = postRepository;
    }

    // 1. HOME PAGE (Route: /)
    // Uses the default convention from Program.cs
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Home";
        ViewData["CurrentCategory"] = null;

        // Fetch recent posts
        var posts = await _postRepository.GetRecentPostsAsync(10);

        var model = MapToDto(posts);
        return View(model);
    }

    // 2. CATEGORY PAGE (Route: /category/{slug})
    // Uses specific Attribute Routing
    [HttpGet("category/{categorySlug}")]
    public async Task<IActionResult> Category(string categorySlug)
    {
        if (string.IsNullOrEmpty(categorySlug)) return RedirectToAction(nameof(Index));

        ViewData["Title"] = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(categorySlug.Replace("-", " "));
        ViewData["CurrentCategory"] = categorySlug;

        // Fetch filtered posts
        var posts = await _postRepository.GetPostsByCategoryAsync(categorySlug);

        var model = MapToDto(posts);

        // Reuse the Index view because the UI is the same
        return View("Index", model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Helper to avoid duplicate mapping code
    private List<PostListDto> MapToDto(IEnumerable<Post> posts)
    {
        return posts.Select(p => new PostListDto
        {
            Title = p.Title,
            Slug = p.Slug,
            Summary = p.Summary,
            AuthorName = p.Author.Username,
            CategoryName = p.Category.Name,
            PublishedDate = p.CreatedAt,
            CoverImageUrl = p.CoverImageUrl
        }).ToList();
    }
}