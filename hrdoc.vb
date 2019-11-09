Imports System.IO
Imports MySql.Data.MySqlClient
Public Class hrdoc
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'OpenFileDialog1.Filter = "PDF Files |*.pdf"

        'If OpenFileDialog1.ShowDialog = DialogResult.OK Then
        '    AxAcroPDF1.src = OpenFileDialog1.FileName
        'End If
        Dim print As New Process()

        If Combo_department.SelectedIndex = 0 Then
            With print
                .StartInfo.CreateNoWindow = True
                .StartInfo.Verb = "print"
                .StartInfo.FileName = appPath + "\HRdoc\leave.pdf"
                .Start()
                .Close()
            End With
        ElseIf Combo_department.SelectedIndex = 1 Then
            With print
                .StartInfo.CreateNoWindow = True
                .StartInfo.Verb = "print"
                .StartInfo.FileName = appPath + "\HRdoc\employeedisciplinaryactionform.pdf"
                .Start()
                .Close()
            End With
        ElseIf Combo_department.SelectedIndex = 2 Then
            With print
                .StartInfo.CreateNoWindow = True
                .StartInfo.Verb = "print"
                .StartInfo.FileName = appPath + "\HRdoc\Warning-Form.pdf"
                .Start()
                .Close()
            End With
        ElseIf Combo_department.SelectedIndex = 3 Then
            'AxAcroPDF1.src = appPath + "\HRdoc\Warning-Form.pdf"
        ElseIf Combo_department.SelectedIndex = 4 Then
            With print
                .StartInfo.CreateNoWindow = True
                .StartInfo.Verb = "print"
                .StartInfo.FileName = appPath + "\HRdoc\Form-Overtime-Approval.pdf"
                .Start()
                .Close()
            End With
        ElseIf Combo_department.SelectedIndex = 5 Then
            With print
                .StartInfo.CreateNoWindow = True
                .StartInfo.Verb = "print"
                .StartInfo.FileName = appPath + "\HRdoc\employment-contract-revised.pdf"
                .Start()
                .Close()
            End With
        End If
    End Sub

    Private Sub hrdoc_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If (Not System.IO.Directory.Exists(appPath + "\HRdoc")) Then
                System.IO.Directory.CreateDirectory(appPath + "\HRdoc")
            End If

            cn.Open()
            Combo_department.Items.Clear()
            Combo_department.Items.Add("-- Select Document --")
            Dim Namequery As String = "SELECT Code,description FROM hrdocuments WHERE description != ''"
            cmd = New MySqlCommand(Namequery, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim sInvet = dr.GetString("Description").ToString
                Combo_department.Items.Add(sInvet)
            End While
            cn.Close()
            Combo_department.SelectedIndex = 0
        Catch ex As Exception
            MessageBox.Show(ex.Message, "HR Documents", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Combo_department_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Combo_department.SelectedIndexChanged
        If Combo_department.SelectedIndex = 1 Then
            AxAcroPDF1.src = appPath + "\HRdoc\leave.pdf"
        ElseIf Combo_department.SelectedIndex = 2 Then
            AxAcroPDF1.src = appPath + "\HRdoc\employeedisciplinaryactionform.pdf"
        ElseIf Combo_department.SelectedIndex = 3 Then
            AxAcroPDF1.src = appPath + "\HRdoc\Warning-Form.pdf"
        ElseIf Combo_department.SelectedIndex = 4 Then
            'AxAcroPDF1.src = appPath + "\HRdoc\Warning-Form.pdf"
        ElseIf Combo_department.SelectedIndex = 5 Then
            AxAcroPDF1.src = appPath + "\HRdoc\Form-Overtime-Approval.pdf"
        ElseIf Combo_department.SelectedIndex = 6 Then
            AxAcroPDF1.src = appPath + "\HRdoc\employment-contract-revised.pdf"
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) 

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label51.Select()
        Close()
    End Sub

    Private Sub hrdoc_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub
End Class