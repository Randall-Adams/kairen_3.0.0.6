Public Class MessageReceivedEventArgs
    Inherits EventArgs
    Public ReadOnly SenderCharacterName As String
    Public ReadOnly ChatChannel As String
    Public ReadOnly ChatMessage As String
    Public Sub New(ByVal _senderCharacterName As String, ByVal _chatChannel As String, ByVal _chatMessage As String) 'add sender name and other data here
        ' TODO: Insert constructor code here
        SenderCharacterName = _senderCharacterName
        ChatChannel = _chatChannel
        ChatMessage = _chatMessage
    End Sub
End Class ' Event argument class for the DataReceived event