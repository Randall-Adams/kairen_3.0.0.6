Public Class Class_InternetManager_ChatProtocolServer
    Inherits Class_InternetManager_ProtocolBaseClass
    Private ClientHandler As ChatProtocolServerClient_Handler

    Sub New(ByVal _localMachineIP As String, ByVal _localMachineUsePort As String, Optional ByVal _timeOut As Integer = 1000)
        MyBase.New(_localMachineIP, _localMachineUsePort, _timeOut)
        ClientHandler = New ChatProtocolServerClient_Handler
    End Sub

    Public Event MessageReceived As EventHandler(Of MessageReceivedEventArgs)

    Public Class ChatProtocolServerClient
        Public ReadOnly IP As String
        Public ReadOnly Port As String
        Public CharacterName As String
        Public Sub New(ByVal _clientIP As String, ByVal _clientPort As String)
            IP = _clientIP
            Port = _clientPort
        End Sub
    End Class
    Public Class ChatProtocolServerClient_Handler
        Private Clients_Pending As List(Of ChatProtocolServerClient)
        Private Clients_Accepted As List(Of ChatProtocolServerClient)
        Public ReadOnly Property ClientList As List(Of ChatProtocolServerClient)
            Get
                Return Clients_Pending
            End Get
        End Property
        Sub New()
            Clients_Pending = New List(Of ChatProtocolServerClient)
            Clients_Accepted = New List(Of ChatProtocolServerClient)
        End Sub
        Public Function AttemptToAddPendingClient(ByVal _clientIP As String, ByVal _clientPort As String) As Boolean
            For Each pendingClient In Clients_Pending
                If pendingClient.IP = _clientIP Then
                    Return False
                End If
            Next
            For Each acceptedClient In Clients_Accepted
                If acceptedClient.IP = _clientIP Then
                    Return False
                End If
            Next

            Clients_Pending.Add(New ChatProtocolServerClient(_clientIP, _clientPort))
            Return True
        End Function
        Public Function IsPendingClient(ByVal _clientIP As String) As Boolean
            For Each pendingClient In Clients_Pending
                If pendingClient.IP = _clientIP Then
                    Return True
                End If
            Next
            Return False
        End Function
        Public Sub AcceptPendingClient(ByVal _clientIP As String, ByVal _clientCharacterName As String)
            If IsPendingClient(_clientIP) = True Then
                Dim UpgradingClient As ChatProtocolServerClient
                UpgradingClient = GetPendingClient(_clientIP)
                UpgradingClient.CharacterName = _clientCharacterName
                Clients_Pending.Remove(UpgradingClient)
                Clients_Accepted.Add(UpgradingClient)
            End If
        End Sub
        Private Function GetPendingClient(ByVal _clientIP As String)
            For Each pendingClient In Clients_Pending
                If pendingClient.IP = _clientIP Then
                    Return pendingClient
                End If
            Next
            Return Nothing
        End Function
        Public Function GetCharacterName(ByVal _clientIP As String) As String
            For Each acceptedClient In Clients_Accepted
                If acceptedClient.IP = _clientIP Then
                    Return acceptedClient.CharacterName
                End If
            Next
            Return Nothing
        End Function
    End Class
    Public Sub SendChatMessage(ByVal _ipToSendTo As String, ByVal _portToSendTo As String, ByVal _messageData As String)
        TCPSender.SendData(_ipToSendTo, _portToSendTo, PacketHeader() & _messageData)
    End Sub
    Public Sub SendServerChatMessage(ByVal _characterName As String, ByVal _channel As String, ByVal _messageToSend As String)
        RaiseEvent MessageReceived(Me, New MessageReceivedEventArgs(_characterName, _channel, _messageToSend))
        RelayChatMessageToClients("ChatMessagev1Input:" & _characterName & ":" & _channel & ":" & _messageToSend)
    End Sub
    Private Sub TCPReceiver_DataReceived(sender As Object, e As Class_InternetManager_TCPReceiver.DataReceivedEventArgs) Handles TCPReceiver.DataReceived
        Dim RawData, ProtocolIDString, SendersIPAddress, SendersUsingPort, MessageType As String
        RawData = e.ReceivedData
        ProtocolIDString = GetAndRemoveNextField(RawData)
        If ProtocolIDString = "KairenChatv1" Then
            SendersIPAddress = GetAndRemoveNextField(RawData)
            SendersUsingPort = GetAndRemoveNextField(RawData)
            If 1 = 1 Then ' if we deal with this ip address..
                MessageType = GetAndRemoveNextField(RawData)
                If MessageType = "ClientConnectionRequest" Then
                    If RawData = "" Then
                        If ClientHandler.AttemptToAddPendingClient(SendersIPAddress, SendersUsingPort) = True Then
                            SendLoginCredentialsRequestPacket(SendersIPAddress, SendersUsingPort)
                        End If
                    End If
                ElseIf MessageType = "LoginCredentials" Then
                    If ClientHandler.IsPendingClient(SendersIPAddress) Then
                        If 1 = 1 Then 'if login credentials are valid..
                            Dim clientCharacterName As String = GetAndRemoveNextField(RawData)
                            If RawData = "" Then
                                ClientHandler.AcceptPendingClient(SendersIPAddress, clientCharacterName)
                                SendLoginAcceptedPacket(SendersIPAddress, SendersUsingPort)
                            End If
                        End If
                    End If
                ElseIf MessageType = "ChatMessagev1Input" Then
                    Dim _senderCharacterName, _chatChannel, _chatMessage As String
                    _senderCharacterName = ClientHandler.GetCharacterName(SendersIPAddress)
                    _chatChannel = GetAndRemoveNextField(RawData)
                    _chatMessage = GetAndRemoveNextField(RawData)
                    If RawData = "" Then
                        RaiseEvent MessageReceived(Me, New MessageReceivedEventArgs(_senderCharacterName, _chatChannel, _chatMessage))
                        RelayChatMessageToClients(_senderCharacterName & _chatChannel & _chatMessage)
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub SendLoginCredentialsRequestPacket(ByVal _clientsIP As String, ByVal _usePort As String)
        TCPSender.SendData(_clientsIP, LocalMachineUsePort, PacketHeader() & "LoginCredentialRequest")
    End Sub
    Private Sub SendLoginAcceptedPacket(ByVal _clientsIP As String, ByVal _usePort As String)
        TCPSender.SendData(_clientsIP, LocalMachineUsePort, PacketHeader() & "LoginAccepted")
    End Sub
    Private Sub RelayChatMessageToClients(ByVal _message As String)
        If ClientHandler.ClientList IsNot Nothing Then
            For Each client In ClientHandler.ClientList
                SendChatMessage(client.IP, LocalMachineUsePort, _message)
            Next
        End If
    End Sub
    Private Function PacketHeader() As String
        Return "KairenChatv1:" & LocalMachineIP & ":" & LocalMachineUsePort & ":"
    End Function
End Class
