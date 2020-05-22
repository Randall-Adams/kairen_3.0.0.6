Public Class Class_ManagedData_TaggedData
    Private vData(-1) As String
    Private vTag As String = Nothing
    Sub New()
    End Sub
    Sub New(ByVal Data() As String)
        vData = Data
    End Sub
    Sub New(ByVal Data As String)
        vData(0) = Data
    End Sub
    Public ReadOnly Property GetAllData As String()
        Get
            Return vData
        End Get
    End Property
    Public WriteOnly Property SetAllData As String()
        Set(value As String())
            vData = value
        End Set
    End Property
    Public ReadOnly Property GetSegmentofData(ByVal SegmentNumber As Integer) As String
        Get
            Return vData(SegmentNumber)
        End Get
    End Property
    Public WriteOnly Property SetSegmentofData(ByVal SegmentNumber As Integer) As String
        Set(value As String)
            vData(SegmentNumber) = value
        End Set
    End Property

    Public ReadOnly Property NumberOfSegments(Optional ByVal CountFromZero As Boolean = False) As Integer
        Get
            If CountFromZero Then
                Return vData.Length - 1
            Else
                Return vData.Length
            End If
        End Get
    End Property
End Class
