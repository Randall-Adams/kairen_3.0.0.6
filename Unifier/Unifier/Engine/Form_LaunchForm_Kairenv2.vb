Public Class Form_LaunchForm_Kairenv2
    Public Event LoadingComplete()
    Public CL As CommonLibrary
    Public SL As SpecificLibrary
    Private CLA As CommandLineArgumentInterpreter
    Private CommandLineArguments_Command(-1) As String
    Private CommandLineArguments_CommandArgument(-1) As String
    Public Path As PathHandler
    Private Sub Form_LaunchForm_Kairenv2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(550, 200)
        Label1.Text = "Loading"
        Timer_AnimateLoad.Start()
        Timer_LoadCompleteChecker.Start()
        LoadSequence()
    End Sub
    Private Sub LoadSequence()
        CLA = New CommandLineArgumentInterpreter(CommandLineArguments_Command, CommandLineArguments_CommandArgument)
        CL = New CommonLibrary
        SL = New SpecificLibrary("3.0.0.6")
        Path = New PathHandler("Kairen")
    End Sub

    Private Sub LoadProgram() Handles Me.LoadingComplete
        Label1.Text = "Loaded"
        Me.MaximumSize = New Point(180, 180)
        Me.Size = New Point(180, 180)
        Me.MinimumSize = New Point(180, 180)
        Button1.Visible = True
        Button1.Select()
        If SL.UniversalOptions.FileExists Then If SL.UniversalOptions.GetTagsData("Autostart", 1) = "true" Then Button1.PerformClick() ' if autostart then autostart
    End Sub
    Private Sub Timer_AnimateLoad_Tick(sender As System.Object, e As System.EventArgs) Handles Timer_AnimateLoad.Tick
        AnimateLoad()
    End Sub
    Private Sub Timer_LoadCompleteChecker_Tick(sender As System.Object, e As System.EventArgs) Handles Timer_LoadCompleteChecker.Tick
        If CL IsNot Nothing And CLA IsNot Nothing Then
            Timer_LoadCompleteChecker.Stop()
            RaiseEvent LoadingComplete()
        End If
    End Sub
    Private Sub AnimateLoad()
        'If Label1.InvokeRequired Then
        'Label1.Invoke()
        'Else
        Dim NewText As String
        Select Case Label1.Text
            Case "Starting"
                NewText = "Loading"
            Case "Loading"
                NewText = "Loading."
            Case "Loading."
                NewText = "Loading.."
            Case "Loading.."
                NewText = "Loading..."
            Case "Loading..."
                NewText = "Loading"
            Case "Loaded"
                NewText = "Loaded."
                Timer_AnimateLoad.Stop()
            Case Else
                NewText = "Loading?"
        End Select
        Label1.Text = NewText
        'End If
    End Sub
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        'Dim StartFormObject As Form_AbilityMaker
        'StartFormObject = New Form_AbilityMaker(CL, Path)
        'StartFormObject.Show()
        Dim StartFormObject As Form_Kairenv2
        StartFormObject = New Form_Kairenv2(CL, SL)
        StartFormObject.Show()
        Me.Close()
    End Sub
End Class