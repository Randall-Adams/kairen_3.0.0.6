Public Class FAPIConnectionManager2
    Private lb As CommonLibrary
    Public o_FAPIData As FAPIFileClass 'input from Kanizah.lua
    Public ObjectsToUpdate As New List(Of Object) 'Forms To Update

    Public i_FAPIDataRequests As FAPIFileClass 'output into Kanizah.lua, requesting data to be sent
    Private Requests(-1) As FAPIConnectionRequestClass
    Private PendingRequests(-1) As String
    Private PendingRequests_Type(-1) As Integer
    Private PendingRequests_OneTime(-1) As String
    Private PendingRequests_Type_OneTime(-1) As Integer
    Private SentRequests(-1) As String
    Private SentRequests_OneTime(-1) As String

    Public i_FAPIData_Unreliable As FAPIFileClass 'output into Kanizah.lua, supplying it with updated values
    Private FAPIDataOutputQueue_Unreliable(-1) As String
    Private FAPIDataOutputQueue_UnreliableAdditionalData(-1) As String

    'the type of the fapi data request is known by the sender, otherwise it is only discoverable by looking up the number
    'temp stuff
    Dim Extension_ReadWrites As String = ".txt"
    Dim Folder_Net_Streams As String = ".txt"

    'create events so forms can be alerted by the global fapi manager that it is doing different things
    Sub New(ByRef _lb As CommonLibrary)
        lb = _lb
    End Sub
    Public Sub UpdateFAPIDisplayObjects()
        'SpoofFapiFile()
        'SendFAPIDataRequest() ' should this actually go here??
        SendFAPICommands()
        If My.Computer.FileSystem.FileExists(Folder_Net_Streams & "o\FAPI Data2" & Extension_ReadWrites) = False Then 'check if there is an update
            'old'lb.DisplayMessage("No FAPI Data2 file found. Was it created?", "Error:", "FAPIConnectionManager2") 'if no update display and exit
            'if no updated file then just don't update yet
            Exit Sub
        End If
        o_FAPIData = New FAPIFileClass(Folder_Net_Streams & "o\FAPI Data2" & Extension_ReadWrites, Nothing, True) 'dim the file
        o_FAPIData.LoadFile() 'load it into memory for manipulation
        Do While o_FAPIData.CurrentIndex <= o_FAPIData.NumberOfLines And o_FAPIData.NumberOfLines > 0
            o_FAPIData.ReadLine() 'read the line data
            o_FAPIData.AdditionalData(o_FAPIData.CurrentIndex) = o_FAPIData.ReadLine() 'set the additional data for the previous readline equal to the next readline
        Loop

        For Each myform In ObjectsToUpdate
            Try
                myform.UpdateFAPIDisplay()
            Catch ex As Exception
                ' this fires incorrectly because it fires when myform has a problem with it's sub, not just when it doesn't have one
                ' orrrr my error message is wrong actually haha
                '    lb.DisplayMessage("Error: " & myform.text & " does not have an UpdateFAPIDisplay() procedure.", "Programmer Error:", "FAPIConnectionManager2")
            End Try
        Next
        o_FAPIData.DeleteFile()
    End Sub 'tealls each form to update it's fapi data, reads the fapi data file

    Public Sub RequestFAPIData(ByVal RequestName As String, Optional ByVal type As Integer = 1)
        For Each request In PendingRequests
            If request = RequestName Then
                request = Nothing
                Exit Sub
            End If
        Next
        For Each request In SentRequests
            If request = RequestName Then
                Exit Sub
            End If
        Next
        Dim i As Integer = PendingRequests.Length
        ReDim Preserve PendingRequests(i)
        ReDim Preserve PendingRequests_Type(i)
        PendingRequests(i) = RequestName
        PendingRequests_Type(i) = type
    End Sub 'queues requests for fapi data from kanizah
    Public Sub SendFAPIDataRequest() 'sends the queued fapi requests to kanizah
        If PendingRequests.Length <= 0 Then Exit Sub
        If My.Computer.FileSystem.FileExists(Folder_Net_Streams & "i\FAPI Data Request" & Extension_ReadWrites) = True Then 'check if there is a pending request
            'put request into queue instead, then exit.
            Exit Sub
        End If
        i_FAPIDataRequests = New FAPIFileClass(Folder_Net_Streams & "i\FAPI Data Request" & Extension_ReadWrites, "--", True) 'dim the file
        i_FAPIDataRequests.LoadFile() 'load it into memory for manipulation
        ReDim Preserve SentRequests(PendingRequests.Length - 1)
        Dim i As Integer
        For Each request In PendingRequests
            'process queue before processing current request
            Select Case PendingRequests_Type(i)
                Case 1
                    i_FAPIDataRequests.WriteLine("AddOutputByAddressName") 'write the request command
                    i_FAPIDataRequests.WriteLine(request) 'write the request data, which is an Address.lua value
                Case 2
                    i_FAPIDataRequests.WriteLine("UpdateVariableByVariableName") 'write the request command
                    i_FAPIDataRequests.WriteLine(request) 'write the request data, which is an Address.lua value
                Case 3
                    i_FAPIDataRequests.WriteLine("PrintToConsole") 'write the request command
                    i_FAPIDataRequests.WriteLine(request) 'write the request data, which is what should be output to the lua console
                Case Else
                    MsgBox("Error - SendFAPIDataRequest: Invalid Request Attempt Type: " & PendingRequests_Type(i))
            End Select
            SentRequests(i) = request
            i = i + 1
        Next
        i_FAPIDataRequests.SaveFile()
    End Sub 'writes the request file

    Public Sub QueueFAPIOutput_Unreliable(ByVal _data As String, ByVal _additionalData As String) 'unreliably sends kanizah updated fapi data
        Dim i As Integer
        If FAPIDataOutputQueue_UnreliableAdditionalData.Contains(_additionalData) = False Then
            i = FAPIDataOutputQueue_Unreliable.Length
            ReDim Preserve FAPIDataOutputQueue_Unreliable(i)
            ReDim Preserve FAPIDataOutputQueue_UnreliableAdditionalData(i)
        Else
            i = Array.IndexOf(FAPIDataOutputQueue_UnreliableAdditionalData, _additionalData)
        End If
        i = i
        FAPIDataOutputQueue_Unreliable(i) = _data
        FAPIDataOutputQueue_UnreliableAdditionalData(i) = _additionalData
    End Sub
    Public Sub SendFAPIOutput_Unreliable() 'unreliably sends kanizah updated fapi data
        'text file needs dimd
        'make a function that will add addtional data to the file, and another function that will update the additonal data for the passed variable
        'then a function that saves the file for the fapi to take in when it can.
        'maybe make an unreliable and a reliable file, one for things that can be skipped and one for things that can't
        i_FAPIData_Unreliable = New FAPIFileClass(Folder_Net_Streams & "i\FDi_Unreliable.txt", "--")
        Dim i As Integer = 0
        i_FAPIData_Unreliable.WriteLine("UpdateVariableByVariableName")
        Do While i <= FAPIDataOutputQueue_UnreliableAdditionalData.Length - 1
            i_FAPIData_Unreliable.WriteLine(FAPIDataOutputQueue_Unreliable(i))
            i_FAPIData_Unreliable.WriteLine(FAPIDataOutputQueue_UnreliableAdditionalData(i))
            i = i + 1
        Loop
        i_FAPIData_Unreliable.SaveFile()
        ReDim FAPIDataOutputQueue_Unreliable(-1)
        ReDim FAPIDataOutputQueue_UnreliableAdditionalData(-1)
    End Sub

    Public Sub IssueFAPICommand(ByVal RequestType As Integer, ByVal RequestData As String)
        'If RequestType = Nothing then Exit Sub
        If RequestData = Nothing Then Exit Sub
        IssueFAPICommand_Real(RequestType, RequestData, Nothing)
    End Sub
    Public Sub IssueFAPICommand(ByVal RequestType As Integer, ByVal RequestDataArray() As String)
        'If RequestType = Nothing then Exit Sub
        If RequestDataArray Is Nothing Then Exit Sub
        IssueFAPICommand_Real(RequestType, Nothing, RequestDataArray)
    End Sub
    Public Sub IssueFAPICommand(ByVal RequestType As Integer, ByVal RequestData As String, ByVal RequestDataArray() As String)
        'If RequestType = Nothing then Exit Sub
        If RequestData = Nothing Then Exit Sub
        If RequestDataArray Is Nothing Then Exit Sub
        IssueFAPICommand_Real(RequestType, RequestData, RequestDataArray)
    End Sub
    Public Sub IssueFAPICommand(ByVal RequestType As Integer, ByVal RequestDataArray() As String, ByVal RequestData As String)
        'If RequestType = Nothing then Exit Sub
        If RequestDataArray Is Nothing Then Exit Sub
        If RequestData = Nothing Then Exit Sub
        IssueFAPICommand_Real(RequestType, RequestData, RequestDataArray)
    End Sub
    Public Sub IssueFAPICommand(ByVal RequestType As Integer, ByVal RequestData1 As String, ByVal RequestData2 As String)
        'If RequestType = Nothing then Exit Sub
        If RequestData1 = Nothing Then Exit Sub
        If RequestData2 = Nothing Then Exit Sub
        Dim RequestDataArray(1) As String
        RequestDataArray(0) = RequestData1
        RequestDataArray(1) = RequestData2
        IssueFAPICommand_Real(RequestType, Nothing, RequestDataArray)
    End Sub
    Private Sub IssueFAPICommand_Real(ByVal RequestType As Integer, ByVal RequestData As String, ByVal RequestDataArray() As String)
        'the requesttype numbers appear to only exist to be able to change each command type in the future, passes english names to the lua, not a number
        Select Case RequestType
            Case 1, 2, 3, 6, 7 ' not arrays
                'these don't get resent, so their names help to identify if they've been sent
                For Each _request In Requests
                    If _request.RequestData = RequestData Then
                        'request already created
                        Exit Sub
                    End If
                Next
                Dim i As Integer = Requests.Length
                ReDim Preserve Requests(i)
                Requests(i) = New FAPIConnectionRequestClass(RequestType, RequestData, Nothing)
            Case 4 'arrays
                'these get sent multiple times, if they have been sent, then redim the old request in memory and fill it once more
                '                               if they have not been sent, then just exit, because at this time the code has the data as read only
                For Each _request In Requests
                    If _request.RequestData_Array(0) = RequestDataArray(0) Then
                        If _request.Sent = True Then
                            _request = New FAPIConnectionRequestClass(RequestType, Nothing, RequestDataArray)
                            Exit Sub
                        Else
                            'request already pending
                            Exit Sub
                        End If
                    End If
                Next
                ReDim Preserve Requests(Requests.Length)
                Requests(Requests.Length - 1) = New FAPIConnectionRequestClass(RequestType, Nothing, RequestDataArray)
            Case 5, 8 'arrays
                'these don't get resent, so their names help to identify if they've been sent
                For Each _request In Requests
                    If _request.RequestData_Array(0) = RequestDataArray(0) Then
                        If _request.Sent = True Then
                            '_request = New FAPIConnectionRequestClass(RequestType, RequestData, RequestDataArray)
                            Exit Sub
                        Else
                            'request already pending
                            Exit Sub
                        End If
                    End If
                Next
                ReDim Preserve Requests(Requests.Length)
                Requests(Requests.Length - 1) = New FAPIConnectionRequestClass(RequestType, Nothing, RequestDataArray)
        End Select
    End Sub

    Private Sub SendFAPICommands()
        If Requests.Length = -1 Then Exit Sub
        If My.Computer.FileSystem.FileExists(Folder_Net_Streams & "i\FAPI Data Request" & Extension_ReadWrites) = True Then 'check if there is a pending request
            'put request into queue instead, then exit.
            Exit Sub
        End If
        Dim exitmarker As Boolean = True
        For Each _request In Requests
            If _request.Sent = False Then
                exitmarker = False
                Exit For
            End If
        Next
        If exitmarker = True Then Exit Sub
        i_FAPIDataRequests = New FAPIFileClass(Folder_Net_Streams & "i\FAPI Data Request" & Extension_ReadWrites, "--", True) 'dim the file
        i_FAPIDataRequests.LoadFile() 'load it into memory for manipulation
        For Each _request In Requests
            If _request.Sent = False Then
                'process queue before processing current request
                Select Case _request.Type
                    Case 1
                        i_FAPIDataRequests.WriteLine("AddOutputByAddressName") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the request data, which is an Address.lua value
                    Case 2
                        i_FAPIDataRequests.WriteLine("UpdateVariableByVariableName") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the request data, which is an Address.lua value
                    Case 3
                        i_FAPIDataRequests.WriteLine("PrintToConsole") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the request data, which is what should be output to the lua console
                    Case 4
                        i_FAPIDataRequests.WriteLine("UpdateAddressByAddressName") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(0)) 'write the address name
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(1)) 'write the address value
                    Case 5
                        i_FAPIDataRequests.WriteLine("UpdatePlayerData") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(0)) 'write the address value name
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(1)) 'write the address value x
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(2)) 'write the address value y
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(3)) 'write the address value z
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(4)) 'write the address value f
                    Case 6
                        i_FAPIDataRequests.WriteLine("SpawnNPC") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the npc name
                    Case 7
                        i_FAPIDataRequests.WriteLine("DespawnNPC") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the npc name
                    Case 8
                        i_FAPIDataRequests.WriteLine("NPCMessage1") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the npc name
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the npc message

                    Case Else
                        MsgBox("Error - SendFAPIDataRequest: Invalid Request Attempt Type: " & _request.Type)
                End Select
                _request.Sent = True
            End If
        Next
        i_FAPIDataRequests.SaveFile()
    End Sub

