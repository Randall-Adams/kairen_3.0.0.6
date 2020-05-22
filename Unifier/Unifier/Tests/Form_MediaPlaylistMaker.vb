Public Class Form_MediaPlaylistMaker
#Region "Global Stuff"
    Public CL As CommonLibrary
    Public Path As PathHandler
    Public PlaylistFile As FileClass_PlaylistFile
#End Region
#Region "This Form's Main Process Code"
    Public Sub New(ByRef _commonLibrary As CommonLibrary, ByRef _path As PathHandler)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        CL = _commonLibrary
        Path = _path
    End Sub
    Private Sub Form_MediaPlaylistMaker_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(50, 50)
        Me.Text = "Media Playlist Maker"
    End Sub
#End Region

#Region "Drag-Drop Stuffs"
    Private MouseIsDown As Boolean = False
    Private Sub TextBox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TextBox1.MouseDown
        ' Set a flag to show that the mouse is down.
        MouseIsDown = True
    End Sub
    Private Sub TextBox1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TextBox1.MouseMove
        If MouseIsDown Then
            ' Initiate dragging.
            TextBox1.DoDragDrop(TextBox1.Text, DragDropEffects.Copy)
        End If
        MouseIsDown = False
    End Sub
    Private Sub TextBox2_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TextBox2.DragEnter
        ' Check the format of the data being dropped.
        If (e.Data.GetDataPresent(DataFormats.Text)) Then
            ' Display the copy cursor.
            e.Effect = DragDropEffects.Copy
        Else
            ' Display the no-drop cursor.
            e.Effect = DragDropEffects.None
        End If
    End Sub
    Private Sub TextBox2_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TextBox2.DragDrop
        ' Paste the text.
        TextBox2.Text = e.Data.GetData(DataFormats.Text)
    End Sub
    Private Sub CheckBox2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox2.CheckedChanged
        Me.TopMost = sender.Checked
    End Sub
