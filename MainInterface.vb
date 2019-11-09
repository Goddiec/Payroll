Imports System.IO
Imports System.Runtime.InteropServices

Public Class MainInterface

    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private m_ChildFormNumber As Integer
    Private Sub MainInterface_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Button14.Enabled = True Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            Dim dialog As New DialogResult
            dialog = MsgBox("NOTE!! It is essential to backup regulary. Please consult your manual for details. Do you want to backup company now?", MsgBoxStyle.Question + MsgBoxStyle.YesNoCancel, "System Exit")

            If dialog = DialogResult.Cancel Then
                e.Cancel = True
                For Each ChildForm As Form In Me.MdiChildren
                    ChildForm.Close()
                Next

                StartWindow.WindowState = FormWindowState.Maximized
                StartWindow.Show()
                StartWindow.MdiParent = Me
            ElseIf dialog = DialogResult.No Then
                Application.ExitThread()
            Else
                'Backup Here
                For Each ChildForm As Form In Me.MdiChildren
                    ChildForm.Close()
                Next

                BackupExit.ShowDialog()
            End If
        End If
    End Sub

    Public reg As Char
    Public Sub CheckForExistingInstance()
        Try
            If System.DateTime.Today.Date < My.Settings.AccessDate.Date Then
                MsgBox("The system was last accessed on " + My.Settings.AccessDate.Date + ". Your system date [" + System.DateTime.Today.Date + "] is before the last accessed date. Please correct your system date.", MsgBoxStyle.Critical, "Registration Assistance")
                Application.ExitThread()
            Else
                My.Settings.AccessDate = DateTime.Now
                My.Settings.Save()

                If System.DateTime.Today.Date >= My.Settings.ExpiryDate.Date.ToString("yyyy/MM/dd") Then
                    reg = "N"
                    MsgBox("Your system is expired or not correctly registered!", MsgBoxStyle.Exclamation, "Registration Assistance")
                    Me.Hide()
                    RegistrationWindow.ShowDialog()
                Else
                    Me.KeyPreview = True
                    startup.Select()
                    startup.Label3.Text = My.Application.Info.Description
                    startup.Label4.Text = My.Application.Info.Copyright '+ " Point Of Sale Technologies, Inc. All Rights Reserved."
                    startup.Label9.Text = "Version " & My.Application.Info.Version.ToString & " (" & My.Application.Info.ProductName & ")"
                    startup.ShowDialog()
                End If
                'End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Application.ExitThread()
        End Try
    End Sub

    Public Sub CheckForExistingInstance11()
        Try
            If System.DateTime.Today.Date < My.Settings.AccessDate.Date Then
                MsgBox("The system was last accessed on " + My.Settings.AccessDate.Date + ". Your system date [" + System.DateTime.Today.Date + "] is before the last accessed date. Please correct your system date.", MsgBoxStyle.Critical, "Registration Assistance")
                Application.ExitThread()
            Else
                My.Settings.AccessDate = DateTime.Now
                My.Settings.Save()

                '~~> Get number of processes of you program
                'If Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName).Length > 1 Then
                'Hide()
                'MessageBox.Show("Financial System is already running", "system", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'Application.Exit()
                'Else
                If System.DateTime.Today.Date >= My.Settings.ExpiryDate.Date.ToString("yyyy/MM/dd") Then
                    reg = "N"
                    MsgBox("Your system is expired or not correctly registered!", MsgBoxStyle.Exclamation, "Registration Assistance")
                    RegistrationWindow.ShowDialog()
                ElseIf System.DateTime.Today.Date < My.Settings.AccessDate.Date.ToString("yyyy/MM/dd") Then
                    MessageBox.Show("Your system was last accessed on [" & My.Settings.AccessDate.Date.ToString("yyyy/MM/dd") & "] and the system date is [" & System.DateTime.Today.Date & "]. Please check your system date!", "Registration Assistance", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Application.ExitThread()
                Else
                    Me.KeyPreview = True
                    startup.Select()
                    startup.Label3.Text = My.Application.Info.Description
                    startup.Label4.Text = My.Application.Info.Copyright '+ " Point Of Sale Technologies, Inc. All Rights Reserved."
                    startup.Label9.Text = "Version " & My.Application.Info.Version.ToString & " (" & My.Application.Info.ProductName & ")"
                    startup.ShowDialog()
                End If
                'End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Application.ExitThread()
        End Try
    End Sub

    Sub DisableIcons()
        EditMenu.Enabled = False
        Button14.Enabled = False
        Button4.Enabled = False
        Button15.Enabled = False
        Button17.Enabled = False
        Button25.Enabled = False
        Button24.Enabled = False
        Button23.Enabled = False
        Button22.Enabled = False
        Button5.Enabled = False
        Button21.Enabled = False
        EditMenu.Enabled = False
        EmployeesToolStripMenuItem.Enabled = False
        SetupToolStripMenuItem.Enabled = False
        ViewMenu.Enabled = False
        BackupToolStripMenuItem.Visible = False
        RestoreToolStripMenuItem.Enabled = False
        ScanDocumentsToolStripMenuItem.Enabled = False
    End Sub

    Private Sub MainInterface_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ToolStrip.Select()
        'ShowLogin()
        DisableIcons()
        CheckForExistingInstance()
        openTImer = "Y"
    End Sub

    Dim plainText As String
    Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
    Sub ShowLogin()
        Try
            If System.IO.File.Exists(appPath + "\Settings.ini") Then
                Dim data As New Database
                With data
                    'Get the value from C:\Deluxe16\BOMI.ini
                    Using sr As New System.IO.StreamReader(appPath + "\Settings.ini")
                        Dim Line As String = sr.ReadLine
                        Dim Line1 As String = sr.ReadLine
                        Dim Line2 As String = sr.ReadLine
                        Dim Line3 As String = sr.ReadLine
                        Dim Line4 As String = sr.ReadLine
                        Dim Line5 As String = sr.ReadLine
                        Dim Line6 As String = sr.ReadLine
                        Dim Line7 As String = sr.ReadLine
                        Dim Line8 As String = sr.ReadLine

                        Dim wrapper As New Simple3Des("12345")
                        plainText = wrapper.DecryptData(Line2.Substring(11))

                        DatabaseSetting.txt_server.Text = Line.Substring(11)
                        DatabaseSetting.txt_uid.Text = Line1.Substring(11)
                        DatabaseSetting.txt_pwd.Text = plainText
                        DatabaseSetting.txt_database.Text = Line3.Substring(11)
                        DatabaseSetting.txt_port.Text = Line4.Substring(11)
                    End Using
                    'Assing the object property values
                    .ServerName = DatabaseSetting.txt_server.Text
                    .DatabaseName = DatabaseSetting.txt_database.Text
                    .UserID = DatabaseSetting.txt_uid.Text
                    .Password = DatabaseSetting.txt_pwd.Text
                    .Port = DatabaseSetting.txt_port.Text

                    'Connection testing
                    If .Connection Then
                        'For Each ChildForm As Form In Me.MdiChildren
                        '    ChildForm.Close()
                        'Next

                        'Employee.WindowState = FormWindowState.Maximized
                        Login.ShowDialog()
                        'Employee.MdiParent = Me
                    Else
                        'Unable to connect
                        DatabaseSetting.ShowDialog()
                    End If
                End With
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            DatabaseSetting.ShowDialog()
        End Try
    End Sub

    Private Sub NotepadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NotepadToolStripMenuItem.Click
        System.Diagnostics.Process.Start("Notepad")
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OptionsToolStripMenuItem.Click
        System.Diagnostics.Process.Start("calc")
    End Sub

    Private Sub EmployeeToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'For Each form In SplitContainer1.Panel2.Controls.OfType(Of Form).ToList()
        '    form.Close()
        'Next

        'Dim xChildWindows = Application.OpenForms.OfType(Of Employee)
        'If xChildWindows.Any Then
        '    xChildWindows.First().Focus() 'Focus if exists
        'Else
        '    Dim xfrmNew As New Employee() 'Open window if doeasn't exists

        '    xfrmNew.TopLevel = False
        '    SplitContainer1.Panel2.Controls.Add(xfrmNew)
        '    xfrmNew.Show()
        'End If
    End Sub

    Private Sub NewCompanyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewCompanyToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        newcompany.ShowDialog()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        openTImer = "N"
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        OpenCompany.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        'Panel1.Select()

        'For Each form In SplitContainer1.Panel2.Controls.OfType(Of Form).ToList()
        '    form.Close()
        'Next

        'users.ShowDialog()

        'Dim xChildWindows = Application.OpenForms.OfType(Of paycalendar)
        'If xChildWindows.Any Then
        '    xChildWindows.First().Focus() 'Focus if exists
        'Else
        '    Dim xfrmNew As New paycalendar() 'Open window if doeasn't exists

        '    xfrmNew.TopLevel = False
        '    Panel3.Controls.Add(xfrmNew)
        '    xfrmNew.Show()
        'End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        about.ShowDialog()
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingsToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
        'setupinterview.ShowDialog()
        DatabaseSetting.ShowDialog()
    End Sub

    Private Sub DepartmentsToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'For Each form In SplitContainer1.Panel2.Controls.OfType(Of Form).ToList()
        '    form.Close()
        'Next

        'departments.ShowDialog()
    End Sub

    Private Sub BanksToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'For Each form In SplitContainer1.Panel2.Controls.OfType(Of Form).ToList()
        '    form.Close()
        'Next

        'banks.ShowDialog()
    End Sub

    Private Sub CompanyToolStripMenuItem2_Click(sender As Object, e As EventArgs)
        company.ShowDialog()
    End Sub

    Private Sub LeaveToolStripMenuItem1_Click(sender As Object, e As EventArgs)
        'For Each form In SplitContainer1.Panel2.Controls.OfType(Of Form).ToList()
        '    form.Close()
        'Next

        'LeaveSetup.ShowDialog()
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs)
        'Panel1.Select()

        'SplitContainer1.Visible = False
        'NewEmployee.WindowState = FormWindowState.Maximized
        'NewEmployee.Show()
        'NewEmployee.MdiParent = Me
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        'Panel1.Select()

        'For Each form In SplitContainer1.Panel2.Controls.OfType(Of Form).ToList()
        '    form.Close()
        'Next

        'departments.ShowDialog()
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs)
        'Panel1.Select()

        'For Each form In SplitContainer1.Panel2.Controls.OfType(Of Form).ToList()
        '    form.Close()
        'Next

        'banks.ShowDialog()
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs)
        'Panel1.Select()

        'For Each form In SplitContainer1.Panel2.Controls.OfType(Of Form).ToList()
        '    form.Close()
        'Next

        'LeaveSetup.ShowDialog()
    End Sub

    Private Sub BackupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackupToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        backup.ShowDialog()
    End Sub

    Private Sub UsersToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'Panel1.Select()

        'For Each form In SplitContainer1.Panel2.Controls.OfType(Of Form).ToList()
        '    form.Close()
        'Next

        'users.ShowDialog()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        'Panel1.Select()

        'For Each form In SplitContainer1.Panel2.Controls.OfType(Of Form).ToList()
        '    form.Close()
        'Next

        'Login.ShowDialog()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        'Panel1.Select()

        'SplitContainer1.Visible = False
        'workerpayroll.WindowState = FormWindowState.Maximized
        'workerpayroll.Show()
        'workerpayroll.MdiParent = Me
        'workerpayroll.ComboBox1.SelectedIndex = 2
        'workerpayroll.txtCustomer.Select()
        'For Each form In SplitContainer1.Panel2.Controls.OfType(Of Form).ToList()
        '    form.Close()
        'Next

        'Dim xChildWindows = Application.OpenForms.OfType(Of workerpayroll)
        'If xChildWindows.Any Then
        '    xChildWindows.First().Focus() 'Focus if exists
        'Else
        '    Dim xfrmNew As New workerpayroll() 'Open window if doeasn't exists

        '    xfrmNew.TopLevel = False
        '    SplitContainer1.Panel2.Controls.Add(xfrmNew)
        '    xfrmNew.Show()
        'End If
    End Sub

    Public employeecode As String
    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        'Dim row As DataGridViewRow = DataGridView1.CurrentRow
        'employeecode = row.Cells(0).Value.ToString()
        'workerpayroll.txtCode.Text = "123"

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub EmployeesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmployeesToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        NewEmployee.WindowState = FormWindowState.Maximized
        NewEmployee.Show()
        NewEmployee.MdiParent = Me

        Button14.ForeColor = Color.Black
        Button4.ForeColor = Color.Black
        Button15.ForeColor = Color.Black
        Button17.ForeColor = Color.Black
        Button25.ForeColor = Color.Black
        Button24.ForeColor = Color.Red
        Button23.ForeColor = Color.Black
        Button22.ForeColor = Color.Black
        Button5.ForeColor = Color.Black
        Button21.ForeColor = Color.Black
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs)


    End Sub

    Private Sub SetupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetupToolStripMenuItem.Click

    End Sub

    Private Sub DatabaseSettingsToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)


    End Sub

    Private Sub ConfigureEmailToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs)
        ToolStrip.Select()
        If Login.database_setup = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            DatabaseSetting.ShowDialog()
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub EditMenu_Click(sender As Object, e As EventArgs) Handles EditMenu.Click
        ToolStrip.Select()
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        workerparyroll.WindowState = FormWindowState.Maximized
        workerparyroll.Show()
        workerparyroll.MdiParent = Me

        Button14.ForeColor = Color.Red
        Button4.ForeColor = Color.Black
        Button15.ForeColor = Color.Black
        Button17.ForeColor = Color.Black
        Button25.ForeColor = Color.Black
        Button24.ForeColor = Color.Black
        Button23.ForeColor = Color.Black
        Button22.ForeColor = Color.Black
        Button5.ForeColor = Color.Black
        Button21.ForeColor = Color.Black
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub RegistrationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegistrationToolStripMenuItem.Click
        ToolStrip.Select()

        Dim dialog As New DialogResult
        dialog = MsgBox("Payroll system will logout for registration and the shutdown after registration.", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "System Registration")

        If dialog = DialogResult.No Then

        Else
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            RegistrationWindow.ShowDialog()
        End If
    End Sub

    Private Sub CompanyParametersToolStripMenuItem_Click(sender As Object, e As EventArgs)

        company.MdiParent = Me
    End Sub

    Private Sub UserSetupToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub RestoreToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestoreToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        backupRestore.ShowDialog()
    End Sub

    Private Sub EmployeeListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmployeeListToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        EmployeeListSelect.ShowDialog()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs)


    End Sub

    Private Sub EmployeeDetailsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmployeeDetailsToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        EmployeeDetailsSelect.ShowDialog()
    End Sub

    Private Sub NewEmployeeFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewEmployeeFormToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        EmployeeFormSelect.ShowDialog()
    End Sub

    Private Sub BirthdayListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BirthdayListToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        EmployeeBirthDaySelest.ShowDialog()
    End Sub

    Private Sub CompanyToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CompanyToolStripMenuItem1.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        CompanyDetailsSelect.ShowDialog()
    End Sub

    Private Sub DepartmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DepartmentToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        DepartmentSelect.ShowDialog()
    End Sub

    Private Sub DesginationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DesginationToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        DesignationSelect.ShowDialog()
    End Sub

    Private Sub LeaveTypeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LeaveTypeToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        LeaveTypeSelect.ShowDialog()
    End Sub

    Private Sub UserAccountsToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub EmployeeLeaveSummaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmployeeLeaveSummaryToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        LeaveHisSelect.ShowDialog()
    End Sub

    Private Sub CompanyParametersToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles CompanyParametersToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        company.WindowState = FormWindowState.Maximized
        company.Show()
        company.MdiParent = Me
    End Sub

    Private Sub BanksSeuptToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BanksSeuptToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        banks.WindowState = FormWindowState.Maximized
        banks.Show()
        banks.MdiParent = Me
    End Sub

    Private Sub ScanDocumentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScanDocumentsToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        scanDocuments.WindowState = FormWindowState.Maximized
        scanDocuments.Show()
        scanDocuments.MdiParent = Me
    End Sub

    Private Sub CurrentPayslipToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CurrentPayslipToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        'payslipselect.WindowState = FormWindowState.Normal
        payslipselect.ShowDialog()
        'payslipselect.MdiParent = Me
    End Sub

    Private Sub PreviousPayslipToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PreviousPayslipToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        payslipprevselect.ShowDialog()
    End Sub

    Private Sub RetirementFundingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RetirementFundingToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        retirementFundSelect.ShowDialog()
    End Sub

    Private Sub LoansToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoansToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        LoansSelect.ShowDialog()
    End Sub

    Private Sub HoursAndUnitInputReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HoursAndUnitInputReportToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        HourInputSelect.ShowDialog()
    End Sub

    Private Sub LeaveByMonthToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LeaveByMonthToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        LeaveByMonthSelect.ShowDialog()
    End Sub

    Private Sub PayrollTotalsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PayrollTotalsToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        PayrollTaxTotalSelect.ShowDialog()

    End Sub

    Private Sub EmployeeIncrementToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmployeeIncrementToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        NumEmployees.ShowDialog()

    End Sub

    Private Sub ListDepartmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListDepartmentToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        DepartmentSelect.ShowDialog()
    End Sub

    Private Sub DesginationToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DesginationToolStripMenuItem1.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        DesignationSelect.ShowDialog()
    End Sub

    Private Sub LeaveTypeListingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LeaveTypeListingToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        LeaveTypeSelect.ShowDialog()
    End Sub

    Private Sub UserAccountsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles UserAccountsToolStripMenuItem1.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        users.WindowState = FormWindowState.Normal
        users.ShowDialog()
        users.MdiParent = Me
    End Sub

    Private Sub ConfigureEmailToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ConfigureEmailToolStripMenuItem1.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        EmailSetup.WindowState = FormWindowState.Maximized
        EmailSetup.Show()
        EmailSetup.MdiParent = Me
    End Sub

    Private Sub DatabaseSetupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DatabaseSetupToolStripMenuItem.Click
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        DatabaseSetting.ShowDialog()
    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'HelpWindow.Show()
        Try
            appPath = Path.GetDirectoryName(Application.ExecutablePath)
            Process.Start(appPath + "\UserManual.chm")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub DocumentationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DocumentationToolStripMenuItem.Click
        Try
            appPath = Path.GetDirectoryName(Application.ExecutablePath)
            Process.Start(appPath + "\Documentation.chm")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs)

    End Sub

    Public openTImer As Char
    Private Sub Button26_Click(sender As Object, e As EventArgs) Handles Button26.Click
        ToolStrip.Select()
        openTImer = "N"
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        OpenCompany.ShowDialog()
    End Sub

    Private Sub Button14_Click_1(sender As Object, e As EventArgs) Handles Button14.Click
        ToolStrip.Select()

        If Login.pay_employees = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            workerparyroll.WindowState = FormWindowState.Maximized
            workerparyroll.Show()
            workerparyroll.MdiParent = Me

            Button14.ForeColor = Color.Red
            Button4.ForeColor = Color.Black
            Button15.ForeColor = Color.Black
            Button17.ForeColor = Color.Black
            Button25.ForeColor = Color.Black
            Button24.ForeColor = Color.Black
            Button23.ForeColor = Color.Black
            Button22.ForeColor = Color.Black
            Button5.ForeColor = Color.Black
            Button21.ForeColor = Color.Black
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        ToolStrip.Select()
        If Login.add_department = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            departments.WindowState = FormWindowState.Maximized
            departments.Show()
            departments.MdiParent = Me

            Button14.ForeColor = Color.Black
            Button4.ForeColor = Color.Red
            Button15.ForeColor = Color.Black
            Button17.ForeColor = Color.Black
            Button25.ForeColor = Color.Black
            Button24.ForeColor = Color.Black
            Button23.ForeColor = Color.Black
            Button22.ForeColor = Color.Black
            Button5.ForeColor = Color.Black
            Button21.ForeColor = Color.Black
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button15_Click_1(sender As Object, e As EventArgs) Handles Button15.Click
        ToolStrip.Select()
        If Login.add_designation = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            designation.WindowState = FormWindowState.Maximized
            designation.Show()
            designation.MdiParent = Me

            Button14.ForeColor = Color.Black
            Button4.ForeColor = Color.Black
            Button15.ForeColor = Color.Red
            Button17.ForeColor = Color.Black
            Button25.ForeColor = Color.Black
            Button24.ForeColor = Color.Black
            Button23.ForeColor = Color.Black
            Button22.ForeColor = Color.Black
            Button5.ForeColor = Color.Black
            Button21.ForeColor = Color.Black
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button17_Click_1(sender As Object, e As EventArgs) Handles Button17.Click
        ToolStrip.Select()
        If Login.add_leave_type = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            LeaveSetup.WindowState = FormWindowState.Maximized
            LeaveSetup.Show()
            LeaveSetup.MdiParent = Me

            Button14.ForeColor = Color.Black
            Button4.ForeColor = Color.Black
            Button15.ForeColor = Color.Black
            Button17.ForeColor = Color.Red
            Button25.ForeColor = Color.Black
            Button24.ForeColor = Color.Black
            Button23.ForeColor = Color.Black
            Button22.ForeColor = Color.Black
            Button5.ForeColor = Color.Black
            Button21.ForeColor = Color.Black
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button25_Click(sender As Object, e As EventArgs) Handles Button25.Click
        ToolStrip.Select()
        If Login.add_employee = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            Employee.WindowState = FormWindowState.Maximized
            Employee.Show()
            Employee.MdiParent = Me

            Button14.ForeColor = Color.Black
            Button4.ForeColor = Color.Black
            Button15.ForeColor = Color.Black
            Button17.ForeColor = Color.Black
            Button25.ForeColor = Color.Red
            Button24.ForeColor = Color.Black
            Button23.ForeColor = Color.Black
            Button22.ForeColor = Color.Black
            Button5.ForeColor = Color.Black
            Button21.ForeColor = Color.Black
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click
        ToolStrip.Select()
        If Login.employee_list = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            NewEmployee.WindowState = FormWindowState.Maximized
            NewEmployee.Show()
            NewEmployee.MdiParent = Me

            Button14.ForeColor = Color.Black
            Button4.ForeColor = Color.Black
            Button15.ForeColor = Color.Black
            Button17.ForeColor = Color.Black
            Button25.ForeColor = Color.Black
            Button24.ForeColor = Color.Red
            Button23.ForeColor = Color.Black
            Button22.ForeColor = Color.Black
            Button5.ForeColor = Color.Black
            Button21.ForeColor = Color.Black
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        ToolStrip.Select()
        If Login.hr_documents = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            hrdoc.WindowState = FormWindowState.Maximized
            hrdoc.Show()
            hrdoc.MdiParent = Me

            Button14.ForeColor = Color.Black
            Button4.ForeColor = Color.Black
            Button15.ForeColor = Color.Black
            Button17.ForeColor = Color.Black
            Button25.ForeColor = Color.Black
            Button24.ForeColor = Color.Black
            Button23.ForeColor = Color.Red
            Button22.ForeColor = Color.Black
            Button5.ForeColor = Color.Black
            Button21.ForeColor = Color.Black
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        ToolStrip.Select()
        If Login.issue_leave = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            LeaveWindow.WindowState = FormWindowState.Maximized
            LeaveWindow.Show()
            LeaveWindow.MdiParent = Me

            Button14.ForeColor = Color.Black
            Button4.ForeColor = Color.Black
            Button15.ForeColor = Color.Black
            Button17.ForeColor = Color.Black
            Button25.ForeColor = Color.Black
            Button24.ForeColor = Color.Black
            Button23.ForeColor = Color.Black
            Button22.ForeColor = Color.Red
            Button5.ForeColor = Color.Black
            Button21.ForeColor = Color.Black
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles Button5.Click
        ToolStrip.Select()
        If Login.employee_loan = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            Loans.WindowState = FormWindowState.Maximized
            Loans.Show()
            Loans.MdiParent = Me

            Button14.ForeColor = Color.Black
            Button4.ForeColor = Color.Black
            Button15.ForeColor = Color.Black
            Button17.ForeColor = Color.Black
            Button25.ForeColor = Color.Black
            Button24.ForeColor = Color.Black
            Button23.ForeColor = Color.Black
            Button22.ForeColor = Color.Black
            Button5.ForeColor = Color.Red
            Button21.ForeColor = Color.Black
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        ToolStrip.Select()
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = Me

        Button14.ForeColor = Color.Black
        Button4.ForeColor = Color.Black
        Button15.ForeColor = Color.Black
        Button17.ForeColor = Color.Black
        Button25.ForeColor = Color.Black
        Button24.ForeColor = Color.Black
        Button23.ForeColor = Color.Black
        Button22.ForeColor = Color.Black
        Button5.ForeColor = Color.Black
        Button21.ForeColor = Color.Red

    End Sub

    Private Sub TestToolStripMenuItem_Click(sender As Object, e As EventArgs)
        googleDrive.ShowDialog()
    End Sub
End Class
