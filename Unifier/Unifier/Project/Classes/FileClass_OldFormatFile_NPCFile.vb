Public Class FileClass_OldFormatFile_NPCFile
    Public FileFormatVersion As String
    Public SafeName As String
    Public GameName As String
    Public X As String
    Public Y As String
    Public Z As String
    Public F As String
    Public Distance_XZ As Integer
    Public Distance_XYZ As Integer
    Sub New(ByVal _fileLocation As String, ByVal _fileName As String, Optional ByVal _fileExtension As String = Nothing)
        Dim sr As New IO.StreamReader(_fileLocation & _fileName & _fileExtension)
        FileFormatVersion = sr.ReadLine
        SafeName = sr.ReadLine
        GameName = sr.ReadLine
        X = sr.ReadLine
        Y = sr.ReadLine
        Z = sr.ReadLine
        F = sr.ReadLine
        sr.Close()
        sr.Dispose()
    End Sub
    Sub New(ByVal _XDistanceCompare As Integer, ByVal _YDistanceCompare As Integer, ByVal _fileLocation As String, ByVal _fileName As String, Optional ByVal _fileExtension As String = Nothing)
        Dim sr As New IO.StreamReader(_fileLocation & _fileName & _fileExtension)
        FileFormatVersion = sr.ReadLine
        SafeName = sr.ReadLine
        GameName = sr.ReadLine
        X = sr.ReadLine
        Y = sr.ReadLine
        Z = sr.ReadLine
        F = sr.ReadLine
        sr.Close()
        sr.Dispose()
        Distance_XZ = Math.Sqrt((_XDistanceCompare - X) ^ 2 + (_YDistanceCompare - Y) ^ 2)
    End Sub
    Sub New(ByVal _XDistanceCompare As Integer, ByVal _YDistanceCompare As Integer, ByVal _ZDistanceCompare As Integer, ByVal _fileLocation As String, ByVal _fileName As String, Optional ByVal _fileExtension As String = Nothing)
        Dim sr As New IO.StreamReader(_fileLocation & _fileName & _fileExtension)
        FileFormatVersion = sr.ReadLine
        SafeName = sr.ReadLine
        GameName = sr.ReadLine
        X = sr.ReadLine
        Y = sr.ReadLine
        Z = sr.ReadLine
        F = sr.ReadLine
        sr.Close()
        sr.Dispose()
    End Sub

End Class
