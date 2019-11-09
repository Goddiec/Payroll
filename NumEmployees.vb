Imports System.IO
Public Class NumEmployees
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Select()
        Close()

        For Each ChildForm As Form In MainInterface.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub

    Dim numEmp As String
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Label1.Select()
        If ComboNoEmp.SelectedIndex = 0 Then
            MsgBox("Please select number of employees to be rigistered.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Registration")
            ComboNoEmp.Select()
        Else
            RegisterNow()
        End If
    End Sub

    Dim RegDays As Integer
    Dim MyDaysCheck As Integer
    Sub RegisterNow()
        txtRegNo1.Select()
        If txtRegNo1.Text = "" Then
            MsgBox("Please enter Serial number!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Registration")
            txtRegNo1.Select()
            txtRegNo1.Focus()
        Else
            Try
                If ComboNoEmp.SelectedIndex = 1 Then
                    numEmp = 5
                ElseIf ComboNoEmp.SelectedIndex = 2 Then
                    numEmp = 15
                ElseIf ComboNoEmp.SelectedIndex = 3 Then
                    numEmp = 30
                ElseIf ComboNoEmp.SelectedIndex = 4 Then
                    numEmp = 45
                ElseIf ComboNoEmp.SelectedIndex = 5 Then
                    numEmp = 60
                ElseIf ComboNoEmp.SelectedIndex = 6 Then
                    numEmp = 90
                ElseIf ComboNoEmp.SelectedIndex = 7 Then
                    numEmp = 99999999
                End If

                MyDaysCheck = CInt(Mid(txtRegNo1.Text, 1, 2)) - CInt(Mid(txtSerialNumber.Text, 1, 1))

                If MyDaysCheck = 2 Or MyDaysCheck = 7 Or MyDaysCheck = 14 Or MyDaysCheck = 14 Or MyDaysCheck = 30 Or MyDaysCheck = 60 Or MyDaysCheck = 90 Then
                    If CInt(txtRegNo1.Text.Substring(2)).ToString("D2") = CInt(txtRegNo1.Text.Substring(2)).ToString("D2") And (CInt(Mid(txtRegNo2.Text, 1, 2))).ToString("D2") = (CInt(Mid(txtSerialNumber.Text, 3, 1)) + 6).ToString("D2") And (CInt(Mid(txtRegNo2.Text, 3, 2))).ToString("D2") = (CInt(Mid(txtSerialNumber.Text, 4, 1)) + 6).ToString("D2") Then
                        My.Settings.EmpNum = CInt(numEmp)
                        My.Settings.Save()
                        Me.Close()
                        MsgBox("Employee registration has been successful.", MsgBoxStyle.Information, "Registration")
                        'Application.ExitThread()
                        Exit Sub
                    Else
                        MsgBox("Invalid registration key", MsgBoxStyle.Critical, "Registration")
                    End If
                Else
                    MsgBox("Invalid registration key", MsgBoxStyle.Critical, "Registration")
                End If
            Catch ex As Exception
                MsgBox("An error had occurred, the system will now close.", MsgBoxStyle.Critical, "Error")
                Me.Close()
                Application.ExitThread()
            End Try
        End If
    End Sub

    Shared random As New Random()
    Private Sub NumEmployees_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Select()
        ComboNoEmp.SelectedIndex = 0
        txtSerialNumber.Text = random.Next(1, 9) & random.Next(1, 9) & random.Next(1, 9) & random.Next(1, 9)
    End Sub
End Class