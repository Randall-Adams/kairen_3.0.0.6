Public Class Form1
#Region "Global Stuff"
    Public CL As CommonLibrary
    Public Path As PathHandler
#End Region

#Region "This Form's Main Process Code"
    Public Sub New(ByRef _commonLibrary As CommonLibrary, ByRef _path As PathHandler)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        CL = _commonLibrary
        Path = _path
        AccountListFile = New FileClass_AccountFileList(Path.RootFolderPath & "Account\", "AccountListFile")
    End Sub
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(400, 200)
        AccountListFile.LoadFile()
        'LoadAccountsIntoListbox()
    End Sub
#End Region

#Region "Code for this form only"
    'Code dealing with the AccountList file
    Private Sub FileLoadComplete_EventHandler() Handles AccountListFile.FileLoadComplete
        LoadAccountsIntoListbox()
        If AccountListFile.AutoLoadingAccount <> "" Then
            If ListBox1.Items.Contains(AccountListFile.AutoLoadingAccount) Then
                ListBox1.SelectedItem = AccountListFile.AutoLoadingAccount
                Button1.PerformClick()
                Me.Close()
            End If
        End If
    End Sub

    'Generated GUI event handler code
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If ListBox1.SelectedItem = Nothing Then Exit Sub
        If CheckBox1.Checked Then AccountListFile.AutoLoadingAccount = ListBox1.SelectedItem
        Dim StartFormObject As Form2
        StartFormObject = New Form2(CL, Path)
        StartFormObject.Show()
        Me.Close()
    End Sub
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Dim newaccoutname As String = InputBox("Enter New Account Name", "New Account Creation")
        If newaccoutname.Trim <> "" Then AccountListFile.AddAccountToFile() = newaccoutname.Trim
    End Sub
    Private Sub CheckBox1_Click(sender As System.Object, e As System.EventArgs) Handles CheckBox1.Click
        If ListBox1.SelectedItem = Nothing Then
            sender.Checked = False
        Else
            If sender.Checked = True Then
                sender.Checked = False
                AccountListFile.AutoLoadingAccount = ""
            Else
                sender.Checked = True
                AccountListFile.AutoLoadingAccount = ListBox1.SelectedItem
            End If
            AccountListFile.SaveFile()
        End If
    End Sub
    Private Sub ListBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        'check if the selected account is the autoload account
        CheckBox1.Checked = False
        If AccountListFile.AutoLoadingAccount = ListBox1.SelectedItem Then
            CheckBox1.Checked = True
        End If
    End Sub
#End Region

#Region "Account File Code"
    Private WithEvents AccountListFile As FileClass_AccountFileList
    Private Sub LoadAccountsIntoListbox()
        ListBox1.Items.Clear()
        For Each item In AccountListFile.AccountNameList
            ListBox1.Items.Add(item)
        Next
    End Sub
#End Region
End Class