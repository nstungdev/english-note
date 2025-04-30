using api.Common;
using api.Common.Data;
using api.Common.Models;
using api.VocabularyDomain.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace api.VocabularyDomain.Services;

public class VocabularyService(AppDbContext context, ILogger<VocabularyService> logger)
{
    private static readonly string[] ValidPartsOfSpeech = [
        "noun", "verb", "adjective", "adverb", "pronoun", "preposition", "conjunction", "interjection", "determiner", "article"
    ];

    public async Task<ApiResponse> CreateAsync(CreateVocabularyRequest request, int userId)
    {
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
    public async Task<ApiResponse> UpdateVocabularyAsync(UpdateVocabularyRequest request, int userId)
    {
        if (!await IsValidUserIdAsync(userId))
        {
            logger.LogWarning("Invalid userId: {UserId}", userId);
            return ApiResponse.ErrorResponse("Invalid user.", 400);
        }
        var vocabulary = await context.Vocabularies
            .Include(v => v.Meanings)
            .FirstOrDefaultAsync(v => v.Id == request.Id && v.UserId == userId);
        if (vocabulary == null)
        {
            logger.LogWarning("Vocabulary id {Id} not found for user {UserId}", request.Id, userId);
            return ApiResponse.ErrorResponse("Vocabulary not found.", 404);
        }
        foreach (var meaning in request.Meanings)
        {
            if (!IsValidPartOfSpeech(meaning.PartOfSpeech))
            {
                logger.LogWarning("Invalid part of speech: {PartOfSpeech}", meaning.PartOfSpeech);
                return ApiResponse.ErrorResponse($"Invalid part of speech: {meaning.PartOfSpeech}", 400);
            }
        }
        // Normalize and update
        vocabulary.Word = request.Word.Trim().ToLower();
        vocabulary.Meanings = request.Meanings.Select(m => new VocabularyMeaning
        {
            PartOfSpeech = m.PartOfSpeech.Trim().ToLower(),
            Meaning = m.Meaning.Trim(),
            Ipa = m.Ipa?.Trim(),
            Pronunciation = m.Pronunciation?.Trim(),
            Example = m.Example?.Trim(),
            Note = m.Note?.Trim(),
            Usage = m.Usage?.Trim()
        }).ToList();
        await context.SaveChangesAsync();
        logger.LogInformation("Vocabulary id {Id} updated for user {UserId}", request.Id, userId);
        return ApiResponse.SuccessResponse(
            message: "Vocabulary updated successfully.",
            statusCode: 200);
    }
    public async Task<ApiResponse> GetVocabularyAsync(int id, int userId)
    {
        if (!await IsValidUserIdAsync(userId))
        {
            logger.LogWarning("Invalid userId: {UserId}", userId);
            return ApiResponse.ErrorResponse("Invalid user.", 400);
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
    public async Task<ApiResponse> GetVocabulariesAsync(int userId, int page, int pageSize)
    {
        if (!await IsValidUserIdAsync(userId))
        {
            logger.LogWarning("Invalid userId: {UserId}", userId);
            return ApiResponse.ErrorResponse("Invalid user.", 400);
        }
        var vocabularies = await context.Vocabularies
            .Include(v => v.Meanings)
            .Where(v => v.UserId == userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var dtos = vocabularies.Select(vocabulary => new VocabularyDTO
        {
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
        return ApiResponse.SuccessResponse(dtos);
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