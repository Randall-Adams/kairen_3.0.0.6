Public Class Form2
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
    End Sub
    Private Sub Form2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(250, 150)
        Me.Text = "Home Window"
        ListBox1.Items.Add("Library Manager")
        ListBox1.Items.Add("Music List")
        ListBox1.Items.Add("Media Playlist Maker")
    End Sub
#End Region

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If ListBox1.SelectedItem IsNot Nothing Then
            Select Case ListBox1.SelectedItem
                Case "Library Manager"
                    Dim StartFormObject As New Form_LibraryManager(CL, Path)
                    StartFormObject.Show()
                Case "Music List"
                    Dim StartFormObject As New Form_MusicList(CL, Path)
                    StartFormObject.Show()
                Case "Media Playlist Maker"
                    Dim StartFormObject As New Form_MediaPlaylistMaker(CL, Path)
                    StartFormObject.Show()
            End Select
        End If
    End Sub

    Private Sub ListBox1_DoubleClick(sender As System.Object, e As System.EventArgs) Handles ListBox1.DoubleClick
        Button1.PerformClick()
    End Sub
End Class