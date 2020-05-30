
Partial Class Notes
  Inherits System.Web.UI.Page

  Public handle As String = ""
  Public Index As String = ""
  Public LoginUser As String = ""
  Public Hd As String = ""
  Public em As String = ""
  Public tl As String = ""

  Private Sub Notes_Load(sender As Object, e As EventArgs) Handles Me.Load
    SIS.SYS.Utilities.ApplicationSpacific.Initialize()
    If Not String.IsNullOrEmpty(Request.QueryString("handle")) Then
      handle = Request.QueryString("handle")
    End If
    If Not String.IsNullOrEmpty(Request.QueryString("Index")) Then
      Index = Request.QueryString("Index")
    End If
    If Not String.IsNullOrEmpty(Request.QueryString("User")) Then
      LoginUser = Request.QueryString("User")
    End If
    If Not String.IsNullOrEmpty(Request.QueryString("Hd")) Then
      Hd = Request.QueryString("Hd")
    End If
    If Not String.IsNullOrEmpty(Request.QueryString("Em")) Then
      em = Request.QueryString("Em")
    End If
    If Not String.IsNullOrEmpty(Request.QueryString("Tl")) Then
      tl = Request.QueryString("Tl")
    End If
  End Sub


  Protected Sub btnSaveNotes_Click(sender As Object, e As EventArgs)
    Try
      If txtTitle.Text <> "" AndAlso txtDescription.Text <> "" Then
        Dim objNotes As SIS.NT.ntNotes = New SIS.NT.ntNotes
        objNotes.NotesHandle = handle
        objNotes.IndexValue = Index
        objNotes.Title = txtTitle.Text.Trim()
        objNotes.Description = txtDescription.Text.Trim()
        objNotes.UserId = LoginUser
        objNotes.SendEmailTo = txtMailTo.Text
        objNotes.Created_Date = Now
        objNotes.ReminderTo = txtMailIdReminder.Text.Trim()
        objNotes.ReminderDateTime = If(txtDate.Text <> "", Convert.ToDateTime(txtDate.Text.Trim()).ToString("yyyy-MM-dd") & " " & "09:00", System.DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") & " " & "09:00")
        If btnSaveNotes.Text = "Submit" Then
          If hdfNewNoteId.Value = "" Then
            objNotes = SIS.NT.ntNotes.InsertNotes(objNotes)
            If objNotes.NotesId <> "0" Then
              If objNotes.ReminderTo <> "" Then
                Dim xRem As New SIS.NT.ntReminder
                With xRem
                  .NotesId = objNotes.NotesId
                  .ReminderTo = objNotes.ReminderTo
                  .ReminderDateTime = objNotes.ReminderDateTime
                  .Status = "New"
                  .CreatedDate = Now
                  .User = objNotes.UserId
                End With
                xRem = SIS.NT.ntReminder.InsertReminder(xRem)
              End If
              If txtMailTo.Text <> "" Then
                SIS.NT.ntNotes.SendEMail(objNotes)
              End If
              txtTitle.Text = ""
              txtDescription.Text = ""
              rptNotes.DataBind()
              ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('Successfully Saved');", True)
            Else
              ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('Notes Handle does not exist');", True)
            End If
          Else
            objNotes.NotesId = hdfNewNoteId.Value
            Try
              SIS.NT.ntNotes.ntNotesUpdate(objNotes)
              txtTitle.Text = ""
              txtDescription.Text = ""
              hdfNewNoteId.Value = ""
              rptNotes.DataBind()
              ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('Successfully Saved');", True)
            Catch ex As Exception
              ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('Not Updated');", True)
            End Try
          End If
        Else
          objNotes.NotesId = hdfNoteId.Value
          Try
            SIS.NT.ntNotes.ntNotesUpdate(objNotes)
            rptNotes.DataBind()
            ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('Successfully Updated');", True)
          Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('Not Updated');", True)
          End Try
        End If
      Else
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('Please Enter all fields');", True)
      End If
    Catch ex As System.Exception
      'ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('Due to some technical issue record not Saved');", True)
      ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('" & ex.Message.Replace("'", " ") & "');", True)
    End Try
  End Sub
  Protected Sub btnDeleteNotes_Click(sender As Object, e As EventArgs)
    Try
      Dim x As SIS.NT.ntNotes = SIS.NT.ntNotes.ntNotesGetByID(hdfNoteId.Value)
      SIS.NT.ntNotes.ntNotesDelete(x)
      txtTitle.Text = ""
      txtDescription.Text = ""
      rptNotes.DataBind()
      ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('Successfully Deleted');", True)
    Catch ex As System.Exception
      ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('Due to some technical issue data Not deleted');", True)
    End Try
  End Sub
  Protected Sub btnNewNotes_Click(sender As Object, e As EventArgs)
    txtTitle.Enabled = True
    txtDescription.Enabled = True
    txtTitle.Text = ""
    txtDescription.Text = ""
    btnSaveNotes.Text = "Submit"
    btnDeleteNotes.Visible = False
    btnSaveNotes.Enabled = True
    txtMailTo.Text = em
    txtTitle.Text = tl
    spIndex.InnerHtml = Hd
    hdfNoteId.Value = ""
    hdfNewNoteId.Value = ""
  End Sub
  Protected Sub lnkUpdate_Click(sender As Object, e As EventArgs)
    Try
      Dim lnkBtn As LinkButton = CType(sender, LinkButton)
      Dim Value As String() = lnkBtn.CommandArgument.Split("&".ToCharArray)
      Dim uId As String = Value(0)
      hdfUser.Value = uId
      hdfNoteId.Value = Value(1)
      Dim x As SIS.NT.ntNotes = SIS.NT.ntNotes.ntNotesGetByID(hdfNoteId.Value)
      txtMailTo.Text = x.SendEmailTo
      txtTitle.Text = x.Title
      txtDescription.Text = x.Description
      txtDescription.Attributes.Add("style", "background-color:" & x.ColorID & ";")
      btnNewNotes.Attributes.Add("style", "background-color:" & x.ColorID & ";")
      txtMailIdReminder.Text = x.ReminderTo
      txtDate.Text = x.ReminderDateTime
      If uId = LoginUser Then
        txtTitle.Enabled = True
        txtDescription.Enabled = True
        btnSaveNotes.Text = "Update"
        btnSaveNotes.Enabled = True
        'btnDeleteNotes.Enabled = True
        'btnDeleteNotes.Visible = True
        btnDeleteNotes.Enabled = False
        btnDeleteNotes.Visible = False
        txtMailTo.Enabled = True
      Else
        txtTitle.Enabled = False
        txtDescription.Enabled = False
        btnSaveNotes.Enabled = False
        btnDeleteNotes.Enabled = False
        txtMailTo.Enabled = False
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('You are not authorised to update records');", True)
      End If
    Catch ex As System.Exception
      ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('Due to some technical issue record not Update');", True)
    End Try
  End Sub
  Dim oNotes As New List(Of SIS.NT.ntNotes)

  Private Sub ODS1_Selected(sender As Object, e As ObjectDataSourceStatusEventArgs) Handles ODS1.Selected
    oNotes = e.ReturnValue
    If Hd = "" Then
      If oNotes.Count > 0 Then
        spIndex.InnerHtml = oNotes(0).TableDescription & ", " & Index
      Else
        Dim oHandle As SIS.NT.ntHandles = SIS.NT.ntHandles.GetByID(handle)
        If oHandle IsNot Nothing Then
          spIndex.InnerHtml = oHandle.TableDescription & ", " & Index
        End If
      End If

    End If
  End Sub

  Private Sub Notes_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
    If em <> "" Then
      txtMailTo.Text = em
    End If
    If tl <> "" Then
      txtTitle.Text = tl
    End If
    spIndex.InnerHtml = Hd
  End Sub
  Protected Sub rptNotes_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
    Try
      If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
        Dim tr As HtmlTableRow = CType(e.Item.FindControl("row"), HtmlTableRow)
        Dim hdfUser As HiddenField = CType(e.Item.FindControl("hdfUserID"), HiddenField)
        tr.Attributes.Add("style", "background-color:" & hdfUser.Value & ";")
      End If
    Catch ex As Exception
    End Try

  End Sub
  Protected Sub btnAttachment_Click(sender As Object, e As EventArgs)
    Try
      Dim url As String
      If hdfNoteId.Value <> "" Then
        If hdfUser.Value = LoginUser Then
          url = "Attachment.aspx?AthHandle=JOOMLA_NOTES" & "&Index=" & hdfNoteId.Value & "&AttachedBy=" & LoginUser & "&ed=n"
        Else
          url = "Attachment.aspx?AthHandle=JOOMLA_NOTES" & "&Index=" & hdfNoteId.Value & "&AttachedBy=" & LoginUser & "&ed=n"
        End If
        Dim s As String = "window.open('" & url & "', 'popup_window','width=500,height=300,left=50,top=50,resizable=yes');"
        ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)
      Else
        Dim x As SIS.NT.ntNotes = Nothing
        If hdfNewNoteId.Value = "" Then
          x = New SIS.NT.ntNotes
          With x
            .NotesHandle = handle
            .IndexValue = Index
            .Title = If(txtTitle.Text <> "", txtTitle.Text.Trim(), "Only Attachment")
            .Description = If(txtDescription.Text <> "", txtDescription.Text.Trim(), "Only Attachment")
            .UserId = LoginUser
            .SendEmailTo = txtMailTo.Text
            .Created_Date = Now
            .ReminderTo = txtMailIdReminder.Text.Trim()
            .ReminderDateTime = If(txtDate.Text <> "", Convert.ToDateTime(txtDate.Text.Trim()).ToString("yyyy-MM-dd") & " " & "09:00", System.DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") & " " & "09:00")
          End With
          x = SIS.NT.ntNotes.InsertNotes(x)
          hdfNewNoteId.Value = x.NotesId
          'If x.ReminderTo <> "" Then
          '  Dim xRem As New SIS.NT.ntReminder
          '  With xRem
          '    .NotesId = x.NotesId
          '    .ReminderTo = x.ReminderTo
          '    .ReminderDateTime = x.ReminderDateTime
          '    .Status = "New"
          '    .CreatedDate = Now
          '    .User = x.UserId
          '  End With
          '  xRem = SIS.NT.ntReminder.InsertReminder(xRem)
          'End If
          'If x.SendEmailTo <> "" Then
          '  SIS.NT.ntNotes.SendEMail(x)
          'End If
          'txtTitle.Text = ""
          'txtDescription.Text = ""
          'rptNotes.DataBind()

        Else
          x = SIS.NT.ntNotes.ntNotesGetByID(hdfNewNoteId.Value)
        End If
        If x.NotesId <> "0" Then
          url = "Attachment.aspx?AthHandle=JOOMLA_NOTES" & "&Index=" & x.NotesId & "&AttachedBy=" & LoginUser & "&ed=y"
          Dim s As String = "window.open('" & url & "', 'popup_window','width=500,height=300,left=50,top=50,resizable=yes');"
          ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)
        Else
          ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('Notes Handle does not exist');", True)
        End If
      End If
    Catch ex As System.Exception
      ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('Due to some technical issue record not Saved');", True)
    End Try
  End Sub
End Class
