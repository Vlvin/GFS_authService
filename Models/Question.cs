using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Backend.Models;
public enum QuestionType
{
    EDIT, // self-written answer
    CHOOSE_ONE, // radio-buttons answer
    CHOOSE_MANY, // checkbox answer
};
public class Question
{
    public static Question Create(QuestionDTO question)
    {
        var result = new Question()
        {
            Type = question.Type,
            Header = question.Header,
            Description = question.Description,
            Answers = []
        };
        result.Answers = question.Answers.Select(ans => Answer.Create(result, ans)).ToList();
        return result;
    }
    public static Question Create(QuestionRequestData question)
    {
        var result = new Question()
        {
            Type = question.Type,
            Header = question.Header,
            Description = question.Description,
            Answers = []
        };
        result.Answers = question.Answers.Select(ans => Answer.Create(result, ans)).ToList();
        return result;
    }
    [Key]
    [Required]
    public int Id { get; set; }
    required public QuestionType Type { get; set; }
    required public string Header { get; set; }
    required public string Description { get; set; }
    required public IList<Answer> Answers { get; set; }
}

public class QuestionDTO(Question question)
{
    public QuestionType Type { get; set; } = question.Type;
    public string Header { get; set; } = question.Header;
    public string Description { get; set; } = question.Description;
    public IList<AnswerDTO> Answers { get; set; } = question.Answers.Select(ans => new AnswerDTO(ans)).ToList();
}

public record QuestionRequestData(QuestionType Type, string Header, string Description, ICollection<AnswerRequestData> Answers);
