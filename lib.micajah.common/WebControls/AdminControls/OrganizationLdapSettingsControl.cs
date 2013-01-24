using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Handlers;
using Micajah.Common.Bll.Providers;
using Micajah.Common.LdapAdapter;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;

namespace Micajah.Common.WebControls.AdminControls
{
    public class OrganizationLdapSettingsControl : BaseEditFormControl
    {
        #region Members

        protected LinkButton GetDomainsButton;
        protected LinkButton GetGroupsButton;
        protected LinkButton GetTestAdReplicationInfo;
        protected LinkButton GetRealAdReplicationInfo;
        protected LinkButton PingLdapServerButton;
        protected HyperLink GoToGroupMapprings;
        protected CommonGridView GroupsCommonGridView;
        protected CommonGridView TestDeactivatedLoginsCommonGridView;
        protected CommonGridView TestActivatedLoginsCommonGridView;
        protected CommonGridView TestCreatedLoginsCommonGridView;
        protected CommonGridView RealDeactivatedLoginsCommonGridView;
        protected CommonGridView RealActivatedLoginsCommonGridView;
        protected CommonGridView RealCreatedLoginsCommonGridView;
        protected ComboBox DomainsComboBox;
        protected Label m_OldPassword;
        protected TextBox m_LdapUpdatePassword;
        protected TextBox m_LdapConfirmNewPassword;
        protected CustomValidator PasswordCompareValidator;
        protected Label m_ChangePasswordErrorLabel;
        protected Label DeactivatedLoginsLabel;
        protected Label ActivatedLoginsLabel;
        protected Label CreatedLoginsLabel;
        protected Label DeactivatedLoginsLabel2;
        protected Label ActivatedLoginsLabel2;
        protected Label CreatedLoginsLabel2;
        protected Label PingLdapServerResultLabel;
        protected UpdateProgress PingLdapServerUpdateProgress;
        protected UpdateProgress GoToGroupMappringsUpdateProgress;
        protected Label TestDeactivatedLoginsLabel;
        protected Label TestActivatedLoginsLabel;
        protected Label TestCreatedLoginsLabel;
        protected Label RealDeactivatedLoginsLabel;
        protected Label RealActivatedLoginsLabel;
        protected Label RealCreatedLoginsLabel;
        protected Label GetGroupsLabel;
        protected HiddenField CheckLdapServerAddressErrorTextHidden;

        //GetDomains controls
        protected System.Web.UI.Timer GetDomainsTimer;
        protected MultiView GetDomainsMultiView;
        protected View GetDomainsViewProcess;
        protected View GetDomainsViewError;
        protected View GetDomainsViewResult;
        protected Image GetDomainsViewProcessImage;
        protected Literal GetDomainsViewProcessLiteral;
        protected Literal GetDomainsViewProcessResultLiteral;
        protected Literal GetDomainsViewErrorLiteral;

        //GetGroups controls
        protected System.Web.UI.Timer GetGroupsTimer;
        protected MultiView GetGroupsMultiView;
        protected View GetGroupsViewProcess;
        protected View GetGroupsViewError;
        protected View GetGroupsViewResult;
        protected Image GetGroupsViewProcessImage;
        protected Literal GetGroupsViewProcessLiteral;
        protected Literal GetGroupsViewProcessResultLiteral;
        protected Literal GetGroupsViewErrorLiteral;

        //TestADReplication controls
        protected System.Web.UI.Timer TestADReplicationTimer;
        protected MultiView TestADReplicationMultiView;
        protected View TestADReplicationViewProcess;
        protected View TestADReplicationViewError;
        protected View TestADReplicationViewResult;
        protected Image TestADReplicationViewProcessImage;
        protected Literal TestADReplicationViewProcessLiteral;
        protected Label TestADReplicationViewProcessResultLabel;
        protected Literal TestADReplicationViewErrorLiteral;

        //RealADReplication controls
        protected System.Web.UI.Timer RealADReplicationTimer;
        protected MultiView RealADReplicationMultiView;
        protected View RealADReplicationViewProcess;
        protected View RealADReplicationViewError;
        protected View RealADReplicationViewResult;
        protected Image RealADReplicationViewProcessImage;
        protected Literal RealADReplicationViewProcessLiteral;
        protected Label RealADReplicationViewProcessResultLabel;
        protected Literal RealADReplicationViewErrorLiteral;

        //CheckPort controls
        protected Label CheckPortResultLabel;
        protected UpdateProgress CheckPortUpdateProgress;
        protected LinkButton CheckPortButton;

        protected Telerik.Web.UI.RadTabStrip rtsTestReplicationProcess;
        protected Telerik.Web.UI.RadTabStrip rtsTestReplicationResult;
        protected Telerik.Web.UI.RadMultiPage rmpTestReplicationResult;
        protected Telerik.Web.UI.RadMultiPage rmpTestReplicationProcess;
        protected Label TestADReplicationViewResultLabel;

        protected Telerik.Web.UI.RadTabStrip rtsRealReplicationProcess;
        protected Telerik.Web.UI.RadTabStrip rtsRealReplicationResult;
        protected Telerik.Web.UI.RadMultiPage rmpRealReplicationResult;
        protected Telerik.Web.UI.RadMultiPage rmpRealReplicationProcess;
        protected Label RealADReplicationViewResultLabel;

        protected Label DescriptionLabel;
        protected Literal LdapSetupLabel;
        protected Label Step1Label;
        protected Label Step2Label;
        protected Label Step3Label;
        protected Label Step4Label;
        protected Label Step5Label;
        protected Label Step6Label;
        protected Label Step7Label;


