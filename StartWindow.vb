Public Class StartWindow
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Label1.Select()
        If SplitContainer1.SplitterDistance = 44 Then
            SplitContainer1.SplitterDistance = 169
            'Button1.Visible = True
            'Button2.Visible = True
            'Button3.Visible = True
            'Button4.Visible = True
            'Button6.Visible = True
            'Button10.Visible = True
            'Button9.Visible = True
            'Button8.Visible = True
            'Button7.Visible = True
        ElseIf SplitContainer1.SplitterDistance = 169 Then
            SplitContainer1.SplitterDistance = 44
            'Button1.Visible = False
            'Button2.Visible = False
            'Button3.Visible = False
            'Button4.Visible = False
            'Button6.Visible = False
            'Button10.Visible = False
            'Button9.Visible = False
            'Button8.Visible = False
            'Button7.Visible = False
        End If
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) 
        Label1.Select()
        If Login.database_setup = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            DatabaseSetting.ShowDialog()
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Label1.Select()
        If Login.employee_loan = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            Loans.WindowState = FormWindowState.Maximized
            Loans.Show()
            Loans.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Label1.Select()
        If Login.issue_leave = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            LeaveWindow.WindowState = FormWindowState.Maximized
            LeaveWindow.Show()
            LeaveWindow.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Label1.Select()
        If Login.hr_documents = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            hrdoc.WindowState = FormWindowState.Maximized
            hrdoc.Show()
            hrdoc.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Label1.Select()
        If Login.employee_list = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            NewEmployee.WindowState = FormWindowState.Maximized
            NewEmployee.Show()
            NewEmployee.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Label1.Select()
        If Login.add_employee = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            Employee.WindowState = FormWindowState.Maximized
            Employee.Show()
            Employee.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Label1.Select()
        If Login.add_leave_type = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            LeaveSetup.WindowState = FormWindowState.Maximized
            LeaveSetup.Show()
            LeaveSetup.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label1.Select()
        If Login.add_designation = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            designation.WindowState = FormWindowState.Maximized
            designation.Show()
            designation.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Label1.Select()
        If Login.add_department = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            departments.WindowState = FormWindowState.Maximized
            departments.Show()
            departments.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Select()

        If Login.pay_employees = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            workerparyroll.WindowState = FormWindowState.Maximized
            workerparyroll.Show()
            workerparyroll.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
        Label1.Select()
        If Login.employee_loan = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            Loans.WindowState = FormWindowState.Maximized
            Loans.Show()
            Loans.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click
        Label1.Select()
        If Login.issue_leave = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            LeaveWindow.WindowState = FormWindowState.Maximized
            LeaveWindow.Show()
            LeaveWindow.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click
        Label1.Select()
        If Login.hr_documents = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            hrdoc.WindowState = FormWindowState.Maximized
            hrdoc.Show()
            hrdoc.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click
        Label1.Select()
        If Login.employee_list = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            NewEmployee.WindowState = FormWindowState.Maximized
            NewEmployee.Show()
            NewEmployee.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Label10_Click(sender As Object, e As EventArgs) Handles Label10.Click
        Label1.Select()
        If Login.add_employee = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            Employee.WindowState = FormWindowState.Maximized
            Employee.Show()
            Employee.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click
        Label1.Select()
        If Login.add_leave_type = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            LeaveSetup.WindowState = FormWindowState.Maximized
            LeaveSetup.Show()
            LeaveSetup.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        Label1.Select()
        If Login.add_designation = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            designation.WindowState = FormWindowState.Maximized
            designation.Show()
            designation.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        Label1.Select()
        If Login.add_department = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            departments.WindowState = FormWindowState.Maximized
            departments.Show()
            departments.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        Label1.Select()

        If Login.pay_employees = "Y" Then
            For Each ChildForm As Form In MainInterface.MdiChildren
                ChildForm.Close()
            Next

            workerparyroll.WindowState = FormWindowState.Maximized
            workerparyroll.Show()
            workerparyroll.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Label1.Select()
        If Login.add_employee = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            Employee.WindowState = FormWindowState.Maximized
            Employee.Show()
            Employee.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Label1.Select()
        If Login.add_employee = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            NewEmployee.WindowState = FormWindowState.Maximized
            NewEmployee.Show()
            NewEmployee.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Label1.Select()

        If Login.pay_employees = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            workerparyroll.WindowState = FormWindowState.Maximized
            workerparyroll.Show()
            workerparyroll.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Label1.Select()
        If Login.issue_leave = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            LeaveWindow.WindowState = FormWindowState.Maximized
            LeaveWindow.Show()
            LeaveWindow.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        Label1.Select()
        If Login.add_department = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            departments.WindowState = FormWindowState.Maximized
            departments.Show()
            departments.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        Label1.Select()
        If Login.add_designation = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            designation.WindowState = FormWindowState.Maximized
            designation.Show()
            designation.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        Label1.Select()
        If Login.hr_documents = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            hrdoc.WindowState = FormWindowState.Maximized
            hrdoc.Show()
            hrdoc.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Label1.Select()
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        Label1.Select()
        If Login.scan_documents = "Y" Then
            For Each ChildForm As Form In Me.MdiChildren
                ChildForm.Close()
            Next

            scanDocuments.WindowState = FormWindowState.Maximized
            scanDocuments.Show()
            scanDocuments.MdiParent = MainInterface
        Else
            MessageBox.Show("You are not permitted to use this funtion.", "Payroll", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
End Class