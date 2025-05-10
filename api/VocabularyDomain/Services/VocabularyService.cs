using api.Common;
using api.Common.Data;
using api.Common.Managers;
using api.Common.Models;
using api.VocabularyDomain.DTOs;
using Microsoft.EntityFrameworkCore;

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

    public async Task<ApiResponse> GetVocabularyAsync(int id)
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
    public async Task<ApiResponse> GetVocabulariesAsync(int page, int pageSize)
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
        return ApiResponse.SuccessResponse(new PagingDto<VocabularyDTO> {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        });
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