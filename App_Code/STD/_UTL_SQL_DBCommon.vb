Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Configuration
Imports Microsoft.VisualBasic
Imports System.Diagnostics
Namespace SIS.SYS.SQLDatabase
  Partial Public Class DBCommon
    Implements IDisposable
    Public Shared Property BaaNLive As Boolean = False
    Public Shared Property JoomlaLive As Boolean = False

    Public Shared Function GetBaaNConnectionString() As String
      BaaNLive = Convert.ToBoolean(ConfigurationManager.AppSettings("BaaNLive"))
      If BaaNLive Then
        Return "Data Source=ganesha;Initial Catalog=inforerpdb;Integrated Security=False;User Instance=False;Persist Security Info=True;User ID=lalit;Password=scorpions"
      Else
        Return "Data Source=gstdrill04;Initial Catalog=inforerpdb;Integrated Security=False;User Instance=False;Persist Security Info=True;User ID=lalit;Password=scorpions"
      End If
    End Function
    Public Shared Function GetConnectionString() As String
      JoomlaLive = Convert.ToBoolean(ConfigurationManager.AppSettings("JoomlaLive"))

      If JoomlaLive Then
        Return "Data Source=perk03;Initial Catalog=IJTPerks;Integrated Security=False;User Instance=False;Persist Security Info=True;User ID=sa;Password=isgec12345"
      Else
        Return "Data Source=.\LGSQL;Initial Catalog=IJTPerks;Integrated Security=False;User Instance=False;Persist Security Info=True;User ID=sa;Password=isgec12345"
      End If
    End Function
    Public Shared Sub AddDBParameter(ByRef Cmd As SqlCommand, ByVal name As String, ByVal type As SqlDbType, ByVal size As Integer, ByVal value As Object)
      Dim Parm As SqlParameter = Cmd.CreateParameter()
      Parm.ParameterName = name
      Parm.SqlDbType = type
      Parm.Size = size
      Parm.Value = value
      Cmd.Parameters.Add(Parm)
    End Sub
    Private disposedValue As Boolean = False    ' To detect redundant calls

    Shared Sub New()
      BaaNLive = Convert.ToBoolean(ConfigurationManager.AppSettings("BaaNLive"))
      JoomlaLive = Convert.ToBoolean(ConfigurationManager.AppSettings("JoomlaLive"))
    End Sub
    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
      If Not Me.disposedValue Then
        If disposing Then
          ' TODO: free unmanaged resources when explicitly called
        End If

        ' TODO: free shared unmanaged resources
      End If
      Me.disposedValue = True
    End Sub
#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
      ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
      Dispose(True)
      GC.SuppressFinalize(Me)
    End Sub
#End Region
    Public Shared Function NewObj(this As Object, Reader As SqlDataReader) As Object
      Try
        For Each pi As System.Reflection.PropertyInfo In this.GetType.GetProperties
          If pi.MemberType = Reflection.MemberTypes.Property Then
            Try
              Dim Found As Boolean = False
              For I As Integer = 0 To Reader.FieldCount - 1
                If Reader.GetName(I).ToLower = pi.Name.ToLower Then
                  Found = True
                  Exit For
                End If
              Next
              If Found Then
                If Convert.IsDBNull(Reader(pi.Name)) Then
                  Select Case Reader.GetDataTypeName(Reader.GetOrdinal(pi.Name))
                    Case "decimal"
                      CallByName(this, pi.Name, CallType.Let, "0.00")
                    Case "bit"
                      CallByName(this, pi.Name, CallType.Let, Boolean.FalseString)
                    Case "bigint"
                      CallByName(this, pi.Name, CallType.Let, 0)
                    Case Else
                      CallByName(this, pi.Name, CallType.Let, String.Empty)
                  End Select
                Else
                  CallByName(this, pi.Name, CallType.Let, Reader(pi.Name))
                End If
              End If
            Catch ex As Exception
            End Try
          End If
        Next
      Catch ex As Exception
        Return Nothing
      End Try
      Return this
    End Function

  End Class
End Namespace
