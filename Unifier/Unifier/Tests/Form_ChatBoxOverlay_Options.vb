Public Class Form_ChatBoxOverlay_Options
    Private MyChatBox As Form_ChatBoxOverlay
    Sub New(ByRef _myChatBox As Form_ChatBoxOverlay)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MyChatBox = _myChatBox
        If MyChatBox.FormBorderStyle = Windows.Forms.FormBorderStyle.None Then CheckBox1.Checked = False Else CheckBox1.Checked = True
        If MyChatBox.TopMost Then CheckBox2.Checked = True Else CheckBox2.Checked = False
        Me.Show()
    End Sub
    Private Sub Form_ChatBoxOverlay_Options_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Button1.Enabled = False
    End Sub

    Private Sub Form_ChatBoxOverlay_Options_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        MyChatBox.MyOptionsForm = Nothing
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            MyChatBox.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
        Else
            MyChatBox.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            MyChatBox.TopMost = True
        Else
            MyChatBox.TopMost = False
        End If
    End Sub
End Class