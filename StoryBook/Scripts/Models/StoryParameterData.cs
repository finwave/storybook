namespace StoryBook.Scripts;

public class StoryParameterData
{
  public string? SectionName { get; set; }
  public int SectionId { get; set; }
  public bool IsMultiSelection { get; set; }
  public List<string>? Parameters { get; set; }
  public List<string>? ImageUrls { get; set; }

  public EStoryParamType ParameterType { get; private set; }

  public void Init()
  {
    string sectionName = SectionName?.ToLower() ?? string.Empty;

    if (sectionName.Contains("theme"))
    {
      ParameterType = EStoryParamType.Theme;
    }
    else if (sectionName.Contains("location"))
    {
      ParameterType = EStoryParamType.Location;
    }
    else if (sectionName.Contains("animal"))
    {
      ParameterType = EStoryParamType.Animal;
    }
    else
    {
      throw new InvalidOperationException($"Unknown section name: {SectionName}");
    }
  }
}