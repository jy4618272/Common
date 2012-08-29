using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Provides visual feedback on the browser when the contents of one or more System.Web.UI.UpdatePanel controls are updated.
    /// </summary>
    public class UpdateProgress : System.Web.UI.UpdateProgress
    {
        #region Members

        /// <summary>
        /// Defines script that creates an instance of a client class.
        /// </summary>
        private class UpdateProgressScriptDescriptor : ScriptDescriptor
        {
            #region Members

            private UpdateProgress m_ScriptControl;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            /// <param name="scriptControl">The associated control.</param>
            public UpdateProgressScriptDescriptor(UpdateProgress scriptControl)
                : base()
            {
                m_ScriptControl = scriptControl;
            }

            #endregion

            #region Private Methods

            private string GetAssociatedUpdatePanelClientId()
            {
                string associatedUpdatePanelClientId = string.Empty;
                if (!string.IsNullOrEmpty(m_ScriptControl.AssociatedUpdatePanelID))
                {
                    UpdatePanel panel = Support.FindTargetControl(m_ScriptControl.AssociatedUpdatePanelID, m_ScriptControl, true) as UpdatePanel;
                    if (panel == null)
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Resources.UpdateProgress_NoUpdatePanel, m_ScriptControl.AssociatedUpdatePanelID));
                    associatedUpdatePanelClientId = panel.ClientID;
                }
                return associatedUpdatePanelClientId;
            }

            #endregion

            #region Overriden Methods

            /// <summary>
            /// Returns script to create a client class or object.
            /// </summary>
            /// <returns>The script to create a client class or object.</returns>
            protected override string GetScript()
            {
                return string.Format(CultureInfo.InvariantCulture
                    , "$create(Micajah.Common.UpdateProgress, {{'associatedUpdatePanelId':'{0}','displayAfter':{1},'dynamicLayout':{2},'hideAfter':{3},'timeout':{4}}}, null, null, $get('{5}'));"
                    , GetAssociatedUpdatePanelClientId(), m_ScriptControl.DisplayAfter, (m_ScriptControl.DynamicLayout ? "true" : "false"), m_ScriptControl.HideAfter, m_ScriptControl.Timeout, m_ScriptControl.ClientID);
            }

            #endregion
        }

        private class DefaultProgressTemplate : ITemplate
        {
            #region Members

            private string m_Text;

            #endregion

            #region Constructors

            public DefaultProgressTemplate(string text)
            {
                m_Text = text;
            }

            #endregion

            #region Public Methods

            public void InstantiateIn(Control container)
            {
                if (container == null) return;

                using (Image img = new Image())
                {
                    img.ImageAlign = ImageAlign.AbsMiddle;
                    img.ImageUrl = ResourceProvider.GetImageUrl(typeof(UpdateProgress), "Loader.gif", true);
                    container.Controls.Add(img);
                }

                using (Label lbl = new Label())
                {
                    ApplyCommonStyle(lbl);
                    lbl.ForeColor = System.Drawing.Color.Gray;
                    lbl.Text = m_Text;
                    container.Controls.Add(lbl);
                }
            }

            #endregion
        }

        private class DefaultSuccessTemplate : ITemplate
        {
            #region Members

            private string m_Text;

            #endregion

            #region Constructors

            public DefaultSuccessTemplate(string text)
            {
                m_Text = text;
            }

            #endregion

            #region Public Methods

            public void InstantiateIn(Control container)
            {
                if (container == null) return;

                using (Label lbl = new Label())
                {
                    ApplyCommonStyle(lbl);
                    lbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#006600");
                    lbl.BackColor = System.Drawing.ColorTranslator.FromHtml("#cfeeb2");
                    lbl.Text = m_Text;
                    container.Controls.Add(lbl);
                }
            }

            #endregion
        }

        private class DefaultFailureTemplate : ITemplate
        {
            #region Members

            private string m_Text;

            #endregion

            #region Constructors

            public DefaultFailureTemplate(string text)
            {
                m_Text = text;
            }

            #endregion

            #region Public Methods

            public void InstantiateIn(Control container)
            {
                if (container == null) return;

                using (Label lbl = new Label())
                {
                    ApplyCommonStyle(lbl);
                    lbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000066");
                    lbl.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffefac");
                    lbl.Text = m_Text;
                    container.Controls.Add(lbl);
                }
            }

            #endregion
        }

        private ITemplate m_FailureTemplate;
        private HtmlGenericControl m_FailureTemplateContainer;
        private HtmlGenericControl m_ProgressTemplateContainer;
        private ITemplate m_SuccessTemplate;
        private HtmlGenericControl m_SuccessTemplateContainer;
        private HtmlGenericControl m_PostBackActionControlContainer;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the time in milliseconds after which the control will be hidden. Any negative number turns off this feature.
        /// </summary>
        [Category("Behavior")]
        [Description("Time in milliseconds after which the control will be hidden. Any negative number turns off this feature.")]
        [DefaultValue(2000)]
        public int HideAfter
        {
            get
            {
                object obj = this.ViewState["HideAfter"];
                return ((obj == null) ? 2000 : (int)obj);
            }
            set { this.ViewState["HideAfter"] = value; }
        }

        /// <summary>
        /// Gets or sets the content which is displayed when async postback request has finished with the errors.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(false)]
        public ITemplate FailureTemplate
        {
            get
            {
                if (m_FailureTemplate == null)
                    m_FailureTemplate = new DefaultFailureTemplate(this.FailureText);
                return m_FailureTemplate;
            }
            set { m_FailureTemplate = value; }
        }

        /// <summary>
        /// Gets or sets the text which is displayed when async postback request has finished with the errors.
        /// </summary>
        [Category("Appearance")]
        [Description("The text which is displayed when async postback request has finished with the errors.")]
        [ResourceDefaultValue("UpdateProgress_FailureText")]
        public string FailureText
        {
            get
            {
                object obj = this.ViewState["FailureText"];
                return ((obj == null) ? Resources.UpdateProgress_FailureText : (string)obj);
            }
            set { this.ViewState["FailureText"] = value; }
        }

        /// <summary>
        /// Gets or sets the template that defines the content of the control.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(false)]
        public new ITemplate ProgressTemplate
        {
            get
            {
                if (base.ProgressTemplate == null)
                    base.ProgressTemplate = new DefaultProgressTemplate(this.ProgressText);
                return base.ProgressTemplate;
            }
            set { base.ProgressTemplate = value; }
        }

        /// <summary>
        /// Gets or sets the text which is displayed during async postbacks.
        /// </summary>
        [Category("Appearance")]
        [Description("The text which is displayed during async postbacks.")]
        [ResourceDefaultValue("UpdateProgress_ProgressText")]
        public string ProgressText
        {
            get
            {
                object obj = this.ViewState["ProgressText"];
                return ((obj == null) ? Resources.UpdateProgress_ProgressText : (string)obj);
            }
            set { this.ViewState["ProgressText"] = value; }
        }

        /// <summary>
        /// Gets or sets the content which is displayed after successful async postbacks.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(false)]
        public ITemplate SuccessTemplate
        {
            get
            {
                if (m_SuccessTemplate == null)
                    m_SuccessTemplate = new DefaultSuccessTemplate(this.SuccessText);
                return m_SuccessTemplate;
            }
            set { m_SuccessTemplate = value; }
        }

        /// <summary>
        /// Gets or sets the text which is displayed after successful async postbacks.
        /// </summary>
        [Category("Appearance")]
        [Description("The text which is displayed after successful async postbacks.")]
        [ResourceDefaultValue("UpdateProgress_SuccessText")]
        public string SuccessText
        {
            get
            {
                object obj = this.ViewState["SuccessText"];
                return ((obj == null) ? Resources.UpdateProgress_SuccessText : (string)obj);
            }
            set { this.ViewState["SuccessText"] = value; }
        }

        /// <summary>
        /// Gets or sets the time-out value for the async postbacks in milliseconds.
        /// </summary>
        [Category("Behavior")]
        [Description("The time-out value for the async postbacks in milliseconds.")]
        [DefaultValue(90000)]
        public int Timeout
        {
            get
            {
                object obj = this.ViewState["Timeout"];
                return ((obj == null) ? 90000 : (int)obj);
            }
            set
            {
                if (value > -1)
                    this.ViewState["Timeout"] = value;
                else
                    this.ViewState.Remove("Timeout");
            }
        }

        // <summary>
        /// Gets or sets a value indicating whether the failed text should be shown.
        /// </summary>
        [Category("Appearance")]
        [Description("The value indicating whether the failure text should be shown..")]
        [DefaultValue(true)]
        public bool ShowFailureText
        {
            get
            {
                object obj = this.ViewState["ShowFailureText"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { this.ViewState["ShowFailureText"] = value; }
        }

        // <summary>
        /// Gets or sets a value indicating whether the success text should be shown.
        /// </summary>
        [Category("Appearance")]
        [Description("The value indicating whether the success text should be shown..")]
        [DefaultValue(true)]
        public bool ShowSuccessText
        {
            get
            {
                object obj = this.ViewState["ShowSuccessText"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { this.ViewState["ShowSuccessText"] = value; }
        }

        /// <summary>
        /// Gets or sets the id of post back action control.
        /// </summary>
        [Category("Appearance")]
        [Description("The id of post back action control.")]
        [DefaultValue("")]
        public string PostBackActionControl
        {
            get
            {
                object obj = this.ViewState["PostBackActionControl"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { this.ViewState["PostBackActionControl"] = value; }
        }        

        #endregion

        #region Private Methods

        private static void ApplyCommonStyle(Label lbl)
        {
            lbl.Font.Name = "Arial";
            lbl.Font.Size = FontUnit.Parse("18px", CultureInfo.InvariantCulture);
            lbl.Font.Bold = true;
            lbl.Style[HtmlTextWriterStyle.Padding] = "3px 8px 3px 8px";
        }

        private void CreateFailureTemplateContainer()
        {
            m_FailureTemplateContainer = new HtmlGenericControl("div");
            m_FailureTemplateContainer.Attributes["id"] = this.ClientID + this.ClientIDSeparator + "FailureTemplate";
            m_FailureTemplateContainer.Style[HtmlTextWriterStyle.Display] = "none";
            this.FailureTemplate.InstantiateIn(m_FailureTemplateContainer);
            this.Controls.Add(m_FailureTemplateContainer);
        }

        private void CreateProgressTemplateContainer()
        {
            m_ProgressTemplateContainer = new HtmlGenericControl("div");
            m_ProgressTemplateContainer.Attributes["id"] = this.ClientID + this.ClientIDSeparator + "ProgressTemplate";
            this.ProgressTemplate.InstantiateIn(m_ProgressTemplateContainer);
            this.Controls.Add(m_ProgressTemplateContainer);
        }

        private void CreateSuccessTemplateContainer()
        {
            m_SuccessTemplateContainer = new HtmlGenericControl("div");
            m_SuccessTemplateContainer.Attributes["id"] = this.ClientID + this.ClientIDSeparator + "SuccessTemplate";            
            m_SuccessTemplateContainer.Style[HtmlTextWriterStyle.Display] = "none";
            this.SuccessTemplate.InstantiateIn(m_SuccessTemplateContainer);
            this.Controls.Add(m_SuccessTemplateContainer);
        }

        private void CreatePostBackContainer()
        {
            m_PostBackActionControlContainer = new HtmlGenericControl("div");
            m_PostBackActionControlContainer.Attributes["id"] = this.ClientID + this.ClientIDSeparator + "PostBackAction";
            m_PostBackActionControlContainer.Style[HtmlTextWriterStyle.Display] = "none";
            m_PostBackActionControlContainer.InnerText = this.PostBackActionControl;
            this.Controls.Add(m_PostBackActionControlContainer);
        }

        #endregion

        #region Overriden Methods

        protected override void CreateChildControls()
        {
            if (this.ShowFailureText)
                this.CreateFailureTemplateContainer();
            this.CreateProgressTemplateContainer();
            if (this.ShowSuccessText)
                this.CreateSuccessTemplateContainer();
            this.CreatePostBackContainer();
        }

        /// <summary>
        /// Returns a list of components, behaviors, and client controls that are required for the control's client functionality.
        /// </summary>
        /// <returns>The list of components, behaviors, and client controls that are required for the control's client functionality.</returns>
        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            List<ScriptDescriptor> list = new List<ScriptDescriptor>();
            list.Add(new UpdateProgressScriptDescriptor(this));
            return list;
        }

        /// <summary>
        /// Returns a list of client script library dependencies for the control.
        /// </summary>
        /// <returns>The list of client script library dependencies for the control.</returns>
        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            List<ScriptReference> list = new List<ScriptReference>();
            list.AddRange(base.GetScriptReferences());
            list.Add(new ScriptReference(ResourceProvider.GetResourceUrl("Scripts.UpdateProgress.js", true)));
            return list;
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            ScriptManager current = ScriptManager.GetCurrent(this.Page);
            if (current != null) current.RegisterScriptControl<UpdateProgress>(this);
        }

        #endregion
    }
}