#Region "FAPIFile"
    Private Sub LoadFAPIFile()
        If My.Computer.FileSystem.FileExists(Folder_Net_Streams & "o\FAPI Data2" & Extension_ReadWrites) = False Then
            'lb.DisplayMessage("No FAPI Data2 file found. Was it created?", "Error:", "FAPIConnectionManager2")
            Exit Sub
        End If
        o_FAPIData = New FAPIFileClass(Folder_Net_Streams & "o\FAPI Data2" & Extension_ReadWrites, "--", True)
        o_FAPIData.LoadFile()
        o_FAPIData.AdditionalData(o_FAPIData.CurrentIndex) = "MyX"
        o_FAPIData.ReadLine()
        o_FAPIData.AdditionalData(o_FAPIData.CurrentIndex) = "MyY"
        o_FAPIData.ReadLine()
        o_FAPIData.AdditionalData(o_FAPIData.CurrentIndex) = "MyZ"
        o_FAPIData.ReadLine()
        'd_yd_tb_X.Text = GrabData_NPC.ReadLine
        'd_yd_tb_Y.Text = GrabData_NPC.ReadLine
        'd_yd_tb_Z.Text = GrabData_NPC.ReadLine
        'd_yd_tb_F.Text = GrabData_NPC.ReadLine
        'TextBox10.Text = GrabData_NPC.ReadLine
        'TextBox14.Text = GrabData_NPC.ReadLine 'columns rows 10\14
        'TextBox11.Text = GrabData_NPC.ReadLine
        'TextBox15.Text = GrabData_NPC.ReadLine 'nest 11\15
        'TextBox5.Text = GrabData_NPC.ReadLine
        'd_yd_tb_ZoneFull.Text = GrabData_NPC.ReadLine
        'd_yd_tb_ZoneName.Text = GrabData_NPC.ReadLine
        'd_yd_tb_ZoneSub.Text = GrabData_NPC.ReadLine
        'If My.Computer.FileSystem.FileExists(lb.Folder_Temp_NPC_Maker & "New_NPC.txt") Then
        '    My.Computer.FileSystem.DeleteFile(lb.Folder_Temp_NPC_Maker & "New_NPC.txt")
        'End If
        'GrabData_NPC = Nothing
    End Sub