#End Region

    Private Sub Button10_Click(sender As System.Object, e As System.EventArgs) Handles Button10.Click
        'Dim folderpath As String = InputBox("Enter the folder to load", "Load a folders contents")
        If My.Computer.FileSystem.DirectoryExists(TextBox6.Text) = False Then Exit Sub
        Dim files() As String = CL.ReturnFilesFromFolder(TextBox6.Text)
        If files.Length <= 0 Then Exit Sub
        ListBox4.Items.Clear()
        For Each item In files
            ListBox4.Items.Add(item)
        Next
    End Sub

    Private Sub Button11_Click(sender As System.Object, e As System.EventArgs) Handles Button11.Click
        If ListBox4.SelectedIndex = -1 Then Exit Sub
        TextBox2.Text = ListBox4.SelectedItem
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Or TextBox1.Text = "" Then Exit Sub
        ListBox1.Items.Add(TextBox1.Text)
        ListBox2.Items.Add(TextBox2.Text)
        TextBox1.Clear()
        TextBox2.Clear()
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        'move item up
        If ListBox1.SelectedIndex = -1 Or ListBox2.SelectedIndex = -1 Then Exit Sub
        Listbox1and2AutoSelectIsLocked = True
        MoveListBoxItemUp(ListBox1, ListBox1.SelectedIndex)
        MoveListBoxItemUp(ListBox2, ListBox2.SelectedIndex)
        Listbox1and2AutoSelectIsLocked = False
    End Sub
    Private Sub MoveListBoxItemUp(ByRef _listbox As ListBox, ByVal _itemIndex As Integer)
        If _listbox.SelectedIndex = -1 Then Exit Sub
        If _listbox.SelectedIndex = 0 Then Exit Sub 'exit if trying to move the first item up
        Dim _listboxselectedindex As Integer = _listbox.SelectedIndex
        Dim _listboxcontents(_listbox.Items.Count - 1) As String
        _listbox.Items.CopyTo(_listboxcontents, 0)
        _listbox.Items.Clear()
        For i As Integer = 0 To _listboxcontents.Length - 1
            If i <> _listboxselectedindex - 1 Then
                _listbox.Items.Add(_listboxcontents(i))
            Else
                i += 1
                _listbox.Items.Add(_listboxcontents(i))
                _listbox.Items.Add(_listboxcontents(i - 1))
            End If
        Next
        _listbox.SelectedIndex = _listboxselectedindex - 1
    End Sub
    Private Sub MoveListBoxItemDown(ByRef _listbox As ListBox, ByVal _itemIndex As Integer)
        If _listbox.SelectedIndex = -1 Then Exit Sub
        If _listbox.SelectedIndex = _listbox.Items.Count - 1 Then Exit Sub 'exit if trying to move the last item down
        Dim _listboxselectedindex As Integer = _listbox.SelectedIndex
        Dim _listboxcontents(_listbox.Items.Count - 1) As String
        _listbox.Items.CopyTo(_listboxcontents, 0)
        _listbox.Items.Clear()
        For i As Integer = 0 To _listboxcontents.Length - 1
            If i <> _listboxselectedindex Then
                _listbox.Items.Add(_listboxcontents(i))
            Else
                i += 1
                _listbox.Items.Add(_listboxcontents(i))
                _listbox.Items.Add(_listboxcontents(i - 1))
            End If
        Next
        _listbox.SelectedIndex = _listboxselectedindex + 1
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        'move item down
        If ListBox1.SelectedIndex = -1 Or ListBox2.SelectedIndex = -1 Then Exit Sub
        Listbox1and2AutoSelectIsLocked = True
        MoveListBoxItemDown(ListBox1, ListBox1.SelectedIndex)
        MoveListBoxItemDown(ListBox2, ListBox2.SelectedIndex)
        Listbox1and2AutoSelectIsLocked = False
    End Sub

    Private Sub Button13_Click(sender As System.Object, e As System.EventArgs) Handles Button13.Click
        FolderBrowserDialog1.SelectedPath = Nothing
        FolderBrowserDialog1.ShowDialog()
        If FolderBrowserDialog1.SelectedPath.LastIndexOf("/") <> FolderBrowserDialog1.SelectedPath.Length - 1 Then FolderBrowserDialog1.SelectedPath = FolderBrowserDialog1.SelectedPath & "\"
        TextBox4.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub Button12_Click(sender As System.Object, e As System.EventArgs) Handles Button12.Click
        FolderBrowserDialog1.SelectedPath = Nothing
        FolderBrowserDialog1.ShowDialog()
        If FolderBrowserDialog1.SelectedPath.LastIndexOf("/") <> FolderBrowserDialog1.SelectedPath.Length - 1 Then FolderBrowserDialog1.SelectedPath = FolderBrowserDialog1.SelectedPath & "\"
        TextBox6.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If TextBox4.Text = "" Or TextBox5.Text = "" Then Exit Sub
        If My.Computer.FileSystem.DirectoryExists(TextBox4.Text) = False Then Exit Sub
        If Microsoft.VisualBasic.Right(TextBox5.Text, 4).ToLower = ".m3u" Then TextBox5.Text = Microsoft.VisualBasic.Left(TextBox5.Text, TextBox5.Text.Length - 4)
        PlaylistFile = New FileClass_PlaylistFile(TextBox4.Text, TextBox5.Text, ".m3u")
        Dim i As Integer = 0
        Do Until i > ListBox1.Items.Count - 1
            PlaylistFile.AddMediaToPlaylist(i + 1, ListBox1.Items.Item(i), ListBox2.Items.Item(i))
            i += 1
        Loop
        PlaylistFile.SaveFile()
    End Sub

    Private Listbox1and2AutoSelectIsLocked As Boolean = False
    Private Sub ListBox1or2_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListBox1.SelectedIndexChanged, ListBox2.SelectedIndexChanged
        Dim notsender As ListBox
        If sender Is ListBox1 Then notsender = ListBox2 Else notsender = ListBox1
        If Listbox1and2AutoSelectIsLocked = False Then
            Listbox1and2AutoSelectIsLocked = True
            notsender.SelectedIndex = sender.SelectedIndex
            Listbox1and2AutoSelectIsLocked = False
        End If
    End Sub

    Private Sub ListBox1and2_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles ListBox1.KeyPress, ListBox2.KeyPress
        If ListBox1.SelectedIndex <> -1 Then
            If e.KeyChar = ChrW(Keys.ShiftKey) And e.KeyChar = ChrW(Keys.Up) Then
                Listbox1and2AutoSelectIsLocked = True
                MoveListBoxItemUp(ListBox1, ListBox1.SelectedIndex)
                MoveListBoxItemUp(ListBox2, ListBox2.SelectedIndex)
                Listbox1and2AutoSelectIsLocked = False
            ElseIf e.KeyChar = ChrW(Keys.ShiftKey) And e.KeyChar = ChrW(Keys.Down) Then
                Listbox1and2AutoSelectIsLocked = True
                MoveListBoxItemDown(ListBox1, ListBox1.SelectedIndex)
                MoveListBoxItemDown(ListBox2, ListBox2.SelectedIndex)
                Listbox1and2AutoSelectIsLocked = False
            End If
        End If
    End Sub
End Class