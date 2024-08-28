using Web.Services.UserPreferences;

namespace Web.Services.Layout;

public class LayoutService
{
    private readonly IUserPreferenceService _userPreferenceService;
    private bool _systemPreferences;
    public DarkLightMode DarkModeToggle = DarkLightMode.System;

    public LayoutService(IUserPreferenceService userPreferenceService)
    {
        _userPreferenceService = userPreferenceService;
    }

    /// <summary>
    /// Gets or sets the user preferences.
    /// </summary>
    public UserPreferences.UserPreferences UserPreferences { get; private set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the layout is right-to-left.
    /// </summary>
    public bool IsRTL { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the layout is in dark mode.
    /// </summary>
    public bool IsDarkMode { get; private set; }
}
