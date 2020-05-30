<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Notes_old.aspx.vb" Inherits="Notes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  <style>
    .row_c{
      display:flex;
      justify-content: center;
    }
    .row_l{
      display:flex;
      flex-direction:row;
    }
  </style>
</head>
<body style="background-color: #d7d7d7;font-family:Tahoma;">
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div style="width:700px;margin:20px auto auto auto;">
      <div class="row_c">
        <div style="font-size:14px;font-weight:bold;">Notes for <strong runat="server" id="spIndex"></strong></div>
      </div>
      <div class="row_l" style="margin-top:2px;">
        <asp:Button runat="server" ID="btnNewNotes" Font-Size="12px" OnClick="btnNewNotes_Click" Text="New Notes"/>
      </div>
      <div style="width: 100%; border: 1px solid black; margin: 2px; min-height: 500px;">
        <table style="margin-top:10px;width:100%;font-size:14px;">
          <tr>
            <td style="font-weight:bold">Title:</td>
            <td>
              <asp:TextBox runat="server" ID="txtTitle" Width="500"></asp:TextBox></td>
          </tr>
          <tr>
            <td style="text-align: start; font-weight: bold">Desription</td>
            <td>
              <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Height="300" Width="500" />
            </td>
          </tr>
          <tr>
            <td style="font-weight: bold">Send mail to:</td>
            <td>
              <asp:TextBox runat="server" ID="txtMailTo" Width="500" />
               
            </td>
          </tr>

          <tr>
            <td style="font-weight: bold">Reminder:</td>
            <td>E-mail Id:<asp:TextBox ID="txtMailIdReminder" runat="server" Width="430" />
            </td>
          </tr>
          <tr>
            <td></td>
            <td>Date :<asp:TextBox runat="server" ID="txtDate" />
                <asp:TextBox runat="server" ID="txtTime" Text="9:00" ReadOnly="true" Enabled="false" Visible="false" />
            </td>
          </tr>
          <tr>
            <td></td>
            <td>
              <asp:Button runat="server" ID="btnSaveNotes" OnClick="btnSaveNotes_Click" Text="Submit" Width="100" />
              <asp:Button runat="server" ID="btnDeleteNotes" OnClick="btnDeleteNotes_Click" Text="Delete" Visible="false" />
              <asp:Button runat="server" ID="btnAttachment" OnClick="btnAttachment_Click" Text="Attachment" /></td>
          </tr>
          </table>
      </div>
      <div style="font-size:12px;font-weight:bold;margin-top:10px;">Notes List</div>
      <div style="background-color: #cccaca; width: 100%; min-height: 100px; font-weight: bold; overflow: hidden; font-size: 12px;">
        <asp:Repeater runat="server" ID="rptNotes" OnItemDataBound="rptNotes_ItemDataBound" DataSourceID="ODS1">
          <ItemTemplate>
            <table style="width:100%;">
              <tr id="row" runat="server">
                <td>
                  <asp:HiddenField Value='<%#Eval("ColorId") %>' runat="server" ID="hdfUserID" />
                  <asp:HiddenField Value='<%#Eval("Notes_RunningNo") %>' runat="server" ID="Notes_RunningNo" />
                </td>
                <td style="width:400px">
                  <asp:LinkButton Text='<%#If(Eval("Description").ToString().Length > 80, Eval("Description").ToString().Substring(0, 80), Eval("Title").ToString()) %>' runat="server" ID="lnkUpdate" CommandArgument='<%#Eval("UserId") & "&" & Eval("NotesId") %>' OnClick="lnkUpdate_Click"></asp:LinkButton></td>
                <td><%# Convert.ToDateTime(Eval("Created_Date")).ToString("dd-MM-yyyy HH:mm ")%></td>
                <td style="text-align:right;"><%#Eval("EmployeeName") %></td>
              </tr>
            </table>
          </ItemTemplate>
        </asp:Repeater>
        <asp:ObjectDataSource
          ID="ODS1"
          DataObjectTypeName="SIS.NT.ntNotes"
          OldValuesParameterFormatString="original_{0}"
          SelectMethod="GetNotesByHandleIndex"
          TypeName="SIS.NT.ntNotes"
          EnablePaging="False"
          runat="server">
          <SelectParameters>
            <asp:QueryStringParameter Name="hndl" QueryStringField="handle" Direction="Input" DbType="String" />
            <asp:QueryStringParameter Name="indx" QueryStringField="Index" Direction="Input" DbType="String" />
          </SelectParameters>
        </asp:ObjectDataSource>
      </div>
    </div>
    <asp:HiddenField runat="server" ID="hdfNoteId" />
    <asp:HiddenField runat="server" ID="hdfNewNoteId" />
    <asp:HiddenField runat="server" ID="hdfUser" />
  </form>
  <br/>
  <br/>
  <br/>
  <br/>

</body>
</html>
