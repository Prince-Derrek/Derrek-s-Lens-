using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DerreksLens.Web.Models;
using DerreksLens.Application.Interfaces.Repositories;
using DerreksLens.Application.DTOs.Posts;

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

    public async Task<IActionResult> Index()
    {
        // 1. Fetch Entities
        var posts = await _postRepository.GetRecentPostsAsync(10);

        // 2. Map to DTOs
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