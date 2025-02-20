using System.ComponentModel.DataAnnotations;
namespace Backend.Models;
public class Post
{
    [Key]
    [Required]
    public int Id { get; set; }
    required public AuthUser Author { get; set; }
    required virtual public ICollection<Question> Questions { get; set; }
    required virtual public ICollection<AuthUser> Likes { get; set; }
}

public class PostDTO(Post post)
{
    public UserDTO Author { get; set; } = new UserDTO(post.Author);
    public ICollection<QuestionDTO> Questions { get; set; } = post.Questions.Select(q => new QuestionDTO(q)).ToList();
    public ICollection<UserDTO> Likes { get; set; } = post.Likes.Select(l => new UserDTO(l)).ToList();
}
