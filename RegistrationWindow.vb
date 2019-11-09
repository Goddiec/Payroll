Imports System.IO
Imports System.Text
Public Class RegistrationWindow

    Sub DisableIcons()
        MainInterface.EditMenu.Enabled = False
        MainInterface.Button14.Enabled = False
        MainInterface.Button4.Enabled = False
        MainInterface.Button15.Enabled = False
        MainInterface.Button17.Enabled = False
        MainInterface.Button25.Enabled = False
        MainInterface.Button24.Enabled = False
        MainInterface.Button23.Enabled = False
        MainInterface.Button22.Enabled = False
        MainInterface.Button5.Enabled = False
        MainInterface.Button21.Enabled = False
        MainInterface.EditMenu.Enabled = False
        MainInterface.EmployeesToolStripMenuItem.Enabled = False
        MainInterface.SetupToolStripMenuItem.Enabled = False
        MainInterface.ViewMenu.Enabled = False
        MainInterface.BackupToolStripMenuItem.Visible = False
        MainInterface.RestoreToolStripMenuItem.Enabled = False
        MainInterface.ScanDocumentsToolStripMenuItem.Enabled = False
    End Sub

    Private Sub RegistrationWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtCompanyName.Clear()
        txtRegNo1.Clear()
        txtRegNo2.Clear()
        txtCompanyName.ReadOnly = False
        Button3.Visible = True
        txtCompanyName.Select()
        My.Settings.Save()
        My.Settings.Reload()
        DisableIcons()
        'Using sr As New System.IO.StreamReader(appPath + "\Admin.inf")
        '    Dim Line As String = sr.ReadLine
        '    Dim Line1 As String = sr.ReadLine
        '    Dim Line2 As String = sr.ReadLine
        '    Dim Line3 As String = sr.ReadLine
        '    Dim Line4 As String = sr.ReadLine
        '    Dim Line5 As String = sr.ReadLine
        '    Dim Line6 As String = sr.ReadLine
        '    Dim Line7 As String = sr.ReadLine
        '    Dim Line8 As String = sr.ReadLine

        '    Label4.Text = Line & vbNewLine & Line1 & vbNewLine & Line2 & Line3 & Line4
        'End Using
    End Sub

    Dim RegDays As Integer
    Dim MyDaysCheck As Integer
    Sub RegisterNow()
        txtRegNo1.Select()
        If txtCompanyName.Text = "" Then
            MsgBox("Please enter a company name!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Registration")
            txtCompanyName.Select()
            txtCompanyName.Focus()
        ElseIf txtCompanyName.TextLength < 5 Then
            MsgBox("Please enter a company name with more than five characters!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Registration")
            txtCompanyName.SelectAll()
            txtCompanyName.Focus()
        ElseIf txtRegNo1.Text = "" Then
            MsgBox("Please enter Serial number!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Registration")
            txtRegNo1.Select()
            txtRegNo1.Focus()
        Else
            Try
                MyDaysCheck = CInt(Mid(txtRegNo1.Text, 1, 2)) - CInt(Mid(txtSerialNumber.Text, 1, 1))

                If MyDaysCheck = 2 Or MyDaysCheck = 7 Or MyDaysCheck = 14 Or MyDaysCheck = 14 Or MyDaysCheck = 30 Or MyDaysCheck = 60 Or MyDaysCheck = 90 Then
                    If CInt(txtRegNo1.Text.Substring(2)).ToString("D2") = CInt(txtRegNo1.Text.Substring(2)).ToString("D2") And (CInt(Mid(txtRegNo2.Text, 1, 2))).ToString("D2") = (CInt(Mid(txtSerialNumber.Text, 3, 1)) + 6).ToString("D2") And (CInt(Mid(txtRegNo2.Text, 3, 2))).ToString("D2") = (CInt(Mid(txtSerialNumber.Text, 4, 1)) + 6).ToString("D2") Then
                        RegDays = CInt(Mid(txtRegNo1.Text, 1, 2)) - CInt(Mid(txtSerialNumber.Text, 1, 1))
                        My.Settings.ExpiryDate = Now.AddDays(RegDays)
                        My.Settings.Save()
                        Me.Close()
                        MsgBox("The system will close to save the pending registration.", MsgBoxStyle.Information, "Registration")
                        Application.ExitThread()
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

    Dim c As String
    Dim d As String
    Dim w As String
    Dim wx As String
    Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)

    Shared random As New Random()
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If txtCompanyName.Text = "" Then
            MsgBox("Please enter a company name!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Registration")
            txtCompanyName.Select()
            txtCompanyName.Focus()
        ElseIf txtCompanyName.TextLength < 5 Then
            MsgBox("Please enter a company name with more than five characters!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Registration")
            txtCompanyName.SelectAll()
            txtCompanyName.Focus()
        Else
            'Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
            'Dim r As New Random
            'Dim sb As New StringBuilder
            'For i As Integer = 1 To 8
            '    Dim idx As Integer = r.Next(0, 35)
            '    sb.Append(s.Substring(idx, 1))
            'Next

            'txtSerialNumber.Text = sb.ToString()
            txtSerialNumber.Text = random.Next(1, 9) & random.Next(1, 9) & random.Next(1, 9) & random.Next(1, 9)
            Button3.Visible = False
            txtRegNo1.Focus()
            txtRegNo1.Select()
            txtCompanyName.ReadOnly = True
        End If
    End Sub

    Private Sub txtCompanyName_TextChanged(sender As Object, e As EventArgs) Handles txtCompanyName.TextChanged

    End Sub

    Private Sub txtCompanyName_MouseDown(sender As Object, e As MouseEventArgs) Handles txtCompanyName.MouseDown
        'If txtCompanyName.Text = "" Then
        '    txtCompanyName.Text = ""
        'Else
        '    txtCompanyName.ForeColor = Color.Black
        'End If
    End Sub

    Private Sub txtCompanyName_LostFocus(sender As Object, e As EventArgs) Handles txtCompanyName.LostFocus
        'If txtCompanyName.Text = "" Then
        '    txtCompanyName.Text = "Enter your company name here"
        '    txtCompanyName.ForeColor = Color.LightGray
        'Else
        '    txtCompanyName.ForeColor = Color.Black
        'End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        RegisterNow()
    End Sub

    Sub cancelReg()
        txtRegNo1.Select()
        txtCompanyName.Clear()
        txtSerialNumber.Text = ""
        txtRegNo1.Clear()
        If MainInterface.reg = "Y" Then
            Close()
        ElseIf MainInterface.reg = "N" Then
            Application.ExitThread()
        End If

        MainInterface.reg = "N"
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Label1.Select()
        cancelReg()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        MsgBox((CInt(Mid(txtSerialNumber.Text, 4, 1)) + 6).ToString("D2"))
    End Sub
End Class