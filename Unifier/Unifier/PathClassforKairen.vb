Public Class PathClassforKairen
    Public ReadOnly Root As FolderClass
    Public Accounts As FolderClass
    Public CurrentAccount As FolderClass_CurrentAccount
    Public CurrentWorld As FolderClass_CurrentWorld
    Sub New(ByVal _rootFolder As String)
        Root = New FolderClass(_rootFolder)
        Accounts = New FolderClass(Root.Location & "Accounts\")
    End Sub
End Class
