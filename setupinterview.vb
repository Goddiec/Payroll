Imports System.IO
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.Runtime.InteropServices

Public Class setupinterview
    Dim cn As New MySqlConnection("server = " & newcompany.txt_server.Text & "; username = " & newcompany.txt_uid.Text & "; password = " & newcompany.txt_pwd.Text & "; database = " & newcompany.txtName.Text & ";port = " & newcompany.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager
    Dim READER As MySqlDataReader
    Dim MysqlConn As MySqlConnection
    Dim COMMAND As MySqlCommand
    Dim sha1 As New System.Security.Cryptography.SHA1Cng

    <DllImport("Gdi32.dll", EntryPoint:="CreateRoundRectRgn")>
    Private Shared Function CreateRoundRectRgn(ByVal iLeft As Integer, ByVal iTop As Integer, ByVal iRight As Integer, ByVal iBottom As Integer, ByVal iWidth As Integer, ByVal iHeight As Integer) As IntPtr
    End Function

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

    Private Sub setupinterview_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel1.Select()
        Button1.BackColor = Color.Red
        Button2.BackColor = Color.AliceBlue

        Panel4.Visible = True
        Panel4.Dock = DockStyle.Fill
        com_num.Select()
        txtnapsa.Text = FormatCurrency(255, 2)
        txtnapsaper.Text = FormatNumber(5, 2)
        Combo_standard_industry_class.SelectedIndex = 0
        ComboBox1.SelectedIndex = 0
        Combo_passport_country.SelectedIndex = 0

        Label66.AutoSize = False
        Label66.Padding = New Padding(1, 1, 1, 1)
        Label66.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label66.Width - 2, Label66.Height - 2, 5, 1))
        PictureBox1.Image = My.Resources.logo1
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Panel1.Select()
        Button1.BackColor = Color.Red
        Button2.BackColor = Color.AliceBlue

        Panel5.Visible = False
        Panel4.Visible = True
        Panel4.Dock = DockStyle.Fill
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        Panel1.Select()
        Button1.BackColor = Color.AliceBlue
        Button2.BackColor = Color.Red

        Panel4.Visible = False
        Panel5.Visible = True
        Panel5.Dock = DockStyle.Fill
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)

    End Sub

    Sub NextBtn()
        If Button7.Text = "Next" Then
            If com_num.Text = String.Empty Then
                MessageBox.Show("Please enter the Company number!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                com_num.Select()
            ElseIf com_name.Text = String.Empty Then
                MessageBox.Show("Please enter the Company name!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                com_name.Select()
            ElseIf Combo_standard_industry_class.SelectedIndex = 0 Then
                MessageBox.Show("Please select the Industry Classification!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Combo_standard_industry_class.Select()
            ElseIf ComboBox1.SelectedIndex = 0 Then
                MessageBox.Show("Please select the number of employees to be registered on this company. NB this can be changed at a later stage.", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                ComboBox1.Select()
            ElseIf txtnapsa.Text = String.Empty Then
                MessageBox.Show("Please enter NASPA amount!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtnapsa.Select()
            ElseIf txtnapsaper.Text = String.Empty Then
                MessageBox.Show("Please enter NASPA percentage!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtnapsaper.Select()
            ElseIf Combo_passport_country.SelectedIndex = 0 Then
                MessageBox.Show("Please select the conuntry which this company is registered under.", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Combo_passport_country.Select()
            Else
                txtUsername.Select()
                Button1.BackColor = Color.AliceBlue
                Button2.BackColor = Color.Red

                Panel4.Visible = False
                Panel5.Visible = True
                Panel5.Dock = DockStyle.Fill
                Button7.Text = "Save"
            End If
        ElseIf Button7.Text = "Save" Then
            Panel1.Select()
            SaveData()
        End If

        'If Panel4.Visible = True Then
        '    If com_num.Text = String.Empty Then
        '        MessageBox.Show("Please enter the Company number!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        com_num.Select()
        '    ElseIf com_name.Text = String.Empty Then
        '        MessageBox.Show("Please enter the Company name!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        com_name.Select()
        '    ElseIf Combo_standard_industry_class.SelectedIndex = 0 Then
        '        MessageBox.Show("Please select the Industry Classification!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        Combo_standard_industry_class.Select()
        '    ElseIf ComboBox1.SelectedIndex = 0 Then
        '        MessageBox.Show("Please select the employees", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        ComboBox1.Select()
        '    ElseIf txtnapsa.Text = String.Empty Then
        '        MessageBox.Show("Please enter NASPA amount!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        txtnapsa.Select()
        '    ElseIf txtnapsaper.Text = String.Empty Then
        '        MessageBox.Show("Please enter NASPA percentage!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        txtnapsaper.Select()
        '    ElseIf Button7.Text = "Next" Then
        '        txtUsername.Select()
        '        Button1.BackColor = Color.AliceBlue
        '        Button2.BackColor = Color.Red

        '        Panel4.Visible = False
        '        Panel5.Visible = True
        '        Panel5.Dock = DockStyle.Fill
        '        Button7.Text = "Save"
        '    ElseIf Button7.Text = "Save" Then
        '        Panel1.Select()
        '        MsgBox("Test")
        '        'SaveData()
        '    End If
        'End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Panel1.Select()
        NextBtn()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        com_num.Select()
        Button1.BackColor = Color.Red
        Button2.BackColor = Color.AliceBlue

        Panel5.Visible = False
        Panel4.Visible = True
        Panel4.Dock = DockStyle.Fill
        Button7.Text = "Next"
    End Sub

    Dim numEmp As Integer
    Dim result As Integer
    Dim Query1 As String
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Label1.Select()
        Close()
    End Sub

    Sub SaveData()
        If Panel5.Visible = True Then
            If txtUsername.Text = String.Empty Then
                MessageBox.Show("Please enter your username!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtUsername.Select()
            ElseIf txtName.Text = String.Empty Then
                MessageBox.Show("Please enter your name!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtName.Select()
            ElseIf txtPassword.Text = String.Empty Then
                MessageBox.Show("Please enter your password!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtPassword.Select()
            ElseIf txtConfirmPassword.Text <> txtPassword.Text Then
                MessageBox.Show("Your password does not match!", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtConfirmPassword.Clear()
                txtConfirmPassword.Select()
            Else
                Try
                    If ComboBox1.SelectedIndex = 1 Then
                        numEmp = 5
                    ElseIf ComboBox1.SelectedIndex = 2 Then
                        numEmp = 15
                    ElseIf ComboBox1.SelectedIndex = 3 Then
                        numEmp = 30
                    ElseIf ComboBox1.SelectedIndex = 4 Then
                        numEmp = 45
                    ElseIf ComboBox1.SelectedIndex = 5 Then
                        numEmp = 60
                    ElseIf ComboBox1.SelectedIndex = 6 Then
                        numEmp = 90
                    ElseIf ComboBox1.SelectedIndex = 7 Then
                        numEmp = 99999999
                    End If

                    'INSERT INTO COMPANY
                    cn.Open()
                    Query1 = "INSERT INTO company(com_num,com_name,com_reg_num,sdl_pay_ref_num,uif_pay_ref_num,uif_reg_num,standard_industry_class,phy_country)
                                VALUES (@com_num, @com_name, @com_reg_num, @sdl_pay_ref_num, @uif_pay_ref_num, @uif_reg_num, @standard_industry_class, @phy_country)"
                    cmd = New MySqlCommand
                    With cmd
                        .Connection = cn
                        .CommandText = Query1
                        .Parameters.AddWithValue("@com_num", com_num.Text)
                        .Parameters.AddWithValue("@com_name", com_name.Text)
                        .Parameters.AddWithValue("@com_reg_num", com_reg_num.Text)
                        .Parameters.AddWithValue("@sdl_pay_ref_num", sdl_pay_ref_num.Text)
                        .Parameters.AddWithValue("@uif_pay_ref_num", uif_pay_ref_num.Text)
                        .Parameters.AddWithValue("@uif_reg_num", uif_reg_num.Text)
                        .Parameters.AddWithValue("@standard_industry_class", Combo_standard_industry_class.Text)
                        .Parameters.AddWithValue("@phy_country", Combo_passport_country.Text)
                        result = .ExecuteNonQuery()
                    End With
                    cn.Close()
                    'END INSERTING INTO COMPANY

                    Dim myAdapter As New MySqlDataAdapter
                    Dim sqlquery = "SELECT * FROM company"
                    Dim myCommand As New MySqlCommand()
                    myCommand.Connection = cn
                    myCommand.CommandText = sqlquery
                    myAdapter.SelectCommand = myCommand
                    cn.Open()
                    Dim ms As New MemoryStream

                    Dim bm As Bitmap = New Bitmap(PictureBox1.Image)
                    bm.Save(ms, PictureBox1.Image.RawFormat)

                    Dim arrPic() As Byte = ms.GetBuffer()

                    sqlquery = "UPDATE company SET ImageFile=@ImageFile"

                    myCommand = New MySqlCommand(sqlquery, cn)
                    myCommand.Parameters.AddWithValue("@ImageFile", arrPic)
                    myCommand.ExecuteNonQuery()
                    cn.Close()

                    cn.Open()
                    Dim Query2 As String
                    Query2 = "UPDATE parameters SET napsa='" & CDec(txtnapsa.Text) & "',napsaper='" & CDec(txtnapsaper.Text) & "'"
                    cmd = New MySqlCommand(Query2, cn)
                    dr = cmd.ExecuteReader
                    cn.Close()

                    'INSERT INTO tax++++++++++++++++++++++++++++++++++
                    If Combo_passport_country.Text = "Rwanda" Then
                        cn.Open()
                        Dim tax12 As String
                        tax12 = "INSERT INTO tax(`taxValue1`, `taxValue2`, `taxValue3`, `taxGross1`, `taxGross2`, `taxGross3`, `use1`, `use2`, `use3`) VALUES ('0.00', '20.00', '30.00', '30000.00', '100000.00', '100000.01', 'Y', 'Y', 'Y');"
                        cmd = New MySqlCommand(tax12, cn)
                        dr = cmd.ExecuteReader
                        cn.Close()
                    ElseIf Combo_passport_country.Text = "Zambia" Then
                        cn.Open()
                        Dim tax1 As String
                        tax1 = "INSERT INTO tax(`taxValue1`, `taxValue2`, `taxValue3`, `taxValue4`, `taxGross1`, `taxGross2`, `taxGross3`, `taxGross4`, `use1`, `use2`, `use3`, `use4`) VALUES ('0', '25.00', '30.00', '37.50', '3300.00', '4100.00', '6200.00', '6200.01', 'Y', 'Y', 'Y', 'Y');"
                        cmd = New MySqlCommand(tax1, cn)
                        dr = cmd.ExecuteReader
                        cn.Close()
                    End If

                    'INSERT INTO USERS
                    cn.Open()
                    Query1 = "INSERT INTO users(user_name,first_name,password,pay_employees,add_department,add_designation,add_leave_type,add_employee,employee_list,hr_documents,issue_leave,deductions_setup,week_setup,scan_documents,employee_loan,database_setup,registration,reports,configure_email,user_accounts,company_parameters,banks_setup,backup,restore,settings)
                             VALUES (@user_name,@first_name,@password,@pay_employees,@add_department,@add_designation,@add_leave_type,@add_employee,@employee_list,@hr_documents,@issue_leave,@deductions_setup,@week_setup,@scan_documents,@employee_loan,@database_setup,@registration,@reports,@configure_email,@user_accounts,@company_parameters,@banks_setup,@backup,@restore,@settings)"
                    cmd = New MySqlCommand
                    With cmd
                        .Connection = cn
                        .CommandText = Query1
                        .Parameters.AddWithValue("@user_name", txtUsername.Text)
                        .Parameters.AddWithValue("@first_name", txtName.Text)
                        .Parameters.AddWithValue("@password", Encrypt(Trim(txtPassword.Text)))
                        .Parameters.AddWithValue("@pay_employees", "Y")
                        .Parameters.AddWithValue("@add_department", "Y")
                        .Parameters.AddWithValue("@add_designation", "Y")
                        .Parameters.AddWithValue("@add_leave_type", "Y")
                        .Parameters.AddWithValue("@add_employee", "Y")
                        .Parameters.AddWithValue("@employee_list", "Y")
                        .Parameters.AddWithValue("@hr_documents", "Y")
                        .Parameters.AddWithValue("@issue_leave", "Y")
                        .Parameters.AddWithValue("@deductions_setup", "Y")
                        .Parameters.AddWithValue("@week_setup", "Y")
                        .Parameters.AddWithValue("@scan_documents", "Y")
                        .Parameters.AddWithValue("@employee_loan", "Y")
                        .Parameters.AddWithValue("@database_setup", "Y")
                        .Parameters.AddWithValue("@registration", "Y")
                        .Parameters.AddWithValue("@reports", "Y")
                        .Parameters.AddWithValue("@configure_email", "Y")
                        .Parameters.AddWithValue("@user_accounts", "Y")
                        .Parameters.AddWithValue("@company_parameters", "Y")
                        .Parameters.AddWithValue("@banks_setup", "Y")
                        .Parameters.AddWithValue("@backup", "Y")
                        .Parameters.AddWithValue("@restore", "Y")
                        .Parameters.AddWithValue("@settings", "Y")
                        result = .ExecuteNonQuery()
                    End With
                    cn.Close()
                    'END INSERTING INTO USERS
                    My.Settings.EmpNum = CInt(numEmp)
                    My.Settings.Save()
                    Hide()
                    Close()
                    newcompany.Hide()
                    save()
                    Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Duluxe", newcompany.txtName.Text, "")
                    'Application.ExitThread()

                    'If OpenCompany.ListBox1.Items.Count > 0 Then
                    '    OpenCompany.ListBox1.SelectedIndex = OpenCompany.ListBox1.Items.Count - 1
                    'End If
                    'OpenCompany.Activate()
                    'OpenCompany.ShowDialog()
                    'OpenCompany.Activate()
                    Logindb()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Users", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    cn.Dispose()
                End Try
            End If
        End If
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

    Sub UserLogged()
        Me.Hide()
        'MainInterface.Text = "Payroll [" + DatabaseSetting.txt_database.Text + "]"
        EnableIcons()
        MainInterface.Activate()
        txtPassword.Clear()
        search()

        'If My.Settings.CheckLast = True Then
        '    My.Settings.LastAccess = OpenCompany.ListBox1.SelectedIndex
        '    My.Settings.Save()
        'Else
        '    My.Settings.LastAccess = ""
        '    My.Settings.Save()
        'End If

        'For Each ChildForm As Form In Me.MdiChildren
        '    ChildForm.Close()
        'Next

        OpenCompany.Hide()
        OpenCompany.Close()
        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface

        'RunScripts.ShowDialog()
        MainInterface.Activate()
        Login.updatedata.DataBase()
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

        If Login.pay_employees = "Y" Then
            MainInterface.EditMenu.Enabled = True
        Else
            MainInterface.EditMenu.Enabled = False
        End If

        If Login.add_employee = "Y" Then
            MainInterface.EmployeesToolStripMenuItem.Enabled = True
        Else
            MainInterface.EmployeesToolStripMenuItem.Enabled = False
        End If

        If Login.company_parameters = "Y" Then
            MainInterface.CompanyParametersToolStripMenuItem.Enabled = True
        Else
            MainInterface.CompanyParametersToolStripMenuItem.Enabled = False
        End If

        If Login.banks_setup = "Y" Then
            MainInterface.BanksSeuptToolStripMenuItem.Enabled = True
        Else
            MainInterface.BanksSeuptToolStripMenuItem.Enabled = False
        End If

        If Login.deductions_setup = "Y" Then
            MainInterface.DeductionsSetupToolStripMenuItem.Enabled = True
        Else
            MainInterface.DeductionsSetupToolStripMenuItem.Enabled = False
        End If

        If Login.week_setup = "Y" Then
            MainInterface.WeekSetupToolStripMenuItem.Enabled = True
        Else
            MainInterface.WeekSetupToolStripMenuItem.Enabled = False
        End If

        If Login.user_accounts = "Y" Then
            MainInterface.UserAccountsToolStripMenuItem1.Enabled = True
        Else
            MainInterface.UserAccountsToolStripMenuItem1.Enabled = False
        End If

        If Login.configure_email = "Y" Then
            MainInterface.ConfigureEmailToolStripMenuItem1.Enabled = True
        Else
            MainInterface.ConfigureEmailToolStripMenuItem1.Enabled = False
        End If

        If Login.database_setup = "Y" Then
            MainInterface.DatabaseSetupToolStripMenuItem.Enabled = True
        Else
            MainInterface.DatabaseSetupToolStripMenuItem.Enabled = False
        End If

        If Login.reports = "Y" Then
            MainInterface.ViewMenu.Enabled = True
        Else
            MainInterface.ViewMenu.Enabled = False
        End If

        If Login.registration = "Y" Then
            MainInterface.RegistrationToolStripMenuItem.Enabled = True
        Else
            MainInterface.RegistrationToolStripMenuItem.Enabled = False
        End If

        If Login.backup = "Y" Then
            MainInterface.BackupToolStripMenuItem.Enabled = True
        Else
            MainInterface.BackupToolStripMenuItem.Enabled = False
        End If

        If Login.restore = "Y" Then
            MainInterface.RestoreToolStripMenuItem.Enabled = True
        Else
            MainInterface.RestoreToolStripMenuItem.Enabled = False
        End If

        If Login.scan_documents = "Y" Then
            MainInterface.ScanDocumentsToolStripMenuItem.Enabled = True
        Else
            MainInterface.ScanDocumentsToolStripMenuItem.Enabled = False
        End If

        If Login.settings = "Y" Then
            MainInterface.SettingsToolStripMenuItem.Enabled = True
        Else
            MainInterface.SettingsToolStripMenuItem.Enabled = False
        End If
    End Sub

    Sub Logindb()
        Try
            cn.Open()
            Dim Query As String
            Query = "SELECT * FROM users WHERE user_name = '" & txtUsername.Text & "' AND password = '" & Encrypt(txtPassword.Text) & "'"
            COMMAND = New MySqlCommand(Query, cn)
            READER = COMMAND.ExecuteReader

            Dim count As Integer
            count = 0

            While READER.Read
                count = count + 1

                Login.add_employee = READER.GetString("add_employee").ToString()
                Login.pay_employees = READER.GetString("pay_employees").ToString()
                Login.add_department = READER.GetString("add_department").ToString()
                Login.add_designation = READER.GetString("add_designation").ToString()
                Login.add_leave_type = READER.GetString("add_leave_type").ToString()
                Login.employee_list = READER.GetString("employee_list").ToString()
                Login.hr_documents = READER.GetString("hr_documents").ToString()
                Login.issue_leave = READER.GetString("issue_leave").ToString()
                Login.employee_loan = READER.GetString("employee_loan").ToString()
                Login.database_setup = READER.GetString("database_setup").ToString()
                Login.registration = READER.GetString("registration").ToString()
                Login.reports = READER.GetString("reports").ToString()
                Login.configure_email = READER.GetString("configure_email").ToString()
                Login.user_accounts = READER.GetString("user_accounts").ToString()
                Login.company_parameters = READER.GetString("company_parameters").ToString()
                Login.banks_setup = READER.GetString("banks_setup").ToString()
                Login.deductions_setup = READER.GetString("deductions_setup").ToString()
                Login.week_setup = READER.GetString("week_setup").ToString()
                Login.backup = READER.GetString("backup").ToString()
                Login.restore = READER.GetString("restore").ToString()
                Login.scan_documents = READER.GetString("scan_documents").ToString()
                Login.settings = READER.GetString("settings").ToString()
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

            cn.Close()
        Catch ex As Exception

        End Try
    End Sub

    Dim plainText As String
    Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
    Sub save()
        Try
            Dim data As New Database
            With data
                'Assing the object property values
                .ServerName = DatabaseSetting.txt_server.Text
                .DatabaseName = DatabaseSetting.txt_database.Text
                .UserID = DatabaseSetting.txt_uid.Text
                .Password = DatabaseSetting.txt_pwd.Text
                .Port = DatabaseSetting.txt_port.Text

                Dim wrapper As New Simple3Des("12345")
                Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
                Using sw As New System.IO.StreamWriter(appPath + "\Settings.ini", False)
                    sw.WriteLine("Server   =>" + DatabaseSetting.txt_server.Text)
                    sw.WriteLine("User     =>" + DatabaseSetting.txt_uid.Text)
                    Dim cipherText As String = "Password =>" + wrapper.EncryptData(DatabaseSetting.txt_pwd.Text)
                    sw.WriteLine(cipherText)
                    sw.WriteLine("Database =>" + newcompany.txtName.Text)
                    sw.WriteLine("Port Num =>" + DatabaseSetting.txt_port.Text)
                End Using
            End With

            If System.IO.File.Exists(appPath + "\Settings.ini") Then
                Dim data1 As New Database
                With data1
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
                    .DatabaseName = DatabaseSetting.txt_database.Text
                    .UserID = DatabaseSetting.txt_uid.Text
                    .Password = DatabaseSetting.txt_pwd.Text
                    .Port = DatabaseSetting.txt_port.Text
                End With
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs)

    End Sub

    Private Sub Combo_passport_country_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Combo_passport_country.SelectedIndexChanged
        If Combo_passport_country.Text = "Zambia" Then
            Label37.Text = "NAPSA"
            Label36.Text = "NAPSA %"
            txtnapsa.Text = FormatCurrency(255, 2)
            txtnapsaper.Text = FormatNumber(5, 2)
            Label37.Visible = True
            txtnapsa.Visible = True
        ElseIf Combo_passport_country.Text = "Rwanda" Then
            Label37.Text = "SSFR"
            Label36.Text = "SSFR %"
            txtnapsa.Text = FormatCurrency(1200, 2)
            txtnapsaper.Text = FormatNumber(2, 2)
            Label37.Visible = True
            txtnapsa.Visible = True
        ElseIf Combo_passport_country.Text = "Zimbabwe" Then
            Label37.Visible = False
            Label36.Text = "AIDS Levy %"
            txtnapsa.Visible = False
            txtnapsaper.Text = FormatNumber(3, 2)
        ElseIf Combo_passport_country.Text = "South Africa" Then
            Label37.Text = "SSFR"
            Label36.Text = "SSFR %"
            txtnapsa.Text = FormatCurrency(1200, 2)
            txtnapsaper.Text = FormatNumber(2, 2)
            Label37.Visible = True
            txtnapsa.Visible = True
        ElseIf Combo_passport_country.Text = "Uganda" Then
            Label37.Text = "SSFR"
            Label36.Text = "SSFR %"
            txtnapsa.Text = FormatCurrency(1200, 2)
            txtnapsaper.Text = FormatNumber(2, 2)
            Label37.Visible = True
            txtnapsa.Visible = True
        End If
    End Sub
End Class