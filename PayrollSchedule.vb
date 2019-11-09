Imports MySql.Data.MySqlClient
Public Class PayrollSchedule
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager
    Dim second As Integer

    Sub dataLoad()
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT * FROM employee WHERE code = '" & txtEmpCode.Text & "'"

            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtEmpCode.Text = dr("code").ToString().ToUpper()
                txtDescription.Text = dr("first_name").ToString() & " " & dr("last_name").ToString()
                txtFixedPay.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                txtOvertimeRate.Text = dr("department").ToString()
                'txtLeave.Text = FormatCurrency(dr("rate_per_hour").ToString(), 2)
                'txtworkinghrs.Text = dr("working_h_day").ToString()
                txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                'decNetPay = dr("fixed_salary").ToString()
                'dtpFrom.Text = dr.GetString("start_date").ToString()
            End While
            cn.Close()

            cn.Open()
            Dim Query As String
            Query = "SELECT overtime_per_hour FROM parameters"

            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtOvertimeRate.Text = FormatCurrency(dr("overtime_per_hour").ToString().ToUpper(), 2)
            End While
            cn.Close()

            calcSalary()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub Employees()
        Try
            Dim dt1 As New DataTable
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT e.code As 'Employee Code', CONCAT(e.first_name, ' ', e.last_name) AS 'Employee Name',
                                e.fixed_salary As 'Regular Pay',p.overtime_per_hour As 'Overtime Rate', s.OvertimeHours As 'Overtime Hours', 0 As 'Leave Hours', ((s.OvertimeHours * p.overtime_per_hour) + (e.fixed_salary))-(p.overtime_per_hour * s.LeaveHours) As 'Salary'
                                FROM employee e
                                RIGHT JOIN scheduletable s
                                ON e.code = s.emp_code
                                RIGHT JOIN parameters p
                                ON p.ID = '1' WHERE s.code = '" & workerparyroll.DataGridView2.CurrentRow.Cells(0).Value & "'
                                ORDER BY e.code"
            End With
            da.SelectCommand = cmd
            dt1.Clear()
            da.Fill(dt1)
            DataGridView1.DataSource = dt1
            cn.Close()

            DataGridView1.Columns(0).Width = 150
            DataGridView1.Columns(1).Width = 203
            DataGridView1.Columns(2).Width = 150
            DataGridView1.Columns(3).Width = 150
            DataGridView1.Columns(4).Width = 150
            DataGridView1.Columns(5).Width = 150
            DataGridView1.Columns(6).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            Me.DataGridView1.Columns(2).DefaultCellStyle.Format = "c"
            Me.DataGridView1.Columns(3).DefaultCellStyle.Format = "c"
            Me.DataGridView1.Columns(6).DefaultCellStyle.Format = "c"

            'DataGridView1.Columns(0).ReadOnly = True
            'DataGridView1.Columns(1).ReadOnly = True
            'DataGridView1.Columns(2).ReadOnly = True
            'DataGridView1.Columns(3).ReadOnly = False
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub calcSalary()
        'txtSalary.Text = FormatCurrency((CDec(txtFixedPay.Text) + (CDec(txtOvertimeRate.Text) * CDec(txtOvertimehours.Text))), 2)
    End Sub

    Sub CLEAR()
        txtEmpCode.Text = ""
        txtDescription.Text = ""
        txtFixedPay.Text = FormatNumber(0, 2)
        txtOvertimeRate.Text = FormatCurrency(0, 2)
        txtLeave.Text = FormatNumber(0, 0)
        txtSalary.Text = FormatCurrency(0, 2)
        txtOvertimeHour.Text = FormatNumber(0, 0)
    End Sub

    Sub enterData()
        Try
            'DataGridView1.Rows.Add(txtEmpCode.Text, txtDescription.Text, FormatNumber(txtFixedPay.Text, 2), FormatCurrency(txtOvertimeRate.Text, 2), FormatNumber(txtOvertimehours.Text, 0), FormatNumber(txtLeaveHours.Text, 0), FormatCurrency(txtSalary.Text, 2))
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "DELETE FROM scheduletable WHERE emp_code = '" & txtEmpCode.Text & "'"
                .ExecuteNonQuery()
            End With
            cn.Close()
            Employees()

            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "INSERT INTO scheduletable(code,emp_code,OvertimeHours,LeaveHours) VALUES('" & workerparyroll.DataGridView2.CurrentRow.Cells(0).Value & "','" & txtEmpCode.Text & "','" & CInt(txtOvertimeHour.Text) & "','" & CInt(txtLeave.Text) & "')"
                .ExecuteNonQuery()
            End With
            cn.Close()

            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "DELETE FROM deductiontransaction WHERE emp_code = '" & txtEmpCode.Text & "'"
                .ExecuteNonQuery()
            End With
            cn.Close()

            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "INSERT INTO deductiontransaction(emp_code) VALUES('" & txtEmpCode.Text & "')"
                .ExecuteNonQuery()
            End With
            cn.Close()

            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "DELETE FROM otherincome WHERE emp_code = '" & txtEmpCode.Text & "'"
                .ExecuteNonQuery()
            End With
            cn.Close()

            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "INSERT INTO otherincome(emp_code) VALUES('" & txtEmpCode.Text & "')"
                .ExecuteNonQuery()
            End With
            cn.Close()
            Employees()
            CLEAR()
            txtEmpCode.Select()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try

    End Sub

    Private Sub PayrollSchedule_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        txtEmpCode.Select()
        Employees()
        DataGridView1.RowTemplate.Height = 30
        CLEAR()
    End Sub

    Private Sub txtEmpCode_Leave(sender As Object, e As EventArgs)
        'If txtEmpCode.Text <> String.Empty Then
        '    dataLoad()
        'End If
    End Sub

    Private Sub txtOvertimehours_Leave(sender As Object, e As EventArgs)
        calcSalary()
    End Sub

    Dim Emplexist As Char
    Sub EmployeCheck()
        Try
            cn.Open()
            Dim Query As String
            Query = "SELECT code FROM employee WHERE code = '" & Trim(txtEmpCode.Text) & "'"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                Emplexist = "Y"
            Else
                Emplexist = "N"
            End If
            cn.Close()

            If Emplexist = "Y" Then
                enterData()
            ElseIf Emplexist = "N" Then
                MessageBox.Show("Invalid employee code!", "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtEmpCode.Clear()
                txtEmpCode.Select()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub PayrollSchedule_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtEmpCode.Text <> String.Empty Then
                EmployeCheck()
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Select()
        Close()

        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        workerparyroll.WindowState = FormWindowState.Maximized
        workerparyroll.Show()
        workerparyroll.MdiParent = MainInterface
    End Sub

    Sub deleteItem()
        If DataGridView1.Rows.Count > 0 Then
            Dim dialog As New DialogResult

            dialog = MsgBox("Are you sure want to delete employee " & DataGridView1.CurrentRow.Cells(0).Value & " " & DataGridView1.CurrentRow.Cells(1).Value & "?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Schedule")

            If dialog = DialogResult.No Then
                DialogResult.Cancel.ToString()
            Else
                Try
                    cn.Open()
                    With cmd
                        .Connection = cn
                        .CommandText = "DELETE FROM scheduletable WHERE emp_code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
                        .ExecuteNonQuery()
                    End With
                    cn.Close()
                    Employees()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    cn.Dispose()
                End Try
            End If
        End If
    End Sub

    Sub clearItems()
        If DataGridView1.Rows.Count > 0 Then
            Dim dialog As New DialogResult

        dialog = MsgBox("Are you sure want to clear employees in this schedule?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Schedule")

            If dialog = DialogResult.No Then
                DialogResult.Cancel.ToString()
            Else
                Try
                    cn.Open()
                    With cmd
                        .Connection = cn
                        .CommandText = "DELETE FROM scheduletable WHERE code = '" & workerparyroll.DataGridView2.CurrentRow.Cells(0).Value & "'"
                        .ExecuteNonQuery()
                    End With
                    cn.Close()
                    workerparyroll.Schedules()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    cn.Dispose()
                End Try
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label1.Select()
        deleteItem()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        clearItems()
        txtEmpCode.Select()
        Employees()
        DataGridView1.RowTemplate.Height = 30
        CLEAR()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Label1.Select()
        If DataGridView1.Rows.Count > 0 Then
            PayrollEdit.ShowDialog()
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellLeave
        'MsgBox("Test 123")
    End Sub

    Private Sub DataGridView1_CellStateChanged(sender As Object, e As DataGridViewCellStateChangedEventArgs) Handles DataGridView1.CellStateChanged

    End Sub

    Private Sub DataGridView1_CellErrorTextChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellErrorTextChanged

    End Sub

    Sub EmployeesleaveCell()
        Try
            cn.Open()
            Dim qry As String = "SELECT e.code As 'EmployeeCode', CONCAT(e.first_name, ' ', e.last_name) AS 'EmployeeName',
                            e.fixed_salary As 'RegularPay',p.overtime_per_hour As 'OvertimeRate', 0 As 'OvertimeHours', 0 As 'LeaveHours', (e.fixed_salary) As 'Salary'
                            FROM employee e
                            RIGHT JOIN scheduletable s
                            ON e.code = s.emp_code
                            RIGHT JOIN parameters p
                            ON p.ID = '1' WHERE e.code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'
                            ORDER BY e.code"
            cmd = New MySqlCommand(qry, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                DataGridView1.CurrentRow.Cells(0).Value = dr.GetString("EmployeeCode").ToString.ToUpper()
                DataGridView1.CurrentRow.Cells(1).Value = dr.GetString("EmployeeName").ToString
                DataGridView1.CurrentRow.Cells(2).Value = dr.GetString("RegularPay").ToString
                DataGridView1.CurrentRow.Cells(3).Value = dr.GetString("OvertimeRate").ToString
                'DataGridView1.CurrentRow.Cells(4).Value = 0 'dr.GetString("OvertimeHours").ToString
                DataGridView1.CurrentRow.Cells(5).Value = dr.GetString("LeaveHours").ToString
                DataGridView1.CurrentRow.Cells(6).Value = (CDec(DataGridView1.CurrentRow.Cells(3).Value) * CDec(DataGridView1.CurrentRow.Cells(4).Value)) + CDec(DataGridView1.CurrentRow.Cells(2).Value)
            End While
            cn.Close()

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim Itemexist As String
    Sub InvalidEmployee()
        Try
            cn.Open()
            Dim Query As String
            Query = "SELECT code FROM employee WHERE code = '" & Trim(txtEmpCode.Text) & "'"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                Itemexist = "Y"
            Else
                Itemexist = "N"
            End If
            cn.Close()

            If Itemexist = "Y" Then
                dataLoad()
            ElseIf Itemexist = "N" Then
                MessageBox.Show("Invalid employee code!", "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtEmpCode.Clear()
                txtEmpCode.Select()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub DataGridView1_KeyDown(sender As Object, e As KeyEventArgs) Handles DataGridView1.KeyDown
        If e.KeyCode = Keys.Tab Then
            InvalidEmployee()
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        PayrollEdit.ShowDialog()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Label1.Select()
        If DataGridView1.Rows.Count > 0 Then
            RunPayroll.ShowDialog()
        End If
    End Sub

    Private Sub txtEmpCode_Leave_1(sender As Object, e As EventArgs) Handles txtEmpCode.Leave
        If txtEmpCode.Text <> String.Empty Then
            InvalidEmployee()
        End If
    End Sub

    Private Sub txtOvertimeHour_TextChanged(sender As Object, e As EventArgs) Handles txtOvertimeHour.TextChanged
        txtSalary.Text = FormatCurrency(CDec(txtFixedPay.Text) + (CDec(txtOvertimeHour.Text) * CDec(txtOvertimeRate.Text)), 2)
        txtOvertimeHour.Text = FormatNumber(txtOvertimeHour.Text, 0)
    End Sub

    Private Sub txtLeave_TextChanged(sender As Object, e As EventArgs) Handles txtLeave.TextChanged

    End Sub

    Private Sub txtLeave_Leave(sender As Object, e As EventArgs) Handles txtLeave.Leave
        txtSalary.Text = FormatCurrency(CDec(txtFixedPay.Text) + (CDec(txtOvertimeHour.Text) * CDec(txtOvertimeRate.Text)), 2)
        txtLeave.Text = FormatNumber(txtLeave.Text, 0)
    End Sub

    Dim total As String
    Private Sub Button6_Click(sender As Object, e As EventArgs)
        txtEmpCode.Select()
        If DataGridView1.Rows.Count > 0 Then
            DataGridView1.CurrentRow.Cells(4).Value = FormatNumber(CInt(DataGridView1.CurrentRow.Cells(4).Value) + 1, 0)
            'DataGridView1.CurrentRow.Cells(6).Value = FormatNumber(CDec(DataGridView1.CurrentRow.Cells(2).Value) + CInt(DataGridView1.CurrentRow.Cells(4).Value) * CDec(DataGridView1.CurrentRow.Cells(3).Value), 2)
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "UPDATE scheduletable SET OvertimeHours = '" & CInt(DataGridView1.CurrentRow.Cells(4).Value) & "' WHERE emp_code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
                .ExecuteNonQuery()
            End With
            cn.Close()
            Employees()
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs)
        txtEmpCode.Select()
        If DataGridView1.CurrentRow.Cells(4).Value > 0 Then
            If DataGridView1.Rows.Count > 0 Then
                DataGridView1.CurrentRow.Cells(4).Value = FormatNumber(CInt(DataGridView1.CurrentRow.Cells(4).Value) - 1, 0)
                'DataGridView1.CurrentRow.Cells(6).Value = FormatNumber(CDec(DataGridView1.CurrentRow.Cells(2).Value) + CInt(DataGridView1.CurrentRow.Cells(4).Value) * CDec(DataGridView1.CurrentRow.Cells(3).Value), 2)
                cn.Open()
                With cmd
                    .Connection = cn
                    .CommandText = "UPDATE scheduletable SET OvertimeHours = '" & CInt(DataGridView1.CurrentRow.Cells(4).Value) & "' WHERE emp_code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
                    .ExecuteNonQuery()
                End With
                cn.Close()
                Employees()
            End If
        End If
    End Sub

    Public EmpSchedule As Char
    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click
        EmpSchedule = "Y"
        ScheduleEmpl.ShowDialog()
    End Sub
End Class