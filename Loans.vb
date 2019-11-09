Imports System.Runtime.InteropServices
Imports MySql.Data.MySqlClient
Public Class Loans
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter
    Dim ds As DataSet = New DataSet
    Dim dv As DataView

    <DllImport("Gdi32.dll", EntryPoint:="CreateRoundRectRgn")>
    Private Shared Function CreateRoundRectRgn(ByVal iLeft As Integer, ByVal iTop As Integer, ByVal iRight As Integer, ByVal iBottom As Integer, ByVal iWidth As Integer, ByVal iHeight As Integer) As IntPtr
    End Function

    Dim emploexist As String
    Sub LoadDB()
        Try
            cn.Open()
            Dim Query As String
            Query = "SELECT code FROM employee WHERE code = '" & Trim(txtcode.Text) & "'"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                emploexist = "Y"
            Else
                emploexist = "N"
            End If
            cn.Close()

            If emploexist = "Y" Then
                cn.Open()
                Dim Namequery As String = "SELECT first_name, last_name FROM employee WHERE code = '" & txtcode.Text & "'"
                cmd = New MySqlCommand(Namequery, cn)
                dr = cmd.ExecuteReader
                While dr.Read
                    txtname.Text = dr("first_name").ToString() & " " & dr("last_name").ToString()
                End While
                cn.Close()
                EmployeessCode()
            ElseIf emploexist = "N" Then
                MessageBox.Show("Employee code " & txtcode.Text.ToUpper & " does not exist. Please enter correct employee code.", "Loan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtcode.Clear()
                txtcode.Select()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Loan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Loans_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtloan.Text = FormatCurrency(txtloan.Text, 2)

        Label70.AutoSize = False
        Label70.Padding = New Padding(1, 1, 1, 1)
        Label70.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label70.Width - 2, Label70.Height - 2, 5, 1))
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label1.Select()
        Close()

        For Each ChildForm As Form In MainInterface.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub

    Dim emploexist1 As Char
    Private Sub txtcode_Leave(sender As Object, e As EventArgs) Handles txtcode.Leave
        If txtcode.Text <> String.Empty Then
            cn.Open()
            Dim Query As String
            Query = "SELECT code FROM employee WHERE code = '" & Trim(txtcode.Text) & "'"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                emploexist1 = "Y"
            Else
                emploexist1 = "N"
            End If
            cn.Close()

            If emploexist1 = "Y" Then
                'LoadDB()
                leaveHistory()
                cn.Open()
                Dim Namequery As String = "SELECT first_name, last_name FROM employee WHERE code = '" & txtcode.Text & "'"
                cmd = New MySqlCommand(Namequery, cn)
                dr = cmd.ExecuteReader
                While dr.Read
                    txtname.Text = dr("first_name").ToString() & " " & dr("last_name").ToString()
                End While
                cn.Close()
            Else
                MessageBox.Show("Invalid employee code.", "Loan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtcode.Clear()
                txtcode.Select()
            End If
        ElseIf txtcode.Text = String.Empty Then
            txtname.Text = ""
        End If
    End Sub

    Private Sub TextBox1_Leave(sender As Object, e As EventArgs) Handles txtloan.Leave
        txtloan.Text = FormatCurrency(txtloan.Text, 2)
    End Sub

    Dim shldpay As Char
    Sub saveData()
        If Check_shld_pay.Checked = True Then
            shldpay = "N"
        Else
            shldpay = "Y"
        End If
        Try
            If txtcode.Text = "" Then
                MessageBox.Show("Please enter employee code.", "Leave", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtcode.Select()
            Else
                Dim dialog As New DialogResult
                dialog = MsgBox("Do you want to issue a Loan of " & FormatCurrency(txtloan.Text, 2) & " to employee " & txtname.Text & "?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Loan")

                If dialog = DialogResult.No Then
                    DialogResult.Cancel.ToString()
                Else
                    Try
                        cn.Open()
                        cmd.Connection = cn
                        cmd.CommandType = CommandType.Text
                        cmd.CommandText = "INSERT INTO loans (employee_code, date, amount, balance, paid, shlpay, paydate) VALUES ('" & txtcode.Text & "', '" & DateTimePicker1.Text & "',  '" & CDec(txtloan.Text) & "',  '" & CDec(txtloan.Text) & "', 'N', '" & shldpay & "', '" & DateTimePicker2.Text & "');"
                        dr = cmd.ExecuteReader
                        cn.Close()

                        MessageBox.Show("Loan have successfully issued to " & txtname.Text & ".", "Loan", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        leaveHistory()
                        'clear()
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Finally
                        cn.Dispose()
                    End Try
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Loan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub leaveHistory()
        Try
            Dim dt1 As New DataTable
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT e.code As 'Code', CONCAT(e.first_name, ' ', e.last_name) AS 'Name', l.date As 'Date', l.amount As 'Amount'
                                FROM loans l
                                LEFT JOIN employee e
                                ON l.employee_code = e.Code
                                WHERE e.code = '" & txtcode.Text & "'
                                ORDER BY e.code"
            End With
            da.SelectCommand = cmd
            dt1.Clear()
            da.Fill(dt1)
            DataGridView1.DataSource = dt1
            cn.Close()

            DataGridView1.Columns(0).Width = 100
            DataGridView1.Columns(1).Width = 250
            DataGridView1.Columns(2).Width = 100
            DataGridView1.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Catch ex As Exception

        End Try
    End Sub

    Sub EmployeessCode()
        Try
            Dim dt1 As New DataTable
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT e.code As 'Employee ID', CONCAT(e.first_name, ' ', e.last_name) AS 'Employee Name', l.date As 'Date Issued', l.amount As 'Amount'
                                FROM loans l
                                LEFT JOIN employee e
                                ON l.employee_code = e.Code
                                WHERE e.code LIKE '%" & txtcode.Text & "%'
                                ORDER BY e.code"
            End With
            da.SelectCommand = cmd
            dt1.Clear()
            da.Fill(dt1)
            DataGridView1.DataSource = dt1
            cn.Close()

            DataGridView1.Columns(0).Width = 100
            DataGridView1.Columns(1).Width = 300
            DataGridView1.Columns(2).Width = 200
            DataGridView1.Columns(3).Width = 150
            DataGridView1.Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Label2.Select()
        saveData()
    End Sub

    Public EmpLoanSear As Char
    Private Sub Label70_Click(sender As Object, e As EventArgs) Handles Label70.Click
        EmpLoanSear = "Y"
        SearchEmployee.ShowDialog()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        leaveHistory()
    End Sub
End Class