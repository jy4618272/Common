<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.InstanceProfileControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="SearchPanel" runat="server" Style="padding-bottom: 7px; position: relative;
            left: -5px;" Width="350px">
            <mits:ComboBox ID="InstanceList" runat="server" DataTextField="Name" DataValueField="InstanceId"
                DataSourceId="InstancesDataSource" Width="100%" AutoPostBack="true" OnDataBound="InstanceList_DataBound" />
        </asp:Panel>
        <mits:magicform id="EditForm" runat="server" datasourceid="EntityDataSource" datakeynames="InstanceId"
            width="550px" OnDataBound="EditForm_DataBound">
            <fields>
                <mits:TextBoxField DataField="Name" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="True" />
                <mits:TextBoxField DataField="Description" MaxLength="1024" Columns="65" ControlStyle-Width="350px" Rows="3"
                    TextMode="MultiLine" />
                <mits:TextBoxField DataField="EmailSuffixes" MaxLength="1024" Columns="65" ControlStyle-Width="350px" Rows="3"
                    TextMode="MultiLine" />                
                <mits:TemplateField PaddingLeft="false">
                    <ItemTemplate>
                        <mits:CheckBoxList ID="WorkingDaysList" runat="server" RepeatColumns="3" />
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField PaddingLeft="false">
                    <ItemTemplate>
                        <asp:DropDownList ID="TimeZoneList" runat="server" Width="372px" />
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField PaddingLeft="false">
                    <ItemTemplate>
                        <asp:DropDownList ID="TimeFormatList" runat="server" Width="140px"  />
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField PaddingLeft="false">
                    <ItemTemplate>
                        <asp:DropDownList ID="DateFormatList" runat="server" Width="140px"  />
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:CheckBoxField DataField="EnableSignUpUser" />
                <mits:CheckBoxField DataField="Beta" />
                <mits:TemplateField>
                    <itemtemplate>
                        <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </itemtemplate>
                </mits:TemplateField>
            </fields>
        </mits:magicform>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" SelectMethod="GetInstance"
            TypeName="Micajah.Common.Bll.Providers.InstanceProvider" UpdateMethod="UpdateInstance"
            OnUpdating="EntityDataSource_Updating">
            <UpdateParameters>
                <asp:Parameter Name="instanceId" Type="Object" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="enableSignUpUser" Type="Boolean" />
                <asp:Parameter Name="timeZoneId" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="timeFormat" Type="Int32" />
                <asp:Parameter Name="dateFormat" Type="Int32" />
                <asp:Parameter Name="workingDays" Type="String" />
                <asp:Parameter Name="beta" Type="Boolean" />
                <asp:Parameter Name="emailSuffixes" Type="String" ConvertEmptyStringToNull="False" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter Name="instanceId" Type="Object" ControlID="InstanceList" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="InstancesDataSource" runat="server" SelectMethod="GetInstances"
            TypeName="Micajah.Common.Bll.Providers.InstanceProvider" OnSelecting="InstancesDataSource_Selecting">
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
