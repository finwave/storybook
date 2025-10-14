namespace StoryBook.Scripts;

public class StoryParameterData
{
    public string? SectionName { get; set; }
    public int SectionId { get; set; }
    public bool IsMultiSelection { get; set; }
    public List<string>? Parameters { get; set; }
}