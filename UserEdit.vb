Imports System.IO
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text
Public Class UserEdit
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter("select * from tbusers", cn)
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager
    Dim READER As MySqlDataReader

    Public Sub filldatasetandview()
        ds = New DataSet
        da.Fill(ds, "tbusers")
        dv = New DataView(ds.Tables("tbusers"))
        cm = CType(Me.BindingContext(dv), CurrencyManager)
    End Sub

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

    Private Sub UserEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Select()
        Me.KeyPreview = True
        Panel1.AutoScroll = True
        LoadDB()
    End Sub

    Dim pay_employees As Char
    Dim add_department As Char
    Dim add_designation As Char
    Dim add_leave_type As Char
    Dim add_employee As Char
    Dim employee_list As Char
    Dim hr_documents As Char
    Dim issue_leave As Char
    Dim employee_loan As Char
    Dim database_setup As Char
    Dim registration As Char
    Dim reports As Char
    Dim configure_email As Char
    Dim user_accounts As Char
    Dim company_parameters As Char
    Dim banks_setup As Char
    Dim deductions_setup As Char
    Dim week_setup As Char
    Dim backup As Char
    Dim restore As Char
    Dim scan_documents As Char
    Dim settings As Char
    Sub LoadDB()
        Try
            cn.Open()
            Dim itm As String = "SELECT * FROM users WHERE user_name = '" & users.DataGridView1.CurrentRow.Cells(1).Value & "'"
            cmd = New MySqlCommand(itm, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtUserName.Text = dr.GetString("user_name").ToString()
                txtName.Text = dr.GetString("first_name").ToString()
                txtPassword.Text = Decrypt(dr.GetString("password").ToString())
                txtConfirmPassword.Text = Decrypt(dr.GetString("password").ToString())

                add_employee = dr.GetString("add_employee").ToString()
                pay_employees = dr.GetString("pay_employees").ToString()
                add_department = dr.GetString("add_department").ToString()
                add_designation = dr.GetString("add_designation").ToString()
                add_leave_type = dr.GetString("add_leave_type").ToString()
                employee_list = dr.GetString("employee_list").ToString()
                hr_documents = dr.GetString("hr_documents").ToString()
                issue_leave = dr.GetString("issue_leave").ToString()
                employee_loan = dr.GetString("employee_loan").ToString()
                database_setup = dr.GetString("database_setup").ToString()
                registration = dr.GetString("registration").ToString()
                reports = dr.GetString("reports").ToString()
                configure_email = dr.GetString("configure_email").ToString()
                user_accounts = dr.GetString("user_accounts").ToString()
                company_parameters = dr.GetString("company_parameters").ToString()
                banks_setup = dr.GetString("banks_setup").ToString()
                deductions_setup = dr.GetString("deductions_setup").ToString()
                week_setup = dr.GetString("week_setup").ToString()
                backup = dr.GetString("backup").ToString()
                restore = dr.GetString("restore").ToString()
                scan_documents = dr.GetString("scan_documents").ToString()
                settings = dr.GetString("settings").ToString()
            End While
            cn.Close()

            If pay_employees = "Y" Then
                Check_01.Checked = True
            Else
                Check_01.Checked = False
            End If

            If add_department = "Y" Then
                Check_02.Checked = True
            Else
                Check_02.Checked = False
            End If

            If add_designation = "Y" Then
                Check_03.Checked = True
            Else
                Check_03.Checked = False
            End If

            If add_leave_type = "Y" Then
                Check_04.Checked = True
            Else
                Check_04.Checked = False
            End If

            If add_employee = "Y" Then
                Check_05.Checked = True
            Else
                Check_05.Checked = False
            End If

            If employee_list = "Y" Then
                Check_06.Checked = True
            Else
                Check_06.Checked = False
            End If

            If hr_documents = "Y" Then
                Check_07.Checked = True
            Else
                Check_07.Checked = False
            End If

            If issue_leave = "Y" Then
                Check_08.Checked = True
            Else
                Check_08.Checked = False
            End If

            If deductions_setup = "Y" Then
                Check_09.Checked = True
            Else
                Check_09.Checked = False
            End If

            If week_setup = "Y" Then
                Check_10.Checked = True
            Else
                Check_10.Checked = False
            End If

            If scan_documents = "Y" Then
                Check_11.Checked = True
            Else
                Check_11.Checked = False
            End If

            If employee_loan = "Y" Then
                Check_12.Checked = True
            Else
                Check_12.Checked = False
            End If

            If database_setup = "Y" Then
                Check_13.Checked = True
            Else
                Check_13.Checked = False
            End If

            If registration = "Y" Then
                Check_14.Checked = True
            Else
                Check_14.Checked = False
            End If

            If reports = "Y" Then
                Check_15.Checked = True
            Else
                Check_15.Checked = False
            End If

            If configure_email = "Y" Then
                Check_16.Checked = True
            Else
                Check_16.Checked = False
            End If

            If user_accounts = "Y" Then
                Check_17.Checked = True
            Else
                Check_17.Checked = False
            End If

            If company_parameters = "Y" Then
                Check_18.Checked = True
            Else
                Check_18.Checked = False
            End If

            If banks_setup = "Y" Then
                Check_19.Checked = True
            Else
                Check_19.Checked = False
            End If

            If backup = "Y" Then
                Check_20.Checked = True
            Else
                Check_20.Checked = False
            End If

            If restore = "Y" Then
                Check_21.Checked = True
            Else
                Check_21.Checked = False
            End If

            If settings = "Y" Then
                Check_22.Checked = True
            Else
                Check_22.Checked = False
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Users", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Label1.Select()
        Close()
    End Sub

    Dim Check01 As Char
    Dim Check02 As Char
    Dim Check03 As Char
    Dim Check04 As Char
    Dim Check05 As Char
    Dim Check06 As Char
    Dim Check07 As Char
    Dim Check08 As Char
    Dim Check09 As Char
    Dim Check10 As Char
    Dim Check11 As Char
    Dim Check12 As Char
    Dim Check13 As Char
    Dim Check14 As Char
    Dim Check15 As Char
    Dim Check16 As Char
    Dim Check17 As Char
    Dim Check18 As Char
    Dim Check19 As Char
    Dim Check20 As Char
    Dim Check21 As Char
    Dim Check22 As Char
    Sub saveUser()
        Try
            If Check_01.Checked = True Then
                Check01 = "Y"
            Else
                Check01 = "N"
            End If

            If Check_02.Checked = True Then
                Check02 = "Y"
            Else
                Check02 = "N"
            End If

            If Check_03.Checked = True Then
                Check03 = "Y"
            Else
                Check03 = "N"
            End If

            If Check_04.Checked = True Then
                Check04 = "Y"
            Else
                Check04 = "N"
            End If

            If Check_05.Checked = True Then
                Check05 = "Y"
            Else
                Check05 = "N"
            End If

            If Check_06.Checked = True Then
                Check06 = "Y"
            Else
                Check06 = "N"
            End If

            If Check_07.Checked = True Then
                Check07 = "Y"
            Else
                Check07 = "N"
            End If

            If Check_08.Checked = True Then
                Check08 = "Y"
            Else
                Check08 = "N"
            End If

            If Check_09.Checked = True Then
                Check09 = "Y"
            Else
                Check09 = "N"
            End If

            If Check_10.Checked = True Then
                Check10 = "Y"
            Else
                Check10 = "N"
            End If

            If Check_11.Checked = True Then
                Check11 = "Y"
            Else
                Check11 = "N"
            End If

            If Check_12.Checked = True Then
                Check12 = "Y"
            Else
                Check12 = "N"
            End If

            If Check_13.Checked = True Then
                Check13 = "Y"
            Else
                Check13 = "N"
            End If

            If Check_14.Checked = True Then
                Check14 = "Y"
            Else
                Check14 = "N"
            End If

            If Check_15.Checked = True Then
                Check15 = "Y"
            Else
                Check15 = "N"
            End If

            If Check_16.Checked = True Then
                Check16 = "Y"
            Else
                Check16 = "N"
            End If

            If Check_17.Checked = True Then
                Check17 = "Y"
            Else
                Check17 = "N"
            End If

            If Check_18.Checked = True Then
                Check18 = "Y"
            Else
                Check18 = "N"
            End If

            If Check_19.Checked = True Then
                Check19 = "Y"
            Else
                Check19 = "N"
            End If

            If Check_20.Checked = True Then
                Check20 = "Y"
            Else
                Check20 = "N"
            End If

            If Check_21.Checked = True Then
                Check21 = "Y"
            Else
                Check21 = "N"
            End If

            If Check_22.Checked = True Then
                Check22 = "Y"
            Else
                Check22 = "N"
            End If

            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "UPDATE users SET pay_employees='" & Check01 & "',add_department='" & Check02 & "',add_designation='" & Check03 & "',add_leave_type='" & Check04 & "',add_employee='" & Check05 & "',employee_list='" & Check06 & "',hr_documents='" & Check07 & "',issue_leave='" & Check08 & "',deductions_setup='" & Check09 & "',week_setup='" & Check10 & "',scan_documents='" & Check11 & "',employee_loan='" & Check12 & "',database_setup='" & Check13 & "',registration='" & Check14 & "',reports='" & Check15 & "',configure_email='" & Check16 & "',user_accounts='" & Check17 & "',company_parameters='" & Check18 & "',banks_setup='" & Check19 & "',backup='" & Check20 & "',restore='" & Check21 & "',settings='" & Check22 & "',first_name = '" & txtName.Text & "', password = '" & Encrypt(Trim(txtPassword.Text)) & "' WHERE user_name = '" & txtUserName.Text & "'"
            dr = cmd.ExecuteReader
            cn.Close()
            Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Edit User", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub selectAll()
        Check_01.Checked = True
        Check_02.Checked = True
        Check_03.Checked = True
        Check_04.Checked = True
        Check_05.Checked = True
        Check_06.Checked = True
        Check_07.Checked = True
        Check_08.Checked = True
        Check_09.Checked = True
        Check_10.Checked = True
        Check_11.Checked = True
        Check_12.Checked = True
        Check_13.Checked = True
        Check_14.Checked = True
        Check_15.Checked = True
        Check_16.Checked = True
        Check_17.Checked = True
        Check_18.Checked = True
        Check_19.Checked = True
        Check_20.Checked = True
        Check_21.Checked = True
        Check_22.Checked = True
    End Sub

    Sub selectNone()
        Check_01.Checked = False
        Check_02.Checked = False
        Check_03.Checked = False
        Check_04.Checked = False
        Check_05.Checked = False
        Check_06.Checked = False
        Check_07.Checked = False
        Check_08.Checked = False
        Check_09.Checked = False
        Check_10.Checked = False
        Check_11.Checked = False
        Check_12.Checked = False
        Check_13.Checked = False
        Check_14.Checked = False
        Check_15.Checked = False
        Check_16.Checked = False
        Check_17.Checked = False
        Check_18.Checked = False
        Check_19.Checked = False
        Check_20.Checked = False
        Check_21.Checked = False
        Check_22.Checked = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Label1.Select()
        saveUser()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        Label1.Select()
        selectNone()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Select()
        selectAll()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Label1.Select()
        Close()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Label1.Select()
        saveUser()
        users.LoadData()
    End Sub

    Private Sub UserEdit_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            Label1.Select()
            saveUser()
            users.LoadData()
        End If

        If e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub
End Class