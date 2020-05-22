Public Class Class_DataBit
    Private vData(-1) As String
    Sub New()

    End Sub
    ''' <summary>
    ''' Performs the Add() action on the passed in string at initialization.
    ''' </summary>
    ''' <param name="FirstAdd">String Value to perform Add() on.</param>
    ''' <remarks>This code saves a line by integrating the Add() into the class's initialization.</remarks>
    Sub New(ByVal FirstAdd As String)
        Add = FirstAdd
    End Sub
    ''' <summary>
    ''' Performs the Add() action on the passed in string array at initialization.
    ''' </summary>
    ''' <param name="FirstAdd">String Array Value to perform Add() on.</param>
    ''' <remarks>This code saves a line by integrating the Add() into the class's initialization.</remarks>
    Sub New(ByVal FirstAdd As String())
        Add = FirstAdd
    End Sub
    Public WriteOnly Property Add()
        Set(value)
            If IsArray(value) Then
                Dim i As Integer = vData.Length
                ReDim Preserve vData((vData.Length + value.Length) - 1)
                For Each item In value
                    vData(i) = item
                    i += 1
                Next
            Else
                ReDim Preserve vData(vData.Length)
                vData(vData.Length - 1) = value
            End If

        End Set
    End Property
    Public WriteOnly Property SetData() As String()
        Set(value As String())
            ReDim vData(value.Length)
            vData = value
        End Set
    End Property
    Public WriteOnly Property ClearData() As String()
        Set(value As String())
            ReDim vData(-1)
        End Set
    End Property
End Class
