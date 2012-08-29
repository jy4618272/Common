﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    ValidateRequest="false" Inherits="Micajah.Common.WebControls.AdminControls.RecurringSchedulePage" %>

<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <mits:CommonGridView ID="List" runat="server" DataKeyNames="RecurringScheduleId,OrganizationId"
        DataSourceID="ScheduleListDataSource" Width="700px" AutoGenerateColumns="False"
        CellPadding="0" EnableDeleteConfirmation="true" AllowSorting="true" AllowPaging="true"
        AutoGenerateDeleteButton="true" AutoGenerateEditButton="true" AutoGenerateSelectButton="false"
        ShowAddLink="true" OnAction="List_Action" Caption="Recurring Schedules">
        <columns>
                <mits:TextBoxField DataField="LocalEntityType" HeaderText="Entity Type" SortExpression="LocalEntityType"
                    HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False"></ItemStyle>
                </mits:TextBoxField>
                <mits:TextBoxField DataField="LocalEntityId" HeaderText="Entity Id" HeaderStyle-Wrap="false"
                    ItemStyle-Wrap="false" SortExpression="LocalEntityId">
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False"></ItemStyle>
                </mits:TextBoxField>
                <mits:TextBoxField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-Wrap="false"
                    ItemStyle-Wrap="false">
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False"></ItemStyle>
                </mits:TextBoxField>
                <mits:TextBoxField DataField="StartDate" HeaderText="Start Date" HeaderStyle-Wrap="false"
                    ItemStyle-Wrap="false" SortExpression="StartDate">
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False"></ItemStyle>
                </mits:TextBoxField>
                <mits:TextBoxField DataField="EndDate" HeaderText="End Date" HeaderStyle-Wrap="false"
                    ItemStyle-Wrap="false" SortExpression="EndDate">
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False"></ItemStyle>
                </mits:TextBoxField>
                <mits:TextBoxField DataField="RecurrenceRule" HeaderText="Recurrence Rule" SortExpression="RecurrenceRule" />
            </columns>
    </mits:CommonGridView>
    <asp:ObjectDataSource ID="ScheduleListDataSource" runat="server" DeleteMethod="DeleteRecurringSchedule"
        SelectMethod="GetRecurringSchedules" TypeName="Micajah.Common.Bll.Providers.RecurringScheduleProvider"
        OnSelecting="ScheduleListDataSource_Selecting">
        <SelectParameters>
            <asp:Parameter Name="organizationId" Type="Object" />
            <asp:Parameter Name="instanceId" Type="Object" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="recurringScheduleId" Type="Object" />
            <asp:Parameter Name="organizationId" Type="Object" />
        </DeleteParameters>
    </asp:ObjectDataSource>
    <mits:RecurrenceSchedule ID="RecurrenceScheduleControl" runat="server" Visible="false"
        VisibleCancelButton="true" OnCancel="RecurrenceScheduleControl_Cancel" OnUpdated="RecurrenceScheduleControl_Updated" />
</asp:Content>
