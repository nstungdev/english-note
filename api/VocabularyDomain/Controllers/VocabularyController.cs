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
        var response = await vocabularyService.GetAsync(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetMany([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var response = await vocabularyService.GetManyAsync(page, pageSize);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        var response = await vocabularyService.ImportAsync(file);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("export-json")]
    public async Task<IActionResult> ExportToJson()
    {
        var response = await vocabularyService.ExportAllToJsonAsync();
        if (!response.Success)
        {
            return StatusCode(response.StatusCode, response);
        }
        var json = response.Data as string;
        var fileName = $"vocabulary-{DateTime.Now:dd-MM-yyyy-HH-mm-ss}.json";
        return File(System.Text.Encoding.UTF8.GetBytes(json ?? "[]"), "application/json", fileName);
    }
}