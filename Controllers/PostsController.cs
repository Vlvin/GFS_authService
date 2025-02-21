/*using Microsoft.AspNetCore.Http;
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Backend.Database;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class PostsController(UserManager<AuthUser> userManager, Context context) : ControllerBase
{
    private readonly UserManager<AuthUser> _userManager = userManager;
    private readonly Context _context = context;

    [AllowAnonymous]
    [HttpGet("[action]")]
    public async Task<ActionResult<AllPostsResponse>> All()
    {

        var posts = _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Questions)
            .ThenInclude(q => q.Answers)
            .Include(p => p.Likes)
            .OrderBy(p => p.Id)
            .Select(p => new PostDTO(p))
            .ToList(); // ERROR: Happens here on PopulateList
        Console.WriteLine($"Count {posts.Count}");
        return Ok(new AllPostsResponse(posts));
    }

    // TODO: Implement
    [HttpGet("[action]")]
    public async Task<ActionResult<SearchPostsResponse>> Search(string keyword)
    {
        keyword = keyword.ToLower();
        var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Questions)
                .ThenInclude(q => q.Answers)
                .Include(p => p.Likes)
                .Where(post =>
                    post.Title.Contains(keyword) ||
                    post.Description.Contains(keyword) ||
                    post.Questions.Any(q =>
                        q.Header.ToLower().Contains(keyword) ||
                        q.Description.ToLower().Contains(keyword) ||
                        q.Answers.Any(a => a.Text.Contains(keyword))
                ))
                .Select(post => new PostDTO(post))
                .ToListAsync();
        return Ok(new SearchPostsResponse(posts));
    }


    [HttpGet("[action]")]
    public async Task<ActionResult<SearchPostsResponse>> MyPosts()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (userEmail == null)
            return Unauthorized("Invalid Token, Email not found");
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
            return Unauthorized("Invalid Token, User not in Base");
        //        var posts = await _context.Posts
        //                .Include(p => p.Author)
        //                .Include(p => p.Questions)
        //                .ThenInclude(q => q.Answers)
        //                .Include(p => p.Likes)
        //                .Where(post => post.Author == user)
        //                .Select(post => new PostDTO(post))
        //                .ToListAsync();
        //        return Ok(new SearchPostsResponse(posts));

        return await UserPosts(user.Id);
    }



    [HttpGet("[action]")]
    public async Task<ActionResult<SearchPostsResponse>> UserPosts(string id)
    {
        var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Questions)
                .ThenInclude(q => q.Answers)
                .Include(p => p.Likes)
                .Where(post => post.Author.Id == id)
                .Select(post => new PostDTO(post))
                .ToListAsync();
        return Ok(new SearchPostsResponse(posts));
    }

    // TODO: Add Request and response Schema
    // TODO: Implement
    [HttpPost("[action]")]
    public async Task<ActionResult> MakePost(MakePostRequest request)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (userEmail == null)
            return Unauthorized("Invalid Token, Email not found");
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
            return Unauthorized("Invalid Token, User not in Base");
        _context.Posts.Add(Post.Create(user, request.post));
        await _context.SaveChangesAsync();
        return Ok("Post was saved");
    }
}

