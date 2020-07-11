Imports System.Web.Script.Serialization
Imports System.Net
Imports ejiVault
Imports System.IO
Imports System.Net.Mail
Partial Class NotesNew
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
  Private Sub NotesNew_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
    If em <> "" Then
      If txtMailTo.Text = "" Then
        txtMailTo.Text = em
      End If
    End If
    If tl <> "" Then
      If txtTitle.Text = "" Then
        txtTitle.Text = tl
      End If
    End If
    divHeader.InnerHtml = Hd
    LoadData()
  End Sub

  Private Sub LoadData()
    Dim ntS As List(Of SIS.NT.ntNotes) = SIS.NT.ntNotes.GetNotesByHandleIndex(handle, Index)
    For Each nt As SIS.NT.ntNotes In ntS
      Dim ctl As String = ""
      If nt.UserId = LoginUser Then
        ctl = GetNote(nt, "myNote")
      Else
        ctl = GetNote(nt, "otherNote")
      End If
      If ctl <> "" Then
        oldNote.Controls.Add(New LiteralControl(ctl))
      End If
    Next
  End Sub
  Private Function GetNote(nt As SIS.NT.ntNotes, clsStr As String) As String
    Dim mRet As String = ""
    mRet &= "          <div class='nt-" & clsStr & "'>"
    mRet &= "            <div class='nt-" & clsStr & "-msg'>"
    mRet &= "              <div class='nt-" & clsStr & "-msg-h'>"
    mRet &= "                <div class='nt-div-row'>" & nt.EmployeeName & "</div>"
    mRet &= "                <div class='nt-div-row'>" & nt.NotesDate & "</div>"
    mRet &= "                <div class='nt-div-row nt-icon' onclick=""return nt_script.display_note('bs_" & nt.NotesId & "');"" ><i class='fas fa-angle-down'></i></div>"
    mRet &= "              </div>"
    mRet &= "              <div id='bs_" & nt.NotesId & "' class='nt-" & clsStr & "-msg-b'>"
    mRet &= "                <p id='title_" & nt.NotesId & "' >"
    mRet &= "                    " & nt.Title
    mRet &= "                </p>"
    mRet &= "                <div>"
    nt.Description = nt.Description.Replace(Chr(10), "<br/>").Replace(Chr(13), "<br/>")
    If nt.Description.Length > 100 Then
      Dim lessPart As String = ""
      Dim morePart As String = ""
      lessPart = nt.Description.Substring(0, 99)
      morePart = nt.Description.Substring(100)
      mRet &= "                <span id='lp_" & nt.NotesId & "'>" & lessPart & "</span>"
      mRet &= "                <span id='dots_" & nt.NotesId & "'>...</span><span id='more_" & nt.NotesId & "' style='display:none;'>"
      mRet &= "                <span id='mp_" & nt.NotesId & "'>" & morePart & "</span>"
      mRet &= "                </span>"
      mRet &= "                 <button class='nt-but-readmore' onclick='return nt_script.show_read(this);' id='cmd_" & nt.NotesId & "'>Read more</button>"
    Else
      mRet &= "                <span id='lp_" & nt.NotesId & "'>" & nt.Description & "</span>"
    End If
    mRet &= "                </div>"
    If nt.SendEmailTo <> "" Then
      mRet &= "              <p class='nt-sendto' data-notesid='" & nt.NotesId & "' onclick='return nt_script.show_new(this);'>"
      mRet &= "                " & nt.SendEmailTo
      mRet &= "              </p>"
    End If
    mRet &= GetAttachments(nt)
    mRet &= "              </div>"
    mRet &= "            </div>"
    mRet &= "          </div>"
    Return mRet
  End Function
  Private Function GetAttachments(nt As SIS.NT.ntNotes) As String
    Dim mRet As String = ""
    Dim aChs As List(Of EJI.ediAFile) = EJI.ediAFile.GetFilesByHandleIndex("JOOMLA_NOTES", nt.NotesId)
    If aChs.Count > 0 Then
      mRet &= "                <div class='nt-note-attachment'>"
      For Each ach As EJI.ediAFile In aChs
        mRet &= "                  <div class='nt-note-attachment-link'>"
        If IsImage(IO.Path.GetExtension(ach.t_fnam)) Then
          Try
            Dim rDoc As EJI.ediAFile = EJI.ediAFile.GetFileByRecordID(ach.t_drid)
            Dim rLib As EJI.ediALib = EJI.ediALib.GetLibraryByID(rDoc.t_lbcd)
            If Not EJI.DBCommon.IsLocalISGECVault Then
              EJI.ediALib.ConnectISGECVault(rLib)
            End If
            Dim FilePath As String = rLib.LibraryPath & "\" & rDoc.t_dcid
            Dim img1 As System.Drawing.Image = System.Drawing.Image.FromFile(FilePath)
            Dim aH As Integer = img1.Height
            Dim aW As Integer = img1.Width
            Dim FixedH As Integer = 100
            Dim NowW As Integer = (FixedH / aH) * aW
            Dim bmp1 As System.Drawing.Image = img1.GetThumbnailImage(NowW, FixedH, Nothing, IntPtr.Zero)
            Dim imgBytes As Byte() = GetByteArray(bmp1)
            Dim imgString As String = Convert.ToBase64String(imgBytes)
            mRet &= "                     <div class='nt-thumb'>"
            mRet &= "                    " & String.Format("<img onclick='return nt_script.display_img(this);' src='data:image/Bmp;base64,{0}'/>", imgString)
            mRet &= "                     </div>"
          Catch ex As Exception
            Dim aa As String = ex.Message
          End Try
        End If
        mRet &= "                    <a href='#' class='nt-but-grey' data-drid='" & ach.t_drid & "' onclick='return nt_script.download(this);'>" & ach.t_fnam & "</a>"
        mRet &= "                  </div>"
      Next
      mRet &= "                </div>"
    End If
    Return mRet
  End Function
  Private Function IsImage(extn As String) As Boolean
    Dim mRet As Boolean = False
    Select Case extn.ToLower
      Case ".bmp", ".jpg", ".jpeg", ".jpe", ".jif", ".jfif", ".jfi", ".png", ".gif", ".tif", ".tiff"
        mRet = True
    End Select
    Return mRet
  End Function
  Private Function GetByteArray(img As System.Drawing.Image) As Byte()
    Dim ms As MemoryStream = New MemoryStream()
    img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp)
    Return ms.ToArray()
  End Function
  Protected Sub cmdDownload_Click(sender As Object, e As EventArgs)
    Dim t_drid As String = cmdText.Text
    Try
      Dim rDoc As EJI.ediAFile = EJI.ediAFile.GetFileByRecordID(t_drid)
      Dim rLib As EJI.ediALib = EJI.ediALib.GetLibraryByID(rDoc.t_lbcd)
      If Not EJI.DBCommon.IsLocalISGECVault Then
        EJI.ediALib.ConnectISGECVault(rLib)
      End If
      Dim FilePath As String = rLib.LibraryPath & "\" & rDoc.t_dcid
      Dim wc As WebClient = New WebClient
      Response.Clear()
      Response.ClearContent()
      Response.ClearHeaders()
      Response.Buffer = True
      Response.AppendHeader("content-disposition", "attachment; filename=" & rDoc.t_fnam)
      Response.ContentType = SIS.SYS.Utilities.ApplicationSpacific.ContentType(rDoc.t_fnam)
      Dim aByte() As Byte = wc.DownloadData(FilePath)
      Response.BinaryWrite(aByte)
      Response.End()
    Catch ex As Exception
      ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "", "alert('" & New JavaScriptSerializer().Serialize(ex.Message) & "');", True)
    End Try

  End Sub
  Dim aMsg As ArrayList = Nothing
  Private mErr As Boolean = False
  Private Function IsValidData() As Boolean
    Dim mRet As Boolean = True
    txtTitle.Text = txtTitle.Text.Trim
    txtDescription.Text = txtDescription.Text.Trim
    txtMailTo.Text = txtMailTo.Text.Trim
    txtReminderTo.Text = txtReminderTo.Text.Trim
    txtDate.Text = txtDate.Text.Trim
    aMsg = New ArrayList
    If Request.Files.Count <= 0 Then
      If txtTitle.Text = "" Or txtDescription.Text = "" Then
        aMsg.Add("Title and Description are mandatory")
        mRet = False
      End If
    End If
    If txtReminderTo.Text <> "" And txtDate.Text = "" Then
      aMsg.Add("Reminder Date is required.")
      mRet = False
    End If
    Dim ad As MailAddress = Nothing
    If txtMailTo.Text <> "" Then
      Dim aTmp As String() = txtMailTo.Text.Split(",;".ToCharArray)
      If aTmp.Count > 0 Then
        For Each sTmp As String In aTmp
          sTmp = sTmp.Trim
          If sTmp = "" Then Continue For
          Try
            ad = New MailAddress(sTmp)
          Catch ex As Exception
            aMsg.Add("Invalid E-Mail ID in Send To: " & sTmp)
            mRet = False
          End Try
        Next
      End If
    End If
    If txtReminderTo.Text <> "" Then
      Dim aTmp As String() = txtReminderTo.Text.Split(",;".ToCharArray)
      If aTmp.Count > 0 Then
        For Each sTmp As String In aTmp
          sTmp = sTmp.Trim
          If sTmp = "" Then Continue For
          Try
            ad = New MailAddress(sTmp)
          Catch ex As Exception
            aMsg.Add("Invalid E-Mail ID in Reminder: " & sTmp)
            mRet = False
          End Try
        Next
      End If
    End If
    If txtDate.Text <> "" Then
      Try
        Dim x As DateTime = Convert.ToDateTime(txtDate.Text)
      Catch ex As Exception
        aMsg.Add("Invalid Reminder Date.")
        mRet = False
      End Try
    End If
    Return mRet
  End Function
  Private Sub cmdSubmit_Click(sender As Object, e As EventArgs) Handles cmdSubmit.Click
    divErr.Controls.Clear()
    divErr.Style("display") = "none"
    aMsg = New ArrayList
    If Not IsValidData() Then
      mErr = True
      For Each str As String In aMsg
        Dim x As String = "<li class='nt-err-msg'>" & str & "</li>"
        divErr.Controls.Add(New LiteralControl(x))
      Next
      divErr.Style("display") = "block"
      afterLoad.InnerHtml = "<script>nt_script.show_new_server();</script>"
      Exit Sub
    End If
    Try
      Dim oNT As SIS.NT.ntNotes = New SIS.NT.ntNotes
      oNT.NotesHandle = handle
      oNT.IndexValue = Index
      oNT.Title = IIf(txtTitle.Text = "", "Only Attachment", txtTitle.Text)
      oNT.Description = IIf(txtDescription.Text = "", "File Attached.", txtDescription.Text)
      oNT.UserId = LoginUser
      oNT.SendEmailTo = txtMailTo.Text
      oNT.Created_Date = Now
      oNT.ReminderTo = txtReminderTo.Text
      oNT.ReminderDateTime = If(txtDate.Text <> "", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") & " " & "09:00", System.DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") & " " & "09:00")
      oNT.NotesId = SIS.NT.ntNotes.GetNextNotesID
      oNT.Notes_RunningNo = oNT.NotesId.Replace("Notes", "")
      oNT.ChildNotesID = F_ChildNotesID.Text
      oNT = SIS.NT.ntNotes.InsertData(oNT)
      'Upload Files
      '=============
      If Request.Files.Count > 0 Then
        UploadFiles(Request, "JOOMLA_NOTES", oNT.NotesId, LoginUser)
      End If
      '=============
      If oNT.ReminderTo <> "" Then
        Dim xRem As New SIS.NT.ntReminder
        With xRem
          .NotesId = oNT.NotesId
          .ReminderTo = oNT.ReminderTo
          .ReminderDateTime = oNT.ReminderDateTime
          .Status = "New"
          .CreatedDate = Now
          .User = oNT.UserId
        End With
        xRem = SIS.NT.ntReminder.InsertReminder(xRem)
      End If
      If txtMailTo.Text <> "" Then
        SIS.NT.ntNotes.SendEMail(oNT)
      End If
      afterLoad.InnerHtml = "<script>nt_script.hide_new();</script>"
    Catch ex As System.Exception
      mErr = True
      aMsg.Add(ex.Message)
      ScriptManager.RegisterStartupScript(Me, GetType(Page), "alert", "alert('" & ex.Message.Replace("'", " ") & "');", True)
    End Try
  End Sub

  Public Shared Function UploadFiles(ByVal oRequest As HttpRequest, ByVal AthHandle As String, ByVal AthIndex As String, Optional ByVal CreatedBy As String = "") As List(Of EJI.ediAFile)
    Dim mRet As New List(Of EJI.ediAFile)
    If oRequest.Files.Count > 0 Then
      Dim LibraryID As String = ""
      Dim LibPath As String = ""
      Dim tmp As EJI.ediALib = EJI.ediALib.GetActiveLibrary
      LibPath = tmp.LibraryPath
      LibraryID = tmp.t_lbcd
      If Not EJI.DBCommon.IsLocalISGECVault Then
        EJI.ediALib.ConnectISGECVault(tmp)
      End If
      Dim tmpFilesToDelete As New ArrayList

      For I As Integer = 0 To oRequest.Files.Count - 1
        Dim fu As HttpPostedFile = oRequest.Files(I)
        If fu.ContentLength <= 0 Then Continue For
        If fu.FileName = "" Then Continue For
        Dim tmpPath As String = HttpContext.Current.Server.MapPath("~/../App_Temp")
        Dim tmpName As String = IO.Path.GetRandomFileName()
        Dim tmpFile As String = tmpPath & "\\" & tmpName
        fu.SaveAs(tmpFile)
        tmpFilesToDelete.Add(tmpFile)

        Dim tmpVault As EJI.ediAFile = New EJI.ediAFile
        Dim LibFileID As String = ""
        With tmpVault
          LibFileID = EJI.ediASeries.GetNextFileID
          .t_drid = EJI.ediASeries.GetNextRecordID
          .t_dcid = LibFileID
          .t_hndl = AthHandle
          .t_indx = AthIndex
          .t_prcd = "EJIMAIN"
          .t_fnam = IO.Path.GetFileName(fu.FileName).Replace(",", "-").Replace("'", "-")
          .t_lbcd = LibraryID
          .t_atby = CreatedBy
          .t_aton = Now.ToString("dd/MM/yyyy")
          .t_Refcntd = 0
          .t_Refcntu = 0
        End With
        tmpVault = EJI.ediAFile.InsertData(tmpVault)
        Try
          If IO.File.Exists(LibPath & "\" & LibFileID) Then
            IO.File.Delete(LibPath & "\" & LibFileID)
          End If
          IO.File.Move(tmpFile, LibPath & "\" & LibFileID)
        Catch ex As Exception
        End Try
        mRet.Add(tmpVault)
      Next
      'Move command is used, Below code not required
      'For Each str As String In tmpFilesToDelete
      '  IO.File.Delete(str)
      'Next
      If Not EJI.DBCommon.IsLocalISGECVault Then
        EJI.ediALib.DisconnectISGECVault()
      End If
    End If
    Return mRet
  End Function

End Class
