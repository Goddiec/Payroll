Imports System.Runtime.InteropServices
Imports MySql.Data.MySqlClient
Public Class LeaveWindow
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter
    Dim ds As DataSet = New DataSet
    Dim dv As DataView

    <DllImport("Gdi32.dll", EntryPoint:="CreateRoundRectRgn")>
    Private Shared Function CreateRoundRectRgn(ByVal iLeft As Integer, ByVal iTop As Integer, ByVal iRight As Integer, ByVal iBottom As Integer, ByVal iWidth As Integer, ByVal iHeight As Integer) As IntPtr
    End Function

    Sub LeaveData()
        Try
            cn.Open()
            ComboLeave.Items.Clear()
            Dim leavequery As String = "SELECT * FROM employeeleave WHERE Description != ''"
            cmd = New MySqlCommand(leavequery, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim sInvet = dr.GetString("Description").ToString
                ComboLeave.Items.Add(sInvet)
            End While
            cn.Close()
            ComboLeave.SelectedIndex = 0
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Leave", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub LeaveWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LeaveData()
        leaveHistory()
        DataGridView1.RowTemplate.Height = 35


        Label70.AutoSize = False
        Label70.Padding = New Padding(1, 1, 1, 1)
        Label70.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label70.Width - 2, Label70.Height - 2, 5, 1))

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If txtcode.Text = "" Then
            MessageBox.Show("Please enter employee code.", "Leave", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtcode.Select()
        Else
            Dim dialog As New DialogResult
        dialog = MsgBox("Do you want to activate leave for employee " & txtname.Text & " ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Leave")

        If dialog = DialogResult.No Then
            DialogResult.Cancel.ToString()
        Else
            If True Then
                Dim dt1 As DateTime = Convert.ToDateTime(DateTimePicker1.Text)
                Dim dt2 As DateTime = Convert.ToDateTime(DateTimePicker2.Text)
                Dim ts As TimeSpan = dt2.Subtract(dt1)
                If Convert.ToInt32(ts.Days) >= 0 Then
                    'MessageBox.Show("Total Days are " & Convert.ToInt32(ts.Days))
                    txtNum.Text = Convert.ToInt32(ts.Days)
                    Try
                        cn.Open()
                        cmd.Connection = cn
                        cmd.CommandType = CommandType.Text
                        cmd.CommandText = "INSERT INTO leavehistory (employee_code, leave_type, start_date, end_date, num_days) VALUES ('" & txtcode.Text & "', '" & ComboLeave.Text & "',  '" & DateTimePicker1.Text & "',  '" & DateTimePicker2.Text & "', '" & CInt(txtNum.Text) & "');"
                        dr = cmd.ExecuteReader
                        cn.Close()

                        MessageBox.Show("Leave have successfully activated", "Leave", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        leaveHistory()
                        clear()
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Finally
                        cn.Dispose()
                    End Try
                Else
                    MessageBox.Show("Invalid Date Input")
                End If
            End If
        End If
        End If
    End Sub

    Sub clear()
        txtcode.Clear()
        txtname.Text = ""
        txtNum.Text = ""
        DateTimePicker1.Value = Today.Date
        DateTimePicker2.Value = Today.Date
        ComboLeave.SelectedIndex = 0
    End Sub

    Sub leaveHistory()
        Try
            Dim dt1 As New DataTable
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT leave_type As 'Description', start_date As 'From', end_date As 'To', num_days As 'Days' FROM leavehistory WHERE employee_code = '" & txtcode.Text & "'"
            End With
            da.SelectCommand = cmd
            dt1.Clear()
            da.Fill(dt1)
            DataGridView1.DataSource = dt1
            cn.Close()

            DataGridView1.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            DataGridView1.Columns(1).Width = 100
            DataGridView1.Columns(2).Width = 100
            DataGridView1.Columns(3).Width = 100
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employee", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub LoadDB()
        Try
            cn.Open()
            Dim Namequery As String = "SELECT first_name, last_name FROM employee WHERE code = '" & txtcode.Text & "'"
            cmd = New MySqlCommand(Namequery, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtname.Text = dr("first_name").ToString() & " " & dr("last_name").ToString()
                'refugee_val = dr("refugee").ToString()
                'default_phy_res_address_val = dr("default_phy_res_address").ToString()
            End While
            cn.Close()
            leaveHistory()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Leave", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub txtcode_Leave(sender As Object, e As EventArgs) Handles txtcode.Leave
        If txtcode.Text <> String.Empty Then
            LoadDB()
        End If
    End Sub

    Private Sub DateTimePicker2_Leave(sender As Object, e As EventArgs) Handles DateTimePicker2.Leave
        Dim dt1 As DateTime = Convert.ToDateTime(DateTimePicker1.Text)
        Dim dt2 As DateTime = Convert.ToDateTime(DateTimePicker2.Text)
        Dim ts As TimeSpan = dt2.Subtract(dt1)
        txtNum.Text = Convert.ToInt32(ts.Days)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label2.Select()
        Close()

        For Each ChildForm As Form In MainInterface.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub

    Public EmpLeaveSear As Char
    Private Sub Label70_Click(sender As Object, e As EventArgs) Handles Label70.Click
        EmpLeaveSear = "Y"
        SearchEmployee.ShowDialog()
    End Sub
End Class