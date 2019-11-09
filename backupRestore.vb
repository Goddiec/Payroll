Imports MySql.Data.MySqlClient
Imports System.IO
Public Class backupRestore
    Dim SqlConnections As MySqlConnection
    Dim dt As New DataTable
    Dim cmd As String
    Dim dtseCt As Integer
    Dim da As MySqlDataAdapter

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Select()
        Dim data As New Database
        With data
            'Assing the object property values
            .ServerName = txtserver.Text
            .DatabaseName = ComboBox1.Text
            .UserID = txtuserid.Text
            .Password = txtpassword.Text
            .Port = txtPort.Text

            'Connection testing
            If .Connection Then
                MessageBox.Show("Database Successfully Conneted.", "Database Connection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MsgBox(.ErrorMessage, MsgBoxStyle.Critical, "Unable to connect")
            End If
        End With
    End Sub

    Public Sub dbconnections()
        Try
            SqlConnections = New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
            If SqlConnections.State = ConnectionState.Closed Then
                SqlConnections.Open()
            End If
        Catch ex As Exception
            MsgBox("Connection Failed!")
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, ByVal e As EventArgs)
        Label1.Select()
        backup()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Label1.Select()
        backupfiles()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Label1.Select()
        Close()
    End Sub

    Sub backup()
        Try
            Dim file As String
            SaveFileDialog1.Filter = "SQL Dump File (*.sql)|*.sql|All files (*.*)|*.*"
            SaveFileDialog1.FileName = DatabaseSetting.txt_database.Text.ToLower + "" + DateTime.Now.ToString("dd/MM/yyyy HH-mm-ss") + ".sql"
            If SaveFileDialog1.ShowDialog = DialogResult.OK Then
                file = SaveFileDialog1.FileName
                Dim myProcess As New Process()
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                myProcess.StartInfo.FileName = "cmd.exe"
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                myProcess.StartInfo.UseShellExecute = False
                myProcess.StartInfo.WorkingDirectory = pathData '"C:\Program Files\MySQL\MySQL Server 5.7\bin\"
                myProcess.StartInfo.RedirectStandardInput = True
                myProcess.StartInfo.RedirectStandardOutput = True
                myProcess.Start()
                Dim myStreamWriter As StreamWriter = myProcess.StandardInput
                Dim mystreamreader As StreamReader = myProcess.StandardOutput
                myStreamWriter.WriteLine("mysqldump -u " + txtuserid.Text + " --password=" + txtpassword.Text + " -h " + txtserver.Text + " """ + ComboBox1.Text + """ > """ + file + """ ")
                myStreamWriter.Close()
                myProcess.WaitForExit()
                myProcess.Close()
                MsgBox("Backup Created Successfully", MsgBoxStyle.Information, "Restore")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Backup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs)

    End Sub

    Dim file As String
    Sub backupfiles()
        Try
            OpenFileDialog1.Filter = "SQL Dump File (*.sql)|*.sql"
            If OpenFileDialog1.ShowDialog = DialogResult.OK Then
                Dim dialog As New DialogResult

                dialog = MsgBox("Are you sure you want to restore the backup?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "System Exit")

                If dialog = DialogResult.No Then
                    'e.Cancel = True
                ElseIf dialog = DialogResult.Yes Then
                    file = OpenFileDialog1.FileName
                    Dim myProcess As New Process()
                    myProcess.StartInfo.FileName = "cmd.exe"
                    myProcess.StartInfo.UseShellExecute = False
                    myProcess.StartInfo.WorkingDirectory = pathData '"C:\Program Files\MySQL\MySQL Server 5.7\bin"
                    myProcess.StartInfo.RedirectStandardInput = True
                    myProcess.StartInfo.RedirectStandardOutput = True
                    myProcess.Start()
                    Dim myStreamWriter As StreamWriter = myProcess.StandardInput
                    Dim mystreamreader As StreamReader = myProcess.StandardOutput
                    'myStreamWriter.WriteLine("mysql -u root --password=12345 -h localhost ""newdata"" < """ + file + """ ")
                    myStreamWriter.WriteLine("mysql -u " + txtuserid.Text + " --password=" + txtpassword.Text + " -h " + txtserver.Text + " """ + ComboBox1.Text + """ < """ + file + """ ")
                    myStreamWriter.Close()
                    myProcess.WaitForExit()
                    myProcess.Close()
                    MsgBox("Database Restoration Successfully!", MsgBoxStyle.Information, "Restore")
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Restore")
        End Try
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)

    End Sub

    Dim pathData As String
    Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
    Private Sub MySqlBackupRestore_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Select()
        txtserver.Text = DatabaseSetting.txt_server.Text
        txtuserid.Text = DatabaseSetting.txt_uid.Text
        txtpassword.Text = DatabaseSetting.txt_pwd.Text
        ComboBox1.Text = DatabaseSetting.txt_database.Text
        txtPort.Text = DatabaseSetting.txt_port.Text

        Try
            Using sr As New System.IO.StreamReader(appPath + "\path.ini")
                Dim Line1 As String = sr.ReadLine
                Dim Line2 As String = sr.ReadLine

                pathData = Line2
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Close()
        End Try
    End Sub

    Private Sub backupRestore_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub
End Class