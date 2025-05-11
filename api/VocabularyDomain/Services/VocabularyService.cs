using api.Common;
using api.Common.Data;
using api.Common.Managers;
using api.Common.Models;
using api.VocabularyDomain.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using api.Common.Constants;
using Sentry.Protocol;

namespace api.VocabularyDomain.Services;

public class VocabularyService(
    AppDbContext context,
    ILogger<VocabularyService> logger,
    IAuthManager authManager)
{
    private static readonly string[] ValidPartsOfSpeech = [
        "noun", "verb", "adjective", "adverb", "pronoun", "preposition", "conjunction", "interjection", "determiner", "article"
    ];
    public async Task<ApiResponse> CreateAsync(CreateVocabularyRequest request)
    {
        if (!authManager.TryGetUserId(out var userId))
        {
            logger.LogWarning(AppMessage.E01);
            return ApiResponse.ErrorResponse(
                message: AppMessage.E01,
                statusCode: 401);
        }

        if (!await IsValidUserIdAsync(userId))
        {
            logger.LogWarning("Invalid userId: {UserId}", userId);
            return ApiResponse.ErrorResponse(
                message: AppMessage.E02,
                statusCode: 400);
        }
        // Normalize word
        var normalizedWord = request.Word.Trim().ToLower();

        // Check for duplicate word
        if (await IsExistingVocabularyAsync(normalizedWord, userId))
        {
            logger.LogWarning("Duplicate word '{Word}' for user {UserId}", normalizedWord, userId);
            return ApiResponse.ErrorResponse(
                message: "Duplicate word.",
                statusCode: 400);
        }
        // Validate meanings
        foreach (var meaning in request.Meanings)
        {
            if (!IsValidPartOfSpeech(meaning.PartOfSpeech))
            {
                logger.LogWarning("Invalid part of speech: {PartOfSpeech}", meaning.PartOfSpeech);
                return ApiResponse.ErrorResponse(
                    message: $"Invalid part of speech: {meaning.PartOfSpeech}",
                    statusCode: 400);
            }
        }
        // Create vocabulary
        var vocabulary = new Vocabulary
        {
            Word = normalizedWord,
            UserId = userId,
            Meanings = [.. request.Meanings.Select(m => new VocabularyMeaning
            {
                PartOfSpeech = m.PartOfSpeech.Trim().ToLower(),
                Meaning = m.Meaning.Trim(),
                Ipa = m.Ipa?.Trim(),
                Pronunciation = m.Pronunciation?.Trim(),
                Example = m.Example?.Trim(),
                Note = m.Note?.Trim(),
                Usage = m.Usage?.Trim()
            })]
        };
        await context.Vocabularies.AddAsync(vocabulary);
        await context.SaveChangesAsync();
        logger.LogInformation("Vocabulary '{Word}' created for user {UserId}", normalizedWord, userId);
        return ApiResponse.SuccessResponse(
            message: "Vocabulary created successfully.",
            statusCode: 201);
    }
    public async Task<ApiResponse> UpdateAsync(UpdateVocabularyRequest request)
    {
        if (!authManager.TryGetUserId(out var userId))
        {
            logger.LogWarning("User not authenticated.");
            return ApiResponse.ErrorResponse(
                message: "User not authenticated.",
                statusCode: 401);
        }

        if (!await IsValidUserIdAsync(userId))
        {
            logger.LogWarning("Invalid userId: {UserId}", userId);
            return ApiResponse.ErrorResponse(
                message: "Invalid user.",
                statusCode: 400);
        }

        var originalVocabulary = await context.Vocabularies
            .Include(v => v.Meanings)
            .FirstOrDefaultAsync(v => v.Id == request.Id && v.UserId == userId);
        if (originalVocabulary == null)
        {
            logger.LogWarning("Vocabulary id {Id} not found for user {UserId}", request.Id, userId);
            return ApiResponse.ErrorResponse(
                message: "Vocabulary not found.",
                statusCode: 404);
        }

        // Normalize word
        var normalizedWord = request.Word.Trim().ToLower();

        // Check if the vocabulary exists if word is changed
        if (request.Word != originalVocabulary.Word && await IsExistingVocabularyAsync(normalizedWord, userId))
        {
            logger.LogWarning("Duplicate word '{Word}' for user {UserId}", normalizedWord, userId);
            return ApiResponse.ErrorResponse(
                message: "Duplicate word.",
                statusCode: 400);
        }

        // Validate meanings
        foreach (var pos in request.Meanings.Select(e => e.PartOfSpeech))
        {
            if (!IsValidPartOfSpeech(pos))
            {
                logger.LogWarning("Invalid part of speech: {PartOfSpeech}", pos);
                return ApiResponse.ErrorResponse(
                    message: $"Invalid part of speech: {pos}",
                    statusCode: 400);
            }
        }

        // Remove meanings that are not in the request
        foreach (var meaning in originalVocabulary.Meanings.Where(m => !request.Meanings.Any(rm => rm.Id == m.Id)).ToList())
        {
            originalVocabulary.Meanings.Remove(meaning); // Loại bỏ khỏi collection để EF nhận biết
        }

        // if meaning id is null, it means it's a new meaning
        // so we need to add it to the vocabulary
        foreach (var meaning in request.Meanings.Where(m => m.Id == null))
        {
            originalVocabulary.Meanings.Add(new VocabularyMeaning
            {
                VocabularyId = originalVocabulary.Id,
                PartOfSpeech = meaning.PartOfSpeech.Trim().ToLower(),
                Meaning = meaning.Meaning.Trim(),
                Ipa = meaning.Ipa?.Trim(),
                Pronunciation = meaning.Pronunciation?.Trim(),
                Example = meaning.Example?.Trim(),
                Note = meaning.Note?.Trim(),
                Usage = meaning.Usage?.Trim()
            });
        }

        // if meaning id is not null, it means it's an existing meaning
        // so we need to update it
        foreach (var meaning in request.Meanings.Where(m => m.Id != null))
        {
            var existingMeaning = originalVocabulary.Meanings.FirstOrDefault(m => m.Id == meaning.Id);
            if (existingMeaning != null)
            {
                existingMeaning.PartOfSpeech = meaning.PartOfSpeech.Trim().ToLower();
                existingMeaning.Meaning = meaning.Meaning.Trim();
                existingMeaning.Ipa = meaning.Ipa?.Trim();
                existingMeaning.Pronunciation = meaning.Pronunciation?.Trim();
                existingMeaning.Example = meaning.Example?.Trim();
                existingMeaning.Note = meaning.Note?.Trim();
                existingMeaning.Usage = meaning.Usage?.Trim();
            }
        }

        // Update the vocabulary word and user id
        originalVocabulary.Word = normalizedWord;

        // Update the vocabulary in the database
        context.Vocabularies.Attach(originalVocabulary);
        context.Entry(originalVocabulary).State = EntityState.Modified;
        // Update the meanings in the database
        foreach (var meaning in originalVocabulary.Meanings)
        {
            if (meaning.Id != 0)
            {
                context.VocabularyMeanings.Attach(meaning);
                context.Entry(meaning).State = EntityState.Modified;
            }
        }

        // Save changes to the database
        await context.SaveChangesAsync();
        logger.LogInformation("Vocabulary '{Word}' updated for user {UserId}", normalizedWord, userId);
        return ApiResponse.SuccessResponse(
            message: "Vocabulary updated successfully.",
            statusCode: 200);
    }
    public async Task<ApiResponse> GetAsync(int id)
    {
        if (!authManager.TryGetUserId(out var userId))
        {
            logger.LogWarning("User not authenticated.");
            return ApiResponse.ErrorResponse(
                message: "User not authenticated.",
                statusCode: 401);
        }

        if (!await IsValidUserIdAsync(userId))
        {
            logger.LogWarning("Invalid userId: {UserId}", userId);
            return ApiResponse.ErrorResponse(
                message: "Invalid user.",
                statusCode: 400);
        }
        var vocabulary = await context.Vocabularies
            .Include(v => v.Meanings)
            .FirstOrDefaultAsync(v => v.Id == id && v.UserId == userId);
        if (vocabulary == null)
        {
            logger.LogWarning("Vocabulary id {Id} not found for user {UserId}", id, userId);
            return ApiResponse.ErrorResponse("Vocabulary not found.", 404);
        }
        var dto = new VocabularyDTO
        {
            Id = vocabulary.Id,
            Word = vocabulary.Word,
            UserId = vocabulary.UserId,
            Meanings = vocabulary.Meanings.Select(m => new VocabularyMeaningDTO
            {
                Id = m.Id,
                VocabularyId = m.VocabularyId,
                PartOfSpeech = m.PartOfSpeech,
                Meaning = m.Meaning,
                Ipa = m.Ipa,
                Pronunciation = m.Pronunciation,
                Example = m.Example,
                Note = m.Note,
                Usage = m.Usage
            }).ToList()
        };
        return ApiResponse.SuccessResponse(dto);
    }
    public async Task<ApiResponse> GetManyAsync(int page, int pageSize)
    {
        if (!authManager.TryGetUserId(out var userId))
        {
            logger.LogWarning("User not authenticated.");
            return ApiResponse.ErrorResponse(
                message: "User not authenticated.",
                statusCode: 401);
        }

        if (!await IsValidUserIdAsync(userId))
        {
            logger.LogWarning("Invalid userId: {UserId}", userId);
            return ApiResponse.ErrorResponse("Invalid user.", 400);
        }
        var totalCount = await context.Vocabularies
            .CountAsync(v => v.UserId == userId);
        var vocabularies = await context.Vocabularies
            .Include(v => v.Meanings)
            .Where(v => v.UserId == userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var dtos = vocabularies.Select(vocabulary => new VocabularyDTO
        {
            Id = vocabulary.Id,
            Word = vocabulary.Word,
            UserId = vocabulary.UserId,
            Meanings = vocabulary.Meanings.Select(m => new VocabularyMeaningDTO
            {
                Id = m.Id,
                VocabularyId = m.VocabularyId,
                PartOfSpeech = m.PartOfSpeech,
                Meaning = m.Meaning,
                Ipa = m.Ipa,
                Pronunciation = m.Pronunciation,
                Example = m.Example,
                Note = m.Note,
                Usage = m.Usage
            }).ToList()
        }).ToList();
        return ApiResponse.SuccessResponse(new PagingDto<VocabularyDTO>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }
    public async Task<ApiResponse> ImportAsync(IFormFile file)
    {
        if (!authManager.TryGetUserId(out var userId))
        {
            logger.LogWarning("User not authenticated.");
            return ApiResponse.ErrorResponse(
                message: "User not authenticated.",
                statusCode: 401);
        }
        if (!await IsValidUserIdAsync(userId))
        {
            logger.LogWarning("Invalid userId: {UserId}", userId);
            return ApiResponse.ErrorResponse(
                message: "Invalid user.",
                statusCode: 400);
        }
        if (file == null || file.Length == 0)
        {
            return ApiResponse.ErrorResponse(
                message: "No file uploaded.",
                statusCode: 400);
        }
        List<ImportVocabularyItem>? items = null;
        try
        {
            using var stream = file.OpenReadStream();
            items = await JsonSerializer.DeserializeAsync<List<ImportVocabularyItem>>(
                stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to parse import file");
            return ApiResponse.ErrorResponse(
                message: "Invalid file format.",
                statusCode: 400);
        }
        if (items == null || items.Count == 0)
        {
            return ApiResponse.ErrorResponse("No data to import.", 400);
        }
        var normalizedWords = items
            .Where(i => !string.IsNullOrWhiteSpace(i.Word))
            .Select(i => i.Word!.Trim().ToLower())
            .Distinct()
            .ToList();
        var existingWords = await context.Vocabularies
            .Where(v => v.UserId == userId && normalizedWords.Contains(v.Word))
            .Select(v => v.Word)
            .ToListAsync();
        var newVocabularies = new List<Vocabulary>();
        int failed = 0;
        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item.Word))
            {
                failed++;
                continue;
            }
            var normalizedWord = item.Word!.Trim().ToLower();
            if (existingWords.Contains(normalizedWord) || newVocabularies.Any(v => v.Word == normalizedWord))
            {
                failed++;
                continue;
            }
            if (item.Meaning == null || item.Meaning.Count == 0
                || item.Meaning.Any(m => string.IsNullOrWhiteSpace(m.PartOfSpeech) || !IsValidPartOfSpeech(m.PartOfSpeech!)))
            {
                failed++;
                continue;
            }
            var vocabulary = new Vocabulary
            {
                Word = normalizedWord,
                UserId = userId,
                Meanings = item.Meaning.Select(m => new VocabularyMeaning
                {
                    PartOfSpeech = m.PartOfSpeech != null ? m.PartOfSpeech.Trim().ToLower() : string.Empty,
                    Meaning = m.Definition != null ? m.Definition.Trim() : string.Empty,
                    Ipa = m.Ipa?.Trim(),
                    Pronunciation = m.Pronunciation?.Trim(),
                    Example = m.ExampleSentence?.Trim(),
                    Note = m.Note?.Trim(),
                }).ToList()
            };
            newVocabularies.Add(vocabulary);
        }
        if (newVocabularies.Count > 0)
        {
            await context.Vocabularies.AddRangeAsync(newVocabularies);
            await context.SaveChangesAsync();
        }
        var result = new ImportResultDto
        {
            Inserted = newVocabularies.Count,
            Failed = failed
        };
        logger.LogInformation("Import completed: {Inserted} inserted, {Failed} failed", result.Inserted, result.Failed);
        return ApiResponse.SuccessResponse(
            data: result,
            message: "Import completed.");
    }
    public async Task<ApiResponse> ExportAllToJsonAsync()
    {
        if (!authManager.TryGetUserId(out var userId))
        {
            logger.LogWarning(AppMessage.E01);
            return ApiResponse.ErrorResponse(
                message: AppMessage.E01,
                statusCode: 401);
        }
        if (!await IsValidUserIdAsync(userId))
        {
            logger.LogWarning("Invalid userId: {UserId}", userId);
            return ApiResponse.ErrorResponse(
                message: "Invalid user.",
                statusCode: 400);
        }
        var vocabularies = await context.Vocabularies
            .Include(v => v.Meanings)
            .Where(v => v.UserId == userId)
            .ToListAsync();
        var exportData = vocabularies.Select(v => new
        {
            v.Word,
            Meanings = v.Meanings.Select(m => new
            {
                m.PartOfSpeech,
                m.Meaning,
                m.Ipa,
                m.Pronunciation,
                m.Example,
                m.Note,
                m.Usage
            }).ToList()
        }).ToList();
        var json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        return ApiResponse.SuccessResponse(json);
    }
    private Task<bool> IsValidUserIdAsync(int userId)
    {
        // check user exists in the database
        return context.Users.AnyAsync(e => e.Id == userId);
    }
    private Task<bool> IsExistingVocabularyAsync(string word, int userId)
    {
        // check if the vocabulary exists in the database
        return context.Vocabularies.AnyAsync(v => v.Word == word && v.UserId == userId);
    }
    private static bool IsValidPartOfSpeech(string partOfSpeech)
    {
        return !string.IsNullOrWhiteSpace(partOfSpeech) &&
            ValidPartsOfSpeech.Contains(partOfSpeech.Trim().ToLower());
    }
}