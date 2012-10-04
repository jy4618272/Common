<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.ProfileControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="UserId"
    Width="550px" OnDataBound="EditForm_DataBound">
    <fields>
        <mits:TextBoxField DataField="Email" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="True"
            ValidationType="RegularExpression" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
        <mits:TextBoxField DataField="FirstName" MaxLength="255" Columns="65" ControlStyle-Width="350px" Required="True" HeaderStyle-Wrap="false" />
        <mits:TextBoxField DataField="MiddleName" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Wrap="false" />
        <mits:TextBoxField DataField="LastName" MaxLength="255" Columns="65" ControlStyle-Width="350px" Required="True" HeaderStyle-Wrap="false" />
        <mits:TextBoxField DataField="Phone" MaxLength="20" Columns="20" />
        <mits:TextBoxField DataField="MobilePhone" MaxLength="20" Columns="20" />
        <mits:TextBoxField DataField="Fax" MaxLength="20" Columns="20" />
        <mits:TextBoxField DataField="Title" MaxLength="20" Columns="20" />
        <mits:TextBoxField DataField="Department" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
        <mits:TextBoxField DataField="Street" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
        <mits:TextBoxField DataField="Street2" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
        <mits:TextBoxField DataField="City" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
        <mits:TextBoxField DataField="State" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
        <mits:TextBoxField DataField="PostalCode" MaxLength="20" Columns="20" />
        <mits:ComboBoxField DataField="Country" DataSourceId="CountriesDataSource" 
            DataTextField="Name" DataValueField="Name" ControlStyle-Width="250px" AllowCustomText="true" MarkFirstMatch="true"
            OnControlInit="CountryList_ControlInit" />
        <mits:TemplateField PaddingLeft="false">
            <ItemTemplate>
                <asp:DropDownList ID="TimeZoneList" runat="server" Width="372px" />
            </ItemTemplate>
        </mits:TemplateField>
        <mits:TemplateField PaddingLeft="false">
            <ItemTemplate>
                <asp:DropDownList ID="TimeFormatList" runat="server" Width="100px"  />
            </ItemTemplate>
        </mits:TemplateField>
        <asp:TemplateField>
            <itemtemplate>
                <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
            </itemtemplate>
        </asp:TemplateField>
    </fields>
</mits:MagicForm>
<asp:Label ID="UserIdLabel" runat="server" Visible="false" />
<asp:ObjectDataSource ID="EntityDataSource" runat="server" SelectMethod="GetUserRow"
    TypeName="Micajah.Common.Bll.Providers.UserProvider" UpdateMethod="UpdateCurrentUser" OnUpdating="EntityDataSource_Updating">
    <SelectParameters>
        <asp:ControlParameter Name="userId" Type="Object" ControlID="UserIdLabel" PropertyName="Text" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="userId" Type="Object" />
        <asp:Parameter Name="email" Type="String" />
        <asp:Parameter Name="firstName" Type="String" />
        <asp:Parameter Name="lastName" Type="String" />
        <asp:Parameter Name="middleName" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="phone" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="mobilePhone" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="fax" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="title" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="department" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="street" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="street2" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="city" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="state" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="postalCode" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="country" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="timeZoneId" Type="String" />
        <asp:Parameter Name="timeFormat" Type="Int32" />
    </UpdateParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="CountriesDataSource" runat="server" SelectMethod="GetCountriesView"
    TypeName="Micajah.Common.Bll.Providers.CountryProvider"></asp:ObjectDataSource>
