using Domain.Entities;

namespace Application.Helpers;

public static class AreaHelper
{
    public static bool IsSupportedArea(string state, string county)
    {
        foreach (var area in GetSupportedAreas())
        {
            if (area.State == state && (area.Counties.Contains(county) || county == "All"))
            {
                return true;
            }
        }
        return false;
    }

    public static List<Area> GetSupportedAreas()
    {
        // Supported areas
        List<Area> supportedAreas = new List<Area>()
        {
            new Area()
            {
                State = "PA",
                Counties = new List<string>()
                {
                    "Berks"
                }
            }
        };

        return supportedAreas;
    }
}