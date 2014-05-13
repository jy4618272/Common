﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.MeasureUnitsControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <contenttemplate>
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="UnitsOfMeasureId" DataSourceID="EntityListDataSource"
            Width="700px" AllowPaging="True" AllowSorting="True" PageSize="50">
            <captioncontrols>
                <asp:LinkButton ID="AddFromBuiltInLink" runat="server" OnClick="AddFromBuiltInLink_Click" 
                    OnInit="AddFromBuiltInLink_Init"></asp:LinkButton>
                <asp:LinkButton ID="AddNewLink" runat="server" CssClass="Cgv_AddNew Button Large" OnClick="AddNewLink_Click"
                    OnInit="AddNewLink_Init"></asp:LinkButton>
            </captioncontrols>
            <columns>
                <mits:ButtonField ButtonType="Link" CommandName="Conversion" ItemStyle-Wrap="false"></mits:ButtonField>
                <mits:TextBoxField DataField="SingularName" SortExpression="SingularName" />
                <mits:TextBoxField DataField="SingularAbbrv" />
                <mits:TextBoxField DataField="PluralName" SortExpression="PluralName" />
                <mits:TextBoxField DataField="PluralAbbrv" />
                <mits:TextBoxField DataField="LocalName" SortExpression="LocalName" />
                <mits:TextBoxField DataField="GroupName" SortExpression="GroupName" />
            </columns>
        </mits:CommonGridView>
        <mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="UnitsOfMeasureId"
            Width="550px" Visible="False">
            <fields>
                <mits:TextBoxField DataField="SingularName" MaxLength="255" Columns="65" ControlStyle-Width="350px" Required="True" HeaderStyle-Width="120px" />
                <mits:TextBoxField DataField="SingularAbbrv" MaxLength="50" Columns="65" ControlStyle-Width="350px" Required="True" />
                <mits:TextBoxField DataField="PluralName" MaxLength="255" Columns="65" ControlStyle-Width="350px" Required="True" />
                <mits:TextBoxField DataField="PluralAbbrv" MaxLength="50" Columns="65" ControlStyle-Width="350px" Required="True" />
                <mits:ComboBoxField DataField="LocalName" Required="True" AllowCustomText="true" MaxLength="50" DataSourceId="MeasureLocalsDataSource" />
                <mits:ComboBoxField DataField="GroupName" Required="True" AllowCustomText="true" MaxLength="50" DataSourceId="MeasureGroupsDataSource" />
                <mits:TemplateField>
                    <itemtemplate>
                        <div id="ErrorPanel" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </itemtemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <br />
        <mits:CommonGridView ID="ConversionList" runat="server" DataKeyNames="SourceUnitsOfMeasureId,TargetUnitsOfMeasureId"
            AutoGenerateEditButton="true" AutoGenerateDeleteButton="true" Visible="false"
            Width="700px" DataSourceID="ConversionListDataSource">
            <columns>
                <mits:TemplateField>
                    <ItemTemplate>
                        <%# "1 " + Eval("SourceSingularName")%>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField>
                    <ItemTemplate>
                        <%# string.Format("{0:F6}", Eval("Factor")) + " " + Eval("TargetPluralName")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <table border="0">
                            <tr>
                                <td>
                                    <mits:TextBox ID="TextBoxFactor" runat="server" Required="true" Text='<%# Bind("Factor") %>' Columns="5" MinimumValue="0.000000001" ValidationType="Double" ValidationErrorMessage="*"></mits:TextBox>
                                </td> 
                                <td>
                                    &nbsp;<asp:DropDownList ID="MeasureUnitList" runat="server" DataValueField="UnitsOfMeasureId" DataTextField="PluralFullName" DataSourceID="MeasureUnitExListDataSource" OnDataBound="MeasureUnitList_OnDataBound"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                </mits:TemplateField>
            </columns>
        </mits:CommonGridView>
        <mits:CommonGridView ID="GlobalList" runat="server" DataKeyNames="UnitsOfMeasureId"
            DataSourceID="GlobalListDataSource" Width="700px" AllowPaging="True" AllowSorting="True"
            PageSize="50">
            <columns>
                <mits:ButtonField ButtonType="Link" CommandName="Add" ItemStyle-Wrap="false"></mits:ButtonField>
                <mits:TextBoxField DataField="SingularName" SortExpression="SingularName"  />
                <mits:TextBoxField DataField="SingularAbbrv" />
                <mits:TextBoxField DataField="PluralName" SortExpression="PluralName"  />
                <mits:TextBoxField DataField="PluralAbbrv" />
                <mits:TextBoxField DataField="LocalName" SortExpression="LocalName" />
                <mits:TextBoxField DataField="GroupName" SortExpression="GroupName"  />
            </columns>
        </mits:CommonGridView>
        <br />
        <div class="Mf_B" style="border: none;">
            <asp:LinkButton ID="LinkButtonBack" runat="server" CssClass="Mf_Cb" OnClick="LinkButtonBack_OnClick"></asp:LinkButton>
        </div>
        <asp:ObjectDataSource ID="MeasureUnitExListDataSource" runat="server" SelectMethod="GetMeasureUnitsByGroupExceptCurrent"
            TypeName="Micajah.Common.Bll.Providers.MeasureUnitsProvider">
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="unitsOfMeasureId"
                    Type="Object" />
                <asp:ControlParameter ControlID="LinkButtonBack" PropertyName="CommandArgument" Name="groupName"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ConversionListDataSource" runat="server" SelectMethod="GetConvertedMeasureUnits"
            TypeName="Micajah.Common.Bll.Providers.MeasureUnitsProvider" DeleteMethod="DeleteMeasureUnitsConversion">
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="unitsOfMeasureId"
                    Type="Object" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="sourceUnitsOfMeasureId" Type="Object" />
                <asp:Parameter Name="targetUnitsOfMeasureId" Type="Object" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" DeleteMethod="DeleteMeasureUnits"
            SelectMethod="GetMeasureUnits" TypeName="Micajah.Common.Bll.Providers.MeasureUnitsProvider">
            <DeleteParameters>
                <asp:Parameter Name="unitsOfMeasureId" Type="Object" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="GlobalListDataSource" runat="server" SelectMethod="GetBuiltInMeasureUnits"
            TypeName="Micajah.Common.Bll.Providers.MeasureUnitsProvider"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" InsertMethod="InsertMeasureUnit"
            SelectMethod="GetMeasureUnitRow" TypeName="Micajah.Common.Bll.Providers.MeasureUnitsProvider"
            UpdateMethod="UpdateMeasureUnit" OnInserting="EntityDataSource_Inserting" OnUpdating="EntityDataSource_Inserting">
            <UpdateParameters>
                <asp:Parameter Name="unitsOfMeasureId" Type="Object" />
                <asp:Parameter Name="singularName" Type="String" />
                <asp:Parameter Name="singularAbbrv" Type="String" />
                <asp:Parameter Name="pluralName" Type="String" />
                <asp:Parameter Name="pluralAbbrv" Type="String" />
                <asp:Parameter Name="groupName" Type="String" />
                <asp:Parameter Name="localName" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="unitsOfMeasureId"
                    Type="Object" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="singularName" Type="String" />
                <asp:Parameter Name="singularAbbrv" Type="String" />
                <asp:Parameter Name="pluralName" Type="String" />
                <asp:Parameter Name="pluralAbbrv" Type="String" />
                <asp:Parameter Name="groupName" Type="String" />
                <asp:Parameter Name="localName" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="MeasureGroupsDataSource" runat="server" SelectMethod="GetUnitGroups"
            TypeName="Micajah.Common.Bll.Providers.MeasureUnitsProvider"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="MeasureLocalsDataSource" runat="server" SelectMethod="GetUnitTypeNames"
            TypeName="Micajah.Common.Bll.Providers.MeasureUnitsProvider"></asp:ObjectDataSource>
    </contenttemplate>
</asp:UpdatePanel>
