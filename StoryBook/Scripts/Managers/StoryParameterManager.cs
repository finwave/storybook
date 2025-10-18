namespace StoryBook.Scripts;

public class StoryParameterManager
{
    private static List<StoryParameterData> s_ListAvailableParameters = [];
    private static List<StoryParameterData> s_ListSelectedParameters = [];

    public static void Initialize()
    {
        // Create available story parameters.
        CreateAvailableParameters();
        // Reset selected story parameters.
        ResetSelectedParameters();
    }

    private static string GetRootPath()
    {
        string rootpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");

        rootpath = rootpath.Replace("bin\\Debug\\net9.0\\", "");
        rootpath = rootpath.Replace("bin/Debug/net9.0/", "");

        return rootpath;
    }

    #region AVAILABLE PARAMETERS

    private static void CreateAvailableParameters()
    {
        if (s_ListAvailableParameters.Count > 0)
        {
            return;
        }

        string rootpath = GetRootPath();

        // get json file path and read the contents
        string jsonFilePath = string.Concat(rootpath, "/json/available_parameters.json");
        string jsonString = File.ReadAllText(jsonFilePath);
        StoryParameterData[]? jsonOutputArray = JsonUtils.FromJson<StoryParameterData>(jsonString);

        if (jsonOutputArray != null)
        {
            for (int i = 0; i < jsonOutputArray.Length; i++)
            {
                StoryParameterData parameterData = jsonOutputArray[i];
                s_ListAvailableParameters.Add(parameterData);
            }
        }
    }

    public static List<StoryParameterData> GetAvailableParameters()
    {
        return s_ListAvailableParameters;
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

    private static string? GetParameterName(int sectionId, int paramIndex)
    {
        foreach (StoryParameterData data in s_ListAvailableParameters)
        {
            if ((data.SectionId == sectionId) && (data.Parameters != null))
            {
                return data.Parameters[paramIndex];
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

    public static List<StoryParameterData> GetSelectedParameters()
    {
        return s_ListSelectedParameters;
    }

    private static void ResetSelectedParameters()
    {
        s_ListSelectedParameters.Clear();

        // select first parameter of each section
        for (int i = 0; i < s_ListAvailableParameters.Count; i++)
        {
            int sectionId = i + 1;
            SelectFirstParameter(sectionId);
        }
    }

    private static void SelectFirstParameter(int sectionId)
    {
        string? firstParamName = GetParameterName(sectionId, 0);

        if (firstParamName != null)
        {
            ToggleSelectedParameter(sectionId, firstParamName, false, true);
        }
    }

    public static bool IsSelectedParameter(int sectionId, string paramName)
    {
        foreach (StoryParameterData data in s_ListSelectedParameters)
        {
            if ((data.SectionId == sectionId) && (data.Parameters != null))
            {
                return data.Parameters.Contains(paramName);
            }
        }

        return false;
    }

    public static int GetSelectedParametersAmount(int sectionId)
    {
        foreach (StoryParameterData data in s_ListSelectedParameters)
        {
            if ((data.SectionId == sectionId) && (data.Parameters != null))
            {
                return data.Parameters.Count;
            }
        }

        return 0;
    }

    public static void ToggleSelectedParameter(int sectionId, string paramName, bool isMultiSelection, bool enable)
    {
        CreateSelectedParameterSection(sectionId);

        for (int i = 0; i < s_ListSelectedParameters.Count; i++)
        {
            StoryParameterData data = s_ListSelectedParameters[i];

            if ((data.SectionId == sectionId) && (data.Parameters != null))
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