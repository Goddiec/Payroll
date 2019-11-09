Imports System.ComponentModel
Imports System.IO
Imports System.Threading
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Public Class NewEmployee
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager
    <STAThread>
    Private Sub Button8_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub MyWorkMethod()
        If ExportEmployees.Visible = True Then
            PleaseWait.Hide()
        Else
            ExportEmployees.ShowDialog()
            Threading.Thread.Sleep(1000)
            Dim t As New Threading.Thread(AddressOf MyWorkMethod)
            t.Abort()
            PleaseWait.Hide()
        End If
    End Sub

    Private Delegate Sub ConsumeData_Delegate(ByVal msg As String)

    Private Sub ConsumeData(ByVal msg As String)
        Try
            If InvokeRequired Then
                Dim delg As New ConsumeData_Delegate(AddressOf ConsumeData)
                Dim obj(0) As String
                obj(0) = msg
                Me.Invoke(delg, obj)
            Else
                Me.Text = msg
                MsgBox(msg)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Button1.Enabled = True
        End Try
    End Sub

    Dim result As Integer
    Dim sql As String
    Dim caption As String
    Sub saveImage()
        Try
            Dim myAdapter As New MySqlDataAdapter
            Dim sqlquery = "SELECT * FROM employee WHERE code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
            Dim myCommand As New MySqlCommand()
            myCommand.Connection = cn
            myCommand.CommandText = sqlquery
            myAdapter.SelectCommand = myCommand
            cn.Open()
            Dim ms As New MemoryStream
            caption = System.IO.Path.GetFileName(OpenFileDialog1.FileName)
            Dim bm As Bitmap = New Bitmap(PictureBox1.Image)
            bm.Save(ms, PictureBox1.Image.RawFormat)

            Dim arrPic() As Byte = ms.GetBuffer()

            sqlquery = "UPDATE employee SET ImageFile=@ImageFile WHERE code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"

            myCommand = New MySqlCommand(sqlquery, cn)
            myCommand.Parameters.AddWithValue("@ImageFile", arrPic)
            myCommand.ExecuteNonQuery()
            cn.Close()

            cn.Open()
            sql = "UPDATE employee SET Caption=@Caption WHERE code =  '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
            cmd = New MySqlCommand
            With cmd
                .Connection = cn
                .CommandText = sql
                .Parameters.AddWithValue("@Caption", caption)
                result = .ExecuteNonQuery()
            End With
            cn.Close()

            MessageBox.Show("Picture has been succefully changed.", "Employee", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Select()
        Try
            With OpenFileDialog1
                .CheckFileExists = True
                .CheckPathExists = True
                .DefaultExt = "jpg"
                .DereferenceLinks = True
                .FileName = ""
                .Filter = "(*.jpg)|*.jpg|(*.png)|*.png|(*.jpg)|*.jpg|All files|*.*"
                .Multiselect = False
                .RestoreDirectory = True
                .Title = "Select a file to open"
                .ValidateNames = True
                If .ShowDialog = DialogResult.OK Then
                    Try
                        PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)

                        Dim dialog As New DialogResult
                        dialog = MsgBox("Are you sure you want to change the employee picture?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Employee")

                        If dialog = DialogResult.No Then
                            imageRet()
                        Else
                            saveImage()
                        End If
                    Catch fileException As Exception
                        Throw fileException
                    End Try
                End If

            End With

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Sub LoadData()
        Try
            cn.Open()
            ComboDepart.Items.Clear()
            ComboDepart.Items.Add("All Departments")
            Dim leavequery As String = "SELECT * FROM departments WHERE Description != ''"
            cmd = New MySqlCommand(leavequery, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim sInvet = dr.GetString("Description").ToString
                ComboDepart.Items.Add(sInvet)
            End While
            cn.Close()

            ComboDepart.SelectedIndex = 0
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Department", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub Employeess1()
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
    End Sub

    Dim deptartcode As String
    Sub Employeess()
        Try
            cn.Open()
            Dim vat As String = "SELECT Code FROM departments WHERE Description = '" & ComboDepart.Text & "'"
            cmd = New MySqlCommand(vat, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                deptartcode = dr.GetString("Code").ToString
            End While
            cn.Close()

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
                                ON e.designation = m.CODE WHERE e.department = '" & deptartcode & "'
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

        End Try
    End Sub

    Sub EmployeessCode()
        Try
            cn.Open()
            Dim vat As String = "SELECT Code FROM departments WHERE Description = '" & ComboDepart.Text & "'"
            cmd = New MySqlCommand(vat, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                deptartcode = dr.GetString("Code").ToString
            End While
            cn.Close()

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
                                ON e.designation = m.CODE WHERE e.code LIKE '%" & txtFind.Text & "%'
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

        End Try
    End Sub

    Sub LoadDataFiles()
        LoadData()
        Employeess1()
        DataGridView1.RowTemplate.Height = 35
        Panel1.Select()

        LoadDB()
        imageRet()
        ActivateDeactivate()
        Button1.Enabled = True
        Button2.Enabled = True
        Button7.Enabled = True
        Button6.Enabled = True
        Button5.Enabled = True
    End Sub

    Sub NoEmployees()
        LoadData()
        Employeess1()
        DataGridView1.RowTemplate.Height = 35
        Panel1.Select()
        Button1.Enabled = False
        Button2.Enabled = False
        Button7.Enabled = False
        Button6.Enabled = False
        Button5.Enabled = False
        Label9.Text = ""
        Label8.Text = ""
        Label7.Text = ""
        Label6.Text = ""
    End Sub

    Dim empCheck As Char
    Sub CheckEmployees()
        Try
            cn.Open()
            Dim Query As String
            Query = "SELECT ID FROM employee"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                empCheck = "Y"
            Else
                empCheck = "N"
            End If
            cn.Close()

            If empCheck = "Y" Then
                LoadDataFiles()
            ElseIf empCheck = "N" Then
                NoEmployees()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub NewEmployee_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckEmployees()
    End Sub

    Private Sub LoadDB()
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT e.code,e.first_name,e.last_name, m.description As 'designation', d.Description As 'department',e.email
                            FROM employee e
                            LEFT JOIN departments d
                            ON e.department = d.Code
                            LEFT JOIN designation m
                            ON e.designation = m.CODE
                            WHERE e.code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"

            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Label1.Text = dr("first_name").ToString() & " " & dr("last_name").ToString()
                Label9.Text = dr("code").ToString()
                Label8.Text = dr("designation").ToString()
                Label7.Text = dr("department").ToString()
                Label6.Text = dr("email").ToString()
            End While
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub imageRet()
        Try
            cn.Open()
            Dim cmd1 As MySqlCommand
            cmd1 = New MySqlCommand("Select ImageFile from employee where code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'", cn)
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

    Private Sub LoadDB1()
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT first_name,last_name,code,designation,department,email FROM employee ORDER BY ID"
            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Label1.Text = dr("first_name").ToString() & " " & dr("last_name").ToString()
                Label9.Text = dr("code").ToString()
                Label8.Text = dr("designation").ToString()
                Label7.Text = dr("department").ToString()
                Label6.Text = dr("email").ToString()

            End While
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub ActivateDeactivate()
        If DataGridView1.CurrentRow.Cells(4).Value = "True" Then
            Button7.Text = "DEACTIVATE EMPLOYEE"
            Button7.BackColor = Color.Tomato
        Else
            Button7.Text = "ACTIVATE EMPLOYEE"
            Button7.BackColor = Color.PaleGreen
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        LoadDB()
        imageRet()
        ActivateDeactivate()
    End Sub

    Private Sub ComboDepart_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboDepart.SelectedIndexChanged
        Try
            cn.Open()
            Dim Query As String
            Query = "SELECT ID FROM employee"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                empCheck = "Y"
            Else
                empCheck = "N"
            End If
            cn.Close()

            If empCheck = "Y" Then
                If ComboDepart.SelectedIndex = 0 Then
                    Employeess1()
                Else
                    Employeess()
                End If
            ElseIf empCheck = "N" Then

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Label1.Select()
        ExportEmployees.Label1.Text = "Export Employees"
        ExportEmployees.ShowDialog()
        ExportEmployees.txtPath.Visible = False
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Label1.Select()
        ExportEmployees.Label1.Text = "Import Employees"
        ExportEmployees.ShowDialog()
        ExportEmployees.txtPath.Visible = True
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub DataGridView1_CellFormatting(ByVal sender As Object, ByVal e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting
        For i As Integer = 0 To Me.DataGridView1.Rows.Count - 1
            If Me.DataGridView1.Rows(i).Cells(4).Value = "False" Then
                Me.DataGridView1.Rows(i).Cells(4).Style.ForeColor = Color.Red
            ElseIf Me.DataGridView1.Rows(i).Cells(4).Value = "True" Then
                Me.DataGridView1.Rows(i).Cells(4).Style.ForeColor = Color.Green
            End If
        Next
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Label1.Select()
        Dim dialog As New DialogResult

        If DataGridView1.CurrentRow.Cells(4).Value = "True" Then
            dialog = MsgBox("Are you sure want to deactivate employee " & DataGridView1.CurrentRow.Cells(1).Value.ToString.ToUpper & " ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Employee")
        ElseIf DataGridView1.CurrentRow.Cells(4).Value = "False" Then
            dialog = MsgBox("Are you sure want to activate employee " & DataGridView1.CurrentRow.Cells(1).Value.ToString.ToUpper & " ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Employee")
        End If

        If dialog = DialogResult.No Then
            DialogResult.Cancel.ToString()
        Else
            Try
                If DataGridView1.CurrentRow.Cells(4).Value = "True" Then
                    cn.Open()
                    With cmd
                        .Connection = cn
                        .CommandText = "UPDATE employee SET employed = 'N' WHERE Code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
                        .ExecuteNonQuery()
                    End With
                    cn.Close()
                    MessageBox.Show("Employee have been deactivated.", "Departments", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf DataGridView1.CurrentRow.Cells(4).Value = "False" Then
                    cn.Open()
                    With cmd
                        .Connection = cn
                        .CommandText = "UPDATE employee SET employed = 'Y' WHERE Code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
                        .ExecuteNonQuery()
                    End With
                    cn.Close()
                    MessageBox.Show("Employee have been activated.", "Departments", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                Employeess1()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                cn.Dispose()
            End Try
        End If
    End Sub

    Dim second As Integer
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        second = second + 1
        If second >= 10 Then
            Timer1.Stop() 'Timer stops functioning
            'MsgBox("Timer Stopped....")
            LoadDataFiles()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        'Employee.WindowState = FormWindowState.Maximized
        'Employee.Show()
        'Employee.MdiParent = MainInterface

        'Employee.code.Enabled = False
        'Employee.SaveLoadDB()
        'TotalSearch()
    End Sub

    Private Sub hrdoc_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub
    Private Sub txtFind_TextChanged(sender As Object, e As EventArgs) Handles txtFind.TextChanged
        EmployeessCode()
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If ExportEmployees.Visible = True Then
            ExportEmployees.ShowDialog()
        Else
            PleaseWait.Hide()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Label1.Select()
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        Employee.WindowState = FormWindowState.Maximized
        Employee.Show()
        Employee.MdiParent = MainInterface

        'Employee.code.Enabled = False
        'Employee.SaveLoadDB()
        'TotalSearch()
    End Sub

    Private Sub Button8_Click_1(sender As Object, e As EventArgs) Handles Button8.Click
        Label1.Select()
        Close()
    End Sub

    Public SerEmp1 As Char
    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click
        SerEmp1 = "Y"
        SearchEmployee.ShowDialog()
    End Sub

    Dim empHistory As Char
    Sub CheckEmp()
        Try
            cn.Open()
            Dim Query As String
            Query = "select * from employee
                     join transactions
                     on employee.code = transactions.emp_code where employee.code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                empHistory = "Y"
            Else
                empHistory = "N"
            End If
            cn.Close()

            If empHistory = "N" Then
                Dim dialog As New DialogResult

                dialog = MsgBox("Are you sure want to delete this employee " & DataGridView1.CurrentRow.Cells(1).Value.ToString.ToUpper & " ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Employee")

                If dialog = DialogResult.No Then
                    DialogResult.Cancel.ToString()
                Else
                    cn.Open()
                    With cmd
                        .Connection = cn
                        .CommandText = "delete FROM employee WHERE code = '" & DataGridView1.CurrentRow.Cells(0).Value & "';"
                        .ExecuteNonQuery()
                    End With
                    cn.Close()

                    cn.Open()
                    With cmd
                        .Connection = cn
                        .CommandText = "delete FROM loans WHERE employee_code = '" & DataGridView1.CurrentRow.Cells(0).Value & "';"
                        .ExecuteNonQuery()
                    End With
                    cn.Close()

                    cn.Open()
                    With cmd
                        .Connection = cn
                        .CommandText = "delete FROM scheduletable WHERE code = '" & DataGridView1.CurrentRow.Cells(0).Value & "';"
                        .ExecuteNonQuery()
                    End With
                    cn.Close()

                    cn.Open()
                    With cmd
                        .Connection = cn
                        .CommandText = "delete FROM leavehistory WHERE employee_code = '" & DataGridView1.CurrentRow.Cells(0).Value & "';"
                        .ExecuteNonQuery()
                    End With
                    cn.Close()

                    cn.Open()
                    With cmd
                        .Connection = cn
                        .CommandText = "delete FROM deductiontransaction WHERE emp_code = '" & DataGridView1.CurrentRow.Cells(0).Value & "';"
                        .ExecuteNonQuery()
                    End With
                    cn.Close()

                    cn.Open()
                    With cmd
                        .Connection = cn
                        .CommandText = "delete FROM allowances WHERE emp_code = '" & DataGridView1.CurrentRow.Cells(0).Value & "';"
                        .ExecuteNonQuery()
                    End With
                    cn.Close()

                    MessageBox.Show("Employee has successfully been deleted", "Employee", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadDataFiles()
                End If
            ElseIf empHistory = "Y" Then
                MessageBox.Show("Employee has history and cannot be deleted", "Employee", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Label1.Select()
        CheckEmp()
    End Sub
End Class