        #endregion

        #region Private Properties

        private string ClientScripts
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (LdapUpdatePassword != null && LdapConfirmNewPassword != null && ChangePasswordErrorLabel != null)
                {
                    sb.Append("function PasswordCompareValidation(source, arguments) { arguments.IsValid = true; ");
                    sb.AppendFormat("var Elem1 = document.getElementById('{0}_txt'); ", LdapUpdatePassword.ClientID);
                    sb.AppendFormat("var Elem2 = document.getElementById('{0}_txt'); ", LdapConfirmNewPassword.ClientID);
                    sb.AppendFormat("var Elem3 = document.getElementById('{0}'); ", ChangePasswordErrorLabel.ClientID);
                    sb.Append("if (Elem1 && Elem2) { arguments.IsValid = (Elem2.value == Elem1.value); if (Elem3) Elem3.style.display = 'none'; } }\r\n");
                }

                return sb.ToString();
            }
        }

        private Label OldPassword
        {
            get
            {
                if (m_OldPassword == null) m_OldPassword = EditForm.FindControl("OldPassword") as Label;
                return m_OldPassword;
            }
        }

        private TextBox LdapUpdatePassword
        {
            get
            {
                if (m_LdapUpdatePassword == null) m_LdapUpdatePassword = EditForm.FindControl("LdapUpdatePassword") as TextBox;
                return m_LdapUpdatePassword;
            }
        }

        private TextBox LdapConfirmNewPassword
        {
            get
            {
                if (m_LdapConfirmNewPassword == null) m_LdapConfirmNewPassword = EditForm.FindControl("LdapConfirmNewPassword") as TextBox;
                return m_LdapConfirmNewPassword;
            }
        }

        private Label ChangePasswordErrorLabel
        {
            get
            {
                if (m_ChangePasswordErrorLabel == null) m_ChangePasswordErrorLabel = EditForm.FindControl("ChangePasswordErrorLabel") as Label;
                return m_ChangePasswordErrorLabel;
            }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets the password comparison error message from resources.
        /// </summary>
        protected static string PasswordCompareErrorMessage
        {
            get { return Resources.ChangePasswordControl_EditForm_PasswordCompareValidator_ErrorMessage; }
        }

        #endregion

        #region Protected Methods

        protected override void EditFormInitialize()
        {
            base.EditFormInitialize();
            EditForm.ShowCloseButton = CloseButtonVisibilityMode.None;

            EditForm.DataBound += new EventHandler(EditForm_DataBound);
        }

        protected void EditForm_DataBound(object sender, EventArgs e)
        {
            (EditForm.UpdateButton as Button).Attributes.Add("onclick", "return checkLdapServerAddress();");
        }

        protected void CheckPortButton_Click(object sender, EventArgs e)
        {
            int ldapServerPort = 0;
            string ldapServerAddress = null;
            try
            {
                ldapServerAddress = (EditForm.Rows[0].Cells[1].Controls[0] as TextBox).Text;
                if (string.IsNullOrEmpty((EditForm.Rows[1].Cells[1].Controls[0] as TextBox).Text))
                    ldapServerPort = 0;
                else
                {
                    if (!int.TryParse((EditForm.Rows[1].Cells[1].Controls[0] as TextBox).Text, out ldapServerPort))
                        ldapServerPort = 0;
                }

                CheckPortResultLabel.Visible = true;
                CheckPortResultLabel.Text = string.Format(CultureInfo.InvariantCulture, LdapInfoProvider.ScanPort(ldapServerAddress, ldapServerPort) ? Resources.OrganizationLdapSettingsControl_CheckPortResultLabel_Text_Open : Resources.OrganizationLdapSettingsControl_CheckPortResultLabel_Text_Closed, ldapServerPort);
            }
            finally
            {
                ldapServerPort = 0;
                ldapServerAddress = null;
            }
        }

        protected void PingLdapServerButton_Click(object sender, EventArgs e)
        {
            int ldapServerPort = 0;
            string ldapServerAddress = null;
            string ldapUserName = null;
            string ldapPassword = null;
            string ldapDomain = null;
            try
            {
                ldapServerAddress = (EditForm.Rows[0].Cells[1].Controls[0] as TextBox).Text;
                if (string.IsNullOrEmpty((EditForm.Rows[1].Cells[1].Controls[0] as TextBox).Text))
                    ldapServerPort = 0;
                else
                {
                    if (!int.TryParse((EditForm.Rows[1].Cells[1].Controls[0] as TextBox).Text, out ldapServerPort))
                        ldapServerPort = 0;
                }
                ldapUserName = (EditForm.Rows[2].Cells[1].Controls[0] as TextBox).Text;
                if (!string.IsNullOrEmpty(LdapUpdatePassword.Text) && !string.IsNullOrEmpty(LdapConfirmNewPassword.Text) && LdapUpdatePassword.Text == LdapConfirmNewPassword.Text)
                    ldapPassword = LdapUpdatePassword.Text;
                else
                    ldapPassword = OldPassword.Text.ToString();
                ldapDomain = (EditForm.Rows[3].Cells[1].Controls[0] as TextBox).Text;

                PingLdapServerResultLabel.Visible = true;
                PingLdapServerResultLabel.Text = LdapInfoProvider.PingLdapServer(ldapServerAddress, ldapServerPort, ldapUserName, ldapPassword, ldapDomain);
            }
            finally
            {
                ldapServerPort = 0;
                ldapServerAddress = null;
                ldapUserName = null;
                ldapPassword = null;
                ldapDomain = null;
            }
        }

        protected void GetDomainsButton_Click(object sender, EventArgs e)
        {
            GetDomainsTimer.Enabled = true;
            GetDomainsMultiView.SetActiveView(GetDomainsViewProcess);
            GetDomainsViewProcessResultLiteral.Text = string.Empty;

            Thread thread = new Thread(LdapProcessGetDomains);
            thread.CurrentCulture = CultureInfo.InvariantCulture;
            thread.CurrentUICulture = CultureInfo.CurrentUICulture;
            thread.Priority = ThreadPriority.Lowest;
            thread.IsBackground = true;
            thread.Start(UserContext.Current);
            GetDomainsButton.Enabled = false;
        }

        protected void GetDomainsTimer_Tick(object sender, EventArgs e)
        {
            ShowResultsGetDomains();
        }

        protected void GetGroupsButton_Click(object sender, EventArgs e)
        {
            if (DomainsComboBox.Visible == true && DomainsComboBox.Items.Count > 0)
            {
                GetGroupsTimer.Enabled = true;
                GetGroupsMultiView.SetActiveView(GetGroupsViewProcess);
                GetGroupsViewProcessResultLiteral.Text = string.Empty;

                Thread thread = new Thread(LdapProcessGetGroups);
                thread.CurrentCulture = CultureInfo.InvariantCulture;
                thread.CurrentUICulture = CultureInfo.CurrentUICulture;
                thread.Priority = ThreadPriority.Lowest;
                thread.IsBackground = true;
                thread.Start(UserContext.Current);

                GetGroupsButton.Enabled = false;
            }
        }

        protected void GetGroupsTimer_Tick(object sender, EventArgs e)
        {
            ShowResultsGetGroups();
        }

        protected void GetTestAdReplicationInfo_Click(object sender, EventArgs e)
        {
            TestADReplicationTimer.Enabled = true;
            TestADReplicationMultiView.SetActiveView(TestADReplicationViewProcess);
            TestADReplicationViewProcessResultLabel.Text = string.Empty;

            rtsTestReplicationProcess.Visible = false;
            rtsTestReplicationResult.Visible = false;
            rmpTestReplicationProcess.Visible = false;
            rmpTestReplicationResult.Visible = false;

            Thread thread = new Thread(LdapProcessTestADReplication);
            thread.CurrentCulture = CultureInfo.InvariantCulture;
            thread.CurrentUICulture = CultureInfo.CurrentUICulture;
            thread.Priority = ThreadPriority.Lowest;
            thread.IsBackground = false;
            thread.Start(UserContext.Current);

            GetTestAdReplicationInfo.Enabled = false;
        }

        protected void TestADReplicationTimer_Tick(object sender, EventArgs e)
        {
            ShowResultsTestADReplication();
        }

        protected void GetRealAdReplicationInfo_Click(object sender, EventArgs e)
        {
            RealADReplicationTimer.Enabled = true;
            RealADReplicationMultiView.SetActiveView(RealADReplicationViewProcess);
            RealADReplicationViewProcessResultLabel.Text = string.Empty;

            rtsRealReplicationProcess.Visible = false;
            rtsRealReplicationResult.Visible = false;
            rmpRealReplicationProcess.Visible = false;
            rmpRealReplicationResult.Visible = false;

            Thread thread = new Thread(LdapProcessRealADReplication);
            thread.CurrentCulture = CultureInfo.InvariantCulture;
            thread.CurrentUICulture = CultureInfo.CurrentUICulture;
            thread.Priority = ThreadPriority.Lowest;
            thread.IsBackground = true;
            thread.Start(UserContext.Current);

            GetRealAdReplicationInfo.Enabled = false;
        }

        protected void RealADReplicationTimer_Tick(object sender, EventArgs e)
        {
            ShowResultsRealADReplication();
        }

        protected void LdapProcessGetDomains(object arg)
        {
            UserContext userContext = null;
            string processId = null;
            Bll.LdapProcess ldapProcess = null;
            string ldapServerAddress = null;
            int ldapServerPort = 0;
            string ldapUserName = null;
            string ldapPassword = null;
            string ldapDomain = null;
            try
            {
                userContext = arg as UserContext;
                if (userContext != null)
                {
                    processId = string.Format(CultureInfo.InvariantCulture, "GetDomains_{0}", userContext.SelectedOrganization.OrganizationId);

                    ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                    if (ldapProcess != null)
                        LdapInfoProvider.LdapProcesses.Remove(ldapProcess);

                    ldapProcess = new Bll.LdapProcess();
                    ldapProcess.ProcessId = processId;
                    ldapProcess.ThreadStateType = Bll.ThreadStateType.Running;
                    ldapProcess.MessageError = string.Empty;
                    ldapProcess.Message = string.Empty;
                    ldapProcess.Data = null;
                    LdapInfoProvider.LdapProcesses.Add(ldapProcess);

                    ldapServerAddress = (EditForm.Rows[0].Cells[1].Controls[0] as TextBox).Text;
                    if (string.IsNullOrEmpty((EditForm.Rows[1].Cells[1].Controls[0] as TextBox).Text) == true)
                        ldapServerPort = 0;
                    else
                        if (!int.TryParse((EditForm.Rows[1].Cells[1].Controls[0] as TextBox).Text, out ldapServerPort))
                            ldapServerPort = 0;
                    ldapUserName = (EditForm.Rows[2].Cells[1].Controls[0] as TextBox).Text;
                    if (string.IsNullOrEmpty(LdapUpdatePassword.Text) == false && string.IsNullOrEmpty(LdapConfirmNewPassword.Text) == false && LdapUpdatePassword.Text == LdapConfirmNewPassword.Text)
                        ldapPassword = LdapUpdatePassword.Text;
                    else
                        ldapPassword = OldPassword.Text.ToString();
                    ldapDomain = (EditForm.Rows[3].Cells[1].Controls[0] as TextBox).Text;

                    ldapProcess.Data = LdapInfoProvider.GetDomains(ldapServerAddress, ldapServerPort, ldapUserName, ldapPassword, ldapDomain);
                    ldapProcess.ThreadStateType = Bll.ThreadStateType.Finished;
                }
            }
            catch (Exception ex)
            {
                ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                if (ldapProcess != null)
                {
                    ldapProcess.ProcessId = processId;
                    ldapProcess.ThreadStateType = Bll.ThreadStateType.Failed;
                    ldapProcess.MessageError = string.Format(CultureInfo.InvariantCulture, "<br/>{0}", ex.ToString().Replace("\r\n", "<br/>"));
                    ldapProcess.Message = string.Empty;
                    ldapProcess.Data = null;
                }
            }
            finally
            {
                userContext = null;
                processId = null;
                ldapProcess = null;
                ldapServerAddress = null;
                ldapServerPort = 0;
                ldapUserName = null;
                ldapPassword = null;
                ldapDomain = null;
            }
        }

        protected void LdapProcessGetGroups(object arg)
        {
            UserContext userContext = null;
            string processId = null;

            Bll.LdapProcess ldapProcess = null;
            string ldapServerAddress = null;
            int ldapServerPort = 0;
            string ldapUserName = null;
            string ldapPassword = null;
            string ldapDomain = null;
            Bll.Handlers.LdapHandler ldapHendler = null;
            try
            {
                userContext = arg as UserContext;
                if (userContext != null)
                {
                    processId = string.Format(CultureInfo.InvariantCulture, "GetGroups_{0}", userContext.SelectedOrganization.OrganizationId);
                    ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                    if (ldapProcess != null)
                        LdapInfoProvider.LdapProcesses.Remove(ldapProcess);

                    ldapProcess = new Bll.LdapProcess();
                    ldapProcess.ProcessId = processId;
                    ldapProcess.ThreadStateType = Bll.ThreadStateType.Running;
                    ldapProcess.MessageError = string.Empty;
                    ldapProcess.Message = string.Empty;
                    ldapProcess.Data = null;
                    LdapInfoProvider.LdapProcesses.Add(ldapProcess);

                    ldapServerAddress = (EditForm.Rows[0].Cells[1].Controls[0] as TextBox).Text;
                    if (string.IsNullOrEmpty((EditForm.Rows[1].Cells[1].Controls[0] as TextBox).Text) == true)
                        ldapServerPort = 0;
                    else
                        if (!int.TryParse((EditForm.Rows[1].Cells[1].Controls[0] as TextBox).Text, out ldapServerPort))
                            ldapServerPort = 0;
                    ldapUserName = (EditForm.Rows[2].Cells[1].Controls[0] as TextBox).Text;
                    if (string.IsNullOrEmpty(LdapUpdatePassword.Text) == false && string.IsNullOrEmpty(LdapConfirmNewPassword.Text) == false && LdapUpdatePassword.Text == LdapConfirmNewPassword.Text)
                        ldapPassword = LdapUpdatePassword.Text;
                    else
                        ldapPassword = OldPassword.Text.ToString();
                    ldapDomain = (EditForm.Rows[3].Cells[1].Controls[0] as TextBox).Text;

                    ldapHendler = new Bll.Handlers.LdapHandler();
                    ldapHendler.ImportLdapGroups(userContext.SelectedOrganization.OrganizationId);

                    ldapProcess.Data = LdapInfoProvider.GetGroups(ldapServerAddress, ldapServerPort, ldapUserName, ldapPassword, ldapDomain, DomainsComboBox.SelectedItem.Text, userContext.SelectedOrganization.OrganizationId);
                    ldapProcess.ThreadStateType = Bll.ThreadStateType.Finished;

                }
            }
            catch (Exception ex)
            {
                ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                if (ldapProcess != null)
                {
                    ldapProcess.ProcessId = processId;
                    ldapProcess.ThreadStateType = Bll.ThreadStateType.Failed;
                    ldapProcess.MessageError = string.Format(CultureInfo.InvariantCulture, "<br/>{0}", ex.ToString().Replace("\r\n", "<br/>"));
                    ldapProcess.Message = string.Empty;
                    ldapProcess.Data = null;
                }
            }
            finally
            {
                userContext = null;
                processId = null;
                ldapProcess = null;
                ldapServerAddress = null;
                ldapServerPort = 0;
                ldapUserName = null;
                ldapPassword = null;
                ldapDomain = null;
                ldapHendler = null;
            }
        }

        protected void LdapProcessTestADReplication(object arg)
        {
            LdapHandler ldap = new LdapHandler();
            ldap.RunADReplication((arg as UserContext).SelectedOrganization.OrganizationId, false);
        }

        protected void LdapProcessRealADReplication(object arg)
        {
            LdapHandler ldap = new LdapHandler();
            ldap.RunADReplication((arg as UserContext).SelectedOrganization.OrganizationId, true);
        }

        protected void ShowResultsGetDomains()
        {
            Bll.LdapProcess ldapProcess = null;
            string processId = string.Format(CultureInfo.InvariantCulture, "GetDomains_{0}", UserContext.Current.SelectedOrganizationId);
            try
            {
                GetDomainsTimer.Enabled = false;
                ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                if (ldapProcess != null)
                {
                    switch (ldapProcess.ThreadStateType)
                    {
                        case Bll.ThreadStateType.Failed:
                            GetDomainsMultiView.SetActiveView(GetDomainsViewError);
                            GetDomainsViewErrorLiteral.Text = ldapProcess.MessageError;
                            LdapInfoProvider.LdapProcesses.Remove(ldapProcess);
                            GetDomainsButton.Enabled = true;
                            break;

                        case Bll.ThreadStateType.Finished:
                            GetDomainsMultiView.SetActiveView(GetDomainsViewResult);
                            DomainsComboBox.DataSource = ldapProcess.Data as DataView;
                            DomainsComboBox.DataBind();
                            LdapInfoProvider.LdapProcesses.Remove(ldapProcess);
                            GetDomainsButton.Enabled = true;
                            break;

                        case Bll.ThreadStateType.Running:
                            GetDomainsMultiView.SetActiveView(GetDomainsViewProcess);
                            GetDomainsViewProcessResultLiteral.Text = string.Empty;
                            if (!string.IsNullOrEmpty(ldapProcess.Message))
                                GetDomainsViewProcessResultLiteral.Text = string.Format(CultureInfo.InvariantCulture, "<br/>{0}", ldapProcess.Message);

                            GetDomainsTimer.Enabled = true;
                            GetDomainsButton.Enabled = false;
                            break;
                    }
                }
            }
            finally
            {
                ldapProcess = null;
                processId = null;
            }
        }

        protected void ShowResultsGetGroups()
        {
            Bll.LdapProcess ldapProcess = null;
            string processId = string.Format(CultureInfo.InvariantCulture, "GetGroups_{0}", UserContext.Current.SelectedOrganizationId);
            try
            {
                GetGroupsTimer.Enabled = false;
                ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                if (ldapProcess != null)
                {
                    switch (ldapProcess.ThreadStateType)
                    {
                        case Bll.ThreadStateType.Failed:
                            GetGroupsMultiView.SetActiveView(GetGroupsViewError);
                            GetGroupsViewErrorLiteral.Text = ldapProcess.MessageError;
                            LdapInfoProvider.LdapProcesses.Remove(ldapProcess);
                            GetGroupsButton.Enabled = true;
                            break;

                        case Bll.ThreadStateType.Finished:
                            GetGroupsMultiView.SetActiveView(GetGroupsViewResult);
                            GroupsCommonGridView.DataSource = ldapProcess.Data as DataView;
                            GroupsCommonGridView.DataBind();
                            LdapInfoProvider.LdapProcesses.Remove(ldapProcess);
                            GetGroupsButton.Enabled = true;
                            break;

                        case Bll.ThreadStateType.Running:
                            GetGroupsMultiView.SetActiveView(GetGroupsViewProcess);
                            GetGroupsViewProcessResultLiteral.Text = string.Empty;
                            if (!string.IsNullOrEmpty(ldapProcess.Message))
                                GetGroupsViewProcessResultLiteral.Text = string.Format(CultureInfo.InvariantCulture, "<br/>{0}", ldapProcess.Message);
                            GetGroupsTimer.Enabled = true;
                            GetGroupsButton.Enabled = false;
                            break;
                    }
                }
            }
            finally
            {
                ldapProcess = null;
                processId = null;
            }
        }

        protected void ShowResultsTestADReplication()
        {
            Bll.LdapProcess ldapProcess = null;
            string processId = string.Format(CultureInfo.InvariantCulture, "TestADReplication_{0}", UserContext.Current.SelectedOrganizationId);
            try
            {
                TestADReplicationTimer.Enabled = false;
                ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                if (ldapProcess != null)
                {
                    switch (ldapProcess.ThreadStateType)
                    {
                        case Bll.ThreadStateType.Failed:
                            TestADReplicationMultiView.SetActiveView(TestADReplicationViewError);
                            TestADReplicationViewErrorLiteral.Text = ldapProcess.MessageError;
                            LdapInfoProvider.LdapProcesses.Remove(ldapProcess);
                            GetTestAdReplicationInfo.Enabled = true;
                            break;

                        case Bll.ThreadStateType.Finished:
                            GetTestAdReplicationInfo.Enabled = true;
                            TestADReplicationMultiView.SetActiveView(TestADReplicationViewResult);

                            CreatedLoginsLabel.Text = ldapProcess.MessageCreatedLogins;
                            CreatedLoginsLabel.Font.Size = FontUnit.Medium;
                            CreatedLoginsLabel.Visible = true;
                            if (ldapProcess.DataCreatedLogins != null)
                            {
                                TestCreatedLoginsCommonGridView.Visible = true;
                                TestCreatedLoginsLabel.Visible = (ldapProcess.DataCreatedLogins.Table.Rows.Count >= 25);
                                TestCreatedLoginsCommonGridView.DataSource = ldapProcess.DataCreatedLogins;
                                TestCreatedLoginsCommonGridView.DataBind();
                            }
                            else
                                TestCreatedLoginsCommonGridView.Visible = false;

                            ActivatedLoginsLabel.Text = ldapProcess.MessageActivatedLogins;
                            ActivatedLoginsLabel.Font.Size = FontUnit.Medium;
                            ActivatedLoginsLabel.Visible = true;
                            if (ldapProcess.DataActivatedLogins != null)
                            {
                                TestActivatedLoginsCommonGridView.Visible = true;
                                TestActivatedLoginsLabel.Visible = (ldapProcess.DataActivatedLogins.Table.Rows.Count >= 25);
                                TestActivatedLoginsCommonGridView.DataSource = ldapProcess.DataActivatedLogins;
                                TestActivatedLoginsCommonGridView.DataBind();
                            }
                            else
                                TestActivatedLoginsCommonGridView.Visible = false;


                            DeactivatedLoginsLabel.Text = ldapProcess.MessageDeactivatedLogins;
                            DeactivatedLoginsLabel.Font.Size = FontUnit.Medium;
                            DeactivatedLoginsLabel.Visible = true;
                            if (ldapProcess.DataDeactivatedLogins != null)
                            {
                                TestDeactivatedLoginsCommonGridView.Visible = true;
                                TestDeactivatedLoginsLabel.Visible = (ldapProcess.DataDeactivatedLogins.Table.Rows.Count >= 25);
                                TestDeactivatedLoginsCommonGridView.DataSource = ldapProcess.DataDeactivatedLogins;
                                TestDeactivatedLoginsCommonGridView.DataBind();
                            }
                            else
                                TestDeactivatedLoginsCommonGridView.Visible = false;

                            rtsTestReplicationResult.Visible = true;
                            rmpTestReplicationResult.Visible = true;
                            rtsTestReplicationResult.SelectedIndex = 1;
                            rmpTestReplicationResult.SelectedIndex = 1;
                            TestADReplicationViewResultLabel.Text = "";
                            foreach (LdapProcessLog log in ldapProcess.Logs)
                            {
                                TestADReplicationViewResultLabel.Text += string.Format(CultureInfo.CurrentCulture, "{0} - {1}<br/>", log.Date, log.Message);
                            }

                            LdapInfoProvider.LdapProcesses.Remove(ldapProcess);
                            break;

                        case Bll.ThreadStateType.Running:
                            TestADReplicationMultiView.SetActiveView(TestADReplicationViewProcess);

                            rtsTestReplicationProcess.Visible = true;
                            rmpTestReplicationProcess.Visible = true;
                            rtsTestReplicationProcess.Tabs[1].Enabled = false;
                            rtsTestReplicationProcess.SelectedIndex = 0;
                            TestADReplicationViewProcessResultLabel.Text = "";
                            foreach (LdapProcessLog log in ldapProcess.Logs)
                            {
                                TestADReplicationViewProcessResultLabel.Text += string.Format(CultureInfo.CurrentCulture, "{0} - {1}<br/>", log.Date, log.Message);
                            }
                            //TestADReplicationViewProcessResultLabel.Text = string.Format(CultureInfo.InvariantCulture, "<br/>{0}<br/><br/>", ldapProcess.MessageDeactivatedLogins);
                            //TestADReplicationViewProcessResultLabel.Text += string.Format(CultureInfo.InvariantCulture, "{0}<br/><br/>", ldapProcess.MessageActivatedLogins);
                            //TestADReplicationViewProcessResultLabel.Text += string.Format(CultureInfo.InvariantCulture, "{0}<br/>", ldapProcess.MessageCreatedLogins);
                            //TestADReplicationViewProcessResultLabel.Font.Size = FontUnit.Medium;
                            TestADReplicationTimer.Enabled = true;
                            GetTestAdReplicationInfo.Enabled = false;
                            break;
                    }
                }
            }
            finally
            {
                ldapProcess = null;
                processId = null;
            }
        }

        protected void ShowResultsRealADReplication()
        {
            Bll.LdapProcess ldapProcess = null;
            string processId = string.Format(CultureInfo.InvariantCulture, "RealADReplication_{0}", UserContext.Current.SelectedOrganizationId);
            try
            {
                RealADReplicationTimer.Enabled = false;
                ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                if (ldapProcess != null)
                {
                    switch (ldapProcess.ThreadStateType)
                    {
                        case Bll.ThreadStateType.Failed:
                            GetRealAdReplicationInfo.Enabled = true;
                            RealADReplicationMultiView.SetActiveView(RealADReplicationViewError);
                            RealADReplicationViewErrorLiteral.Text = ldapProcess.MessageError;
                            LdapInfoProvider.LdapProcesses.Remove(ldapProcess);
                            break;

                        case Bll.ThreadStateType.Finished:
                            GetRealAdReplicationInfo.Enabled = true;
                            RealADReplicationMultiView.SetActiveView(RealADReplicationViewResult);

                            CreatedLoginsLabel2.Text = ldapProcess.MessageCreatedLogins;
                            CreatedLoginsLabel2.Font.Size = FontUnit.Medium;
                            CreatedLoginsLabel2.Visible = true;
                            if (ldapProcess.DataCreatedLogins != null)
                            {
                                RealCreatedLoginsCommonGridView.Visible = true;
                                RealCreatedLoginsLabel.Visible = (ldapProcess.DataCreatedLogins.Table.Rows.Count >= 25);
                                RealCreatedLoginsCommonGridView.DataSource = ldapProcess.DataCreatedLogins;
                                RealCreatedLoginsCommonGridView.DataBind();
                            }
                            else
                                RealCreatedLoginsCommonGridView.Visible = false;

                            ActivatedLoginsLabel2.Text = ldapProcess.MessageActivatedLogins;
                            ActivatedLoginsLabel2.Font.Size = FontUnit.Medium;
                            ActivatedLoginsLabel2.Visible = true;
                            if (ldapProcess.DataActivatedLogins != null)
                            {
                                RealActivatedLoginsCommonGridView.Visible = true;
                                RealActivatedLoginsLabel.Visible = (ldapProcess.DataActivatedLogins.Table.Rows.Count >= 25);
                                RealActivatedLoginsCommonGridView.DataSource = ldapProcess.DataActivatedLogins;
                                RealActivatedLoginsCommonGridView.DataBind();
                            }
                            else
                                RealActivatedLoginsCommonGridView.Visible = false;


                            DeactivatedLoginsLabel2.Text = ldapProcess.MessageDeactivatedLogins;
                            DeactivatedLoginsLabel2.Font.Size = FontUnit.Medium;
                            DeactivatedLoginsLabel2.Visible = true;
                            if (ldapProcess.DataDeactivatedLogins != null)
                            {
                                RealDeactivatedLoginsCommonGridView.Visible = true;
                                RealDeactivatedLoginsLabel.Visible = (ldapProcess.DataDeactivatedLogins.Table.Rows.Count >= 25);
                                RealDeactivatedLoginsCommonGridView.DataSource = ldapProcess.DataDeactivatedLogins;
                                RealDeactivatedLoginsCommonGridView.DataBind();
                            }
                            else
                                RealDeactivatedLoginsCommonGridView.Visible = false;


                            rtsRealReplicationResult.Visible = true;
                            rmpRealReplicationResult.Visible = true;
                            rtsRealReplicationResult.SelectedIndex = 1;
                            rmpRealReplicationResult.SelectedIndex = 1;
                            RealADReplicationViewResultLabel.Text = "";
                            foreach (LdapProcessLog log in ldapProcess.Logs)
                            {
                                RealADReplicationViewResultLabel.Text += string.Format(CultureInfo.CurrentCulture, "{0} - {1}<br/>", log.Date, log.Message);
                            }

                            LdapInfoProvider.LdapProcesses.Remove(ldapProcess);
                            break;

                        case Bll.ThreadStateType.Running:
                            RealADReplicationMultiView.SetActiveView(RealADReplicationViewProcess);

                            rtsRealReplicationProcess.Visible = true;
                            rmpRealReplicationProcess.Visible = true;
                            rtsRealReplicationProcess.Tabs[1].Enabled = false;
                            rtsRealReplicationProcess.SelectedIndex = 0;
                            RealADReplicationViewProcessResultLabel.Text = "";
                            foreach (LdapProcessLog log in ldapProcess.Logs)
                            {
                                RealADReplicationViewProcessResultLabel.Text += string.Format(CultureInfo.CurrentCulture, "{0} - {1}<br/>", log.Date, log.Message);
                            }
                            //RealADReplicationViewProcessResultLabel.Text = string.Format(CultureInfo.InvariantCulture, "<br/>{0}<br/><br/>", ldapProcess.MessageDeactivatedLogins);
                            //RealADReplicationViewProcessResultLabel.Text += string.Format(CultureInfo.InvariantCulture, "{0}<br/><br/>", ldapProcess.MessageActivatedLogins);
                            //RealADReplicationViewProcessResultLabel.Text += string.Format(CultureInfo.InvariantCulture, "{0}<br/>", ldapProcess.MessageCreatedLogins);
                            //RealADReplicationViewProcessResultLabel.Font.Size = FontUnit.Medium;
                            RealADReplicationTimer.Enabled = true;
                            GetRealAdReplicationInfo.Enabled = false;
                            break;
                    }
                }
            }
            finally
            {
                ldapProcess = null;
                processId = null;
            }
        }

        protected void EntityDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["organizationId"] = UserContext.Current.SelectedOrganizationId;
        }

        protected void EntityDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            e.InputParameters["ldapUpdatePassword"] = this.LdapUpdatePassword.Text;
            e.InputParameters["ldapConfirmNewPassword"] = this.LdapConfirmNewPassword.Text;
        }

        #endregion

        #region Overriden Methods

        protected override void LoadResources()
        {
            BaseControl.LoadResources(EditForm, typeof(OrganizationLdapSettingsControl).Name);
            EditForm.Fields[4].HeaderText = Resources.OrganizationLdapSettingsControl_EditForm_LdapUpdatePassword_HeaderText;
            EditForm.Fields[5].HeaderText = Resources.OrganizationLdapSettingsControl_EditForm_LdapConfirmNewPassword_HeaderText;

            GetDomainsButton.Text = Resources.OrganizationLdapSettingsControl_GetDomainsButton_Text;
            GetGroupsButton.Text = Resources.OrganizationLdapSettingsControl_GetGroupsButton_Text;
            GetTestAdReplicationInfo.Text = Resources.OrganizationLdapSettingsControl_GetTestAdReplicationInfo_Text;
            GetRealAdReplicationInfo.Text = Resources.OrganizationLdapSettingsControl_GetRealAdReplicationInfo_Text;
            PingLdapServerButton.Text = Resources.OrganizationLdapSettingsControl_PingLdapServerButton_Text;
            CheckPortButton.Text = Resources.OrganizationLdapSettingsControl_CheckPortButton_Text;

            PingLdapServerUpdateProgress.ProgressText = Resources.OrganizationLdapSettingsControl_UpdateProgress_Text;
            PingLdapServerUpdateProgress.Timeout = int.MaxValue;
            PingLdapServerUpdateProgress.HideAfter = -1;
            PingLdapServerUpdateProgress.ShowSuccessText = false;
            PingLdapServerUpdateProgress.PostBackControlId = this.PingLdapServerButton.ClientID;

            GoToGroupMappringsUpdateProgress.ProgressText = Resources.OrganizationLdapSettingsControl_UpdateProgress_Text;
            GoToGroupMappringsUpdateProgress.Timeout = int.MaxValue;
            GoToGroupMappringsUpdateProgress.HideAfter = -1;
            GoToGroupMappringsUpdateProgress.ShowSuccessText = false;
            GoToGroupMappringsUpdateProgress.PostBackControlId = this.GoToGroupMapprings.ClientID;

            CheckPortUpdateProgress.ProgressText = Resources.OrganizationLdapSettingsControl_UpdateProgress_Text;
            CheckPortUpdateProgress.Timeout = int.MaxValue;
            CheckPortUpdateProgress.HideAfter = int.MaxValue;
            CheckPortUpdateProgress.ShowSuccessText = false;
            CheckPortUpdateProgress.PostBackControlId = this.PingLdapServerButton.ClientID;

            Bll.Action action = ActionProvider.FindAction(ActionProvider.LdapGroupMappingsPageActionId);
            if (action != null)
            {
                GoToGroupMapprings.Visible = true;
                GoToGroupMapprings.Text = action.Name;
                GoToGroupMapprings.NavigateUrl = ResolveUrl(string.Format(CultureInfo.InvariantCulture, "~{0}", action.NavigateUrl));
            }
            else
                GoToGroupMapprings.Visible = false;

            TestDeactivatedLoginsLabel.Text = Resources.OrganizationLdapSettingsControl_LimitGridLabel_Text;
            TestActivatedLoginsLabel.Text = Resources.OrganizationLdapSettingsControl_LimitGridLabel_Text;
            TestCreatedLoginsLabel.Text = Resources.OrganizationLdapSettingsControl_LimitGridLabel_Text;
            RealDeactivatedLoginsLabel.Text = Resources.OrganizationLdapSettingsControl_LimitGridLabel_Text;
            RealActivatedLoginsLabel.Text = Resources.OrganizationLdapSettingsControl_LimitGridLabel_Text;
            RealCreatedLoginsLabel.Text = Resources.OrganizationLdapSettingsControl_LimitGridLabel_Text;
            GetGroupsLabel.Text = Resources.OrganizationLdapSettingsControl_LimitGroupGridLabel_Text;

            //GetDomains
            GetDomainsViewProcessLiteral.Text = Resources.OrganizationLdapSettingsControl_UpdateProgress_Text;
            GetDomainsViewProcessImage.ImageAlign = ImageAlign.AbsMiddle;
            GetDomainsViewProcessImage.ImageUrl = ResourceProvider.GetImageUrl(typeof(UpdateProgress), "Loader.gif", true);

            //GetGroups
            GetGroupsViewProcessLiteral.Text = Resources.OrganizationLdapSettingsControl_UpdateProgress_Text;
            GetGroupsViewProcessImage.ImageAlign = ImageAlign.AbsMiddle;
            GetGroupsViewProcessImage.ImageUrl = ResourceProvider.GetImageUrl(typeof(UpdateProgress), "Loader.gif", true);

            //TestADReplication
            TestADReplicationViewProcessLiteral.Text = Resources.OrganizationLdapSettingsControl_UpdateProgress_Text;
            TestADReplicationViewProcessImage.ImageAlign = ImageAlign.AbsMiddle;
            TestADReplicationViewProcessImage.ImageUrl = ResourceProvider.GetImageUrl(typeof(UpdateProgress), "Loader.gif", true);

            //RealADReplication
            RealADReplicationViewProcessLiteral.Text = Resources.OrganizationLdapSettingsControl_UpdateProgress_Text;
            RealADReplicationViewProcessImage.ImageAlign = ImageAlign.AbsMiddle;
            RealADReplicationViewProcessImage.ImageUrl = ResourceProvider.GetImageUrl(typeof(UpdateProgress), "Loader.gif", true);

            CheckLdapServerAddressErrorTextHidden.Value = Resources.OrganizationLdapSettingsControl_CheckLdapServerAddress_ErrorText;

            DescriptionLabel.Text = Resources.OrganizationLdapSettingsControl_Description_Text;
            LdapSetupLabel.Text = Resources.OrganizationLdapSettingsControl_LdapSetup_Text;
            Step1Label.Text = Resources.OrganizationLdapSettingsControl_LdapSetupStep1_Text;
            Step2Label.Text = Resources.OrganizationLdapSettingsControl_LdapSetupStep2_Text;
            Step3Label.Text = Resources.OrganizationLdapSettingsControl_LdapSetupStep3_Text;
            Step4Label.Text = Resources.OrganizationLdapSettingsControl_LdapSetupStep4_Text;
            Step5Label.Text = Resources.OrganizationLdapSettingsControl_LdapSetupStep5_Text;
            Step6Label.Text = Resources.OrganizationLdapSettingsControl_LdapSetupStep6_Text;
            Step7Label.Text = Resources.OrganizationLdapSettingsControl_LdapSetupStep7_Text;
        }

        protected override void EditForm_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e == null) return;

            if (e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
            {
                this.RedirectToConfigurationPage();
            }
        }

        /// <summary>
        /// Raises the PreRender event and registers client scripts.
        /// </summary>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string scripts = ClientScripts;
            if (!string.IsNullOrEmpty(scripts))
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ClientScripts", scripts, true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                ShowResultsGetDomains();
                ShowResultsGetGroups();
                ShowResultsTestADReplication();
                ShowResultsRealADReplication();
            }
        }

        #endregion
    }
}
