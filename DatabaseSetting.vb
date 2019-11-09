Imports System.IO
Imports System.Runtime.InteropServices

Public Class DatabaseSetting

    Private Sub Button23_Click(sender As Object, e As EventArgs)

    End Sub

    Dim plainText As String
    Sub files()
        Try
            Dim data As New Database
            With data
                'Get the value from C:\Deluxe16\BOMI.ini
                Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
                Using sr As New System.IO.StreamReader(appPath + "\Settings.ini")
                    Dim Line As String = sr.ReadLine
                    Dim Line1 As String = sr.ReadLine
                    Dim Line2 As String = sr.ReadLine
                    Dim Line3 As String = sr.ReadLine
                    Dim Line4 As String = sr.ReadLine
                    Dim Line5 As String = sr.ReadLine
                    Dim Line6 As String = sr.ReadLine
                    Dim Line7 As String = sr.ReadLine
                    Dim Line8 As String = sr.ReadLine

                    'DatabaseSetup.txt_pwd.Text = Line2.Substring(11)

                    Dim wrapper As New Simple3Des("12345")
                    plainText = wrapper.DecryptData(Line2.Substring(11))
                End Using
            End With
        Catch ex As Exception
        End Try
    End Sub

    Sub save()
        Dim data As New Database
        With data
            'Assing the object property values
            .ServerName = txt_server.Text
            .DatabaseName = txt_database.Text
            .UserID = txt_uid.Text
            .Password = txt_pwd.Text
            .Port = txt_port.Text
            Try
                Dim wrapper As New Simple3Des("12345")
                Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
                Using sw As New System.IO.StreamWriter(appPath + "\Settings.ini", False)
                    sw.WriteLine("Server   =>" + txt_server.Text)
                    sw.WriteLine("User     =>" + txt_uid.Text)
                    Dim cipherText As String = "Password =>" + wrapper.EncryptData(txt_pwd.Text)
                    sw.WriteLine(cipherText)
                    sw.WriteLine("Database =>" + txt_database.Text)
                    sw.WriteLine("Port Num =>" + txt_port.Text)
                End Using

                MsgBox("This program will now shut down for the changes to take effect.", MsgBoxStyle.Information, "Database Setup")
                Application.ExitThread()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End With
    End Sub

    <DllImport("Gdi32.dll", EntryPoint:="CreateRoundRectRgn")>
    Private Shared Function CreateRoundRectRgn(ByVal iLeft As Integer, ByVal iTop As Integer, ByVal iRight As Integer, ByVal iBottom As Integer, ByVal iWidth As Integer, ByVal iHeight As Integer) As IntPtr
    End Function

    Private Sub DatabaseSetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label10.Select()

        Label2.AutoSize = False
        Label2.Padding = New Padding(1, 1, 1, 1)
        Label2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label2.Width - 2, Label2.Height - 2, 5, 1))

        ShowLogin()
        txt_database.Text = OpenCompany.Label1.Text
    End Sub

    Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
    Sub ShowLogin()
        Try
            If System.IO.File.Exists(appPath + "\Settings.ini") Then
                Dim data As New Database
                With data
                    'Get the value from C:\Deluxe16\BOMI.ini
                    Using sr As New System.IO.StreamReader(appPath + "\Settings.ini")
                        Dim Line As String = sr.ReadLine
                        Dim Line1 As String = sr.ReadLine
                        Dim Line2 As String = sr.ReadLine
                        Dim Line3 As String = sr.ReadLine
                        Dim Line4 As String = sr.ReadLine
                        Dim Line5 As String = sr.ReadLine
                        Dim Line6 As String = sr.ReadLine
                        Dim Line7 As String = sr.ReadLine
                        Dim Line8 As String = sr.ReadLine

                        Dim wrapper As New Simple3Des("12345")
                        plainText = wrapper.DecryptData(Line2.Substring(11))

                        txt_server.Text = Line.Substring(11)
                        txt_uid.Text = Line1.Substring(11)
                        txt_pwd.Text = plainText
                        txt_database.Text = Line3.Substring(11)
                        txt_port.Text = Line4.Substring(11)
                    End Using
                    'Assing the object property values
                    .ServerName = txt_server.Text
                    .DatabaseName = txt_database.Text
                    .UserID = txt_uid.Text
                    .Password = txt_pwd.Text
                    .Port = txt_port.Text

                    'Connection testing
                    If .Connection Then

                    Else
                        'Unable to connect
                        MessageBox.Show("Unable to connect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                End With
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If System.IO.File.Exists(appPath + "\Settings.ini") Then
            GroupBox3.Select()
            Close()

            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            StartWindow.WindowState = FormWindowState.Maximized
            StartWindow.Show()
            StartWindow.MdiParent = MainInterface
        Else
            MessageBox.Show("Unable to connect to the database. System closing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Application.ExitThread()
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Label11.Select()
        save()
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        Label12.Select()
        Dim data As New Database
        With data
            'Assing the object property values
            .ServerName = txt_server.Text
            .DatabaseName = txt_database.Text
            .UserID = txt_uid.Text
            .Password = txt_pwd.Text
            .Port = txt_port.Text
            'Connection testing
            If .Connection Then
                MessageBox.Show("Database Successfully Conneted.", "Database Connection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MsgBox(.ErrorMessage, MsgBoxStyle.Critical, "Unable to connect")
            End If
        End With
    End Sub
End Class