using OpenAI;
using OpenAI.Chat;

public class AIService
{
  private readonly OpenAIClient _client;
  public int StatusCode { get; private set; }

  public AIService(IConfiguration configuration)
  {
    var apiKey = configuration["OpenAI:ApiKey"]
                 ?? throw new InvalidOperationException("OpenAI:ApiKey is not set");
    _client = new OpenAIClient(apiKey);
  }

  public async Task<string> AskAI(string question)
  {
    ChatClient client = _client.GetChatClient("gpt-5-mini");

    try
    {
      ChatCompletion completion = await client.CompleteChatAsync(question);
      StatusCode = StatusCodes.Status200OK;
      return completion.Content[0].Text;
    }
    catch (Exception e)
    {
      StatusCode = StatusCodes.Status400BadRequest;
      return e.Message;
    }
  }
}