using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.VocabularyDomain.DTOs;
using api.VocabularyDomain.Services;
using System.Security.Claims;

namespace api.VocabularyDomain.Controllers;

[ApiController]
[Route("api/vocabularies")]
[Authorize]
public class VocabularyController(VocabularyService vocabularyService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVocabularyRequest request)
    {
        var response = await vocabularyService.CreateAsync(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateVocabularyRequest request)
    {
        var response = await vocabularyService.UpdateAsync(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var response = await vocabularyService.GetVocabularyAsync(id, userId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetMany([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var response = await vocabularyService.GetVocabulariesAsync(userId, page, pageSize);
        return StatusCode(response.StatusCode, response);
    }
}