Imports System.Net
Imports System.Web.Script.Serialization
Imports ejiVault
Partial Class Attachment
  Inherits System.Web.UI.Page
  Public ReadOnly Property Editable As Boolean
    Get
      Return IIf(ed.ToLower = "y", True, False)
    End Get
  End Property


  Protected Sub btnUpload_Click(sender As Object, e As EventArgs)
    If AthHandle <> "" AndAlso Index <> "" AndAlso AttachedBy <> "" Then
      UploadFiles(Request, AthHandle, Index, AttachedBy)
      gvAttachment.DataBind()
    Else
      ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "", "alert('" & New JavaScriptSerializer().Serialize("Handle/Index/User may be missing.") & "');", True)
    End If
  End Sub
  Private AthHandle As String = ""
  Private Index As String = ""
  Private AttachedBy As String = ""
  Private ed As String = ""
  Private Sub Attachment_Load(sender As Object, e As EventArgs) Handles Me.Load
    SIS.SYS.Utilities.ApplicationSpacific.Initialize()
    If Not String.IsNullOrEmpty(Request.QueryString("AthHandle")) Then
      AthHandle = Request.QueryString("AthHandle")
    End If
    If Not String.IsNullOrEmpty(Request.QueryString("Index")) Then
      Index = Request.QueryString("Index")
    End If
    If Not String.IsNullOrEmpty(Request.QueryString("AttachedBy")) Then
      AttachedBy = Request.QueryString("AttachedBy")
    End If
    If Not String.IsNullOrEmpty(Request.QueryString("ed")) Then
      ed = Request.QueryString("ed")
    End If

    If ed.ToLower = "n" Then
      divUploadAttachment.Visible = False
    End If
    Dim Admins As String = ConfigurationManager.AppSettings("Admin")
    If Admins.IndexOf(AttachedBy) >= 0 Then
      gvAttachment.Columns(5).Visible = True
      'gvAttachment.Columns(3).Visible = True
    Else
      gvAttachment.Columns(5).Visible = False
      'gvAttachment.Columns(3).Visible = False
    End If
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

  Private Sub gvAttachment_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvAttachment.RowCommand
    If e.CommandName.ToLower = "lgDownload".ToLower Then
      Try
        Dim t_drid As String = gvAttachment.DataKeys(e.CommandArgument).Values("t_drid")
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
    End If
    If e.CommandName.ToLower = "lgView".ToLower Then
      'Both Open in New Window and open in same window
      'Given: This is for open in same window
      'Open in new window is called from Client side javascript
      'http://perk01/Attachment/Attachment.aspx?AthHandle=J_IDMSPOSTORDERREC&Index=2835_1_1540_1529&AttachedBy=0340&ed=n
      Try
        Dim t_drid As String = gvAttachment.DataKeys(e.CommandArgument).Values("t_drid")
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
        Response.AppendHeader("content-disposition", "inline; filename=" & rDoc.t_fnam)
        Response.ContentType = SIS.SYS.Utilities.ApplicationSpacific.ContentType(rDoc.t_fnam)
        Dim aByte() As Byte = wc.DownloadData(FilePath)
        Response.BinaryWrite(aByte)
        Response.End()
      Catch ex As Exception
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "", "alert('" & New JavaScriptSerializer().Serialize(ex.Message) & "');", True)
      End Try
    End If
    If e.CommandName.ToLower = "lgDelete".ToLower Then
      Try
        Dim t_drid As String = gvAttachment.DataKeys(e.CommandArgument).Values("t_drid")
        EJI.ediAFile.FileDelete(t_drid)
        gvAttachment.DataBind()
      Catch ex As Exception
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "", "alert('" & New JavaScriptSerializer().Serialize(ex.Message) & "');", True)
      End Try
    End If
    If e.CommandName.ToLower = "lgFDelete".ToLower Then
      Try
        Dim t_drid As String = gvAttachment.DataKeys(e.CommandArgument).Values("t_drid")
        EJI.ediAFile.ForcedFileDelete(t_drid)
        gvAttachment.DataBind()
      Catch ex As Exception
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "", "alert('" & New JavaScriptSerializer().Serialize(ex.Message) & "');", True)
      End Try
    End If

  End Sub
End Class
