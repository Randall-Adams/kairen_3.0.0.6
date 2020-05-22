Public Class FolderClass
    Public ReadOnly Location As String
    Public ReadOnly NameWithoutPath As String
    Sub New(ByVal _location As String)
        If _location.EndsWith("\") = False Then _location += "\"
        Location = _location
        NameWithoutPath = Microsoft.VisualBasic.Right(Location, Location.Length - Microsoft.VisualBasic.Left(Location, Location.LastIndexOf("\")).LastIndexOf("\")).Replace("\", "")
    End Sub
    Public ReadOnly Property Exists() As Boolean
        Get
            Return My.Computer.FileSystem.DirectoryExists(Location)
        End Get
    End Property
    Public Sub Create()
        If Exists = False Then
            My.Computer.FileSystem.CreateDirectory(Location)
        End If
    End Sub
    Public Sub Delete()
        If Exists = True Then
            My.Computer.FileSystem.DeleteDirectory(Location, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
        End If
    End Sub
End Class
