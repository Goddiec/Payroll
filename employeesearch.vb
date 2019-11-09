Imports System.IO
Imports MySql.Data.MySqlClient
Public Class employeesearch
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim da As New MySqlDataAdapter
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager

    Sub customer()
        Try
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT code As 'Code',title As 'Title',first_name As 'First Name',last_name As 'Last Name' FROM employee ORDER BY code"
            End With
            da.SelectCommand = cmd
            dt.Clear()
            da.Fill(dt)
            DataGridView1.DataSource = dt
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Dim asylum_seeker_val As Char
    Dim refugee_val As Char
    Dim default_phy_res_address_val As Char
    Private Sub LoadDB()
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT * FROM employee WHERE code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Employee.code.Text = dr("code").ToString()
                Employee.title.Text = dr("title").ToString()
                Employee.first_name.Text = dr("first_name").ToString()
                Employee.last_name.Text = dr("last_name").ToString()
                Employee.id_number.Text = dr("id_number").ToString()
                Employee.passport_num.Text = dr("passport_num").ToString()
                Employee.initial.Text = dr("initial").ToString()
                Employee.second_name.Text = dr("second_name").ToString()
                Employee.know_name.Text = dr("know_name").ToString()
                Employee.date_of_birth.Text = dr("date_of_birth").ToString()
                Employee.Combo_passport_country.Text = dr("passport_country").ToString()
                Employee.unit_num.Text = dr("unit_num").ToString()
                Employee.complex_name.Text = dr("complex_name").ToString()
                Employee.street_num.Text = dr("street_num").ToString()
                Employee.street_farm_name.Text = dr("street_farm_name").ToString()
                Employee.suburb_district.Text = dr("suburb_district").ToString()
                Employee.city_town.Text = dr("city_town").ToString()
                Employee.post_code.Text = dr("post_code").ToString()
                Employee.Combo_country.Text = dr("country").ToString()
                Employee.post_unit_num.Text = dr("post_unit_num").ToString()
                Employee.post_complex_name.Text = dr("post_complex_name").ToString()
                Employee.post_street_num.Text = dr("post_street_num").ToString()
                Employee.post_street_farm_name.Text = dr("post_street_farm_name").ToString()
                Employee.post_suburb_district.Text = dr("post_suburb_district").ToString()
                Employee.post_city_town.Text = dr("post_city_town").ToString()
                Employee.post_postal_code.Text = dr("post_postal_code").ToString()
                Employee.Combo_post_country.Text = dr("post_country").ToString()
                Employee.Combo_pay_with.Text = dr("pay_with").ToString()
                Employee.Combo_acc_type.Text = dr("acc_type").ToString()
                Employee.Combo_bank.Text = dr("bank").ToString()
                Employee.branch_code.Text = dr("branch_code").ToString()
                Employee.acc_num.Text = dr("acc_num").ToString()
                Employee.acc_holder_name.Text = dr("acc_holder_name").ToString()
                Employee.other_bank.Text = dr("other_bank").ToString()
                Employee.branch_name.Text = dr("branch_name").ToString()
                Employee.Combo_account_holder_rel.Text = dr("account_holder_rel").ToString()
                Employee.start_date.Text = dr("start_date").ToString()
                Employee.start_as.Text = dr("start_as").ToString()
                Employee.Combo_department.Text = dr("department").ToString()
                Employee.working_h_day.Text = FormatNumber(dr("working_h_day").ToString(), 2)
                Employee.working_d_week.Text = FormatNumber(dr("working_d_week").ToString(), 2)
                Employee.avrg_working_h_month.Text = FormatNumber(dr("avrg_working_h_month").ToString(), 2)
                Employee.avrg_working_d_month.Text = FormatNumber(dr("avrg_working_d_month").ToString(), 2)
                Employee.annual_salary.Text = FormatCurrency(dr("annual_salary").ToString(), 2)
                Employee.fixed_salary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                Employee.rate_per_day.Text = FormatCurrency(dr("rate_per_day").ToString(), 2)
                Employee.rate_per_hour.Text = FormatCurrency(dr("rate_per_hour").ToString(), 2)

                asylum_seeker_val = dr("asylum_seeker").ToString()
                refugee_val = dr("refugee").ToString()
                default_phy_res_address_val = dr("default_phy_res_address").ToString()

                If asylum_seeker_val = "Y" Then
                    Employee.Check_asylum_seeker.Checked = True
                Else
                    Employee.Check_asylum_seeker.Checked = False
                End If

                If refugee_val = "Y" Then
                    Employee.Check_refugee.Checked = True
                Else
                    Employee.Check_refugee.Checked = False
                End If

                If default_phy_res_address_val = "Y" Then
                    Employee.Check_default_phy_res_address.Checked = True
                Else
                    Employee.Check_default_phy_res_address.Checked = False
                End If
            End While
            cn.Close()
            Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        'DataGridView1.Select()
        'MsgBox("Test")

    End Sub

    Private Sub employeesearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.Select()
        Me.KeyPreview = True
        DataGridView1.RowTemplate.Height = 30
        customer()
        SetFontAndColor()
        DataGridView1.Columns(0).Width = 120
        DataGridView1.Columns(1).Width = 120
        DataGridView1.Columns(2).Width = 120
        DataGridView1.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
    End Sub

    Private Sub employeesearch_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadDB()
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        LoadDB()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)

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

                    Employee.PictureBox1.Image = Image.FromStream(ms, True)
                End Using
            End If
            cn.Close()
        Catch ex As Exception
            Employee.PictureBox1.Image = Nothing
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub btnViewRecord_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        LoadDB()
        imageRet()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        DataGridView1.Select()
        Close()
    End Sub
End Class