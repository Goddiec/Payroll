Imports System.IO
Imports MySql.Data.MySqlClient
Public Class RunPayroll
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Panel1.Select()
        Close()
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

    Dim tax1 As String
    Dim tax2 As String
    Dim tax3 As String
    Dim tax4 As String
    Dim uif As String
    Dim napsa As String
    Dim napsaper As String
    Dim totalnapsa As String
    Dim mynapsa As String
    Dim uifAmount As String
    Sub SaveDataZambia()
        Try
            taxValues()

            For Each row As DataGridViewRow In PayrollSchedule.DataGridView1.Rows
                cn.Open()
                Dim qry4 As String = "SELECT * FROM parameters"
                cmd = New MySqlCommand(qry4, cn)
                dr = cmd.ExecuteReader
                While dr.Read
                    napsa = dr("napsa").ToString()
                    napsaper = dr("napsaper").ToString()
                    uif = dr("uif").ToString()
                    'overTimeHour = dr("overtime_per_hour").ToString()
                End While
                cn.Close()

                totalnapsa = (CDec(napsaper) / 100) * CDec(row.Cells(2).FormattedValue)

                If CDec(totalnapsa) > CDec(napsa) Then
                    mynapsa = napsa
                Else
                    mynapsa = totalnapsa
                End If

                uifAmount = (CDec(uif) / 100) * CDec(row.Cells(2).FormattedValue)
            Next

            For i As Integer = 0 To PayrollSchedule.DataGridView1.Rows.Count - 1
                cn.Open()
                With cmd
                    .Connection = cn
                    .CommandText = "INSERT INTO transactions(emp_code,datepaid,totaltax,uif,subsalary,otherincome,otherdedicincome,capitalgain,loans,naspa,pension,riskbenefitper,riskbenefitamnt,retirementper,retirementamnt,medicalaid,pocketexpen,period,netsalary)
                                    SELECT dt.emp_code,'" & Today.Date.ToString("yyyy-MM-dd") & "',('" & CDec(taxValue1) / 100 * CDec(taxGross1) & "') + (('" & CDec(taxValue2) / 100 & "') * ('" & CDec(taxGross2) - CDec(taxGross1) & "')) + (('" & CDec(taxValue3) / 100 & "') * ('" & CDec(taxGross3) - CDec(taxGross2) & "')) + (('" & CDec(taxValue4) / 100 & "') * (((e.fixed_salary+i.OtherIncome+i.DeductOtherIncomce+i.CapitalGain-i.Loans) - ('" & CDec(mynapsa) & "')) - ('" & CDec(taxGross1) & "' + ('" & CDec(taxGross2) - CDec(taxGross1) & "') + ('" & CDec(taxGross3) - CDec(taxGross2) & "')))),'" & (CDec(uif) / 100) & "' * e.fixed_salary ,e.fixed_salary,i.OtherIncome,i.DeductOtherIncomce,i.CapitalGain,i.Loans,'" & CDec(mynapsa) & "',dt.pension,e.riskbenefits, (e.riskbenefits/100*e.fixed_salary), e.retirementfund ,(e.retirementfund/100*e.fixed_salary),e.medicalaid,e.pocketexpense,'" & Today.Month & "',((e.fixed_salary+i.OtherIncome+i.DeductOtherIncomce+i.CapitalGain-i.Loans) - ('" & (CDec(taxValue1) / 100) * CDec(taxGross1) & "') + (('" & CDec(taxValue2) / 100 & "') * ('" & CDec(taxGross2) - CDec(taxGross1) & "')) + (('" & CDec(taxValue3) / 100 & "') * ('" & CDec(taxGross3) - CDec(taxGross2) & "')) + (('" & CDec(taxValue4) / 100 & "') * (((e.fixed_salary+i.OtherIncome+i.DeductOtherIncomce+i.CapitalGain-i.Loans) - ('" & CDec(mynapsa) & "')) - ('" & CDec(taxGross1) & "' + ('" & CDec(taxGross2) - CDec(taxGross1) & "') + ('" & CDec(taxGross3) - CDec(taxGross2) & "')))))
                                    FROM employee e
                                    JOIN deductiontransaction dt
                                    ON e.code = dt.emp_code
                                    JOIN otherincome i
                                    ON e.code = i.emp_code
                                    WHERE e.code = '" & PayrollSchedule.DataGridView1.Rows(i).Cells(0).Value & "'
                                    "
                    .ExecuteNonQuery()
                End With
                cn.Close()
            Next
            PrintEmail.ShowDialog()

            Close()
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            workerparyroll.WindowState = FormWindowState.Maximized
            workerparyroll.Show()
            workerparyroll.MdiParent = MainInterface
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try

    End Sub

    Dim ssfr As String
    Dim tax As String
    Dim fixed_salary As String
    Dim netpay As String
    Dim uif1 As String
    Sub SaveDataRwanda()
        Try
            taxValues()

            For i As Integer = 0 To PayrollSchedule.DataGridView1.Rows.Count - 1
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
                Dim qry4 As String = "SELECT * FROM employee WHERE code = '" & PayrollSchedule.DataGridView1.Rows(i).Cells(0).Value & "'"
                cmd = New MySqlCommand(qry4, cn)
                dr = cmd.ExecuteReader
                While dr.Read
                    fixed_salary = dr("fixed_salary").ToString()
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

                cn.Open()
                With cmd
                    .Connection = cn
                    .CommandText = "INSERT INTO transactions(emp_code,datepaid,totaltax,uif,subsalary,otherincome,otherdedicincome,capitalgain,loans,naspa,pension,riskbenefitper,riskbenefitamnt,retirementamnt,medicalaid,pocketexpen,period,netsalary)
                                        SELECT dt.emp_code,'" & Today.Date.ToString("yyyy-MM-dd") & "','" & CDec(tax) & "','" & (CDec(uif) / 100) & "' * e.fixed_salary ,e.fixed_salary,i.OtherIncome,i.DeductOtherIncomce,i.CapitalGain,i.Loans,'" & CDec(ssfr) & "',e.pension,e.riskbenefits, (e.riskbenefits/100*e.fixed_salary), e.retirementfund ,e.medicalaid,e.pocketexpense,'" & Today.Month & "','" & netpay & "' + (i.OtherIncome + i.DeductOtherIncomce + i.CapitalGain + i.Loans)
                                        FROM employee e
                                        JOIN deductiontransaction dt
                                        ON e.code = dt.emp_code
                                        JOIN otherincome i
                                        ON e.code = i.emp_code
                                        WHERE e.code = '" & PayrollSchedule.DataGridView1.Rows(i).Cells(0).Value & "'"
                    .ExecuteNonQuery()
                End With
                cn.Close()
            Next

            PrintEmail.ShowDialog()

            Close()
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            workerparyroll.WindowState = FormWindowState.Maximized
            workerparyroll.Show()
            workerparyroll.MdiParent = MainInterface
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim pathData As String
    Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
    Sub backup()
        Try
            Using sr As New System.IO.StreamReader(appPath + "\path.ini")
                Dim Line1 As String = sr.ReadLine
                Dim Line2 As String = sr.ReadLine

                pathData = Line2
            End Using

            Dim file As String
            SaveFileDialog1.Filter = "SQL Dump File (*.sql)|*.sql|All files (*.*)|*.*"
            SaveFileDialog1.FileName = "Database Backup " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".sql"
            If SaveFileDialog1.ShowDialog = DialogResult.OK Then
                file = SaveFileDialog1.FileName
                Dim myProcess As New Process()
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                myProcess.StartInfo.FileName = "cmd.exe"
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                myProcess.StartInfo.UseShellExecute = False
                myProcess.StartInfo.WorkingDirectory = pathData '"C:\Program Files\MySQL\MySQL Server 5.7\bin\"
                myProcess.StartInfo.RedirectStandardInput = True
                myProcess.StartInfo.RedirectStandardOutput = True
                myProcess.Start()
                Dim myStreamWriter As StreamWriter = myProcess.StandardInput
                Dim mystreamreader As StreamReader = myProcess.StandardOutput '"server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";"
                myStreamWriter.WriteLine("mysqldump -u " + DatabaseSetting.txt_uid.Text + " --password=" + DatabaseSetting.txt_pwd.Text + " -h " + DatabaseSetting.txt_server.Text + " """ + DatabaseSetting.txt_database.Text + """ > """ + file + """ ")
                myStreamWriter.Close()
                myProcess.WaitForExit()
                myProcess.Close()
                MsgBox("Backup Created Successfully", MsgBoxStyle.Information, "Backup")
                SaveDataZambia()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Backup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Dim phy_country As String
    Sub RunPayData()
        Try
            cn.Open()
            Dim qry4 As String = "SELECT phy_country FROM company"
            cmd = New MySqlCommand(qry4, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                phy_country = dr("phy_country").ToString()
            End While
            cn.Close()

            If CheckBox1.Checked = True Then
                backup()
            Else
                If phy_country = "Zambia" Then
                    SaveDataZambia()
                ElseIf phy_country = "Rwanda" Then
                    SaveDataRwanda()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Panel1.Select()
        RunPayData()
    End Sub

    Private Sub RunPayroll_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel1.Select()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub
End Class