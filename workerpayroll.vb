Imports System.Runtime.InteropServices
Imports MySql.Data.MySqlClient
Public Class workerpayroll
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager
    Dim second As Integer

    <DllImport("Gdi32.dll", EntryPoint:="CreateRoundRectRgn")>
    Private Shared Function CreateRoundRectRgn(ByVal iLeft As Integer, ByVal iTop As Integer, ByVal iRight As Integer, ByVal iBottom As Integer, ByVal iWidth As Integer, ByVal iHeight As Integer) As IntPtr
    End Function

    Dim loanBalance As String
    Private Sub workerpayroll_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Interval = 10
        Timer1.Start() 'Timer starts functioning
    End Sub

    Sub connect()
        Try
            cn.Open()
            Dim qry As String = "SELECT * FROM parameters"
            cmd = New MySqlCommand(qry, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                napsaval = dr.GetString("napsa").ToString
                napsaper = dr.GetString("napsaper").ToString
            End While
            cn.Close()

            lblDate.Text = Today.Date.ToString("yyyy-MM-dd")
            txtCode.Select()
            'MainInterface.SplitContainer1.Visible = False
            DataGridView1.RowTemplate.Height = 35
            Employeess()
            DataGridView1.Columns(0).Width = 100
            DataGridView1.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            txttax.Text = FormatCurrency(txttax.Text, 2)
            txtLoan.Text = FormatCurrency(txtLoan.Text, 2)
            txtOthers.Text = FormatCurrency(txtOthers.Text, 2)

            txtGrossPay.Text = FormatCurrency(txtGrossPay.Text, 2)
            txtnetpay.Text = FormatCurrency(txtnetpay.Text, 2)
            txtTotalDeduct.Text = FormatCurrency(txtTotalDeduct.Text, 2)

            Label12.AutoSize = False
            Label12.Padding = New Padding(1, 1, 1, 1)
            Label12.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label12.Width - 2, Label12.Height - 2, 5, 1))

        Catch ex As Exception
            MessageBox.Show(ex.Message, "System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)

    End Sub

    Dim desg As String
    Dim netpay As Decimal
    Dim enddate As String
    Dim napsaper As String
    Dim napsaval As String
    Public Sub EmployeeData()
        Try
            cn.Open()
            Dim tax As String = "SELECT * FROM employee WHERE code = '" & txtCode.Text & "'"
            cmd = New MySqlCommand(tax, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtworkerid.Text = dr.GetString("id_number").ToString & "" & dr.GetString("passport_num").ToString
                txtname.Text = dr.GetString("first_name").ToString & " " & dr.GetString("last_name").ToString
                txtratehour.Text = FormatCurrency(dr.GetString("rate_per_hour").ToString, 2)
                desg = dr.GetString("employed").ToString
                txtGrossPay.Text = FormatCurrency(dr.GetString("fixed_salary").ToString, 2)
                txtworkinghrs.Text = FormatNumber(dr.GetString("working_h_day").ToString, 2)
                dtpFrom.Text = dr.GetString("start_date").ToString()
                enddate = dr.GetString("end_date").ToString()
            End While
            cn.Close()

            If desg = "Y" Then
                txtdesignation.Text = "True"
                dtpTo.Text = Today.Date
            Else
                txtdesignation.Text = "False"
                dtpTo.Text = enddate
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub Employeess()
        Dim dt1 As New DataTable
        cn.Open()
        With cmd
            .Connection = cn
            .CommandText = "SELECT code As 'Code',CONCAT(first_name, ' ', last_name) As 'Name' FROM employee WHERE employed = 'Y'"
        End With
        da.SelectCommand = cmd
        dt1.Clear()
        da.Fill(dt1)
        DataGridView1.DataSource = dt1
        cn.Close()
    End Sub

    Private Sub workerpayroll_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        'MainInterface.SplitContainer1.Visible = True
    End Sub

    Dim value As String
    Dim grossamnt As String
    Public Sub TaxCal()
        Try
            cn.Open()
            Dim tax As String = "SELECT * FROM tax"
            cmd = New MySqlCommand(tax, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                value = dr.GetString("value").ToString
                grossamnt = dr.GetString("grosspaytaxable").ToString
            End While
            cn.Close()

            'MsgBox(value & " - " & grossamnt)
            If (decSalary <= 3300.0) Then
                txttax.Text = ((CDec(txtGrossPay.Text) * 12) * (0 / 100)) / 12
            ElseIf (decSalary > 3300.0 Or decSalary <= 4100.0) Then
                txttax.Text = ((CDec(txtGrossPay.Text) * 12) * (25 / 100)) / 12
            ElseIf (decSalary > 4100.0 Or decSalary <= 6200.0) Then
                txttax.Text = ((CDec(txtGrossPay.Text) * 12) * (30 / 100)) / 12
            ElseIf (decSalary > 6200.0) Then
                txttax.Text = ((CDec(txtGrossPay.Text) * 12) * (37.5 / 100)) / 12
                'decTax *= 37.5
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
        decSalary = CDec(txtGrossPay.Text) - 255.0
        decTax = decSalary * 0.2
        decTax2 = decSalary * 0.4
        decTax3 = decSalary * 0.45
        decAmount = decSalary - decTax


    End Sub

    Dim decSalary As String
    Dim decTax As String
    Dim decTax2 As String
    Dim decTax3 As String
    Dim decAmount As String
    Dim decNetPay As String
    Dim CaldecNetPay As String
    Dim tax1 As String
    Dim tax2 As String
    Dim tax3 As String
    Dim tax4 As String
    Sub TaxCalations()
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT e.fixed_salary,e.working_h_day,e.rate_per_day,e.code,e.id_number,e.passport_num,e.first_name,e.last_name, m.description As 'designation', d.Description As 'department',e.email
                            FROM employee e
                            LEFT JOIN departments d
                            ON e.department = d.Code
                            LEFT JOIN designation m
                            ON e.designation = m.CODE
                            WHERE e.code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"

            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                decNetPay = FormatCurrency(dr("fixed_salary").ToString(), 2)
            End While
            cn.Close()

            Calnapsaval = (CDec(napsaper) / 100) * CDec(decNetPay)

            If CDec(Calnapsaval) > CDec(napsaval) Then
                UseNAPSA = napsaval
            Else
                UseNAPSA = Calnapsaval
            End If

            CaldecNetPay = CDec(decNetPay) - CDec(UseNAPSA)
            tax1 = CDec(3300.0 * 0)
            tax2 = CDec(25 / 100) * CDec(4100.0 - 3300.0)
            tax3 = CDec(30 / 100) * CDec(6200.0 - 4100.0)
            tax4 = CDec(37.5 / 100) * (CDec(CaldecNetPay) - (3300.0 + (4100.0 - 3300.0) + (6200.0 - 4100.0)))
            Dim totaltax = CDec(tax1) + CDec(tax2) + CDec(tax3) + CDec(tax4)

            If decNetPay = CDec(0) Then
                txttax.Text = FormatCurrency(0, 2)
            Else
                txttax.Text = FormatCurrency(totaltax, 2)
            End If

            'MsgBox("Total Pay K" & CaldecNetPay & " Tax 1 K" & tax1 & " Tax 2 K" & tax2 & " Tax 3 K" & tax3 & " Tax 4 K" & tax4 & " Total Tax K" & totaltax)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub txtOthers_Leave_1(sender As Object, e As EventArgs)
        txtTotalDeduct.Text = FormatCurrency(CDec(txttax.Text) + CDec(txtLoan.Text) + CDec(txtOthers.Text), 2)
        txtOthers.Text = FormatCurrency(txtOthers.Text, 2)
    End Sub

    Private Sub txtCode_Leave(sender As Object, e As EventArgs) Handles txtCode.Leave
        If txtCode.Text <> "" Then
            EmployeeData()
            TaxCal()

            'txtTotalDeduct.Text = FormatCurrency(CDec(txttax.Text) + CDec(txtLoan.Text) + CDec(txtOthers.Text), 2)
            'txtnetpay.Text = FormatCurrency(CDec(txtGrossPay.Text) - CDec(txtTotalDeduct.Text), 2)
            'txtOthers.Text = FormatCurrency(txtOthers.Text, 2)
            'txttax.Text = FormatCurrency(txttax.Text, 2)
            'txtLoan.Text = FormatCurrency(txtLoan.Text, 2)
        End If
    End Sub

    Private Sub txttax_Leave(sender As Object, e As EventArgs)
        txtTotalDeduct.Text = FormatCurrency(CDec(txttax.Text) + CDec(txtLoan.Text) + CDec(txtOthers.Text), 2)
        txttax.Text = FormatCurrency(txttax.Text, 2)
    End Sub

    Private Sub txtPagIbig_Leave(sender As Object, e As EventArgs)
        txtTotalDeduct.Text = FormatCurrency(CDec(txttax.Text) + CDec(txtLoan.Text) + CDec(txtOthers.Text), 2)
        txtLoan.Text = FormatCurrency(txtLoan.Text, 2)
    End Sub

    Private Sub btnSearchWorkerID_Click(sender As Object, e As EventArgs)

    End Sub

    Dim Calnapsaval As Decimal
    Dim UseNAPSA As Decimal
    Dim loanexist As Char
    Sub DataLoad()
        txtOthers.Text = FormatCurrency(0, 2)
        txtLoan.Text = FormatCurrency(0, 2)
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT e.rate_per_hour,e.fixed_salary,e.working_h_day,e.rate_per_day,e.code,e.id_number,e.passport_num,e.first_name,e.last_name, m.description As 'designation', d.Description As 'department',e.email
                            FROM employee e
                            LEFT JOIN departments d
                            ON e.department = d.Code
                            LEFT JOIN designation m
                            ON e.designation = m.CODE
                            WHERE e.code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"

            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtCode.Text = dr("code").ToString()
                txtworkerid.Text = dr("id_number").ToString() & "" & dr("passport_num").ToString()
                txtname.Text = dr("first_name").ToString() & " " & dr("last_name").ToString()
                txtdesignation.Text = dr("department").ToString()
                txtratehour.Text = FormatCurrency(dr("rate_per_hour").ToString(), 2)
                'txtoverhour.Text = dr("code").ToString()
                'txtpresentdays.Text = dr("code").ToString()
                txtworkinghrs.Text = dr("working_h_day").ToString()
                'txtLoan.Text = dr("loan").ToString()
                txtGrossPay.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
            End While
            cn.Close()

            Calnapsaval = (CDec(napsaper) / 100) * CDec(txtGrossPay.Text)

            If CDec(Calnapsaval) > CDec(napsaval) Then
                UseNAPSA = napsaval
            Else
                UseNAPSA = Calnapsaval
            End If

            cn.Open()
            Dim qry1 As String = "SELECT balance FROM loans WHERE employee_code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
            cmd = New MySqlCommand(qry1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                loanBalance = dr.GetString("balance").ToString
            End While
            cn.Close()

            'TaxCal()
            cn.Open()
            Dim Query2 As String
            Query2 = "SELECT e.fixed_salary,e.working_h_day,e.rate_per_day,e.code,e.id_number,e.passport_num,e.first_name,e.last_name, m.description As 'designation', d.Description As 'department',e.email
                            FROM employee e
                            LEFT JOIN departments d
                            ON e.department = d.Code
                            LEFT JOIN designation m
                            ON e.designation = m.CODE
                            WHERE e.code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"

            cmd = New MySqlCommand(Query2, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                decNetPay = FormatCurrency(dr("fixed_salary").ToString(), 2)
            End While
            cn.Close()

            Calnapsaval = (CDec(napsaper) / 100) * CDec(decNetPay)

            If CDec(Calnapsaval) > CDec(napsaval) Then
                UseNAPSA = napsaval
            Else
                UseNAPSA = Calnapsaval
            End If

            CaldecNetPay = CDec(decNetPay) - CDec(UseNAPSA)
            tax1 = CDec(3300.0 * 0)
            tax2 = CDec(25 / 100) * CDec(4100.0 - 3300.0)
            tax3 = CDec(30 / 100) * CDec(6200.0 - 4100.0)
            tax4 = CDec(37.5 / 100) * (CDec(CaldecNetPay) - (3300.0 + (4100.0 - 3300.0) + (6200.0 - 4100.0)))
            Dim totaltax = CDec(tax1) + CDec(tax2) + CDec(tax3) + CDec(tax4)


            TaxCalations()
            txttax.Text = FormatCurrency(totaltax, 2)
            txtLoan.Text = FormatCurrency(loanBalance, 2)
            txtTotalDeduct.Text = FormatCurrency(CDec(txttax.Text) + CDec(txtLoan.Text) + CDec(txtOthers.Text), 2)
            txtnetpay.Text = FormatCurrency(CDec(txtGrossPay.Text) - CDec(txtTotalDeduct.Text), 2)
            txtOthers.Text = FormatCurrency(txtOthers.Text, 2)
            'txttax.Text = FormatCurrency(txttax.Text, 2)


            txtnapsa.Text = FormatCurrency(UseNAPSA, 2)
            If txtGrossPay.Text = CDec(0) Then
                txttax.Text = FormatCurrency(0, 2)
            Else
                txttax.Text = FormatCurrency(totaltax, 2)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        DataLoad()
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick

    End Sub

    Private Sub lblWorkerID_Click(sender As Object, e As EventArgs) Handles lblWorkerID.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label12.Select()
        Close()

        For Each ChildForm As Form In MainInterface.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'TaxCal()

        'txtTotalDeduct.Text = FormatCurrency(CDec(txttax.Text) + CDec(txtLoan.Text) + CDec(txtOthers.Text), 2)
        'txtnetpay.Text = FormatCurrency(CDec(txtGrossPay.Text) - CDec(txtTotalDeduct.Text), 2)
        'txtOthers.Text = FormatCurrency(txtOthers.Text, 2)
        'txttax.Text = FormatCurrency(txttax.Text, 2)
        'txtLoan.Text = FormatCurrency(txtLoan.Text, 2)
        TaxCalations()
    End Sub

    Private Sub btnViewRecord_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click
        Label12.Select()
        employeesearch.ShowDialog()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        second = second + 1
        If second >= 10 Then
            Timer1.Stop() 'Timer stops functioning
            'MsgBox("Timer Stopped....")
            Connect()
        End If
    End Sub
End Class