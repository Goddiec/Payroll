Imports System.IO
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Public Class CompanyAdd
    Dim conn As MySqlConnection
    Dim cmd As MySqlCommand
    Dim strConn As String
    Dim dr As MySqlDataReader
    Dim regBaseKey As RegistryKey = Registry.LocalMachine
    Dim SetPath As String

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Label1.Select()
        Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Select()
        Try
            Panel1.Select()
            Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Duluxe", Label1.Text, "")
            OpenCompany.openCom()
            Close()
        Catch ex As Exception
            MessageBox.Show("Access Denied", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Sub dataload()
        If OpenCompany.manage = "Y" Then
            Button1.Enabled = True
            Button2.Enabled = False
            Button3.Enabled = True
            Button4.Enabled = True
            Try
                Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
                Using sr As New System.IO.StreamReader(appPath + "\path.ini")
                    Dim Line As String = sr.ReadLine
                    SetPath = Line.ToString()
                End Using

                ListBox1.Items.Clear()
                For Each folder As String In My.Computer.FileSystem.GetDirectories(SetPath)
                    Dim datalist As String = System.IO.Path.GetFileName(folder)
                    ListBox1.Items.Add(datalist)
                    ListBox1.SetSelected(0, True)
                Next
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        ElseIf OpenCompany.manage = "N" Then
            Button1.Enabled = True
            Button2.Enabled = True
            Button3.Enabled = False
            Button4.Enabled = True
            Try
                ListBox1.Items.Clear()
                Dim regkey As RegistryKey = regBaseKey.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Duluxe", False)
                ListBox1.Items.AddRange(regkey.GetValueNames)
                ListBox1.SetSelected(0, True)
            Catch ex As Exception
                MessageBox.Show("Access Denied", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub CompanyAdd_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Select()
        dataload()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Label1.Select()
        Try
            Dim key As Microsoft.Win32.RegistryKey
            key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Duluxe", True)
            key.DeleteValue(Label1.Text)
            OpenCompany.openCom()
            Close()
        Catch ex As Exception
            MessageBox.Show("Access Denied", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label1.Select()
        Dim dialog As New DialogResult
        dialog = MsgBox("Are you sure you want to delete this database?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "System Exit")
        If dialog = DialogResult.No Then

        Else dialog = DialogResult.Yes
            Try
                Dim key As Microsoft.Win32.RegistryKey
                key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Duluxe", True)
                key.DeleteValue(Label1.Text)
                OpenCompany.openCom()

                strConn = "Server = " & DatabaseSetting.txt_server.Text & "; userid = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & ";"
                strConn &= "Database = mysql; pooling=false;"
                conn = New MySqlConnection(strConn)
                cmd = New MySqlCommand("Drop Database " & Label1.Text & "", conn)
                conn.Open()
                cmd.ExecuteNonQuery()
                conn.ChangeDatabase("" & Label1.Text & "")
                Close()
                MessageBox.Show("Database deleted.", "Database", MessageBoxButtons.OK, MessageBoxIcon.Information)
                dataload()
            Catch ex As Exception
                'MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        For i As Int16 = 0 To ListBox1.SelectedItems.Count - 1
            Label1.Text = ""
            Label1.Text += ListBox1.SelectedItems.Item(i).ToString() ' & ControlChars.NewLine
            DatabaseSetting.txt_database.Text = Label1.Text
        Next
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Label1.Text = "performance_schema" Or Label1.Text = "mysql" Then
            Button3.Enabled = False
        Else
            Button3.Enabled = True
        End If
    End Sub
End Class