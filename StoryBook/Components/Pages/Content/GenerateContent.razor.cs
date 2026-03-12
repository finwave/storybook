namespace StoryBook.Scripts;

using System.Net;
using Microsoft.AspNetCore.Components;

public partial class GenerateContent : ComponentBase
{
  // IHttpClientFactory set using dependency injection 
  [Inject]
  public required IHttpClientFactory HttpClientFactory { get; set; }

  public string description { get; set; } = "Story is being generated. Please wait...";
  public HttpStatusCode statusCode { get; set; } = HttpStatusCode.OK;

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (StoryGenerateManager.IsStarting)
    {
      await OnGenerateStory();
    }
  }

  private async Task OnGenerateStory()
  {
    StoryGenerateManager.Generate();
    statusCode = HttpStatusCode.OK;

    await Task.Delay(100);

    // Create the HTTP client using the StoryBookAPI named factory
    var httpClient = HttpClientFactory.CreateClient("StoryBookAPI");

    // Perform the GET request and store the response.
    // The parameter in GetAsync specifies the endpoint in the API.
    using HttpResponseMessage response = await httpClient.GetAsync("/api/ai/chat?question=Say 'This is a test.'");
    string responseContent = await response.Content.ReadAsStringAsync();

    if (response.IsSuccessStatusCode && StoryGenerateManager.IsGenerating)
    {
      StoryGenerateManager.SetStoryText(responseContent);
      StoryGenerateManager.Finish();

      description = responseContent;
    }
    // Error has occured
    else if (!response.IsSuccessStatusCode)
    {
      statusCode = response.StatusCode;
      string debugMessage = string.Empty;

      if ((responseContent != null) && (responseContent != string.Empty))
      {
        debugMessage = responseContent;
      }
      else if ((response.ReasonPhrase != null) && (response.ReasonPhrase != string.Empty))
      {
        debugMessage = response.ReasonPhrase;
      }

      if ((debugMessage != null) && (debugMessage != string.Empty))
      {
        await OnDebugMessage(debugMessage);
      }
    }

    StateHasChanged();
  }
}