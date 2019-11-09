Imports System.IO
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text
Public Class UserAcc
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter("select * from tbusers", cn)
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager
    Dim READER As MySqlDataReader

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
    Sub save()
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

        txtUserName.Select()
        If txtName.Text = "" Then
            MessageBox.Show("Please enter Name to contiune!", "User Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtName.Focus()
        ElseIf txtUserName.Text = "" Then
            MessageBox.Show("Please enter Username to contiune!", "User Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtUserName.Focus()
        ElseIf txtPassword.Text = "" Then
            MessageBox.Show("Password entered does not match!", "User Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtPassword.Focus()
        ElseIf txtPassword.Text <> txtConfirmPassword.Text Then
            MessageBox.Show("Password entered does not match!", "User Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtConfirmPassword.Clear()
            txtConfirmPassword.Focus()
        Else
            Try
                cn.Open()
                Dim Query As String
                Query = "INSERT INTO users(user_name,first_name,password,pay_employees,add_department,add_designation,add_leave_type,add_employee,employee_list,hr_documents,issue_leave,deductions_setup,week_setup,scan_documents,employee_loan,database_setup,registration,reports,configure_email,user_accounts,company_parameters,banks_setup,backup,restore,settings) VALUES('" & Trim(txtUserName.Text) & "','" & Trim(txtName.Text) & "','" & Encrypt(Trim(txtPassword.Text)) & "','" & Check01 & "','" & Check02 & "','" & Check03 & "','" & Check04 & "','" & Check05 & "','" & Check06 & "','" & Check07 & "','" & Check08 & "','" & Check09 & "','" & Check10 & "','" & Check11 & "','" & Check12 & "','" & Check13 & "','" & Check14 & "','" & Check15 & "','" & Check16 & "','" & Check17 & "','" & Check18 & "','" & Check19 & "','" & Check20 & "','" & Check21 & "','" & Check22 & "')"
                cmd = New MySqlCommand(Query, cn)
                dr = cmd.ExecuteReader
                cn.Close()
                users.LoadData()
                Close()

                clear()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Users", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                cn.Dispose()
            End Try
        End If
    End Sub

    Sub clear()
        txtName.Clear()
        txtUserName.Clear()
        txtPassword.Clear()
        txtConfirmPassword.Clear()
    End Sub

    Private Sub UserAcc_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        clear()
        Me.KeyPreview = True
        Panel1.AutoScroll = True
        txtName.Select()
    End Sub

    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs)
        'If Asc(e.KeyChar) <> 8 Then
        '    If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
        '        e.Handled = True
        '    End If
        'End If
    End Sub

    Private Sub txtConfirmPassword_KeyPress(sender As Object, e As KeyPressEventArgs)
        'If Asc(e.KeyChar) <> 8 Then
        '    If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
        '        e.Handled = True
        '    End If
        'End If
    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs)
        Close()
        Label3.Select()
    End Sub

    Private Sub Button2_Click_2(sender As Object, e As EventArgs)
        Label3.Select()

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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Select()
        selectAll()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Label1.Select()
        selectNone()
    End Sub

    Private Sub Check_21_CheckedChanged(sender As Object, e As EventArgs) Handles Check_21.CheckedChanged

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Label1.Select()
        save()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Label1.Select()
        Close()
    End Sub

    Private Sub UserAcc_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            Label1.Select()
            save()
        End If

        If e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub
End Class