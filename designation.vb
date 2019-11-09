Imports MySql.Data.MySqlClient
Public Class designation
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
                .CommandText = "SELECT Code As 'Code',Description FROM designation"
            End With

            da.SelectCommand = cmd
            dt.Clear()
            da.Fill(dt)
            DataGridView1.DataSource = dt
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer Category", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub SetFontAndColor()
        With DataGridView1.DefaultCellStyle
            .Font = New Font("Microsoft Sans Serif", 9)
            .ForeColor = Color.Black
            .SelectionForeColor = Color.White
            .SelectionBackColor = Color.Navy
        End With
    End Sub

    Private Sub designation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Panel1.Select()
        DataGridView1.RowTemplate.Height = 25
        Categories()
        SetFontAndColor()
        DataGridView1.Columns(0).Width = 100
        DataGridView1.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DataGridView1.Columns(0).ReadOnly = True
        DataGridView1.Columns(1).ReadOnly = False

        Dim i As Integer
        For i = 0 To DataGridView1.Columns.Count - 1
            DataGridView1.Columns.Item(i).SortMode = DataGridViewColumnSortMode.NotSortable
        Next i
    End Sub

    Sub save()
        Try
            cn.Open()
            For Each row As DataGridViewRow In DataGridView1.Rows
                With cmd
                    .Connection = cn
                    .CommandText = "UPDATE designation SET Description = '" & CStr(row.Cells(1).FormattedValue) & "' WHERE Code ='" & CStr(row.Cells(0).FormattedValue) & "'"
                    .ExecuteNonQuery()
                End With
            Next
            cn.Close()
            Categories()
            MessageBox.Show("Designation updated successfully", "Departments", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Departments", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub dataGridView1_DataBindingComplete(ByVal sender As Object, ByVal e As DataGridViewBindingCompleteEventArgs) Handles DataGridView1.DataBindingComplete
        Dim strikethrough_style As New DataGridViewCellStyle
        strikethrough_style.Font = New Font(DataGridView1.Font.Name, DataGridView1.Font.Size, FontStyle.Bold)
        strikethrough_style.ForeColor = Color.Crimson

        For Each row As DataGridViewRow In DataGridView1.Rows
            row.DefaultCellStyle = strikethrough_style
        Next
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
                    .CommandText = "UPDATE designation SET Description = '" & DataGridView1.CurrentRow.Cells(1).Value & "' WHERE Code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
                    .ExecuteNonQuery()
                End With
                cn.Close()
                Categories()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub designation_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
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
        Panel1.Select()

        For Each ChildForm As Form In MainInterface.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Panel1.Select()
        save()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Panel1.Select()
        deleteItem()
    End Sub
End Class