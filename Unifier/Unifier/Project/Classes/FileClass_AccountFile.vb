Public Class FileClass_AccountFile
    Inherits FileClass_Base
    Private LockedBy As New List(Of Object)
    Sub New(ByVal _location As String, ByVal _name As String)
        MyBase.New(_location, _name)
    End Sub
    Public WriteOnly Property Lock()
        Set(value)
            If LockedBy.Contains(value) = False Then LockedBy.Add(value)
        End Set
    End Property
    Public WriteOnly Property Unlock()
        Set(value)
            If LockedBy.Contains(value) Then LockedBy.Remove(value)
        End Set
    End Property
    Public ReadOnly Property IsLocked As Boolean
        Get
            If LockedBy.Count = 0 Then Return False Else Return True
        End Get
    End Property
End Class
