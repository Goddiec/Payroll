﻿Imports MySql.Data.MySqlClient
Public Class banks
        Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
        Dim gender As String
        Dim COMMAND As MySqlCommand
        Dim cmd As New MySqlCommand
        Dim da As New MySqlDataAdapter
        Dim dt As New DataTable
        Dim dr As MySqlDataReader
        Dim ds As DataSet

        Sub Categories()
            Try
                cn.Open()
                With cmd
                    .Connection = cn
                .CommandText = "SELECT Code,Description,BranchCode As 'Branch Code' FROM bank"
            End With

                da.SelectCommand = cmd
                dt.Clear()
                da.Fill(dt)
                DataGridView1.DataSource = dt
                cn.Close()
            Catch ex As Exception
            MessageBox.Show(ex.Message, "Bank", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
                cn.Dispose()
            End Try
        End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Sub save()
        Try
            cn.Open()
            For Each row As DataGridViewRow In DataGridView1.Rows
                With cmd
                    .Connection = cn
                    .CommandText = "UPDATE bank SET Description = '" & CStr(row.Cells(1).FormattedValue) & "',BranchCode = '" & CStr(row.Cells(2).FormattedValue) & "' WHERE Code ='" & CStr(row.Cells(0).FormattedValue) & "'"
                    .ExecuteNonQuery()
                End With
            Next
            cn.Close()
            Categories()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Bank", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
        Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub SetFontAndColor()
        With DataGridView1.DefaultCellStyle
            .Font = New Font("Microsoft Sans Serif", 9)
            .ForeColor = Color.Black
            .SelectionForeColor = Color.White
            .SelectionBackColor = Color.Navy
        End With
    End Sub

    Private Sub CUSTOMERCAT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Panel2.Select()
        Categories()
        SetFontAndColor()
        DataGridView1.RowTemplate.Height = 25
        DataGridView1.Columns(0).Width = 50
        DataGridView1.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DataGridView1.Columns(2).Width = 150
        DataGridView1.Columns(0).ReadOnly = True
        DataGridView1.Columns(1).ReadOnly = False
        DataGridView1.Columns(2).ReadOnly = False
    End Sub

    Private Sub DataGridView1_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs)
        'If TypeOf e.Control Is TextBox Then
        '    DirectCast(e.Control, TextBox).CharacterCasing = CharacterCasing.Upper
        'End If
    End Sub

    Sub deleteItem()
        Dim dialog As New DialogResult

        dialog = MsgBox("Are you sure want to delete the current line?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Transaction")

        If dialog = DialogResult.No Then
            DialogResult.Cancel.ToString()
        Else
            Try
                DataGridView1.CurrentRow.Cells(1).Value = ""
                cn.Open()
                With cmd
                    .Connection = cn
                    .CommandText = "UPDATE departments SET Description = '" & DataGridView1.CurrentRow.Cells(1).Value & "',BranchCode = '" & DataGridView1.CurrentRow.Cells(2).Value & "' WHERE Code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
                    .ExecuteNonQuery()
                End With
                cn.Close()
                Categories()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Bank", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                cn.Dispose()
            End Try
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub CUSTOMERCAT_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
            If e.KeyCode = Keys.Escape Then
                Close()
            End If

            If e.KeyCode = Keys.Enter Then
                save()
            End If

            If e.KeyCode = Keys.Delete Then
                deleteItem()
            End If
        End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Close()
        Panel2.Select()

        For Each ChildForm As Form In MainInterface.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Panel2.Select()
        save()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Panel2.Select()
        deleteItem()
    End Sub
End Class