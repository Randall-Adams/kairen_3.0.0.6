Public Class FolderClass_CurrentWorld
    Inherits FolderClass
    Public Abilities As FileClass
    Public NPCs As FileClass
    Sub New(ByVal _location As String)
        MyBase.New(_location)
        Abilities = New FileClass(MyBase.Location & "Abilities\")
        NPCs = New FileClass(MyBase.Location & "NPCs\")
    End Sub
End Class
