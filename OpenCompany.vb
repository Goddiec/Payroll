Imports Microsoft.Win32
Imports System.IO
Public Class OpenCompany
    Dim second As Integer
    Dim DSN As String
    Dim path1 As String
    Dim regBaseKey As RegistryKey = Registry.LocalMachine

    Public Sub openCom()
        Try
            ListBox1.Items.Clear()
            Dim regkey As RegistryKey = regBaseKey.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Duluxe", False)
            ListBox1.Items.AddRange(regkey.GetValueNames)
            ListBox1.SetSelected(0, True)
        Catch ex As Exception
            MessageBox.Show("Access Denied " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

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

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        DisableIcons()
        startup.Close()
        Panel1.Select()
        Close()
    End Sub

    'Dim InvenList As New Nav
    Dim com As String
    Private Sub OPENCOMPANY_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel1.Select()
        Me.KeyPreview = True
        DisableIcons()
        openCom()
        ListBox1.SelectedIndex = 0 'My.Settings.LastAccess = ListBox1.SelectedIndex
        MainInterface.Text = "Payroll"

        'CheckBox1.Checked = My.Settings.CheckLast

        'If MainInterface.openTImer = "N" Then
        '    Timer2.Stop()
        '    MainInterface.openTImer = "Y"
        'Else
        '    'Dim menu As Int64
        '    'menu = GetSystemMenu(Me.Handle.ToInt32, choice.Disable)
        '    'RemoveMenu(CInt(menu), Position.rmclose, MF_POSITION)

        '    'Panel1.Enabled = False
        '    'If CheckBox1.Checked = True Then
        '    '    Timer2.Start()
        '    'End If
        '    MainInterface.openTImer = "Y"
        'End If

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        For i As Int16 = 0 To ListBox1.SelectedItems.Count - 1
            Label1.Text = ""
            Label1.Text += ListBox1.SelectedItems.Item(i).ToString() ' & ControlChars.NewLine
            DatabaseSetting.txt_database.Text = Trim(Label1.Text)
        Next
    End Sub

    Sub save()
        Try
            Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
            File.Create(appPath + "\company.ini").Dispose()
            Using sw As New System.IO.StreamWriter(appPath + "\company.ini", False)
                sw.WriteLine(Label1.Text)
            End Using
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Public openOpen As Char
    Private Sub OpenCompany_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            DisableIcons()
            startup.Close()
            Panel1.Select()
            Close()
        End If

        'If (e.Alt AndAlso (e.KeyCode = Keys.O)) Then
        '    Panel1.Select()
        '    LOGIN.ShowDialog()
        'End If

        'If (e.Alt AndAlso (e.KeyCode = Keys.C)) Then
        '    DisableIcons()
        '    'STARTUP.Close()
        '    Panel1.Select()
        '    Close()
        'End If

        'If (e.Alt AndAlso (e.KeyCode = Keys.M)) Then
        '    manage = "Y"
        '    Panel1.Select()
        '    CompanyAdd.ShowDialog()
        'End If

        If (e.Control AndAlso (e.KeyCode = Keys.I)) Then
            DatabaseSetting.ShowDialog()
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        Panel1.Select()
        DatabaseSetting.ShowDialog()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Label1.Text = "" Then
            Button1.Enabled = False
        Else
            Button1.Enabled = True
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        'Cursor = Cursors.WaitCursor
        'second = second + 1
        'If second >= 20 Then

        '    Timer2.Stop()

        '    Dim menu2 As Int64
        '    menu2 = GetSystemMenu(Me.Handle.ToInt32, choice.Enable)
        '    RemoveMenu(CInt(menu2), Position.rmclose, MF_POSITION)

        '    Panel1.Enabled = True
        '    Login.ShowDialog()
        '    Cursor = Cursors.Default
        'End If
    End Sub

    Public manage As Char
    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        manage = "Y"
        Panel1.Select()
        CompanyAdd.ShowDialog()
    End Sub

    Enum choice
        Disable = 0
        Enable = 1
    End Enum

    Enum Position
        rmclose = 6
    End Enum

    Structure xPoint
        Dim x, y As Int64
    End Structure

    Declare Function RemoveMenu Lib "user32" (ByVal menu As Int32, ByVal Pos As Int32, ByVal u As Int32) As Int32
    Declare Function GetSystemMenu Lib "user32" (ByVal hwnd As Int32, ByVal rseve As Int32) As Int32
    Declare Function WindowFromPoint Lib "user32" (ByVal x As Int64, ByVal y As Int64) As Int32
    Private Const MF_POSITION As Int32 = &H400
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        'If My.Application.Info.Version.ToString() < My.Settings.Version.ToString() Then
        '    MsgBox("You are trying to open a database created with a previous version of Point of Sale Technologies." & vbNewLine & "This database file may contain newer information than this version can support. It may not open or retrieve information correctly. We recommends that you upgrade to the latest version.", MsgBoxStyle.Exclamation, "Company Open")
        '    Label1.Select()
        'Else
        'If CheckBox1.Checked = True Then
        '    save()
        'End If


        Beep()
        Panel1.Select()
        Login.ShowDialog()
        'End If
    End Sub

    Private Sub Button2_Click_2(sender As Object, e As EventArgs) Handles Button2.Click
        DisableIcons()
        'STARTUP.Close()
        Panel1.Select()
        Close()
    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        manage = "N"
        Panel1.Select()
        CompanyAdd.ShowDialog()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Panel1.Select()
        ' newcompany.ShowDialog()
        'MsgBox(ListBox1.SelectedIndex)
    End Sub

    Private Sub Timer4_Tick(sender As Object, e As EventArgs) Handles Timer4.Tick
        'Login.ShowDialog()
    End Sub

    Dim first As String
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            My.Settings.CheckLast = True
            My.Settings.LastAccess = ListBox1.SelectedIndex
            My.Settings.Save()
        Else
            My.Settings.CheckLast = False
            My.Settings.LastAccess = ListBox1.Items.Item(0)
            My.Settings.Save()
        End If
    End Sub
End Class