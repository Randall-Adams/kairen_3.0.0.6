Public Class Form_Kairenv2
#Region "Global Stuff"
    Public CL As CommonLibrary
    Public SL As SpecificLibrary
    'Public Path As PathHandler
#End Region
#Region "This Form's Main Process Code"
    Public Sub New(ByRef _commonLibrary As CommonLibrary, ByRef _specificLibrary As SpecificLibrary)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CL = _commonLibrary
        SL = _specificLibrary
    End Sub
    Private Sub Form_Kairenv2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(250, 150)
        Me.Text = "Kairen v2 Home Window"
        'ListBox1.Items.Add("NPC Maker")
        'ListBox1.Items.Add("Item Maker")
        'ListBox1.Items.Add("Ability Maker")
        'ListBox1.Items.Add("Recipe Maker")
        ''ListBox1.Items.Add("Spellscroll Maker") - maybe subset of item maker
        'ListBox1.Items.Add("Quest Maker")
        'ListBox1.Items.Add("CM Maker")
        'ListBox1.Items.Add("Proc Maker")
        'ListBox1.Items.Add("Effect Maker")
        ''ListBox1.Items.Add("Recipescroll Maker")
        'ListBox1.Items.Add("Faction Maker")

        GroupBox1.Enabled = False 'Account Controls groupbox
        Button1.Enabled = False 'New Account button
        Button5.Enabled = False 'Login button
        Button6.Enabled = False 'Logout button
        ListBox1.Enabled = False 'Account List listbox
        CheckBox1.Enabled = False 'Default Login Account
        ListBox2.Enabled = False ' World List listbox
        'CheckedListBox1.Enabled = False ' Window List checkedlistbox
        CheckedListBox1.Items.Add("Ability Maker") ' Window List checkedlistbox
        CheckedListBox1.Items.Add("Item Maker") ' Window List checkedlistbox
        CheckedListBox1.Items.Add("NPC Window") ' Window List checkedlistbox
        Button2.Enabled = False ' Update button
        Label7.Text = SL.Kairen_Current_Version_FULL

        If SL.Path.Root.Exists = False Then
            Label11.Text = "Kairen Files Not Present" ' Install Status Label label
            Label6.Text = "-Accounts Cannot Be Present-" ' Account Status Label label
            Button1.Enabled = True 'New Account button
        Else
            Label11.Text = "Kairen Files Detected" ' Install Status Label label
            If SL.Path.Accounts.Exists Then
                LoadAccountNamesIntoList()
                If ListBox1.Items.Contains(SL.UniversalOptions.GetTagsData("Autoload Account", 0)) Then
                    LoadAccount(SL.UniversalOptions.GetTagsData("Autoload Account", 0))
                Else
                    Button5.Enabled = True 'Login button
                End If
                If SL.UniversalOptions.GetTagsData("Autostart", 0) = "true" Then CheckBox2.Checked = True
            Else
                Label6.Text = "No Accounts Detected" ' Account Status Label label
            End If
        End If

        ModeCheck()
        'Timer1.Start() ' Auto Checks Latest Version
        'CheckedListBox1.SetItemChecked(2, True)
        'Me.WindowState = FormWindowState.Minimized
    End Sub
    Sub ModeCheck()
        Select Case SL.Kairen_Current_Version_TYPE & SL.Kairen_Current_Version_TYPENUMBER
            Case "TAUIH"
            Case "TAUNI"
            Case "TAUSE"
                Button8.Enabled = False
            Case "RHOSE"
                Button8.Enabled = False
        End Select
    End Sub
