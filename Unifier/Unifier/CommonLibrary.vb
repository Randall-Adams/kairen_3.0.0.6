'Imports System.Runtime.InteropServices 'This allows the DLLImport line for AllocConsole and FreeConsole
'AllocConsole() 'Makes Console
'Console.WriteLine("Important Information Here") 'Displays Console Text
'FreeConsole() 'Closes Console
'<DllImport("kernel32.dll")> Public Shared Function AllocConsole() As Boolean
'End Function
'<DllImport("kernel32.dll")> Public Shared Function FreeConsole() As Boolean
'End Function
'Dim proc As New System.Diagnostics.Process()
'proc = Process.Start(Location, "")
'proc = Process.Start(Path, "")
Public Class CommonLibrary
    Sub New()

    End Sub

    Public Function ReadFoldersToArray(ByVal _folderPath As String)
        If My.Computer.FileSystem.DirectoryExists(_folderPath) = False Then Return Nothing

        Dim _returnVar() As String
        Dim i As Integer = 0

        Dim di As New IO.DirectoryInfo(_folderPath)
        Dim diar1 As IO.DirectoryInfo() = di.GetDirectories()
        Dim dra As IO.DirectoryInfo

        'list the names of all files in the specified directory
        If diar1.Length = 0 Then
            ReDim _returnVar(-1)
        Else
            For Each dra In diar1
                ReDim Preserve _returnVar(i)
                _returnVar(i) = dra.ToString.Replace(_folderPath, "")
                i = i + 1
            Next
        End If

        Return _returnVar
    End Function
    Public Function ReturnFilesFromFolder(ByVal _lookInThisFolder As String, Optional ByVal _onlyReturnThisExtension As String = "")
        If My.Computer.FileSystem.DirectoryExists(_lookInThisFolder) = False Then Return Nothing

        Dim _returnVar() As String
        Dim i As Integer = 0

        Dim di As New IO.DirectoryInfo(_lookInThisFolder)
        Dim diar1 As IO.FileInfo() = di.GetFiles()
        Dim dra As IO.FileInfo

        'list the names of all files in the specified directory
        If diar1.Length = 0 Then
            ReDim _returnVar(0)
            _returnVar(0) = ""
        ElseIf _onlyReturnThisExtension = "" Then
            For Each dra In diar1
                ReDim Preserve _returnVar(i)
                _returnVar(i) = dra.ToString()
                i = i + 1
            Next
        Else
            For Each dra In diar1
                If Microsoft.VisualBasic.Right(dra.ToString(), _onlyReturnThisExtension.Length) = _onlyReturnThisExtension Then
                    ReDim Preserve _returnVar(i)
                    _returnVar(i) = dra.ToString.Replace(_onlyReturnThisExtension, "")
                    i = i + 1
                End If
            Next
        End If

        Return _returnVar
    End Function
End Class
