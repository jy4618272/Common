<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.LogosControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<%@ Register Src="~/Resources.Micajah.Common/Controls/Security/ChangePassword.ascx"
    TagName="ChangePassword" TagPrefix="uc" %>
<mits:CommonGridView ID="List" runat="server" Width="700px" DataKeyNames="ObjectId,ObjectType"
    AutoGenerateEditButton="true">
    <columns>
        <mits:TextBoxField DataField="Name" SortExpression="Name" />
        <mits:TextBoxField DataField="Type" SortExpression="Type" />
        <mits:ImageField DataImageUrlField="Logo" SortExpression="Logo" />
    </columns>
</mits:CommonGridView>
<mits:magicform id="EditForm" runat="server" datakeynames="ObjectId" width="550px"
    OnDataBinding="EditForm_DataBinding" OnModeChanging="EditForm_ModeChanging" OnItemUpdating="EditForm_ItemUpdating">
    <fields>
        <mits:TemplateField>
            <ItemTemplate>
                <mits:ImageUpload ID="LogoImageUpload" runat="server" LocalObjectType='<%# Eval("ObjectType") %>' LocalObjectId='<%# Eval("ObjectId") %>' EnablePopupWindow="false" />
            </ItemTemplate>
        </mits:TemplateField>                
        <mits:TemplateField>
            <itemtemplate>
                <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
            </itemtemplate>
        </mits:TemplateField>
    </fields>
</mits:magicform>
<asp:ObjectDataSource ID="EntityListDataSource" runat="server" />
