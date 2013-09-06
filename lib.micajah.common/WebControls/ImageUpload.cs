using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// The control for uploading the single image.
    /// </summary>
    [ToolboxData("<{0}:ImageUpload runat=server></{0}:ImageUpload>")]
    [ParseChildren(true)]
    public class ImageUpload : ScriptControl, INamingContainer
    {
        #region Members

        /// <summary>
        /// Defines script that creates an instance of a client class.
        /// </summary>
        private class ImageUploadScriptDescriptor : ScriptDescriptor
        {
            #region Members

            private ImageUpload m_ScriptControl;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            /// <param name="scriptControl">The associated control.</param>
            public ImageUploadScriptDescriptor(ImageUpload scriptControl)
                : base()
            {
                m_ScriptControl = scriptControl;
            }

            #endregion

            #region Overriden Methods

            /// <summary>
            /// Returns script to create a client class or object.
            /// </summary>
            /// <returns>The script to create a client class or object.</returns>
            protected override string GetScript()
            {
                if (m_ScriptControl.EnablePopupWindow)
                    return string.Format(CultureInfo.InvariantCulture, "$create(Micajah.Common.ImageUpload, null, null, null, $get(\"{0}\"));", m_ScriptControl.ClientID);
                else
                {
                    return string.Format(CultureInfo.InvariantCulture
                        , "$create(Micajah.Common.ImageUpload, {{\"allowedFileExtensions\":\"{0}\", \"isPostBack\":{1}, \"deletingConfirmationText\":\"{2}\", \"enablePopupWindow\":{3}, \"errorMessages\":\"[\\\"{4}\\\", \\\"{5}\\\", \\\"{6}\\\", \\\"{7}\\\"]\"}}, null, null, $get(\"{8}\"));"
                        , AllowedFileExtensions
                        , (m_ScriptControl.Page.IsPostBack ? "true" : "false")
                        , ((m_ScriptControl.EnableDeleting && m_ScriptControl.EnableDeletingConfirmation) ? Resources.ImageUpload_DeletingConfirmationText.Replace("\"", "\\\"") : string.Empty)
                        , (m_ScriptControl.EnablePopupWindow ? "true" : "false")
                        , Resources.ImageUpload_FilePathIsEmpty.Replace("\"", "\\\"")
                        , Resources.ImageUpload_UrlIsEmpty.Replace("\"", "\\\"")
                        , Resources.ImageUpload_InvalidUrl.Replace("\"", "\\\"")
                        , Resources.ImageUpload_InvalidFileExtension.Replace("\"", "\\\"")
                        , m_ScriptControl.ClientID);
                }
            }

            #endregion
        }

        private const string AllowedFileExtensions = "jpg,jpe,jpeg,gif,png,tif,tiff,bmp";
        private const int PopupWindowWidth = 430;
        private const int PopupWindowHeight = 235;

        private HyperLink OpenLink;
        private RadWindow PopupWindow;
        private HiddenField OriginalFileField;
        private HiddenField CurrentFileField;
        private HiddenField PreviousFilesField;
        private RadioButtonList UploadTypeList;
        private Panel FileFromMyComputerPanel;
        private FileUpload FileFromMyComputer;
        private Panel FileFromWebPanel;
        private System.Web.UI.WebControls.TextBox FileFromWeb;
        private HtmlGenericControl ErrorDiv;
        private Button UploadButton;
        private Panel UploadedImageViewPanel;
        private Image UploadedImage;
        private HyperLink DeleteButton;
        private Label NoImageLabel;
        private HyperLink ClosePopupWindowButton;
        private Label ButtonSeparator;

        private string m_ErrorMessage;
        private bool m_ObjectChanged;

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets unique identifier of the image.
        /// </summary>
        private Guid? ImageResourceId
        {
            get
            {
                this.EnsureFields();
                object obj = Support.ConvertStringToType(CurrentFileField.Value, typeof(Guid));
                return ((obj == null) ? null : new Guid?((Guid)obj));
            }
        }

        private bool ObjectChanged
        {
            get { return (((this.Page != null) && (!this.Page.IsPostBack)) || m_ObjectChanged); }
            set { m_ObjectChanged = value; }
        }

        private string PopupWindowNavigateUrl
        {
            get
            {
                Hashtable table = new Hashtable();
                table["EnableDeleting"] = this.EnableDeleting;
                table["EnableDeletingConfirmation"] = this.EnableDeletingConfirmation;
                table["EnablePopupWindow"] = false;
                table["LocalObjectType"] = this.LocalObjectType;
                table["LocalObjectId"] = this.LocalObjectId;

                return CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.ImageUploadPageVirtualPath)
                    + "?p=" + HttpUtility.UrlEncodeUnicode(Support.SaveProperties(table));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or set value indicating that the deleting is enabled or disabled.
        /// </summary>
        [Category("Behavior")]
        [Description("Whether the deleting is enabled.")]
        [DefaultValue(true)]
        public bool EnableDeleting
        {
            get
            {
                object obj = ViewState["EnableDeleting"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["EnableDeleting"] = value; }
        }

        /// <summary>
        /// Gets or set value indicating that the deleting requires confirmation.
        /// </summary>
        [Category("Behavior")]
        [Description("Whether the deleting requires confirmation.")]
        [DefaultValue(true)]
        public bool EnableDeletingConfirmation
        {
            get
            {
                object obj = ViewState["EnableDeletingConfirmation"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["EnableDeletingConfirmation"] = value; }
        }

        /// <summary>
        /// Gets or set value indicating whether the control is displayed in the pop-up window.
        /// </summary>
        [Category("Behavior")]
        [Description("Whether the control is displayed in the pop-up window.")]
        [DefaultValue(true)]
        public bool EnablePopupWindow
        {
            get
            {
                object obj = ViewState["EnablePopupWindow"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["EnablePopupWindow"] = value; }
        }

        /// <summary>
        /// Gets a message that describes the current error, if it occured.
        /// </summary>
        [Browsable(false)]
        public string ErrorMessage
        {
            get { return m_ErrorMessage; }
        }

        /// <summary>
        /// Gets a value indicating that an error occurred.
        /// </summary>
        [Browsable(false)]
        public bool ErrorOccurred
        {
            get { return (!string.IsNullOrEmpty(m_ErrorMessage)); }
        }

        /// <summary>
        /// Gets or sets the type of the object which the uploaded files are associated with.
        /// </summary>
        [Category("Data")]
        [Description("The type of the object which the uploaded files are associated with.")]
        [DefaultValue("")]
        public string LocalObjectType
        {
            get
            {
                object obj = this.ViewState["LocalObjectType"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set
            {
                this.ChangeObject(this.LocalObjectType, value);
                this.ViewState["LocalObjectType"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the object which the uploaded files are associated with.
        /// </summary>
        [Category("Data")]
        [Description("The unique identifier of the object which the uploaded files are associated with.")]
        [DefaultValue("")]
        public string LocalObjectId
        {
            get
            {
                object obj = this.ViewState["LocalObjectId"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set
            {
                this.ChangeObject(this.LocalObjectId, value);
                this.ViewState["LocalObjectId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximal size for the uploaded files in bytes. The default value is 0 that's mean the unlimited size.
        /// </summary>
        [Category("Behavior")]
        [Description("The maximal size for the uploaded file in bytes.")]
        [DefaultValue(0)]
        public int MaxFileSize
        {
            get
            {
                object obj = this.ViewState["MaxFileSize"];
                return ((obj == null) ? 0 : (int)obj);
            }
            set { this.ViewState["MaxFileSize"] = value; }
        }

        /// <summary>
        /// Gets or set a value indicating whether the error message is displayed in the control.
        /// </summary>
        [Category("Appearance")]
        [Description("Whether the error message is displayed in the control.")]
        [DefaultValue(true)]
        public bool ShowErrorMessage
        {
            get
            {
                object obj = ViewState["ShowErrorMessage"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["ShowErrorMessage"] = value; }
        }

        #endregion

        #region Private Methods

        private void ChangeObject(string currentValue, string newValue)
        {
            if ((this.Page != null) && this.Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(currentValue))
                {
                    if (!string.IsNullOrEmpty(newValue))
                        this.ObjectChanged = true;
                }
                else
                {
                    if (string.IsNullOrEmpty(newValue))
                        this.ObjectChanged = true;
                    else
                        this.ObjectChanged = (string.Compare(newValue, currentValue, StringComparison.Ordinal) != 0);
                }
            }
        }

        private void CreateEmbeddedControl()
        {
            HtmlTable table = new HtmlTable();
            table.Style[HtmlTextWriterStyle.Width] = string.Concat((PopupWindowWidth - 20), "px");
            table.Style[HtmlTextWriterStyle.Height] = string.Concat((PopupWindowHeight - 110), "px");
            HtmlTableRow tr = new HtmlTableRow();
            tr.Style[HtmlTextWriterStyle.VerticalAlign] = "top";
            HtmlTableCell td = new HtmlTableCell();

            UploadTypeList = new RadioButtonList();
            UploadTypeList.ID = "UploadTypeList";
            UploadTypeList.CellPadding = 5;
            UploadTypeList.CellSpacing = 0;
            UploadTypeList.Style[HtmlTextWriterStyle.Position] = "relative";
            UploadTypeList.Style[HtmlTextWriterStyle.Left] = "-8px";
            ListItem listItem = new ListItem(Resources.ImageUpload_UploadTypeList_FromMyComputerItem_Text, "0");
            listItem.Selected = true;
            UploadTypeList.Items.Add(listItem);
            UploadTypeList.Items.Add(new ListItem(Resources.ImageUpload_UploadTypeList_FromWebItem_Text, "1"));
            td.Controls.Add(UploadTypeList);

            FileFromMyComputerPanel = new Panel();
            FileFromMyComputerPanel.ID = "Panel0";
            FileFromMyComputerPanel.CssClass = "iuPanel";
            td.Controls.Add(FileFromMyComputerPanel);

            FileFromMyComputer = new FileUpload();
            FileFromMyComputer.ID = "FileFromMyComputer";
            FileFromMyComputer.Attributes["size"] = "29";
            FileFromMyComputer.Height = Unit.Pixel(21);
            FileFromMyComputer.CssClass = "iuFromComp";
            FileFromMyComputerPanel.Controls.Add(FileFromMyComputer);

            FileFromWebPanel = new Panel();
            FileFromWebPanel.ID = "Panel1";
            FileFromWebPanel.CssClass = "iuPanel";
            td.Controls.Add(FileFromWebPanel);

            FileFromWeb = new System.Web.UI.WebControls.TextBox();
            FileFromWeb.ID = "FileFromWeb";
            FileFromWeb.Columns = 45;
            FileFromWeb.Width = Unit.Pixel(250);
            FileFromWeb.CssClass = "iuFromWeb";
            FileFromWebPanel.Controls.Add(FileFromWeb);

            ErrorDiv = CreateErrorDiv("iuError ErrorMessage Block");
            td.Controls.Add(ErrorDiv);

            UploadButton = new Button();
            UploadButton.ID = "UploadButton";
            UploadButton.CssClass = "iuUploadButton";
            UploadButton.CausesValidation = false;
            UploadButton.Text = Resources.ImageUpload_UploadButton_Text;
            UploadButton.Click += new EventHandler(UploadButton_Click);
            td.Controls.Add(UploadButton);

            tr.Cells.Add(td);

            td = new HtmlTableCell();
            td.Style[HtmlTextWriterStyle.Width] = "135px";

            this.EnsureUploadedImageViewPanel();
            td.Controls.Add(UploadedImageViewPanel);

            if (this.EnableDeleting)
            {
                UploadedImageViewPanel.Controls.Add(new LiteralControl("<br />"));
                DeleteButton = new HyperLink();
                DeleteButton.ID = "DeleteButton";
                DeleteButton.CssClass = "iuRemove";
                DeleteButton.Style[HtmlTextWriterStyle.Display] = "none";
                DeleteButton.Text = Resources.ImageUpload_DeleteText;
                DeleteButton.NavigateUrl = "javascript:void(0);";
                UploadedImageViewPanel.Controls.Add(DeleteButton);
            }

            tr.Cells.Add(td);
            table.Rows.Add(tr);

            tr = new HtmlTableRow();
            tr.Style[HtmlTextWriterStyle.VerticalAlign] = "top";

            td = new HtmlTableCell();
            td.ColSpan = 2;

            ButtonSeparator = new Label();
            ButtonSeparator.ID = "ButtonSeparator";
            ButtonSeparator.CssClass = "iuButtonSeparator";
            ButtonSeparator.Text = Resources.ImageUpload_ButtonSeparator_Text;
            ButtonSeparator.Style[HtmlTextWriterStyle.Display] = "none";
            td.Controls.Add(ButtonSeparator);

            ClosePopupWindowButton = new HyperLink();
            ClosePopupWindowButton.ID = "ClosePopupWindowButton";
            ClosePopupWindowButton.NavigateUrl = "#";
            ClosePopupWindowButton.CssClass = "iuCloseButton";
            ClosePopupWindowButton.Style[HtmlTextWriterStyle.Display] = "none";
            ClosePopupWindowButton.Text = Resources.ImageUpload_ClosePopupWindowButton_Text;
            td.Controls.Add(ClosePopupWindowButton);

            tr.Cells.Add(td);
            table.Rows.Add(tr);

            this.Controls.Add(table);
        }

        private static HtmlGenericControl CreateErrorDiv(string cssClass)
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.ID = "ErrorDiv";
            div.EnableViewState = false;
            div.Style[HtmlTextWriterStyle.Display] = "none";
            if (!string.IsNullOrEmpty(cssClass)) div.Attributes["class"] = cssClass;
            return div;
        }

        private void CreatePopupWindow()
        {
            OpenLink = new HyperLink();
            OpenLink.ID = "OpenLink";
            OpenLink.NavigateUrl = "#";
            OpenLink.ToolTip = Resources.ImageUpload_OpenLink_ToolTip;
            OpenLink.Style[HtmlTextWriterStyle.Cursor] = "pointer";
            this.Controls.Add(OpenLink);

            this.EnsureUploadedImageViewPanel();
            OpenLink.Controls.Add(UploadedImageViewPanel);

            ErrorDiv = CreateErrorDiv("iuError ErrorMessage Block");
            this.Controls.Add(ErrorDiv);

            PopupWindow = new RadWindow();
            PopupWindow.ID = "PopupWindow";
            PopupWindow.Behaviors = (WindowBehaviors.Close | WindowBehaviors.Move);
            PopupWindow.Width = Unit.Pixel(PopupWindowWidth);
            PopupWindow.Height = Unit.Pixel(PopupWindowHeight);
            PopupWindow.OpenerElementID = OpenLink.ClientID;
            PopupWindow.Title = Resources.ImageUpload_PopupWindow_Title;
            PopupWindow.VisibleStatusbar = false;
            this.Controls.Add(PopupWindow);
        }

        private void EnsureFields()
        {
            if (CurrentFileField == null)
            {
                CurrentFileField = new HiddenField();
                CurrentFileField.ID = "CurrentFileField";
            }

            if (OriginalFileField == null)
            {
                OriginalFileField = new HiddenField();
                OriginalFileField.ID = "OriginalFileField";
                OriginalFileField.PreRender += new EventHandler(OriginalFileField_PreRender);
            }

            if (PreviousFilesField == null)
            {
                PreviousFilesField = new HiddenField();
                PreviousFilesField.ID = "PreviousFilesField";
            }
        }

        private void EnsureUploadedImageViewPanel()
        {
            if (UploadedImageViewPanel != null) return;

            UploadedImageViewPanel = new Panel();
            UploadedImageViewPanel.ID = "UploadedImageViewPanel";
            UploadedImageViewPanel.CssClass = "iuImageView";
            UploadedImageViewPanel.Width = Unit.Pixel(131);
            UploadedImageViewPanel.Height = Unit.Pixel(150);

            NoImageLabel = new Label();
            NoImageLabel.ID = "NoImageLabel";
            NoImageLabel.Text = Resources.ImageUpload_NoImageLabel_Text;
            NoImageLabel.Style[HtmlTextWriterStyle.Position] = "relative";
            NoImageLabel.Style[HtmlTextWriterStyle.Top] = "65px";
            UploadedImageViewPanel.Controls.Add(NoImageLabel);

            UploadedImage = new Image();
            UploadedImage.ID = "UploadedImage";
            UploadedImage.Width = Unit.Pixel(130);
            UploadedImage.Height = Unit.Pixel(130);
            UploadedImage.Style[HtmlTextWriterStyle.Cursor] = "pointer";
            UploadedImage.Style[HtmlTextWriterStyle.Display] = "none";
            UploadedImageViewPanel.Controls.Add(UploadedImage);
        }

        private void UploadFile()
        {
            if (!this.ChildControlsCreated) return;

            Guid? resourceId = null;

            switch (UploadTypeList.SelectedValue)
            {
                case "0":
                    if (!string.IsNullOrEmpty(FileFromMyComputer.FileName))
                    {
                        if (FileFromMyComputer.HasFile)
                        {
                            HttpPostedFile file = FileFromMyComputer.PostedFile;
                            if (ValidateExtension(Path.GetExtension(file.FileName).Replace(".", string.Empty)) && MimeType.IsImageType(file.ContentType))
                            {
                                long fileSize = file.InputStream.Length;
                                if ((this.MaxFileSize < 1) || (fileSize <= this.MaxFileSize))
                                {
                                    resourceId = ResourceProvider.InsertResource(null, this.LocalObjectType, this.LocalObjectId, FileFromMyComputer.FileBytes, file.ContentType, Path.GetFileName(file.FileName), 0, 0, 0, true);
                                    if (!resourceId.HasValue)
                                        m_ErrorMessage = Resources.ImageUpload_FileIsNotSaved;
                                }
                                else
                                    m_ErrorMessage = Resources.ImageUpload_InvalidFileSize;
                            }
                            else
                                m_ErrorMessage = Resources.ImageUpload_InvalidFileExtension;
                        }
                        else
                            m_ErrorMessage = Resources.ImageUpload_FileNotExists;
                    }
                    else
                        m_ErrorMessage = Resources.ImageUpload_FilePathIsEmpty;
                    break;
                case "1":
                    string url = FileFromWeb.Text;
                    if (!string.IsNullOrEmpty(url))
                    {
                        if (Support.ValidateUrl(url, false))
                        {
                            if (ValidateExtension(Support.GetLastPartOfString(url, ".")))
                            {
                                try
                                {
                                    resourceId = ResourceProvider.InsertResource(url, this.LocalObjectType, this.LocalObjectId, true, this.MaxFileSize);
                                    if (!resourceId.HasValue)
                                        m_ErrorMessage = Resources.ImageUpload_FileIsNotSaved;
                                }
                                catch (InvalidDataException)
                                {
                                    m_ErrorMessage = Resources.ImageUpload_InvalidFileSize;
                                }
                            }
                            else
                                m_ErrorMessage = Resources.ImageUpload_InvalidFileExtension;
                        }
                        else
                            m_ErrorMessage = Resources.ImageUpload_InvalidUrl;
                    }
                    else
                        m_ErrorMessage = Resources.ImageUpload_UrlIsEmpty;
                    break;
            }

            if (!this.ErrorOccurred)
            {
                FileFromWeb.Text = string.Empty;

                if (!CurrentFileField.Value.Equals(OriginalFileField.Value, StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(PreviousFilesField.Value))
                        PreviousFilesField.Value = CurrentFileField.Value;
                    else
                        PreviousFilesField.Value += "|" + CurrentFileField.Value;
                }
                CurrentFileField.Value = (resourceId.HasValue ? resourceId.Value.ToString("N") : null);
            }
        }

        private static bool ValidateExtension(string extension)
        {
            return (string.Concat(",", AllowedFileExtensions.ToUpperInvariant(), ",").Contains("," + extension.ToUpperInvariant() + ","));
        }

        private void OriginalFileField_PreRender(object sender, EventArgs e)
        {
            if (this.ObjectChanged && (!string.IsNullOrEmpty(this.LocalObjectType)) && (!string.IsNullOrEmpty(this.LocalObjectId)))
            {
                MasterDataSet.ResourceRow originalFile = ResourceProvider.GetResourceRow(this.LocalObjectType, this.LocalObjectId);
                OriginalFileField.Value = CurrentFileField.Value = ((originalFile == null) ? string.Empty : originalFile.ResourceId.ToString("N"));
            }
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            this.UploadFile();
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates the child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (this.EnablePopupWindow)
                this.CreatePopupWindow();
            else
                this.CreateEmbeddedControl();

            this.EnsureFields();
            this.Controls.Add(OriginalFileField);
            this.Controls.Add(CurrentFileField);
            this.Controls.Add(PreviousFilesField);
        }

        /// <summary>
        /// Returns a list of components, behaviors, and client controls that are required for the control's client functionality.
        /// </summary>
        /// <returns>The list of components, behaviors, and client controls that are required for the control's client functionality.</returns>
        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            List<ScriptDescriptor> list = new List<ScriptDescriptor>();
            list.Add(new ImageUploadScriptDescriptor(this));
            return list;
        }

        /// <summary>
        /// Returns a list of client script library dependencies for the control.
        /// </summary>
        /// <returns>The list of client script library dependencies for the control.</returns>
        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            List<ScriptReference> list = new List<ScriptReference>();
            list.Add(new ScriptReference(ResourceProvider.GetResourceUrl("Scripts.ImageUpload.js", true)));
            return list;
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event and registers the style sheet of the control.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ResourceProvider.RegisterStyleSheetResource(this, "Styles.ImageUpload.css", "ImageUploadStyleSheet", true);

            if (this.ShowErrorMessage && this.ErrorOccurred)
            {
                ErrorDiv.InnerHtml = m_ErrorMessage;
                ErrorDiv.Style.Remove(HtmlTextWriterStyle.Display);
            }
        }

        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <param name="writer">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (writer == null) return;

            if (this.DesignMode)
                writer.Write(string.Format(CultureInfo.InvariantCulture, "[{0} \"{1}\"]", this.GetType().Name, this.ID));
            else
            {
                if (OpenLink != null)
                {
                    OpenLink.MergeStyle(base.ControlStyle);
                    base.ControlStyle.Reset();
                }
                if (PopupWindow != null) PopupWindow.NavigateUrl = this.PopupWindowNavigateUrl;
                Guid? resourceId = this.ImageResourceId;
                if (resourceId.HasValue)
                {
                    UploadedImage.ImageUrl = ResourceProvider.GetResourceUrl(resourceId.Value, 130, 130, 1, true);
                    UploadedImage.Attributes["currentFileUrl"] = ResourceProvider.GetResourceUrl(resourceId.Value, true);
                }
                else
                    UploadedImage.ImageUrl = string.Empty;
                base.CssClass = "iuContainer";
                base.RenderControl(writer);
            }
        }

        /// <summary>
        /// Gets the System.Web.UI.HtmlTextWriterTag value that corresponds to this Web server control.
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Commits all the changes since the last time AcceptChanges was called.
        /// </summary>
        /// <returns>true, if the changes are commited successfully; otherwise, false.</returns>
        public bool AcceptChanges()
        {
            if ((!this.ChildControlsCreated) || CurrentFileField.Value.Equals(OriginalFileField.Value, StringComparison.OrdinalIgnoreCase))
                return true;

            m_ErrorMessage = string.Empty;

            ResourceProvider.DeleteResources(PreviousFilesField.Value.Split('|'));
            if (!this.ErrorOccurred) ResourceProvider.DeleteResources(OriginalFileField.Value);

            if (!this.ErrorOccurred)
            {
                Guid? resourceId = this.ImageResourceId;
                if (resourceId.HasValue) ResourceProvider.UpdateResource(resourceId.Value, this.LocalObjectType, this.LocalObjectId, false);
            }

            if (!this.ErrorOccurred)
            {
                OriginalFileField.Value = CurrentFileField.Value;
                PreviousFilesField.Value = string.Empty;
            }

            return (!this.ErrorOccurred);
        }

        /// <summary>
        /// Rolls back all changes that have been made to the control since it was loaded, or the last time AcceptChanges was called.
        /// </summary>
        /// <returns>true, if the changes are rolled back successfully; otherwise, false.</returns>
        public bool RejectChanges()
        {
            if ((!this.ChildControlsCreated) || CurrentFileField.Value.Equals(OriginalFileField.Value, StringComparison.OrdinalIgnoreCase))
                return true;

            m_ErrorMessage = string.Empty;

            ResourceProvider.DeleteResources(PreviousFilesField.Value.Split('|'));
            if (!this.ErrorOccurred) ResourceProvider.DeleteResources(CurrentFileField.Value);

            if (!this.ErrorOccurred)
            {
                CurrentFileField.Value = OriginalFileField.Value;
                PreviousFilesField.Value = string.Empty;
            }

            return (!this.ErrorOccurred);
        }

        public void LoadProperties(string value)
        {
            Support.LoadProperties(this, value);
        }

        #endregion
    }
}