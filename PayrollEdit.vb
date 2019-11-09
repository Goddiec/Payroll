Imports System.IO
Imports MySql.Data.MySqlClient
Public Class PayrollEdit
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager
    Dim second As Integer

    Private Sub imageRet()
        Try
            cn.Open()
            Dim cmd1 As MySqlCommand
            cmd1 = New MySqlCommand("Select ImageFile from employee where code = '" & PayrollSchedule.DataGridView1.CurrentRow.Cells(0).Value & "'", cn)
            Dim imageData As Byte() = DirectCast(cmd1.ExecuteScalar(), Byte())

            If Not imageData Is Nothing Then
                Using ms As New MemoryStream(imageData, 0, imageData.Length)
                    ms.Write(imageData, 0, imageData.Length)

                    PictureBox1.Image = Image.FromStream(ms, True)
                End Using
            End If
            cn.Close()
        Catch ex As Exception
            PictureBox1.Image = Nothing
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim daysPerMonth As String
    Dim code As String
    Dim basicSalary As String
    Dim paidunpaid As Char
    Dim napsa As String
    Dim napsaper As String
    Dim totalnapsa As String
    Dim mynapsa As String
    Dim uif As String
    Dim uifAmount As String
    Dim overTimeHour As String
    Dim designation As String
    Sub LoadData()
        Try
            txtOtherMonhtlyIncome.Text = FormatCurrency(0, 2)
            txtIncomeDeduction.Text = FormatCurrency(0, 2)
            txtTaxbleCapitalGain.Text = FormatCurrency(0, 2)

            imageRet()
            cn.Open()
            Dim qry As String = "SELECT * FROM employee WHERE code = '" & PayrollSchedule.DataGridView1.CurrentRow.Cells(0).Value & "'"
            cmd = New MySqlCommand(qry, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Emp_Code.Text = dr.GetString("code").ToString
                lblSearchWorkerID.Text = dr.GetString("title").ToString & " " & dr.GetString("first_name").ToString & " " & dr.GetString("last_name").ToString
                code = dr.GetString("code").ToString
                'txtRate.Text = FormatCurrency(dr("rate_per_hour").ToString(), 2)
                'txtHour.Text = FormatNumber(dr("working_h_day").ToString(), 0)
                'daysPerMonth = dr("avrg_working_d_month").ToString()
                'basicSalary = dr("fixed_salary").ToString()
                txtMonthlySalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                txtPensionCont.Text = FormatCurrency(dr("pension").ToString(), 2)
                txtRiskPer.Text = FormatNumber(dr("riskbenefits").ToString(), 2)
                txtRetirementPer.Text = FormatNumber(dr("retirementfund").ToString(), 2)
                txtMedicalAid.Text = FormatCurrency(dr("medicalaid").ToString(), 2)
                txtOutofPocket.Text = FormatCurrency(dr("pocketexpense").ToString(), 2)
                txtNoofDependants.Text = FormatNumber(dr("dependantnum").ToString(), 0)
                Label32.Text = dr("id_number").ToString() & "" & dr("passport_num").ToString()
                designation = dr("designation").ToString()
                Label35.Text = FormatCurrency(dr("rate_per_hour").ToString(), 2)
                Label39.Text = FormatCurrency(dr("rate_per_day").ToString(), 2)
            End While
            cn.Close()

            cn.Open()
            Dim qry2 As String = "SELECT description FROM designation WHERE Code = '" & designation & "'"
            cmd = New MySqlCommand(qry2, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Label33.Text = dr("description").ToString().ToUpper
            End While
            cn.Close()

            'txtOvertime.Text = FormatNumber(0, 0)
            cn.Open()
            Dim qry3 As String = "SELECT * FROM scheduletable WHERE emp_code = '" & PayrollSchedule.DataGridView1.CurrentRow.Cells(0).Value & "'"
            cmd = New MySqlCommand(qry3, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtOvertime.Text = FormatNumber(dr("OvertimeHours").ToString(), 0)
                txtLeave.Text = FormatNumber(dr("LeaveHours").ToString(), 0)
            End While
            cn.Close()

            cn.Open()
            Dim qry5 As String = "SELECT * FROM otherincome WHERE emp_code = '" & Emp_Code.Text & "'"
            cmd = New MySqlCommand(qry5, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtOtherMonhtlyIncome.Text = FormatCurrency(dr("OtherIncome").ToString(), 2)
                txtIncomeDeduction.Text = FormatCurrency(dr("DeductOtherIncomce").ToString(), 2)
                txtTaxbleCapitalGain.Text = FormatCurrency(dr("CapitalGain").ToString(), 2)
                txtLoans.Text = FormatCurrency(dr("Loans").ToString(), 2)
            End While
            cn.Close()

            cn.Open()
            Dim qry4 As String = "SELECT * FROM parameters"
            cmd = New MySqlCommand(qry4, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                napsa = dr("napsa").ToString()
                napsaper = dr("napsaper").ToString()
                uif = dr("uif").ToString()
                overTimeHour = dr("overtime_per_hour").ToString()
            End While
            cn.Close()

            Label37.Text = FormatCurrency(overTimeHour, 2)
            totalnapsa = (CDec(napsaper) / 100) * CDec(txtMonthlySalary.Text)

            If CDec(totalnapsa) > CDec(napsa) Then
                mynapsa = napsa
            Else
                mynapsa = totalnapsa
            End If

            uifAmount = (CDec(uif) / 100) * CDec(txtMonthlySalary.Text)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                                e.fixed_salary As 'Regular Pay',p.overtime_per_hour As 'Overtime Rate', s.OvertimeHours As 'Overtime Hours', s.LeaveHours As 'Leave Hours', ((s.OvertimeHours * p.overtime_per_hour) + (e.fixed_salary))-(p.overtime_per_hour * s.LeaveHours) As 'Salary'
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
            PayrollSchedule.DataGridView1.DataSource = dt1
            cn.Close()

            PayrollSchedule.DataGridView1.Columns(0).Width = 150
            PayrollSchedule.DataGridView1.Columns(1).Width = 203
            PayrollSchedule.DataGridView1.Columns(2).Width = 150
            PayrollSchedule.DataGridView1.Columns(3).Width = 150
            PayrollSchedule.DataGridView1.Columns(4).Width = 150
            PayrollSchedule.DataGridView1.Columns(5).Width = 150
            PayrollSchedule.DataGridView1.Columns(6).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            PayrollSchedule.DataGridView1.Columns(2).DefaultCellStyle.Format = "c"
            PayrollSchedule.DataGridView1.Columns(3).DefaultCellStyle.Format = "c"
            PayrollSchedule.DataGridView1.Columns(6).DefaultCellStyle.Format = "c"

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

    Private Sub PayrollEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Label1.Select()
        LoadData()
        FormatTextValues()
        txtOtherMonhtlyIncome.Select()
        calculations()
    End Sub

    Sub FormatTextValues()
        txtMonthlySalary.Text = FormatCurrency(txtMonthlySalary.Text, 2)
        txtOtherMonhtlyIncome.Text = FormatCurrency(txtOtherMonhtlyIncome.Text, 2)
        txtIncomeDeduction.Text = FormatCurrency(txtIncomeDeduction.Text, 2)
        txtTaxbleCapitalGain.Text = FormatCurrency(txtTaxbleCapitalGain.Text, 2)
        txtLoans.Text = FormatCurrency(txtLoans.Text, 2)
        txtPensionCont.Text = FormatCurrency(txtPensionCont.Text, 2)
        txtRiskPer.Text = FormatNumber(txtRiskPer.Text, 2)
        txtRetirementPer.Text = FormatNumber(txtRetirementPer.Text, 2)
        txtMedicalAid.Text = FormatCurrency(txtMedicalAid.Text, 2)
        txtOutofPocket.Text = FormatCurrency(txtOutofPocket.Text, 2)
        txtNoofDependants.Text = FormatNumber(txtNoofDependants.Text, 0)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Select()
        Close()
    End Sub

    Dim Itemexist As String
    Sub InvalidEmployee()
        Try
            cn.Open()
            Dim Query As String
            Query = "SELECT emp_code FROM allowances WHERE emp_code = '" & Trim(Emp_Code.Text) & "'"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                Itemexist = "Y"
            Else
                Itemexist = "N"
            End If
            cn.Close()

            If Itemexist = "Y" Then
                cn.Open()
                With cmd
                    .Connection = cn
                    '.CommandText = "UPDATE allowances SET amount = '" & CDec(txtAllowance.Text) & "' WHERE emp_code = '" & Emp_Code.Text & "'"
                    .ExecuteNonQuery()
                End With
                cn.Close()
            ElseIf Itemexist = "N" Then
                cn.Open()
                With cmd
                    .Connection = cn
                    '.CommandText = "INSERT INTO allowances (emp_code,amount,description) VALUES('" & Emp_Code.Text & "','" & CDec(txtAllowance.Text) & "','Allowance')"
                    .ExecuteNonQuery()
                End With
                cn.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label1.Select()

        Try
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "UPDATE employee SET pension='" & CDec(txtPensionCont.Text) & "',riskbenefits='" & CDec(txtRiskPer.Text) & "',retirementfund='" & CDec(txtRetirementPer.Text) & "',medicalaid='" & CDec(txtMedicalAid.Text) & "',pocketexpense='" & CDec(txtOutofPocket.Text) & "',dependantnum='" & CDec(txtNoofDependants.Text) & "' WHERE code = '" & Emp_Code.Text & "'"
                .ExecuteNonQuery()
            End With
            cn.Close()

            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "UPDATE scheduletable SET OvertimeHours='" & CInt(txtOvertime.Text) & "',LeaveHours='" & CInt(txtLeave.Text) & "' WHERE emp_code = '" & Emp_Code.Text & "'"
                .ExecuteNonQuery()
            End With
            cn.Close()

            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "UPDATE otherincome SET OtherIncome='" & CDec(txtOtherMonhtlyIncome.Text) & "',DeductOtherIncomce='" & CDec(txtIncomeDeduction.Text) & "',CapitalGain='" & CDec(txtTaxbleCapitalGain.Text) & "',Loans='" & CDec(txtLoans.Text) & "' WHERE emp_code = '" & Emp_Code.Text & "'"
                .ExecuteNonQuery()
            End With
            cn.Close()
            InvalidEmployee()

            MessageBox.Show("Employee payroll information successfully updated.", "Employee", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Close()
            Employees()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub txtOvertime_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtLeave_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtOvertime_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub txtLeave_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub PayrollEdit_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            cn.Open()
            With cmd
                .Connection = cn
                '.CommandText = "INSERT INTO deductions(emp_code,description,amount) VALUES('" & Emp_Code.Text & "','" & txtItemName.Text & "','" & CDec(txtAmount.Text) & "')"
                .ExecuteNonQuery()
            End With
            cn.Close()

            'DataGridView1.Rows.Add(txtItemName.Text, FormatCurrency(txtAmount.Text, 2))
        End If
    End Sub

    Private Sub txtup1_Click(sender As Object, e As EventArgs) Handles txtup1.Click
        Panel4.Visible = False
        Label2.Location = New Point(134, 258)
        txtdown2.Location = New Point(572, 258)
        txtdown1.Visible = True
        txtup1.Visible = False
    End Sub

    Private Sub txtdown1_Click(sender As Object, e As EventArgs) Handles txtdown1.Click
        Panel4.Visible = True
        Label2.Location = New Point(134, 350)
        txtdown2.Location = New Point(572, 350)
        txtup1.Visible = True
        txtdown1.Visible = False
    End Sub

    Private Sub txtup2_Click(sender As Object, e As EventArgs) Handles txtup2.Click
        Panel5.Visible = False
        Panel4.Visible = False
        Label2.Location = New Point(134, 258)
        txtdown2.Location = New Point(572, 258)
    End Sub

    Private Sub txtdown2_Click(sender As Object, e As EventArgs) Handles txtdown2.Click
        Panel5.Visible = True
        Panel5.Location = New Point(134, 286)

    End Sub

    Private Sub txtOtherMonhtlyIncome_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtOtherMonhtlyIncome.KeyPress
        If (Not e.KeyChar = ChrW(Keys.Back) And ("0123456789.").IndexOf(e.KeyChar) = -1) Or (e.KeyChar = "." And txtOtherMonhtlyIncome.Text.ToCharArray().Count(Function(c) c = ".") > 0) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtIncomeDeduction_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtIncomeDeduction.KeyPress
        If (Not e.KeyChar = ChrW(Keys.Back) And ("0123456789.").IndexOf(e.KeyChar) = -1) Or (e.KeyChar = "." And txtIncomeDeduction.Text.ToCharArray().Count(Function(c) c = ".") > 0) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtTaxbleCapitalGain_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTaxbleCapitalGain.KeyPress
        If (Not e.KeyChar = ChrW(Keys.Back) And ("0123456789.").IndexOf(e.KeyChar) = -1) Or (e.KeyChar = "." And txtTaxbleCapitalGain.Text.ToCharArray().Count(Function(c) c = ".") > 0) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtLoans_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLoans.KeyPress
        If (Not e.KeyChar = ChrW(Keys.Back) And ("0123456789.").IndexOf(e.KeyChar) = -1) Or (e.KeyChar = "." And txtLoans.Text.ToCharArray().Count(Function(c) c = ".") > 0) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtPensionCont_KeyPress(sender As Object, e As KeyPressEventArgs)
        If (Not e.KeyChar = ChrW(Keys.Back) And ("0123456789.").IndexOf(e.KeyChar) = -1) Or (e.KeyChar = "." And txtPensionCont.Text.ToCharArray().Count(Function(c) c = ".") > 0) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtRiskPer_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRiskPer.KeyPress
        If (Not e.KeyChar = ChrW(Keys.Back) And ("0123456789.").IndexOf(e.KeyChar) = -1) Or (e.KeyChar = "." And txtRiskPer.Text.ToCharArray().Count(Function(c) c = ".") > 0) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtRetirementPer_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRetirementPer.KeyPress
        If (Not e.KeyChar = ChrW(Keys.Back) And ("0123456789.").IndexOf(e.KeyChar) = -1) Or (e.KeyChar = "." And txtRetirementPer.Text.ToCharArray().Count(Function(c) c = ".") > 0) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtMedicalAid_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMedicalAid.KeyPress
        If (Not e.KeyChar = ChrW(Keys.Back) And ("0123456789.").IndexOf(e.KeyChar) = -1) Or (e.KeyChar = "." And txtMedicalAid.Text.ToCharArray().Count(Function(c) c = ".") > 0) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtOutofPocket_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtOutofPocket.KeyPress
        If (Not e.KeyChar = ChrW(Keys.Back) And ("0123456789.").IndexOf(e.KeyChar) = -1) Or (e.KeyChar = "." And txtOutofPocket.Text.ToCharArray().Count(Function(c) c = ".") > 0) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtNoofDependants_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNoofDependants.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub txtMonthlySalary_KeyPress(sender As Object, e As KeyPressEventArgs)
        If (Not e.KeyChar = ChrW(Keys.Back) And ("0123456789.").IndexOf(e.KeyChar) = -1) Or (e.KeyChar = "." And txtMonthlySalary.Text.ToCharArray().Count(Function(c) c = ".") > 0) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtOtherMonhtlyIncome_Leave(sender As Object, e As EventArgs) Handles txtOtherMonhtlyIncome.Leave
        txtOtherMonhtlyIncome.Text = FormatCurrency(txtOtherMonhtlyIncome.Text, 2)
        calculations()
    End Sub

    Private Sub txtIncomeDeduction_Leave(sender As Object, e As EventArgs) Handles txtIncomeDeduction.Leave
        txtIncomeDeduction.Text = FormatCurrency(txtIncomeDeduction.Text, 2)
        calculations()
    End Sub

    Private Sub txtTaxbleCapitalGain_Leave(sender As Object, e As EventArgs) Handles txtTaxbleCapitalGain.Leave
        txtTaxbleCapitalGain.Text = FormatCurrency(txtTaxbleCapitalGain.Text, 2)
        calculations()
    End Sub

    Private Sub txtLoans_Leave(sender As Object, e As EventArgs) Handles txtLoans.Leave
        txtLoans.Text = FormatCurrency(txtLoans.Text, 2)
        calculations()
    End Sub

    Private Sub txtPensionCont_Leave(sender As Object, e As EventArgs)
        txtPensionCont.Text = FormatCurrency(txtPensionCont.Text, 2)
        calculations()
    End Sub

    Private Sub txtRiskPer_Leave(sender As Object, e As EventArgs) Handles txtRiskPer.Leave
        txtRiskPer.Text = FormatNumber(txtRiskPer.Text, 2)
        calculations()
    End Sub

    Private Sub txtRetirementPer_Leave(sender As Object, e As EventArgs) Handles txtRetirementPer.Leave
        txtRetirementPer.Text = FormatNumber(txtRetirementPer.Text, 2)
        calculations()
    End Sub

    Private Sub txtMedicalAid_Leave(sender As Object, e As EventArgs) Handles txtMedicalAid.Leave
        txtMedicalAid.Text = FormatCurrency(txtMedicalAid.Text, 2)
        calculations()
    End Sub

    Private Sub txtOutofPocket_Leave(sender As Object, e As EventArgs) Handles txtOutofPocket.Leave
        txtOutofPocket.Text = FormatCurrency(txtOutofPocket.Text, 2)
    End Sub

    Private Sub txtNoofDependants_Leave(sender As Object, e As EventArgs) Handles txtNoofDependants.Leave
        txtNoofDependants.Text = FormatNumber(txtNoofDependants.Text, 0)
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

    Dim phy_country As String
    Dim tax1 As String
    Dim tax2 As String
    Dim tax3 As String
    Dim tax4 As String
    Dim fixed_salary As String
    Dim tax As String
    Dim ssfr As String
    Dim netpay As String
    Dim uif1 As String
    Sub calculations()
        Try
            taxValues()
            cn.Open()
            Dim Query2 As String
            Query2 = "SELECT phy_country FROM company WHERE ID = 1"
            cmd = New MySqlCommand(Query2, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                phy_country = dr("phy_country").ToString()
            End While
            cn.Close()

            If phy_country = "Rwanda" Then
                cn.Open()
                Dim qry3 As String = "SELECT * FROM parameters"
                cmd = New MySqlCommand(qry3, cn)
                dr = cmd.ExecuteReader
                While dr.Read
                    napsaper = dr("napsaper").ToString()
                    uif = dr("uif").ToString()
                    'overTimeHour = dr("overtime_per_hour").ToString()
                End While
                cn.Close()

                cn.Open()
                Dim qry4 As String = "SELECT * FROM employee WHERE code = '" & Emp_Code.Text & "'"
                cmd = New MySqlCommand(qry4, cn)
                dr = cmd.ExecuteReader
                While dr.Read
                    fixed_salary = dr("fixed_salary").ToString()
                    txtMonthlySalary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                End While
                cn.Close()
                'MsgBox(fixed_salary)
                If CDec(fixed_salary) <= CDec(taxGross1) Then
                    tax = (CDec(taxValue1) / 100) * CDec(fixed_salary)
                ElseIf CDec(fixed_salary) > CDec(taxGross1) And CDec(fixed_salary) <= CDec(taxGross2) Then
                    tax = (CDec(taxValue2) / 100) * (CDec(fixed_salary) - CDec(taxGross1))
                ElseIf CDec(fixed_salary) > CDec(taxGross2) Then
                    tax = (0.2 * 70000) + ((CDec(taxValue3) / 100) * (CDec(fixed_salary) - CDec(taxGross2)))
                End If

                ssfr = (CDec(napsaper) / 100) * CDec(fixed_salary)
                netpay = CDec(fixed_salary) - (CDec(tax) + CDec(ssfr))
                uif1 = (CDec(uif) / 100) * CDec(fixed_salary)
                Label27.Text = "SSFR"

                txtTotalIncome.Text = FormatCurrency(CDec(txtMonthlySalary.Text) + CDec(txtOtherMonhtlyIncome.Text) + CDec(txtIncomeDeduction.Text) + CDec(txtTaxbleCapitalGain.Text) - CDec(txtLoans.Text), 2)
                txtTotalRetirement.Text = FormatCurrency(CDec(txtPensionCont.Text) + ((CDec(txtRiskPer.Text) / 100) * CDec(txtMonthlySalary.Text)) + ((CDec(txtRetirementPer.Text) / 100) * CDec(txtMonthlySalary.Text)), 2)
                txtMedical.Text = FormatCurrency(CDec(txtMedicalAid.Text) + CDec(txtOutofPocket.Text), 2)
                txtnapsa.Text = FormatCurrency(ssfr, 2)
                txtUIF.Text = FormatCurrency(uifAmount, 2)
                txtOvertimeLeave.Text = FormatCurrency((CDec(txtOvertime.Text) * CDec(overTimeHour)) - (CDec(txtLeave.Text) * CDec(overTimeHour)), 2)
                txtNetSalary.Text = FormatCurrency(CDec(netpay) - CDec(txtMedical.Text) - CDec(txtTotalRetirement.Text) - CDec(txtUIF.Text) + CDec(txtOtherMonhtlyIncome.Text) + CDec(txtOvertimeLeave.Text) - CDec(txtLoans.Text) - CDec(txtIncomeDeduction.Text), 2)

                txtTotalTax.Text = FormatCurrency(tax, 2)
            ElseIf phy_country = "Zambia" Then
                Label27.Text = "NAPSA"

                txtTotalIncome.Text = FormatCurrency(CDec(txtMonthlySalary.Text) + CDec(txtOtherMonhtlyIncome.Text) + CDec(txtIncomeDeduction.Text) + CDec(txtTaxbleCapitalGain.Text) - CDec(txtLoans.Text), 2)
                txtTotalRetirement.Text = FormatCurrency(CDec(txtPensionCont.Text) + ((CDec(txtRiskPer.Text) / 100) * CDec(txtMonthlySalary.Text)) + ((CDec(txtRetirementPer.Text) / 100) * CDec(txtMonthlySalary.Text)), 2)
                txtMedical.Text = FormatCurrency(CDec(txtMedicalAid.Text) + CDec(txtOutofPocket.Text), 2)
                txtnapsa.Text = FormatCurrency(mynapsa, 2)
                txtUIF.Text = FormatCurrency(uifAmount, 2)
                txtOvertimeLeave.Text = FormatCurrency((CDec(txtOvertime.Text) * CDec(overTimeHour)) - (CDec(txtLeave.Text) * CDec(overTimeHour)), 2)
                txtNetSalary.Text = FormatCurrency(CDec(txtTotalIncome.Text) - CDec(txtTotalRetirement.Text) - CDec(txtMedical.Text) - CDec(txtnapsa.Text) + CDec(txtOvertimeLeave.Text) - CDec(txtUIF.Text), 2)

                tax1 = CDec(3300.0 * 0)
                tax2 = CDec(25 / 100) * CDec(4100.0 - 3300.0)
                tax3 = CDec(30 / 100) * CDec(6200.0 - 4100.0)
                tax4 = CDec(37.5 / 100) * ((CDec(txtTotalIncome.Text) - CDec(mynapsa)) - (3300.0 + (4100.0 - 3300.0) + (6200.0 - 4100.0)))
                'tax4 = (200 + 630)
                Dim totaltax = CDec(tax1) + CDec(tax2) + CDec(tax3) + CDec(tax4)

                txtTotalTax.Text = FormatCurrency(totaltax, 2)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub txtOvertime_Leave(sender As Object, e As EventArgs) Handles txtOvertime.Leave
        txtOvertime.Text = FormatNumber(txtOvertime.Text, 0)
        calculations()
    End Sub

    Private Sub txtLeave_Leave(sender As Object, e As EventArgs) Handles txtLeave.Leave
        txtLeave.Text = FormatNumber(txtLeave.Text, 0)
        calculations()
    End Sub

    Private Sub txtPensionCont_Leave_1(sender As Object, e As EventArgs) Handles txtPensionCont.Leave
        txtPensionCont.Text = FormatCurrency(txtPensionCont.Text, 2)
        calculations()
    End Sub

    Private Sub txtnapsa_Click(sender As Object, e As EventArgs) Handles txtnapsa.Click

    End Sub
End Class