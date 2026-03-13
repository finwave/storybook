namespace StoryBook.Scripts;

public class StoryGenerateManager
{
  private enum EState
  {
    Invalid,
    Starting,
    Generating,
    Finished
  }

  private static EState s_CurrentState;
  private static string s_GeneratedText = string.Empty;

  public static bool IsValidState
  {
    get { return s_CurrentState != EState.Invalid; }
  }

  public static bool IsStarting
  {
    get { return s_CurrentState == EState.Starting; }
  }

  public static bool IsGenerating
  {
    get { return s_CurrentState == EState.Generating; }
  }

  public static bool IsFinished
  {
    get { return s_CurrentState == EState.Finished; }
  }

  public static void Reset()
  {
    SwitchState(EState.Invalid);
  }

  public static void Activate()
  {
    SwitchState(EState.Starting);
  }

  public static void Generate()
  {
    SwitchState(EState.Generating);
  }

  public static void Finish()
  {
    SaveStory();
    SwitchState(EState.Finished);
  }

  public static void SetStoryText(string text)
  {
    s_GeneratedText = text;
  }

  public static void SetStoryImages()
  {
  }

  public static void SetStoryAudio()
  {
  }

  public static string GetStoryText()
  {
    return s_GeneratedText;
  }

  private static void SwitchState(EState nextState)
  {
    switch (nextState)
    {
      case EState.Generating:
        ClearResults();
        break;
    }

    s_CurrentState = nextState;
  }

  private static void ClearResults()
  {
    s_GeneratedText = string.Empty;
  }

  private static void SaveStory()
  {
    //TODO: AWS logic (saving story text, image files and audio files)
  }
}