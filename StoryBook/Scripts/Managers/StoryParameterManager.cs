#pragma warning disable CS8602
namespace StoryBook.Scripts;

using System.Xml;

public class StoryParameterManager
{
    #region AVAILABLE PARAMETERS

    private static List<StoryParameterData> s_ListAvailableParameters = [];

    public static void CreateAvailableParameters()
    {
        if (s_ListAvailableParameters.Count > 0)
        {
            return;
        }

        string rootpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");

        // We are in debug mode (local environment)
        if (rootpath.Contains(":\\"))
        {
            rootpath = rootpath.Replace("bin\\Debug\\net9.0\\", "");
        }

        //Load the XML file in XmlDocument.
        XmlDocument doc = new XmlDocument();
        doc.Load(string.Concat(rootpath, "/XML/StoryParameters.xml"));

        //Loop through the selected Nodes.
        foreach (XmlNode sectionNode in doc.SelectNodes("/StoryParameters/Section"))
        {
            StoryParameterData data = new StoryParameterData();

            data.SectionName = sectionNode.Attributes["name"].Value;
            data.SectionId = int.Parse(sectionNode.Attributes["id"].Value);
            data.IsMultiSelection = bool.Parse(sectionNode.Attributes["isMultiSelection"].Value);

            List<string> parameters = [];

            foreach (XmlNode parameterNode in sectionNode.ChildNodes)
            {
                parameters.Add(parameterNode.InnerText);
            }

            data.Parameters = parameters;
            s_ListAvailableParameters.Add(data);
        }
    }

    public static List<StoryParameterData> GetAvailableParameters()
    {
        return s_ListAvailableParameters;
    }

    public static List<string>? GetAvailableParameters(int sectionId)
    {
        foreach (StoryParameterData data in s_ListAvailableParameters)
        {
            if (data.SectionId == sectionId)
            {
                return data.Parameters;
            }
        }

        return null;
    }

    private static string? GetSectionName(int sectionId)
    {
        foreach (StoryParameterData data in s_ListAvailableParameters)
        {
            if (data.SectionId == sectionId)
            {
                return data.SectionName;
            }
        }

        return null;
    }

    private static bool IsMultiSelection(int sectionId)
    {
        foreach (StoryParameterData data in s_ListAvailableParameters)
        {
            if (data.SectionId == sectionId)
            {
                return data.IsMultiSelection;
            }
        }

        return false;
    }

    #endregion

    #region SELECTED PARAMETERS

    private static List<StoryParameterData> s_ListSelectedParameters = [];

    public static List<StoryParameterData> GetSelectedParameters()
    {
        return s_ListSelectedParameters;
    }

    public static void ClearSelectedParameters()
    {
        s_ListSelectedParameters.Clear();
    }

    public static void ToggleSelectedParameter(int sectionId, string paramName, bool isMultiSelection, bool enable)
    {
        CreateSelectedParameterSection(sectionId);

        for (int i = 0; i < s_ListSelectedParameters.Count; i++)
        {
            if (s_ListSelectedParameters[i].SectionId == sectionId)
            {
                StoryParameterData data = s_ListSelectedParameters[i];

                if ((data != null) && (data.Parameters != null))
                {
                    List<string> parameters = data.Parameters;

                    if (enable)
                    {
                        if (!isMultiSelection)
                        {
                            parameters.Clear();
                        }

                        if (!parameters.Contains(paramName))
                        {
                            parameters.Add(paramName);
                        }
                    }
                    else
                    {
                        parameters.Remove(paramName);
                    }

                    data.Parameters = parameters;
                    s_ListSelectedParameters[i] = data;
                }

                break;
            }
        }
    }

    private static void CreateSelectedParameterSection(int sectionId)
    {
        for (int i = 0; i < s_ListSelectedParameters.Count; i++)
        {
            if (s_ListSelectedParameters[i].SectionId == sectionId)
            {
                // already exists
                return;
            }
        }

        StoryParameterData data = new StoryParameterData();

        data.SectionId = sectionId;
        data.SectionName = "";
        data.IsMultiSelection = IsMultiSelection(sectionId);
        data.Parameters = [];

        string? templateSectionName = GetSectionName(sectionId);

        if (templateSectionName != null)
        {
            data.SectionName = templateSectionName;
        }

        s_ListSelectedParameters.Add(data);
    }

    #endregion
}