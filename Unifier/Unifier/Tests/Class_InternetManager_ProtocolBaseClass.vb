Public MustInherit Class Class_InternetManager_ProtocolBaseClass
    'things that inherit this must keep track of their client list or what client they are
    Public ReadOnly LocalMachineIP As String
    Public ReadOnly LocalMachineUsePort As String
    Friend TCPSender As Class_InternetManager_TCPSender
    Friend WithEvents TCPReceiver As Class_InternetManager_TCPReceiver
    Sub New(ByVal _localMachineIP As String, ByVal _localMachineUsePort As String, Optional ByVal _timeOut As Integer = 1000)
        LocalMachineIP = _localMachineIP
        LocalMachineUsePort = _localMachineUsePort
        TCPSender = New Class_InternetManager_TCPSender()
        TCPReceiver = New Class_InternetManager_TCPReceiver(LocalMachineIP, LocalMachineUsePort, _timeOut)
        TCPReceiver.StartListening()
    End Sub

    Friend Function GetAndRemoveNextField(ByRef _dataToClip As String)
        Dim returnData As String
        If _dataToClip.IndexOf(":") = -1 Then
            returnData = _dataToClip
            _dataToClip = ""
            Return returnData
        End If
        returnData = Microsoft.VisualBasic.Left(_dataToClip, _dataToClip.IndexOf(":"))
        _dataToClip = Microsoft.VisualBasic.Right(_dataToClip, _dataToClip.Length - _dataToClip.IndexOf(":") - 1)
        Return returnData
    End Function
End Class
