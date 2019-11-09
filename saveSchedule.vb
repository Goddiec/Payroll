Imports MySql.Data.MySqlClient
Public Class saveSchedule
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager

    Dim ms1 As String
    Dim ms2 As String
    Dim ms3 As String
    Dim ms4 As String
    Dim ms5 As String
    Dim ms6 As String
    Dim ms7 As String
    Dim ms8 As String
    Dim ms9 As String
    Dim ms10 As String
    Dim ms11 As String
    Dim ms12 As String
    Dim ms13 As String

    Dim me1 As String
    Dim me2 As String
    Dim me3 As String
    Dim me4 As String
    Dim me5 As String
    Dim me6 As String
    Dim me7 As String
    Dim me8 As String
    Dim me9 As String
    Dim me10 As String
    Dim me11 As String
    Dim me12 As String
    Dim me13 As String

    Dim md1 As String
    Dim md2 As String
    Dim md3 As String
    Dim md4 As String
    Dim md5 As String
    Dim md6 As String
    Dim md7 As String
    Dim md8 As String
    Dim md9 As String
    Dim md10 As String
    Dim md11 As String
    Dim md12 As String
    Dim md13 As String

    Dim codes As Int32
    Private Sub saveSchedule_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ComboSchedule.SelectedIndex = 2
            cn.Open()
            Dim qry As String = "SELECT schedule_num FROM schedulelist"
            cmd = New MySqlCommand(qry, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                codes = CInt(dr.GetString("schedule_num").ToString) + 1
            End While
            cn.Close()
            'MsgBox(codes.ToString("D3"))

            ms1 = DateSerial(Today.Year, Month:=1, Day:=1).ToString("yyyy/MM/dd")
            me1 = DateSerial(Today.Year, Month:=2, Day:=0).ToString("yyyy/MM/dd")
            md1 = MonthName(1)

            ms2 = DateSerial(Today.Year, Month:=2, Day:=1).ToString("yyyy/MM/dd")
            me2 = DateSerial(Today.Year, Month:=3, Day:=0).ToString("yyyy/MM/dd")
            md2 = MonthName(2)

            ms3 = DateSerial(Today.Year, Month:=3, Day:=1).ToString("yyyy/MM/dd")
            me3 = DateSerial(Today.Year, Month:=4, Day:=0).ToString("yyyy/MM/dd")
            md3 = MonthName(3)

            ms4 = DateSerial(Today.Year, Month:=4, Day:=1).ToString("yyyy/MM/dd")
            me4 = DateSerial(Today.Year, Month:=5, Day:=0).ToString("yyyy/MM/dd")
            md4 = MonthName(4)

            ms5 = DateSerial(Today.Year, Month:=5, Day:=1).ToString("yyyy/MM/dd")
            me5 = DateSerial(Today.Year, Month:=6, Day:=0).ToString("yyyy/MM/dd")
            md5 = MonthName(5)

            ms6 = DateSerial(Today.Year, Month:=6, Day:=1).ToString("yyyy/MM/dd")
            me6 = DateSerial(Today.Year, Month:=7, Day:=0).ToString("yyyy/MM/dd")
            md6 = MonthName(6)

            ms7 = DateSerial(Today.Year, Month:=7, Day:=1).ToString("yyyy/MM/dd")
            me7 = DateSerial(Today.Year, Month:=8, Day:=0).ToString("yyyy/MM/dd")
            md7 = MonthName(7)

            ms8 = DateSerial(Today.Year, Month:=8, Day:=1).ToString("yyyy/MM/dd")
            me8 = DateSerial(Today.Year, Month:=9, Day:=0).ToString("yyyy/MM/dd")
            md8 = MonthName(8)

            ms9 = DateSerial(Today.Year, Month:=9, Day:=1).ToString("yyyy/MM/dd")
            me9 = DateSerial(Today.Year, Month:=10, Day:=0).ToString("yyyy/MM/dd")
            md9 = MonthName(9)

            ms10 = DateSerial(Today.Year, Month:=10, Day:=1).ToString("yyyy/MM/dd")
            me10 = DateSerial(Today.Year, Month:=11, Day:=0).ToString("yyyy/MM/dd")
            md10 = MonthName(10)

            ms11 = DateSerial(Today.Year, Month:=11, Day:=1).ToString("yyyy/MM/dd")
            me11 = DateSerial(Today.Year, Month:=12, Day:=0).ToString("yyyy/MM/dd")
            md11 = MonthName(11)

            ms12 = DateSerial(Today.Year, Month:=12, Day:=1).ToString("yyyy/MM/dd")
            me12 = DateSerial(Today.Year, Month:=13, Day:=0).ToString("yyyy/MM/dd")
            md12 = MonthName(12)

            'Combo_pay_period.Items.Add(md1 & " " & ms1 & " - " & me1)
            'Combo_pay_period.Items.Add(md2 & " " & ms2 & " - " & me2)
            'Combo_pay_period.Items.Add(md3 & " " & ms3 & " - " & me3)
            'Combo_pay_period.Items.Add(md4 & " " & ms4 & " - " & me4)
            'Combo_pay_period.Items.Add(md5 & " " & ms5 & " - " & me5)
            'Combo_pay_period.Items.Add(md6 & " " & ms6 & " - " & me6)
            'Combo_pay_period.Items.Add(md7 & " " & ms7 & " - " & me7)
            'Combo_pay_period.Items.Add(md8 & " " & ms8 & " - " & me8)
            'Combo_pay_period.Items.Add(md9 & " " & ms9 & " - " & me9)
            'Combo_pay_period.Items.Add(md10 & " " & ms10 & " - " & me10)
            'Combo_pay_period.Items.Add(md12 & " " & ms12 & " - " & me12)
            'Combo_pay_period.Items.Add(md13 & " " & ms13 & " - " & me13)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label33.Select()
        Close()
    End Sub

    Dim sql As String
    Dim result As Integer
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label33.Select()
        Try
            cn.Open()
            sql = "INSERT INTO schedulelist (schedule_num, process_date, status, schedule, check_date)
                                VALUES (@schedule_num, @process_date, @status, @schedule, @check_date)"
            cmd = New MySqlCommand
            With cmd
                .Connection = cn
                .CommandText = sql
                .Parameters.AddWithValue("@schedule_num", codes.ToString("D3"))
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

End Class