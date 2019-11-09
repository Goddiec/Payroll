Imports MySql.Data.MySqlClient
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel
Imports Excel = Microsoft.Office.Interop.Excel
Public Class ExportEmployees
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim da As New MySqlDataAdapter
    Dim READER As MySqlDataReader
    Dim dr As MySqlDataReader
    Dim MySqlCmd As New MySqlCommand

    Dim connString As String
    Dim excelLocation As String
    Dim myCon As MySqlConnection
    Dim ds As DataSet
    Dim tables As DataTableCollection
    Dim APP As New Excel.Application
    Dim worksheet As Excel.Worksheet
    Dim workbook As Excel.Workbooks
    Dim MyConnection As System.Data.OleDb.OleDbConnection
    Dim DtSet As System.Data.DataSet
    Dim MyCommand As System.Data.OleDb.OleDbDataAdapter

    Private Excel03ConString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
    Private Excel07ConString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"

    Private Sub Execute_Local(SQLStatement As String)
        Using CN As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
            Using CMD As New MySqlCommand(SQLStatement, CN)
                CN.Open()
                CMD.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Sub EmployeeExported()
        'Initialize the objects before use
        Dim dataAdapter As New MySqlDataAdapter()
        Dim dataSet As New DataSet
        Dim command As New MySqlCommand
        Dim datatableMain As New System.Data.DataTable()
        Dim connection As New MySqlConnection

        'Assign your connection string to connection object
        connection.ConnectionString = "server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";"
        command.Connection = connection
        command.CommandType = CommandType.Text
        'You can use any command select
        command.CommandText = "Select 
                                        code As 'Code',
                                        title As 'Title',
                                        first_name As 'First Name',
                                        last_name As 'Last Name',
                                        id_number As 'ID Number',
                                        passport_num As 'Passport Number',
                                        initial As 'Initials',
                                        second_name As 'Second Name',
                                        know_name As 'Know Name',
                                        date_of_birth As 'Date Of Birth',
                                        passport_country As 'Passport Country',
                                        asylum_seeker As 'Asylum Seeker',
                                        refugee As 'refugee',
                                        unit_num As 'Unit Number',
                                        complex_name As 'Complex Name',
                                        street_num As 'Street Number',
                                        street_farm_name As 'Street Farm Name',
                                        suburb_district As 'Suburb District',
                                        city_town As 'City Town',
                                        post_code As 'Post Code',
                                        country As 'Country',
                                        default_phy_res_address As 'Default Physical Address',
                                        post_unit_num As 'Postal Unit Number',
                                        post_complex_name As 'Postal Complex Name',
                                        post_street_num As 'Postal Street Number',
                                        post_street_farm_name As 'Postal Street Farm Name',
                                        post_suburb_district As 'Postal Suburb District',
                                        post_city_town As 'Postal City Town',
                                        post_postal_code As 'Postal Post Code',
                                        post_country As 'Postal Country',
                                        pay_with As 'Pay With',
                                        acc_type As 'Acc Type',
                                        bank As 'Bank',
                                        branch_code As 'Branch Code',
                                        acc_num As 'Acc Num',
                                        acc_holder_name As 'Acc Holder Name',
                                        other_bank As 'Other Bank',
                                        branch_name As 'Branch Name',
                                        account_holder_rel As 'Account Holder',
                                        start_date As 'Start Date',
                                        end_date As 'End Date',
                                        start_as As 'Start As',
                                        department As 'Department',
                                        working_h_day As 'Working Hour Day',
                                        working_d_week As 'Working Day Week',
                                        avrg_working_h_month As 'Avrg Working Hour Month',
                                        avrg_working_d_month As 'Avrg Working Days Month',
                                        annual_salary As 'Annual Salary',
                                        fixed_salary As 'Net Salary',
                                        rate_per_day As 'Rate Per Day',
                                        rate_per_hour As 'Rate Per Hour',
                                        employed As 'Employed',
                                        paybasis As 'Pay Basis',
                                        designation As 'Designation',
                                        email As 'Email',
                                        phonenum As 'Phone Number',
                                        taxnum As 'Tax Number',
                                        loan As 'Loan',
                                        pension As 'Pension',
                                        riskbenefits As 'Risk Benefit',
                                        retirementfund As 'Retirement Fund',
                                        medicalaid As 'Medical Aid',
                                        pocketexpense As 'Pocket Expense',
                                        dependantnum As 'Dependances'
                                        From employee" ' WHERE code BETWEEN '" & Trim(txtCodeStart.Text) & "' AND '" & Trim(txtCodeEnd.Text) & "' AND department BETWEEN '" & ComboDepart1.Text.Remove(3) & "' AND  '" & ComboDepart2.Text.Remove(3) & "'"
        dataAdapter.SelectCommand = command

        Dim f As FolderBrowserDialog = New FolderBrowserDialog
        Try
            If f.ShowDialog() = DialogResult.OK Then
                'This section help you if your language is not English.
                System.Threading.Thread.CurrentThread.CurrentCulture =
                System.Globalization.CultureInfo.CreateSpecificCulture("en-US")
                Dim oExcel As Excel.Application
                Dim oBook As Excel.Workbook
                Dim oSheet As Excel.Worksheet
                oExcel = CreateObject("Excel.Application")
                oBook = oExcel.Workbooks.Add(Type.Missing)
                oSheet = oBook.Worksheets(1)

                Dim dc As System.Data.DataColumn
                Dim dr As System.Data.DataRow
                Dim colIndex As Integer = 0
                Dim rowIndex As Integer = 0

                'Fill data to datatable
                connection.Open()
                dataAdapter.Fill(datatableMain)
                connection.Close()

                'Export the Columns to excel file
                For Each dc In datatableMain.Columns
                    colIndex = colIndex + 1
                    oSheet.Cells(1, colIndex) = dc.ColumnName
                Next

                'Export the rows to excel file
                For Each dr In datatableMain.Rows
                    rowIndex = rowIndex + 1
                    colIndex = 0
                    For Each dc In datatableMain.Columns
                        colIndex = colIndex + 1
                        oSheet.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
                    Next
                Next

                'Set final path
                Dim fileName As String = "Export File " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".csv" '"\ExportedAuthors" + ".csv"
                Dim finalPath = f.SelectedPath + fileName
                txtPath.Text = finalPath
                oSheet.Columns.AutoFit()
                'Save file in final path
                oBook.SaveAs(finalPath, XlFileFormat.xlWorkbookNormal, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing)

                'Release the objects
                releaseObject(oSheet)
                oBook.Close(False, Type.Missing, Type.Missing)
                releaseObject(oBook)
                oExcel.Quit()
                releaseObject(oExcel)
                'Some time Office application does not quit after automation: 
                'so i am calling GC.Collect method.
                GC.Collect()

                MessageBox.Show("Export done successfully!")
                Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK)
        End Try
    End Sub

    Sub ExportInventoryItems()
        Try
            cn.Open()
            Dim Namequery As String = "Select COUNT(ID) As 'ID' FROM employee"
            cmd = New MySqlCommand(Namequery, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                num = dr.GetString("ID").ToString
            End While
            cn.Close()

            Dim file As String
            sfd.Filter = "Excel Files (*.csv)|*.csv|All files (*.*)|*.*"
            sfd.FileName = "Export File " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".csv"

            If sfd.ShowDialog = DialogResult.OK Then
                Dim dataAdapter As New MySqlDataAdapter()
                Dim dataSet As New DataSet
                Dim command As New MySqlCommand
                Dim datatableMain As New System.Data.DataTable()
                Dim connection As New MySqlConnection

                'Assign your connection string to connection object
                connection.ConnectionString = "server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";"
                command.Connection = connection
                command.CommandType = CommandType.Text
                'You can use any command select
                command.CommandText = "SELECT 
                                        code As 'Code',
                                        title As 'Title',
                                        first_name As 'First Name',
                                        last_name As 'Last Name',
                                        id_number As 'ID Number',
                                        passport_num As 'Passport Number',
                                        initial As 'Initials',
                                        second_name As 'Second Name',
                                        know_name As 'Know Name',
                                        date_of_birth As 'Date Of Birth',
                                        passport_country As 'Passport Country',
                                        asylum_seeker As 'Asylum Seeker',
                                        refugee As 'refugee',
                                        unit_num As 'Unit Number',
                                        complex_name As 'Complex Name',
                                        street_num As 'Street Number',
                                        street_farm_name As 'Street Farm Name',
                                        suburb_district As 'Suburb District',
                                        city_town As 'City Town',
                                        post_code As 'Post Code',
                                        country As 'Country',
                                        default_phy_res_address As 'Default Physical Address',
                                        post_unit_num As 'Postal Unit Number',
                                        post_complex_name As 'Postal Complex Name',
                                        post_street_num As 'Postal Street Number',
                                        post_street_farm_name As 'Postal Street Farm Name',
                                        post_suburb_district As 'Postal Suburb District',
                                        post_city_town As 'Postal City Town',
                                        post_postal_code As 'Postal Post Code',
                                        post_country As 'Postal Country',
                                        pay_with As 'Pay With',
                                        acc_type As 'Acc Type',
                                        bank As 'Bank',
                                        branch_code As 'Branch Code',
                                        acc_num As 'Acc Num',
                                        acc_holder_name As 'Acc Holder Name',
                                        other_bank As 'Other Bank',
                                        branch_name As 'Branch Name',
                                        account_holder_rel As 'Account Holder',
                                        start_date As 'Start Date',
                                        end_date As 'End Date',
                                        start_as As 'Start As',
                                        department As 'Department',
                                        working_h_day As 'Working Hour Day',
                                        working_d_week As 'Working Day Week',
                                        avrg_working_h_month As 'Avrg Working Hour Month',
                                        avrg_working_d_month As 'Avrg Working Days Month',
                                        annual_salary As 'Annual Salary',
                                        fixed_salary As 'Net Salary',
                                        rate_per_day As 'Rate Per Day',
                                        rate_per_hour As 'Rate Per Hour',
                                        employed As 'Employed',
                                        paybasis As 'Pay Basis',
                                        designation As 'Designation',
                                        email As 'Email',
                                        phonenum As 'Phone Number',
                                        taxnum As 'Tax Number',
                                        loan As 'Loan',
                                        pension As 'Pension',
                                        riskbenefits As 'Risk Benefit',
                                        retirementfund As 'Retirement Fund',
                                        medicalaid As 'Medical Aid',
                                        pocketexpense As 'Pocket Expense',
                                        dependantnum As 'Dependances'
                                        FROM employee" ' WHERE code BETWEEN '" & Trim(txtCodeStart.Text) & "' AND '" & Trim(txtCodeEnd.Text) & "' AND department BETWEEN '" & ComboDepart1.Text.Remove(3) & "' AND  '" & ComboDepart2.Text.Remove(3) & "'"
                dataAdapter.SelectCommand = command

                'Dim file As String
                'sfd.Filter = "Excel Files (*.csv)|*.csv|All files (*.*)|*.*"
                'sfd.FileName = "Export File " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".csv"
                Dim numRows As Integer = 0

                file = sfd.FileName
                'This section help you if your language is not English.
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US")
                Dim oExcel As Excel.Application
                Dim oBook As Excel.Workbook
                Dim oSheet As Excel.Worksheet
                oExcel = CreateObject("Excel.Application")
                oBook = oExcel.Workbooks.Add(Type.Missing)
                oSheet = oBook.Worksheets(1)

                Dim dc As System.Data.DataColumn
                Dim dr As System.Data.DataRow
                Dim colIndex As Integer = 0
                Dim rowIndex As Integer = 0

                'Fill data to datatable
                connection.Open()
                dataAdapter.Fill(datatableMain)
                connection.Close()

                'Dim obooks As Excel.Workbook
                'Dim oapp As Excel.Application

                'oapp = New Excel.Application
                'obooks = oapp.Workbooks.Open(sfd.FileName)

                'While (obooks.Activatesheet.Cells(numRows, 1).value IsNot Nothing)
                '    numRows += 1
                'End While
                oSheet.Columns.AutoFit()
                'Save file in final path
                oBook.SaveAs(sfd.FileName, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing)

                'Release the objects
                releaseObject(oSheet)
                oBook.Close(False, Type.Missing, Type.Missing)
                releaseObject(oBook)
                oExcel.Quit()
                releaseObject(oExcel)
                'Some time Office application does not quit after automation: 
                'so i am calling GC.Collect method.
                GC.Collect()
            End If

            Label33.Visible = True
            Label33.Text = num & " rows exported."
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Export Error")
        End Try
    End Sub

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    'Import
    Dim numScv As System.Data.OleDb.OleDbDataAdapter
    Dim sql As String
    Dim result As Integer
    Dim fBrowse As New OpenFileDialog
    Sub import()
        Try
            If fBrowse.ShowDialog() = DialogResult.OK Then
                'Label3.Text = "Importing Customer Accounts. Please Wait..."
                Try
                    Dim fname As String
                    fname = fBrowse.FileName
                    MyConnection = New System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source='" & fname & " '; " & "Extended Properties=Excel 8.0;")
                    MyCommand = New System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection)
                    MyCommand.TableMappings.Add("employee", "employee")
                    DtSet = New System.Data.DataSet
                    MyCommand.Fill(DtSet)
                    MyConnection.Close()
                    For Each Drr As DataRow In DtSet.Tables(0).Rows
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
                                .Parameters.AddWithValue("@end_date", Trim(Drr(41).ToString))
                                .Parameters.AddWithValue("@start_as", Trim(Drr(40).ToString))
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
                                .Parameters.AddWithValue("@phonenum", Trim(Drr(55).ToString))
                                .Parameters.AddWithValue("@taxnum", Trim(Drr(56).ToString))
                                .Parameters.AddWithValue("@loan", Trim(Drr(57).ToString))
                                .Parameters.AddWithValue("@pension", Trim(Drr(58).ToString))
                                .Parameters.AddWithValue("@riskbenefits", Trim(Drr(59).ToString))
                                .Parameters.AddWithValue("@retirementfund", Trim(Drr(60).ToString))
                                .Parameters.AddWithValue("@medicalaid", Trim(Drr(61).ToString))
                                .Parameters.AddWithValue("@pocketexpense", Trim(Drr(62).ToString))
                                .Parameters.AddWithValue("@dependantnum", Trim(Drr(63).ToString))
                                .Parameters.AddWithValue("@ImageFile", My.Resources.pic)

                                result = .ExecuteNonQuery()
                            End With

                            If result > 0 Then
                                MessageBox.Show("Employee successfully created.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                MsgBox("Error query", MsgBoxStyle.Exclamation)
                            End If
                        Else
                            Execute_Local("UPDATE tbcustomers SET Description = '" & Trim(Drr(1).ToString) & "',Category = '00" & Drr(2).ToString & "',Address01 = '" & Drr(3).ToString & "',Address02 = '" & Drr(4).ToString & "',Address03 = '" & Drr(5).ToString & "',Address04 = '" & Drr(6).ToString & "',Address05 = '" & Drr(7).ToString & "',TaxCode = '00" & Drr(8).ToString & "',DiscountType = '00" & Drr(9).ToString & "',Blocked = '" & Drr(10).ToString & "',Creditlimit = '" & Drr(11).ToString & "',PriceRegime = '00" & Drr(12).ToString & "',Cellphone = '" & Drr(13).ToString & "',Tel = '" & Drr(14).ToString & "',Fax = '" & Drr(15).ToString & "',Email = '" & Drr(16).ToString & "',TaxRef = '" & Drr(17).ToString & "',UserDefined1 = '" & Drr(18).ToString & "',UserDefined2 = '" & Drr(19).ToString & "',UserDefined3 = '" & Drr(20).ToString & "',UserDefined4 = '" & Drr(21).ToString & "',UserDefined5 = '" & Drr(22).ToString & "',UserDefined6 = '" & Drr(23).ToString & "',Freight = '" & Drr(24).ToString & "',Ship = '" & Drr(25).ToString & "',UpdatedOn = CURDATE() WHERE Code = '" & Drr(0).ToString & "'")

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
                                .Parameters.AddWithValue("@phonenum", Trim(Drr(55).ToString))
                                .Parameters.AddWithValue("@taxnum", Trim(Drr(56).ToString))
                                .Parameters.AddWithValue("@loan", Trim(Drr(57).ToString))
                                .Parameters.AddWithValue("@pension", Trim(Drr(58).ToString))
                                .Parameters.AddWithValue("@riskbenefits", Trim(Drr(59).ToString))
                                .Parameters.AddWithValue("@retirementfund", Trim(Drr(60).ToString))
                                .Parameters.AddWithValue("@medicalaid", Trim(Drr(61).ToString))
                                .Parameters.AddWithValue("@pocketexpense", Trim(Drr(62).ToString))
                                .Parameters.AddWithValue("@dependantnum", Trim(Drr(63).ToString))
                                result = .ExecuteNonQuery()
                            End With

                            If result > 0 Then
                                MessageBox.Show("Employee successfully updated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                MsgBox("Error query", MsgBoxStyle.Exclamation)
                            End If
                        End If
                        cn.Close()
                    Next
                    'Label3.Text = "Customers Successfully Imported"
                Catch ex As Exception
                    MessageBox.Show(ex.Message & "This problem arises when you are running on a 64bit system and installed ADO.NET provider (Microsoft ACE.OLEDB.12.0) is the 32bit version.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    MyConnection.Dispose()
                End Try
            End If

            'ComboBox1.SelectedIndex = 0
            'Label3.Text = "Inventories Successfully Imported"
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            MyConnection.Dispose()
        End Try
    End Sub

    Private Sub MyWorkMethod()
        import()
        Threading.Thread.Sleep(10000)
        Dim t As New Threading.Thread(AddressOf MyWorkMethod)
        t.Abort()
    End Sub

    'Import End
    Dim num As String
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Panel1.Select()
        If Label1.Text = "Export Employees" Then
            'ExportInventoryItems()
            EmployeeExported()
        ElseIf Label1.Text = "Import Employees" Then
            import()
        End If
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Panel1.Select()
        Close()

        'Dim table As New DataTable()
        'table.Columns.Add("ID", GetType(Integer))
        'table.Columns.Add("ItemID")

        ''Add 10 rows of data
        'For i As Integer = 1 To 10
        '    table.Rows.Add(i, Nothing) : Next

        ''Configure ProgressBar
        'Me.ProgressBar1.Minimum = 0
        'Me.ProgressBar1.Maximum = table.Rows.Count
        'Me.ProgressBar1.Value = 0

        ''Export data
        'For i As Integer = 0 To table.Rows.Count - 1

        '    'Export row to excel

        '    'Increment Progress Bar
        '    Me.ProgressBar1.Value += 1

        'Next
    End Sub

    Public Sub Periods()
        cn.Open()
        ComboDepart1.Items.Clear()
        ComboDepart2.Items.Clear()
        Dim store As String = "SELECT * FROM departments WHERE Description <> '' ORDER BY Code"
        cmd = New MySqlCommand(store, cn)
        dr = cmd.ExecuteReader
        While dr.Read
            Dim Code = dr.GetString("Code").ToString
            Dim Description = dr.GetString("Description").ToString
            ComboDepart1.Items.Add(Code + " - " + Description)
            ComboDepart2.Items.Add(Code + " - " + Description)
        End While
        cn.Close()
    End Sub

    Private Sub ExportEmployees_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel1.Select()
        Periods()
        Label2.Select()
        txtCodeStart.Text = ""
        txtCodeEnd.Text = "ZZZZZZZZZZZZZZZ"
        ComboDepart1.SelectedIndex = 0
        Dim lastitem As Integer = 0
        lastitem = ComboDepart2.Items.Count
        Me.ComboDepart2.SelectedIndex = lastitem - 1
        txtPath.Clear()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs)

    End Sub

    Public exportEmp1 As Char
    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click
        exportEmp1 = "Y"
        SearchEmployee.ShowDialog()
    End Sub

    Public exportEmp2 As Char
    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        exportEmp2 = "Y"
        SearchEmployee.ShowDialog()
    End Sub
End Class