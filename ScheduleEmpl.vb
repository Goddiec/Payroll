Imports System.IO
Imports MySql.Data.MySqlClient
Public Class ScheduleEmpl
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim da As New MySqlDataAdapter
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager

    Sub customer()
        Try
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT code As 'Code',title As 'Title',first_name As 'First Name',last_name As 'Last Name' FROM employee ORDER BY code"
            End With
            da.SelectCommand = cmd
            dt.Clear()
            da.Fill(dt)
            DataGridView1.DataSource = dt
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub SetFontAndColor()
        With DataGridView1.DefaultCellStyle
            .Font = New Font("Microsoft Sans Serif", 9)
            .ForeColor = Color.Black
            .SelectionForeColor = Color.White
            .SelectionBackColor = Color.Navy
        End With
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DataGridView1.Select()
        Close()
    End Sub

    Private Sub SearchEmployee_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.Select()
        Me.KeyPreview = True
        DataGridView1.RowTemplate.Height = 30
        customer()
        SetFontAndColor()
        DataGridView1.Columns(0).Width = 120
        DataGridView1.Columns(1).Width = 120
        DataGridView1.Columns(2).Width = 120
        DataGridView1.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
    End Sub

    Sub populateEmp()
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT e.code As 'EmployeeID',e.loan,e.id_number,e.working_h_day,e.fixed_salary,e.rate_per_hour,p.overtime_per_hour, CONCAT(e.first_name, ' ', e.last_name) AS 'EmployeeName', m.description As 'Designation'
                          FROM employee e
                          LEFT JOIN designation m
                          ON e.designation = m.CODE 
                          JOIN parameters p
                          WHERE e.code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                PayrollSchedule.txtEmpCode.Text = dr("EmployeeID").ToString()
                PayrollSchedule.txtDescription.Text = dr("EmployeeName").ToString()
                PayrollSchedule.txtFixedPay.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                PayrollSchedule.txtOvertimeRate.Text = FormatCurrency(dr("overtime_per_hour").ToString(), 2)
                'PayrollSchedule.txtratehour.Text = FormatCurrency(dr("rate_per_hour").ToString(), 2)
                'PayrollSchedule.txtoverhour.Text = FormatCurrency(dr("overtime_per_hour").ToString(), 2)
                'PayrollSchedule.txtworkinghrs.Text = FormatNumber(dr("working_h_day").ToString(), 0)
                'PayrollSchedule.txtLoans.Text = FormatCurrency(dr("loan").ToString(), 2)
            End While
            cn.Close()
            Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Panel1.Select()
        populateEmp()
    End Sub

    Private Sub SearchEmployee_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Close()
        End If

        If e.KeyCode = Keys.Enter Then
            populateEmp()
        End If
    End Sub
End Class