using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AIController : ControllerBase
{
  private readonly AIService _aiService;

  public AIController(AIService aiService)
  {
    _aiService = aiService;
  }

  [HttpGet("chat")]
  public async Task<IActionResult> Ask(string question)
  {
    var result = await _aiService.AskAI(question);
    int statusCode = _aiService.StatusCode;
    return StatusCode(statusCode, result);
  }
}