#End Region
#Region "FAPI File Class"
    Class FAPIFileClass

        Private vLines() As String
        Private vIsComment() As Boolean ' Not a Property
        Private vFilePath As String
        Private vCommentMarker As String
        Private vindex_ReadFile As Integer = -1
        Private vIOMode As String
        Private vAdditionalData(0) As String

        Private vvTag(-1, -1) As String '(y=0 tag, y=1 tag's index)
        Private vvControlList(-1, -1) As Object '(y=0 tag's index, y=1 control)
        Private vDataParserFunctionSet() As Boolean

        Private vTag(-1) As String
        Private vControlList(-1) As Object
        Private vDataParser(-1) As DataParserDelegate
        Delegate Function DataParserDelegate(ByVal _additionaldata As String, ByRef _control As Object, ByVal _text As String)

        Private vIsLoaded As Boolean = False
        Private AllowFileDeletion As Boolean = False
        Private NewLine = vbNewLine

        Public Sub New(ByVal _filepath As String, ByVal _commentMarker As String, Optional ByVal WithDeletes As Boolean = False, Optional AutoLoad As Boolean = False)
            vFilePath = _filepath
            If _commentMarker <> Nothing Then
                vCommentMarker = _commentMarker
            End If
            AllowFileDeletion = WithDeletes
            If AutoLoad = True Then
                LoadFile()
            End If
        End Sub

