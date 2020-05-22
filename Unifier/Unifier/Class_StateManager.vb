Public Class Class_StateManager
    Private CL As CommonLibrary
    Private SL As SpecificLibrary
    Private Setup As SetupMonitor
    Public Sub New(ByRef _commonLibrary As CommonLibrary, ByRef _specificLibrary As SpecificLibrary)
        CL = _commonLibrary
        SL = _specificLibrary
    End Sub

    Class SetupMonitor
        Sub New()

        End Sub
        Private Sub Check()

        End Sub
    End Class
End Class
