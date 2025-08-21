namespace StoryBook.Scripts;

public class CreateStoryData
{
    private static List<StoryParameterData> s_ListSelectedParameters = [];

    public static void Reset()
    {
        s_ListSelectedParameters.Clear();
    }

    public static List<StoryParameterData> GetSelectedParameters()
    {
        return s_ListSelectedParameters;
    }

    public static void ToggleParameter(int sectionId, string paramName, bool isMultiSelection, bool enable)
    {
        CreateParameterDataStruct(sectionId);

        for (int i = 0; i < s_ListSelectedParameters.Count; i++)
        {
            if (s_ListSelectedParameters[i].sectionId == sectionId)
            {
                StoryParameterData data = s_ListSelectedParameters[i];
                List<string> parameters = data.parameters;

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

                data.parameters = parameters;
                s_ListSelectedParameters[i] = data;

                return;
            }
        }
    }

    private static void CreateParameterDataStruct(int sectionId)
    {
        for (int i = 0; i < s_ListSelectedParameters.Count; i++)
        {
            if (s_ListSelectedParameters[i].sectionId == sectionId)
            {
                // already exists
                return;
            }
        }

        StoryParameterData data = new StoryParameterData();

        data.sectionId = sectionId;
        data.sectionName = "";
        data.isMultiSelection = StoryParameters.IsMultiSelection(sectionId);
        data.parameters = [];

        string? templateSectionName = StoryParameters.GetSectionName(sectionId);

        if (templateSectionName != null)
        {
            data.sectionName = templateSectionName;
        }

        s_ListSelectedParameters.Add(data);
    }
}
