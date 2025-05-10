namespace api.VocabularyDomain.DTOs;

public record VocabularyDTO
{
    public int Id { get; init; }
    public string Word { get; init; } = null!;
    public int UserId { get; init; }
    public List<VocabularyMeaningDTO> Meanings { get; init; } = [];
}

public record CreateVocabularyRequest
{
    public required string Word { get; init; }
    public required IEnumerable<VocabularyMeaningDTO> Meanings { get; init; }
}

public record UpdateVocabularyRequest
{
    public required int Id { get; init; }
    public required string Word { get; init; }
    public required List<VocabularyMeaningDTO> Meanings { get; init; }
}

public record VocabularyMeaningDTO
{
    public int? Id { get; init; } // optional for update
    public int? VocabularyId { get; init; } // optional for update
    public required string PartOfSpeech { get; init; }
    public required string Meaning { get; init; }
    public string? Ipa { get; init; }
    public string? Pronunciation { get; init; }
    public string? Example { get; init; }
    public string? Note { get; init; }
    public string? Usage { get; init; }
}

// DTO cho import
public record ImportVocabularyItem
{
    public string? Word { get; set; }
    public List<ImportVocabularyMeaning>? Meaning { get; set; }
}
public record ImportVocabularyMeaning
{
    public string? Definition { get; set; }
    public string? PartOfSpeech { get; set; }
    public string? Ipa { get; set; }
    public string? Pronunciation { get; set; }
    public string? Note { get; set; }
    public string? ExampleSentence { get; set; }
}
public record ImportResultDto
{
    public int Inserted { get; set; }
    public int Failed { get; set; }
}