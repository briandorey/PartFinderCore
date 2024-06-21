namespace PartFinderCore.Classes;

public class GlobalData
{
    public static SiteData SiteData { get; set; } = default!;
}

public class SiteData
{
    public string DataBasePath { get; set; } = default!;
    public bool RequireLogin { get; set; }
}