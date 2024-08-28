using System.Globalization;

namespace Web.Services.UserPreferences;

public class UserPreferences
{
    public static readonly List<string> PrimaryColors = new()
    {
        "#2d4275",
        "#6A1B9A",
        "#4CAF50" ,
        "#FF9800",
        "#F44336",
        "#FF69B4" 
    };

    public static readonly List<string> DarkPrimaryColors = new()
    {
        "#0077b6",
        "#a541be",
        "#388E3C",
        "#FB8C00",
        "#ca322d",
        "#cf2d86",
    };

    /// <summary>
    ///     Set the direction layout of the docs to RTL or LTR. If true RTL is used
    /// </summary>
    public bool RightToLeft { get; set; }

    /// <summary>
    ///     If true DarkTheme is used. LightTheme otherwise
    /// </summary>
    public bool IsDarkMode { get; set; }

    public string PrimaryColor { get; set; } = "#2d4275";

    public string DarkPrimaryColor { get; set; } = "#8b9ac6";

    public string PrimaryDarken => AdjustBrightness(PrimaryColor, 0.8);

    public string PrimaryLighten => AdjustBrightness(PrimaryColor, 0.7);

    private string AdjustBrightness(string hexColor, double factor)
    {
        if (hexColor.StartsWith("#")) hexColor = hexColor.Substring(1);

        if (hexColor.Length != 6) throw new ArgumentException("Invalid hex color code. It must be 6 character long");

        var r = int.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);

        var g = int.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);

        var b = int.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);

        var newR = (int)Math.Clamp(r * factor, 0, 255);

        var newG = (int)Math.Clamp(g * factor, 0, 255);

        var newB = (int)Math.Clamp(b * factor, 0, 255);

        return $"#{newR:X2}{newG:X2}{newB:X2}";
    }
}
