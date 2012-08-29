<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.RecurrenceScheduleInternalControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:PlaceHolder ID="PageContent" runat="server">
    <table border="0" cellpadding="1" cellspacing="1" style="border-width: 0px; white-space: nowrap;
        width: 100%;">
        <tr valign="top">
            <td align="right">
                Name
            </td>
            <td colspan="5">
                <mits:TextBox ID="TextBoxName" runat="server" Required="true" ValidationGroup="_RecurringSchedule"
                    TextMode="MultiLine" Rows="2" Width="100%" />
            </td>
        </tr>
        <tr valign="top">
            <td align="right">
                Local Entity Type
            </td>
            <td>
                <mits:ComboBox ID="ComboBoxLocalEntityType" runat="server" AllowCustomText="true"
                    Required="true" ValidationGroup="_RecurringSchedule">
                </mits:ComboBox>
            </td>
            <td align="right">
                &nbsp;&nbsp;&nbsp;&nbsp;Local Entity Identifier
            </td>
            <td colspan="2">
                <mits:TextBox ID="TextBoxLocalEntityId" runat="server" Required="true" ValidationGroup="_RecurringSchedule"
                    Width="100%" />
            </td>
        </tr>
        <tr valign="middle">
            <td align="right">
                Start Date
            </td>
            <td>
                <mits:DatePicker ID="DatePickerStartDate" runat="server" Required="true" EnableTyping="true"
                    Type="DateTimePicker" ValidationGroup="_RecurringSchedule" />
            </td>
            <td align="right">
                &nbsp;&nbsp;&nbsp;&nbsp;End Date
            </td>
            <td>
                <mits:DatePicker ID="DatePickerEndDate" runat="server" Required="true" EnableTyping="true"
                    Type="DateTimePicker" ValidationGroup="_RecurringSchedule" />
            </td>
            <td>
                <asp:CheckBox ID="CheckBoxAllDay" runat="server" Checked="false" Text="All day" onclick="recurr_sched_checkallday(this);" />
            </td>
        </tr>
    </table>
    <br />
    <asp:CheckBox ID="CheckBoxRecurrence" runat="server" Text="Recurrence" onclick="recurr_sched_showRecurrence(this);" />
    <div style="width: 100%; height: 100%; vertical-align: middle;">
        <div id="PanelRecurrence" runat="server" style="display: none;">
            <div style="border: 1px solid rgb(171, 171, 171); overflow: hidden; padding: 8px 0px 7px 10px;
                margin: 11px 0px 5px; vertical-align: middle;">
                <table border="0">
                    <tr>
                        <td>
                            <div style="float: left; height: 100px; width: 118px;">
                                <asp:RadioButtonList ID="RadioButtonListRecurringOption" runat="server" RepeatLayout="Flow"
                                    RepeatDirection="Vertical">
                                    <asp:ListItem Text="Hourly" Value="Hourly" onclick="recurr_sched_changePeriod(this);"></asp:ListItem>
                                    <asp:ListItem Text="Daily" Value="Daily" onclick="recurr_sched_changePeriod(this);"></asp:ListItem>
                                    <asp:ListItem Text="Weekly" Value="Weekly" onclick="recurr_sched_changePeriod(this);"></asp:ListItem>
                                    <asp:ListItem Text="Monthly" Value="Monthly" onclick="recurr_sched_changePeriod(this);"></asp:ListItem>
                                    <asp:ListItem Text="Yearly" Value="Yearly" onclick="recurr_sched_changePeriod(this);"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </td>
                        <td>
                            <div style="float: right; top: 0; height: 100px; width: 500px; border-left: 1px solid rgb(171, 171, 171);">
                                <div id="PanelOption" runat="server">
                                </div>
                                <div id="PanelOption_Hourly" runat="server" style="display: none; width: 100%; padding: 5px 0 0 11px;
                                    white-space: nowrap;">
                                    Every&nbsp;<telerik:radnumerictextbox id="NumericTextBoxHourly" runat="server" datatype="System.Int32"
                                        maxvalue="300" minvalue="1" width="50px" showspinbuttons="True" value="1">
                                    <NumberFormat AllowRounding="false" />
                                </telerik:radnumerictextbox>&nbsp;hour(s)
                                </div>
                                <div id="PanelOption_Daily" runat="server" style="display: none; width: 100%; padding: 5px 0 0 11px;
                                    white-space: nowrap;">
                                    <asp:RadioButton ID="RadioButtonListRecurringOptionDaily_Everyday" runat="server"
                                        Text="Every" GroupName="RadioButtonListRecurrOptionDaily" onclick="recurr_sched_dailyfocus();"
                                        Style="margin: 4px 2px 4px 2px;" Checked="true"></asp:RadioButton>&nbsp;<telerik:radnumerictextbox
                                            id="NumericTextBoxDaily" runat="server" datatype="System.Int32" maxvalue="300"
                                            minvalue="1" width="50px" showspinbuttons="True" value="1">
                                        <NumberFormat AllowRounding="false" />
                                    </telerik:radnumerictextbox>&nbsp;day(s)<br />
                                    <asp:RadioButton ID="RadioButtonListRecurringOptionDaily_EveryWeekday" runat="server"
                                        Text="Every weekday" GroupName="RadioButtonListRecurrOptionDaily" Style="margin: 4px 2px 4px 2px;">
                                    </asp:RadioButton>
                                </div>
                                <div id="PanelOption_Weekly" runat="server" style="display: none; width: 100%; padding: 5px 0 0 11px;
                                    white-space: nowrap;">
                                    Recur&nbsp;every&nbsp;<telerik:radnumerictextbox id="NumericTextBoxWeekly" runat="server"
                                        datatype="System.Int32" maxvalue="300" minvalue="1" width="50px" showspinbuttons="True"
                                        value="1">
                                    <NumberFormat AllowRounding="false" />
                                </telerik:radnumerictextbox>&nbsp;week(s) on<br />
                                    <ul style="list-style: none outside;">
                                        <li style="float: left; width: 95px;">
                                            <asp:CheckBox ID="CheckBoxWeeklySunday" runat="server" Text="Sunday" />
                                        </li>
                                        <li style="float: left; width: 95px;">
                                            <asp:CheckBox ID="CheckBoxWeeklyMonday" runat="server" Text="Monday" Checked="true" />
                                        </li>
                                        <li style="float: left; width: 95px;">
                                            <asp:CheckBox ID="CheckBoxWeeklyTuesday" runat="server" Text="Tuesday" />
                                        </li>
                                        <li style="float: left; width: 95px;">
                                            <asp:CheckBox ID="CheckBoxWeeklyWednesday" runat="server" Text="Wednesday" />
                                        </li>
                                        <li style="float: left; width: 95px; clear: left;">
                                            <asp:CheckBox ID="CheckBoxWeeklyThursday" runat="server" Text="Thursday" />
                                        </li>
                                        <li style="float: left; width: 95px;">
                                            <asp:CheckBox ID="CheckBoxWeeklyFriday" runat="server" Text="Friday" />
                                        </li>
                                        <li style="float: left; width: 95px;">
                                            <asp:CheckBox ID="CheckBoxWeeklySaturday" runat="server" Text="Saturday" />
                                        </li>
                                    </ul>
                                </div>
                                <div id="PanelOption_Monthly" runat="server" style="display: none; width: 100%; padding: 5px 0 0 11px;
                                    white-space: nowrap; vertical-align: middle;">
                                    <asp:RadioButton ID="RadioButtonListRecurringOptionMonthly_Day" runat="server" Text="Day"
                                        Checked="true" GroupName="RadioButtonListRecurrOptionMonthly" onclick="recurr_sched_monthfocus();"
                                        Style="margin: 4px 2px 4px 2px;"></asp:RadioButton>&nbsp;<telerik:radnumerictextbox
                                            id="NumericTextBoxDayInMonth" runat="server" datatype="System.Int32" maxvalue="31"
                                            minvalue="1" width="50px" showspinbuttons="True" value="5">
                                        <NumberFormat AllowRounding="false" />
                                    </telerik:radnumerictextbox>&nbsp;of&nbsp;every&nbsp;<telerik:radnumerictextbox id="NumericTextBoxMonth"
                                        runat="server" datatype="System.Int32" maxvalue="300" minvalue="1" width="50px"
                                        showspinbuttons="True" value="1">
                                        <NumberFormat AllowRounding="false" />
                                    </telerik:radnumerictextbox>&nbsp;month(s)<br />
                                    <asp:RadioButton ID="RadioButtonListRecurringOptionMonthly_The" runat="server" Text="The"
                                        GroupName="RadioButtonListRecurrOptionMonthly" Style="margin: 4px 2px 4px 2px;" />&nbsp;<telerik:radcombobox
                                            id="ComboBoxMonthly_NumberDayInWeek" runat="server" width="100px">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="first" Value="1" />
                                            <telerik:RadComboBoxItem runat="server" Text="second" Value="2" />
                                            <telerik:RadComboBoxItem runat="server" Text="third" Value="3" />
                                            <telerik:RadComboBoxItem runat="server" Text="fourth" Value="4" />
                                            <telerik:RadComboBoxItem runat="server" Text="last" Value="-1" />
                                        </Items>
                                    </telerik:radcombobox>
                                    <telerik:radcombobox id="ComboBoxMonthly_DayInWeek" runat="server" width="100px">
                                    <Items>
                                        <telerik:RadComboBoxItem runat="server" Text='day' Value="127" />
                                        <telerik:RadComboBoxItem runat="server" Text="weekday" Value="62" />
                                        <telerik:RadComboBoxItem runat="server" Text="weekend day" Value="65" />
                                        <telerik:RadComboBoxItem runat="server" Text="Sunday" Value="1" />
                                        <telerik:RadComboBoxItem runat="server" Text="Monday" Value="2" />
                                        <telerik:RadComboBoxItem runat="server" Text="Tuesday" Value="4" />
                                        <telerik:RadComboBoxItem runat="server" Text="Wednesday" Value="8" />
                                        <telerik:RadComboBoxItem runat="server" Text="Thursday" Value="16" />
                                        <telerik:RadComboBoxItem runat="server" Text="Friday" Value="32" />
                                        <telerik:RadComboBoxItem runat="server" Text="Saturday" Value="64" />
                                    </Items>
                                </telerik:radcombobox>
                                    &nbsp;of&nbsp;every&nbsp;<telerik:radnumerictextbox id="NumericTextBoxMonth_Destine"
                                        runat="server" datatype="System.Int32" maxvalue="300" minvalue="1" width="50px"
                                        showspinbuttons="True" value="1">
                                    <NumberFormat AllowRounding="false" />
                                </telerik:radnumerictextbox>&nbsp;month(s)
                                </div>
                                <div id="PanelOption_Yearly" runat="server" style="display: none; width: 100%; padding: 5px 0 0 11px;
                                    white-space: nowrap;">
                                    <asp:RadioButton ID="RadioButtonListRecurringOptionYearly_Every" runat="server" Text="Every"
                                        GroupName="RadioButtonListRecurrOptionYearly" Style="margin: 4px 2px 4px 2px;"
                                        Checked="true"></asp:RadioButton>&nbsp;<telerik:radcombobox id="ComboBoxYearly_EveryMonth"
                                            runat="server" width="100px">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Text="January" Value="1" />
                                            <telerik:RadComboBoxItem runat="server" Text="February" Value="2" />
                                            <telerik:RadComboBoxItem runat="server" Text="March" Value="3" />
                                            <telerik:RadComboBoxItem runat="server" Text="April" Value="4" />
                                            <telerik:RadComboBoxItem runat="server" Text="May" Value="5" />
                                            <telerik:RadComboBoxItem runat="server" Text="June" Value="6" />
                                            <telerik:RadComboBoxItem runat="server" Text="July" Value="7" />
                                            <telerik:RadComboBoxItem runat="server" Text="August" Value="8" />
                                            <telerik:RadComboBoxItem runat="server" Text="September" Value="9" />
                                            <telerik:RadComboBoxItem runat="server" Text="October" Value="10" />
                                            <telerik:RadComboBoxItem runat="server" Text="November" Value="11" />
                                            <telerik:RadComboBoxItem runat="server" Text="December" Value="12" />
                                        </Items>
                                    </telerik:radcombobox>
                                    <telerik:radnumerictextbox id="NumericTextBoxDayOfMonth" runat="server" datatype="System.Int32"
                                        maxvalue="31" minvalue="1" width="50px" showspinbuttons="True" value="5">
                                    <NumberFormat AllowRounding="false" />
                                </telerik:radnumerictextbox>
                                    <br />
                                    <asp:RadioButton ID="RadioButtonListRecurringOptionYearly_The" runat="server" Text="The"
                                        GroupName="RadioButtonListRecurrOptionYearly" Style="margin: 4px 2px 4px 2px;">
                                    </asp:RadioButton>&nbsp;<telerik:radcombobox id="ComboBoxYearly_NumberDayInMonth"
                                        runat="server" width="100px">
                                    <Items>
                                        <telerik:RadComboBoxItem runat="server" Text="first" Value="1" />
                                        <telerik:RadComboBoxItem runat="server" Text="second" Value="2" />
                                        <telerik:RadComboBoxItem runat="server" Text="third" Value="3" />
                                        <telerik:RadComboBoxItem runat="server" Text="fourth" Value="4" />
                                        <telerik:RadComboBoxItem runat="server" Text="last" Value="-1" />
                                    </Items>
                                </telerik:radcombobox>
                                    <telerik:radcombobox id="ComboBoxYearly_DayInMonth" runat="server" width="100px">
                                    <Items>
                                        <telerik:RadComboBoxItem runat="server" Text='day' Value="127" />
                                        <telerik:RadComboBoxItem runat="server" Text="weekday" Value="62" />
                                        <telerik:RadComboBoxItem runat="server" Text="weekend day" Value="65" />
                                        <telerik:RadComboBoxItem runat="server" Text="Sunday" Value="1" />
                                        <telerik:RadComboBoxItem runat="server" Text="Monday" Value="2" />
                                        <telerik:RadComboBoxItem runat="server" Text="Tuesday" Value="4" />
                                        <telerik:RadComboBoxItem runat="server" Text="Wednesday" Value="8" />
                                        <telerik:RadComboBoxItem runat="server" Text="Thursday" Value="16" />
                                        <telerik:RadComboBoxItem runat="server" Text="Friday" Value="32" />
                                        <telerik:RadComboBoxItem runat="server" Text="Saturday" Value="64" />
                                    </Items>
                                </telerik:radcombobox>
                                    &nbsp;of&nbsp;<telerik:radcombobox id="ComboBoxYearly_TheMonth" runat="server" width="100px">
                                    <Items>
                                        <telerik:RadComboBoxItem runat="server" Text="January" Value="1" />
                                        <telerik:RadComboBoxItem runat="server" Text="February" Value="2" />
                                        <telerik:RadComboBoxItem runat="server" Text="March" Value="3" />
                                        <telerik:RadComboBoxItem runat="server" Text="April" Value="4" />
                                        <telerik:RadComboBoxItem runat="server" Text="May" Value="5" />
                                        <telerik:RadComboBoxItem runat="server" Text="June" Value="6" />
                                        <telerik:RadComboBoxItem runat="server" Text="July" Value="7" />
                                        <telerik:RadComboBoxItem runat="server" Text="August" Value="8" />
                                        <telerik:RadComboBoxItem runat="server" Text="September" Value="9" />
                                        <telerik:RadComboBoxItem runat="server" Text="October" Value="10" />
                                        <telerik:RadComboBoxItem runat="server" Text="November" Value="11" />
                                        <telerik:RadComboBoxItem runat="server" Text="December" Value="12" />
                                    </Items>
                                </telerik:radcombobox>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="border: 1px solid rgb(171, 171, 171); overflow: hidden; padding: 8px 0px 7px 10px;
                margin: 11px 0px 5px; vertical-align: middle;">
                <table border="0" cellspacing="1" cellpadding="1">
                    <tr>
                        <td>
                            <asp:RadioButton ID="RadioButtonEndDate_NoEnd" runat="server" Text="No end date"
                                Checked="true" GroupName="RadioButtonEndDate" Style="margin: 4px 2px 4px 2px;">
                            </asp:RadioButton>&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="RadioButtonEndDate_EndAfter"
                                runat="server" Text="End after" GroupName="RadioButtonEndDate" onclick="recurr_sched_endafterfocus();"
                                Style="margin: 4px 2px 4px 2px;"></asp:RadioButton>&nbsp;<telerik:radnumerictextbox
                                    id="NumericTextBoxEndDate_Number" runat="server" datatype="System.Int32" maxvalue="300"
                                    minvalue="1" width="50px" showspinbuttons="True" value="1">
                                <NumberFormat AllowRounding="false" />
                            </telerik:radnumerictextbox>&nbsp;occurrences&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButtonEndDate_EndBy" runat="server" Text="End by" GroupName="RadioButtonEndDate"
                                Style="margin: 4px 2px 4px 2px;" onclick="recurr_sched_endbyexpand();"></asp:RadioButton>
                        </td>
                        <td>
                            <mits:DatePicker ID="DatePickerRecurringOptionEndDate" runat="server" Type="DatePicker"
                                DateFormat="M/dd/yyyy" Width="100px" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="Mf_B" style="border: none;">
            <asp:Button ID="ButtonSave" runat="server" ValidationGroup="_RecurringSchedule" OnClick="ButtonSave_Click" />
            <asp:PlaceHolder ID="ButtonsSeparator" runat="server" />
            <asp:LinkButton ID="ButtonCancel" runat="server" CssClass="Mf_Cb" OnClick="ButtonCancel_Click" />
        </div>
        <asp:PlaceHolder ID="ClientScripts1" runat="server">
            <script type="text/javascript">
                //<![CDATA[
                function recurr_sched_endbyexpand() {
                    var datepicker = $find("<% = DatePickerRecurringOptionEndDate.ClientID %>_rdp");
                    if (datepicker) {
                        datepicker.showPopup();
                        datepicker.get_dateInput().focus();
                    }
                }

                function recurr_sched_endafterfocus() {
                    $find("<% = NumericTextBoxEndDate_Number.ClientID %>").focus();
                }

                function recurr_sched_monthfocus() {
                    $find("<% = NumericTextBoxDayInMonth.ClientID %>").focus();
                }

                function recurr_sched_dailyfocus() {
                    $find("<% = NumericTextBoxDaily.ClientID %>").focus();
                }

                function recurr_sched_clearAllPanels() {
                    $get("<% = PanelOption_Hourly.ClientID %>").style.display = "none";
                    $get("<% = PanelOption_Daily.ClientID %>").style.display = "none";
                    $get("<% = PanelOption_Weekly.ClientID %>").style.display = "none";
                    $get("<% = PanelOption_Monthly.ClientID %>").style.display = "none";
                    $get("<% = PanelOption_Yearly.ClientID %>").style.display = "none";
                }

                function recurr_sched_changePeriod(sender) {
                    if (sender) {
                        recurr_sched_clearAllPanels();
                        var panel = $get("<% = PanelOption.ClientID %>_" + sender.value);
                        if (panel) {
                            panel.style.display = "block";
                        }
                    }
                }

                function recurr_sched_showRecurrence(sender) {
                    var panelRecurrence = $get("<% = PanelRecurrence.ClientID %>");
                    if (sender && panelRecurrence) {
                        if (sender.checked == true)
                            panelRecurrence.style.display = "block";
                        else
                            panelRecurrence.style.display = "none";
                    }
                }

                function recurr_sched_checkallday(sender) {
                    var startTime = $find("<% = DatePickerStartDate.ClientID %>_rdp");
                    var endTime = $find("<% = DatePickerEndDate.ClientID %>_rdp");
                    if (sender && startTime && endTime) {
                        if (sender.checked == true) {
                            startTime.get_timePopupButton().style.display = "none";
                            startTime.get_dateInput().set_dateFormat("M/dd/yyyy");
                            startTime.get_dateInput().set_displayDateFormat("M/dd/yyyy");
                            startTime.get_dateInput().focus()
                            endTime.get_timePopupButton().style.display = "none";
                            endTime.get_dateInput().set_dateFormat("M/dd/yyyy");
                            endTime.get_dateInput().set_displayDateFormat("M/dd/yyyy");
                            endTime.get_dateInput().focus()
                        }
                        else {
                            startTime.get_timePopupButton().style.display = "";
                            startTime.get_dateInput().set_dateFormat("M/dd/yyyy h:mm tt");
                            startTime.get_dateInput().set_displayDateFormat("M/dd/yyyy h:mm tt");
                            startTime.get_dateInput().focus()
                            endTime.get_timePopupButton().style.display = "";
                            endTime.get_dateInput().set_dateFormat("M/dd/yyyy h:mm tt");
                            endTime.get_dateInput().set_displayDateFormat("M/dd/yyyy h:mm tt");
                            endTime.get_dateInput().focus()
                        }
                        sender.focus();
                    }
                }
                //]]>
            </script>
        </asp:PlaceHolder>
    </div>
</asp:PlaceHolder>
