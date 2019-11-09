Imports System.IO
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text
Public Class EmailSetup
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager
    Dim READER As MySqlDataReader

    Private Function Decrypt(cipherText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
         &H65, &H64, &H76, &H65, &H64, &H65,
         &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function

    Private Function Encrypt(clearText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
         &H65, &H64, &H76, &H65, &H64, &H65,
         &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText
    End Function

    Sub saveData()
        Try
            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "UPDATE parameters SET email = '" & txtEmail.Text & "', emailpassword = '" & Encrypt(Trim(txtEmailPassword.Text)) & "', port = '" & txtPort.Text & "' WHERE ID = '1'"
            dr = cmd.ExecuteReader
            cn.Close()
            MessageBox.Show("Settings have been updated.", "Email Settings", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Edit User", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label4.Select()
        Close()

        For Each ChildForm As Form In MainInterface.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Label4.Select()
        saveData()
    End Sub

    Sub LoadDB()
        Try
            cn.Open()
            Dim itm As String = "SELECT * FROM parameters"
            cmd = New MySqlCommand(itm, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtEmail.Text = dr.GetString("email").ToString()
                txtEmailPassword.Text = Decrypt(dr.GetString("emailpassword").ToString())
                txtPort.Text = dr.GetString("port").ToString()
            End While
            cn.Close()
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Users", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtEmail.Clear()
            txtEmailPassword.Clear()
            txtPort.Clear()
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim Itemexist As Char
    Private Sub EmailSetup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label4.Select()
        Try
            cn.Open()
            Dim Query As String
            Query = "SELECT email FROM parameters"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                Itemexist = "Y"
            Else
                Itemexist = "N"
            End If
            cn.Close()

            If Itemexist = "Y" Then
                LoadDB()
            ElseIf Itemexist = "N" Then
                txtEmail.Clear()
                txtEmailPassword.Clear()
                txtPort.Clear()
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtEmail.Clear()
            txtEmailPassword.Clear()
            txtPort.Clear()
        Finally
            cn.Dispose()
        End Try
    End Sub
End Class