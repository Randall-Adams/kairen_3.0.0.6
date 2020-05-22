Public Class FormlessForm
    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim po As New Threading.Thread(AddressOf Start.Main)
        po.Start()
    End Sub
    Private Sub FormlessForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'Dim po As New Threading.Thread(AddressOf Start.Main)
        'po.Start()
        Me.Close()
    End Sub
End Class