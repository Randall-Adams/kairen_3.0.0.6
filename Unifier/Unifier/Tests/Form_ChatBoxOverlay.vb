Public Class Form_ChatBoxOverlay
    Dim i As Integer = 0
    Dim i2 As Integer = 0
    Public MyOptionsForm As Form_ChatBoxOverlay_Options
    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Show()
    End Sub
    Private Sub Form_ChatBoxOverlay_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(413, 300)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.Opacity = 0.75
        NotifyIcon1.Icon = My.Resources.Chat_Icon
        NotifyIcon1.Visible = True
        RichTextBox1.ReadOnly = True
        RichTextBox1.SelectionProtected = True
        Timer1.Start()
    End Sub
    Private Sub KillFocus()
        Button1.Visible = True
        Button1.Select()
        'Button1.Visible = False
        RichTextBox1.SelectionStart = RichTextBox1.TextLength
    End Sub
    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        KillFocus()
        If 1 = 2 Then
            If i2 < 17 Then
                Dim colorr As New Color
                If i = 1 Then
                    colorr = Color.Red
                ElseIf i = 2 Then
                    colorr = Color.Blue
                ElseIf i = 3 Then
                    colorr = Color.Green
                End If
                RichTextBox1.Text = RichTextBox1.Text & "bobby"
                RichTextBox1.SelectionProtected = False
                RichTextBox1.Select(RichTextBox1.TextLength - 5, 5)
                RichTextBox1.SelectionColor = colorr
                RichTextBox1.ScrollToCaret()
                RichTextBox1.SelectionProtected = True
                RichTextBox1.Text = RichTextBox1.Text & Environment.NewLine
                KillFocus()
                i += 1
                If i = 4 Then i = 1
            ElseIf i2 = 17 Then
                RichTextBox1.Text = RichTextBox1.Text & "end."
                RichTextBox1.SelectionProtected = False
                RichTextBox1.Select(RichTextBox1.TextLength - 5, 5)
                RichTextBox1.ScrollToCaret()
                RichTextBox1.SelectionProtected = True
                KillFocus()
            Else
                KillFocus()
                'Timer1.Stop()
            End If
            i2 += 1
        End If
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles RichTextBox1.Click
        'Timer1.Start()
    End Sub
    Public WriteOnly Property AddChatText As String
        Set(value As String)
            RichTextBox1.Text = RichTextBox1.Text & value
            RichTextBox1.SelectionProtected = False
            RichTextBox1.Select(RichTextBox1.TextLength - 5, 5)
            RichTextBox1.ScrollToCaret()
            RichTextBox1.SelectionProtected = True
            RichTextBox1.Text = RichTextBox1.Text & Environment.NewLine
            KillFocus()
        End Set
    End Property

    Private Sub ContextMenuStrip1_ItemClicked(sender As System.Object, e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ContextMenuStrip1.ItemClicked
        If e.ClickedItem Is OptionsToolStripMenuItem Then
            If MyOptionsForm IsNot Nothing Then
                MyOptionsForm.WindowState = FormWindowState.Normal
                MyOptionsForm.BringToFront()
            Else
                MyOptionsForm = New Form_ChatBoxOverlay_Options(Me)
            End If

        ElseIf e.ClickedItem Is ExitChatToolStripMenuItem Then
            Me.Close()
        End If
    End Sub

    Private Sub Form_ChatBoxOverlay_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        NotifyIcon1.Visible = False
        NotifyIcon1.Icon = Nothing
    End Sub
End Class