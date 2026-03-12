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

  private static EState m_CurrentState;
  private static string m_GeneratedText = string.Empty;

  public static bool IsValidState
  {
    get { return m_CurrentState != EState.Invalid; }
  }

  public static bool IsStarting
  {
    get { return m_CurrentState == EState.Starting; }
  }

  public static bool IsGenerating
  {
    get { return m_CurrentState == EState.Generating; }
  }

  public static bool IsFinished
  {
    get { return m_CurrentState == EState.Finished; }
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

  public static void SetStoryText(string text)
  {
    m_GeneratedText = text;
  }

  public static void SetStoryImages()
  {
  }

  public static void SetStoryAudio()
  {
  }

  public static string GetStoryText()
  {
    return m_GeneratedText;
  }

  public static void Finish()
  {
    SaveStory();
    SwitchState(EState.Finished);
  }

  private static void SwitchState(EState nextState)
  {
    switch (nextState)
    {
      case EState.Generating:
        ClearResults();
        break;
    }

    m_CurrentState = nextState;
  }

  private static void ClearResults()
  {
    m_GeneratedText = string.Empty;
  }

  private static void SaveStory()
  {
    //TODO: AWS logic (saving story text, image files and audio files)
  }
}