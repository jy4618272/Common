using System;

namespace Micajah.Common.Pages
{
    /// <summary>
    /// Represents the different themes for the master page.
    /// </summary>
    [Serializable]
    public enum MasterPageTheme
    {
        /// <summary>
        /// The theme which uses the gradient effects.
        /// </summary>
        Gradient,

        /// <summary>
        /// The standard text theme.
        /// </summary>
        Standard,

        /// <summary>
        /// The standard text theme the main menu items of which are displayed as tabs.
        /// </summary>
        StandardTabs,

        /// <summary>
        /// The new modern look.
        /// </summary>
        Modern
    }

    /// <summary>
    /// Represents the different colors for the theme of the master page.
    /// </summary>
    [Serializable]
    public enum MasterPageThemeColor
    {
        /// <summary>
        /// The color is not set. To use for a theme that doesn't have colors.
        /// </summary>
        NotSet,

        /// <summary>
        /// The blue color.
        /// </summary>
        Blue,

        /// <summary>
        /// The brown color.
        /// </summary>
        Brown,

        /// <summary>
        /// The gray color.
        /// </summary>
        Gray,

        /// <summary>
        /// The olive color.
        /// </summary>
        Olive,

        /// <summary>
        /// The orange color.
        /// </summary>
        Orange,

        /// <summary>
        /// The red color.
        /// </summary>
        Red,

        /// <summary>
        /// The violet color.
        /// </summary>
        Violet,

        /// <summary>
        /// The yellow color.
        /// </summary>
        Yellow
    }
}
