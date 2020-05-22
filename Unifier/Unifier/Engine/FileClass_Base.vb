Public Class FileClass_Base
    Private vFileLocation As String
    Private vFileName As String
    Private vFileExtension As String
    Public Event FileLoadComplete()

    Sub New(ByVal _fileLocation As String, ByVal _fileName As String, Optional ByVal _fileExtension As String = ".txt")
        vFileLocation = _fileLocation
        vFileName = _fileName
        vFileExtension = _fileExtension
        DataHandler = New TagDataClass
        If My.Computer.FileSystem.FileExists(Path) Then ReadFileToMemory()
    End Sub

#Region "Private Code"
    'internal codes
    Private Function CopyFileToArray(ByVal _filePath As String, Optional _ignoreBlankLines As Boolean = True) As String()
        Dim vLines As New List(Of String)
        Dim sr As New IO.StreamReader(_filePath)
        Do Until sr.EndOfStream = True
            vLines.Add(sr.ReadLine())
        Loop
        sr.Close()
        Dim vLines_array(vLines.Count - 1) As String
        If _ignoreBlankLines Then
            vLines.RemoveAll(Function(str) String.IsNullOrWhiteSpace(str)) 'remove blank lines from the string list
        End If
        Return vLines.ToArray
    End Function
    Private Sub ReadFileToMemory()
        Dim vLines() As String = CopyFileToArray(Path)
        ''''
        Try
            'file has been read to variables, now parse those variables to sort the data into the entry class
            Dim eos As Integer
            Dim i As Integer
            Dim iTagStart As Integer
            Dim iTagEnd As Integer
            Dim tag As String
            'DataHandler = New TagDataClass
            Do Until i > vLines.Length - 1
                If ThisIsATagStarter(vLines(i)) Then 'proper tag start detected
                    iTagStart = i
                    tag = GetTagFromTagLine(vLines(i))
                    i += 1
                    Do Until i > vLines.Length - 1 Or ThisIsATagStarterOrEnder(vLines(i)) 'increment i while it is a tag's data
                        i += 1
                    Loop
                    If i > vLines.Length - 1 Then
                        'error this tag never ended
                    ElseIf ThisIsATagStarter(vLines(i)) Then
                        'error another tag has started
                    ElseIf ThisIsThisTagEnder(vLines(i), tag) = False Then
                        'error another tag has ended
                    Else
                        'no errors, the tag is complete
                        iTagEnd = i
                        '
                        ''add tag data to class here
                        '
                        i = iTagStart + 1 ' set's i to first data line
                        Do Until i = iTagEnd
                            DataHandler.AddDataToTag(tag) = vLines(i)
                            i += 1
                        Loop

                    End If
                Else 'improper tag start detected
                    If ThisIsATagEnder(vLines(i)) Then
                        'error tag ended without a tag opening
                    Else
                        'error data started without a tag opening
                    End If
                End If
                i += 1
                '
                '' restart loop for next tag here?
                '

                'below does not properly read tags...
                If 1 = 2 Then
                    If Left(vLines(i), 8) <> "[Start] " Then
                        'error the file does not start with a start tag
                    Else
                        iTagStart = i
                        Dim i2 As Integer = i 'marks start index of current tag
                        Dim TagName As String 'current tag's name
                        TagName = Right(vLines(i), vLines(i).Length - 8)
                        Do While vLines(i2) <> "[End] " & TagName
                            If Left(vLines(i2), 8) = "[Start] " Then
                                'error start of a new tag
                            End If
                            If Left(vLines(i2), 6) = "[End] " Then
                                'error end of a new tag
                            End If
                            If i2 >= vLines.Length Then
                                'tag doesn't end error?
                                'error i'm guessing since i2 continues where i left off, and i is not supposed to exceed end of file
                            End If
                            i2 += 1
                        Loop
                        Dim EntryType As String 'This is only for tags that weren't registered that are found in the file
                        If i2 > i + 2 Then
                            'this is a "Multiple Lines" entry type
                            EntryType = "Multiple Lines"
                        ElseIf i2 = i + 2 Then
                            EntryType = "Single Line"
                        ElseIf i2 = i + 1 Then
                            'the data section of the tag is empty or missing
                        Else
                            'error
                        End If
                        If i2 >= eos Then
                            'error no end for current tag
                        Else
                            'so the file itself is fully correct at this point i hear???
                            'end for current tag was found, so continue
                            'the below adds the tag and it's data to a DataEntry instance?

                            'below adds the read data to the by-now established tag
                            i += 1 'i at this point is still the tag's original start index, so [Start] whateverhere
                            Do Until vLines(i) = "[End] " & TagName
                                Try
                                    DataHandler.AddDataToTag(TagName) = vLines(i)
                                Catch ex As Exception
                                    '
                                End Try
                                i += 1
                            Loop
                            i += 1
                        End If
                    End If
                    i += 1
                End If ' if 1 = 2's end if
            Loop
            'file is separated into it's different data sections at this point
            ' vIsLoaded = True
            ' vFileChangeMade = False
            ' If UIElementsRegistered = True Then
            '    UpdateUIElementsByFile()
            ' End If
        Catch ex As Exception
            'error
        End Try
        ' For Each entry In vDataEntries
        '     entry.FileHasChanged_IsLocked = False
        ' Next
        ''''
        RaiseEvent FileLoadComplete()
    End Sub
    Private Sub CreateFolder(ByVal _folderPath As String)
        My.Computer.FileSystem.CreateDirectory(_folderPath)
    End Sub

    'quick/simplfying code
    Private Function ThisIsATagStarter(ByVal _lineToCheck As String) As Boolean
        If Left(_lineToCheck, 8) = "[Start] " Then Return True Else Return False
    End Function
    Private Function ThisIsATagEnder(ByVal _lineToCheck As String) As Boolean
        If Left(_lineToCheck, 6) = "[End] " Then Return True Else Return False
    End Function
    Private Function ThisIsATagStarterOrEnder(ByVal _lineToCheck As String) As Boolean
        If ThisIsATagStarter(_lineToCheck) Or ThisIsATagEnder(_lineToCheck) Then Return True Else Return False
    End Function
    Private Function ThisIsThisTagStarter(ByVal _tagStartToCheck As String, ByVal _tagToCheck As String) As Boolean
        If _tagStartToCheck = "[Start] " & _tagToCheck Then Return True Else Return False
    End Function
    Private Function ThisIsThisTagEnder(ByVal _tagEndToCheck As String, ByVal _tagToCheck As String) As Boolean
        If _tagEndToCheck = "[End] " & _tagToCheck Then Return True Else Return False
    End Function
    Private Function GetTagFromTagLine(ByVal _tagLineToGetTagFrom) As String
        If ThisIsATagStarter(_tagLineToGetTagFrom) Then
            Return Right(_tagLineToGetTagFrom, _tagLineToGetTagFrom.length - 8)
        ElseIf ThisIsATagEnder(_tagLineToGetTagFrom) Then
            Return Right(_tagLineToGetTagFrom, _tagLineToGetTagFrom.length - 6)
        Else
            Return ""
        End If
    End Function
