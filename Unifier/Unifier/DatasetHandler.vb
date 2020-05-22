Public Class DatasetHandler
    'a file class, or anything, will inherit this to easily store and retrieve data via naming that data.
    'that data should be kept in order as it is given to the class, or somehow given an index order if that is relevant.
    ' ..or figure out where that is relevent to..
    'Subdata will have names, the entire class is the overall Data, so this class is to break that data down and handle it individually.
    'the fileclass class will inherit this class to handle it's data.
    'this class should be useable by more things though, like maybe checkedlistboxes?
    Private vNames(-1) As String
    Private vDataBit(-1) As Class_DataBit
    Sub New()

    End Sub
    ''' <summary>
    ''' Performs the AddData() action on the passed in data at initialization.
    ''' </summary>
    ''' <param name="FirstAddName">String Value that is the name of the held data.</param>
    ''' <param name="FirstAddData">String Value that is the data to be held.</param>
    ''' <remarks>This code saves a line by integrating the AddData() into the class's initialization.</remarks>
    Sub New(ByVal FirstAddName As String, ByVal FirstAddData As String)
        AddData(FirstAddName) = FirstAddData
    End Sub
    Public WriteOnly Property AddData(ByVal DataToAddTo As String) As String
        Set(value As String)
            Dim i As Integer = 0
            Do Until i = vNames.Length
                If vNames(i) = DataToAddTo Then
                    vDataBit(i).Add = value
                    Exit Property
                End If
                i += 1
            Loop
            ReDim vNames(i)
            vNames(i) = DataToAddTo
            ReDim vDataBit(i)
            vDataBit(i) = New Class_DataBit(value)
        End Set
    End Property
    Public WriteOnly Property SetData(ByVal DataToSetTo As String) As String()
        Set(value As String())
            Dim i As Integer = 0
            Do Until i = vNames.Length
                If vNames(i) = DataToSetTo Then
                    vDataBit(i).SetData = value
                    Exit Property
                End If
                i += 1
            Loop
            ReDim vNames(i)
            vNames(i) = DataToSetTo
            ReDim vDataBit(i)
            vDataBit(i) = New Class_DataBit(value)
        End Set
    End Property
End Class
