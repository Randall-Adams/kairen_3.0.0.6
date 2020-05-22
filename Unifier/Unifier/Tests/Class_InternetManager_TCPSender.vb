Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Public Class Class_InternetManager_TCPSender
    Dim Client As New TcpClient
    Public Sub SendData(ByVal _IP As String, ByVal _Port As Integer, ByRef _data As String)
        Try
            Client = New TcpClient(_IP, _Port) ' Declare the Client as an IP:Port Address. 
            Dim Writer As New StreamWriter(Client.GetStream())
            Writer.Write(_data)
            Writer.Flush()
        Catch ex As Exception
            Console.WriteLine(ex)
            Dim Errorresult As String = ex.Message
            MessageBox.Show(Errorresult & vbNewLine & vbNewLine & _
                            "Please Review Client Address", _
                            "Error Sending Message", _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
