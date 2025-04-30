using System.ComponentModel.DataAnnotations;

namespace api.Common.Data;

public class VocabularyMeaning
{
    public int Id { get; set; }
    public int VocabularyId { get; set; }
    [Required]
    [MaxLength(30)]
    public string PartOfSpeech { get; set; } = null!;
    [Required]
    [MaxLength(255)]
    public string Meaning { get; set; } = null!;
    [MaxLength(255)]
    public string? Ipa { get; set; }
    [MaxLength(255)]
    public string? Pronunciation { get; set; }
    [MaxLength(255)]
    public string? Example { get; set; }
    [MaxLength(255)]
    public string? Note { get; set; }
    [MaxLength(500)]
    public string? Usage { get; set; }
    public Vocabulary Vocabulary { get; set; } = null!;
}