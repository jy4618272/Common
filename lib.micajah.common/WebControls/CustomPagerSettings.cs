using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Micajah.Common;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Style class for Custom paging
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [ParseChildren(true)]
    public class CustomPagerSettings
    {
        #region Members

        private bool _visible = true;
        private string _titleFirst = "Page:";
        private string _titleLast = "of";
        private string _firstImageUrl = string.Empty;
        private string _firstDisableImageUrl = string.Empty;
        private string _firstText = "<<";
        private string _lastImageUrl = string.Empty;
        private string _lastDisableImageUrl = string.Empty;
        private string _lastText = ">>";
        private string _leftImageUrl = string.Empty;
        private string _leftDisableImageUrl = string.Empty;
        private string _leftText = "<";
        private string _rightImageUrl = string.Empty;
        private string _rightDisableImageUrl = string.Empty;
        private string _rightText = ">";
        private PagingMode _pagerMode = PagingMode.DropDownList;
        private PagerButtonsStyle _buttonsStyle = PagerButtonsStyle.Link;
        private HorizontalAlign _hAlign = HorizontalAlign.NotSet;
        private Unit _width = Unit.Empty;
        private PagerPosition _position = PagerPosition.Bottom;

        #endregion

        #region Properties

        ///<summary>
        ///Gets or sets a value that indicates whether a Custom Pager is rendered as UI on the page.
        ///</summary>
        /// <value>
        /// Defines custom pager visibility state.
        /// </value>
        [DefaultValue(true),
        NotifyParentProperty(true),
        Description("Defines line state visibility for CustomPaging")]
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        ///<summary>
        ///Gets or sets a value that will be displayed as begin title on the pager. 
        ///</summary>
        /// <value>
        /// Begin title naming. Title doesn't show if value is empty. Default value is "Page:" 
        /// </value>
        [DefaultValue("Page:"),
        NotifyParentProperty(true),
        Description("Defines line state naming for CustomPaging")]
        public string FirstTitle
        {
            get { return _titleFirst; }
            set { _titleFirst = value; }
        }

        ///<summary>
        ///Gets or Sets a value that will be displayed after navigation buttons, usually it's number of pages 
        ///</summary>
        /// <value>
        /// Last title naming. Shows pages count with defined title. Doesn't show if property is not defined. Default value is "from".
        /// </value>
        [DefaultValue("of"),
        NotifyParentProperty(true),
        Description("Defines end of a line state for CustomPaging")]
        public string LastTitle
        {
            get { return _titleLast; }
            set { _titleLast = value; }
        }

        ///<summary>
        ///Gets or Sets Url to image for "First" page navigation button.
        ///</summary>
        /// <value>
        /// Url to image for "First" page navigation button. Navigation button doesn't show if Url is empty. Url is empty string by default.
        /// </value>
        [DefaultValue(""),
        NotifyParentProperty(true),
        EditorAttribute(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
        Description("Url of a picture 'first' line state for CustomPaging")]
        public string FirstImageUrl
        {
            get { return _firstImageUrl; }
            set { _firstImageUrl = value; }
        }

        ///<summary>
        ///Gets or Sets Url to image for disabled "First" page navigation button.
        ///</summary>
        /// <value>
        /// Url to image for disabled "First" page navigation button. Navigation button doesn't show if Url is empty. Url is empty string by default.
        /// </value>
        [DefaultValue(""),
        NotifyParentProperty(true),
        EditorAttribute(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
        Description("Url of a disable picture 'first' line state for CustomPaging")]
        public string FirstDisableImageUrl
        {
            get { return _firstDisableImageUrl; }
            set { _firstDisableImageUrl = value; }
        }

        ///<summary>
        ///Gets or Sets Description text for "First" navigation button.
        ///</summary>
        /// <value>
        /// Description text for "First" page navigation button. Button doesn't show if text is empty and ButtonnStyles property defined as Link buttons. Default value is "&lt;&lt;"
        /// </value>
        [DefaultValue("<<"),
        NotifyParentProperty(true),
        Description("Text of 'first' button for CustomPaging")]
        public string FirstText
        {
            get { return _firstText; }
            set { _firstText = value; }
        }

        ///<summary>
        ///Gets or Sets Url to image for "Last" page navigation button.
        ///</summary>
        /// <value>
        /// Url to image for "Last" page navigation button. Navigation button doesn't show if Url is empty. Property value is empty string by default.
        /// </value>
        [DefaultValue(""),
        NotifyParentProperty(true),
        EditorAttribute(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
        Description("Url of a picture 'last' line state for CustomPaging")]
        public string LastImageUrl
        {
            get { return _lastImageUrl; }
            set { _lastImageUrl = value; }
        }

        ///<summary>
        ///Gets or Sets Url to image for disabled "Last" page navigation button.
        ///</summary>
        /// <value>
        /// Url to image for disabled "Last" page navigation button. Navigation button doesn't show if Url is empty. Property value is empty string by default.
        /// </value>
        [DefaultValue(""),
        NotifyParentProperty(true),
        EditorAttribute(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
        Description("Url of a Disable picture 'last' line state for CustomPaging")]
        public string LastDisableImageUrl
        {
            get { return _lastDisableImageUrl; }
            set { _lastDisableImageUrl = value; }
        }

        ///<summary>
        ///Gets or Sets Description text for "Last" page navigation button.
        ///</summary>
        /// <value>
        /// Description text for "Last" page navigation button. Button doesn't show if text is empty and ButtonnStyles property defined as Link buttons. Default value is "&gt;&gt;"
        /// </value>
        [DefaultValue(">>"),
        NotifyParentProperty(true),
        Description("Text of 'last' button for CustomPaging")]
        public string LastText
        {
            get { return _lastText; }
            set { _lastText = value; }
        }

        ///<summary>
        ///Gets or Sets Url to image for "Previous" page navigation button.
        ///</summary>
        /// <value>
        /// Url to image for "Previous" page navigation button. Navigation button doesn't show if Url is empty. Property value is empty string by default.
        /// </value>
        [DefaultValue(""),
        NotifyParentProperty(true),
        EditorAttribute(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
        Description("Url of a picture '-1' line state for CustomPaging")]
        public string LeftImageUrl
        {
            get { return _leftImageUrl; }
            set { _leftImageUrl = value; }
        }

        ///<summary>
        ///Gets or Sets Url to image for disabled "Previous" page navigation button.
        ///</summary>
        /// <value>
        /// Url to image for disabled "Previous" page navigation button. Navigation button doesn't show if Url is empty. Property value is empty string by default.
        /// </value>
        [DefaultValue(""),
        NotifyParentProperty(true),
        EditorAttribute(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
        Description("Url of a Disable picture '-1' line state for CustomPaging")]
        public string LeftDisableImageUrl
        {
            get { return _leftDisableImageUrl; }
            set { _leftDisableImageUrl = value; }
        }

        ///<summary>
        ///Gets or Sets Description text for "Previous" page navigation button.
        ///</summary>
        /// <value>
        /// Description text for "Previous" page navigation button. Button doesn't show if text is empty and ButtonnStyles property defined as Link buttons. Default value is "&lt;"
        /// </value>
        [DefaultValue("<"),
        NotifyParentProperty(true),
        Description("Text of 'left' button for CustomPaging")]
        public string LeftText
        {
            get { return _leftText; }
            set { _leftText = value; }
        }

        ///<summary>
        ///Gets or Sets to image for "Next" page navigation button.
        ///</summary>
        /// <value>
        /// Url to image for "Next" page navigation button. Navigation button doesn't show if Url is empty. Property value is empty string by default.
        /// </value>
        [DefaultValue(""),
        NotifyParentProperty(true),
        EditorAttribute(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
        Description("Url of a picture '+1' line state for CustomPaging")]
        public string RightImageUrl
        {
            get { return _rightImageUrl; }
            set { _rightImageUrl = value; }
        }

        ///<summary>
        ///Gets or Sets to image for disabled "Next" page navigation button.
        ///</summary>
        /// <value>
        /// Url to image for disabled "Next" page navigation button. Navigation button doesn't show if Url is empty. Property value is empty string by default.
        /// </value>
        [DefaultValue(""),
        NotifyParentProperty(true),
        EditorAttribute(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
        Description("Url of a Disable picture '+1' line state for CustomPaging")]
        public string RightDisableImageUrl
        {
            get { return _rightDisableImageUrl; }
            set { _rightDisableImageUrl = value; }
        }

        ///<summary>
        ///Gets or Sets Description text for "Next" page navigation button.
        ///</summary>
        /// <value>
        /// Description text for "Next" page navigation button. Button doesn't show if text is empty and ButtonnStyles property defined as Link buttons. Default value is "&gt;"
        /// </value>
        [DefaultValue(">"),
        NotifyParentProperty(true),
        Description("Text of 'right' button for CustomPaging")]
        public string RightText
        {
            get { return _rightText; }
            set { _rightText = value; }
        }

        ///<summary>
        ///Gets or Sets style how to display and select another page, using TextBox or ComboBox.
        ///</summary>
        /// <value>
        /// Defines style how to display selected page. Also, you can select page using TextBox or DropDown list. Selected page shows as TextBox by default.
        /// </value>
        [DefaultValue(PagingMode.DropDownList),
        NotifyParentProperty(true),
        Description("Defines showing mode for CustomPager")]
        public PagingMode Mode
        {
            get { return _pagerMode; }
            set { _pagerMode = value; }
        }

        ///<summary>
        ///Gets or Sets how to display page navigation buttons as Link Images or Text Links.
        ///</summary>
        /// <value>
        /// Defines navigation buttons style. You can set navigation buttons as Images or Text.
        /// </value>
        [DefaultValue(PagerButtonsStyle.Link),
        NotifyParentProperty(true),
        Description("Defines navigation buttons style for CustomPager")]
        public PagerButtonsStyle ButtonsStyle
        {
            get { return _buttonsStyle; }
            set { _buttonsStyle = value; }
        }

        ///<summary>
        ///Gets or Sets Horizontal alignment for display datagrid custom pager. 
        ///</summary>
        /// <value>
        /// Defines horizontal alignment for navigation buttons and titles.
        /// </value>
        [DefaultValue(HorizontalAlign.NotSet),
        NotifyParentProperty(true),
        Description("Defines Horisontal Alignment for CustomPager")]
        public HorizontalAlign HorizontalAlign
        {
            get { return _hAlign; }
            set { _hAlign = value; }
        }

        /// <summary>
        /// Gets or sets width of CustomPager
        /// </summary>
        [DefaultValue(typeof(Unit), ""),
        NotifyParentProperty(true),
        Description("Defines width of CustomPager")]
        public Unit Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Gets or sets a value that specifies the location where the pager is displayed.
        /// </summary>
        [DefaultValue(PagerPosition.Bottom),
        NotifyParentProperty(true),
        Description("The position of the navigation bar.")]
        public PagerPosition Position
        {
            get { return _position; }
            set { _position = value; }
        }

        #endregion
    }
}