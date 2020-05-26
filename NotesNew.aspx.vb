
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


End Class
