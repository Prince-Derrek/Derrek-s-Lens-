using DerreksLens.Application.DTOs.Posts;
using DerreksLens.Application.Interfaces.Repositories;
using DerreksLens.Core.Domain.Entities;
using DerreksLens.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

    [HttpGet]
    [HttpGet("category/{categorySlug}")]
    public async Task<IActionResult> Index(string? categorySlug = null)
    {
        IEnumerable<Post> posts;
        ViewData["CurrentCategory"] = categorySlug;

        if (string.IsNullOrEmpty(categorySlug))
        {
            // Get all (limit 10 for home)
            posts = await _postRepository.GetRecentPostsAsync(10);
            ViewData["Title"] = "Home";
        }
        else
        {
            // Get by category
            posts = await _postRepository.GetPostsByCategoryAsync(categorySlug);
            ViewData["Title"] = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(categorySlug.Replace("-", " "));
        }

        var postDtos = posts.Select(p => new PostListDto
        {
            Title = p.Title,
            Slug = p.Slug,
            Summary = p.Summary,
            AuthorName = p.Author.Username,
            CategoryName = p.Category.Name,
            PublishedDate = p.CreatedAt,
            CoverImageUrl = p.CoverImageUrl
        }).ToList();

        return View(postDtos);
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
}