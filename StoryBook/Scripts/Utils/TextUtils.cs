namespace StoryBook.Scripts;

using System.Text.RegularExpressions;

public static class TextUtils
{
  private static List<int> m_ParameterNumberList = new List<int>(6);
  private static string[] m_SingleSeparator = new string[1];

  public static string SetParameters(string text, string[] parameters)
  {
    m_ParameterNumberList.Clear();
    char character;

    for (int i = 0; i < text.Length;)
    {
      character = text[i];

      if (character.Equals('%'))
      {
        if ((i + 1) == text.Length)
        {
          break;
        }

        character = text[i + 1];

        if (character.Equals('U'))
        {
          if ((i + 2) == text.Length)
          {
            break;
          }

          character = text[i + 2];

          string temp = "";
          temp += character;

          int number = int.Parse(temp);

          m_ParameterNumberList.Add(number);

          i += 3;
        }
        else
        {
          i += 2;
        }
      }
      else
      {
        i += 1;
      }
    }

    string[] splitText;

    int count = 0;
    string result = "";

    while (true)
    {
      if (count == m_ParameterNumberList.Count)
      {
        result += text;
        break;
      }

      string param = "%U" + m_ParameterNumberList[count].ToString();

      m_SingleSeparator[0] = param;
      splitText = text.Split(m_SingleSeparator, System.StringSplitOptions.None);

      result += splitText[0] + parameters[count];
      text = splitText[1];

      count++;
    }

    text = Regex.Unescape(text);
    return result;
  }
}