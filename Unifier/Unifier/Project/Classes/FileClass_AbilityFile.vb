Public Class FileClass_AbilityFile
    Inherits FileClass_Base
    Sub New(ByVal _fileLocation As String, ByVal _fileName As String, Optional ByVal _fileExtension As String = ".txt")
        MyBase.New(_fileLocation, _fileName, _fileExtension)
    End Sub

End Class
