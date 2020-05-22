Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Public Class TestForm_ExternalIPFinder
    Private Sub TestForm_ExternalIPFinder_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        TryIPGet()
    End Sub
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        TryIPGet()
    End Sub
    Private Function TryIPGet() As Boolean
        Try
            Dim rt As String
            Dim wRequest As WebRequest
            Dim wResponse As WebResponse
            Dim SR As StreamReader
            wRequest = WebRequest.Create("https://www.ip-adress.com/")
            wResponse = wRequest.GetResponse
            SR = New StreamReader(wResponse.GetResponseStream)
            rt = SR.ReadToEnd
            SR.Close()
            rt = Mid(rt, rt.IndexOf("Your IP address is: <strong>") + 1)
            rt = Microsoft.VisualBasic.Left(rt, rt.IndexOf("</strong></h1>"))
            rt = Microsoft.VisualBasic.Right(rt, rt.IndexOf("<strong>") - 6)
            TextBox1.Text = rt
            Return True
        Catch ex As Exception
            TextBox1.Text = ex.Message & "(" & Environment.NewLine & ex.ToString & ")"
            Return False
        End Try
    End Function
End Class