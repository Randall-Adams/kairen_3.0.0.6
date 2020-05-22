Public Class Form_AbilityMaker
#Region "Global Stuff"
    Public CL As CommonLibrary
    Public SL As SpecificLibrary
    Public WithEvents AbilityFile As FileClass_AbilityFile
#End Region
#Region "This Form's Main Process Code"
    Public Sub New(ByRef _commonLibrary As CommonLibrary, ByRef _specificLibrary As SpecificLibrary)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CL = _commonLibrary
        SL = _specificLibrary
    End Sub

    Private Sub Form_AbilityMaker_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(250, 150)

        Dim RaceList(9) As String
        RaceList(0) = "Dark Elf"
        RaceList(1) = "Troll"
        RaceList(2) = "Ogre"
        RaceList(3) = "Elf"
        RaceList(4) = "Dwarf"
        RaceList(5) = "Gnome"
        RaceList(6) = "Halfling"
        RaceList(7) = "Erudite"
        RaceList(8) = "Human"
        RaceList(9) = "Barbarian"
        For Each line In RaceList
            CheckedListBox1.Items.Add(line, False)
        Next
        Dim Classlist(14) As String
        Classlist(0) = "Warrior"
        Classlist(1) = "Paladin"
        Classlist(2) = "Shadowknight"
        Classlist(3) = "Enchanter"
        Classlist(4) = "Magician"
        Classlist(5) = "Wizard"
        Classlist(6) = "Alchemist"
        Classlist(7) = "Necromancer"
        Classlist(8) = "Monk"
        Classlist(9) = "Rogue"
        Classlist(10) = "Ranger"
        Classlist(11) = "Bard"
        Classlist(12) = "Druid"
        Classlist(13) = "Shaman"
        Classlist(14) = "Cleric"
        For Each line In Classlist
            CheckedListBox2.Items.Add(line, False)
        Next

        Dim EquipRequirementList(10) As String
        EquipRequirementList(0) = "1HS"
        EquipRequirementList(1) = "2HS"
        EquipRequirementList(2) = "1HB"
        EquipRequirementList(3) = "2HB"
        EquipRequirementList(4) = "1HP"
        EquipRequirementList(5) = "2HP"
        EquipRequirementList(6) = "1 HCROSSBOW"
        EquipRequirementList(7) = "2 HCROSSBOW"
        EquipRequirementList(8) = "BOW"
        EquipRequirementList(9) = "THROWN"
        EquipRequirementList(10) = "SHIELD"
        For Each line In EquipRequirementList
            CheckedListBox4.Items.Add(line, False)
        Next

        ListBox1.Items.Add("Livenilla")
        ListBox1.SelectedIndex = 0
        ListBox2.Items.Add("Player Spell")
        ListBox2.SelectedIndex = 0


        ComboBox2.Items.Add("- None -")
        ComboBox3.Items.Add("- None -")
        ComboBox4.Items.Add("- None -")
        ComboBox4.Items.Add("Has Animation - Not Set")
        CheckedListBox3.Items.Add("- None -")
        CheckedListBox3.Items.Add("Has Effect - Not Set")

        If SL.AccountFile IsNot Nothing Then
            SL.AccountFile.Lock = Me
            Me.Text = "Kairen - Ability Maker - " & SL.AccountFile.GetTagsData("Name", 0)
            LoadAbilityFilesIntoListBox()
            LoadWorldFilesIntoListBox()
            LoadWorldFilesIntoCheckedListBox()
            LoadIconForegroundFilesIntoComboBox()
            LoadIconBackgroundFilesIntoComboBox()
            Button2.Enabled = True
        Else
            Me.Text = "Kairen - Ability Maker"
            CheckBox3.Checked = True
            CheckBox3.Enabled = False
            Button2.Enabled = False
        End If
    End Sub
