Public Class Class_InternetManager_ChatProtocolClient
    Inherits Class_InternetManager_ProtocolBaseClass
    Public ReadOnly RemoteMachineIP As String
    Public ReadOnly RemoteMachineUsePort As String
    Sub New(ByVal _localMachineIP As String, ByVal _localMachineUsePort As String, ByVal _remoteMachineIP As String, ByVal _remoteMachineUsePort As String, Optional ByVal _timeOut As Integer = 1000)
        MyBase.New(_localMachineIP, _localMachineUsePort, _timeOut)
        RemoteMachineIP = _remoteMachineIP
        RemoteMachineUsePort = _remoteMachineUsePort
        SendConnectionRequestToServer()
    End Sub

    Public Event MessageReceived As EventHandler(Of MessageReceivedEventArgs)

    Public Sub SendChatMessage(ByVal _ipToSendTo As String, ByVal _portToSendTo As String, ByVal _messageToSend As String)
        'edit _message before send so it's a chat message
        TCPSender.SendData(_ipToSendTo, _portToSendTo, PacketHeader() & _messageToSend)
    End Sub
    Private Sub TCPReceiver_DataReceived(sender As Object, e As Class_InternetManager_TCPReceiver.DataReceivedEventArgs) Handles TCPReceiver.DataReceived
        Dim RawData, ProtocolIDString, SendersIPAddress, SendersUsingPort, MessageType As String
        RawData = e.ReceivedData
        ProtocolIDString = GetAndRemoveNextField(RawData)
        If ProtocolIDString = "KairenChatv1" Then
            MessageType = GetAndRemoveNextField(RawData)
            SendersIPAddress = GetAndRemoveNextField(RawData)
            SendersUsingPort = GetAndRemoveNextField(RawData)
            If 1 = 1 Then 'if sender ip is the server ip...
                If MessageType = "LoginCredentialRequest" Then
                    SendLoginCredentialsPacketToServer()
                ElseIf MessageType = "LoginAccepted" Then

                ElseIf MessageType = "ChatMessagev1Output" Then
                    Dim _senderCharacterName, _chatChannel, _chatMessage As String
                    _senderCharacterName = GetAndRemoveNextField(RawData)
                    _chatChannel = GetAndRemoveNextField(RawData)
                    _chatMessage = GetAndRemoveNextField(RawData)
                    If RawData = "" Then
                        RaiseEvent MessageReceived(Me, New MessageReceivedEventArgs(_senderCharacterName, _chatChannel, _chatMessage))
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub SendConnectionRequestToServer()
        Dim ConnectionRequestData As String
        ConnectionRequestData = PacketHeader() & "ClientConnectionRequest"
        'client says it wants to connect
        'server ignores or acks
        'server acks -> asks for username and password
        'client sends username and password
        'server ignores or acks
        'server acks -> confirms or gives error
        'server confirms -> gives login success message. starts relaying data to client. end of login procedure.
        TCPSender.SendData(LocalMachineIP, LocalMachineUsePort, ConnectionRequestData)
    End Sub
    Private Sub SendLoginCredentialsPacketToServer()
        Dim CredentialData As String
        CredentialData = PacketHeader() & "LoginCredentials:" & "CLIENT"
        TCPSender.SendData(LocalMachineIP, LocalMachineUsePort, CredentialData)
    End Sub
    Private Function PacketHeader() As String
        Return "KairenChatv1:" & RemoteMachineIP & ":" & RemoteMachineUsePort & ":"
    End Function
End Class
