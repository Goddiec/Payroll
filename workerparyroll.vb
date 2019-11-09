Imports System.Runtime.InteropServices
Imports MySql.Data.MySqlClient
Public Class workerparyroll
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

    Sub connect()
        Try
            txtCode.Select()
            txtSalary.Text = FormatCurrency(txtSalary.Text, 2)
            txtAllowance.Text = FormatCurrency(txtAllowance.Text, 2)
            txtRemuneration.Text = FormatCurrency(txtRemuneration.Text, 2)
            txtNAPSA.Text = FormatCurrency(txtNAPSA.Text, 2)
            txtUIF.Text = FormatCurrency(txtUIF.Text, 2)
            txtTotalContribution.Text = FormatCurrency(txtTotalContribution.Text, 2)
            txtRemun.Text = FormatCurrency(txtRemun.Text, 2)
            txtTotalTaxableRemun.Text = FormatCurrency(txtTotalTaxableRemun.Text, 2)
            lblDate.Text = Today.Date.ToShortDateString()
            txtLoans.Text = FormatCurrency(txtLoans.Text, 2)
            txtOtherDeductions.Text = FormatCurrency(txtOtherDeductions.Text, 2)
            txtTotalDeductions.Text = FormatCurrency(txtTotalDeductions.Text, 2)

            Label12.AutoSize = False
            Label12.Padding = New Padding(1, 1, 1, 1)
            Label12.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label12.Width - 2, Label12.Height - 2, 5, 1))
            DataGridView1.RowTemplate.Height = 35
            Employeess()
            DataGridView1.Columns(0).Width = 100
            DataGridView1.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            'Employeess1()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub connect1()
        Try
            txtCode.Select()
            txtSalary.Text = FormatCurrency(txtSalary.Text, 2)
            txtAllowance.Text = FormatCurrency(txtAllowance.Text, 2)
            txtRemuneration.Text = FormatCurrency(txtRemuneration.Text, 2)
            txtNAPSA.Text = FormatCurrency(txtNAPSA.Text, 2)
            txtUIF.Text = FormatCurrency(txtUIF.Text, 2)
            txtTotalContribution.Text = FormatCurrency(txtTotalContribution.Text, 2)
            txtRemun.Text = FormatCurrency(txtRemun.Text, 2)
            txtTotalTaxableRemun.Text = FormatCurrency(txtTotalTaxableRemun.Text, 2)
            lblDate.Text = Today.Date.ToShortDateString()
            txtLoans.Text = FormatCurrency(txtLoans.Text, 2)
            txtOtherDeductions.Text = FormatCurrency(txtOtherDeductions.Text, 2)
            txtTotalDeductions.Text = FormatCurrency(txtTotalDeductions.Text, 2)

            Label12.AutoSize = False
            Label12.Padding = New Padding(1, 1, 1, 1)
            Label12.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label12.Width - 2, Label12.Height - 2, 5, 1))
            Schedules()
        Catch ex As Exception
        MessageBox.Show(ex.Message, "System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
        cn.Dispose()
        End Try
    End Sub

    Sub Employeess()
        Try
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
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim process_date As String
    Dim status As String
    Public Sub Schedules()
        Try
            'cn.Open()
            'Dim Query As String
            'Query = "SELECT process_date FROM schedulelist"
            'cmd = New MySqlCommand(Query, cn)
            'dr = cmd.ExecuteReader
            'If dr.HasRows = True Then
            '    Itemexist = "Y"
            'Else
            '    Itemexist = "N"
            'End If
            'cn.Close()

            'If Itemexist = "Y" Then
            '    If CDate(process_date).ToShortDateString >= Today.Date.ToShortDateString Then
            '        status = "Normal"
            '    Else
            '        status = "Overdue"
            '    End If
            'ElseIf Itemexist = "N" Then

            'End If

            Dim dt1 As New DataTable
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT schedule_num As 'Number',process_date As 'Process Date',status As 'Status',schedule As 'Schedule',check_date As 'Check Date' FROM schedulelist"
            End With
            da.SelectCommand = cmd
            dt1.Clear()
            da.Fill(dt1)
            DataGridView2.DataSource = dt1
            cn.Close()

            DataGridView2.Columns(0).Width = 120
            DataGridView2.Columns(1).Width = 120
            DataGridView2.Columns(2).Width = 120
            DataGridView2.Columns(3).Width = 120
            DataGridView2.Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub SetFontAndColorNormal()
        With DataGridView2.DefaultCellStyle
            .ForeColor = Color.Black
            .SelectionForeColor = Color.White
            .SelectionBackColor = Color.Navy
        End With
    End Sub

    Private Sub SetFontAndColorRed()
        With DataGridView2.DefaultCellStyle
            .ForeColor = Color.Black
            .SelectionForeColor = Color.White
            .SelectionBackColor = Color.Red
        End With
    End Sub

    Private Sub DataGridView2_CellFormatting(ByVal sender As Object, ByVal e As DataGridViewCellFormattingEventArgs) Handles DataGridView2.CellFormatting
        For i As Integer = 0 To Me.DataGridView2.Rows.Count - 1
            If Me.DataGridView2.Rows(i).Cells(2).Value = "Overdue" Then
                Me.DataGridView2.Rows(i).Cells(0).Style.ForeColor = Color.Red
                Me.DataGridView2.Rows(i).Cells(1).Style.ForeColor = Color.Red
                Me.DataGridView2.Rows(i).Cells(2).Style.ForeColor = Color.Red
                Me.DataGridView2.Rows(i).Cells(3).Style.ForeColor = Color.Red
                Me.DataGridView2.Rows(i).Cells(4).Style.ForeColor = Color.Red
            ElseIf Me.DataGridView2.Rows(i).Cells(2).Value = "Normal" Then
                Me.DataGridView2.Rows(i).Cells(0).Style.ForeColor = Color.Black
                Me.DataGridView2.Rows(i).Cells(1).Style.ForeColor = Color.Black
                Me.DataGridView2.Rows(i).Cells(2).Style.ForeColor = Color.Black
                Me.DataGridView2.Rows(i).Cells(3).Style.ForeColor = Color.Black
                Me.DataGridView2.Rows(i).Cells(4).Style.ForeColor = Color.Black
            End If
        Next
    End Sub

    Sub Employeess1()
        Try
            Dim dt1 As New DataTable
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT e.code As 'Employee ID', CONCAT(e.first_name, ' ', e.last_name) AS 'Employee Name', m.description As 'Designation', d.Description As 'Department', 
                            case 
                            when e.employed = 'Y' 
                            then 'True' 
                            when e.employed = 'N' 
                            then 'False'
                            end As 'Active' FROM employee e
                            LEFT JOIN departments d
                            ON e.department = d.Code
                            LEFT JOIN designation m
                            ON e.designation = m.CODE
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
            MessageBox.Show(ex.Message, "System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub workerparyroll_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        connect1()
        clearData()
        Panel10.Width = Panel4.Width + 6
        DataGridView2.RowTemplate.Height = 30
        Label65.AutoSize = False
        Label65.Padding = New Padding(1, 1, 1, 1)
        Label65.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label65.Width - 2, Label65.Height - 2, 5, 1))

        Label67.AutoSize = False
        Label67.Padding = New Padding(1, 1, 1, 1)
        Label67.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label67.Width - 2, Label67.Height - 2, 5, 1))

        Panel10.AutoSize = False
        Panel10.Padding = New Padding(1, 1, 1, 1)
        Panel10.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Panel10.Width - 2, Panel10.Height - 2, 5, 1))

        Label4.AutoSize = False
        Label4.Padding = New Padding(1, 1, 1, 1)
        Label4.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label4.Width - 2, Label4.Height - 2, 5, 1))

        Label1.AutoSize = False
        Label1.Padding = New Padding(1, 1, 1, 1)
        Label1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label1.Width - 2, Label1.Height - 2, 5, 1))

        Label22.AutoSize = False
        Label22.Padding = New Padding(1, 1, 1, 1)
        Label22.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label22.Width - 2, Label22.Height - 2, 5, 1))

        Label65.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Panel10.BackColor = Color.Red

        'Panel2.Visible = True
        DataGridView1.Visible = False
        Panel4.Dock = DockStyle.Fill
        'Panel2.Location = New Point(0, 41)
        'Panel2.Dock = DockStyle.Fill

        Dim currentRegion = System.Globalization.RegionInfo.CurrentRegion.DisplayName
        'MsgBox(currentRegion)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label10.Select()
        Close()

        For Each ChildForm As Form In MainInterface.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub

    Private Sub txtpresentdays_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtdesignation_Click(sender As Object, e As EventArgs) Handles txtdesignation.Click

    End Sub

    Dim tax1 As String
    Dim tax2 As String
    Dim tax3 As String
    Dim tax4 As String
    Dim netpay As Decimal
    Dim enddate As String
    Dim napsaper As String
    Dim napsaval As String
    Dim CaldecNetPay As String
    Dim Calnapsaval As Decimal
    Dim UseNAPSA As Decimal
    Dim loanexist As Char
    Dim decNetPay As String
    Dim uif As String
    Dim caluif As String
    Sub dataLoadZambia()
        Try
            taxValues()
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT e.start_date,e.end_date,e.rate_per_hour,e.fixed_salary,e.working_h_day,e.rate_per_day,e.code,e.id_number,e.passport_num,e.first_name,e.last_name, m.description As 'designation', d.Description As 'department',e.email
                            FROM employee e
                            LEFT JOIN departments d
                            ON e.department = d.Code
                            LEFT JOIN designation m
                            ON e.designation = m.CODE
                            WHERE e.code = '" & txtCode.Text & "'"

            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtCode.Text = dr("code").ToString()
                txtworkerid.Text = dr("id_number").ToString() & "" & dr("passport_num").ToString()
                txtname.Text = dr("first_name").ToString() & " " & dr("last_name").ToString()
                txtdesignation.Text = dr("department").ToString()
                txtratehour.Text = FormatCurrency(dr("rate_per_hour").ToString(), 2)
                txtworkinghrs.Text = dr("working_h_day").ToString()
                txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                decNetPay = dr("fixed_salary").ToString()
                dtpFrom.Text = dr.GetString("start_date").ToString()
                enddate = dr.GetString("end_date").ToString()
                'txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                'txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                'txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                'txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                'txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
            End While
            cn.Close()

            cn.Open()
            Dim qry As String = "SELECT * FROM parameters"
            cmd = New MySqlCommand(qry, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                napsaval = dr.GetString("napsa").ToString
                napsaper = dr.GetString("napsaper").ToString
                uif = dr("uif").ToString()
            End While
            cn.Close()

            caluif = (CDec(uif) / 100) * CDec(decNetPay)
            txtUIF.Text = FormatCurrency(caluif, 2)
            Calnapsaval = (CDec(napsaper) / 100) * CDec(decNetPay)

            If CDec(Calnapsaval) > CDec(napsaval) Then
                UseNAPSA = napsaval
            Else
                UseNAPSA = Calnapsaval
            End If

            txtEmpNAPSA.Text = FormatCurrency(UseNAPSA, 2)
            txtNAPSA.Text = FormatCurrency(UseNAPSA, 2)
            txtAllowance.Text = FormatCurrency(txtAllowance.Text, 2)
            txtRemuneration.Text = FormatCurrency(CDec(txtSalary.Text) + CDec(txtAllowance.Text), 2)
            txtTotalContribution.Text = FormatCurrency(CDec(txtNAPSA.Text) + CDec(txtUIF.Text), 2)
            txtRemun.Text = FormatCurrency(CDec(txtSalary.Text) + CDec(txtAllowance.Text), 2)
            txtGrossSalary.Text = FormatCurrency(txtRemuneration.Text, 2)
            CaldecNetPay = CDec(txtRemuneration.Text) - (CDec(UseNAPSA) + CDec(caluif))
            'tax1 = CDec(3300.0 * 0)
            'tax2 = CDec(25 / 100) * CDec(4100.0 - 3300.0)
            'tax3 = CDec(30 / 100) * CDec(6200.0 - 4100.0)
            'tax4 = CDec(37.5 / 100) * (CDec(CaldecNetPay) - (3300.0 + (4100.0 - 3300.0) + (6200.0 - 4100.0)))
            tax1 = CDec(taxGross1) * CDec(taxValue1 / 100)
            tax2 = (CDec(taxValue2) / 100) * (CDec(taxGross2) - CDec(taxGross1))
            tax3 = (CDec(taxValue3) / 100) * (CDec(taxGross3) - CDec(taxGross2))
            tax4 = (CDec(taxValue4) / 100) * (CDec(CaldecNetPay) - (CDec(taxGross1) + (CDec(taxGross2) - CDec(taxGross1)) + (CDec(taxGross3) - CDec(taxGross2))))
            Dim totaltax = CDec(tax1) + CDec(tax2) + CDec(tax3) + CDec(tax4)

            If decNetPay = CDec(0) Then
                txtPaye.Text = FormatCurrency(0, 2)
            Else
                txtPaye.Text = FormatCurrency(totaltax, 2)
            End If

            txtNetIncome.Text = FormatCurrency(CDec(txtTotalTaxableRemun.Text) - (CDec(txtTotalContribution.Text) + CDec(txtPaye.Text)), 2)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim taxValue1 As String
    Dim taxValue2 As String
    Dim taxValue3 As String
    Dim taxValue4 As String
    Dim taxValue5 As String
    Dim taxValue6 As String
    Dim taxValue7 As String
    Dim taxValue8 As String
    Dim taxValue9 As String
    Dim taxValue10 As String
    Dim taxGross1 As String
    Dim taxGross2 As String
    Dim taxGross3 As String
    Dim taxGross4 As String
    Dim taxGross5 As String
    Dim taxGross6 As String
    Dim taxGross7 As String
    Dim taxGross8 As String
    Dim taxGross9 As String
    Dim taxGross10 As String
    Sub taxValues()
        Try
            cn.Open()
            Dim qry As String = "SELECT * FROM tax WHERE ID = 1"
            cmd = New MySqlCommand(qry, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                taxValue1 = dr("taxValue1").ToString
                taxValue2 = dr("taxValue2").ToString
                taxValue3 = dr("taxValue3").ToString()
                taxValue4 = dr("taxValue4").ToString()
                taxValue5 = dr("taxValue5").ToString()
                taxValue6 = dr("taxValue6").ToString()
                taxValue7 = dr("taxValue7").ToString()
                taxValue8 = dr("taxValue8").ToString()
                taxValue9 = dr("taxValue9").ToString()
                taxValue10 = dr("taxValue10").ToString()
                taxGross1 = dr("taxGross1").ToString()
                taxGross2 = dr("taxGross2").ToString()
                taxGross3 = dr("taxGross3").ToString()
                taxGross4 = dr("taxGross4").ToString()
                taxGross5 = dr("taxGross5").ToString()
                taxGross6 = dr("taxGross6").ToString()
                taxGross7 = dr("taxGross7").ToString()
                taxGross8 = dr("taxGross8").ToString()
                taxGross9 = dr("taxGross9").ToString()
                taxGross10 = dr("taxGross10").ToString()
            End While
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub dataLoadRwanda1()
        Try
            taxValues()
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT e.start_date,e.end_date,e.rate_per_hour,e.fixed_salary,e.working_h_day,e.rate_per_day,e.code,e.id_number,e.passport_num,e.first_name,e.last_name, m.description As 'designation', d.Description As 'department',e.email
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
                txtworkinghrs.Text = dr("working_h_day").ToString()
                txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                decNetPay = dr("fixed_salary").ToString()
                dtpFrom.Text = dr.GetString("start_date").ToString()
            End While
            cn.Close()

            cn.Open()
            Dim qry As String = "SELECT * FROM parameters"
            cmd = New MySqlCommand(qry, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                napsaval = dr.GetString("napsa").ToString
                napsaper = dr.GetString("napsaper").ToString
                uif = dr("uif").ToString()
            End While
            cn.Close()

            'caluif = (CDec(uif) / 100) * CDec(decNetPay)
            'txtUIF.Text = FormatCurrency(caluif, 2)
            'Calnapsaval = (CDec(napsaper) / 100) * CDec(decNetPay)

            'If CDec(Calnapsaval) > CDec(napsaval) Then
            '    UseNAPSA = napsaval
            'Else
            '    UseNAPSA = Calnapsaval
            'End If

            'txtEmpNAPSA.Text = FormatCurrency(UseNAPSA, 2)
            'txtNAPSA.Text = FormatCurrency(UseNAPSA, 2)
            'txtAllowance.Text = FormatCurrency(txtAllowance.Text, 2)
            'txtRemuneration.Text = FormatCurrency(CDec(txtSalary.Text) + CDec(txtAllowance.Text), 2)
            'txtTotalContribution.Text = FormatCurrency(CDec(txtNAPSA.Text) + CDec(txtUIF.Text), 2)
            'txtRemun.Text = FormatCurrency(CDec(txtSalary.Text) + CDec(txtAllowance.Text), 2)
            'txtGrossSalary.Text = FormatCurrency(decNetPay, 2)
            'CaldecNetPay = CDec(decNetPay) - (CDec(UseNAPSA) + CDec(caluif))
            'tax1 = CDec(taxGross1) * CDec(taxValue1 / 100)
            'tax2 = (CDec(taxValue2) / 100) * (CDec(taxGross2) - CDec(taxGross1))
            'tax3 = (CDec(taxValue3) / 100) * (CDec(taxGross3) - CDec(taxGross2))
            'tax4 = (CDec(taxValue4) / 100) * (CDec(CaldecNetPay) - (CDec(taxGross1) + (CDec(taxGross2) - CDec(taxGross1)) + (CDec(taxGross3) - CDec(taxGross2))))
            'Dim totaltax = CDec(tax1) + CDec(tax2) + CDec(tax3) + CDec(tax4)

            'If decNetPay = CDec(0) Then
            '    txtPaye.Text = FormatCurrency(0, 2)
            'Else
            '    txtPaye.Text = FormatCurrency(totaltax, 2)
            'End If

            'txtNetIncome.Text = FormatCurrency(CDec(txtTotalTaxableRemun.Text) - (CDec(txtTotalContribution.Text) + CDec(txtPaye.Text)), 2)
            Dim Rtax As Decimal
            If CDec(decNetPay) <= CDec(taxGross1) Then
                'Rtax =
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub dataLoadZambia1()
        Try
            taxValues()
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT e.start_date,e.end_date,e.rate_per_hour,e.fixed_salary,e.working_h_day,e.rate_per_day,e.code,e.id_number,e.passport_num,e.first_name,e.last_name, m.description As 'designation', d.Description As 'department',e.email
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
                txtworkinghrs.Text = dr("working_h_day").ToString()
                txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                decNetPay = dr("fixed_salary").ToString()
                dtpFrom.Text = dr.GetString("start_date").ToString()
                'enddate = dr.GetString("end_date").ToString()
                'txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                'txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                'txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                'txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                'txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
            End While
            cn.Close()

            cn.Open()
            Dim qry As String = "SELECT * FROM parameters"
            cmd = New MySqlCommand(qry, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                napsaval = dr.GetString("napsa").ToString
                napsaper = dr.GetString("napsaper").ToString
                uif = dr("uif").ToString()
            End While
            cn.Close()

            caluif = (CDec(uif) / 100) * CDec(decNetPay)
            txtUIF.Text = FormatCurrency(caluif, 2)
            Calnapsaval = (CDec(napsaper) / 100) * CDec(decNetPay)

            If CDec(Calnapsaval) > CDec(napsaval) Then
                UseNAPSA = napsaval
            Else
                UseNAPSA = Calnapsaval
            End If

            txtEmpNAPSA.Text = FormatCurrency(UseNAPSA, 2)
            txtNAPSA.Text = FormatCurrency(UseNAPSA, 2)
            txtAllowance.Text = FormatCurrency(txtAllowance.Text, 2)
            txtRemuneration.Text = FormatCurrency(CDec(txtSalary.Text) + CDec(txtAllowance.Text), 2)
            txtTotalContribution.Text = FormatCurrency(CDec(txtNAPSA.Text) + CDec(txtUIF.Text), 2)
            txtRemun.Text = FormatCurrency(CDec(txtSalary.Text) + CDec(txtAllowance.Text), 2)
            txtGrossSalary.Text = FormatCurrency(decNetPay, 2)
            CaldecNetPay = CDec(decNetPay) - (CDec(UseNAPSA) + CDec(caluif))
            tax1 = CDec(taxGross1) * CDec(taxValue1 / 100)
            tax2 = (CDec(taxValue2) / 100) * (CDec(taxGross2) - CDec(taxGross1))
            tax3 = (CDec(taxValue3) / 100) * (CDec(taxGross3) - CDec(taxGross2))
            tax4 = (CDec(taxValue4) / 100) * (CDec(CaldecNetPay) - (CDec(taxGross1) + (CDec(taxGross2) - CDec(taxGross1)) + (CDec(taxGross3) - CDec(taxGross2))))
            Dim totaltax = CDec(tax1) + CDec(tax2) + CDec(tax3) + CDec(tax4)

            If decNetPay = CDec(0) Then
                txtPaye.Text = FormatCurrency(0, 2)
            Else
                txtPaye.Text = FormatCurrency(totaltax, 2)
            End If

            txtNetIncome.Text = FormatCurrency(CDec(txtTotalTaxableRemun.Text) - (CDec(txtTotalContribution.Text) + CDec(txtPaye.Text)), 2)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim napsaAmnt As String
    Dim napasPer As String
    Dim napasValue As String
    Sub MyData()
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT e.code As 'EmployeeID',e.loan,e.id_number,e.working_h_day,e.fixed_salary,p.uif,e.rate_per_hour,p.napsa,p.napsaper,p.overtime_per_hour, CONCAT(e.first_name, ' ', e.last_name) AS 'EmployeeName', m.description As 'Designation'
                          FROM employee e
                          LEFT JOIN designation m
                          ON e.designation = m.CODE 
                          JOIN parameters p
                          WHERE e.code = '" & Trim(txtCode.Text) & "'"
            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtCode.Text = dr("EmployeeID").ToString()
                txtworkerid.Text = dr("id_number").ToString()
                txtworkerid.Text = dr("EmployeeID").ToString()
                txtname.Text = dr("EmployeeName").ToString()
                txtSalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                txtdesignation.Text = dr("Designation").ToString()
                txtratehour.Text = FormatCurrency(dr("rate_per_hour").ToString(), 2)
                txtoverhour.Text = FormatCurrency(dr("overtime_per_hour").ToString(), 2)
                txtworkinghrs.Text = FormatNumber(dr("working_h_day").ToString(), 0)
                txtLoans.Text = FormatCurrency(dr("loan").ToString(), 2)
                txtTotalDeductions.Text = FormatCurrency(CDec(txtLoans.Text) + CDec(txtOtherDeductions.Text), 2)
                napasPer = (CDec(dr("napsaper").ToString()) / 100) * CDec(dr("fixed_salary").ToString())
                napasValue = CDec(dr("napsa").ToString())
                txtUIF.Text = FormatCurrency((CDec(dr("uif").ToString()) / 100) * CDec(dr("fixed_salary").ToString()))
                If napasPer > napasValue Then
                    napsaAmnt = napasValue
                Else
                    napsaAmnt = napasPer
                End If


                txtEmpNAPSA.Text = FormatCurrency(napsaAmnt, 2)
                txtAllowance.Text = FormatCurrency(0, 2)
                txtRemuneration.Text = FormatCurrency(CDec(txtSalary.Text) + CDec(txtAllowance.Text), 2)
                txtNAPSA.Text = FormatCurrency(napsaAmnt, 2)
                txtTotalContribution.Text = FormatCurrency(CDec(txtNAPSA.Text) + CDec(txtUIF.Text), 2)

                txtRemun.Text = FormatCurrency(txtRemuneration.Text, 2)
                txtTotalTaxableRemun.Text = FormatCurrency(CDec(txtRemuneration.Text) - CDec(txtTotalContribution.Text), 2)
                txtDeductions.Text = FormatCurrency(CDec(txtTotalContribution.Text) + CDec(txtTotalDeductions.Text), 2)
                txtGrossSalary.Text = FormatCurrency(txtRemuneration.Text, 2)
            End While
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim Itemexist As Char
    Private Sub CheckItem()
        Try
            cn.Open()
            Dim Query As String
            Query = "SELECT code FROM employee WHERE code = '" & Trim(txtCode.Text) & "'"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                Itemexist = "Y"
            Else
                Itemexist = "N"
            End If
            cn.Close()

            If Itemexist = "Y" Then
                'dataLoad()
                MyData()
            ElseIf Itemexist = "N" Then
                MessageBox.Show("Employee code " & txtCode.Text & " does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtCode.Clear()
                txtCode.Select()
                clearData()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub clearData()
        txtSalary.Text = FormatCurrency(0, 2)
        txtAllowance.Text = FormatCurrency(0, 2)
        txtRemuneration.Text = FormatCurrency(0, 2)
        txtNAPSA.Text = FormatCurrency(0, 2)
        txtUIF.Text = FormatCurrency(0, 2)
        txtTotalContribution.Text = FormatCurrency(0, 2)
        txtLoans.Text = FormatCurrency(0, 2)
        txtOtherDeductions.Text = FormatCurrency(0, 2)
        txtTotalDeductions.Text = FormatCurrency(0, 2)
        txtRemun.Text = FormatCurrency(0, 2)
        txtTotalTaxableRemun.Text = FormatCurrency(0, 2)
        txtworkerid.Text = ""
        txtname.Text = ""
        txtdesignation.Text = ""
        txtratehour.Text = FormatCurrency(0, 2)
        txtoverhour.Text = FormatCurrency(0, 2)
        txtworkinghrs.Text = FormatNumber(0, 2)
        txtGrossSalary.Text = FormatCurrency(0, 2)
        txtDeductions.Text = FormatCurrency(0, 2)
        txtEmpNAPSA.Text = FormatCurrency(0, 2)
        txtPaye.Text = FormatCurrency(0, 2)
        txtNetIncome.Text = FormatCurrency(0, 2)
    End Sub

    Sub Freezing()
        DataGridView1.Enabled = True

    End Sub

    Private Sub txtCode_Leave(sender As Object, e As EventArgs) Handles txtCode.Leave
        If txtCode.Text <> String.Empty Then
            CheckItem()
            Freezing()
        Else
            clearData()
        End If
    End Sub

    Private Sub txtAllowance_Leave(sender As Object, e As EventArgs) Handles txtAllowance.Leave
        txtRemuneration.Text = FormatCurrency(CDec(txtSalary.Text) + CDec(txtAllowance.Text), 2)
        txtRemun.Text = FormatCurrency(CDec(txtSalary.Text) + CDec(txtAllowance.Text), 2)
        txtAllowance.Text = FormatCurrency(txtAllowance.Text, 2)
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        dataLoadZambia1()
        txtAllowance.Select()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        second = second + 1
        If second >= 10 Then
            Timer1.Stop()
            connect()
        End If
    End Sub

    Private Sub txtLoans_Leave(sender As Object, e As EventArgs) Handles txtLoans.Leave
        If txtLoans.Text = String.Empty Then
            txtLoans.Text = FormatCurrency(0, 2)
        Else
            txtTotalDeductions.Text = FormatCurrency(CDec(txtLoans.Text) + CDec(txtOtherDeductions.Text), 2)
            txtRemun.Text = FormatCurrency((CDec(txtSalary.Text) + CDec(txtAllowance.Text)) - (CDec(txtLoans.Text) + CDec(txtOtherDeductions.Text)), 2)
            txtLoans.Text = FormatCurrency(txtLoans.Text, 2)
        End If
    End Sub

    Private Sub txtOtherDeductions_Leave(sender As Object, e As EventArgs) Handles txtOtherDeductions.Leave
        If txtOtherDeductions.Text = "" Then
            txtOtherDeductions.Text = FormatCurrency(0, 2)
        Else
            txtTotalDeductions.Text = FormatCurrency(CDec(txtLoans.Text) + CDec(txtOtherDeductions.Text), 2)
            txtRemun.Text = FormatCurrency((CDec(txtSalary.Text) + CDec(txtAllowance.Text)) - (CDec(txtLoans.Text) + CDec(txtOtherDeductions.Text)), 2)
            txtOtherDeductions.Text = FormatCurrency(txtOtherDeductions.Text, 2)

            txtTotalTaxableRemun.Text = FormatCurrency(CDec(txtRemuneration.Text) - CDec(txtTotalContribution.Text), 2)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label10.Select()
        If txtCode.Text <> String.Empty Then
            txtNetIncome.Text = FormatCurrency(CDec(txtTotalTaxableRemun.Text) - (CDec(txtTotalContribution.Text) + CDec(txtPaye.Text)), 2)
        End If
    End Sub

    Sub EmployeessSearch()
        Try
            Dim dt1 As New DataTable
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT code As 'Code',CONCAT(first_name, ' ', last_name) As 'Name' FROM employee WHERE employed = 'Y'
                                AND code LIKE '%" & txtCode.Text & "%'
                                ORDER BY code"
            End With
            da.SelectCommand = cmd
            dt1.Clear()
            da.Fill(dt1)
            DataGridView1.DataSource = dt1
            cn.Close()

            DataGridView1.Columns(0).Width = 100
            DataGridView1.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub txtCode_TextChanged(sender As Object, e As EventArgs) Handles txtCode.TextChanged
        'If txtCode.TextLength >= 3 Then
        '    EmployeessSearch()
        'End If
    End Sub

    Private Sub Label65_Click(sender As Object, e As EventArgs) Handles Label65.Click

        Label65.BackColor = Color.Red
        Label67.BackColor = Color.Lime
        Label16.BackColor = Color.Lime

        DataGridView1.Visible = True
        Panel4.Visible = False
        Panel6.Visible = True
        'Panel2.Location = New Point(0, 41)
        DataGridView1.Dock = DockStyle.Fill
        Timer1.Interval = 10
        Timer1.Start()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Label16_Click(sender As Object, e As EventArgs) Handles Label16.Click
        Label65.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Panel10.BackColor = Color.Red

        DataGridView1.Visible = False
        Panel4.Visible = True
        Panel6.Visible = False

        'Panel2.Location = New Point(0, 41)
        Panel4.Dock = DockStyle.Fill
        'Schedules()
    End Sub

    Private Sub Label67_Click(sender As Object, e As EventArgs) Handles Label67.Click
        Label65.BackColor = Color.Lime
        Label67.BackColor = Color.Red
        Label16.BackColor = Color.Lime

        DataGridView1.Visible = False
        Panel4.Visible = False
        'Panel2.Location = New Point(0, 41)
        DataGridView1.Dock = DockStyle.None
    End Sub

    Private Sub DataGridView2_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellDoubleClick
        'PayrollSchedule.WindowState = FormWindowState.Maximized
        'PayrollSchedule.Show()
        'PayrollSchedule.MdiParent = MainInterface
    End Sub

    Sub Delete()
        If DataGridView2.Rows.Count > 0 Then
            Dim dialog As New DialogResult
            dialog = MsgBox("Are you sure you want to delete this Payroll Schedule?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Payroll Schedule")

            If dialog = DialogResult.No Then

            Else
                Try
                    cn.Open()
                    With cmd
                        .Connection = cn
                        .CommandText = "DELETE FROM scheduletable WHERE code = '" & DataGridView2.CurrentRow.Cells(0).Value & "'"
                        .ExecuteNonQuery()
                    End With
                    cn.Close()

                    cn.Open()
                    With cmd
                        .Connection = cn
                        .CommandText = "DELETE FROM schedulelist WHERE schedule_num = '" & DataGridView2.CurrentRow.Cells(0).Value & "'"
                        .ExecuteNonQuery()
                    End With
                    cn.Close()

                    Schedules()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    cn.Dispose()
                End Try
            End If
        End If
    End Sub

    Private Sub Label22_Click(sender As Object, e As EventArgs) Handles Label22.Click

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        If DataGridView2.Rows.Count > 0 Then
            PayrollSchedule.WindowState = FormWindowState.Maximized
            PayrollSchedule.Show()
            PayrollSchedule.MdiParent = MainInterface
        End If
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub DeleteScheduleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteScheduleToolStripMenuItem.Click
        Delete()
    End Sub

    Private Sub NewScheduleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewScheduleToolStripMenuItem.Click
        saveSchedule.ShowDialog()
    End Sub

    Private Sub EditScheduleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditScheduleToolStripMenuItem.Click
        ScheduleEdit.ShowDialog()
    End Sub

    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick

    End Sub

    Public payrollEmpSearch As Char
    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click
        payrollEmpSearch = "Y"
        SearchEmployee.ShowDialog()
    End Sub

    Private Async Sub Flash()
        While True
            Await Task.Delay(100)
            Label16.Visible = Not Label1.Visible
        End While
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        'If Label16.Left >= Me.Width Then
        '    Label16.Left = 1
        'Else
        '    Label16.Left = Label16.Left + 60
        'End If
        'If txtCode.Text = "" Then
        '    Label37.Enabled = True
        '    Label37.BackColor = Color.Black
        'Else
        '    Label37.Enabled = False
        '    Label37.BackColor = Color.Gray
        'End If
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub workerparyroll_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtCode.Text <> String.Empty Then
                CheckItem()
            Else
                clearData()
            End If
        End If
    End Sub

    Private Sub Label23_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label37_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label23_DoubleClick(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label36_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        'ContextMenuStrip1.Show(Button5, 1, -70)
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs)

    End Sub
End Class