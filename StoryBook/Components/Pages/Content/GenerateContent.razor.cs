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
    string question = GetAiChatInput();

    if (question == string.Empty)
    {
      statusCode = HttpStatusCode.BadRequest;
      StateHasChanged();
      return;
    }

    string requestUri = "/api/ai/chat?question=" + question;
    using HttpResponseMessage response = await httpClient.GetAsync(requestUri);
    string responseContent = await response.Content.ReadAsStringAsync();

    if (response.IsSuccessStatusCode && StoryGenerateManager.IsGenerating)
    {
      StoryGenerateManager.SetStoryText(responseContent);
      StoryGenerateManager.Finish();
      OnBrowse();
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

      StateHasChanged();
    }
  }

  private string GetAiChatInput()
  {
    string inputTemplate = @LocaleGenerate["StoryTextInput"].ToString();

    List<StoryParameterData> listSelectedParameters = StoryParameterManager.GetSelectedParameters();
    string[] textParameters = new string[3];

    foreach (StoryParameterData parameterData in listSelectedParameters)
    {
      EStoryParamType paramType = parameterData.ParameterType;
      List<string>? paramNames = parameterData.Parameters;

      if ((paramNames != null) && (paramNames.Count > 0))
      {
        string tid = string.Empty;

        if (paramType == EStoryParamType.Theme)
        {
          tid = paramNames[0];
          textParameters[0] = @LocaleParameters[tid].ToString().ToLower();
        }
        else if (paramType == EStoryParamType.Location)
        {
          tid = paramNames[0];
          textParameters[1] = @LocaleParameters[tid].ToString().ToLower();
        }
        else if (paramType == EStoryParamType.Animal)
        {
          string animals = "";

          for (int i = 0; i < paramNames.Count; i++)
          {
            if (i > 0)
            {
              animals += ", ";
            }

            tid = paramNames[i];
            animals += @LocaleParameters[tid].ToString().ToLower();
          }

          textParameters[2] = animals;
        }
      }
    }

    for (int i = 0; i < textParameters.Length; i++)
    {
      if (textParameters[i] == null)
      {
        return string.Empty;
      }
    }

    return TextUtils.SetParameters(inputTemplate, textParameters);
  }
}