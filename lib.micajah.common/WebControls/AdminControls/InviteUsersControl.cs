﻿using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls.AdminControls
{
    public class InviteUsersControl : UserControl
    {
        #region Members

        protected const int MaximumEmailsPerSend = 20;

        protected Table FormTable;
        protected Literal CaptionLiteral;
        protected Literal DescriptionLabel;
        protected Label EmailLabel;
        protected Literal EmailHintLabel;
        protected TextBox EmailTextBox;
        protected CustomValidator EmailValidator;
        protected Label GroupsLabel;
        protected CheckBoxList GroupList;
        protected Label MessageLabel;
        protected Literal MessageHintLabel;
        protected TextBox MessageTextBox;
        protected Button SendButton;
        protected PlaceHolder ButtonsSeparator;
        protected HyperLink CancelLink;

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            DescriptionLabel.Text = string.Format(CultureInfo.InvariantCulture, Resources.InviteUsersControl_DescriptionLabel_Text, MaximumEmailsPerSend);
            CaptionLiteral.Text = Resources.InviteUsersControl_FormTable_Caption;
            SendButton.Text = Resources.InviteUsersControl_SendButton_Text;
            CancelLink.Text = Resources.AutoGeneratedButtonsField_CancelButton_Text;
            EmailLabel.Text = Resources.InviteUsersControl_EmailLabel_Text;
            EmailHintLabel.Text = Resources.InviteUsersControl_EmailHintLabel_Text;
            GroupsLabel.Text = Resources.InviteUsersControl_GroupsLabel_Text;
            MessageLabel.Text = Resources.InviteUsersControl_MessageLabel_Text;
            MessageHintLabel.Text = Resources.InviteUsersControl_MessageHintLabel_Text;
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            MagicForm.ApplyStyle(FormTable, ColorScheme.White, false, true);
            AutoGeneratedButtonsField.InsertButtonSeparator(ButtonsSeparator);

            if (!this.IsPostBack)
            {
                this.LoadResources();
                CancelLink.NavigateUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.UsersPageVirtualPath);
                EmailValidator.ErrorMessage = Resources.TextBox_RegularExpressionValidator_ErrorMessage;
                
                EmailValidator.Attributes["controltovalidate2"] = EmailTextBox.ClientID;
            }
        }

        protected void SendButton_Click(object sender, EventArgs e)
        {
            UserContext user = UserContext.Current;
            LoginProvider.Current.Invite(user.UserId, user.FirstName + " " + user.LastName, user.Email, EmailTextBox.Text, user.OrganizationId, GroupList.SelectedValue, MessageTextBox.Text);
            Response.Redirect(ResourceProvider.UsersPageVirtualPath);
        }

        #endregion
    }
}
