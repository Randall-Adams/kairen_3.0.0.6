Public Class CommandLineArgumentInterpreter
    Private CommandLineArguments As String() = Environment.GetCommandLineArgs()
    Private CommandLineArgument_Command(-1) As String
    Private CommandLineArgument_Argument() As String
    ''' <summary>
    ''' Fills the passed in string arrays with the command line arguments provided to the program, and the arguments for those arguments.
    ''' </summary>
    ''' <param name="_CommandLineArgument_Command">The string array to fill with each command line argument.</param>
    ''' <param name="_CommandLineArgument_Argument">The string array to fill with each command line argument's argument.</param>
    ''' <remarks>Right now this only sees "/" commands, "-" needs added.</remarks>
    Public Sub New(ByRef _CommandLineArgument_Command() As String, ByRef _CommandLineArgument_Argument() As String)
        If CommandLineArguments.GetUpperBound(0) = 0 Then Exit Sub
        Dim i2 As Integer
        For i As Integer = 1 To CommandLineArguments.GetUpperBound(0)
            i2 = CommandLineArgument_Command.Length
            ParseCmdArgs(i, i2)
        Next
        _CommandLineArgument_Command = CommandLineArgument_Command
        _CommandLineArgument_Argument = CommandLineArgument_Argument
    End Sub
    
    Private Sub ParseCmdArgs(ByRef CurrentIndex As Integer, ByRef CommandIndex As Integer)
        ReDim Preserve CommandLineArgument_Command(CommandIndex)
        ReDim Preserve CommandLineArgument_Argument(CommandIndex)
        CommandLineArgument_Command(CommandIndex) = CommandLineArguments(CurrentIndex)
        CurrentIndex = CurrentIndex + 1
        While CurrentIndex <= CommandLineArguments.GetUpperBound(0)
            If Microsoft.VisualBasic.Left(CommandLineArguments(CurrentIndex), 1) <> "/" Then
                If CommandLineArgument_Argument(CommandIndex) <> "" Then CommandLineArgument_Argument(CommandIndex) = CommandLineArgument_Argument(CommandIndex) & " "
                CommandLineArgument_Argument(CommandIndex) = CommandLineArgument_Argument(CommandIndex) + CommandLineArguments(CurrentIndex)
            Else
                CurrentIndex = CurrentIndex - 1
                Exit While
            End If
            CurrentIndex = CurrentIndex + 1
        End While
    End Sub
End Class