#Region "Properties"
        Public Property Line(ByVal i As Integer) As String
            Get
                If vLines(i) <> Nothing Then
                    Return vLines(i)
                Else
                    'out of bounds error
                    Return Nothing
                End If
            End Get
            Set(ByVal value As String)
                'If i < 0 Then Exit Property
                If vLines.Length < i Then
                    ReDim Preserve vLines(i)
                End If
                vLines(i) = value
            End Set
        End Property
        Public ReadOnly Property Line() As String()
            Get
                Return vLines
            End Get
        End Property
        Public Property FilePath() As String
            Get
                Return vFilePath
            End Get
            Set(value As String)
                vFilePath = value
            End Set
        End Property
        Public Property CommentMarker(ByVal line As String) As String
            Get
                Return vCommentMarker
            End Get
            Set(value As String)
                vCommentMarker = value
            End Set
        End Property
        Public Property CurrentIndex As Integer
            Get
                Return vindex_ReadFile
            End Get
            Set(value As Integer)
                vindex_ReadFile = value
            End Set
        End Property
        Public Property AdditionalData(ByVal i As Integer) As String
            Get
                Return vAdditionalData(i)
            End Get
            Set(value As String)
                If vAdditionalData.Length <= i Then
                    ReDim Preserve vAdditionalData(i)
                End If
                vAdditionalData(i) = value
            End Set
        End Property
        Public ReadOnly Property AdditionalData() As String()
            Get
                Return vAdditionalData
            End Get
        End Property
        Public ReadOnly Property IsLoaded() As Boolean
            Get
                Return vIsLoaded
            End Get
        End Property
        Private Sub RegisterUIElement0(ByVal _tag As String, ByRef _control As Windows.Forms.Control)
            Dim _tagindex As Integer
            Dim x As Integer = vvTag.GetLength(0)
            Do While _tagindex < x
                If vvTag(_tagindex, 0) = _tag Then
                    'this is the tag we are adding a control to
                    Dim _controlindex As Integer
                    Do While _controlindex <= vvControlList.GetLength(1)
                        If vvControlList(vvTag(_tagindex, 1), _controlindex) Is _control Then
                            'this is the control we are adding to the list, so exit
                            Exit Sub
                        End If
                        _controlindex = _controlindex + 1
                    Loop
                    'control is not in the tag's list, so add it
                    ReDim Preserve vvControlList(vvTag(_tagindex, 1), _controlindex)
                    vvControlList(vvTag(_tagindex, 1), _controlindex) = _control
                    Exit Sub
                End If
                _tagindex = _tagindex + 1
            Loop
            'create tag
            ReDim Preserve vvTag(_tagindex, 1)
            vvTag(_tagindex, 0) = _tag
            x = vvTag.GetLength(0) - 1
            vvTag(_tagindex, 1) = x ' make the vTag value equal to the index of the tag's index in the vControlList
            ReDim Preserve vvControlList(vvTag(_tagindex, 1), 1)
            vvControlList(vvTag(_tagindex, 1), 0) = vvTag(_tagindex, 1)
            vvControlList(vvTag(_tagindex, 1), 1) = _control
            Exit Sub
            x = vvControlList.GetLength(0) - 1
        End Sub
        Private Sub UpdateUIElements0()
            'updates the ui when the file loads and stuff
            Dim _tagindex As Integer
            'Dim _controlindex As Integer
            '        vControlList(vTag(_tagindex, 1), 1) = GetValueByAdditionalData(vTag(_tagindex, _controlindex))
            Dim x As Integer = vvTag.GetLength(0)
            Do While _tagindex < x
                'this is the tag we are adding a control to
                Dim _controlindex As Integer
                Do While _controlindex < vvControlList.GetLength(1) - 1
                    Dim poo As String = GetValueByAdditionalData(vvTag(_tagindex, _controlindex))
                    vvControlList(vvTag(_tagindex, 1), 1).Text = poo
                    _controlindex = _controlindex + 1
                Loop
                _tagindex = _tagindex + 1
            Loop
        End Sub
        Private Sub RegisterUIElement1(ByVal _tag As String, ByRef _control As Windows.Forms.Control)
            Dim _tagindex As Integer
            Dim x As Integer = vvTag.GetLength(1)
            Do While _tagindex < x
                If vvTag(0, _tagindex) = _tag Then
                    'this is the tag we are adding a control to
                    Dim _controlindex As Integer
                    Do While _controlindex <= vvControlList.GetLength(0)
                        If vvControlList(_controlindex, vvTag(1, _tagindex)) Is _control Then
                            'this is the control we are adding to the list, so exit
                            Exit Sub
                        End If
                        _controlindex = _controlindex + 1
                    Loop
                    'control is not in the tag's list, so add it
                    ReDim Preserve vvControlList(_controlindex, vvTag(1, _tagindex))
                    vvControlList(_controlindex, vvTag(1, _tagindex)) = _control
                    Exit Sub
                End If
                _tagindex = _tagindex + 1
            Loop
            'create tag
            ReDim Preserve vvTag(1, _tagindex)
            vvTag(0, _tagindex) = _tag
            x = vvTag.GetLength(1) - 1
            vvTag(1, _tagindex) = x ' make the vTag value equal to the index of the tag's index in the vControlList
            ReDim Preserve vvControlList(1, vvTag(1, _tagindex))
            vvControlList(0, vvTag(1, _tagindex)) = vvTag(1, _tagindex)
            vvControlList(1, vvTag(1, _tagindex)) = _control
            Exit Sub
            x = vvControlList.GetLength(1) - 1
        End Sub
        Private Sub UpdateUIElements1()
            'updates the ui when the file loads and stuff
            Dim _tagindex As Integer
            'Dim _controlindex As Integer
            '        vControlList(vTag(_tagindex, 1), 1) = GetValueByAdditionalData(vTag(_tagindex, _controlindex))
            Dim x As Integer = vvTag.GetLength(1)
            Do While _tagindex < x
                Dim _controlindex As Integer
                Do While _controlindex < vvControlList.GetLength(0) - 1
                    Dim poo As String = GetValueByAdditionalData(vvTag(_controlindex, _tagindex))
                    vvControlList(1, vvTag(1, _tagindex)).Text = poo
                    _controlindex = _controlindex + 1
                Loop
                _tagindex = _tagindex + 1
            Loop
        End Sub
        Public Sub RegisterUIElement(ByVal _tag As String, ByRef _control As Windows.Forms.Control, Optional _dataParser As DataParserDelegate = Nothing, Optional ByVal OverWrite As Boolean = False)
            If _tag.Trim = "" Then Exit Sub
            If _control Is Nothing Then Exit Sub
            Dim i As Integer
            Do Until i = vTag.Length
                If vTag(i) = _tag Then
                    'this is the tag we are registering, it's already registered so..
                    If OverWrite = False Then
                        'tag already registered, overwrite flag is off, so exit
                        Exit Sub
                    End If
                    'over flag's old contol with new one
                    vControlList(i) = _control
                    vDataParser(i) = _dataParser
                    Exit Sub
                End If
                i += 1
            Loop
            'tag was not found, so let's register it now
            ReDim Preserve vTag(i)
            vTag(i) = _tag
            ReDim Preserve vControlList(i)
            vControlList(i) = _control
            ReDim Preserve vDataParser(i)
            vDataParser(i) = _dataParser
        End Sub
        Public Sub UpdateUIElements()
            Dim i As Integer
            Do Until i = vTag.Length
                GetValueByAdditionalData(vTag(i), vControlList(i), vDataParser(i))
                i += 1
            Loop
        End Sub
        Public Sub UpdateFileByUIElements()
            'updates the file based off the data in the UI elements
            Dim i As Integer
            Do Until i = vTag.Length
                UpdateValueByAdditionalData(vTag(i), vControlList(i))
                i += 1
            Loop
            SaveFile()
        End Sub
