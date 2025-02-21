using System.ComponentModel.DataAnnotations;
namespace Backend.Models;
public class Post
{
    public static Post Create(AuthUser User, PostDTO post)
    {
        return new Post()
        {
            Author = User,
            Title = post.Title,
            Description = post.Description,
            Questions = post.Questions.Select(q => Question.Create(q)).ToList(),
            Likes = []
        };
    }
    public static Post Create(AuthUser User, PostRequestData post)
    {
        return new Post()
        {
            Author = User,
            Title = post.Title,
            Description = post.Description,
            Questions = post.Questions.Select(q => Question.Create(q)).ToList(),
            Likes = []
        };
    }
    [Key]
    [Required]
    public int Id { get; set; }
    required public AuthUser Author { get; set; }
    required public string Title { get; set; }
    required public string Description { get; set; }
    required virtual public ICollection<Question> Questions { get; set; }
    required virtual public ICollection<AuthUser> Likes { get; set; }
}

public class PostDTO(Post post)
{
    public int Id { get; set; } = post.Id;
    public UserDTO Author { get; set; } = new UserDTO(post.Author);
    public string Title { get; set; } = post.Title;
    public string Description { get; set; } = post.Description;
    public ICollection<QuestionDTO> Questions { get; set; } = post.Questions.Select(q => new QuestionDTO(q)).ToList();
    public ICollection<UserDTO> Likes { get; set; } = post.Likes.Select(l => new UserDTO(l)).ToList();
}

public record PostRequestData(string Title, string Description, ICollection<QuestionRequestData> Questions);
