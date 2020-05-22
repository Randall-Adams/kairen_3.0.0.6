Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Public Class SpecificLibrary
    Public ReadOnly Kairen_Current_Version_BASE As String
    Public ReadOnly Kairen_Current_Version_FULL As String
    Public ReadOnly Kairen_Current_Version_TYPE As String = "RHO" 'DELTA 'RHO 'PHI 'TAU 'GAMMA -dev'r rele prog'er tester preview
    Public ReadOnly Kairen_Current_Version_TYPENUMBER As String = "SE" 'zIltcH uNI SEc TrI qUAD fIvE
    'TES = ME IH, KAIREN CHAT UI, THING MAKERS SE,
    'REL = FULL IH, NI, NO ONLINE SE,

    Public Path As PathClassforKairen
    Public AccountFile As FileClass_AccountFile
    Public UniversalOptions As FileClass_Base
    Public ChatBoxOverlayObject As Form_ChatBoxOverlay
    Public WorldFile As FileClass_WorldFile
    Public OldNPCFile(-1) As FileClass_OldFormatFile_NPCFile
    Sub New(ByVal _kairenCurrentVersion As String)
        Kairen_Current_Version_BASE = _kairenCurrentVersion
        Kairen_Current_Version_FULL = _kairenCurrentVersion
        If Kairen_Current_Version_TYPE <> "RHO" Then Kairen_Current_Version_FULL += "-" & Kairen_Current_Version_TYPE & Kairen_Current_Version_TYPENUMBER
        Path = New PathClassforKairen(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\Kairen\")
        UniversalOptions = New FileClass_Base(Path.Accounts.Location, "UniversalOptions")
        UniversalOptions.LoadFile()
    End Sub
    Public Function ReadLineFromFileOnline(ByVal _urlToCheck As String) As String
        'this should always be ran in a secondary thready
        Try
            Dim rt As String
            Dim wRequest As System.Net.WebRequest
            Dim wResponse As System.Net.WebResponse
            Dim SR As System.IO.StreamReader
            wRequest = System.Net.WebRequest.Create(_urlToCheck)
            wResponse = wRequest.GetResponse
            SR = New System.IO.StreamReader(wResponse.GetResponseStream)
            rt = SR.ReadToEnd
            SR.Close()
            rt = CStr(rt.Trim)
            Return rt
        Catch ex As Exception
            Return "error"
        End Try
    End Function
    Public Function TryIPGet() As String
        Try
            Dim rt As String
            Dim wRequest As WebRequest
            Dim wResponse As WebResponse
            Dim SR As IO.StreamReader
            wRequest = WebRequest.Create("https://www.ip-adress.com/")
            wResponse = wRequest.GetResponse
            SR = New StreamReader(wResponse.GetResponseStream)
            rt = SR.ReadToEnd
            SR.Close()
            rt = Mid(rt, rt.IndexOf("Your IP address is: <strong>") + 1)
            rt = Microsoft.VisualBasic.Left(rt, rt.IndexOf("</strong></h1>"))
            rt = Microsoft.VisualBasic.Right(rt, rt.IndexOf("<strong>") - 6)
            Return rt
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Private Sub GetAllIPsOnLocalMachine()
        Dim listy As List(Of String)
        For Each netInterface In System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
            listy.Add("Name: " + netInterface.Name)
            listy.Add("Description: " + netInterface.Description)
            listy.Add("Addresses: ")
            Dim ipProps As System.Net.NetworkInformation.IPInterfaceProperties = netInterface.GetIPProperties()
            For Each addr In ipProps.UnicastAddresses
                listy.Add(" " + addr.Address.ToString())
            Next
            listy.Add("")
        Next
    End Sub 'displays all ips on machine
End Class