#End Region

#Region "Functions"
        Public Sub SaveFile()
            Dim sw As New IO.StreamWriter(vFilePath)
            sw.NewLine = ""
            If vLines.Length = Nothing Then Exit Sub
            Dim lastLineMarker As Integer = vLines.Length
            Dim i As Integer = 0
            For Each _line In vLines
                If IsNothing(_line) = False Then
                    If _line.Trim <> "" Then
                        sw.WriteLine(_line)
                        If i < (lastLineMarker - 1) And _line <> "" Then
                            sw.WriteLine(vbCrLf)
                        End If
                    End If
                End If
                i = i + 1
            Next
            sw.Close()
        End Sub
        Public Sub LoadFile()
            If My.Computer.FileSystem.FileExists(vFilePath) = False Then Exit Sub
            Try
                Dim sr As New IO.StreamReader(vFilePath)
                Dim i As Integer = 0
                Do Until sr.EndOfStream()
                    ReDim Preserve vLines(i)
                    ReDim Preserve vIsComment(i)
                    vLines(i) = sr.ReadLine ' Reads file's line to variable
                    If vLines(i).Trim <> "" Then 'if the line is blank, don't read it and skip over it
                        If vCommentMarker <> Nothing Then
                            If Left(vLines(i), vCommentMarker.Length) = vCommentMarker Then ' Check if Line is a Comment
                                ' Line is a Comment
                                vIsComment(i) = True
                            Else
                                ' Line is Not a Comment
                                vIsComment(i) = False
                            End If
                        Else
                            ' Line is Not a Comment
                            vIsComment(i) = False
                        End If
                        i = i + 1
                    End If
                Loop
                sr.Close()
                vIsLoaded = True
            Catch ex As Exception

            End Try
        End Sub
        Public Function ReadLine()
            'idk if i tried this before, but what if i pass in the additional data for the present readline?
            'If vindex_ReadFile = -1 Then
            '    Return Nothing
            'End If
            vindex_ReadFile = vindex_ReadFile + 1 ' increase index to next line..
            'If vLines.Length = Nothing Then Return Nothing 'there are no lines to return
            If vLines Is Nothing Then Return Nothing
            If vindex_ReadFile >= vLines.Length Then Return Nothing 'if you are trying to read past the end of file then return nothing..
            Dim _line As String = vLines(vindex_ReadFile) ' read new index line..
            'If _line = "" Then Return Nothing ' if line is blank, don't read it and return nothing'
            Do While vLines(vindex_ReadFile) <> Nothing
                If vCommentMarker = Nothing Then
                    Exit Do
                Else
                    If Left(_line, vCommentMarker.Length) = vCommentMarker Then ' if it's a comment..
                        vindex_ReadFile = vindex_ReadFile + 1 ' increase index..
                        If vindex_ReadFile >= vLines.Length Then Return Nothing
                        _line = vLines(vindex_ReadFile) ' read line at new index..
                    Else
                        Exit Do
                    End If

                End If
            Loop ' then skip the comment and attempt to loop again..

            ' when not a comment or when no more lines..
            If vLines(vindex_ReadFile) = Nothing Then Return Nothing ' if no more lines..
            Return vLines(vindex_ReadFile) ' if not a comment..
        End Function
        Public Sub WriteLine(ByVal _newline As String)
            If _newline = Nothing Then Exit Sub
            If _newline.Trim = "" Then Exit Sub
            vindex_ReadFile = vindex_ReadFile + 1 ' increase index to next line..
            ReDim Preserve vLines(vindex_ReadFile)
            vLines(vindex_ReadFile) = _newline ' replace old line with the new one..
        End Sub
        Public Sub WriteAllLines(ByVal _newlines() As String)
            vLines = _newlines
        End Sub
        Public Sub UpdateValueByAdditionalData(ByVal _text As String, ByVal _newvalue As Object)
            If TypeOf (_newvalue) Is String Then
            ElseIf TypeOf (_newvalue) Is TextBox Then
                _newvalue = _newvalue.Text
            End If
            If _newvalue = "1" Then _newvalue = "true" Else If _newvalue = "0" Then _newvalue = "false"
            Dim i As Integer = 0
            Try
                Do Until vAdditionalData(i) = _text
                    i = i + 1
                    If i >= AdditionalData.Length Then Exit Sub
                Loop
            Catch ex As Exception
                MsgBox("An error occured in TextFileClass.UpdateValueByText(). Please give the data from the next window to the developer, as this error is completely unprogrammed for." & NewLine & _
                       "(" & _text & ", " & _newvalue & ")", MsgBoxStyle.Critical, "Error!")
                InputBox("This is the data the developer needs. Thank you, and I am sorry for the inconvenience.", "Error Help", ex.Message)
                Exit Sub
            End Try
            vLines(i) = _newvalue
        End Sub
        'Public Function GetValueByAdditionalData(ByVal _text As String, Optional ByRef _objectToSetTo As Object = Nothing, Optional _dataParser As DataParserDelegate = Nothing)
        Public Function GetValueByAdditionalData(ByVal _text As String, Optional ByRef _objectToSetTo As Object = Nothing, Optional _dataParser As DataParserDelegate = Nothing)
            'this could be altered to return an error/success code only, and only set data to sent in object
            Dim i As Integer = 0
            Dim returnValue As Object
            Dim errorRaised As Boolean = False
            Try
                Do Until (vAdditionalData.Length = i) Or (vAdditionalData(i) = _text)
                    i = i + 1
                    If vAdditionalData.Length = i Then
                        'additional data not found
                        errorRaised = True
                        returnValue = -1
                        Exit Do
                    End If
                Loop
            Catch ex As Exception
                MsgBox("An error occured in TextFileClass.GetValueByText(). Please give the data from the next window to the developer, as this error has nev." & NewLine & _
                       "(" & _text & ")", MsgBoxStyle.Critical, "Error - GetValueByText()")
                InputBox("This is the data the developer needs. Thank you, and I am sorry for the inconvenience.", "Error Help - GetValueByText()", ex.Message)
                errorRaised = True
                returnValue = -2
            End Try
            If errorRaised = True Then
                Return returnValue
            Else
                returnValue = vLines(i)
                If _objectToSetTo IsNot Nothing Then
                    If _dataParser IsNot Nothing Then
                        _dataParser.Invoke(_text, _objectToSetTo, vLines(i))
                    Else
                        Dim _objectType As String = TypeName(_objectToSetTo)
                        Select Case _objectType
                            Case "TextBox", "Label"
                                If _dataParser IsNot Nothing Then
                                    _objectToSetTo.Text = _dataParser.Invoke(_text, _objectToSetTo, vLines(i))
                                Else
                                    _objectToSetTo.Text = vLines(i)
                                End If
                            Case "CheckBox"
                                If vLines(i) = "true" Then
                                    _objectToSetTo.Checked = True
                                ElseIf vLines(i) = "false" Then
                                    _objectToSetTo.Checked = False
                                Else
                                    'error or false, idk yet ..
                                End If
                            Case "CheckedListBox"
                                Dim i2 As Integer
                                Do Until i2 = _objectToSetTo.Items.Count - 1
                                    If _objectToSetTo.Items.Item(i2) = _text Then
                                        If _dataParser IsNot Nothing Then
                                            _objectToSetTo.Text = _dataParser.Invoke(_text, _objectToSetTo, vLines(i))
                                        Else
                                            If vLines(i2) = "true" Then
                                                _objectToSetTo.Items.Item(i2).Checked = True
                                            ElseIf vLines(i2) = "false" Then
                                                _objectToSetTo.Items.Item(i2).Checked = False
                                            Else
                                                'error or false, i2dk yet ..
                                            End If
                                        End If
                                        Exit Do
                                    End If
                                    i2 += 1
                                Loop
                        End Select
                    End If
                End If
                Return returnValue
            End If
        End Function
        Public Function GetIndexByAdditionalData(ByVal _text As String)
            Dim i As Integer = 0
            Try
                Do Until (vAdditionalData.Length = i) Or (vAdditionalData(i) = _text)
                    i = i + 1
                    If vAdditionalData.Length = i Then
                        'additional data not found
                        Return -1
                    End If
                Loop
                'Do While i < NumberOfLines() And i <= vAdditionalData.Length
                '    If vAdditionalData(i) = _text Then
                '         Return i
                '    End If
                '    i = i + 1
                'Loop
            Catch ex As Exception
                MsgBox("An error occured in TextFileClass.GetIndexByText(). Please give the data from the next window to the developer, as this error is completely unprogrammed for." & NewLine & _
                      "(" & _text & ")", MsgBoxStyle.Critical, "Error - GetValueByText()")
                InputBox("This is the data the developer needs. Thank you, and I am sorry for the inconvenience.", "Error Help - GetIndexByText()", ex.Message)
                Return -2
            End Try

            Return i
        End Function
        Public Function GetIndexByValue(ByVal _value As String, Optional ByVal StartIndex As Integer = 0, Optional ByVal EndIndex As Integer = 0)
            If StartIndex < 0 Then Return StartIndex
            If EndIndex < 0 Then Return EndIndex
            'reads the start index and the end index
            Dim i As Integer
            For Each _line In vLines
                If i >= StartIndex Then
                    If _line = _value Then
                        Return i
                    ElseIf EndIndex <> 0 And i > EndIndex Then
                        Return -1
                    End If
                End If
                i = i + 1
            Next
            Return -1
        End Function
        Public Function FileExists() As Boolean
            Return My.Computer.FileSystem.FileExists(vFilePath)
        End Function

        Public Function NumberOfLines()
            If vLines Is Nothing Then Return 0
            Return vLines.Length
        End Function
        Sub DeleteFile()
            Try
                If AllowFileDeletion = False Then
                    My.Computer.FileSystem.DeleteFile(vFilePath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                ElseIf AllowFileDeletion = True Then
                    My.Computer.FileSystem.DeleteFile(vFilePath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
                End If
            Catch ex As Exception

            End Try
        End Sub
#End Region
        Dim TheQuestion As QuestionParserDelegate
        Delegate Function QuestionParserDelegate(ByVal _additionaldata As String, ByRef _control As Object)
        Public Sub GetAnswer()
            MsgBox("Your answer was: " & TheQuestion(InputBox(TheQuestion("What is the question?", Nothing), "The Question is:", ""), Nothing), MsgBoxStyle.OkOnly, "The Result of Your Answer is:")
        End Sub
        Public Sub SetQuestionAndAnswer(Optional _theQuestion As QuestionParserDelegate = Nothing)
            TheQuestion = _theQuestion
        End Sub
    End Class
#End Region
End Class
