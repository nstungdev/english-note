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