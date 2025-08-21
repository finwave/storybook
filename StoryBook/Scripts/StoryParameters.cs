#pragma warning disable CS8602
namespace StoryBook.Scripts;

using System.Xml;

public struct StoryParameterData
{
    public string sectionName;
    public int sectionId;
    public bool isMultiSelection;
    public List<string> parameters;
}

public class StoryParameters
{
    private static List<StoryParameterData> s_ListParameterData = [];

    public static List<StoryParameterData> ParameterData
    {
        get { return s_ListParameterData; }
    }

    public static void Initialize()
    {
        if (s_ListParameterData.Count > 0)
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

            data.sectionName = sectionNode.Attributes["name"].Value;
            data.sectionId = int.Parse(sectionNode.Attributes["id"].Value);
            data.isMultiSelection = bool.Parse(sectionNode.Attributes["isMultiSelection"].Value);

            List<string> parameters = [];

            foreach (XmlNode parameterNode in sectionNode.ChildNodes)
            {
                parameters.Add(parameterNode.InnerText);
            }

            data.parameters = parameters;
            s_ListParameterData.Add(data);
        }
    }

    public static string? GetSectionName(int sectionId)
    {
        foreach (StoryParameterData data in s_ListParameterData)
        {
            if (data.sectionId == sectionId)
            {
                return data.sectionName;
            }
        }

        return null;
    }

    public static List<string>? GetParameters(int sectionId)
    {
        foreach (StoryParameterData data in s_ListParameterData)
        {
            if (data.sectionId == sectionId)
            {
                return data.parameters;
            }
        }

        return null;
    }

    public static bool IsMultiSelection(int sectionId)
    {
        foreach (StoryParameterData data in s_ListParameterData)
        {
            if (data.sectionId == sectionId)
            {
                return data.isMultiSelection;
            }
        }

        return false;
    }
}