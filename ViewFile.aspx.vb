
Imports System.Net
Imports System.Web.Script.Serialization
Imports ejiVault
Partial Class ViewFile
  Inherits System.Web.UI.Page

  Private Sub ViewFile_Load(sender As Object, e As EventArgs) Handles Me.Load
    SIS.SYS.Utilities.ApplicationSpacific.Initialize()
    Try
      Dim t_drid As String = Request.QueryString("t_drid")
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
      Response.Headers.Add("content-disposition", "inline; filename=" & rDoc.t_fnam)
      Response.ContentType = SIS.SYS.Utilities.ApplicationSpacific.ContentType(rDoc.t_fnam)
      Dim aByte() As Byte = wc.DownloadData(FilePath)
      Response.BinaryWrite(aByte)
      Response.End()
    Catch ex As Exception
      ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "", "alert('" & New JavaScriptSerializer().Serialize(ex.Message) & "');", True)
    End Try

  End Sub
End Class
