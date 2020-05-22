Public Class Form_LibraryManager
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
    Private Sub Form_LibraryManager_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(250, 150)
        Me.Text = "Item Library Manager"
    End Sub
#End Region

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        ListView1.Columns.Add("Name")
        ListView1.Columns(0).Width = 120
        ListView1.Columns.Add("Types")
        ListView1.Columns(1).Width = 80
        ListView1.Columns.Add("Sources")
        ListView1.Columns(2).Width = 80
        ListView1.Columns.Add("Outputs")
        ListView1.Columns(3).Width = 120

        ListView1.Items.Add("I Took a Pill in Ibiza")
        ListView1.Items(0).SubItems.Add("Music")
        ListView1.Items(0).SubItems.Add("Youtube")
        ListView1.Items(0).SubItems.Add("Eidolon, toshi3")

        ListView1.Items.Add("Me Myself & I")
        ListView1.Items(1).SubItems.Add("Music")
        ListView1.Items(1).SubItems.Add("Youtube")
        ListView1.Items(1).SubItems.Add("Eidolon, toshi3")
    End Sub
End Class