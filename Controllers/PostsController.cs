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

    // TODO: Fix Proble with eager-loading Posts as List with Lists 
    [HttpGet("[action]")]
    public async Task<ActionResult<AllPostsResponse>> All()
    {

        var posts = _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Questions)
            .ThenInclude(q => q.Answers)
            .Include(p => p.Likes)
            .Select(p => new PostDTO(p))
            // .Where(p => true)
            .ToList(); // ERROR: Happens here on PopulateList
        Console.WriteLine($"Count {posts.Count}");
        return Ok(new AllPostsResponse(posts));
    }

    // TODO: Implement
    // TODO: Fix Proble with eager-loading Posts as List with Lists
    [HttpGet("[action]")]
    public async Task<ActionResult<SearchPostsResponse>> Search(string keyword)
    {
        //var author = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
        //_context.Posts.Add(new Post()
        //{
        //    Author = author,
        //    Questions = [
        //        new Question() {
        //            Type = QuestionType.CHOOSE_ONE,
        //            Header = "First Quest on this portal",
        //            Decription = "Are you strainght... SOSAL?",
        //            Answers = [
        //                "Yes", "No", "OHHH NOOOO YOU FOOLED ME"
        //            ]
        //        },
        //    ],
        //    Likes = []
        //});
        //_context.SaveChanges();
        keyword = keyword.ToLower();
        var posts =
            _context.Posts.Select(post => post).Where(post =>
                post.Questions.Any(q =>
                        q.Header.ToLower().Contains(keyword) ||
                        q.Decription.ToLower().Contains(keyword)
                    )).Include(p => p.Questions).Include(p => p.Likes).ToList();
        Console.WriteLine(posts.GetType());
        return Ok(new SearchPostsResponse(posts));
    }

    // TODO: Add Request and response Schema
    // TODO: Implement
    [HttpPost("[action]")]
    public async Task<ActionResult> MakePost()
    {
        var question = new Question() {
                Type = QuestionType.CHOOSE_ONE,
                Header = "First Quest on this portal",
                Decription = "Are you strainght... SOSAL?",
                Answers = [
                    // "Yes", "No", "OHHH NOOOO YOU FOOLED ME"
                ]
            };
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
        foreach (var ans in new List<string>() {"Yes", "No", "OHHH NOOOO YOU FOOLED ME"})
        {
            var answer = new Answer() { Text = ans, Question = question };
            _context.Answers.Add(answer);
            question.Answers.Add(answer);
        }
        await _context.SaveChangesAsync();
        var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value) ?? throw new Exception("Fuck you nigga");
        _context.Posts.Add(new Post() {
            Author = user,
            Questions = [ question ],
            Likes = []
        });
        await _context.SaveChangesAsync();
        return Ok("Post was saved");
    }
}

