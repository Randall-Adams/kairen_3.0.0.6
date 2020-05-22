Public Class FileClass_PlaylistFile
    Inherits FileClass_Base
    Sub New(ByVal _fileLocation As String, ByVal _fileName As String, Optional ByVal _fileExtension As String = ".txt")
        MyBase.New(_fileLocation, _fileName, _fileExtension)
    End Sub
    'general adder. specific adders like for .m3u can be created accepting parameters for the track length, etc.
    Public Sub AddMediaToPlaylist(ByVal _mediaOrderID As Integer, ByVal _mediaName As String, ByVal _mediaPath As String)
        DataHandler.AddDataToTag(_mediaOrderID) = _mediaName
        DataHandler.AddDataToTag(_mediaOrderID) = _mediaPath
    End Sub
    Public Overrides Sub SaveFile()
        Dim towrite As New List(Of String)
        towrite.Add("#EXTM3U")
        Dim i As Integer = 1
        Do Until i > DataHandler.NumberOfTags
            Dim tagsdata() As String = DataHandler.ReadTagsData(i, -1)
            towrite.Add("#EXTINF:0," & tagsdata(0))
            towrite.Add(tagsdata(1))
            towrite.Add("")
            i += 1
        Loop
        Dim sw As New IO.StreamWriter(Path)
        For Each line In towrite
            sw.WriteLine(line)
        Next
        sw.Close()
    End Sub
End Class
