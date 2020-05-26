Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.NT
  <DataObject()> _
  Partial Public Class ntReminder
    Private Shared _RecordCount As Integer
    Private _ReminderID As Int32 = 0
    Private _NotesId As String = ""
    Private _ReminderTo As String = ""
    Private _ReminderDateTime As String = ""
    Private _Status As String = ""
    Private _CreatedDate As String = ""
    Private _User As String = ""
    Private _aspnet_Users1_UserFullName As String = ""
    Private _FK_Note_Reminder_User As SIS.QCM.qcmUsers = Nothing
    Public ReadOnly Property ForeColor() As System.Drawing.Color
      Get
        Dim mRet As System.Drawing.Color = Drawing.Color.Blue
        Try
          mRet = GetColor()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public ReadOnly Property Visible() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetVisible()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public ReadOnly Property Enable() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetEnable()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public Property ReminderID() As Int32
      Get
        Return _ReminderID
      End Get
      Set(ByVal value As Int32)
        _ReminderID = value
      End Set
    End Property
    Public Property NotesId() As String
      Get
        Return _NotesId
      End Get
      Set(ByVal value As String)
        _NotesId = value
      End Set
    End Property
    Public Property ReminderTo() As String
      Get
        Return _ReminderTo
      End Get
      Set(ByVal value As String)
        _ReminderTo = value
      End Set
    End Property
    Public Property ReminderDateTime() As String
      Get
        If Not _ReminderDateTime = String.Empty Then
          Return Convert.ToDateTime(_ReminderDateTime).ToString("dd/MM/yyyy HH:mm")
        End If
        Return _ReminderDateTime
      End Get
      Set(ByVal value As String)
         _ReminderDateTime = value
      End Set
    End Property
    Public Property Status() As String
      Get
        Return _Status
      End Get
      Set(ByVal value As String)
        _Status = value
      End Set
    End Property
    Public Property CreatedDate() As String
      Get
        If Not _CreatedDate = String.Empty Then
          Return Convert.ToDateTime(_CreatedDate).ToString("dd/MM/yyyy HH:mm")
        End If
        Return _CreatedDate
      End Get
      Set(ByVal value As String)
         _CreatedDate = value
      End Set
    End Property
    Public Property User() As String
      Get
        Return _User
      End Get
      Set(ByVal value As String)
        _User = value
      End Set
    End Property
    Public Property aspnet_Users1_UserFullName() As String
      Get
        Return _aspnet_Users1_UserFullName
      End Get
      Set(ByVal value As String)
        _aspnet_Users1_UserFullName = value
      End Set
    End Property
    Public Readonly Property DisplayField() As String
      Get
        Return ""
      End Get
    End Property
    Public Readonly Property PrimaryKey() As String
      Get
        Return _ReminderID
      End Get
    End Property
    Public Shared Property RecordCount() As Integer
      Get
        Return _RecordCount
      End Get
      Set(ByVal value As Integer)
        _RecordCount = value
      End Set
    End Property
    Public Class PKntReminder
      Private _ReminderID As Int32 = 0
      Public Property ReminderID() As Int32
        Get
          Return _ReminderID
        End Get
        Set(ByVal value As Int32)
          _ReminderID = value
        End Set
      End Property
    End Class
    Public ReadOnly Property FK_Note_Reminder_User() As SIS.QCM.qcmUsers
      Get
        If _FK_Note_Reminder_User Is Nothing Then
          _FK_Note_Reminder_User = SIS.QCM.qcmUsers.qcmUsersGetByID(_User)
        End If
        Return _FK_Note_Reminder_User
      End Get
    End Property
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function ntReminderGetNewRecord() As SIS.NT.ntReminder
      Return New SIS.NT.ntReminder()
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)>
    Public Shared Function ntReminderGetByID(ByVal ReminderID As Int32) As SIS.NT.ntReminder
      Dim Results As SIS.NT.ntReminder = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spntReminderSelectByID"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ReminderID", SqlDbType.Int, ReminderID.ToString.Length, ReminderID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 9, "")
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.NT.ntReminder(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function IsReminderByNotesID(NotesID As String) As Boolean
      Dim mRet As Boolean = True
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "select isnull(count(*),0) as cnt from Note_Reminder where NotesId='" & NotesID & "'"
          Con.Open()
          Dim cnt As Integer = Cmd.ExecuteScalar
          If cnt = 0 Then
            mRet = False
          End If
        End Using
      End Using
      Return mRet
    End Function

    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function ntReminderSelectList(ByVal StartRowIndex As Integer, ByVal MaximumRows As Integer, ByVal OrderBy As String, ByVal SearchState As Boolean, ByVal SearchText As String) As List(Of SIS.NT.ntReminder)
      Dim Results As List(Of SIS.NT.ntReminder) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          If SearchState Then
            Cmd.CommandText = "spntReminderSelectListSearch"
            SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWord", SqlDbType.NVarChar, 250, SearchText)
          Else
            Cmd.CommandText = "spntReminderSelectListFilteres"
          End If
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StartRowIndex", SqlDbType.Int, -1, StartRowIndex)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaximumRows", SqlDbType.Int, -1, MaximumRows)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 9, "")
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OrderBy", SqlDbType.NVarChar, 50, OrderBy)
          Cmd.Parameters.Add("@RecordCount", SqlDbType.Int)
          Cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Results = New List(Of SIS.NT.ntReminder)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.NT.ntReminder(Reader))
          End While
          Reader.Close()
          _RecordCount = Cmd.Parameters("@RecordCount").Value
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function ntReminderSelectCount(ByVal SearchState As Boolean, ByVal SearchText As String) As Integer
      Return _RecordCount
    End Function
      'Select By ID One Record Filtered Overloaded GetByID
    <DataObjectMethod(DataObjectMethodType.Insert, True)> _
    Public Shared Function ntReminderInsert(ByVal Record As SIS.NT.ntReminder) As SIS.NT.ntReminder
      Dim _Rec As SIS.NT.ntReminder = SIS.NT.ntReminder.ntReminderGetNewRecord()
      With _Rec
        .ReminderID = Record.ReminderID
        .NotesId = Record.NotesId
        .ReminderTo = Record.ReminderTo
        .ReminderDateTime = Record.ReminderDateTime
        .Status = Record.Status
        .CreatedDate = Record.CreatedDate
        .User = Record.User
      End With
      Return SIS.NT.ntReminder.InsertData(_Rec)
    End Function
    Public Shared Function InsertData(ByVal Record As SIS.NT.ntReminder) As SIS.NT.ntReminder
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spntReminderInsert"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ReminderID",SqlDbType.Int,11, Record.ReminderID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@NotesId",SqlDbType.VarChar,201, Record.NotesId)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ReminderTo",SqlDbType.VarChar,501, Record.ReminderTo)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ReminderDateTime", SqlDbType.DateTime, 21, IIf(Record.ReminderDateTime = "", Now.AddDays(2), Record.ReminderDateTime))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Status",SqlDbType.VarChar,11, Record.Status)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedDate",SqlDbType.DateTime,21, Record.CreatedDate)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@User",SqlDbType.NVarChar,9, Record.User)
          Cmd.Parameters.Add("@Return_ReminderID", SqlDbType.Int, 11)
          Cmd.Parameters("@Return_ReminderID").Direction = ParameterDirection.Output
          Con.Open()
          Cmd.ExecuteNonQuery()
          Record.ReminderID = Cmd.Parameters("@Return_ReminderID").Value
        End Using
      End Using
      Return Record
    End Function
    Public Shared Function InsertReminder(ByVal Record As SIS.NT.ntReminder) As SIS.NT.ntReminder
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spnt_LG_ReminderInsert"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ReminderID", SqlDbType.Int, 11, Record.ReminderID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@NotesId", SqlDbType.VarChar, 201, Record.NotesId)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ReminderTo", SqlDbType.VarChar, 501, Record.ReminderTo)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ReminderDateTime", SqlDbType.DateTime, 21, IIf(Record.ReminderDateTime = "", Now.AddDays(2), Record.ReminderDateTime))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Status", SqlDbType.VarChar, 11, Record.Status)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedDate", SqlDbType.DateTime, 21, Record.CreatedDate)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@User", SqlDbType.NVarChar, 9, Record.User)
          Cmd.Parameters.Add("@Return_ReminderID", SqlDbType.Int, 11)
          Cmd.Parameters("@Return_ReminderID").Direction = ParameterDirection.Output
          Con.Open()
          Cmd.ExecuteNonQuery()
          Record.ReminderID = Cmd.Parameters("@Return_ReminderID").Value
        End Using
      End Using
      Return Record
    End Function
    <DataObjectMethod(DataObjectMethodType.Update, True)>
    Public Shared Function ntReminderUpdate(ByVal Record As SIS.NT.ntReminder) As SIS.NT.ntReminder
      Dim _Rec As SIS.NT.ntReminder = SIS.NT.ntReminder.ntReminderGetByID(Record.ReminderID)
      With _Rec
        .NotesId = Record.NotesId
        .ReminderTo = Record.ReminderTo
        .ReminderDateTime = Record.ReminderDateTime
        .Status = Record.Status
        .CreatedDate = Record.CreatedDate
        .User = Record.User
      End With
      Return SIS.NT.ntReminder.UpdateData(_Rec)
    End Function
    Public Shared Function UpdateData(ByVal Record As SIS.NT.ntReminder) As SIS.NT.ntReminder
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spntReminderUpdate"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_ReminderID",SqlDbType.Int,11, Record.ReminderID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ReminderID",SqlDbType.Int,11, Record.ReminderID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@NotesId",SqlDbType.VarChar,201, Record.NotesId)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ReminderTo",SqlDbType.VarChar,501, Record.ReminderTo)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ReminderDateTime",SqlDbType.DateTime,21, Record.ReminderDateTime)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Status",SqlDbType.VarChar,11, Record.Status)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedDate",SqlDbType.DateTime,21, Record.CreatedDate)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@User",SqlDbType.NVarChar,9, Record.User)
          Cmd.Parameters.Add("@RowCount", SqlDbType.Int)
          Cmd.Parameters("@RowCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Con.Open()
          Cmd.ExecuteNonQuery()
          _RecordCount = Cmd.Parameters("@RowCount").Value
        End Using
      End Using
      Return Record
    End Function
    <DataObjectMethod(DataObjectMethodType.Delete, True)> _
    Public Shared Function ntReminderDelete(ByVal Record As SIS.NT.ntReminder) As Int32
      Dim _Result as Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spntReminderDelete"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_ReminderID",SqlDbType.Int,Record.ReminderID.ToString.Length, Record.ReminderID)
          Cmd.Parameters.Add("@RowCount", SqlDbType.Int)
          Cmd.Parameters("@RowCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Con.Open()
          Cmd.ExecuteNonQuery()
          _RecordCount = Cmd.Parameters("@RowCount").Value
        End Using
      End Using
      Return _RecordCount
    End Function
    Public Sub New(ByVal Reader As SqlDataReader)
      Try
        For Each pi As System.Reflection.PropertyInfo In Me.GetType.GetProperties
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
                      CallByName(Me, pi.Name, CallType.Let, "0.00")
                    Case "bit"
                      CallByName(Me, pi.Name, CallType.Let, Boolean.FalseString)
                    Case Else
                      CallByName(Me, pi.Name, CallType.Let, String.Empty)
                  End Select
                Else
                  CallByName(Me, pi.Name, CallType.Let, Reader(pi.Name))
                End If
              End If
            Catch ex As Exception
            End Try
          End If
        Next
      Catch ex As Exception
      End Try
    End Sub
    Public Sub New()
    End Sub
  End Class
End Namespace
