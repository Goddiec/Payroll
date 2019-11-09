Imports System.IO
Imports System.ComponentModel
Imports System.Threading
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Public Class startup
    Public SetPath As String
    Dim plainText As String
    Dim appPathT As String = Path.GetDirectoryName(Application.ExecutablePath)
    Public incorrectDate As Char

    Private Sub Splash_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ProgressBar1.Value = 1
        Visible = True
        Label3.Select()
        ProgressBar1.Value = 1
        Me.KeyPreview = True
        Label3.Text = My.Application.Info.Description
        Label4.Text = My.Application.Info.Copyright '+ " Point Of Sale Technologies, Inc. All Rights Reserved."
        Label9.Text = "Version " & My.Application.Info.Version.ToString & " (" & My.Application.Info.ProductName & ")"
    End Sub

    'Sub LDG()
    '    Try
    '        Dim sqlConn As New MySqlConnection("server = localhost; username = root; password = 12345;")
    '        sqlConn.Open()
    '        Dim cmd As New MySqlCommand("select table_name from information_schema.tables where TABLE_SCHEMA = 'posnetdb'", sqlConn)
    '        Dim dsColumns As New DataSet
    '        Dim daAdapter As New MySqlDataAdapter(cmd)

    '        daAdapter.Fill(dsColumns)
    '        If dsColumns.Tables(0).Rows.Count > 0 Then
    '            'LOGIN.ShowDialog()
    '            'OpenCompany.ShowDialog()
    '        Else
    '            MessageBox.Show("Database posnetdb not found. Click OK button to create a new Database file and setup configarations", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '            'NewCompany.ShowDialog()
    '            'OpenCompany.ShowDialog()
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '    End Try
    'End Sub

    Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
    Sub ShowLogin()
        Me.Visible = False
        MainInterface.Activate()
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

                        DatabaseSetting.txt_server.Text = Line.Substring(11)
                        DatabaseSetting.txt_uid.Text = Line1.Substring(11)
                        DatabaseSetting.txt_pwd.Text = plainText
                        DatabaseSetting.txt_database.Text = Line3.Substring(11)
                        DatabaseSetting.txt_port.Text = Line4.Substring(11)
                    End Using
                    'Assing the object property values
                    .ServerName = DatabaseSetting.txt_server.Text
                    '.DatabaseName = DatabaseSetting.txt_database.Text
                    .UserID = DatabaseSetting.txt_uid.Text
                    .Password = DatabaseSetting.txt_pwd.Text
                    .Port = DatabaseSetting.txt_port.Text

                    'Connection testing
                    If .Connection Then
                        'Database Successfully Conneted
                        Dim days As Integer = DateDiff(DateInterval.Day, System.DateTime.Today.Date, My.Settings.ExpiryDate.Date)
                        If days < 21 Then
                            MsgBox("WARNING: Your system is scheduled to expire on " & My.Settings.ExpiryDate.Date.ToString("yyyy/MM/dd") & " which is " & days & " days from now. The system will no longer function on or after that date. Please contact our adminstrator for a new registration code.", MsgBoxStyle.Information, "Registration Assistance")
                            OpenCompany.ShowDialog()
                        Else
                            OpenCompany.ShowDialog()
                        End If
                    Else
                        'Unable to connect
                        DatabaseSetting.ShowDialog()
                    End If
                End With
            Else
                DatabaseSetting.ShowDialog()
                '    Dim wrapper As New Simple3Des("12345")
                '    Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
                '    Using sw As New System.IO.StreamWriter(appPath + "\Settings.ini", False)
                '        sw.WriteLine("Server   =>localhost")
                '        sw.WriteLine("User     =>root")
                '        Dim cipherText As String = "Password =>" + wrapper.EncryptData("12345")
                '        sw.WriteLine(cipherText)
                '        sw.WriteLine("Database =>test")
                '    End Using

                '    Dim days As Integer = DateDiff(DateInterval.Day, System.DateTime.Today.Date, My.Settings.ExpiryDate.Date)
                '    If days < 21 Then
                '        MsgBox("WARNING: Your system is scheduled to expire on " & My.Settings.ExpiryDate.Date.ToString("yyyy/MM/dd") & " which is " & days & " days from now. The system will no longer function on or after that date. Please contact our adminstrator for a new registration code.", MsgBoxStyle.Information, "Registration Assistance")
                '        LDG()
                '    Else
                '        LDG()
                '    End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            DatabaseSetting.ShowDialog()
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ProgressBar1.Value = ProgressBar1.Value + 2
        If ProgressBar1.Value >= 99 Then
            Timer1.Enabled = False
            ShowLogin()
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Static count As Integer
        If count < 1 Then
            Label2.Text = "Starting Accounting System..."
            count += 1
        ElseIf count < 2 Then
            Label2.Text = "Checking Database..."
            count += 1
        ElseIf count < 3 Then
            Label2.Text = "Loading Environment Variables..."
            count += 1
        ElseIf count < 4 Then
            Label2.Text = "Initialising Main Interface"
            count += 1
        ElseIf count < 5 Then
            Label2.Text = "Verifying License..."
            count += 1
        ElseIf count < 6 Then
            Label2.Text = "Starting Services..."
            count += 1
        ElseIf count < 7 Then
            Label2.Text = "Finalising..."
            count += 1

            If Not Directory.Exists(appPath + "\IMAGES") Then
                Directory.CreateDirectory(appPath + "\IMAGES")
            End If

            If Not Directory.Exists(appPath + "\DOCUMENTS") Then
                Directory.CreateDirectory(appPath + "\DOCUMENTS")
            End If

            If Not Directory.Exists(appPath + "\HRdoc") Then
                Directory.CreateDirectory(appPath + "\HRdoc")
            End If

            If Not Directory.Exists(appPath + "\IMAGES") Then
                Directory.CreateDirectory(appPath + "\IMAGES")
            End If

            If Not Directory.Exists(appPath + "\Reports") Then
                Directory.CreateDirectory(appPath + "\Reports")
            End If
        ElseIf count < 8 Then
            Label2.Text = "System Ready..."
        End If
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class