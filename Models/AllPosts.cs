


namespace Backend.Models;
public record AllPostsRequest();
public record AllPostsResponse(ICollection<PostDTO> posts);
