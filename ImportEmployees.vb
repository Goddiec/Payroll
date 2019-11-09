Imports MySql.Data.MySqlClient
Public Class ImportEmployees
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim da As New MySqlDataAdapter
    Dim READER As MySqlDataReader
    Dim dr As MySqlDataReader
    Dim MySqlCmd As New MySqlCommand

    Private Function saveData(sql As String)
        Dim mysqlCOn As MySqlConnection = New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
        Dim mysqlCmd As MySqlCommand
        Dim resul As Boolean

        Try
            mysqlCOn.Open()
            mysqlCmd = New MySqlCommand
            With mysqlCmd
                .Connection = mysqlCOn
                .CommandText = sql
                resul = .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            mysqlCOn.Close()
        End Try
        Return resul
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        With OpenFileDialog1
            .Filter = "Excel files(*.xlsx)|*.xlsx|All files (*.*)|*.*"
            .FilterIndex = 1
            .Title = "Import data from Excel file"
        End With
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            txtLocation.Text = OpenFileDialog1.FileName


        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If pg_load.Value = 100 Then
            Timer1.Stop()
            MsgBox("Success")
            pg_load.Value = 0
        Else
            pg_load.Value += 1
        End If
    End Sub

    Dim result As String
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim OLEcon As OleDb.OleDbConnection = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" & txtLocation.Text & " ; " & "Extended Properties=Excel 8.0;")
        Dim OLEcmd As New OleDb.OleDbCommand
        Dim OLEda As New OleDb.OleDbDataAdapter
        Dim OLEdt As New DataTable
        Dim sql As String
        Dim resul As Boolean

        'Try
        OLEcon.Open()
            With OLEcmd
                .Connection = OLEcon
                .CommandText = "select * from [Sheet1$]"
            End With
            OLEda.SelectCommand = OLEcmd
        OLEda.Fill(OLEdt)
        OLEcon.Close()
        For Each Drr As DataRow In OLEdt.Rows
            'For Each r As DataRow In OLEdt.Rows

            'sql = "INSERT INTO tblperson (FNAME,LNAME,ADDRESS) VALUES ('" & r(0).ToString & "','" & r(1).ToString & "','" & r(2).ToString & "')"

            cn.Close()
            cn.Open()
            Dim Query As String
            Query = "SELECT code FROM employee WHERE code = '" & Drr(0).ToString & "'"
                cmd = New MySqlCommand(Query, cn)
                READER = cmd.ExecuteReader
                If READER.HasRows = False Then
                    sql = "INSERT INTO employee (code, title, first_name, last_name, id_number, passport_num, initial, second_name, know_name, date_of_birth, passport_country, asylum_seeker, refugee, unit_num, complex_name, street_num, street_farm_name, suburb_district, city_town, post_code, country, default_phy_res_address, post_unit_num, post_complex_name, post_street_num, post_street_farm_name, post_suburb_district, post_city_town, post_postal_code, post_country, pay_with, acc_type, bank, branch_code, acc_num, acc_holder_name, other_bank, branch_name, account_holder_rel, start_date, start_as, department, working_h_day, working_d_week, avrg_working_h_month, avrg_working_d_month, annual_salary, fixed_salary, rate_per_day, rate_per_hour,employed,paybasis,designation,email,Caption,ImageFile)
                                VALUES (@code, @title, @first_name, @last_name, @id_number, @passport_num, @initial, @second_name, @know_name, @date_of_birth, @passport_country, @asylum_seeker, @refugee, @unit_num, @complex_name, @street_num, @street_farm_name, @suburb_district, @city_town, @post_code, @country, @default_phy_res_address, @post_unit_num, @post_complex_name, @post_street_num, @post_street_farm_name, @post_suburb_district, @post_city_town, @post_postal_code, @post_country, @pay_with, @acc_type, @bank, @branch_code, @acc_num, @acc_holder_name, @other_bank, @branch_name, @account_holder_rel, @start_date, @start_as, @department, @working_h_day, @working_d_week, @avrg_working_h_month, @avrg_working_d_month, @annual_salary, @fixed_salary, @rate_per_day, @rate_per_hour, @employed, @paybasis, @designation, @email, @Caption, @ImageFile)"
                    cmd = New MySqlCommand
                    With cmd
                        .Connection = cn
                        .CommandText = sql
                        .Parameters.AddWithValue("@code", Trim(Drr(0).ToString))
                        .Parameters.AddWithValue("@title", Trim(Drr(1).ToString))
                        .Parameters.AddWithValue("@first_name", Trim(Drr(2).ToString))
                        .Parameters.AddWithValue("@last_name", Trim(Drr(3).ToString))
                        .Parameters.AddWithValue("@id_number", Trim(Drr(4).ToString))
                        .Parameters.AddWithValue("@passport_num", Trim(Drr(5).ToString))
                        .Parameters.AddWithValue("@initial", Trim(Drr(6).ToString))
                        .Parameters.AddWithValue("@second_name", Trim(Drr(7).ToString))
                        .Parameters.AddWithValue("@know_name", Trim(Drr(8).ToString))
                        .Parameters.AddWithValue("@date_of_birth", Trim(Drr(9).ToString))
                        .Parameters.AddWithValue("@passport_country", Trim(Drr(10).ToString))
                        .Parameters.AddWithValue("@asylum_seeker", Trim(Drr(11).ToString))
                        .Parameters.AddWithValue("@refugee", Trim(Drr(12).ToString))
                        .Parameters.AddWithValue("@unit_num", Trim(Drr(13).ToString))
                        .Parameters.AddWithValue("@complex_name", Trim(Drr(14).ToString))
                        .Parameters.AddWithValue("@street_num", Trim(Drr(15).ToString))
                        .Parameters.AddWithValue("@street_farm_name", Trim(Drr(16).ToString))
                        .Parameters.AddWithValue("@suburb_district", Trim(Drr(17).ToString))
                        .Parameters.AddWithValue("@city_town", Trim(Drr(18).ToString))
                        .Parameters.AddWithValue("@post_code", Trim(Drr(19).ToString))
                        .Parameters.AddWithValue("@country", Trim(Drr(20).ToString))
                        .Parameters.AddWithValue("@default_phy_res_address", Trim(Drr(21).ToString))
                        .Parameters.AddWithValue("@post_unit_num", Trim(Drr(22).ToString))
                        .Parameters.AddWithValue("@post_complex_name", Trim(Drr(23).ToString))
                        .Parameters.AddWithValue("@post_street_num", Trim(Drr(24).ToString))
                        .Parameters.AddWithValue("@post_street_farm_name", Trim(Drr(25).ToString))
                        .Parameters.AddWithValue("@post_suburb_district", Trim(Drr(26).ToString))
                        .Parameters.AddWithValue("@post_city_town", Trim(Drr(27).ToString))
                        .Parameters.AddWithValue("@post_postal_code", Trim(Drr(28).ToString))
                        .Parameters.AddWithValue("@post_country", Trim(Drr(29).ToString))
                        .Parameters.AddWithValue("@pay_with", Trim(Drr(30).ToString))
                        .Parameters.AddWithValue("@acc_type", Trim(Drr(31).ToString))
                        .Parameters.AddWithValue("@bank", Trim(Drr(32).ToString))
                        .Parameters.AddWithValue("@branch_code", Trim(Drr(33).ToString))
                        .Parameters.AddWithValue("@acc_num", Trim(Drr(34).ToString))
                        .Parameters.AddWithValue("@acc_holder_name", Trim(Drr(35).ToString))
                        .Parameters.AddWithValue("@other_bank", Trim(Drr(36).ToString))
                        .Parameters.AddWithValue("@branch_name", Trim(Drr(37).ToString))
                        .Parameters.AddWithValue("@account_holder_rel", Trim(Drr(38).ToString))
                        .Parameters.AddWithValue("@start_date", Trim(Drr(39).ToString))
                        .Parameters.AddWithValue("@start_as", Trim(Drr(40).ToString))
                        .Parameters.AddWithValue("@end_date", Trim(Drr(41).ToString))
                        .Parameters.AddWithValue("@department", Trim(Drr(42).ToString))
                        .Parameters.AddWithValue("@working_h_day", CDec(Trim(Drr(43).ToString)))
                        .Parameters.AddWithValue("@working_d_week", CDec(Trim(Drr(44).ToString)))
                        .Parameters.AddWithValue("@avrg_working_h_month", CDec(Trim(Drr(45).ToString)))
                        .Parameters.AddWithValue("@avrg_working_d_month", CDec(Trim(Drr(46).ToString)))
                        .Parameters.AddWithValue("@annual_salary", CDec(Trim(Drr(47).ToString)))
                        .Parameters.AddWithValue("@fixed_salary", CDec(Trim(Drr(48).ToString)))
                        .Parameters.AddWithValue("@rate_per_day", CDec(Trim(Drr(49).ToString)))
                        .Parameters.AddWithValue("@rate_per_hour", CDec(Trim(Drr(50).ToString)))
                        .Parameters.AddWithValue("@employed", Trim(Drr(51).ToString))
                        .Parameters.AddWithValue("@paybasis", Trim(Drr(52).ToString))
                        .Parameters.AddWithValue("@designation", Trim(Drr(53).ToString))
                        .Parameters.AddWithValue("@email", Trim(Drr(54).ToString))
                        .Parameters.AddWithValue("@Caption", "pic.png")
                        .Parameters.AddWithValue("@ImageFile", My.Resources.pic)

                        result = .ExecuteNonQuery()
                    End With

                    If result > 0 Then
                        MessageBox.Show("Employee successfully created.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MsgBox("Error query", MsgBoxStyle.Exclamation)
                    End If
                Else
                    sql = "UPDATE employee SET employed=@employed,paybasis=@paybasis,designation=@designation,email=@email,title=@title, first_name=@first_name, last_name=@last_name, id_number=@id_number, passport_num=@passport_num, initial=@initial, second_name=@second_name, know_name=@know_name, date_of_birth=@date_of_birth, passport_country=@passport_country, asylum_seeker=@asylum_seeker, refugee=@refugee, unit_num=@unit_num, complex_name=@complex_name, street_num=@street_num, street_farm_name=@street_farm_name, suburb_district=@suburb_district, city_town=@city_town, post_code=@post_code, country=@country, default_phy_res_address=@default_phy_res_address, post_unit_num=@post_unit_num, post_complex_name=@post_complex_name, post_street_num=@post_street_num, post_street_farm_name=@post_street_farm_name, post_suburb_district=@post_suburb_district, post_city_town=@post_city_town, post_postal_code=@post_postal_code, post_country=@post_country, pay_with=@pay_with, acc_type=@acc_type, bank=@bank, branch_code=@branch_code, acc_num=@acc_num, acc_holder_name=@acc_holder_name, other_bank=@other_bank, branch_name=@branch_name, account_holder_rel=@account_holder_rel, start_date=@start_date, start_as=@start_as, department=@department, working_h_day=@working_h_day, working_d_week=@working_d_week, avrg_working_h_month=@avrg_working_h_month, avrg_working_d_month=@avrg_working_d_month, annual_salary=@annual_salary, fixed_salary=@fixed_salary, rate_per_day=@rate_per_day, rate_per_hour=@rate_per_hour WHERE  code='" & Trim(Drr(0).ToString) & "'"
                    cmd = New MySqlCommand
                    With cmd
                        .Connection = cn
                        .CommandText = sql
                        .Parameters.AddWithValue("@code", Trim(Drr(0).ToString))
                        .Parameters.AddWithValue("@title", Trim(Drr(1).ToString))
                        .Parameters.AddWithValue("@first_name", Trim(Drr(2).ToString))
                        .Parameters.AddWithValue("@last_name", Trim(Drr(3).ToString))
                        .Parameters.AddWithValue("@id_number", Trim(Drr(4).ToString))
                        .Parameters.AddWithValue("@passport_num", Trim(Drr(5).ToString))
                        .Parameters.AddWithValue("@initial", Trim(Drr(6).ToString))
                        .Parameters.AddWithValue("@second_name", Trim(Drr(7).ToString))
                        .Parameters.AddWithValue("@know_name", Trim(Drr(8).ToString))
                        .Parameters.AddWithValue("@date_of_birth", Trim(Drr(9).ToString))
                        .Parameters.AddWithValue("@passport_country", Trim(Drr(10).ToString))
                        .Parameters.AddWithValue("@asylum_seeker", Trim(Drr(11).ToString))
                        .Parameters.AddWithValue("@refugee", Trim(Drr(12).ToString))
                        .Parameters.AddWithValue("@unit_num", Trim(Drr(13).ToString))
                        .Parameters.AddWithValue("@complex_name", Trim(Drr(14).ToString))
                        .Parameters.AddWithValue("@street_num", Trim(Drr(15).ToString))
                        .Parameters.AddWithValue("@street_farm_name", Trim(Drr(16).ToString))
                        .Parameters.AddWithValue("@suburb_district", Trim(Drr(17).ToString))
                        .Parameters.AddWithValue("@city_town", Trim(Drr(18).ToString))
                        .Parameters.AddWithValue("@post_code", Trim(Drr(19).ToString))
                        .Parameters.AddWithValue("@country", Trim(Drr(20).ToString))
                        .Parameters.AddWithValue("@default_phy_res_address", Trim(Drr(21).ToString))
                        .Parameters.AddWithValue("@post_unit_num", Trim(Drr(22).ToString))
                        .Parameters.AddWithValue("@post_complex_name", Trim(Drr(23).ToString))
                        .Parameters.AddWithValue("@post_street_num", Trim(Drr(24).ToString))
                        .Parameters.AddWithValue("@post_street_farm_name", Trim(Drr(25).ToString))
                        .Parameters.AddWithValue("@post_suburb_district", Trim(Drr(26).ToString))
                        .Parameters.AddWithValue("@post_city_town", Trim(Drr(27).ToString))
                        .Parameters.AddWithValue("@post_postal_code", Trim(Drr(28).ToString))
                        .Parameters.AddWithValue("@post_country", Trim(Drr(29).ToString))
                        .Parameters.AddWithValue("@pay_with", Trim(Drr(30).ToString))
                        .Parameters.AddWithValue("@acc_type", Trim(Drr(31).ToString))
                        .Parameters.AddWithValue("@bank", Trim(Drr(32).ToString))
                        .Parameters.AddWithValue("@branch_code", Trim(Drr(33).ToString))
                        .Parameters.AddWithValue("@acc_num", Trim(Drr(34).ToString))
                        .Parameters.AddWithValue("@acc_holder_name", Trim(Drr(35).ToString))
                        .Parameters.AddWithValue("@other_bank", Trim(Drr(36).ToString))
                        .Parameters.AddWithValue("@branch_name", Trim(Drr(37).ToString))
                        .Parameters.AddWithValue("@account_holder_rel", Trim(Drr(38).ToString))
                        .Parameters.AddWithValue("@start_date", Trim(Drr(39).ToString))
                        .Parameters.AddWithValue("@start_as", Trim(Drr(40).ToString))
                        .Parameters.AddWithValue("@end_date", Trim(Drr(41).ToString))
                        .Parameters.AddWithValue("@department", Trim(Drr(42).ToString))
                        .Parameters.AddWithValue("@working_h_day", CDec(Trim(Drr(43).ToString)))
                        .Parameters.AddWithValue("@working_d_week", CDec(Trim(Drr(44).ToString)))
                        .Parameters.AddWithValue("@avrg_working_h_month", CDec(Trim(Drr(45).ToString)))
                        .Parameters.AddWithValue("@avrg_working_d_month", CDec(Trim(Drr(46).ToString)))
                        .Parameters.AddWithValue("@annual_salary", CDec(Trim(Drr(47).ToString)))
                        .Parameters.AddWithValue("@fixed_salary", CDec(Trim(Drr(48).ToString)))
                        .Parameters.AddWithValue("@rate_per_day", CDec(Trim(Drr(49).ToString)))
                        .Parameters.AddWithValue("@rate_per_hour", CDec(Trim(Drr(50).ToString)))
                        .Parameters.AddWithValue("@employed", Trim(Drr(51).ToString))
                        .Parameters.AddWithValue("@paybasis", Trim(Drr(52).ToString))
                        .Parameters.AddWithValue("@designation", Trim(Drr(53).ToString))
                        .Parameters.AddWithValue("@email", Trim(Drr(54).ToString))

                        result = .ExecuteNonQuery()
                    End With

                    If result > 0 Then
                        MessageBox.Show("Employee successfully updated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MsgBox("Error query", MsgBoxStyle.Exclamation)
                    End If
                End If
            cn.Close()
            resul = saveData(sql)
            If resul Then
                    Timer1.Start()
                End If
            Next
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message)
        'Finally
        '    OLEcon.Close()
        'End Try
    End Sub
End Class