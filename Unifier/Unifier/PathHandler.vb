Public Class PathHandler
    Public RootFolderLocation As String
    Public RootFolderName As String
    Public RootFolderPath As String
    Sub New(ByVal _rootFolderName As String, Optional ByVal _rootFolderLocation As String = Nothing)
        If _rootFolderLocation = Nothing Then _rootFolderLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
        RootFolderName = _rootFolderName
        RootFolderLocation = _rootFolderLocation
        RootFolderPath = RootFolderLocation & "\" & RootFolderName & "\"
    End Sub
    Public WriteOnly Property AddReference() As String
        Set(value As String)

        End Set
    End Property
    Public Class PathThing
        Public typeFolder As Folder
        Public typeFile As File
        Public typeExtension As Extension
        Private ThingName As String
        Private ThingLocation As String
        Private ThingExtension As String
        Private ThingTypeIsh As String
        Sub New(ByVal _thingName As String, ByVal _thingLocation As String, ByVal _thingType As Folder)
            ThingName = _thingName
            ThingLocation = _thingLocation
            ThingTypeIsh = "Folder"
        End Sub
        Sub New(ByVal _thingName As String, ByVal _thingLocation As String, ByVal _thingType As File)
            ThingName = _thingName
            ThingLocation = _thingLocation
            ThingTypeIsh = "File"
        End Sub
        Sub New(ByVal _thingName As String, ByVal _thingLocation As String, ByVal _thingType As Extension)
            ThingName = _thingName
            ThingLocation = _thingLocation
            ThingTypeIsh = "Extension"
        End Sub

        Public ReadOnly Property Path() As String
            Get
                Return ThingLocation & "\" & ThingName & "\"
            End Get
        End Property
        Public ReadOnly Property Name() As String
            Get
                Return ThingName
            End Get
        End Property
        Public ReadOnly Property Location() As String
            Get
                Return ThingLocation
            End Get
        End Property
        Public ReadOnly Property Type()
            Get
                Select Case ThingTypeIsh
                    Case "File"
                        Return typeFile
                    Case "Folder"
                        Return typeFolder
                    Case "Extension"
                        Return typeExtension
                    Case Else
                        Return Nothing
                End Select
            End Get
        End Property

        Public Class Folder
        End Class
        Public Class File
        End Class
        Public Class Extension
        End Class
    End Class
    Private Class FolderObject

    End Class
    Private Class FileObject

    End Class
End Class
