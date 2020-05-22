Public Class Form_NPCMaker
    Public CL As CommonLibrary
    Public SL As SpecificLibrary
    Dim bl As Boolean = True
    Sub New(ByRef _commonLibrary As CommonLibrary, ByRef _specificLibrary As SpecificLibrary)
        InitializeComponent()
        CL = _commonLibrary
        SL = _specificLibrary
    End Sub
    Private Sub Form_NPCMaker_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        TextBox4.ScrollBars = ScrollBars.Vertical
        TextBox6.ScrollBars = ScrollBars.Vertical
        TextBox1.Text = 24640.26953125
        TextBox3.Text = 54.125995635986
        TextBox2.Text = 11865.365234375
        NumericUpDown1.Maximum = 10000
        NumericUpDown1.Value = 23
        RadioButton1.Select()
        If SL.AccountFile IsNot Nothing Then
            SL.AccountFile.Lock = Me
            Me.Text = "Kairen - NPC Window - " & SL.AccountFile.GetTagsData("Name", 0)
            TextBox5.Text = SL.Path.CurrentAccount.Custom_Data.Location & "NPC Maker\"
            Button1.PerformClick()
            bl = False
        Else
            Me.Text = "Kairen - NPC Window"
        End If
    End Sub

    Private Sub Form_NPCMaker_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If SL.AccountFile IsNot Nothing Then SL.AccountFile.Unlock = Me
    End Sub

    Dim npclist(-1) As String
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        TextBox4.Clear()
        TextBox6.Clear()
        npclist = CL.ReturnFilesFromFolder(TextBox5.Text)
        ReDim SL.OldNPCFile(npclist.Count - 1)
        'Dim cd1(NumericUpDown1.Value - 1, 0) As String
        'Dim cd2(NumericUpDown1.Value - 1, 0) As Integer
        For i1 As Integer = 0 To npclist.Count - 1
            SL.OldNPCFile(i1) = New FileClass_OldFormatFile_NPCFile(TextBox1.Text, TextBox2.Text, TextBox5.Text, npclist(i1))
            NPCDistanceAdd(SL.OldNPCFile(i1))
        Next

        Exit Sub
        'Dim i As Integer = 0
        'For Each npc In npclist
        '    SL.OldNPCFile = New FileClass_OldFormatFile_NPCFile(TextBox5.Text, npc)
        '    TextBox4.AppendText("File: " & SL.OldNPCFile.SafeName)
        '    TextBox4.AppendText(vbNewLine)
        '    TextBox4.AppendText("Vers: " & SL.OldNPCFile.FileFormatVersion)
        '    TextBox4.AppendText(vbNewLine)
        '    TextBox4.AppendText("Name: " & SL.OldNPCFile.GameName)
        '    TextBox4.AppendText(vbNewLine)
        '    TextBox6.AppendText(SL.OldNPCFile.GameName)
        '    TextBox6.AppendText(vbNewLine)
        '    If RadioButton1.Checked Then
        '        TextBox4.AppendText("LocX: " & SL.OldNPCFile.X)
        '        TextBox4.AppendText(vbNewLine)
        '        TextBox4.AppendText("LocY: " & SL.OldNPCFile.Y)
        '        TextBox4.AppendText(vbNewLine)
        '        TextBox4.AppendText("LocZ: " & SL.OldNPCFile.Z)
        '        TextBox4.AppendText(vbNewLine)
        '    Else
        '        TextBox4.AppendText("LocX: " & SL.OldNPCFile.X)
        '        TextBox4.AppendText(vbNewLine)
        '        TextBox4.AppendText("LocZ: " & SL.OldNPCFile.Y)
        '        TextBox4.AppendText(vbNewLine)
        '        TextBox4.AppendText("LocY: " & SL.OldNPCFile.Z)
        '        TextBox4.AppendText(vbNewLine)
        '    End If
        '    TextBox4.AppendText("Face: " & SL.OldNPCFile.F)
        '    TextBox4.AppendText(vbNewLine)
        '    TextBox4.AppendText(vbNewLine)
        '    i += 1
        '    If i >= NumericUpDown1.Value Then Exit For
        'Next
        'Label6.Text = "Number of Results: " & i
    End Sub

    Private Sub NPCDistanceAdd(ByVal _NPC As FileClass_OldFormatFile_NPCFile)

    End Sub
    Public Class DistanceList
        Dim Place(-1) As List(Of String)
        Dim Name As New List(Of String)
        Dim NPCIndex As New List(Of Integer)
        Dim Value As New List(Of Integer)
        Sub New()

        End Sub
        Sub Add(ByVal _name As String, ByVal _value As Integer, ByVal _npcIndex As Integer)
            If Place.Count = -1 Then
                ReDim Place(0)
                Place(0) = New List(Of String)
                Name.Add(_name)
                NPCIndex.Add(_npcIndex)
                Value.Add(_value)
            Else
                If _value Then
                End If
            End If
        End Sub
    End Class
    Private Sub RadioButton1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            Label3.Text = "Y Position"
            Label4.Text = "Z Position"
        ElseIf RadioButton2.Checked Then
            Label3.Text = "Z Position"
            Label4.Text = "Y Position"
        End If
        If Not bl Then Button1.PerformClick()
    End Sub
End Class