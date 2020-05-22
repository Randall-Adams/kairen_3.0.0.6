Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Public Class TestForm_OnlineUpdate
    Private Sub TestForm_OnlineUpdate_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(413, 100)
        Dim lv, lvl As Object
        lv = GetLatestVersionFromOnline()
        If lv <> "error" Then
            lvl = GetVersionDownloadLinksOnline(lv)
        Else
            lvl = "lv was error"
        End If
        MsgBox("Latest Version " & Environment.NewLine & lv & Environment.NewLine & Environment.NewLine & "Download Link" & Environment.NewLine & lvl)
    End Sub
    Private Function GetLatestVersionFromOnline()
        Try
            Dim rt As String
            Dim wRequest As WebRequest
            Dim wResponse As WebResponse
            Dim SR As StreamReader
            wRequest = WebRequest.Create("http://kairen.returnhome.co/databits/kairenprogram/kairenlatestversion.txt")
            wResponse = wRequest.GetResponse
            SR = New StreamReader(wResponse.GetResponseStream)
            rt = SR.ReadToEnd
            SR.Close()
            rt = CStr(rt.Trim)
            Return rt
        Catch ex As Exception
            Return "error"
            '= ex.Message & "(" & Environment.NewLine & ex.ToString & ")"
        End Try
    End Function
    Private Function GetVersionDownloadLinksOnline(ByVal _versionNumber)
        Try
            Dim rt As String
            Dim wRequest As WebRequest
            Dim wResponse As WebResponse
            Dim SR As StreamReader
            wRequest = WebRequest.Create("http://kairen.returnhome.co/databits/kairenprogram/" & _versionNumber & ".txt")
            wResponse = wRequest.GetResponse
            SR = New StreamReader(wResponse.GetResponseStream)
            rt = CStr(SR.ReadToEnd)
            SR.Close()
            Return rt.Trim
        Catch ex As Exception
            Return "error"
            '= ex.Message & "(" & Environment.NewLine & ex.ToString & ")"
        End Try
    End Function

    Private Sub TrySaveLatestVersionFileFromOnline()
        Dim lv, lvl As Object
        lv = GetLatestVersionFromOnline()
        If lv <> "error" Then
            lvl = GetVersionDownloadLinksOnline(lv)
        Else
            lvl = "lv was error"
        End If

        Dim remoteUri As String = lvl
        Dim myStringWebResource As String = Nothing
        Dim myWebClient As New WebClient()
        myStringWebResource = remoteUri
        myWebClient.DownloadFile(myStringWebResource, My.Computer.FileSystem.SpecialDirectories.Desktop & "\Kairen.exe")
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        TrySaveLatestVersionFileFromOnline()
    End Sub

    Private Function ReadLineFromFileOnline(ByVal _urlToCheck As String) As String
        Try
            Dim rt As String
            Dim wRequest As WebRequest
            Dim wResponse As WebResponse
            Dim SR As StreamReader
            wRequest = WebRequest.Create(_urlToCheck)
            wResponse = wRequest.GetResponse
            SR = New StreamReader(wResponse.GetResponseStream)
            rt = SR.ReadToEnd
            SR.Close()
            rt = CStr(rt.Trim)
            Return rt
        Catch ex As Exception
            Return "error"
        End Try
    End Function
End Class