using System.ComponentModel.DataAnnotations;

namespace api.Common.Data;

public class Vocabulary
{
    public int Id { get; set; }
    [Required]
    [MaxLength(150)]
    public string Word { get; set; } = null!;
    public ICollection<VocabularyMeaning> Meanings { get; set; } = [];
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}