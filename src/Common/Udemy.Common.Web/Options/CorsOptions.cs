namespace Udemy.Common.Web.Options;
public class CorsOptions
{
    public const string SectionName = "Cors";

    public Dictionary<string, CorsPolicyOptions> Policies { get; } = null!;
}

public class CorsPolicyOptions
{
    public const string DefaultSectionName = "Default";

    public string[] Origins { get; set; } = null!;

    public string[] Headers { get; set; } = null!;

    public string[] Methods { get; set; } = null!;
}