#End Region
#Region "Public Code"
    'public, for higher use
    Public Overridable Sub SaveFile()
        Dim towrite As New List(Of String)
        Dim i As Integer
        Do Until i >= DataHandler.NumberOfTags()
            towrite.Add("[Start] " & DataHandler.GetTagByIndex(i))
            If TypeOf DataHandler.ReadTagsData(DataHandler.GetTagByIndex(i)) Is Array Then
                For Each dataline In DataHandler.ReadTagsData(DataHandler.GetTagByIndex(i))
                    towrite.Add(dataline)
                Next
            ElseIf TypeOf DataHandler.ReadTagsData(DataHandler.GetTagByIndex(i)) Is String Then
                towrite.Add(DataHandler.ReadTagsData(DataHandler.GetTagByIndex(i)))
            End If
            towrite.Add("[End] " & DataHandler.GetTagByIndex(i))
            i += 1
        Loop
        Dim sw As New IO.StreamWriter(Path)
        For Each line In towrite
            sw.WriteLine(line)
        Next
        sw.Close()
    End Sub
    Public Sub LoadFile()
        If FileExists Then ReadFileToMemory()
    End Sub
    Public Sub SaveAndReloadFile()
        SaveFile()
        LoadFile()
    End Sub

    'modifying file, interacts with subclass
    Public Sub AddTagToFile(ByVal _tagToAdd As String)
        DataHandler.AddTagToFile(_tagToAdd)
    End Sub
    Public WriteOnly Property UpdateDataInTag(ByVal _tagToUpdate As String)
        Set(value)
            DataHandler.UpdateDataInTag(_tagToUpdate) = value
        End Set
    End Property
    Public WriteOnly Property AddDataToTag(ByVal _dataToAdd As String)
        Set(value)
            DataHandler.AddDataToTag(_dataToAdd) = value
        End Set
    End Property
    Public WriteOnly Property RemoveDataFromTag(ByVal _dataToRemove As String)
        Set(value)
            DataHandler.RemoveDataFromTag(_dataToRemove) = value
        End Set
    End Property
    Public Sub ClearDataInTag(ByVal _tagToClear As String)
        DataHandler.ClearDataInTag(_tagToClear)
    End Sub
