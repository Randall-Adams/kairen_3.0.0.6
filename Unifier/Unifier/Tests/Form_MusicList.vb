Public Class Form_MusicList
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
        Me.Text = "Music List"
        LoadList("Music List")
    End Sub
#End Region

    Private Sub LoadList(ByVal _listToLoad As String)

    End Sub

End Class