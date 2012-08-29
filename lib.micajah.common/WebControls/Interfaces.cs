using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Pages;

namespace Micajah.Common.WebControls
{
    public interface IThemeable
    {
        MasterPageTheme Theme { get; set; }
    }

    public interface IValidated : IThemeable
    {
        string ValidationGroup { get; set; }
        string ValidatorInitialValue { get; set; }
        bool Required { get; set; }
        bool ShowRequired { get; set; }
        string ErrorMessage { get; set; }
        bool Enabled { get; set; }
        short TabIndex { get; set; }
    }

    public interface ISpanned
    {
        int ColumnSpan { get; set; }
        bool CreateNewRow { get; set; }
        string HeaderGroup { get; set; }
    }

    public interface ICheckBox : IValidated
    {
        string Text { get; set; }
        TextAlign TextAlign { get; set; }
        bool AutoPostBack { get; set; }
        CheckBoxRenderingMode RenderingMode { get; set; }
        string CheckedText { get; set; }
        string UncheckedText { get; set; }
    }

    public interface IComboBox : IValidated
    {
        bool AllowCustomText { get; set; }
        bool AppendDataBoundItems { get; set; }
        string AutoCompleteSeparator { get; set; }
        bool AutoPostBack { get; set; }
        object DataSource { get; set; }
        string DataTextField { get; set; }
        string DataValueField { get; set; }
        string Description { get; set; }
        Unit DropDownWidth { get; set; }
        bool EnableLoadOnDemand { get; set; }
        string ExternalCallbackPage { get; set; }
        bool IsCallback { get; set; }
        bool IsCaseSensitive { get; set; }
        bool IsEmpty { get; }
        int ItemRequestTimeout { get; set; }
        ITemplate ItemTemplate { get; set; }
        string LoadingMessage { get; set; }
        bool MarkFirstMatch { get; set; }
        int MaxLength { get; set; }
        bool NoWrap { get; set; }
        string PostBackUrl { get; set; }
        string Text { get; set; }
        string Value { get; set; }
        string SelectedValue { get; set; }
    }

    public interface IDatePicker : IValidated
    {
        bool EnableTyping { get; set; }
        DateTime MinDate { get; set; }
        DateTime MaxDate { get; set; }
        DateTime SelectedDate { get; set; }
        string DateFormat { get; set; }
        DatePickerType Type { get; set; }
        bool IsEmpty { get; }
    }

    public interface ITextBox
    {
        TextBoxMode TextMode { get; set; }
        int Columns { get; set; }
        int Rows { get; set; }
        int MaxLength { get; set; }
        bool LengthInfo { get; set; }
        bool ReadOnly { get; set; }
        string Mask { get; set; }
        MaskedTextType TextType { get; set; }
        CustomValidationDataType ValidationType { get; set; }
        string ValidationExpression { get; set; }
        string ValidationErrorMessage { get; set; }
        string MaximumValue { get; set; }
        string MinimumValue { get; set; }
        string EmptyText { get; set; }
    }
}
