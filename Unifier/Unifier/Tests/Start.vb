'Imports System.Windows.Forms
Public Module Start
    Public Sub Main()
        Dim po As New Threading.Thread(AddressOf Application.Run)
        po.Start()
        MsgBox("Press okay and I should close in like a few seconds...")
        Threading.Thread.Sleep(3000)
        Application.Exit()
        'Do
        'Threading.Thread.Sleep(1)
        'Loop
    End Sub
End Module
