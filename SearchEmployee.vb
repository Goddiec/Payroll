Imports System.IO
Imports MySql.Data.MySqlClient
Public Class SearchEmployee
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
            If workerparyroll.payrollEmpSearch = "Y" Then
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
                    workerparyroll.txtCode.Text = dr("EmployeeID").ToString()
                    workerparyroll.txtworkerid.Text = dr("id_number").ToString()
                    workerparyroll.txtname.Text = dr("EmployeeName").ToString()
                    workerparyroll.txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                    workerparyroll.txtdesignation.Text = dr("Designation").ToString()
                    workerparyroll.txtratehour.Text = FormatCurrency(dr("rate_per_hour").ToString(), 2)
                    workerparyroll.txtoverhour.Text = FormatCurrency(dr("overtime_per_hour").ToString(), 2)
                    workerparyroll.txtworkinghrs.Text = FormatNumber(dr("working_h_day").ToString(), 0)
                    workerparyroll.txtLoans.Text = FormatCurrency(dr("loan").ToString(), 2)
                    workerparyroll.txtTotalDeductions.Text = FormatCurrency(CDec(workerparyroll.txtLoans.Text) + CDec(workerparyroll.txtOtherDeductions.Text), 2)
                    'workerparyroll.txtname.Text = dr("first_name").ToString() & " " & dr("last_name").ToString()
                    'workerparyroll.txtSalary.Text = FormatCurrency(dr("txtSalary").ToString(), 2)
                    'workerparyroll.txtCode.Text = dr("Code").ToString()
                    'workerparyroll.txtworkerid.Text = dr("id_number").ToString() & "" & dr("passport_num").ToString()
                    'workerparyroll.txtname.Text = dr("first_name").ToString() & " " & dr("last_name").ToString()
                    'workerparyroll.txtSalary.Text = FormatCurrency(dr("txtSalary").ToString(), 2)
                    'workerparyroll.txtCode.Text = dr("Code").ToString()
                    'workerparyroll.txtworkerid.Text = dr("id_number").ToString() & "" & dr("passport_num").ToString()
                    'workerparyroll.txtname.Text = dr("first_name").ToString() & " " & dr("last_name").ToString()
                    'workerparyroll.txtSalary.Text = FormatCurrency(dr("txtSalary").ToString(), 2)
                    'workerparyroll.txtCode.Text = dr("Code").ToString()
                    'workerparyroll.txtworkerid.Text = dr("id_number").ToString() & "" & dr("passport_num").ToString()
                    'workerparyroll.txtname.Text = dr("first_name").ToString() & " " & dr("last_name").ToString()
                    'workerparyroll.txtSalary.Text = FormatCurrency(dr("txtSalary").ToString(), 2)
                    'workerparyroll = dr("txtSalary").ToString()
                End While
                cn.Close()
                Close()
            ElseIf EmployeeBirthDaySelest.empBirth1 = "Y" Then
                EmployeeBirthDaySelest.txtCodeStart.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                EmployeeBirthDaySelest.empBirth1 = "N"
            ElseIf EmployeeBirthDaySelest.empBirth2 = "Y" Then
                EmployeeBirthDaySelest.txtCodeEnd.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                EmployeeBirthDaySelest.empBirth2 = "N"
            ElseIf EmployeeDetailsSelect.empDetails1 = "Y" Then
                EmployeeDetailsSelect.txtCodeStart.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                EmployeeDetailsSelect.empDetails1 = "N"
            ElseIf EmployeeDetailsSelect.empDetails2 = "Y" Then
                EmployeeDetailsSelect.txtCodeEnd.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                EmployeeDetailsSelect.empDetails2 = "N"
            ElseIf EmployeeListSelect.empList1 = "Y" Then
                EmployeeListSelect.txtCodeStart.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                EmployeeListSelect.empList1 = "N"
            ElseIf EmployeeListSelect.empList2 = "Y" Then
                EmployeeListSelect.txtCodeEnd.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                EmployeeListSelect.empList2 = "N"
            ElseIf HourInputSelect.hourInput1 = "Y" Then
                HourInputSelect.txtCodeStart.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                HourInputSelect.hourInput1 = "N"
            ElseIf HourInputSelect.hourInput2 = "Y" Then
                HourInputSelect.txtCodeEnd.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                HourInputSelect.hourInput2 = "N"
            ElseIf LeaveByMonthSelect.LeavByMon1 = "Y" Then
                LeaveByMonthSelect.txtCodeStart.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                LeaveByMonthSelect.LeavByMon1 = "N"
            ElseIf LeaveByMonthSelect.LeavByMon2 = "Y" Then
                LeaveByMonthSelect.txtCodeEnd.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                LeaveByMonthSelect.LeavByMon2 = "N"
            ElseIf LeaveHisSelect.LeavHis1 = "Y" Then
                LeaveHisSelect.txtCodeStart.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                LeaveHisSelect.LeavHis1 = "N"
            ElseIf LeaveHisSelect.LeavHis2 = "Y" Then
                LeaveHisSelect.txtCodeEnd.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                LeaveHisSelect.LeavHis2 = "N"
            ElseIf LoansSelect.LoanGiven1 = "Y" Then
                LoansSelect.txtCodeStart.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                LoansSelect.LoanGiven1 = "N"
            ElseIf LoansSelect.LoanGiven2 = "Y" Then
                LoansSelect.txtCodeEnd.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                LoansSelect.LoanGiven2 = "N"
            ElseIf payslipprevselect.paySlipPrev1 = "Y" Then
                payslipprevselect.txtCodeStart.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                payslipprevselect.paySlipPrev1 = "N"
            ElseIf payslipprevselect.paySlipPrev2 = "Y" Then
                payslipprevselect.txtCodeEnd.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                payslipprevselect.paySlipPrev2 = "N"
            ElseIf payslipselect.paySlipNow1 = "Y" Then
                payslipselect.txtCodeStart.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                payslipselect.paySlipNow1 = "N"
            ElseIf payslipselect.paySlipNow2 = "Y" Then
                payslipselect.txtCodeEnd.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                payslipselect.paySlipNow2 = "N"
            ElseIf retirementFundSelect.retireFund1 = "Y" Then
                retirementFundSelect.txtCodeStart.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                retirementFundSelect.retireFund1 = "N"
            ElseIf retirementFundSelect.retireFund2 = "Y" Then
                retirementFundSelect.txtCodeEnd.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                retirementFundSelect.retireFund2 = "N"
            ElseIf LeaveWindow.EmpLeaveSear = "Y" Then
                LeaveWindow.txtcode.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                LeaveWindow.EmpLeaveSear = "N"
            ElseIf Loans.EmpLoanSear = "Y" Then
                Loans.txtcode.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                Loans.EmpLoanSear = "N"
            ElseIf PayrollSchedule.EmpSchedule = "Y" Then
                PayrollSchedule.txtEmpCode.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                PayrollSchedule.EmpSchedule = "N"
            ElseIf scanDocuments.scanDoc = "Y" Then
                scanDocuments.txtEmp.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                scanDocuments.scanDoc = "N"
            ElseIf ExportEmployees.exportEmp1 = "Y" Then
                ExportEmployees.txtCodeStart.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                ExportEmployees.exportEmp1 = "N"
            ElseIf ExportEmployees.exportEmp2 = "Y" Then
                ExportEmployees.txtCodeEnd.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                ExportEmployees.exportEmp2 = "N"
            ElseIf NewEmployee.SerEmp1 = "Y" Then
                NewEmployee.txtFind.Text = DataGridView1.CurrentRow.Cells(0).Value
                Close()
                NewEmployee.SerEmp1 = "N"
            End If
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