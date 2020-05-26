Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.NT
  Public Class ntHandles
    Public Property NotesHandle As String = ""
    Public Property DBID As String = ""
    Public Property TableName As String = ""
    Public Property TableDescription As String = ""
    Public Property AccessIndex As String = ""
    Public Property Remarks As String = ""
    Public Shared Function GetByID(Handle As String) As SIS.NT.ntHandles
      Dim Results As SIS.NT.ntHandles = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = " select top 1 * from Note_Handle where NotesHandle='" & Handle & "'"
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.NT.ntHandles(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    Sub New(rd As SqlDataReader)
      SIS.SYS.SQLDatabase.DBCommon.NewObj(Me, rd)
    End Sub
    Sub New()

    End Sub
  End Class
End Namespace
