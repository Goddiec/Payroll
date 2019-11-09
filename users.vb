Imports System.IO
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text
Public Class users
    Dim cn1 As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter("select * from tbusers", cn1)
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager
    Dim READER As MySqlDataReader

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        GroupBox3.Select()
        Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DataGridView1.Select()
        DataGridView1.Rows(0).Selected = True
        UserEdit.ShowDialog()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        GroupBox3.Select()
        UserAcc.txtName.Select()
        'UserAcc.selectAllStart()
        UserAcc.ShowDialog()
    End Sub

    Dim TheUseID As String
    Dim numUsers As Char
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            cn1.Open()
            Dim userCount As String = "SELECT count(ID) FROM users"
            cmd = New MySqlCommand(userCount, cn1)
            dr = cmd.ExecuteReader
            While dr.Read
                numUsers = dr.GetString("count(ID)").ToString
            End While
            cn1.Close()

            If numUsers = "1" Then
                MsgBox("You may not delete the only user as this may result in a system lock-out.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Users")
                GroupBox3.Select()
            Else
                'If Login.USERID = DataGridView1.CurrentRow.Cells(0).Value Then
                '    MsgBox("User is currently in use. Please logout and login as a different use to delete!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Users")
                '    Label1.Select()
                'Else
                Dim dialog As New DialogResult
                dialog = MsgBox("Are you sure you want to delete this user?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Users")

                If dialog = DialogResult.No Then

                ElseIf dialog = DialogResult.Yes Then
                    cn1.Open()
                    Dim Query1 As String
                    Query1 = "DELETE FROM users WHERE Username = '" & DataGridView1.CurrentRow.Cells(1).Value & "'"
                    cmd = New MySqlCommand(Query1, cn1)
                    READER = cmd.ExecuteReader
                    cn1.Close()
                End If
                'End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn1.Dispose()
        End Try

        DataGridView1.Select()
        LoadData()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_MouseClick(sender As Object, e As MouseEventArgs) Handles DataGridView1.MouseClick
        GroupBox3.Select()
    End Sub

    Private Sub SetFontAndColor()
        With DataGridView1.DefaultCellStyle
            .Font = New Font("Microsoft Sans Serif", 9)
            .ForeColor = Color.Black
            .SelectionForeColor = Color.White
            .SelectionBackColor = Color.Navy
        End With
    End Sub

    Private Sub dataGridView1_DataBindingComplete(ByVal sender As Object, ByVal e As DataGridViewBindingCompleteEventArgs) Handles DataGridView1.DataBindingComplete
        Dim strikethrough_style As New DataGridViewCellStyle
        strikethrough_style.Font = New Font(DataGridView1.Font.Name, DataGridView1.Font.Size, FontStyle.Bold)
        strikethrough_style.ForeColor = Color.Crimson

        For Each row As DataGridViewRow In DataGridView1.Rows
            row.DefaultCellStyle = strikethrough_style
        Next
    End Sub

    Public Sub LoadData()
        Try
            Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
            SetFontAndColor()

            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT ID As 'User ID', user_name As 'Code',first_name As 'Name' FROM users"
            End With
            da.SelectCommand = cmd
            dt.Clear()
            da.Fill(dt)
            DataGridView1.DataSource = dt
            cn.Close()

            DataGridView1.Columns(0).Visible = False
            DataGridView1.Columns(1).Width = 100
            DataGridView1.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            '
        Catch ex As Exception

        End Try
    End Sub

    Private Sub UsersList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        DataGridView1.Select()
        LoadData()
        'DataGridView1.RowTemplate.Height = 35
    End Sub

    Private Sub users_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub
End Class