Imports MySql.Data.MySqlClient
Public Class ScheduleEdit
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Select()
        Close()
    End Sub

    Private Sub ScheduleEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Select()
        Try
            cn.Open()
            Dim vat As String = "SELECT * FROM schedulelist WHERE schedule_num = '" & workerparyroll.DataGridView2.CurrentRow.Cells(0).Value & "'"
            cmd = New MySqlCommand(vat, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                dtpFrom.Text = dr.GetString("process_date").ToString
                ComboSchedule.Text = dr.GetString("schedule").ToString
                DateTimePicker1.Text = dr.GetString("check_date").ToString
            End While
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim sql As String
    Dim result As Integer
    Sub updateData()
        Try
            cn.Open()
            sql = "UPDATE schedulelist SET process_date=@process_date, status=@status, schedule=@schedule, check_date=@check_date WHERE schedule_num = '" & workerparyroll.DataGridView2.CurrentRow.Cells(0).Value & "'"
            cmd = New MySqlCommand
            With cmd
                .Connection = cn
                .CommandText = sql
                .Parameters.AddWithValue("@process_date", dtpFrom.Text)
                .Parameters.AddWithValue("@status", "Normal")
                .Parameters.AddWithValue("@schedule", ComboSchedule.Text)
                .Parameters.AddWithValue("@check_date", DateTimePicker1.Text)
                result = .ExecuteNonQuery()
            End With
            cn.Close()
            Close()
            Schedules()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim process_date As String
    Dim status As String
    Sub Schedules()
        Try

            cn.Open()
            Dim qry As String = "SELECT process_date FROM schedulelist"
            cmd = New MySqlCommand(qry, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                process_date = dr.GetString("process_date").ToString
            End While
            cn.Close()

            If CDate(process_date).ToShortDateString >= Today.Date.ToShortDateString Then
                status = "Normal"
            Else
                status = "Overdue"
            End If

            Dim dt1 As New DataTable
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT schedule_num As 'Number',process_date As 'Process Date',status As 'Status',schedule As 'Schedule',check_date As 'Check Date' FROM schedulelist"
            End With
            da.SelectCommand = cmd
            dt1.Clear()
            da.Fill(dt1)
            workerparyroll.DataGridView2.DataSource = dt1
            cn.Close()

            workerparyroll.DataGridView2.Columns(0).Width = 120
            workerparyroll.DataGridView2.Columns(1).Width = 120
            workerparyroll.DataGridView2.Columns(2).Width = 120
            workerparyroll.DataGridView2.Columns(3).Width = 120
            workerparyroll.DataGridView2.Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label1.Select()
        updateData()
    End Sub
End Class