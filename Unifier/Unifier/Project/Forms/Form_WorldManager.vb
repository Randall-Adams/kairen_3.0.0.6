Public Class Form_WorldManager
    Public CL As CommonLibrary
    Public SL As SpecificLibrary
    Sub New(ByRef _commonLibrary As CommonLibrary, ByRef _specificLibrary As SpecificLibrary)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CL = _commonLibrary
        SL = _specificLibrary
        CreateNewWorld()
    End Sub
    Sub New(ByRef _commonLibrary As CommonLibrary, ByRef _specificLibrary As SpecificLibrary, ByVal _worldToLoad As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CL = _commonLibrary
        SL = _specificLibrary
        'load an existing world
        LoadWorld(_worldToLoad)
    End Sub
    Private Sub Form_WorldManager_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If SL.AccountFile IsNot Nothing Then SL.AccountFile.Lock = Me
        Me.Location = New Point(250, 150)
        LoadAbilityFilesIntoListBox()
    End Sub
    Private Sub CreateNewWorld()
        Dim NewWorldName As String = ""
        Do Until NewWorldName.Trim <> ""
            NewWorldName = InputBox("Enter Your New World's Name", "New World Creation")
            If NewWorldName.Trim = "" Then
                If MsgBox("No Name Supplied. Do you want to cancel Creating a New World?", MsgBoxStyle.YesNo, "New World Creation") = MsgBoxResult.Yes Then
                    Me.Close()
                    Exit Sub
                End If
            End If
        Loop
        TextBox1.Text = NewWorldName
        SL.Path.CurrentWorld = New FolderClass_CurrentWorld(SL.Path.CurrentAccount.Worlds.Location & NewWorldName & "\")
        SL.WorldFile = New FileClass_WorldFile(SL.Path.CurrentWorld.Location, SL.Path.CurrentWorld.NameWithoutPath)
        Me.Show()
        If SL.Path.CurrentAccount.Abilities.Exists Then
            ListBox1.Items.Add("- Empty -")
        Else
            ListBox1.Items.Add("- No Abilities Present -")
        End If
        SL.WorldFile.CreateWorld()
    End Sub
    Private Sub LoadWorld(ByVal _worldToLoad As String)
        If My.Computer.FileSystem.DirectoryExists(SL.Path.CurrentAccount.Worlds.Location & _worldToLoad & "\") Then
            Me.Text = "World Manager - " & _worldToLoad
            TextBox1.Text = _worldToLoad
            SL.Path.CurrentWorld = New FolderClass_CurrentWorld(SL.Path.CurrentAccount.Worlds.Location & _worldToLoad & "\")
            SL.WorldFile = New FileClass_WorldFile(SL.Path.CurrentWorld.Location, SL.Path.CurrentWorld.NameWithoutPath)
            Me.Show()
            If SL.Path.CurrentAccount.Abilities.Exists Then
                'load abilites herr
                For Each item In SL.WorldFile.GetTagsData("Abilities")
                    ListBox1.Items.Add(item)
                Next
            Else
                ListBox1.Items.Add("- No Abilities Present -")
            End If
        Else
            Me.Close()
            Exit Sub
        End If
    End Sub
    Private Sub LoadAbilityFilesIntoListBox()
        ListBox1.Items.Clear()
        If SL.AccountFile IsNot Nothing Then
            'SL.Path.CurrentWorld.Abilities.Location
            'If CL.ReturnFilesFromFolder(SL.Path.CurrentWorld.Abilities.Location, ".txt") Is Nothing Then Exit Sub
            'For Each ability In CL.ReturnFilesFromFolder(SL.Path.CurrentWorld.Abilities.Location, ".txt")
            '    ListBox1.Items.Add(ability)
            'Next
        End If
    End Sub
    Private Sub LoadNPCFilesIntoListBox()
        ListBox2.Items.Clear()
        If SL.AccountFile IsNot Nothing Then
            'SL.Path.CurrentWorld.NPCs.Location
            'If CL.ReturnFilesFromFolder(SL.Path.CurrentWorld.NPCs.Location, ".txt") Is Nothing Then Exit Sub
            'For Each ability In CL.ReturnFilesFromFolder(SL.Path.CurrentWorld.Abilities.Location, ".txt")
            '    ListBox2.Items.Add(ability)
            'Next
        End If
    End Sub

    Private Sub Form_WorldManager_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If SL.AccountFile IsNot Nothing Then SL.AccountFile.Unlock = Me
    End Sub
End Class