#End Region
    Sub LoadIconForegroundFilesIntoComboBox()
        Dim IconList() As String = CL.ReturnFilesFromFolder(SL.Path.CurrentAccount.Images.Location & "Spells\Foreground\")
        If IconList IsNot Nothing Then
            For Each Image In IconList
                If Microsoft.VisualBasic.Right(Image, 4).ToLower = ".jpg" Then
                    ComboBox2.Items.Add(Microsoft.VisualBasic.Left(Image, Image.Length - 4))
                End If
            Next
        End If
    End Sub
    Sub LoadIconBackgroundFilesIntoComboBox()
        Dim IconList() As String = CL.ReturnFilesFromFolder(SL.Path.CurrentAccount.Images.Location & "Spells\Background\")
        If IconList IsNot Nothing Then
            For Each Image In IconList
                If Microsoft.VisualBasic.Right(Image, 4).ToLower = ".jpg" Then
                    ComboBox3.Items.Add(Microsoft.VisualBasic.Left(Image, Image.Length - 4))
                End If
            Next
        End If
    End Sub
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
        FolderBrowserDialog1.ShowDialog()
        If My.Computer.FileSystem.DirectoryExists(FolderBrowserDialog1.SelectedPath) = False Then
            MsgBox("Error: Folder doesn't exist.", MsgBoxStyle.Exclamation, "Error")
            FolderBrowserDialog1.SelectedPath = Nothing
        Else
            TextBox3.Text = FolderBrowserDialog1.SelectedPath & "\"
        End If
        If My.Computer.FileSystem.FileExists(TextBox3.Text & TextBox9.Text) Then
            If MsgBox("File already exists. Do you want to overwrite it?", MsgBoxStyle.YesNo, "Warning: You are about to overwrite a file") = MsgBoxResult.No Then Exit Sub
        End If
        AbilityFile = New FileClass_AbilityFile(TextBox3.Text, TextBox9.Text)
        AbilityFile.UpdateDataInTag("nameSafe") = TextBox9.Text
        AbilityFile.UpdateDataInTag("Name") = TextBox1.Text
        AbilityFile.UpdateDataInTag("Cast") = NumericUpDown1.Value
        AbilityFile.UpdateDataInTag("Recast") = NumericUpDown1.Value
        AbilityFile.UpdateDataInTag("Range") = NumericUpDown3.Value
        AbilityFile.UpdateDataInTag("AoE Range") = NumericUpDown6.Value
        AbilityFile.UpdateDataInTag("Power Cost") = NumericUpDown4.Value
        AbilityFile.UpdateDataInTag("Level") = NumericUpDown5.Value
        AbilityFile.UpdateDataInTag("Scope") = ComboBox5.Text
        AbilityFile.UpdateDataInTag("Requires Line of Sight") = CheckBox1.Checked
        AbilityFile.UpdateDataInTag("Description") = TextBox2.Text
        'ItemFile.UpdateDataInTag("Dark Elf") = 
        'ItemFile.UpdateDataInTag("Troll") = 
        'ItemFile.UpdateDataInTag("Ogre") = 
        'ItemFile.UpdateDataInTag("Elf") = 
        'ItemFile.UpdateDataInTag("Dwarf") = 
        'ItemFile.UpdateDataInTag("Gnome") = 
        'ItemFile.UpdateDataInTag("Halfling") = 
        'ItemFile.UpdateDataInTag("Erudite") = 
        'ItemFile.UpdateDataInTag("Human") = 
        'ItemFile.UpdateDataInTag("Barbarian") = 
        'ItemFile.UpdateDataInTag("Warrior") = 
        'ItemFile.UpdateDataInTag("Paladin") = 
        'ItemFile.UpdateDataInTag("Shadowknight") = 
        'ItemFile.UpdateDataInTag("Enchanter") = 
        'ItemFile.UpdateDataInTag("Magician") = 
        'ItemFile.UpdateDataInTag("Wizard") = 
        'ItemFile.UpdateDataInTag("Alchemist") = 
        'ItemFile.UpdateDataInTag("Necromancer") = 
        'ItemFile.UpdateDataInTag("Monk") = 
        'ItemFile.UpdateDataInTag("Rogue") = 
        'ItemFile.UpdateDataInTag("Ranger") = 
        'ItemFile.UpdateDataInTag("Bard") = 
        'ItemFile.UpdateDataInTag("Druid") = 
        'ItemFile.UpdateDataInTag("Shaman") = 
        'ItemFile.UpdateDataInTag("Cleric") = 
        AbilityFile.SaveFile()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs)
        OpenFileDialog1.Reset()
        OpenFileDialog1.Title = "Select File to Load"
        OpenFileDialog1.ShowDialog()
        If OpenFileDialog1.SafeFileName = "" Then Exit Sub 'cancelled file select
        If My.Computer.FileSystem.FileExists(OpenFileDialog1.FileName) = False Then
            MsgBox("Error: File doesn't exist for loading.", MsgBoxStyle.Exclamation, "Error: No File to Load")
        Else
            Dim filepath As String = Microsoft.VisualBasic.Left(OpenFileDialog1.FileName, OpenFileDialog1.FileName.LastIndexOf("\") + 1)
            Dim filename As String = Microsoft.VisualBasic.Right(OpenFileDialog1.FileName, OpenFileDialog1.FileName.Length - OpenFileDialog1.FileName.LastIndexOf("\") - 1)
            If Microsoft.VisualBasic.Right(filename, 4) = ".txt" Then filename = Microsoft.VisualBasic.Left(filename, filename.Length - 4)
            AbilityFile = New FileClass_AbilityFile(filepath, filename)
            AbilityFile.LoadFile()
        End If
    End Sub
    Private Sub AbilityFile_Loaded() Handles AbilityFile.FileLoadComplete
        UncheckAllItemsInCheckedListBox(CheckedListBox1)
        UncheckAllItemsInCheckedListBox(CheckedListBox2)
        UncheckAllItemsInCheckedListBox(CheckedListBox4)
        UncheckAllItemsInCheckedListBox(CheckedListBox3)

        TextBox3.Text = AbilityFile.Location
        TextBox9.Text = AbilityFile.GetTagsData("nameSafe", 1)
        TextBox1.Text = AbilityFile.GetTagsData("Name", 1)
        NumericUpDown1.Value = AbilityFile.GetTagsData("Cast", 1)
        NumericUpDown1.Value = AbilityFile.GetTagsData("Recast", 1)
        NumericUpDown3.Value = AbilityFile.GetTagsData("Range", 1)
        NumericUpDown6.Value = AbilityFile.GetTagsData("AoE Range", 1)
        NumericUpDown4.Value = AbilityFile.GetTagsData("Power Cost", 1)
        NumericUpDown5.Value = AbilityFile.GetTagsData("Level", 1)
        ComboBox5.Text = AbilityFile.GetTagsData("Scope", 1)
        CheckBox1.Checked = AbilityFile.GetTagsData("Requires Line of Sight", 1)
        TextBox2.Text = AbilityFile.GetTagsData("Description", 1)

        CheckItemsInCheckedListBox(AbilityFile.GetTagsData("Race"), CheckedListBox1)
        CheckItemsInCheckedListBox(AbilityFile.GetTagsData("Class"), CheckedListBox2)
        CheckItemsInCheckedListBox(AbilityFile.GetTagsData("Equip Requirement"), CheckedListBox4)
        CheckItemsInCheckedListBox(AbilityFile.GetTagsData("Effect"), CheckedListBox3)

        ComboBox2.SelectedIndex = -1
        ComboBox3.SelectedIndex = -1
        ComboBox4.SelectedIndex = -1
        PictureBox1.Visible = False
        PictureBox2.Visible = False
        ComboBox2.SelectedItem = AbilityFile.GetTagsData("Foreground Icon Name", 1)
        ComboBox3.SelectedItem = AbilityFile.GetTagsData("Background Icon Name", 1)
        ComboBox4.SelectedItem = AbilityFile.GetTagsData("Animation", 1)

    End Sub
    Private Sub CheckItemInCheckedListBox(ByVal _itemToCheck As String, ByRef _checkedListBoxControl As CheckedListBox)
        Dim i As Integer = 0
        i = 0
        Do Until i >= _checkedListBoxControl.Items.Count
            If _checkedListBoxControl.Items.Item(i) = _itemToCheck Then
                _checkedListBoxControl.SetItemCheckState(i, CheckState.Checked)
                Exit Do
            End If
            i += 1
        Loop
    End Sub
    Private Sub CheckItemsInCheckedListBox(ByVal _itemsToCheck() As String, ByRef _checkedListBoxControl As CheckedListBox)
        Dim i As Integer = 0
        For Each item In _itemsToCheck
            i = 0
            Do Until i >= _checkedListBoxControl.Items.Count
                If _checkedListBoxControl.Items.Item(i) = item Then
                    _checkedListBoxControl.SetItemCheckState(i, CheckState.Checked)
                    Exit Do
                End If
                i += 1
            Loop
        Next
    End Sub
    Private Sub UncheckAllItemsInCheckedListBox(ByRef _checkedListBoxControl As CheckedListBox)
        For i As Integer = 0 To _checkedListBoxControl.Items.Count - 1
            _checkedListBoxControl.SetItemCheckState(i, CheckState.Unchecked)
        Next
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        If TextBox1.Text.Trim = "" Then Exit Sub
        If CheckBox3.Checked Then
            FolderBrowserDialog1.ShowDialog()
            FolderBrowserDialog1.SelectedPath = FolderBrowserDialog1.SelectedPath & "\"
            If My.Computer.FileSystem.DirectoryExists(FolderBrowserDialog1.SelectedPath) = False Then
                MsgBox("Error: Folder doesn't exist.", MsgBoxStyle.Exclamation, "Error")
                FolderBrowserDialog1.SelectedPath = Nothing
            Else
                TextBox3.Text = FolderBrowserDialog1.SelectedPath & "\"
            End If
            If My.Computer.FileSystem.FileExists(TextBox3.Text & TextBox9.Text & ".txt") Then
                If MsgBox("File already exists. Do you want to overwrite it?", MsgBoxStyle.YesNo, "Warning: You are about to overwrite a file") = MsgBoxResult.No Then Exit Sub
            End If
            SaveAbilityFile(FolderBrowserDialog1.SelectedPath, TextBox9.Text)
        Else
            If My.Computer.FileSystem.FileExists(SL.Path.CurrentAccount.Abilities.Location & TextBox1.Text & ".txt") Then
                If MsgBox("Ability already exists. Do you want to replace it with this one?", MsgBoxStyle.YesNo, "Overwrite Existing File?") = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
            If ListBox4.SelectedIndex > 0 Then
                Dim worldFile As New FileClass_WorldFile(SL.Path.CurrentAccount.Worlds.Location & ListBox4.SelectedItem & "\", ListBox4.SelectedItem)
                Dim worldfileabilities As New List(Of String)
                For Each ability In worldFile.GetTagsData("Abilities")
                    worldfileabilities.Add(ability)
                Next
                If worldfileabilities.Contains(TextBox1.Text) = False Then
                    worldFile.AddDataToTag("Abilities") = TextBox1.Text
                    worldFile.SaveFile()
                End If
            End If
            SaveAbilityFile(SL.Path.CurrentAccount.Abilities.Location, TextBox1.Text)
            LoadWorldFilesIntoCheckedListBox()
        End If
    End Sub
    Private Sub SaveAbilityFile(ByVal _fileLocation As String, ByVal _fileName As String)
        AbilityFile = New FileClass_AbilityFile(_fileLocation, _fileName)
        AbilityFile.UpdateDataInTag("nameSafe") = TextBox9.Text
        AbilityFile.UpdateDataInTag("Name") = TextBox1.Text
        AbilityFile.UpdateDataInTag("Cast") = NumericUpDown1.Value
        AbilityFile.UpdateDataInTag("Recast") = NumericUpDown1.Value
        AbilityFile.UpdateDataInTag("Range") = NumericUpDown3.Value
        AbilityFile.UpdateDataInTag("AoE Range") = NumericUpDown6.Value
        AbilityFile.UpdateDataInTag("Power Cost") = NumericUpDown4.Value
        AbilityFile.UpdateDataInTag("Level") = NumericUpDown5.Value
        AbilityFile.UpdateDataInTag("Scope") = ComboBox5.Text
        AbilityFile.UpdateDataInTag("Requires Line of Sight") = CheckBox1.Checked
        AbilityFile.UpdateDataInTag("Description") = TextBox2.Text
        AbilityFile.ClearDataInTag("Race")
        For Each _race In CheckedListBox1.CheckedItems
            AbilityFile.AddDataToTag("Race") = _race
        Next
        AbilityFile.ClearDataInTag("Class")
        For Each _class In CheckedListBox2.CheckedItems
            AbilityFile.AddDataToTag("Class") = _class
        Next
        AbilityFile.ClearDataInTag("Equip Requirement")
        For Each _eqReq In CheckedListBox4.CheckedItems
            AbilityFile.AddDataToTag("Equip Requirement") = _eqReq
        Next
        AbilityFile.ClearDataInTag("Effect")
        For Each _effect In CheckedListBox3.CheckedItems
            AbilityFile.AddDataToTag("Effect") = _effect
        Next
        'ItemFile.UpdateDataInTag("Dark Elf") = 
        'ItemFile.UpdateDataInTag("Troll") = 
        'ItemFile.UpdateDataInTag("Ogre") = 
        'ItemFile.UpdateDataInTag("Elf") = 
        'ItemFile.UpdateDataInTag("Dwarf") = 
        'ItemFile.UpdateDataInTag("Gnome") = 
        'ItemFile.UpdateDataInTag("Halfling") = 
        'ItemFile.UpdateDataInTag("Erudite") = 
        'ItemFile.UpdateDataInTag("Human") = 
        'ItemFile.UpdateDataInTag("Barbarian") = 
        'ItemFile.UpdateDataInTag("Warrior") = 
        'ItemFile.UpdateDataInTag("Paladin") = 
        'ItemFile.UpdateDataInTag("Shadowknight") = 
        'ItemFile.UpdateDataInTag("Enchanter") = 
        'ItemFile.UpdateDataInTag("Magician") = 
        'ItemFile.UpdateDataInTag("Wizard") = 
        'ItemFile.UpdateDataInTag("Alchemist") = 
        'ItemFile.UpdateDataInTag("Necromancer") = 
        'ItemFile.UpdateDataInTag("Monk") = 
        'ItemFile.UpdateDataInTag("Rogue") = 
        'ItemFile.UpdateDataInTag("Ranger") = 
        'ItemFile.UpdateDataInTag("Bard") = 
        'ItemFile.UpdateDataInTag("Druid") = 
        'ItemFile.UpdateDataInTag("Shaman") = 
        'ItemFile.UpdateDataInTag("Cleric") = 
        AbilityFile.UpdateDataInTag("Foreground Icon Name") = ComboBox2.SelectedItem
        AbilityFile.UpdateDataInTag("Foreground Icon Endian") = "Little Endian" 'CheckBox2.Checked
        AbilityFile.UpdateDataInTag("Background Icon Name") = ComboBox3.SelectedItem
        AbilityFile.UpdateDataInTag("Background Icon Endian") = "Little Endian" 'CheckBox4.Checked
        AbilityFile.UpdateDataInTag("Animation") = ComboBox4.SelectedItem
        AbilityFile.SaveFile()
        LoadAbilityFilesIntoListBox()
    End Sub

    Private Sub LoadAbilityFilesIntoListBox()
        ListBox3.Items.Clear()
        If SL.AccountFile IsNot Nothing Then
            If ListBox4.SelectedIndex > 0 Then
                If ListBox4.SelectedIndex < 1 Then Exit Sub
                Dim worldFile As New FileClass_WorldFile(SL.Path.CurrentAccount.Worlds.Location & ListBox4.SelectedItem & "\", ListBox4.SelectedItem)
                For Each ability In worldFile.GetTagsData("Abilities")
                    ListBox3.Items.Add(ability)
                Next
            Else
                If CL.ReturnFilesFromFolder(SL.Path.CurrentAccount.Abilities.Location, ".txt") Is Nothing Then Exit Sub
                For Each ability In CL.ReturnFilesFromFolder(SL.Path.CurrentAccount.Abilities.Location, ".txt")
                    ListBox3.Items.Add(ability)
                Next
            End If
        End If
    End Sub
    Private Sub LoadWorldFilesIntoListBox()
        If SL.AccountFile IsNot Nothing Then
            ListBox4.Items.Clear()
            ListBox4.Items.Add("- Show All -")
            ListBox4.SelectedIndex = 0
            If CL.ReadFoldersToArray(SL.Path.CurrentAccount.Worlds.Location) Is Nothing Then Exit Sub
            For Each world In CL.ReadFoldersToArray(SL.Path.CurrentAccount.Worlds.Location)
                ListBox4.Items.Add(world)
            Next
        End If
    End Sub
    Private Sub LoadWorldFilesIntoCheckedListBox()
        If SL.AccountFile IsNot Nothing Then
            CheckedListBox5.Items.Clear()
            CheckedListBox5.SelectedIndex = -1
            If CL.ReadFoldersToArray(SL.Path.CurrentAccount.Worlds.Location) Is Nothing Then Exit Sub
            For Each world In CL.ReadFoldersToArray(SL.Path.CurrentAccount.Worlds.Location)
                CheckedListBox5.Items.Add(world)
            Next
        End If
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        If CheckBox3.Checked Then
            OpenFileDialog1.Reset()
            OpenFileDialog1.Title = "Select File to Load"
            OpenFileDialog1.ShowDialog()
            If OpenFileDialog1.SafeFileName = "" Then Exit Sub 'cancelled file select
            If My.Computer.FileSystem.FileExists(OpenFileDialog1.FileName) = False Then
                MsgBox("Error: File doesn't exist for loading.", MsgBoxStyle.Exclamation, "Error: No File to Load")
            Else
                Dim filepath As String = Microsoft.VisualBasic.Left(OpenFileDialog1.FileName, OpenFileDialog1.FileName.LastIndexOf("\") + 1)
                Dim filename As String = Microsoft.VisualBasic.Right(OpenFileDialog1.FileName, OpenFileDialog1.FileName.Length - OpenFileDialog1.FileName.LastIndexOf("\") - 1)
                If Microsoft.VisualBasic.Right(filename, 4) = ".txt" Then filename = Microsoft.VisualBasic.Left(filename, filename.Length - 4)
                AbilityFile = New FileClass_AbilityFile(filepath, filename)
                AbilityFile.LoadFile()
            End If
        Else
            AbilityFile = New FileClass_AbilityFile(SL.Path.CurrentAccount.Abilities.Location, ListBox3.SelectedItem)
            AbilityFile.LoadFile()
        End If
    End Sub

    Private Sub ListBox4_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListBox4.SelectedIndexChanged
        ListBox3.SelectedIndex = -1
        LoadAbilityFilesIntoListBox()
    End Sub

    Private Sub CheckedListBox5_ItemCheck(sender As System.Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles CheckedListBox5.ItemCheck
        If CheckedListBox5.SelectedIndex = -1 Then Exit Sub
        Dim worldFile As New FileClass_WorldFile(SL.Path.CurrentAccount.Worlds.Location & CheckedListBox5.SelectedItem & "\", CheckedListBox5.SelectedItem)
        If e.NewValue = CheckState.Checked Then
            worldFile.AddDataToTag("Abilities") = ListBox3.SelectedItem
        Else
            worldFile.RemoveDataFromTag("Abilities") = ListBox3.SelectedItem
        End If
        worldFile.SaveFile()
    End Sub

    Private Sub ListBox3_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListBox3.SelectedIndexChanged
        If ListBox3.SelectedIndex = -1 Then
            CheckedListBox5.Enabled = False
            LoadWorldFilesIntoCheckedListBox()
        Else
            CheckedListBox5.Enabled = True
            LoadWorldFilesIntoCheckedListBox()
            Dim tochecklist As New List(Of String)
            For Each worldPath In CL.ReadFoldersToArray(SL.Path.CurrentAccount.Worlds.Location)
                Dim worldFile As New FileClass_WorldFile(SL.Path.CurrentAccount.Worlds.Location & worldPath & "\", worldPath)
                worldFile.LoadFile()
                Dim abilities() As String = worldFile.GetTagsData("Abilities")
                If abilities.Contains(ListBox3.SelectedItem) Then
                    tochecklist.Add(worldPath)
                End If
            Next
            Dim listIndex(CheckedListBox5.Items.Count - 1) As String
            For i As Integer = 0 To CheckedListBox5.Items.Count - 1
                If tochecklist.Count = 0 Then Exit For
                If tochecklist.Contains(CheckedListBox5.Items.Item(i).ToString) Then
                    CheckedListBox5.SetItemCheckState(i, CheckState.Checked)
                End If
            Next
        End If
    End Sub

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If ListBox3.SelectedIndex = -1 Then Exit Sub
        My.Computer.FileSystem.DeleteFile(SL.Path.CurrentAccount.Abilities.Location & ListBox3.SelectedItem & ".txt")
        Dim tochecklist As New List(Of String)
        For Each worldPath In CL.ReadFoldersToArray(SL.Path.CurrentAccount.Worlds.Location)
            Dim worldFile As New FileClass_WorldFile(SL.Path.CurrentAccount.Worlds.Location & worldPath & "\", worldPath)
            worldFile.RemoveDataFromTag("Abilities") = ListBox3.SelectedItem
            worldFile.SaveFile()
        Next
        LoadAbilityFilesIntoListBox()
        LoadWorldFilesIntoCheckedListBox()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedIndex = 0 Then
            PictureBox1.Image = Nothing
        Else
            If My.Computer.FileSystem.FileExists(SL.Path.CurrentAccount.Images.Location & "Spells\Foreground\" & ComboBox2.SelectedItem & ".jpg") = False Then Exit Sub
            PictureBox1.ImageLocation = SL.Path.CurrentAccount.Images.Location & "Spells\Foreground\" & ComboBox2.SelectedItem & ".jpg"
            PictureBox1.Show()
        End If
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.SelectedIndex = 0 Then
            PictureBox2.Image = Nothing
        Else
            If My.Computer.FileSystem.FileExists(SL.Path.CurrentAccount.Images.Location & "Spells\Background\" & ComboBox3.SelectedItem & ".jpg") = False Then Exit Sub
            PictureBox2.ImageLocation = SL.Path.CurrentAccount.Images.Location & "Spells\Background\" & ComboBox3.SelectedItem & ".jpg"
            PictureBox2.Show()
        End If
    End Sub

    Private Sub Button2_Click_1(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        If My.Computer.FileSystem.DirectoryExists(SL.Path.CurrentAccount.Images.Location & "Spells\") Then
            If MsgBox("Ability icon files appear to exist. Delete and reinstall Anyway?", MsgBoxStyle.YesNo, "Overwrite Existing Folder?") = MsgBoxResult.No Then
                Exit Sub
            Else
                My.Computer.FileSystem.DeleteDirectory(SL.Path.CurrentAccount.Images.Location & "Spells\", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If
        End If
        IO.File.WriteAllBytes(SL.Path.CurrentAccount.Images.Location & "Spells.zip", My.Resources.Spells)
        IO.Compression.ZipFile.ExtractToDirectory(SL.Path.CurrentAccount.Images.Location & "Spells.zip", SL.Path.CurrentAccount.Images.Location)
        My.Computer.FileSystem.DeleteFile(SL.Path.CurrentAccount.Images.Location & "Spells.zip")
        LoadIconForegroundFilesIntoComboBox()
        LoadIconBackgroundFilesIntoComboBox()
    End Sub

    Private Sub Form_AbilityMaker_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If SL.AccountFile IsNot Nothing Then SL.AccountFile.Unlock = Me
    End Sub
End Class