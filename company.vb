Imports System.IO
Imports System.Runtime.InteropServices
Imports MySql.Data.MySqlClient
Public Class company
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager

    Dim sars As String
    Dim default_phy_address_val As Char
    Dim default_sars_cont_person_val As Char

    <DllImport("Gdi32.dll", EntryPoint:="CreateRoundRectRgn")>
    Private Shared Function CreateRoundRectRgn(ByVal iLeft As Integer, ByVal iTop As Integer, ByVal iRight As Integer, ByVal iBottom As Integer, ByVal iWidth As Integer, ByVal iHeight As Integer) As IntPtr
    End Function

    Dim Calc As Char
    Dim UseTax1 As Char
    Dim UseTax2 As Char
    Dim UseTax3 As Char
    Dim UseTax4 As Char
    Dim UseTax5 As Char
    Dim UseTax6 As Char
    Dim UseTax7 As Char
    Dim UseTax8 As Char
    Dim UseTax9 As Char
    Dim UseTax10 As Char
    Private Sub LoadDB()
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT * FROM company WHERE ID = 1"
            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                com_num.Text = dr("com_num").ToString()
                com_name.Text = dr("com_name").ToString()
                com_reg_num.Text = dr("com_reg_num").ToString()
                sdl_pay_ref_num.Text = dr("sdl_pay_ref_num").ToString()
                uif_pay_ref_num.Text = dr("uif_pay_ref_num").ToString()
                uif_reg_num.Text = dr("uif_reg_num").ToString()
                Combo_standard_industry_class.Text = dr("standard_industry_class").ToString()
                phy_unit_num.Text = dr("phy_unit_num").ToString()
                phy_complex_num.Text = dr("phy_complex_num").ToString()
                phy_street_num.Text = dr("phy_street_num").ToString()
                phy_street_farm_name.Text = dr("phy_street_farm_name").ToString()
                phy_suburb_district.Text = dr("phy_suburb_district").ToString()
                phy_city_town.Text = dr("phy_city_town").ToString()
                phy_postal_code.Text = dr("phy_postal_code").ToString()
                default_phy_address_val = dr("default_phy_address").ToString()
                post_address1.Text = dr("post_address1").ToString()
                post_address2.Text = dr("post_address2").ToString()
                post_address3.Text = dr("post_address3").ToString()
                post_postal_code.Text = dr("post_postal_code").ToString()
                sars_contact_person.Text = dr("sars_contact_person").ToString()
                sars_email_address.Text = dr("sars_email_address").ToString()
                sars_tel.Text = dr("sars_tel").ToString()
                uif_contact_person.Text = dr("uif_contact_person").ToString()
                uif_email_address.Text = dr("uif_email_address").ToString()
                uif_tel.Text = dr("uif_tel").ToString()
                default_sars_cont_person_val = dr("default_sars_cont_person").ToString()
                revenue_autho.Text = dr("revenue_autho").ToString()
                Combo_phy_country.Text = dr("phy_country").ToString()
                sars = dr("revenue_autho").ToString()

                Label4.Text = "Tax Number(" & sars.ToUpper & ")"
                Label5.Text = "TTP Number(" & sars.ToUpper & ")"
                Label29.Text = sars.ToUpper & " Contact Person Detail"
                'Label18
                If default_phy_address_val = "Y" Then
                    Check_default_phy_address.Checked = True
                Else
                    Check_default_phy_address.Checked = False
                End If

                If default_sars_cont_person_val = "Y" Then
                    Check_default_sars_cont_person.Checked = True
                Else
                    Check_default_sars_cont_person.Checked = False
                End If

            End While
            cn.Close()

            cn.Open()
            Dim Query2 As String
            Query2 = "SELECT * FROM parameters WHERE ID = 1"
            cmd = New MySqlCommand(Query2, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtnapsa.Text = FormatCurrency(dr("napsa").ToString(), 2)
                txtnapsaper.Text = FormatNumber(dr("napsaper").ToString(), 2)
                txtuif.Text = FormatNumber(dr("uif").ToString(), 2)
                txtOTRate.Text = FormatCurrency(dr("overtime_per_hour").ToString(), 2)
                Calc = dr("Calc").ToString()
            End While

            If Calc = "A" Then
                Radio1.Checked = True
            ElseIf Calc = "M" Then
                Radio2.Checked = True
            ElseIf Calc = "D" Then
                Radio3.Checked = True
            ElseIf Calc = "H" Then
                Radio4.Checked = True
            End If
            cn.Close()

            cn.Open()
            Dim Q1 As String
            Q1 = "SELECT * FROM tax"
            cmd = New MySqlCommand(Q1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtTaxP1.Text = FormatNumber(dr("taxValue1").ToString(), 2)
                txtV1.Text = FormatCurrency(dr("taxGross1").ToString(), 2)
                txtTaxP2.Text = FormatNumber(dr("taxValue2").ToString(), 2)
                txtV2.Text = FormatCurrency(dr("taxGross2").ToString(), 2)
                txtTaxP3.Text = FormatNumber(dr("taxValue3").ToString(), 2)
                txtV3.Text = FormatCurrency(dr("taxGross3").ToString(), 2)
                txtTaxP4.Text = FormatNumber(dr("taxValue4").ToString(), 2)
                txtV4.Text = FormatCurrency(dr("taxGross4").ToString(), 2)
                txtTaxP5.Text = FormatNumber(dr("taxValue5").ToString(), 2)
                txtV5.Text = FormatCurrency(dr("taxGross5").ToString(), 2)
                txtTaxP6.Text = FormatNumber(dr("taxValue6").ToString(), 2)
                txtV6.Text = FormatCurrency(dr("taxGross6").ToString(), 2)
                txtTaxP7.Text = FormatNumber(dr("taxValue7").ToString(), 2)
                txtV7.Text = FormatCurrency(dr("taxGross7").ToString(), 2)
                txtTaxP8.Text = FormatNumber(dr("taxValue8").ToString(), 2)
                txtV8.Text = FormatCurrency(dr("taxGross8").ToString(), 2)
                txtTaxP9.Text = FormatNumber(dr("taxValue9").ToString(), 2)
                txtV9.Text = FormatCurrency(dr("taxGross9").ToString(), 2)
                txtTaxP10.Text = FormatNumber(dr("taxValue10").ToString(), 2)
                txtV10.Text = FormatCurrency(dr("taxGross10").ToString(), 2)

                UseTax1 = dr("use1").ToString()
                UseTax2 = dr("use2").ToString()
                UseTax3 = dr("use3").ToString()
                UseTax4 = dr("use4").ToString()
                UseTax5 = dr("use5").ToString()
                UseTax6 = dr("use6").ToString()
                UseTax7 = dr("use7").ToString()
                UseTax8 = dr("use8").ToString()
                UseTax9 = dr("use9").ToString()
                UseTax10 = dr("use10").ToString()
            End While
            cn.Close()

            If UseTax1 = "Y" Then
                CheckBox01.Checked = True
            Else
                CheckBox01.Checked = False
            End If

            If UseTax2 = "Y" Then
                CheckBox02.Checked = True
            Else
                CheckBox02.Checked = False
            End If

            If UseTax3 = "Y" Then
                CheckBox03.Checked = True
            Else
                CheckBox03.Checked = False
            End If

            If UseTax4 = "Y" Then
                CheckBox04.Checked = True
            Else
                CheckBox04.Checked = False
            End If

            If UseTax5 = "Y" Then
                CheckBox05.Checked = True
            Else
                CheckBox05.Checked = False
            End If

            If UseTax6 = "Y" Then
                CheckBox06.Checked = True
            Else
                CheckBox06.Checked = False
            End If

            If UseTax7 = "Y" Then
                CheckBox07.Checked = True
            Else
                CheckBox07.Checked = False
            End If

            If UseTax8 = "Y" Then
                CheckBox08.Checked = True
            Else
                CheckBox08.Checked = False
            End If

            If UseTax9 = "Y" Then
                CheckBox09.Checked = True
            Else
                CheckBox09.Checked = False
            End If

            If UseTax10 = "Y" Then
                CheckBox10.Checked = True
            Else
                CheckBox10.Checked = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Company", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Dim default_phy_address As Char
    Dim default_sars_cont_person As Char
    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Sub taxVal()
        'Dim dt1 As New DataTable
        'cn.Open()
        'With cmd
        '    .Connection = cn
        '    .CommandText = "SELECT code As 'Tax Code', value As 'Value', grosspaytaxable As 'Taxable' FROM tax"
        'End With
        'da.SelectCommand = cmd
        'dt1.Clear()
        'da.Fill(dt1)
        'DataGridView1.DataSource = dt1
        'cn.Close()

        'DataGridView1.Columns(0).Width = 100
        'DataGridView1.Columns(1).Width = 100
        'DataGridView1.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
    End Sub

    Private Sub imageRet()
        Try
            cn.Open()
            Dim cmd1 As MySqlCommand
            cmd1 = New MySqlCommand("Select ImageFile from company", cn)
            Dim imageData As Byte() = DirectCast(cmd1.ExecuteScalar(), Byte())

            If Not imageData Is Nothing Then
                Using ms As New MemoryStream(imageData, 0, imageData.Length)
                    ms.Write(imageData, 0, imageData.Length)

                    PictureBox1.Image = Image.FromStream(ms, True)
                End Using
            End If
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Company", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim phy_country As String
    Sub LoadFill()
        Try
            cn.Open()
            Dim Query2 As String
            Query2 = "SELECT phy_country FROM company WHERE ID = 1"
            cmd = New MySqlCommand(Query2, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                phy_country = dr("phy_country").ToString()
            End While
            cn.Close()

            If phy_country = "Rwanda" Then
                Label37.Text = ""
                Label36.Text = ""
                Label37.Text = "SSFR"
                Label36.Text = "SSFR %"
            ElseIf phy_country = "Zambia" Then
                Label37.Text = "NAPSA"
                Label36.Text = "NAPSA %"
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Company", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub company_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        LoadDB()
        Panel1.Select()
        LoadFill()

        Label65.AutoSize = False
        Label65.Padding = New Padding(1, 1, 1, 1)
        Label65.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label65.Width - 2, Label65.Height - 2, 5, 1))

        Label67.AutoSize = False
        Label67.Padding = New Padding(1, 1, 1, 1)
        Label67.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label67.Width - 2, Label67.Height - 2, 5, 1))

        Label69.AutoSize = False
        Label69.Padding = New Padding(1, 1, 1, 1)
        Label69.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label69.Width - 2, Label69.Height - 2, 5, 1))

        Label68.AutoSize = False
        Label68.Padding = New Padding(1, 1, 1, 1)
        Label68.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label68.Width - 2, Label68.Height - 2, 5, 1))

        Label66.AutoSize = False
        Label66.Padding = New Padding(1, 1, 1, 1)
        Label66.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label66.Width - 2, Label66.Height - 2, 5, 1))

        Label65.BackColor = Color.Red
        Label67.BackColor = Color.Lime
        Label69.BackColor = Color.Lime
        Label68.BackColor = Color.Lime

        Panel2.Visible = True
        Panel4.Visible = False
        Panel5.Visible = False

        Panel2.Location = New Point(0, 41)
        Panel2.Dock = DockStyle.Fill

        taxVal()

        LoadTax()
        imageRet()

        Panel1.Select()
    End Sub

    Sub LoadTax()
        '1
        Label65.BackColor = Color.Red
        Label67.BackColor = Color.Lime
        Label69.BackColor = Color.Lime
        Label68.BackColor = Color.Lime

        Panel2.Visible = True
        Panel4.Visible = False
        Panel5.Visible = False
        Panel1.Visible = False

        Panel2.Location = New Point(0, 41)
        Panel2.Dock = DockStyle.Fill

        '2
        Label65.BackColor = Color.Lime
        Label67.BackColor = Color.Red
        Label69.BackColor = Color.Lime
        Label68.BackColor = Color.Lime

        Panel2.Visible = False
        Panel4.Visible = True
        Panel5.Visible = False
        Panel1.Visible = False

        Panel4.Location = New Point(0, 41)
        Panel4.Dock = DockStyle.Fill

        '3
        Label65.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label69.BackColor = Color.Red
        Label68.BackColor = Color.Lime

        Panel2.Visible = False
        Panel4.Visible = False
        Panel5.Visible = True
        Panel1.Visible = False

        Panel5.Location = New Point(0, 41)
        Panel5.Dock = DockStyle.Fill

        '4
        Label65.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label69.BackColor = Color.Lime
        Label68.BackColor = Color.Red

        Panel2.Visible = False
        Panel4.Visible = False
        Panel5.Visible = False
        Panel1.Visible = True

        Panel1.Location = New Point(0, 41)
        Panel1.Dock = DockStyle.Fill

        '1
        Label65.BackColor = Color.Red
        Label67.BackColor = Color.Lime
        Label69.BackColor = Color.Lime
        Label68.BackColor = Color.Lime

        Panel2.Visible = True
        Panel4.Visible = False
        Panel5.Visible = False
        Panel1.Visible = False

        Panel2.Location = New Point(0, 41)
        Panel2.Dock = DockStyle.Fill
    End Sub

    Private Sub Check_default_sars_cont_person_CheckedChanged(sender As Object, e As EventArgs)
        If Check_default_sars_cont_person.Checked = True Then
            uif_contact_person.Text = sars_contact_person.Text
            uif_email_address.Text = sars_email_address.Text
            uif_tel.Text = sars_tel.Text
        Else
            uif_contact_person.Clear()
            uif_email_address.Clear()
            uif_tel.Clear()
        End If
    End Sub

    Private Sub Check_default_phy_address_CheckedChanged(sender As Object, e As EventArgs)
        If Check_default_phy_address.Checked = True Then
            post_address1.Text = phy_unit_num.Text & " " & phy_complex_num.Text
            post_address2.Text = phy_street_num.Text
            post_address3.Text = phy_street_farm_name.Text & " " & phy_suburb_district.Text & " " & phy_city_town.Text
            post_postal_code.Text = phy_postal_code.Text
        Else
            post_address1.Clear()
            post_address2.Clear()
            post_address3.Clear()
            post_postal_code.Clear()
        End If
    End Sub

    Public Sub search()
        Dim MysqlConn As MySqlConnection
        Dim COMMAND As MySqlCommand

        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString = "server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";"
        Dim READER As MySqlDataReader

        MysqlConn.Open()
        Dim Query As String
        Query = "SELECT com_name FROM company WHERE ID = '1'"
        Command = New MySqlCommand(Query, MysqlConn)
        READER = Command.ExecuteReader
        While READER.Read
            MainInterface.Text = "Payroll[" & READER.GetString("com_name") & "]"
        End While
        MysqlConn.Close()
    End Sub

    Sub changeMan()
        Label65.BackColor = Color.Lime
        Label67.BackColor = Color.Red
        Label69.BackColor = Color.Lime
        Label68.BackColor = Color.Lime

        Panel2.Visible = False
        Panel4.Visible = True
        Panel5.Visible = False
        Panel1.Visible = False

        Panel4.Location = New Point(0, 41)
        Panel4.Dock = DockStyle.Fill
    End Sub

    Sub changeMan1()
        Label65.BackColor = Color.Red
        Label67.BackColor = Color.Lime
        Label69.BackColor = Color.Lime
        Label68.BackColor = Color.Lime

        Panel2.Visible = True
        Panel4.Visible = False
        Panel5.Visible = False
        Panel1.Visible = False

        Panel2.Location = New Point(0, 41)
        Panel2.Dock = DockStyle.Fill
    End Sub

    Dim CalcValue As Char
    Dim check1 As Char
    Dim check2 As Char
    Dim check3 As Char
    Dim check4 As Char
    Dim check5 As Char
    Dim check6 As Char
    Dim check7 As Char
    Dim check8 As Char
    Dim check9 As Char
    Dim check10 As Char
    Dim caption As String
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Panel1.Select()
        If com_num.Text = String.Empty Then
            changeMan1()
            MessageBox.Show("Please enter your company registration number.", "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            com_num.Select()
        ElseIf com_name.Text = String.Empty Then
            changeMan1()
            MessageBox.Show("Please enter your company name.", "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            com_name.Select()
        ElseIf Combo_standard_industry_class.SelectedIndex = 0 Then
            changeMan1()
            MessageBox.Show("Please select your company Industrial Classification.", "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Combo_standard_industry_class.Select()
            Combo_standard_industry_class.Focus()
        ElseIf phy_unit_num.Text = String.Empty Then
            changeMan()
            MessageBox.Show("Please enter your Physical Address.", "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            phy_unit_num.Select()
        ElseIf phy_complex_num.Text = String.Empty Then
            changeMan()
            MessageBox.Show("Please enter your Physical Address.", "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            phy_complex_num.Select()
        ElseIf phy_street_num.Text = String.Empty Then
            changeMan()
            MessageBox.Show("Please enter your Physical Address.", "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            phy_street_num.Select()
        ElseIf phy_city_town.Text = String.Empty Then
            changeMan()
            MessageBox.Show("Please enter your Physical Address.", "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            phy_city_town.Select()
        ElseIf phy_postal_code.Text = String.Empty Then
            changeMan()
            MessageBox.Show("Please enter your Physical Address.", "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            phy_postal_code.Select()
        ElseIf Combo_phy_country.Text = String.Empty Then
            changeMan()
            MessageBox.Show("Please enter your Physical Address.", "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Combo_phy_country.Select()
            Combo_phy_country.Focus()
        Else
            SavaData()
        End If
    End Sub

    Sub SavaData()
        Label1.Select()
        caption = System.IO.Path.GetFileName(OpenFileDialog1.FileName)

        Try
            If CheckBox01.Checked = True Then
                check1 = "Y"
            Else
                check1 = "N"
            End If

            If CheckBox02.Checked = True Then
                check2 = "Y"
            Else
                check2 = "N"
            End If

            If CheckBox03.Checked = True Then
                check3 = "Y"
            Else
                check3 = "N"
            End If

            If CheckBox04.Checked = True Then
                check4 = "Y"
            Else
                check4 = "N"
            End If

            If CheckBox05.Checked = True Then
                check5 = "Y"
            Else
                check5 = "N"
            End If

            If CheckBox06.Checked = True Then
                check6 = "Y"
            Else
                check6 = "N"
            End If

            If CheckBox07.Checked = True Then
                check7 = "Y"
            Else
                check7 = "N"
            End If

            If CheckBox08.Checked = True Then
                check8 = "Y"
            Else
                check8 = "N"
            End If

            If CheckBox09.Checked = True Then
                check9 = "Y"
            Else
                check9 = "N"
            End If

            If CheckBox10.Checked = True Then
                check10 = "Y"
            Else
                check10 = "N"
            End If

            If Check_default_phy_address.Checked = True Then
                default_phy_address = "Y"
            Else
                default_phy_address = "N"
            End If

            If Check_default_phy_address.Checked = True Then
                default_sars_cont_person = "Y"
            Else
                default_sars_cont_person = "N"
            End If

            If Radio1.Checked = True Then
                CalcValue = "A"
            ElseIf Radio2.Checked = True Then
                CalcValue = "M"
            ElseIf Radio3.Checked = True Then
                CalcValue = "D"
            ElseIf Radio4.Checked = True Then
                CalcValue = "H"
            End If

            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "UPDATE company SET Caption = '" & caption & "',com_num='" & com_num.Text & "', com_name='" & com_name.Text & "', com_reg_num='" & com_reg_num.Text & "', sdl_pay_ref_num='" & sdl_pay_ref_num.Text & "', uif_pay_ref_num='" & uif_pay_ref_num.Text & "', uif_reg_num='" & uif_reg_num.Text & "', standard_industry_class='" & Combo_standard_industry_class.Text & "', phy_unit_num='" & phy_unit_num.Text & "', phy_complex_num='" & phy_complex_num.Text & "', phy_street_num='" & phy_street_num.Text & "', phy_street_farm_name='" & phy_street_farm_name.Text & "', phy_suburb_district='" & phy_suburb_district.Text & "', phy_city_town='" & phy_city_town.Text & "', phy_postal_code= '" & phy_postal_code.Text & "', phy_country='" & Combo_phy_country.Text & "', default_phy_address= '" & default_phy_address & "', post_address1='" & post_address1.Text & "',post_address2='" & post_address2.Text & "', post_address3='" & post_address3.Text & "', post_postal_code= '" & post_postal_code.Text & "', sars_contact_person='" & sars_contact_person.Text & "', sars_email_address='" & sars_email_address.Text & "', sars_tel='" & sars_tel.Text & "', uif_contact_person='" & uif_contact_person.Text & "', uif_email_address='" & uif_email_address.Text & "', uif_tel= '" & uif_tel.Text & "',default_sars_cont_person = '" & default_sars_cont_person & "',revenue_autho = '" & revenue_autho.Text & "' WHERE id = 1"
            dr = cmd.ExecuteReader
            cn.Close()

            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "UPDATE parameters SET Calc = '" & CalcValue & "',overtime_per_hour='" & CDec(txtOTRate.Text) & "', uif='" & CDec(txtuif.Text) & "',napsa='" & CDec(txtnapsa.Text) & "', napsaper='" & CDec(txtnapsaper.Text) & "' WHERE id = 1"
            dr = cmd.ExecuteReader
            cn.Close()

            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "UPDATE tax SET taxValue1='" & CDec(txtTaxP1.Text) & "', taxValue2='" & CDec(txtTaxP2.Text) & "',taxValue3='" & CDec(txtTaxP3.Text) & "', taxValue4='" & CDec(txtTaxP4.Text) & "',taxValue5='" & CDec(txtTaxP5.Text) & "',taxValue6='" & CDec(txtTaxP6.Text) & "',taxValue7='" & CDec(txtTaxP7.Text) & "',taxValue8='" & CDec(txtTaxP8.Text) & "',taxValue9='" & CDec(txtTaxP9.Text) & "',taxValue10='" & CDec(txtTaxP10.Text) & "',taxGross1='" & CDec(txtV1.Text) & "',taxGross2='" & CDec(txtV2.Text) & "',taxGross3='" & CDec(txtV3.Text) & "',taxGross4='" & CDec(txtV4.Text) & "',taxGross5='" & CDec(txtV5.Text) & "',taxGross6='" & CDec(txtV6.Text) & "',taxGross7='" & CDec(txtV7.Text) & "',taxGross8='" & CDec(txtV8.Text) & "',taxGross9='" & CDec(txtV9.Text) & "',taxGross10='" & CDec(txtV10.Text) & "',use1='" & check1 & "',use2='" & check2 & "',use3='" & check3 & "',use4='" & check4 & "',use5='" & check5 & "',use6='" & check6 & "',use7='" & check7 & "',use8='" & check8 & "',use9='" & check9 & "',use10='" & check10 & "' WHERE id = 1"
            dr = cmd.ExecuteReader
            cn.Close()

            Dim myAdapter As New MySqlDataAdapter
            Dim sqlquery = "SELECT * FROM company WHERE ID= '1'"
            Dim myCommand As New MySqlCommand()
            myCommand.Connection = cn
            myCommand.CommandText = sqlquery
            myAdapter.SelectCommand = myCommand
            cn.Open()
            Dim ms As New MemoryStream

            Dim bm As Bitmap = New Bitmap(PictureBox1.Image)
            bm.Save(ms, PictureBox1.Image.RawFormat)

            Dim arrPic() As Byte = ms.GetBuffer()

            sqlquery = "UPDATE company SET ImageFile=@ImageFile WHERE ID = '1'"

            myCommand = New MySqlCommand(sqlquery, cn)
            myCommand.Parameters.AddWithValue("@ImageFile", arrPic)
            myCommand.ExecuteNonQuery()
            cn.Close()

            MessageBox.Show("Company data updated.", "System", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Label1.Select()
            search()

            Label65.BackColor = Color.Red
            Label67.BackColor = Color.Lime
            Label69.BackColor = Color.Lime
            Label68.BackColor = Color.Lime

            Panel2.Visible = True
            Panel4.Visible = False
            Panel5.Visible = False
            Panel1.Visible = False

            Panel2.Location = New Point(0, 41)
            Panel2.Dock = DockStyle.Fill

            txtTaxP1.Text = FormatNumber(txtTaxP1.Text, 2)
            txtV1.Text = FormatCurrency(txtV1.Text, 2)
            txtTaxP2.Text = FormatNumber(txtTaxP2.Text, 2)
            txtV2.Text = FormatCurrency(txtV2.Text, 2)
            txtTaxP3.Text = FormatNumber(txtTaxP3.Text, 2)
            txtV3.Text = FormatCurrency(txtV3.Text, 2)
            txtTaxP4.Text = FormatNumber(txtTaxP4.Text, 2)
            txtV4.Text = FormatCurrency(txtV4.Text, 2)
            txtTaxP5.Text = FormatNumber(txtTaxP5.Text, 2)
            txtV5.Text = FormatCurrency(txtV5.Text, 2)
            txtTaxP6.Text = FormatNumber(txtTaxP6.Text, 2)
            txtV6.Text = FormatCurrency(txtV6.Text, 2)
            txtTaxP7.Text = FormatNumber(txtTaxP7.Text, 2)
            txtV7.Text = FormatCurrency(txtV7.Text, 2)
            txtTaxP8.Text = FormatNumber(txtTaxP8.Text, 2)
            txtV8.Text = FormatCurrency(txtV8.Text, 2)
            txtTaxP9.Text = FormatNumber(txtTaxP9.Text, 2)
            txtV9.Text = FormatCurrency(txtV9.Text, 2)
            txtTaxP10.Text = FormatNumber(txtTaxP10.Text, 2)
            txtV10.Text = FormatCurrency(txtV10.Text, 2)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label1.Select()
        Close()

        For Each ChildForm As Form In MainInterface.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub

    Private Sub Label65_Click(sender As Object, e As EventArgs) Handles Label65.Click
        Label65.BackColor = Color.Red
        Label67.BackColor = Color.Lime
        Label69.BackColor = Color.Lime
        Label68.BackColor = Color.Lime

        Panel2.Visible = True
        Panel4.Visible = False
        Panel5.Visible = False
        Panel1.Visible = False

        Panel2.Location = New Point(0, 41)
        Panel2.Dock = DockStyle.Fill
    End Sub

    Private Sub Label67_Click(sender As Object, e As EventArgs) Handles Label67.Click
        Label65.BackColor = Color.Lime
        Label67.BackColor = Color.Red
        Label69.BackColor = Color.Lime
        Label68.BackColor = Color.Lime

        Panel2.Visible = False
        Panel4.Visible = True
        Panel5.Visible = False
        Panel1.Visible = False

        Panel4.Location = New Point(0, 41)
        Panel4.Dock = DockStyle.Fill
    End Sub

    Private Sub Label69_Click(sender As Object, e As EventArgs) Handles Label69.Click
        Label65.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label69.BackColor = Color.Red
        Label68.BackColor = Color.Lime

        Panel2.Visible = False
        Panel4.Visible = False
        Panel5.Visible = True
        Panel1.Visible = False

        Panel5.Location = New Point(0, 41)
        Panel5.Dock = DockStyle.Fill
    End Sub

    Private Sub Label68_Click(sender As Object, e As EventArgs) Handles Label68.Click
        Label65.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label69.BackColor = Color.Lime
        Label68.BackColor = Color.Red

        Panel2.Visible = False
        Panel4.Visible = False
        Panel5.Visible = False
        Panel1.Visible = True

        Panel1.Location = New Point(0, 41)
        Panel1.Dock = DockStyle.Fill
    End Sub

    Sub deleteItem()
        Dim dialog As New DialogResult

        dialog = MsgBox("Are you sure want to delete the current line?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Transaction")

        If dialog = DialogResult.No Then
            DialogResult.Cancel.ToString()
        Else
            Try
                'DataGridView1.CurrentRow.Cells(1).Value = ""
                'cn.Open()
                'With cmd
                '    .Connection = cn
                '    .CommandText = "UPDATE tax SET value = '" & DataGridView1.CurrentRow.Cells(1).Value & "', grosspaytaxable = '" & DataGridView1.CurrentRow.Cells(2).Value & "' WHERE Code = '" & DataGridView1.CurrentRow.Cells(0).Value & "'"
                '    .ExecuteNonQuery()
                'End With
                'cn.Close()
                'taxVal()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Tax Setup", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                cn.Dispose()
            End Try
        End If
    End Sub

    Sub save()
        Try
            'cn.Open()
            'For Each row As DataGridViewRow In DataGridView1.Rows
            '    With cmd
            '        .Connection = cn
            '        .CommandText = "UPDATE tax SET value = '" & CStr(row.Cells(1).FormattedValue) & "', grosspaytaxable = '" & CStr(row.Cells(2).FormattedValue) & "' WHERE Code ='" & CStr(row.Cells(0).FormattedValue) & "'"
            '        .ExecuteNonQuery()
            '    End With
            'Next
            'cn.Close()
            'taxVal()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Tax Setup", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        Label1.Select()
        save()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs)
        Label1.Select()
        deleteItem()
    End Sub

    Private Sub Label66_Click(sender As Object, e As EventArgs) Handles Label66.Click
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
                    Catch fileException As Exception
                        Throw fileException
                    End Try
                End If

            End With

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If CheckBox01.Checked = True Then
            txtTaxP1.Enabled = True
            txtV1.Enabled = True
        Else
            txtTaxP1.Enabled = False
            txtV1.Enabled = False
        End If

        If CheckBox02.Checked = True Then
            txtTaxP2.Enabled = True
            txtV2.Enabled = True
        Else
            txtTaxP2.Enabled = False
            txtV2.Enabled = False
        End If

        If CheckBox03.Checked = True Then
            txtTaxP3.Enabled = True
            txtV3.Enabled = True
        Else
            txtTaxP3.Enabled = False
            txtV3.Enabled = False
        End If

        If CheckBox04.Checked = True Then
            txtTaxP4.Enabled = True
            txtV4.Enabled = True
        Else
            txtTaxP4.Enabled = False
            txtV4.Enabled = False
        End If

        If CheckBox05.Checked = True Then
            txtTaxP5.Enabled = True
            txtV5.Enabled = True
        Else
            txtTaxP5.Enabled = False
            txtV5.Enabled = False
        End If

        If CheckBox06.Checked = True Then
            txtTaxP6.Enabled = True
            txtV6.Enabled = True
        Else
            txtTaxP6.Enabled = False
            txtV6.Enabled = False
        End If

        If CheckBox07.Checked = True Then
            txtTaxP7.Enabled = True
            txtV7.Enabled = True
        Else
            txtTaxP7.Enabled = False
            txtV7.Enabled = False
        End If

        If CheckBox08.Checked = True Then
            txtTaxP8.Enabled = True
            txtV8.Enabled = True
        Else
            txtTaxP8.Enabled = False
            txtV8.Enabled = False
        End If

        If CheckBox09.Checked = True Then
            txtTaxP9.Enabled = True
            txtV9.Enabled = True
        Else
            txtTaxP9.Enabled = False
            txtV9.Enabled = False
        End If

        If CheckBox10.Checked = True Then
            txtTaxP10.Enabled = True
            txtV10.Enabled = True
        Else
            txtTaxP10.Enabled = False
            txtV10.Enabled = False
        End If
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs)
        Panel1.Select()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Check_default_phy_address_CheckedChanged_1(sender As Object, e As EventArgs) Handles Check_default_phy_address.CheckedChanged
        If Check_default_phy_address.Checked = True Then
            post_address1.Text = phy_unit_num.Text & ", " & phy_complex_num.Text & " " & phy_street_num.Text
            post_address2.Text = phy_street_farm_name.Text & " " & phy_suburb_district.Text & " " & phy_city_town.Text
            post_address3.Text = Combo_phy_country.Text
            post_postal_code.Text = phy_postal_code.Text
        Else
            post_address1.Text = ""
            post_address2.Text = ""
            post_address3.Text = ""
            post_postal_code.Text = ""
        End If
    End Sub

    Private Sub Check_default_sars_cont_person_CheckedChanged_1(sender As Object, e As EventArgs) Handles Check_default_sars_cont_person.CheckedChanged
        If Check_default_sars_cont_person.Checked = True Then
            uif_contact_person.Text = sars_contact_person.Text
            uif_email_address.Text = sars_email_address.Text
            uif_tel.Text = sars_tel.Text
        Else
            uif_contact_person.Clear()
            uif_email_address.Clear()
            uif_tel.Clear()
        End If
    End Sub
End Class