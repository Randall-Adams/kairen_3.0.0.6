Imports System.Net
Imports System.IO
Public Class TestForm_WebReader_FullPage

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            If TextBox1.Text.Contains("http://") = False And TextBox1.Text.Contains("https://") = False Then
                TextBox1.Text = "http://" & TextBox1.Text
            End If
            Dim rt As String
            Dim wRequest As WebRequest
            Dim wResponse As WebResponse
            Dim SR As StreamReader
            wRequest = WebRequest.Create(TextBox1.Text)
            wResponse = wRequest.GetResponse
            SR = New StreamReader(wResponse.GetResponseStream)
            rt = SR.ReadToEnd
            SR.Close()
            TextBox2.Text = rt
        Catch ex As Exception
            TextBox2.Text = ex.Message & Environment.NewLine & Environment.NewLine & ex.ToString
        End Try
    End Sub
End Class