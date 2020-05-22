    Imports System.Net   'Web
Imports System.IO    'Files
Public Class TestForm_FTPUploader
    Private Sub TestForm_FTPUploader_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        po()
    End Sub
    Sub po()
        MsgBox(Microsoft.VisualBasic.Right(TextBox3.Text, TextBox3.Text.Length - 1 - TextBox3.Text.LastIndexOf("\")))
        ' Get the object used to communicate with the server.
        'Dim request As FtpWebRequest = CType(WebRequest.Create(TextBox5.Text), FtpWebRequest)
        Dim request As FtpWebRequest = CType(WebRequest.Create(TextBox5.Text & "/" & Microsoft.VisualBasic.Left(TextBox3.Text, TextBox3.Text.LastIndexOf("\"))), FtpWebRequest)
        request.Method = WebRequestMethods.Ftp.UploadFile
        ' This example assumes the FTP site uses anonymous logon.
        request.Credentials = New NetworkCredential(TextBox1.Text, TextBox2.Text)
        ' Copy the contents of the file to the request stream.

        Dim sourceStream As StreamReader = New StreamReader(TextBox3.Text)
        Dim fileContents() As Byte ' = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd)
        sourceStream.Close()
        request.ContentLength = fileContents.Length
        Dim requestStream As Stream = request.GetRequestStream
        requestStream.Write(fileContents, 0, fileContents.Length)
        requestStream.Close()
        Dim response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
        MsgBox("Upload File Complete, status {0}", MsgBoxStyle.OkOnly, response.StatusDescription)
        response.Close()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        OpenFileDialog1.Reset()
        OpenFileDialog1.Title = "Select File to Upload"
        OpenFileDialog1.ShowDialog()
        TextBox3.Text = OpenFileDialog1.FileName
    End Sub
    Sub upload()
        'Upload File to FTP site
        'Create Request To Upload File'
        Dim wrUpload As FtpWebRequest = DirectCast(WebRequest.Create("ftp://ftp.test.com/file.txt"), FtpWebRequest)

        'Specify Username & Password'
        wrUpload.Credentials = New NetworkCredential("user", "password")

        'Start Upload Process'
        wrUpload.Method = WebRequestMethods.Ftp.UploadFile

        'Locate File And Store It In Byte Array'
        Dim btfile() As Byte = File.ReadAllBytes("c:\file.txt")

        'Get File'
        Dim strFile As Stream = wrUpload.GetRequestStream()

        'Upload Each Byte'
        strFile.Write(btfile, 0, btfile.Length)

        'Close'
        strFile.Close()

        'Free Memory'
        strFile.Dispose()
    End Sub
    Sub download()
        'Download A File From FTP Site'

        'Create Request To Download File'
        Dim wrDownload As FtpWebRequest = _
           WebRequest.Create("ftp://ftp.test.com/file.txt")

        'Specify That You Want To Download A File'
        wrDownload.Method = WebRequestMethods.Ftp.DownloadFile

        'Specify Username & Password'
        wrDownload.Credentials = New NetworkCredential("user", "password")

        'Response Object'
        Dim rDownloadResponse As FtpWebResponse = _
           wrDownload.GetResponse()

        'Incoming File Stream'
        Dim strFileStream As Stream = _
           rDownloadResponse.GetResponseStream()

        'Read File Stream Data'
        Dim srFile As StreamReader = New StreamReader(strFileStream)

        Console.WriteLine(srFile.ReadToEnd())

        'Show Status Of Download'
        Console.WriteLine("Download Complete, status {0}", rDownloadResponse.StatusDescription)

        srFile.Close() 'Close

        rDownloadResponse.Close()


        'Delete File On FTP Server'

        'Create Request To Delete File'
        Dim wrDelete As FtpWebRequest = CType(WebRequest.Create("ftp://ftp.test.com/file.txt"), FtpWebRequest)

        'Specify That You Want To Delete A File'
        wrDelete.Method = WebRequestMethods.Ftp.DeleteFile

        'Response Object'
        Dim rDeleteResponse As FtpWebResponse = CType(wrDelete.GetResponse(), FtpWebResponse)

        'Show Status Of Delete'
        Console.WriteLine("Delete status: {0}", 
           rDeleteResponse.StatusDescription)

        'Close'
        rDeleteResponse.Close()

    End Sub
    Sub rename()
        'rename File On FTP Server'

        'Create Request To Rename File'
        Dim wrRename As System.Net.FtpWebRequest = _
           CType(FtpWebRequest.Create("ftp://ftp.test.com/file.txt"), FtpWebRequest)

        'Specify Username & Password'
        wrRename.Credentials = New NetworkCredential("user", "password")

        'Rename A File'
        wrRename.Method = WebRequestMethods.Ftp.Rename

        wrRename.RenameTo() = "TEST.TXT"

        'Determine Response Of Operation'
        Dim rResponse As System.Net.FtpWebResponse

        Try
            rResponse = CType(wrRename.GetResponse, FtpWebResponse)

            'Get Description'
            Dim strStatusDesc As String = rResponse.StatusDescription

            'Get Code'
            Dim strStatusCode As FtpStatusCode = rResponse.StatusCode

            If strStatusCode <> Net.FtpStatusCode.FileActionOK Then

                MessageBox.Show("Rename failed.  Returned status = " & strStatusCode & " " & strStatusDesc)

            Else

                MessageBox.Show("Rename succeeded")

            End If

        Catch ex As Exception

            MessageBox.Show("Rename failed. " & ex.Message)


        End Try
    End Sub
End Class
