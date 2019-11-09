Imports System.Data.SqlClient
Imports System.Configuration
Imports Google.Apis.Auth
Imports Google.Apis.Download
Imports Google.Apis.Drive.v2
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Services
Imports System.Threading
Imports Google.Apis.Drive.v2.Data
Imports Google.Apis.Upload
Public Class googleDrive
    Dim service As New DriveService

    Private Sub createservice()
        Dim clientid = "464373743158-kqc8im0tcoh5u216m71lfad4qfd4ae6d.apps.googleusercontent.com"
        Dim clientsecret = "k4SjXcwmbgwKmSfBzbT97ImC"

        Dim uc As UserCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(New ClientSecrets() With {.ClientId = clientid, .ClientSecret = clientsecret}, {DriveService.Scope.Drive}, "user", CancellationToken.None).Result
        service = New DriveService(New BaseClientService.Initializer() With {.HttpClientInitializer = uc, .ApplicationName = "POS Technologies m"})
    End Sub

    Private Sub googleDrive_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FilePath.Select()
        FilePath.Text = ""
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FilePath.Select()
        Dim f As New OpenFileDialog
        If f.ShowDialog = DialogResult.OK Then
            FilePath.Text = f.FileName
        Else
            Exit Sub
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FilePath.Select()
        If service.ApplicationName <> "POS Technologies m" Then createservice()
        Dim myfile As New File
        Dim bytearry As Byte() = System.IO.File.ReadAllBytes(FilePath.Text)
        Dim stream As New System.IO.MemoryStream(bytearry)
        Dim uploadrequest As FilesResource.InsertMediaUpload = service.Files.Insert(myfile, stream, myfile.MimeType)
        uploadrequest.Upload()
        Dim file As File = uploadrequest.ResponseBody
        MessageBox.Show("Upload Successful. File ID is " + file.Id)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        FilePath.Select()
        Close()
    End Sub
End Class