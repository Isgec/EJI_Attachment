<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Attachment.aspx.vb" Inherits="Attachment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  <style>
    .row_c{
      display:flex;
      justify-content: center;
    }
  </style>
</head>
<body style="background-color:#d7d7d7">
    <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row_c">
      <div style="font-weight:bold; text-align: center; font-size: 20px;">Attachments</div>
    </div>
    <div class="row_c" style="margin-top: 20px" runat="server" id="divUploadAttachment">
        <asp:FileUpload CssClass="button" ID="FileUpload" runat="server" AllowMultiple="true" />
        <asp:Button ID="btnUpload" CssClass="button" Text="Upload Attachment" runat="server" OnClick="btnUpload_Click" BackColor="#53a9ff" ForeColor="White" />
    </div>
    <div class="row_c" style="margin-top:20px; font-size:12px; color: #646363" runat="server" id="divViewAttachment">
      <div style="flex-grow:1;">

        <asp:GridView 
          runat="server" 
          ID="gvAttachment" 
          AutoGenerateColumns="false" 
          Width="90%"
          HeaderStyle-BackColor="White" 
          HeaderStyle-Height="40"  
          AllowPaging="false" 
          DataSourceID="ODS1"
          GridLines="Both"
          HorizontalAlign="Center"
          DataKeyNames="t_drid">
          <Columns>
            <asp:TemplateField HeaderText="File Name">
              <ItemTemplate>
                <%#Eval("t_fnam") %>
              </ItemTemplate>
              <HeaderStyle Width="30%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Upload Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120">
              <ItemTemplate>
                <%#Convert.ToDateTime(Eval("t_aton")).ToString("dd-MM-yyyy") %>
              </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Download" ItemStyle-HorizontalAlign="Center">
              <ItemTemplate>
                <asp:ImageButton runat="server" ID="lnkDownload" AlternateText="dl" ImageUrl="~/download.png" CommandName="lgDownload" CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
              </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
              <ItemTemplate>
                <asp:ImageButton runat="server" ID="lnkDelete" AlternateText="Delete" ImageUrl="~/delete.png" CommandName="lgDelete" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Delete File ?');"></asp:ImageButton>
              </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Forced Delete" ItemStyle-HorizontalAlign="Center">
              <ItemTemplate>
                <asp:ImageButton runat="server" ID="lnkFDelete" AlternateText="Delete" ImageUrl="~/delete.png" CommandName="lgFDelete" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Delete File ?');"></asp:ImageButton>
              </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="e-mail" ItemStyle-HorizontalAlign="Center" Visible="false">
              <ItemTemplate>
                <asp:LinkButton runat="server" ID="lnkSend" Text="Send" CommandName="lgSend" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
              </ItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
      <asp:ObjectDataSource
        ID="ODS1"
      DataObjectTypeName = "ejiVault.EJI.ediAFile"
      OldValuesParameterFormatString = "original_{0}"
      SelectMethod = "GetFilesByHandleIndex"
      TypeName = "ejiVault.EJI.ediAFile"
      EnablePaging="False"
      runat="server">
      <SelectParameters >
        <asp:QueryStringParameter Name="t_hndl" QueryStringField="AthHandle" Direction="Input" DbType="String" />
        <asp:QueryStringParameter Name="t_indx" QueryStringField="Index" Direction="Input" DbType="String" />
      </SelectParameters>
      </asp:ObjectDataSource>
      </div>
    </div>

    </form>
</body>
</html>