#End Region

#Region "ReadOnly Properties"
    Public ReadOnly Property Location As String
        Get
            Return vFileLocation
        End Get
    End Property
    Public ReadOnly Property Name As String
        Get
            Return vFileName
        End Get
    End Property
    Public ReadOnly Property Extension As String
        Get
            Return vFileExtension
        End Get
    End Property
    Public ReadOnly Property Path As String
        Get
            Return vFileLocation & vFileName & vFileExtension
        End Get
    End Property
    Public ReadOnly Property ListOfTags() As String()
        Get
            Return DataHandler.ListOfTags
        End Get
    End Property
    Public ReadOnly Property FileExists() As Boolean
        Get
            Return My.Computer.FileSystem.FileExists(Path)
        End Get
    End Property
    Public ReadOnly Property GetTagsData(ByVal _tagToReadFrom As String, Optional ByVal _lineNumberToRead As Integer = -1)
        Get
            Return DataHandler.ReadTagsData(_tagToReadFrom, _lineNumberToRead)
        End Get
    End Property
#End Region

    Public DataHandler As TagDataClass
    Class TagDataClass
        Private vDataName(-1) As String
        Private vDataType(-1) As String
        Private vDataLines(-1) As LineDataClass
        Public WriteOnly Property AddDataToTag(ByVal _tagToAddTo As String)
            Set(value)
                Dim i As Integer
                Dim i2 As Integer = AddTagToFile(_tagToAddTo) 'add param for returning index of tag instead of a fail error
                If i2 <> -1 Then i = i2
                If IsArray(value) Then
                    Do Until vDataName(i) = _tagToAddTo
                        i += 1
                    Loop
                    If vDataLines(i) Is Nothing Then
                        ReDim Preserve vDataLines(i)
                        vDataLines(i) = New LineDataClass
                    End If
                    For Each line In value
                        vDataLines(i).AddLine = line
                    Next
                Else
                    Do Until vDataName(i) = _tagToAddTo
                        i += 1
                    Loop
                    If vDataLines(i) Is Nothing Then
                        ReDim Preserve vDataLines(i)
                        vDataLines(i) = New LineDataClass
                    End If
                    vDataLines(i).AddLine = value
                End If
            End Set
        End Property
        Public WriteOnly Property RemoveDataFromTag(ByVal _tagToRemoveFrom As String)
            Set(value)
                Dim i As Integer
                Dim i2 As Integer = AddTagToFile(_tagToRemoveFrom) 'add param for returning index of tag instead of a fail error
                If i2 <> -1 Then i = i2
                If IsArray(value) Then
                    Do Until vDataName(i) = _tagToRemoveFrom
                        i += 1
                    Loop
                    For Each line In value
                        vDataLines(i).RemoveLine = line
                    Next
                Else
                    vDataLines(i).RemoveLine = value
                End If
            End Set
        End Property
        Public WriteOnly Property UpdateDataInTag(ByVal _tagToAddTo As String)
            Set(value)
                Dim i As Integer
                Dim i2 As Integer = AddTagToFile(_tagToAddTo) 'add param for returning index of tag instead of a fail error
                If i2 <> -1 Then i = i2
                If IsArray(value) Then
                    Do Until vDataName(i) = _tagToAddTo
                        i += 1
                    Loop

                    'ReDim Preserve vDataLines(i)
                    vDataLines(i) = New LineDataClass
                    vDataLines(i).UpdateLine = value
                Else
                    Do Until vDataName(i) = _tagToAddTo
                        i += 1
                    Loop
                    ' MsgBox(vDataLines.Length)
                    'ReDim Preserve vDataLines(i)
                    vDataLines(i) = New LineDataClass
                    vDataLines(i).AddLine() = value
                End If
            End Set
        End Property
        Public Sub ClearDataInTag(ByVal _tagToClearFrom As String)
            Dim i As Integer
            Dim i2 As Integer = AddTagToFile(_tagToClearFrom) 'add param for returning index of tag instead of a fail error
            If i2 <> -1 Then i = i2
            Do Until vDataName(i) = _tagToClearFrom
                i += 1
            Loop
            ' MsgBox(vDataLines.Length)
            'ReDim Preserve vDataLines(i)
            vDataLines(i) = New LineDataClass
            'vDataLines(i).AddLine() = value
        End Sub
        Public Function AddTagToFile(ByVal _tagToAdd As String) As Integer
            If vDataName.Contains(_tagToAdd) = False Then
                ReDim Preserve vDataName(vDataName.Length)
                ReDim Preserve vDataLines(vDataLines.Length)
                vDataName(vDataName.Length - 1) = _tagToAdd
                vDataLines(vDataLines.Length - 1) = New LineDataClass
                Return vDataName.Length - 1
            Else
                'tag already exists
                Return -1
            End If
        End Function
        Public ReadOnly Property ReadTagsData(ByVal _tagToReadFrom As String, Optional ByVal _lineNumberToRead As Integer = -1)
            Get
                If vDataName.Length > 0 Then
                    Dim i As Integer = 0
                    Do Until (i = vDataName.Length) Or (vDataName(i) = _tagToReadFrom)
                        i += 1
                        If i = vDataName.Length Then
                            If _lineNumberToRead = -1 Then
                                Dim _retarr(0) As String
                                _retarr(0) = ""
                                Return _retarr
                            Else
                                Return ""
                            End If
                        End If
                    Loop
                    If _lineNumberToRead = -1 Then
                        Return vDataLines(i).ReadLine()
                    Else
                        Return vDataLines(i).ReadLine(_lineNumberToRead)
                    End If
                Else
                    If _lineNumberToRead = -1 Then
                        Dim ea(-1) As String
                        Return ea
                    Else
                        Return ""
                    End If
                End If
            End Get
        End Property
        Public ReadOnly Property NumberOfTags() As Integer
            Get
                Return vDataName.Length
            End Get
        End Property
        Public ReadOnly Property ListOfTags() As String()
            Get
                Dim taglist As New List(Of String)
                For Each item As String In vDataName
                    taglist.Add(item)
                Next
                'If vDataName.Length = 0 Then taglist.Add("")
                Return taglist.ToArray
            End Get
        End Property
        Public ReadOnly Property GetTagByIndex(ByVal _tagIndex As Integer) As String
            Get
                Return vDataName(_tagIndex)
            End Get
        End Property

        Private Class LineDataClass
            Private Lines(-1) As String
            Public Initialized As Boolean = False
            Sub New()
                Initialized = True
            End Sub
            Public WriteOnly Property AddLine() As String
                Set(value As String)
                    ReDim Preserve Lines(Lines.Length)
                    Lines(Lines.Length - 1) = value
                End Set
            End Property
            Public WriteOnly Property RemoveLine() As String
                Set(value As String)
                    Dim newLines As New List(Of String)
                    Dim i As Integer = 0
                    For Each item In Lines
                        If item <> value Then
                            newLines.Add(item)
                        End If
                    Next
                    ReDim Lines(newLines.Count - 1)
                    Lines = newLines.ToArray
                End Set
            End Property
            Public WriteOnly Property UpdateLine() As String()
                Set(value As String())
                    Lines = value
                End Set
            End Property
            Public ReadOnly Property ReadLine(Optional ByVal _lineToRead As Integer = -1)
                Get
                    If _lineToRead = -1 Then
                        Return Lines
                    Else
                        Return Lines(_lineToRead)
                    End If
                End Get
            End Property
        End Class
    End Class
End Class