#End Region
    Private Sub LoadAccountNamesIntoList()
        ListBox1.Items.Clear()
        If SL.Path.Root.Exists Then Label11.Text = "Kairen Files Detected" ' Install Status Label label
        Dim actar() As String = CL.ReadFoldersToArray(SL.Path.Accounts.Location)
        If actar.Length = 1 Then
            Label6.Text = actar.Length & " Account Detected" ' Account Status Label label
        Else
            Label6.Text = actar.Length & " Accounts Detected" ' Account Status Label label
        End If
        If actar.Length > 0 Then
            For Each account In actar
                ListBox1.Items.Add(account) 'Account List listbox
            Next
            ListBox1.Enabled = True 'Account List listbox
        End If
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        'this whole process needs multithreaded
        Label9.Text = "checking" ' Latest Version Label label
        Dim verres As String = SL.ReadLineFromFileOnline("http://kairen.returnhome.co/databits/kairenprogram/kairenlatestversion.txt")
        If verres = "error" Then
            Label9.Text = verres
            Exit Sub
        End If
        Label9.Text = verres ' Latest Version Label label
        Button3.Enabled = False ' Check Latest Version button
        Timer1.Stop()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click ' Check Latest Version button
        Timer1_Tick(sender, e)
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim resp As String = InputBox("New Account Name", "Create New Account")
        If resp = "" Then Exit Sub
        Dim thisisfirstaccount As Boolean = False
        If SL.Path.Root.Exists = False Then
            thisisfirstaccount = True
            SL.Path.Root.Create()
        End If
        If SL.Path.Accounts.Exists = False Then
            SL.Path.Accounts.Create()
        End If
        SL.Path.CurrentAccount = New FolderClass_CurrentAccount(SL.Path.Accounts.Location & resp & "\")
        If SL.Path.CurrentAccount.Exists Then
            MsgBox("Account Name Taken", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        SL.Path.CurrentAccount.Create()

        SL.Path.CurrentAccount.Abilities = New FolderClass(SL.Path.CurrentAccount.Location & "Abilities")
        SL.Path.CurrentAccount.Abilities.Create()

        SL.Path.CurrentAccount.Worlds = New FolderClass(SL.Path.CurrentAccount.Location & "Worlds")
        SL.Path.CurrentAccount.Worlds.Create()

        SL.Path.CurrentAccount.Images = New FolderClass(SL.Path.CurrentAccount.Location & "Images")
        SL.Path.CurrentAccount.Images.Create()



        SL.AccountFile = New FileClass_AccountFile(SL.Path.CurrentAccount.Location, resp)
        SL.AccountFile.UpdateDataInTag("Name") = resp
        SL.AccountFile.SaveFile()
        If thisisfirstaccount Then
            SL.UniversalOptions.UpdateDataInTag("Autoload Account") = resp
            SL.UniversalOptions.SaveFile()
        End If
        LoadAccountNamesIntoList()
        LoadAccount(resp)
    End Sub
    Private Sub LoadAccount(ByVal _accountToLoad As String)
        If My.Computer.FileSystem.DirectoryExists(SL.Path.Accounts.Location & _accountToLoad) = False Then Exit Sub
        SL.Path.CurrentAccount = New FolderClass_CurrentAccount(SL.Path.Accounts.Location & _accountToLoad & "\")
        SL.AccountFile = New FileClass_AccountFile(SL.Path.CurrentAccount.Location, _accountToLoad)
        GroupBox1.Enabled = True 'Account Controls groupbox
        TextBox1.Text = _accountToLoad ' Account Name textbox
        CheckBox1.Enabled = True 'Default Login Account
        If SL.UniversalOptions.GetTagsData("Autoload Account", 0) = TextBox1.Text Then
            CheckBox1.Checked = True 'Default Login Account
        Else
            CheckBox1.Checked = False 'Default Login Account
        End If
        If SL.UniversalOptions.GetTagsData("Autostart", 0) = "true" Then
            CheckBox2.Checked = True
        End If

        Button5.Enabled = False 'Login button
        Button6.Enabled = True 'Logout button
        ListBox1.SelectedItem = TextBox1.Text ' Account List listbox
        ListBox1.Enabled = False ' Account List listbox
        Button1.Enabled = False 'New Account button
        CheckedListBox1.Enabled = True ' Window List checkedlistbox
        ListBox2.Enabled = True ' World List listbox
        ListBox2.Items.Add("- New World -") ' World List listbox
        Dim worldlist() As String = CL.ReadFoldersToArray(SL.Path.CurrentAccount.Worlds.Location)
        If worldlist IsNot Nothing Then
            For Each world In worldlist
                ListBox2.Items.Add(world)
            Next
        End If
        Me.Text = "Kairen v2 Home Window - " & SL.AccountFile.GetTagsData("Name", 0)
        TextBox2.Text = SL.AccountFile.GetTagsData("Emulator Path", 0)
        TextBox3.Text = SL.AccountFile.GetTagsData("ISO Path", 0)
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("")
        For Each drive As System.IO.DriveInfo In My.Computer.FileSystem.Drives
            If drive.RootDirectory.FullName <> "A:\" And drive.RootDirectory.FullName <> "B:\" And drive.RootDirectory.FullName <> "C:\" Then
                ComboBox1.Items.Add(drive.RootDirectory.FullName)
            End If
        Next
        If ComboBox1.Items.Contains(SL.AccountFile.GetTagsData("RAM Disq Drive", 0)) Then
            ComboBox1.SelectedItem = SL.AccountFile.GetTagsData("RAM Disq Drive", 0)
        End If
    End Sub
    Private Sub UnloadAccount()
        GroupBox1.Enabled = False 'Account Controls groupbox
        TextBox1.Text = "" ' Account Name textbox
        TextBox2.Text = "" ' Emulator Path textbox
        TextBox3.Text = "" ' Disc Image Path textbox
        ComboBox1.SelectedIndex = -1
        Button5.Enabled = True 'Login button
        Button6.Enabled = False 'Logout button
        ListBox1.SelectedIndex = -1 ' Account List listbox
        ListBox1.Enabled = True ' Account List listbox
        CheckBox1.Checked = False 'Default Login Account
        Button1.Enabled = True 'New Account button
        'CheckedListBox1.Enabled = False ' Window List checkedlistbox
        ListBox2.Items.Clear() ' World List listbox
        ListBox2.Enabled = False ' World List listbox
        SL.AccountFile = Nothing
        SL.WorldFile = Nothing
        SL.Path.CurrentAccount = Nothing
        SL.Path.CurrentWorld = Nothing
        Me.Text = "Kairen v2 Home Window"
    End Sub
    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        If MsgBox("Delete For really the folder of kairen?", MsgBoxStyle.YesNo, "HEY BROBRO") = MsgBoxResult.Yes Then
            If MsgBox("Frill?", MsgBoxStyle.YesNo, "HEYHEY BROBROBROBROBRO CLICK NO TO KILL IT YO") = MsgBoxResult.No Then
                SL.Path.Root.Delete()
            End If
        End If
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        If SL.AccountFile.IsLocked Then
            MsgBox("You must close all windows using your account before logging out of it.")
        Else
            UnloadAccount()
        End If
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        If ListBox1.SelectedItem = Nothing Then Exit Sub
        LoadAccount(ListBox1.SelectedItem)
    End Sub

    Private Sub CheckBox1_Click(sender As System.Object, e As System.EventArgs) Handles CheckBox1.Click
        If CheckBox1.Checked = False Then
            CheckBox1.Checked = True
            SL.UniversalOptions.UpdateDataInTag("Autoload Account") = SL.AccountFile.GetTagsData("Name")
        Else
            CheckBox1.Checked = False
            SL.UniversalOptions.UpdateDataInTag("Autoload Account") = "none"
        End If
        SL.UniversalOptions.SaveFile()
    End Sub

    Private Sub CheckBox2_Click(sender As System.Object, e As System.EventArgs) Handles CheckBox2.Click
        If CheckBox2.Checked = False Then
            CheckBox2.Checked = True
            SL.UniversalOptions.UpdateDataInTag("Autostart") = "true"
        Else
            CheckBox2.Checked = False
            SL.UniversalOptions.UpdateDataInTag("Autostart") = "false"
        End If
        SL.UniversalOptions.SaveFile()
    End Sub

    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
        'world name
        'that's all?
        'select which files the world should reference to use
        If Application.OpenForms().OfType(Of Form_WorldManager).Any Then Exit Sub
        Dim ObjectForm_WorldManager As Form_WorldManager
        If ListBox2.SelectedIndex <> -1 Then
            If ListBox2.SelectedIndex = 0 Then
                ObjectForm_WorldManager = New Form_WorldManager(CL, SL)
            Else
                ObjectForm_WorldManager = New Form_WorldManager(CL, SL, ListBox2.SelectedItem)
            End If
        End If

    End Sub

    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click
        Dim OnlineControlPanelObject As Form_OnlineControlPanel
        OnlineControlPanelObject = New Form_OnlineControlPanel(CL, SL)
    End Sub
    Dim OpenFileDialog1 As New OpenFileDialog
    Private Sub Button9_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click
        Dim thisTextBox = TextBox2
        OpenFileDialog1.Filter = "Programs|*.exe|Any Type|*.*"
        OpenFileDialog1.DefaultExt = ".exe"
        OpenFileDialog1.Title = "Select your Playstation 2 Emulator"
        If thisTextBox.Text <> "" Then
            OpenFileDialog1.InitialDirectory = Microsoft.VisualBasic.Left(thisTextBox.Text, thisTextBox.Text.LastIndexOf("\"))
            OpenFileDialog1.FileName = Microsoft.VisualBasic.Right(thisTextBox.Text, thisTextBox.Text.Length - thisTextBox.Text.LastIndexOf("\") - 1)
        Else
            OpenFileDialog1.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            OpenFileDialog1.FileName = ""
        End If
        If OpenFileDialog1.ShowDialog() = 1 Then
            thisTextBox.Text = OpenFileDialog1.FileName
        Else
            'they didn't select a correcto
        End If
    End Sub

    Private Sub Button10_Click(sender As System.Object, e As System.EventArgs) Handles Button10.Click
        Dim thisTextBox = TextBox3
        OpenFileDialog1.Filter = "ISO|*.iso|Any Type|*.*"
        OpenFileDialog1.DefaultExt = ".iso"
        OpenFileDialog1.Title = "Select your EQOA Disc Image"
        If thisTextBox.Text <> "" Then
            OpenFileDialog1.InitialDirectory = Microsoft.VisualBasic.Left(thisTextBox.Text, thisTextBox.Text.LastIndexOf("\"))
            OpenFileDialog1.FileName = Microsoft.VisualBasic.Right(thisTextBox.Text, thisTextBox.Text.Length - thisTextBox.Text.LastIndexOf("\") - 1)
        Else
            OpenFileDialog1.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            OpenFileDialog1.FileName = ""
        End If
        If OpenFileDialog1.ShowDialog() = 1 Then
            thisTextBox.Text = OpenFileDialog1.FileName
        Else
            'they didn't select a correcto
        End If
    End Sub

    Private Sub Button14_Click(sender As System.Object, e As System.EventArgs) Handles Button14.Click
        'if pcsx2 isn't present then exit
        If My.Computer.FileSystem.FileExists(SL.AccountFile.GetTagsData("Emulator Path", 0)) = False Then
            MsgBox("Warning! Your Emulator seems to be missing, cancelling Auto-Attach Setup.", MsgBoxStyle.Exclamation, "Auto-Attach Setup Error")
            Exit Sub
        End If
        'HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CheatEngine\DefaultIcon ' has my machines cheat engine location
        'HKEY_CURRENT_USER\SOFTWARE\CheatEngine\DefaultIcon ' has my machines cheat engine location
        'C:\Program Files (x86)\Cheat Engine 6.4\Cheat Engine.exe,0
        If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Cheat Engine", "AutoAttach", Nothing) IsNot Nothing Then
            Dim reg_value_cheatengine_AutoLaunch As String = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Cheat Engine", "AutoAttach", Nothing).ToString
            Dim EmulatorFileName As String = Microsoft.VisualBasic.Right(SL.AccountFile.GetTagsData("Emulator Path", 0), SL.AccountFile.GetTagsData("Emulator Path", 0).Length() - SL.AccountFile.GetTagsData("Emulator Path", 0).LastIndexOf("\") - 1)
            ' MsgBox(EmulatorFileName)
            ' MsgBox(reg_value_cheatengine_AutoLaunch.Contains(EmulatorFileName))
            If reg_value_cheatengine_AutoLaunch.Contains(EmulatorFileName) = False Then
                'MsgBox("installing auto attach")
                Dim lastindexof As String = reg_value_cheatengine_AutoLaunch.LastIndexOf(";")
                Dim length As String = (reg_value_cheatengine_AutoLaunch.Length - 1)
                If length = 0 Or lastindexof <> length Then
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\SOFTWARE\Cheat Engine", "AutoAttach", reg_value_cheatengine_AutoLaunch & ";" & EmulatorFileName, Microsoft.Win32.RegistryValueKind.String)
                Else
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\SOFTWARE\Cheat Engine", "AutoAttach", reg_value_cheatengine_AutoLaunch & EmulatorFileName, Microsoft.Win32.RegistryValueKind.String)
                End If
                MsgBox("Auto-Attach setup complete.", MsgBoxStyle.Information, "CE Auto-Attach Setup")
            Else
                MsgBox("Auto-Attach was already setup.", MsgBoxStyle.Information, "CE Auto-Attach Setup")
            End If
        Else
            'ce's autoattach registry key is missing
            Dim EmulatorFileName As String = Microsoft.VisualBasic.Right(SL.AccountFile.GetTagsData("Emulator Path", 0), SL.AccountFile.GetTagsData("Emulator Path", 0).Length() - SL.AccountFile.GetTagsData("Emulator Path", 0).LastIndexOf("\") - 1)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\SOFTWARE\Cheat Engine", "AutoAttach", EmulatorFileName, Microsoft.Win32.RegistryValueKind.String)
            MsgBox("Auto-Attach setup complete." & vbNewLine & "The Registry Key for Cheat Engine was created from scratch, so it was missing.", MsgBoxStyle.Information, "CE Auto-Attach Setup")
        End If
    End Sub

    Private Sub Button15_Click(sender As System.Object, e As System.EventArgs) Handles Button15.Click
        'HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CheatEngine\DefaultIcon ' has my machines cheat engine location
        'C:\Program Files (x86)\Cheat Engine 6.4\Cheat Engine.exe,0
        Dim reg_value_cheatengine As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CheatEngine\DefaultIcon", "", Nothing).ToString
        'MsgBox(reg_value_cheatengine)
        Dim cheatengine_rootfolder As String
        Dim cheatengine_autorunfolder As String
        cheatengine_rootfolder = Microsoft.VisualBasic.Left(reg_value_cheatengine, reg_value_cheatengine.LastIndexOf("Cheat Engine"))
        cheatengine_autorunfolder = cheatengine_rootfolder & "autorun\"
        If My.Computer.FileSystem.FileExists(cheatengine_autorunfolder & "Ihtol.lua") Then
            'plan a installation, in ce\autorun folder
            Dim continueResponse As MsgBoxResult
            continueResponse = MsgBox("The modification file is already present. Do you want to overwrite it?", MsgBoxStyle.YesNoCancel, "Modification File Already Present")
            If continueResponse = MsgBoxResult.Yes Then
                IO.File.WriteAllBytes(cheatengine_autorunfolder & "Ihtol.lua", My.Resources.Ihtol)
                If My.Computer.FileSystem.FileExists(cheatengine_autorunfolder) Then
                    MsgBox("Process Complete." & vbNewLine & "Modification file installed.", MsgBoxStyle.Information, "Modification Process")
                    Exit Sub
                End If
            ElseIf continueResponse = MsgBoxResult.No Then
                MsgBox("Process complete. No changes made.", MsgBoxStyle.Information, "Modification Process")
                Exit Sub
            ElseIf continueResponse = MsgBoxResult.Cancel Then
                MsgBox("No changes made. Cancelling...", MsgBoxStyle.Information, "Cancelling Installation..")
                Exit Sub
            End If
        Else
            IO.File.WriteAllBytes(cheatengine_autorunfolder & "Ihtol.lua", My.Resources.Ihtol)
            If My.Computer.FileSystem.FileExists(cheatengine_autorunfolder & "Ihtol.lua") Then
                MsgBox("Process Complete." & vbNewLine & "Modification file installed.", MsgBoxStyle.Information, "Modification Process")
                Exit Sub
            Else
                'process a did not work, let's try b, then make c and try that i guess lol
            End If
        End If
        'plan b installation, add directly into main.lua file
        If My.Computer.FileSystem.FileExists(cheatengine_rootfolder & "Ihtol.lua") Then
            Dim continueResponse As MsgBoxResult
            continueResponse = MsgBox("The modification file is already present. Do you want to overwrite it?", MsgBoxStyle.YesNoCancel, "Modification File Already Present")
            If continueResponse = MsgBoxResult.Yes Then
                IO.File.WriteAllBytes(cheatengine_rootfolder & "Ihtol.lua", My.Resources.Ihtol)
            ElseIf continueResponse = MsgBoxResult.No Then
                '
            ElseIf continueResponse = MsgBoxResult.Cancel Then
                MsgBox("No changes made. Cancelling...", MsgBoxStyle.OkOnly, "Cancelling Installation..")
                Exit Sub
            End If
        End If
        Dim ces_mainlua As List(Of String)
        Dim sr As New IO.StreamReader(cheatengine_rootfolder & "\main.lua")
        Do Until sr.EndOfStream
            ces_mainlua.Add(sr.ReadLine)
        Loop
        sr.Close()
        For Each _line In ces_mainlua
            If Microsoft.VisualBasic.Left(_line, 17) = "--Runnindatshityo" Then
                MsgBox("Process Complete." & vbNewLine & "Modification reference already detected, no change made.", MsgBoxStyle.Information, "Modification Process")
                Exit Sub
            End If
        Next
        If My.Computer.FileSystem.FileExists(cheatengine_rootfolder & "\main - This was your original copy.lua") = False Then
            My.Computer.FileSystem.CopyFile(cheatengine_rootfolder & "\main.lua", cheatengine_rootfolder & "\main - This was your original copy.lua", False)
        End If

        'ces_mainlua.CurrentIndex = ces_mainlua.Count
        ces_mainlua.Add("--Runnindatshityo " & SL.Kairen_Current_Version_BASE)
        ces_mainlua.Add("local f=io.open(" & Chr(34) & "Ihtol.lua" & Chr(34) & "," & Chr(34) & "r" & Chr(34) & ")")
        ces_mainlua.Add("if f~=nil then")
        ces_mainlua.Add("io.close(f)")
        ces_mainlua.Add("dofile(" & Chr(34) & "Ihtol.lua" & Chr(34) & ")")
        ces_mainlua.Add("else")
        ces_mainlua.Add("print(" & Chr(34) & "You may not find your way today after all.." & Chr(34) & ")")
        ces_mainlua.Add("end")
        Dim sw As New IO.StreamWriter(cheatengine_rootfolder & "\main.lua")
        For Each item In ces_mainlua
            sw.WriteLine(item)
        Next
        'Shell(http_path & " " & action_info)
        'add error check here '"C:\Program Files (x86)\Mozilla Firefox\firefox.exe" -osint -url "%1" 'is my machine's entry
        MsgBox("Modification installed." & vbNewLine & "Modification reference wrote.", MsgBoxStyle.Information, "Modification Process")
    End Sub

    Private Sub Button11_Click(sender As System.Object, e As System.EventArgs) Handles Button11.Click
        SL.AccountFile.UpdateDataInTag("Emulator Path") = TextBox2.Text
        SL.AccountFile.SaveFile()
    End Sub

    Private Sub Button12_Click(sender As System.Object, e As System.EventArgs) Handles Button12.Click
        SL.AccountFile.UpdateDataInTag("ISO Path") = TextBox3.Text
        SL.AccountFile.SaveFile()
    End Sub
    Public Sub LaunchCE(Optional ByVal _suffix As String = "", Optional ByVal _copyToAndRunFromFolder As String = "")
        If My.Computer.FileSystem.DirectoryExists(_copyToAndRunFromFolder) Then
            Try
                'Shell() works too
                If My.Computer.FileSystem.DirectoryExists(SL.Path.CurrentAccount.LUAs.Location) = True Then My.Computer.FileSystem.CopyDirectory(SL.Path.CurrentAccount.LUAs.Location, _copyToAndRunFromFolder & "LUAs\", True)
                If My.Computer.FileSystem.DirectoryExists(SL.Path.CurrentAccount.Cheat_Tables.Location) = True Then My.Computer.FileSystem.CopyDirectory(SL.Path.CurrentAccount.Cheat_Tables.Location, _copyToAndRunFromFolder & "Cheat Tables\", True)
                If My.Computer.FileSystem.DirectoryExists(_copyToAndRunFromFolder & "Net Streams\") = False Then My.Computer.FileSystem.CreateDirectory(_copyToAndRunFromFolder & "Net Streams\")
                If My.Computer.FileSystem.DirectoryExists(_copyToAndRunFromFolder & "Net Streams\i\") = False Then My.Computer.FileSystem.CreateDirectory(_copyToAndRunFromFolder & "Net Streams\i\")
                If My.Computer.FileSystem.DirectoryExists(_copyToAndRunFromFolder & "Net Streams\o\") = False Then My.Computer.FileSystem.CreateDirectory(_copyToAndRunFromFolder & "Net Streams\o\")
                Process.Start(_copyToAndRunFromFolder & "Cheat Tables\" & "MainTable" & _suffix & ".CT")
            Catch ex As Exception
            End Try
        Else
            Try
                'Shell() works 
                If My.Computer.FileSystem.DirectoryExists(SL.Path.CurrentAccount.Location & "Net Streams\") = False Then My.Computer.FileSystem.CreateDirectory(SL.Path.CurrentAccount.Location & "Net Streams\")
                If My.Computer.FileSystem.DirectoryExists(SL.Path.CurrentAccount.Location & "Net Streams\i\") = False Then My.Computer.FileSystem.CreateDirectory(SL.Path.CurrentAccount.Location & "Net Streams\i\")
                If My.Computer.FileSystem.DirectoryExists(SL.Path.CurrentAccount.Location & "Net Streams\o\") = False Then My.Computer.FileSystem.CreateDirectory(SL.Path.CurrentAccount.Location & "Net Streams\o\")
                Process.Start(SL.Path.CurrentAccount.Cheat_Tables.Location & "MainTable" & _suffix & ".CT")
            Catch ex As Exception
            End Try
        End If
    End Sub
    Public Sub LaunchEmulatorAndDiscImage()
        Try
            Dim path As String = SL.AccountFile.GetTagsData("Emulator Path", 0)
            Dim arg As String = Chr(34) & SL.AccountFile.GetTagsData("ISO Path", 0) & Chr(34)
            Dim myProcess As New Process
            myProcess.StartInfo.WorkingDirectory = Microsoft.VisualBasic.Left(path, path.LastIndexOf("\"))
            myProcess.StartInfo.FileName = Microsoft.VisualBasic.Right(path, path.Length - path.LastIndexOf("\") - 1)
            myProcess.StartInfo.Arguments = arg
            myProcess.Start()
        Catch ex As Exception
        End Try
    End Sub
    Public Sub LaunchGameFullExperience()
        If My.Computer.FileSystem.FileExists(SL.AccountFile.GetTagsData("Emulator Path", 0)) And My.Computer.FileSystem.FileExists(SL.AccountFile.GetTagsData("ISO Path", 0)) Then
            Try
                Dim path As String = SL.AccountFile.GetTagsData("Emulator Path", 0)
                Dim arg As String = Chr(34) & SL.AccountFile.GetTagsData("ISO Path", 0) & Chr(34)
                Dim myProcess As New Process
                myProcess.StartInfo.WorkingDirectory = Microsoft.VisualBasic.Left(path, path.LastIndexOf("\"))
                myProcess.StartInfo.FileName = Microsoft.VisualBasic.Right(path, path.Length - path.LastIndexOf("\") - 1)
                myProcess.StartInfo.Arguments = arg
                LaunchCE("_Silent", SL.AccountFile.GetTagsData("RAM Disq Drive", 0))
                Threading.Thread.Sleep(500)
                myProcess.Start()
            Catch ex As Exception
            End Try
        Else
            LaunchCE()
        End If
    End Sub

    Private Sub Button16_Click(sender As System.Object, e As System.EventArgs) Handles Button16.Click
        LaunchGameFullExperience()
    End Sub

    Private Sub Button17_Click(sender As System.Object, e As System.EventArgs) Handles Button17.Click
        LaunchCE(Nothing, SL.AccountFile.GetTagsData("RAM Disq Drive", 0))
    End Sub

    Private Sub Button18_Click(sender As System.Object, e As System.EventArgs) Handles Button18.Click
        LaunchEmulatorAndDiscImage()
    End Sub

    Private Sub Button19_Click(sender As System.Object, e As System.EventArgs) Handles Button19.Click
        SL.AccountFile.UpdateDataInTag("RAM Disq Drive") = ComboBox1.SelectedItem
        SL.AccountFile.SaveFile()
    End Sub

    Private Sub Button20_Click(sender As System.Object, e As System.EventArgs) Handles Button20.Click
        If SL.Path.CurrentAccount Is Nothing Then Exit Sub
        Process.Start(SL.Path.CurrentAccount.Location)
        If SL.Path.CurrentAccount.Cheat_Tables.Exists = False Then SL.Path.CurrentAccount.Cheat_Tables.Create()
        If SL.Path.CurrentAccount.LUAs.Exists = False Then SL.Path.CurrentAccount.LUAs.Create()
    End Sub

    Private Sub Button21_Click(sender As System.Object, e As System.EventArgs) Handles Button21.Click
        Dim dl As Boolean = False
        Dim dc As Boolean = False
        If My.Computer.FileSystem.DirectoryExists(SL.Path.CurrentAccount.Location & "LUAs\") Then
            If MsgBox("LUA files appear to exist. Delete and reinstall Anyway?", MsgBoxStyle.YesNo, "Overwrite Existing Folder?") = MsgBoxResult.No Then
                Exit Sub
            Else
                dl = True
            End If
        End If
        If My.Computer.FileSystem.DirectoryExists(SL.Path.CurrentAccount.Location & "LUAs\") Then
            If MsgBox("Cheat Table files appear to exist. Delete and reinstall Anyway?", MsgBoxStyle.YesNo, "Overwrite Existing Folder?") = MsgBoxResult.No Then
                Exit Sub
            Else
                dc = True
            End If
        End If
        If dl Then My.Computer.FileSystem.DeleteDirectory(SL.Path.CurrentAccount.Location & "LUAs\", FileIO.DeleteDirectoryOption.DeleteAllContents)
        If dc Then My.Computer.FileSystem.DeleteDirectory(SL.Path.CurrentAccount.Location & "Cheat Tables\", FileIO.DeleteDirectoryOption.DeleteAllContents)
        IO.File.WriteAllBytes(SL.Path.CurrentAccount.Location & "LUAs.zip", My.Resources.LUAs)
        IO.Compression.ZipFile.ExtractToDirectory(SL.Path.CurrentAccount.Location & "LUAs.zip", SL.Path.CurrentAccount.Location)
        My.Computer.FileSystem.DeleteFile(SL.Path.CurrentAccount.Location & "LUAs.zip")
        IO.File.WriteAllBytes(SL.Path.CurrentAccount.Location & "Cheat Tables.zip", My.Resources.Cheat_Tables)
        IO.Compression.ZipFile.ExtractToDirectory(SL.Path.CurrentAccount.Location & "Cheat Tables.zip", SL.Path.CurrentAccount.Location)
        My.Computer.FileSystem.DeleteFile(SL.Path.CurrentAccount.Location & "Cheat Tables.zip")
    End Sub
    Private Sub Button22_Click(sender As System.Object, e As System.EventArgs) Handles Button22.Click
        Dim dl As Boolean = False
        If My.Computer.FileSystem.DirectoryExists(SL.Path.CurrentAccount.Game_Data.Location) Then
            If MsgBox("Custom Files appear to exist. Delete and reinstall Anyway?", MsgBoxStyle.YesNo, "Overwrite Existing Folder?") = MsgBoxResult.No Then
                Exit Sub
            Else
                dl = True
            End If
        End If
        If dl Then My.Computer.FileSystem.DeleteDirectory(SL.Path.CurrentAccount.Game_Data.Location, FileIO.DeleteDirectoryOption.DeleteAllContents)
        IO.File.WriteAllBytes(SL.Path.CurrentAccount.Location & "Kairen Content.zip", My.Resources.Game_Data)
        IO.Compression.ZipFile.ExtractToDirectory(SL.Path.CurrentAccount.Location & "Kairen Content.zip", SL.Path.CurrentAccount.Game_Data.Location)
        My.Computer.FileSystem.DeleteFile(SL.Path.CurrentAccount.Location & "Kairen Content.zip")
    End Sub

    Dim ObjectForm_AbilityMaker As Form_AbilityMaker
    Dim ObjectForm_ItemMaker As Form_ItemMaker
    Dim ObjectForm_NPCWindow As Form_NPCMaker
    Private Sub CheckedListBox1_ItemCheck(sender As System.Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles CheckedListBox1.ItemCheck
        Select Case CheckedListBox1.Items.Item(e.Index)
            Case "Ability Maker"
                If e.NewValue = CheckState.Checked Then
                    If ObjectForm_AbilityMaker Is Nothing Then
                        ObjectForm_AbilityMaker = New Form_AbilityMaker(CL, SL)
                        ObjectForm_AbilityMaker.Show()
                    End If
                Else
                    If ObjectForm_AbilityMaker IsNot Nothing Then
                        ObjectForm_AbilityMaker.Close()
                        ObjectForm_AbilityMaker = Nothing
                    End If
                End If
                
            Case "Item Maker"
                If e.NewValue = CheckState.Checked Then
                    If ObjectForm_ItemMaker Is Nothing Then
                        ObjectForm_ItemMaker = New Form_ItemMaker(CL, SL)
                        ObjectForm_ItemMaker.Show()
                    End If
                Else
                    If ObjectForm_ItemMaker IsNot Nothing Then
                        ObjectForm_ItemMaker.Close()
                        ObjectForm_ItemMaker = Nothing
                    End If
                End If

            Case "NPC Window"
                If e.NewValue = CheckState.Checked Then
                    If ObjectForm_NPCWindow Is Nothing Then
                        ObjectForm_NPCWindow = New Form_NPCMaker(CL, SL)
                        ObjectForm_NPCWindow.Show()
                    End If
                Else
                    If ObjectForm_NPCWindow IsNot Nothing Then
                        ObjectForm_NPCWindow.Close()
                        ObjectForm_NPCWindow = Nothing
                    End If
                End If
        End Select
    End Sub

End Class