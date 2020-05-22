Public Class Class_InternetManager
    'Project Globals
    Public CL As CommonLibrary
    Public SL As SpecificLibrary

    'Form Globals
    Private vPublicIP As String
    Public ReadOnly PrivateIP As String = "0.0.0.0" '"192.168.1.9"
    Public ReadOnly KairenPort As Integer = 51649
    Sub New(ByRef _commonLibrary As CommonLibrary, ByRef _specificLibrary As SpecificLibrary)
        CL = _commonLibrary
        SL = _specificLibrary
        UpdatePublicIP()
    End Sub
    Public ReadOnly Property PublicIP As String
        Get
            Return vPublicIP
        End Get
    End Property
    Public Sub UpdatePublicIP()
        vPublicIP = SL.TryIPGet
    End Sub
#Region "Chat Client/Server Code"
    Public ReadOnly KairenChatPort As Integer = 52649
    'Chat Client Code
    Private WithEvents ChatProtocolClient As Class_InternetManager_ChatProtocolClient
    Public Event ChatProtocolClient_MessageReceived_Event(ByVal MessageData As String)
    Public Sub StartChatClient(ByVal _theServerIPToConnectTo As String)
        ChatProtocolClient = New Class_InternetManager_ChatProtocolClient(PublicIP, KairenChatPort, _theServerIPToConnectTo, KairenChatPort)
    End Sub
    Public Sub SendChatMessage(ByVal _senderCharacterName As String, ByVal _chatChannel As String, ByVal _chatMessage As String)
        _senderCharacterName = _senderCharacterName.Replace(":", "")
        _chatChannel = _chatChannel.Replace(":", "")
        _chatMessage = _chatMessage.Replace(":", "")
        Dim msg As String = _senderCharacterName & ":" & _chatChannel & ":" & _chatMessage
        If ChatProtocolServer IsNot Nothing Then
            ChatProtocolServer.SendServerChatMessage(_senderCharacterName, _chatChannel, _chatMessage)
        ElseIf ChatProtocolClient IsNot Nothing Then
            ChatProtocolClient.SendChatMessage(PublicIP, KairenChatPort, msg)
        End If
    End Sub
    Private Sub ChatProtocolClient_MessageReceived(ByVal sender As Object, ByVal e As MessageReceivedEventArgs) Handles ChatProtocolClient.MessageReceived, ChatProtocolServer.MessageReceived
        'properly handle e's data after it's updated
        RaiseEvent ChatProtocolClient_MessageReceived_Event(e.SenderCharacterName & " said on the channel " & e.ChatChannel & ": " & e.ChatMessage)
    End Sub
    'Chat Server Code
    Private WithEvents ChatProtocolServer As Class_InternetManager_ChatProtocolServer
    Public Event ChatProtocolServer_MessageReceived_Event(ByVal MessageData As String)
    Public Sub StartChatServer(ByVal _ipToHostServerOn As String)
        ChatProtocolServer = New Class_InternetManager_ChatProtocolServer(_ipToHostServerOn, KairenChatPort)
    End Sub
#End Region

#Region "Game Client/Server Code"
    Public ReadOnly KairenGamePort As Integer = 50649
    'Game Client Code
    Private WithEvents GameProtocolClient As Class_InternetManager_ChatProtocolClient
    Public Event GameProtocolClient_MessageReceived_Event(ByVal MessageData As String)
    Public Sub StartGameClient(ByVal _theServerIPToConnectTo As String)
        GameProtocolClient = New Class_InternetManager_ChatProtocolClient(PublicIP, KairenChatPort, _theServerIPToConnectTo, KairenChatPort)
    End Sub
    Public Sub SendGameMessage(ByVal _senderCharacterName As String, ByVal _chatChannel As String, ByVal _chatMessage As String)
        _senderCharacterName = _senderCharacterName.Replace(":", "")
        _chatChannel = _chatChannel.Replace(":", "")
        _chatMessage = _chatMessage.Replace(":", "")
        Dim msg As String = _senderCharacterName & ":" & _chatChannel & ":" & _chatMessage
        If GameProtocolClient IsNot Nothing Then
            GameProtocolServer.SendServerChatMessage(_senderCharacterName, _chatChannel, _chatMessage)
        ElseIf GameProtocolClient IsNot Nothing Then
            GameProtocolClient.SendChatMessage(PublicIP, KairenChatPort, msg)
        End If
    End Sub
    Private Sub GameProtocolClient_MessageReceived(ByVal sender As Object, ByVal e As MessageReceivedEventArgs) Handles ChatProtocolClient.MessageReceived, ChatProtocolServer.MessageReceived
        'properly handle e's data after it's updated
        RaiseEvent GameProtocolClient_MessageReceived_Event(e.SenderCharacterName & " said on the channel " & e.ChatChannel & ": " & e.ChatMessage)
    End Sub
    'Game Server Code
    Private WithEvents GameProtocolServer As Class_InternetManager_ChatProtocolServer
    Public Event GameProtocolServer_MessageReceived_Event(ByVal MessageData As String)
    Public Sub StartGameServer(ByVal _ipToHostServerOn As String)
        GameProtocolServer = New Class_InternetManager_ChatProtocolServer(_ipToHostServerOn, KairenChatPort)
    End Sub
#End Region
End Class
