Public Class FileClass
    Public ReadOnly Location As String
    Sub New(ByVal _location As String)
        Location = _location
    End Sub
    Public ReadOnly Property Exists() As Boolean
        Get
            Return My.Computer.FileSystem.FileExists(Location)
        End Get
    End Property
    Public Sub Delete()
        If Exists = True Then
            My.Computer.FileSystem.DeleteFile(Location, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
        End If
    End Sub
End Class
