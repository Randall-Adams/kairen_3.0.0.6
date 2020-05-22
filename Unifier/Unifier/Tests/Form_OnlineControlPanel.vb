Public Class Form_OnlineControlPanel
    Public CL As CommonLibrary
    Public SL As SpecificLibrary
    Public WithEvents IM As Class_InternetManager
    Private RIP As String
    Sub New(ByRef _commonLibrary As CommonLibrary, ByRef _specificLibrary As SpecificLibrary)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CL = _commonLibrary
        SL = _specificLibrary
        Me.Show()
    End Sub
    Private Sub Form_OnlineControlPanel_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If SL.AccountFile IsNot Nothing Then SL.AccountFile.Lock = Me
        Me.Location = New Point(250, 150)
        LoadWorldFilesIntoListBox()
        If SL.Kairen_Current_Version_TYPE <> "DELTA" Then
            GroupBox1.Visible = False
        End If
    End Sub
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        IM = Nothing
        SL.ChatBoxOverlayObject = Nothing
        IM = New Class_InternetManager(CL, SL)
        Label4.Text = IM.PublicIP
        TextBox2.Text = IM.KairenChatPort
        SL.ChatBoxOverlayObject = New Form_ChatBoxOverlay() 'CL, SL)
        IM.StartChatServer("0.0.0.0")
        'this needs to starts server though .. right now it server modes but shit's gotona change!
    End Sub

    Private Sub UpdateChatBox(ByVal ChatMessageData As String) Handles IM.ChatProtocolClient_MessageReceived_Event, IM.ChatProtocolServer_MessageReceived_Event
        If SL.ChatBoxOverlayObject Is Nothing Then Exit Sub
        SL.ChatBoxOverlayObject.AddChatText() = ChatMessageData
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        IM.SendChatMessage(TextBox3.Text, TextBox4.Text, TextBox5.Text)
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        RIP = SL.ReadLineFromFileOnline("http://kairen.returnhome.co/databits/kairenrip/rip.txt")
        IM = Nothing
        SL.ChatBoxOverlayObject = Nothing
        IM = New Class_InternetManager(CL, SL)
        Label4.Text = IM.PublicIP
        TextBox2.Text = IM.KairenChatPort
        SL.ChatBoxOverlayObject = New Form_ChatBoxOverlay() 'CL, SL)
        IM.StartChatClient(RIP)
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Button2.PerformClick()
        RIP = SL.ReadLineFromFileOnline("http://kairen.returnhome.co/databits/kairenrip/rip.txt")
        IM.StartChatClient(RIP)
    End Sub
    Private Sub LoadWorldFilesIntoListBox()
        ListBox1.Items.Clear()
        If SL.AccountFile IsNot Nothing Then
            If CL.ReadFoldersToArray(SL.Path.CurrentAccount.Worlds.Location) Is Nothing Then Exit Sub
            For Each world In CL.ReadFoldersToArray(SL.Path.CurrentAccount.Worlds.Location)
                ListBox1.Items.Add(world)
            Next
        End If
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        If ListBox1.SelectedIndex = -1 Then Exit Sub
        Try
            Dim path As String = SL.AccountFile.GetTagsData("Emulator Path", 0)
            Dim arg As String = Chr(34) & SL.AccountFile.GetTagsData("ISO Path", 0) & Chr(34)
            Dim myProcess As New Process
            myProcess.StartInfo.WorkingDirectory = Microsoft.VisualBasic.Left(path, path.LastIndexOf("\"))
            myProcess.StartInfo.FileName = Microsoft.VisualBasic.Right(path, path.Length - path.LastIndexOf("\") - 1)
            myProcess.StartInfo.Arguments = arg
            Process.Start(SL.Path.CurrentAccount.Cheat_Tables.Location & "MainTable" & "_Silent" & ".CT")
            Threading.Thread.Sleep(500)
            myProcess.Start()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        IM = New Class_InternetManager(CL, SL)
        Label4.Text = IM.PublicIP
        TextBox2.Text = IM.KairenChatPort

        Label9.Text = IM.PublicIP
        TextBox6.Text = IM.KairenGamePort
    End Sub

    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
        IM.StartChatServer("0.0.0.0")
        Label12.Text = "Online"
        'IM.StartGameServer("0.0.0.0")
        'Label14.Text = "Online"
    End Sub
    Private Sub UpdateServerChat(ByVal ChatMessageData As String) Handles IM.ChatProtocolClient_MessageReceived_Event, IM.ChatProtocolServer_MessageReceived_Event
        TextBox1.Text = TextBox1.Text & ChatMessageData & Environment.NewLine
    End Sub

    Private Sub Form_OnlineControlPanel_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If SL.AccountFile IsNot Nothing Then SL.AccountFile.Unlock = Me
    End Sub
End Class