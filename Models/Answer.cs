

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;
public class Answer
{
    public static Answer Create(Question quest, AnswerDTO answer)
    {
        return new Answer()
        {
            Question = quest,
            Text = answer.Text
        };
    }
    public static Answer Create(Question quest, AnswerRequestData answer)
    {
        return new Answer()
        {
            Question = quest,
            Text = answer.Text
        };
    }
    [Key]
    public int Id { get; set; }
    required public Question Question { get; set; }
    [Required]
    required public string Text { get; set; }
}

public struct AnswerDTO(string Text)
{
    public string Text { get; set; } = Text;

    public AnswerDTO(Answer answer)
        : this(answer.Text)
    {
    }
};

public record AnswerRequestData(string Text);
