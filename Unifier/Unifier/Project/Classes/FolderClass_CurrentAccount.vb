Public Class FolderClass_CurrentAccount
    Inherits FolderClass
    Public Worlds As FolderClass
    Public Abilities As FolderClass
    Public Cheat_Tables As FolderClass
    Public LUAs As FolderClass
    Public Images As FolderClass
    Public Game_Data As FolderClass
    Public Custom_Data As FolderClass
    Sub New(ByVal _location As String)
        MyBase.New(_location)
        Worlds = New FolderClass(MyBase.Location & "Worlds\")
        Abilities = New FolderClass(MyBase.Location & "Abilities\")
        Cheat_Tables = New FolderClass(MyBase.Location & "Cheat Tables\")
        LUAs = New FolderClass(MyBase.Location & "LUAs\")
        Images = New FolderClass(MyBase.Location & "Images\")
        Game_Data = New FolderClass(MyBase.Location & "Game Data\")
        Custom_Data = New FolderClass(MyBase.Location & "Custom Data\")
    End Sub
End Class
