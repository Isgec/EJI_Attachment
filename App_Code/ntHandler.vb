Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Data

<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class ntHandler
  Inherits System.Web.Services.WebService

  <WebMethod()>
  Public Function getEMailIDs(context As String, cnt As String) As String
    Return apiTags.selectAutoComplete(context, cnt)
  End Function
  Public Class apiResp
    Public Property err As Boolean = False
    Public Property msg As String = ""
    Public Property strHTML As New List(Of String)
  End Class

  Public Class apiTags
      Public Property tagID As String = ""
      Public Property tagType As String = ""
      Public Property tagDescription As String = ""
      Public Property active As Boolean = False
      Public Property parentTagID As String = ""

      Public Shared Function selectAutoComplete(prefix As String, Optional count As Integer = 10) As String
      Dim mret As New apiResp
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "select top " & count & " username, userfullname,emailid from aspnet_users where (upper(userfullname) like '" & prefix.ToUpper & "%' or upper(username) like '" & prefix.ToUpper & "%')"
          Con.Open()
            Dim Reader As SqlDataReader = Cmd.ExecuteReader()
            While Reader.Read()
            mret.strHTML.Add(Reader("userfullname") & ":" & Reader("username") & ":" & Reader("emailid"))
          End While
            Reader.Close()
          End Using
        End Using
        Return New JavaScriptSerializer().Serialize(mret)
      End Function

      Sub New(rd As SqlDataReader)
        SIS.SYS.SQLDatabase.DBCommon.NewObj(Me, rd)
      End Sub
      Sub New()
      End Sub
    End Class

  End Class

