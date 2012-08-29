using System;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Represents the different themes for the detail menu.
    /// </summary>
    [Flags]
    public enum CommandActions
    {
        Add = 1,
        Edit = 2,
        Delete = 4,
        Update = 8,
        Cancel = 16,
        Select = 32,
        Insert = 64,
        Close = 128,
        Search = 256
    }

    /// <summary>
    /// Represents the different themes for the detail menu.
    /// </summary>
    [Serializable]
    public enum DetailMenuTheme
    {
        /// <summary>
        /// The standard text theme.
        /// </summary>
        Standard = 0,

        /// <summary>
        /// The reflective theme.
        /// </summary>
        Reflective = 1,

        /// <summary>
        /// The text theme on which the name and description of the item are displayed side by side.
        /// </summary>
        SideBySide = 2,

        /// <summary>
        /// The decorated theme.
        /// </summary>
        Decorated = 3,

        /// <summary>
        /// The new modern look.
        /// </summary>
        Modern = 4
    }

    /// <summary>
    /// Represents the different sizes of icon.
    /// </summary>
    public enum IconSize
    {
        /// <summary>
        /// The size is not set.
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// The size is 96 x 96 pixels.
        /// </summary>
        Big = 96,

        /// <summary>
        /// The size is 64 x 64 pixels.
        /// </summary>
        Large = 64,

        /// <summary>
        /// The size is 48 x 48 pixels.
        /// </summary>
        Normal = 48,

        /// <summary>
        /// The size is 32 x 32 pixels.
        /// </summary>
        Small = 32,

        /// <summary>
        /// The size is 16 x 16 pixels.
        /// </summary>
        Smaller = 16,

        /// <summary>
        /// The size is 40 x 40 pixels.
        /// </summary>
        x40 = 40,

        /// <summary>
        /// The size is 80 x 80 pixels.
        /// </summary>
        x80 = 80
    }

    /// <summary>
    /// Represents the standard date ranges.
    /// </summary>
    [Serializable]
    public enum StandardDateRange
    {
        /// <summary>
        /// Previous 365 days.
        /// </summary>
        Rolling365Days,

        /// <summary>
        /// Previous 90 days.
        /// </summary>
        Rolling90Days,

        /// <summary>
        /// Previous 30 days.
        /// </summary>
        Rolling30Days,

        /// <summary>
        /// Last fiscal year.
        /// </summary>
        LastFiscalYear,

        /// <summary>
        /// Last year.
        /// </summary>
        LastYear,

        /// <summary>
        /// Last month.
        /// </summary>
        LastMonth,

        /// <summary>
        /// Last week.
        /// </summary>
        LastWeek,

        /// <summary>
        /// Any date range.
        /// </summary>
        Custom,

        /// <summary>
        /// Today.
        /// </summary>
        Today,

        /// <summary>
        /// This week.
        /// </summary>
        ThisWeek,

        /// <summary>
        /// This month.
        /// </summary>
        ThisMonth,

        /// <summary>
        /// This year.
        /// </summary>
        ThisYear,

        /// <summary>
        /// This fiscal year.
        /// </summary>
        ThisFiscalYear,

        /// <summary>
        /// Next week.
        /// </summary>
        NextWeek,

        /// <summary>
        /// Next month.
        /// </summary>
        NextMonth,

        /// <summary>
        /// Next year.
        /// </summary>
        NextYear,

        /// <summary>
        /// Next fiscal year.
        /// </summary>
        NextFiscalYear,

        /// <summary>
        /// Next 30 days.
        /// </summary>
        Next30Days,

        /// <summary>
        /// Next 90 days.
        /// </summary>
        Next90Days,

        /// <summary>
        /// Next 365 days.
        /// </summary>
        Next365Days
    }

    /// <summary>
    /// Represents the different parts of StandardDateRange enumeration.
    /// </summary>
    [Flags]
    public enum StandardDateRangeParts
    {
        /// <summary>
        /// Empty part.
        /// </summary>
        None = 0,

        /// <summary>
        /// Custom part.
        /// </summary>
        Custom = 1,

        /// <summary>
        /// The part related to the past.
        /// </summary>
        Past = 2,

        /// <summary>
        /// The part related to the present.
        /// </summary>
        Present = 4,

        /// <summary>
        /// The part related to the future.
        /// </summary>
        Future = 8
    }

    /// <summary>
    /// Specifies the validation data types used by the validator of the Micajah.Common.WebControls.TextBox control.
    /// </summary>
    public enum CustomValidationDataType
    {
        /// <summary>
        /// A string data type. The value is treated as a System.String.
        /// </summary>
        String = 0,

        /// <summary>
        /// A 32-bit signed integer data type. The value is treated as a System.Int32.
        /// </summary>
        Integer = 1,

        /// <summary>
        /// A double precision floating point number data type. The value is treated as a System.Double.
        /// </summary>
        Double = 2,

        /// <summary>
        /// A date data type. Only numeric dates are allowed. The time portion cannot be specified.
        /// </summary>
        Date = 3,

        /// <summary>
        /// A monetary data type. The value is treated as a System.Decimal. However, currency and grouping symbols are still allowed.
        /// </summary>
        Currency = 4,

        /// <summary>
        /// A regular expression data type. The value is treated as a regular expression.
        /// </summary>
        RegularExpression = 5
    }

    /// <summary>
    /// Specifies the different captions of update button.
    /// </summary>
    public enum UpdateButtonCaptionType
    {
        /// <summary>
        /// Save caption.
        /// </summary>
        Save = 0,

        /// <summary>
        /// Send caption.
        /// </summary>
        Send = 1
    }

    /// <summary>
    /// Specifies the different captions of insert button.
    /// </summary>
    public enum InsertButtonCaptionType
    {
        /// <summary>
        /// Create caption.
        /// </summary>
        Create = 0,

        /// <summary>
        /// Send caption.
        /// </summary>
        Send = 1
    }

    /// <summary>
    /// Specifies the different captions of delete button.
    /// </summary>
    public enum DeleteButtonCaptionType
    {
        /// <summary>
        /// Delete caption.
        /// </summary>
        Delete = 0,

        /// <summary>
        /// Remove caption.
        /// </summary>
        Remove = 1
    }

    /// <summary>
    /// Represents the different modes in which a close button is shown.
    /// </summary>
    public enum CloseButtonVisibilityMode
    {
        /// <summary>
        /// The button isn't shown.
        /// </summary>
        None = 0,

        /// <summary>
        /// The button is shown always.
        /// </summary>
        Always = 1,

        /// <summary>
        /// The button is shown in read-only mode.
        /// </summary>
        ReadOnly = 2,

        /// <summary>
        /// The button is shown in edit mode.
        /// </summary>
        Edit = 3,

        /// <summary>
        /// The button is shown in insert mode.
        /// </summary>
        Insert = 4
    }

    /// <summary>
    /// Represents the different types of DatePicker control.
    /// Defines how to display and validate value in the control.
    /// </summary>
    public enum DatePickerType
    {
        /// <summary>
        /// Displays simple textbox and validates entered value on date data type.
        /// </summary>
        Date,

        /// <summary>
        /// Displays simple textbox and validates entered value on DateTime data type.
        /// </summary>
        DateTime,

        /// <summary>
        /// Displays simple textbox and validates entered value on time data type.
        /// </summary>
        Time,

        /// <summary>
        /// Displays date picker and validates entered value on date data type.
        /// </summary>
        DatePicker,

        /// <summary>
        /// Displays date and time selection picker, validates entered value on DateTime data type.
        /// </summary>
        DateTimePicker,

        /// <summary>
        /// Displays time selection picker and validates entered value on time data type.
        /// </summary>
        TimePicker,
    }

    /// <summary>
    /// Represents the different types of text box field.
    /// Defines how to display and validate value in the text box field control.
    /// </summary>
    public enum TextFieldType
    {
        /// <summary>
        /// As simple textbox.
        /// </summary>
        Text,

        /// <summary>
        /// Display simple textbox and validate entered value on integer data type.
        /// </summary>
        Integer,

        /// <summary>
        /// Display simple textbox and validate entered value on numeric (double) data type.
        /// </summary>
        Double,

        /// <summary>
        /// Display simple textbox and validate entered value on currency data type.
        /// </summary>
        Currency,

        /// <summary>
        /// Display simple textbox and validate entered value on date data type.
        /// </summary>
        Date,

        /// <summary>
        /// Display simple textbox and validate entered value on DateTime data type.
        /// </summary>
        DateTime,

        /// <summary>
        /// Display simple textbox and validate entered value on time data type.
        /// </summary>
        Time,

        /// <summary>
        /// Display simple textbox and validate entered value on date data type.
        /// </summary>
        DatePicker,

        /// <summary>
        /// Display simple textbox and time selection, validate entered value on DateTime data type.
        /// </summary>
        DateTimePicker,

        /// <summary>
        /// Display time selection comboboxes and validate entered value on time data type.
        /// </summary>
        TimePicker,

        /// <summary>
        /// Display simple textbox and check on valid email address.
        /// </summary>
        Email,

        /// <summary>
        /// Display simple textbox and check on valid phone number.
        /// </summary>
        PhoneNumber,

        /// <summary>
        /// Display simple textbox and check on valid zip code.
        /// </summary>
        ZipCode,

        /// <summary>
        /// Display simple textbox and check on valid url.
        /// </summary>
        InternetUrl,

        /// <summary>
        /// Display simple textbox in a secure mode.
        /// </summary>
        Password,

        /// <summary>
        /// Display text area.
        /// </summary>
        TextArea
    }

    /// <summary>
    /// Represents the different types indicating what text will be returned as value of masked text box.
    /// </summary>
    public enum MaskedTextType
    {
        /// <summary>
        /// Returns the text only of the control.
        /// </summary>
        Text,

        /// <summary>
        /// Returns the text of the control including any literals.
        /// </summary>
        TextWithLiterals,

        /// <summary>
        /// Returns the text of the control including any prompt characters.
        /// </summary>
        TextWithPrompt,

        /// <summary>
        /// Returns the text of the control including any literals and promt characters.
        /// </summary>
        TextWithPromptAndLiterals,
    }

    /// <summary>
    /// Represents the different positions of the submenu on the page.
    /// </summary>
    public enum SubmenuPosition
    {
        /// <summary>
        /// The left submenu.
        /// </summary>
        Left,

        /// <summary>
        /// The top submenu.
        /// </summary>
        Top
    }

    /// <summary>
    /// Represents the different type of the item in submenu.
    /// </summary>
    public enum SubmenuItemType
    {
        /// <summary>
        /// The link.
        /// </summary>
        Link = 0,

        /// <summary>
        /// The button.
        /// </summary>
        Button = 1,

        /// <summary>
        /// The image button.
        /// </summary>
        ImageButton = 2
    }

    /// <summary>
    /// Standard Color Schemes
    /// </summary>
    public enum ColorScheme
    {
        /// <summary>
        /// Default color scheme
        /// </summary>
        TanGray,

        /// <summary>
        /// Blue scheme
        /// </summary>
        Blue,

        /// <summary>
        /// Brown scheme
        /// </summary>
        Brown,

        /// <summary>
        /// Gray scheme
        /// </summary>
        Gray,

        /// <summary>
        /// Green scheme
        /// </summary>
        Green,

        /// <summary>
        /// Yellow scheme
        /// </summary>
        Yellow,

        /// <summary>
        /// Red scheme
        /// </summary>
        Red,

        /// <summary>
        /// Silver scheme
        /// </summary>
        Silver,

        /// <summary>
        /// White scheme
        /// </summary>
        White
    }

    /// <summary>
    /// Represents the different types of the message for the notice box.
    /// </summary>
    [Serializable]
    public enum NoticeMessageType
    {
        /// <summary>
        /// The error message.
        /// </summary>
        Error = 0,

        /// <summary>
        /// The information message which is displayed after successfully finished operation.
        /// </summary>
        Success = 1,

        /// <summary>
        /// The warning message.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// The information message.
        /// </summary>
        Information = 3
    }

    /// <summary>
    /// Represents the different sizes of the notice message box.
    /// </summary>
    [Serializable]
    public enum NoticeMessageBoxSize
    {
        /// <summary>
        /// Normal
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Small
        /// </summary>
        Small = 1
    }

    /// <summary>
    /// Navigation buttons styles for Custom Pager
    /// </summary>
    [Serializable]
    public enum PagerButtonsStyle
    {
        /// <summary>
        /// Show navigation buttons as images
        /// </summary>
        Image,

        /// <summary>
        /// Show navigation buttons as links
        /// </summary>
        Link
    }

    /// <summary>
    /// Mode enumeration for Custom Pager
    /// </summary>
    [Serializable]
    public enum PagingMode
    {
        /// <summary>
        /// Show current page and allow select another page using TextBox 
        /// </summary>
        TextBox,

        /// <summary>
        /// Show current page and allow select another page using DropDown list
        /// </summary>
        DropDownList
    }

    /// <summary>
    /// Represents the different toolbar configurations of the Micajah.Common.WebControls.TextEditor control.
    /// </summary>
    [Serializable]
    public enum TextEditorToolBarConfiguration
    {
        Standard,
        Lite
    }

    /// <summary>
    /// Defines the different rendering modes for the Micajah.Commmon.WebControls.CheckBox control.
    /// </summary>
    [Serializable]
    public enum CheckBoxRenderingMode
    {
        /// <summary>
        /// The control is rendered as standard checkbox.
        /// </summary>
        CheckBox,

        /// <summary>
        /// The control is rendered as on-off switch.
        /// </summary>
        OnOffSwitch
    }
}