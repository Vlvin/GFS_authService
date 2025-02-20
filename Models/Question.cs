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
    [Key]
    [Required]
    public int Id { get; set; }
    required public QuestionType Type { get; set; }
    required public string Header { get; set; }
    required public string Decription { get; set; }
    required public IList<Answer> Answers { get; set; }
}

public class QuestionDTO(Question question)
{
    public QuestionType Type { get; set; } = question.Type;
    public string Header { get; set; } = question.Header;
    public string Decription { get; set; } = question.Decription;
    public IList<AnswerDTO> Answers { get; set; } = question.Answers.Select(ans => new AnswerDTO(ans)).ToList();
}
