Imports System.ComponentModel
Imports System.IO
Imports System.Threading
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Public Class newcompany
    Dim conn As MySqlConnection
    Dim cmd As MySqlCommand
    Dim strConn As String
    Dim dr As MySqlDataReader
    <STAThread>
    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Dim count As Integer = 0
    Public server As String
    Public databases As String
    Public password As String
    Public user As String
    Public Sub DataBase()
        If RadioButtonNew.Checked = True Then
            Dim data As New Database
            With data
                'Assing the object property values
                .ServerName = txt_server.Text
                .UserID = txt_uid.Text
                .Password = txt_pwd.Text
                .Port = txt_port.Text

                If .Connection Then
                    If txtName.Text = "" Then
                        MsgBox("Please enter company name to proceed.", MsgBoxStyle.Exclamation, "New Company")
                        txtName.Focus()
                    Else
                        'If My.Settings.CompanyNum > 2 Then
                        '    MsgBox("You have exceeded the number[2] of companies!", MsgBoxStyle.Exclamation, "Company")
                        '    'Application.ExitThread()
                        '    Me.Close()
                        'Else
                        Try
                            With data
                                .ServerName = txt_server.Text
                                .UserID = txt_uid.Text
                                .Password = txt_pwd.Text
                                .Port = txt_port.Text

                                If .Connection Then
                                    Try
                                        Label3.Visible = True
                                        Label3.Text = "Activating new company data...Please wait"

                                        strConn = "Server = " & txt_server.Text & "; userid = " & txt_uid.Text & "; password = " & txt_pwd.Text & "; port = " & txt_port.Text & ";"
                                        strConn &= "Database = mysql; pooling=false;"
                                        conn = New MySqlConnection(strConn)
                                        cmd = New MySqlCommand("Create Database If Not exists " & txtName.Text & "", conn)
                                        conn.Open()
                                        cmd.ExecuteNonQuery()
                                        conn.ChangeDatabase("" & txtName.Text & "")

                                        'employee
                                        count = 1
                                        Label3.Text = "Creating table employee..."
                                        Dim employee = "CREATE TABLE If Not EXISTS employee(ID INT(11) Not NULL AUTO_INCREMENT,
                                                        code VARCHAR(100),
                                                        title VARCHAR(10),
                                                        first_name VARCHAR(100),
                                                        last_name  VARCHAR(100),
                                                        id_number VARCHAR(100),
                                                        passport_num VARCHAR(100),
                                                        initial VARCHAR(10),
                                                        second_name VARCHAR(100),
                                                        know_name VARCHAR(100),
                                                        date_of_birth DATE,
                                                        passport_country VARCHAR(100),
                                                        asylum_seeker ENUM('Y','N') NOT NULL DEFAULT 'N',
                                                        refugee ENUM('Y','N') NOT NULL DEFAULT 'N',
                                                        unit_num VARCHAR(100),
                                                        complex_name VARCHAR(100),
                                                        street_num VARCHAR(100),
                                                        street_farm_name VARCHAR(100),
                                                        suburb_district VARCHAR(100),
                                                        city_town VARCHAR(100),
                                                        post_code VARCHAR(100),
                                                        country VARCHAR(100),
                                                        default_phy_res_address ENUM('Y','N') NOT NULL DEFAULT 'N',
                                                        post_unit_num VARCHAR(100),
                                                        post_complex_name VARCHAR(100),
                                                        post_street_num VARCHAR(100),
                                                        post_street_farm_name VARCHAR(100),
                                                        post_suburb_district VARCHAR(100),
                                                        post_city_town VARCHAR(100),
                                                        post_postal_code VARCHAR(100),
                                                        post_country VARCHAR(100),
                                                        pay_with VARCHAR(100),
                                                        acc_type VARCHAR(100),
                                                        bank VARCHAR(100),
                                                        branch_code VARCHAR(100),
                                                        acc_num VARCHAR(100),
                                                        acc_holder_name VARCHAR(100),
                                                        other_bank VARCHAR(100),
                                                        branch_name VARCHAR(100),
                                                        account_holder_rel VARCHAR(100),
                                                        start_date DATE,
                                                        end_date DATE,
                                                        start_as VARCHAR(100),
                                                        department VARCHAR(100),
                                                        working_h_day DECIMAL(10,2) Default '0.00',
                                                        working_d_week DECIMAL(10,2) Default '0.00',
                                                        avrg_working_h_month DECIMAL(10,2) Default '0.00',
                                                        avrg_working_d_month DECIMAL(10,2) Default '0.00',
                                                        annual_salary DECIMAL(10,2) Default '0.00',
                                                        fixed_salary DECIMAL(10,2) Default '0.00',
                                                        rate_per_day DECIMAL(10,2) Default '0.00',
                                                        rate_per_hour DECIMAL(10,2) Default '0.00',
                                                        ImageFile LONGBLOB,
                                                        Caption VARCHAR(255),
                                                        employed ENUM('Y', 'N'),
                                                        paybasis VARCHAR(255),
                                                        designation VARCHAR(45),
                                                        phonenum VARCHAR(20),
                                                        taxnum VARCHAR(50),
                                                        email VARCHAR(500),
                                                        loan DECIMAL(10,2) Default '0.00',
                                                        pension DECIMAL(10,2) Default '0.00',
                                                        riskbenefits DECIMAL(10,2) Default '0.00',
                                                        retirementfund DECIMAL(10,2) Default '0.00',
                                                        medicalaid DECIMAL(10,2) Default '0.00',
                                                        pocketexpense DECIMAL(10,2) Default '0.00',
                                                        dependantnum DECIMAL(10,2) Default '0.00',
                                                        PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(employee.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'company
                                        count = 2
                                        Label3.Text = "Creating table company..."
                                        Dim company = "CREATE TABLE IF NOT EXISTS company(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                                com_num VARCHAR(100),
                                                                com_name VARCHAR(100),
                                                                com_reg_num VARCHAR(100),
                                                                sdl_pay_ref_num VARCHAR(100),
                                                                uif_pay_ref_num VARCHAR(100),
                                                                uif_reg_num VARCHAR(100),
                                                                standard_industry_class VARCHAR(100),
                                                                phy_unit_num VARCHAR(100),
                                                                phy_complex_num VARCHAR(100),
                                                                phy_street_num VARCHAR(100),
                                                                phy_street_farm_name VARCHAR(100),
                                                                phy_suburb_district VARCHAR(100),
                                                                phy_city_town VARCHAR(100),
                                                                phy_postal_code VARCHAR(100),
                                                                phy_country VARCHAR(100),
                                                                default_phy_address VARCHAR(100),
                                                                post_address1 VARCHAR(100),
                                                                post_address2 VARCHAR(100),
                                                                post_address3 VARCHAR(100),
                                                                post_postal_code VARCHAR(100),
                                                                sars_contact_person VARCHAR(100),
                                                                sars_email_address VARCHAR(100),
                                                                sars_tel VARCHAR(100),
                                                                revenue_autho VARCHAR(100),
                                                                default_sars_cont_person VARCHAR(100),
                                                                uif_contact_person VARCHAR(100),
                                                                uif_email_address VARCHAR(100),
                                                                uif_tel VARCHAR(100),
                                                                ImageFile LONGBLOB,
                                                                Caption VARCHAR(500),
                                                                PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(company.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'allowances
                                        Label3.Text = "Creating table Allowances..."
                                        Dim allowances = "CREATE TABLE IF NOT EXISTS allowances(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                                emp_code CHAR(3),
                                                                description VARCHAR(30),
                                                                amount DECIMAL(10,2) Default '0.00',
                                                                PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(allowances.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'Users
                                        count = 2
                                        Label3.Text = "Creating table Users..."
                                        Dim users = "CREATE TABLE IF NOT EXISTS users(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                    user_name VARCHAR(50),
                                                    email_address VARCHAR(100),
                                                    first_name VARCHAR(50),
                                                    last_name VARCHAR(50),
                                                    password VARCHAR(500),
                                                    pay_employees ENUM('Y', 'N') Default 'N',
                                                    add_department ENUM('Y', 'N') Default 'N',
                                                    add_designation ENUM('Y', 'N') Default 'N',
                                                    add_leave_type ENUM('Y', 'N') Default 'N',
                                                    add_employee ENUM('Y', 'N') Default 'N',
                                                    employee_list ENUM('Y', 'N') Default 'N',
                                                    hr_documents ENUM('Y', 'N') Default 'N',
                                                    issue_leave ENUM('Y', 'N') Default 'N',
                                                    employee_loan ENUM('Y', 'N') Default 'N',
                                                    database_setup ENUM('Y', 'N') Default 'N',
                                                    registration ENUM('Y', 'N') Default 'N',
                                                    reports ENUM('Y', 'N') Default 'N',
                                                    configure_email ENUM('Y', 'N') Default 'N',
                                                    user_accounts ENUM('Y', 'N') Default 'N',
                                                    company_parameters ENUM('Y', 'N') Default 'N',
                                                    banks_setup ENUM('Y', 'N') Default 'N',
                                                    deductions_setup ENUM('Y', 'N') Default 'N',
                                                    week_setup ENUM('Y', 'N') Default 'N',
                                                    backup ENUM('Y', 'N') Default 'N',
                                                    restore ENUM('Y', 'N') Default 'N',
                                                    scan_documents ENUM('Y', 'N') Default 'N',
                                                    settings ENUM('Y', 'N') Default 'N',
                                                    PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(users.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'Departments
                                        Label3.Text = "Creating table Departments..."
                                        Dim departments = "CREATE TABLE IF NOT EXISTS departments(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                                Code CHAR(3),
                                                                Description VARCHAR(30),
                                                                PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(departments.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'Banks
                                        Label3.Text = "Creating table Banks..."
                                        Dim bank = "CREATE TABLE IF NOT EXISTS bank(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                    Code VARCHAR(4),
                                                    Description VARCHAR(50),
                                                    BranchCode VARCHAR(50),
                                                    PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(bank.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'deductions
                                        Label3.Text = "Creating table Deductions..."
                                        Dim deductions = "CREATE TABLE IF NOT EXISTS deductions(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                    emp_code VARCHAR(4),
                                                    code VARCHAR(50),
                                                    description VARCHAR(50),
                                                    amount DECIMAL(10,2) Default '0.00',
                                                    active ENUM('Y', 'N') Default 'Y',
                                                    PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(deductions.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'deductiontransaction
                                        Label3.Text = "Creating table deduction transaction..."
                                        Dim deductiontransaction = "CREATE TABLE IF NOT EXISTS deductiontransaction(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                    emp_code VARCHAR(45),
                                                    pension DECIMAL(10,2) Default '0.00',
                                                    riskbenefits DECIMAL(10,2) Default '0.00',
                                                    retirementfund DECIMAL(10,2) Default '0.00',
                                                    medicalaid DECIMAL(10,2) Default '0.00',
                                                    pocketexpense DECIMAL(10,2) Default '0.00',
                                                    dependantnum DECIMAL(10,2) Default '0.00',
                                                    PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(deductiontransaction.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'otherincome
                                        Label3.Text = "Creating table otherincome..."
                                        Dim otherincome = "CREATE TABLE IF NOT EXISTS otherincome(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                    emp_code VARCHAR(45),
                                                    OtherIncome DECIMAL(10,2) Default '0.00',
                                                    DeductOtherIncomce DECIMAL(10,2) Default '0.00',
                                                    CapitalGain DECIMAL(10,2) Default '0.00',
                                                    Loans DECIMAL(10,2) Default '0.00',
                                                    PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(otherincome.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'employeeleave
                                        Label3.Text = "Creating table employeeleave..."
                                        Dim employeeleave = "CREATE TABLE IF NOT EXISTS employeeleave(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                                Code VARCHAR(45),
                                                                Description VARCHAR(500),
                                                                PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(employeeleave.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'hrdocuments
                                        Label3.Text = "Creating table hrdocuments..."
                                        Dim hrdocuments = "CREATE TABLE IF NOT EXISTS hrdocuments(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                            Code VARCHAR(45),
                                                            Description VARCHAR(100),
                                                            docname VARCHAR(100),
                                                            PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(hrdocuments.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'leavehistory
                                        Label3.Text = "Creating table leavehistory..."
                                        Dim leavehistory = "CREATE TABLE IF NOT EXISTS leavehistory(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                            employee_code VARCHAR(44),
                                                            leave_type VARCHAR(100),
                                                            start_date DATE,
                                                            end_date DATE,
                                                            num_days INT(11),
                                                            leave_for VARCHAR(45),
                                                            paid ENUM('Y', 'N') Default 'Y',
                                                            PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(leavehistory.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'loans
                                        Label3.Text = "Creating table loans..."
                                        Dim loans = "CREATE TABLE IF NOT EXISTS loans(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                            employee_code VARCHAR(45),
                                                            date DATE,
                                                            amount DECIMAL(10,2) Default '0.00',
                                                            balance DECIMAL(10,2) Default '0.00',
                                                            paid ENUM('Y', 'N') Default 'Y',
                                                            shlpay ENUM('Y', 'N') Default 'Y',
                                                            paydate DATE,
                                                            PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(loans.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'transactions
                                        Label3.Text = "Creating table transactions..."
                                        Dim transactions = "CREATE TABLE IF NOT EXISTS transactions(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                            emp_code VARCHAR(50),
                                                            datepaid DATE,
                                                            totaltax DECIMAL(10,2) Default '0.00',
                                                            uif DECIMAL(10,2) Default '0.00',
                                                            subsalary DECIMAL(10,2) Default '0.00',
                                                            otherincome DECIMAL(10,2) Default '0.00',
                                                            otherdedicincome DECIMAL(10,2) Default '0.00',
                                                            capitalgain DECIMAL(10,2) Default '0.00',
                                                            loans DECIMAL(10,2) Default '0.00',
                                                            naspa DECIMAL(10,2) Default '0.00',
                                                            pension DECIMAL(10,2) Default '0.00',
                                                            riskbenefitper DECIMAL(10,2) Default '0.00',
                                                            riskbenefitamnt DECIMAL(10,2) Default '0.00',
                                                            retirementper DECIMAL(10,2) Default '0.00',
                                                            retirementamnt DECIMAL(10,2) Default '0.00',
                                                            medicalaid DECIMAL(10,2) Default '0.00',
                                                            pocketexpen DECIMAL(10,2) Default '0.00',
                                                            overtimeamnt DECIMAL(10,2) Default '0.00',
                                                            leaveamnt DECIMAL(10,2) Default '0.00',
                                                            netsalary DECIMAL(10,2) Default '0.00',
                                                            period DECIMAL(10,2) Default '0.00',
                                                            PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(transactions.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'tax
                                        Label3.Text = "Creating table tax..."
                                        Dim tax = "CREATE TABLE IF NOT EXISTS tax(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                    taxValue1 DECIMAL(10,2) Default '0.00',
                                                    taxValue2 DECIMAL(10,2) Default '0.00',
                                                    taxValue3 DECIMAL(10,2) Default '0.00',
                                                    taxValue4 DECIMAL(10,2) Default '0.00',
                                                    taxValue5 DECIMAL(10,2) Default '0.00',
                                                    taxValue6 DECIMAL(10,2) Default '0.00',
                                                    taxValue7 DECIMAL(10,2) Default '0.00',
                                                    taxValue8 DECIMAL(10,2) Default '0.00',
                                                    taxValue9 DECIMAL(10,2) Default '0.00',
                                                    taxValue10 DECIMAL(10,2) Default '0.00',
                                                    taxGross1 DECIMAL(10,2) Default '0.00',
                                                    taxGross2 DECIMAL(10,2) Default '0.00',
                                                    taxGross3 DECIMAL(10,2) Default '0.00',
                                                    taxGross4 DECIMAL(10,2) Default '0.00',
                                                    taxGross5 DECIMAL(10,2) Default '0.00',
                                                    taxGross6 DECIMAL(10,2) Default '0.00',
                                                    taxGross7 DECIMAL(10,2) Default '0.00',
                                                    taxGross8 DECIMAL(10,2) Default '0.00',
                                                    taxGross9 DECIMAL(10,2) Default '0.00',
                                                    taxGross10 DECIMAL(10,2) Default '0.00',
                                                    use1 ENUM('Y', 'N') Default 'N',
                                                    use2 ENUM('Y', 'N') Default 'N',
                                                    use3 ENUM('Y', 'N') Default 'N',
                                                    use4 ENUM('Y', 'N') Default 'N',
                                                    use5 ENUM('Y', 'N') Default 'N',
                                                    use6 ENUM('Y', 'N') Default 'N',
                                                    use7 ENUM('Y', 'N') Default 'N',
                                                    use8 ENUM('Y', 'N') Default 'N',
                                                    use9 ENUM('Y', 'N') Default 'N',
                                                    use10 ENUM('Y', 'N') Default 'N',
                                                    PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(tax.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'scandoc
                                        Label3.Text = "Creating table scandoc..."
                                        Dim scandoc = "CREATE TABLE IF NOT EXISTS scandoc(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                    code VARCHAR(45),
                                                    Image LONGBLOB,
                                                    Name VARCHAR(45),
                                                    PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(scandoc.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'parameters
                                        Label3.Text = "Creating table parameters..."
                                        Dim parameters = "CREATE TABLE IF NOT EXISTS parameters(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                          napsa DECIMAL(10,2) Default '0.00',
                                                          napsaper DECIMAL(10,2) Default '0.00',
                                                          uif DECIMAL(10,2) Default '0.00',
                                                          tax DECIMAL(10,2) Default '0.00',
                                                          overtime_per_hour DECIMAL(10,2) Default '0.00',
                                                          email VARCHAR(100),
                                                          emailpassword VARCHAR(500),
                                                          port VARCHAR(10),
                                                          Calc VARCHAR(1) Default 'A',
                                                          PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(parameters.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'designation
                                        Label3.Text = "Creating table designation..."
                                        Dim designation = "CREATE TABLE IF NOT EXISTS designation(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                           code VARCHAR(45),
                                                           description VARCHAR(100),
                                                           PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(designation.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'scheduletable
                                        Label3.Text = "Creating table scheduletable..."
                                        Dim scheduletable = "CREATE TABLE IF NOT EXISTS scheduletable(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                             code VARCHAR(45),
                                                             emp_code VARCHAR(50),
                                                             OvertimeHours INT(11),
                                                             LeaveHours INT(11),
                                                             PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(scheduletable.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'schedulelist
                                        Label3.Text = "Creating table schedulelist..."
                                        Dim schedulelist = "CREATE TABLE IF NOT EXISTS schedulelist(ID INT(11) NOT NULL AUTO_INCREMENT,
                                                            schedule_num VARCHAR(45),
                                                            process_date DATE,
                                                            status VARCHAR(45),
                                                            schedule VARCHAR(45),
                                                            check_date DATE,
                                                            PRIMARY KEY (ID));"
                                        cmd = New MySqlCommand(schedulelist.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'INSERT INTO Departments++++++++++++++++++++++++++++++++++
                                        Label3.Text = "Inserting into table Departments...(1)"
                                        Dim tbcustomercategory1 = "INSERT INTO departments (Code,Description) VALUES('001','None')"
                                        cmd = New MySqlCommand(tbcustomercategory1.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(2)"
                                        Dim tbcustomercategory2 = "INSERT INTO departments (Code) VALUES('002')"
                                        cmd = New MySqlCommand(tbcustomercategory2.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(3)"
                                        Dim tbcustomercategory3 = "INSERT INTO departments (Code) VALUES('003')"
                                        cmd = New MySqlCommand(tbcustomercategory3.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(4)"
                                        Dim tbcustomercategory4 = "INSERT INTO departments (Code) VALUES('004')"
                                        cmd = New MySqlCommand(tbcustomercategory4.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(5)"
                                        Dim tbcustomercategory5 = "INSERT INTO departments (Code) VALUES('005')"
                                        cmd = New MySqlCommand(tbcustomercategory5.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(6)"
                                        Dim tbcustomercategory6 = "INSERT INTO departments (Code) VALUES('006')"
                                        cmd = New MySqlCommand(tbcustomercategory6.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(7)"
                                        Dim tbcustomercategory7 = "INSERT INTO departments (Code) VALUES('007')"
                                        cmd = New MySqlCommand(tbcustomercategory7.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(8)"
                                        Dim tbcustomercategory8 = "INSERT INTO departments (Code) VALUES('008')"
                                        cmd = New MySqlCommand(tbcustomercategory8.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(9)"
                                        Dim tbcustomercategory9 = "INSERT INTO departments (Code) VALUES('009')"
                                        cmd = New MySqlCommand(tbcustomercategory9.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(10)"
                                        Dim tbcustomercategory10 = "INSERT INTO departments (Code) VALUES('010')"
                                        cmd = New MySqlCommand(tbcustomercategory10.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(11)"
                                        Dim tbcustomercategory11 = "INSERT INTO departments (Code) VALUES('011')"
                                        cmd = New MySqlCommand(tbcustomercategory11.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(12)"
                                        Dim tbcustomercategory12 = "INSERT INTO departments (Code) VALUES('012')"
                                        cmd = New MySqlCommand(tbcustomercategory12.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(13)"
                                        Dim tbcustomercategory13 = "INSERT INTO departments (Code) VALUES('013')"
                                        cmd = New MySqlCommand(tbcustomercategory13.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(14)"
                                        Dim tbcustomercategory14 = "INSERT INTO departments (Code) VALUES('014')"
                                        cmd = New MySqlCommand(tbcustomercategory14.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(15)"
                                        Dim tbcustomercategory15 = "INSERT INTO departments (Code) VALUES('015')"
                                        cmd = New MySqlCommand(tbcustomercategory15.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(16)"
                                        Dim tbcustomercategory16 = "INSERT INTO departments (Code) VALUES('016')"
                                        cmd = New MySqlCommand(tbcustomercategory16.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(17)"
                                        Dim tbcustomercategory17 = "INSERT INTO departments (Code) VALUES('017')"
                                        cmd = New MySqlCommand(tbcustomercategory17.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(18)"
                                        Dim tbcustomercategory18 = "INSERT INTO departments (Code) VALUES('018')"
                                        cmd = New MySqlCommand(tbcustomercategory18.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(19)"
                                        Dim tbcustomercategory19 = "INSERT INTO departments (Code) VALUES('019')"
                                        cmd = New MySqlCommand(tbcustomercategory19.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(20)"
                                        Dim tbcustomercategory20 = "INSERT INTO departments (Code) VALUES('020')"
                                        cmd = New MySqlCommand(tbcustomercategory20.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(21)"
                                        Dim tbcustomercategory21 = "INSERT INTO departments (Code) VALUES('021')"
                                        cmd = New MySqlCommand(tbcustomercategory21.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(22)"
                                        Dim tbcustomercategory22 = "INSERT INTO departments (Code) VALUES('022')"
                                        cmd = New MySqlCommand(tbcustomercategory22.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(23)"
                                        Dim tbcustomercategory23 = "INSERT INTO departments (Code) VALUES('023')"
                                        cmd = New MySqlCommand(tbcustomercategory23.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(24)"
                                        Dim tbcustomercategory24 = "INSERT INTO departments (Code) VALUES('024')"
                                        cmd = New MySqlCommand(tbcustomercategory24.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(25)"
                                        Dim tbcustomercategory25 = "INSERT INTO departments (Code) VALUES('025')"
                                        cmd = New MySqlCommand(tbcustomercategory25.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(26)"
                                        Dim tbcustomercategory26 = "INSERT INTO departments (Code) VALUES('026')"
                                        cmd = New MySqlCommand(tbcustomercategory26.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(27)"
                                        Dim tbcustomercategory27 = "INSERT INTO departments (Code) VALUES('027')"
                                        cmd = New MySqlCommand(tbcustomercategory27.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(28)"
                                        Dim tbcustomercategory28 = "INSERT INTO departments (Code) VALUES('028')"
                                        cmd = New MySqlCommand(tbcustomercategory28.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(29)"
                                        Dim tbcustomercategory29 = "INSERT INTO departments (Code) VALUES('029')"
                                        cmd = New MySqlCommand(tbcustomercategory29.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(30)"
                                        Dim tbcustomercategory30 = "INSERT INTO departments (Code) VALUES('030')"
                                        cmd = New MySqlCommand(tbcustomercategory30.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(31)"
                                        Dim tbcustomercategory31 = "INSERT INTO departments (Code) VALUES('031')"
                                        cmd = New MySqlCommand(tbcustomercategory31.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(32)"
                                        Dim tbcustomercategory32 = "INSERT INTO departments (Code) VALUES('032')"
                                        cmd = New MySqlCommand(tbcustomercategory32.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(33)"
                                        Dim tbcustomercategory33 = "INSERT INTO departments (Code) VALUES('033')"
                                        cmd = New MySqlCommand(tbcustomercategory33.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(34)"
                                        Dim tbcustomercategory34 = "INSERT INTO departments (Code) VALUES('034')"
                                        cmd = New MySqlCommand(tbcustomercategory34.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(35)"
                                        Dim tbcustomercategory35 = "INSERT INTO departments (Code) VALUES('035')"
                                        cmd = New MySqlCommand(tbcustomercategory35.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(36)"
                                        Dim tbcustomercategory36 = "INSERT INTO departments (Code) VALUES('036')"
                                        cmd = New MySqlCommand(tbcustomercategory36.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(37)"
                                        Dim tbcustomercategory37 = "INSERT INTO departments (Code) VALUES('037')"
                                        cmd = New MySqlCommand(tbcustomercategory37.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(38)"
                                        Dim tbcustomercategory38 = "INSERT INTO departments (Code) VALUES('038')"
                                        cmd = New MySqlCommand(tbcustomercategory38.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(39)"
                                        Dim tbcustomercategory39 = "INSERT INTO departments (Code) VALUES('039')"
                                        cmd = New MySqlCommand(tbcustomercategory39.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(40)"
                                        Dim tbcustomercategory40 = "INSERT INTO departments (Code) VALUES('040')"
                                        cmd = New MySqlCommand(tbcustomercategory40.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(41)"
                                        Dim tbcustomercategory41 = "INSERT INTO departments (Code) VALUES('041')"
                                        cmd = New MySqlCommand(tbcustomercategory41.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(42)"
                                        Dim tbcustomercategory42 = "INSERT INTO departments (Code) VALUES('042')"
                                        cmd = New MySqlCommand(tbcustomercategory42.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(43)"
                                        Dim tbcustomercategory43 = "INSERT INTO departments (Code) VALUES('043')"
                                        cmd = New MySqlCommand(tbcustomercategory43.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(44)"
                                        Dim tbcustomercategory44 = "INSERT INTO departments (Code) VALUES('044')"
                                        cmd = New MySqlCommand(tbcustomercategory44.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(45)"
                                        Dim tbcustomercategory45 = "INSERT INTO departments (Code) VALUES('045')"
                                        cmd = New MySqlCommand(tbcustomercategory45.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(46)"
                                        Dim tbcustomercategory46 = "INSERT INTO departments (Code) VALUES('046')"
                                        cmd = New MySqlCommand(tbcustomercategory46.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(47)"
                                        Dim tbcustomercategory47 = "INSERT INTO departments (Code) VALUES('047')"
                                        cmd = New MySqlCommand(tbcustomercategory47.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(48)"
                                        Dim tbcustomercategory48 = "INSERT INTO departments (Code) VALUES('048')"
                                        cmd = New MySqlCommand(tbcustomercategory48.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(49)"
                                        Dim tbcustomercategory49 = "INSERT INTO departments (Code) VALUES('049')"
                                        cmd = New MySqlCommand(tbcustomercategory49.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(50)"
                                        Dim tbcustomercategory50 = "INSERT INTO departments (Code) VALUES('050')"
                                        cmd = New MySqlCommand(tbcustomercategory50.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(51)"
                                        Dim tbcustomercategory51 = "INSERT INTO departments (Code) VALUES('051')"
                                        cmd = New MySqlCommand(tbcustomercategory51.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(52)"
                                        Dim tbcustomercategory52 = "INSERT INTO departments (Code) VALUES('052')"
                                        cmd = New MySqlCommand(tbcustomercategory52.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(53)"
                                        Dim tbcustomercategory53 = "INSERT INTO departments (Code) VALUES('053')"
                                        cmd = New MySqlCommand(tbcustomercategory53.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(54)"
                                        Dim tbcustomercategory54 = "INSERT INTO departments (Code) VALUES('054')"
                                        cmd = New MySqlCommand(tbcustomercategory54.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(55)"
                                        Dim tbcustomercategory55 = "INSERT INTO departments (Code) VALUES('055')"
                                        cmd = New MySqlCommand(tbcustomercategory55.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(56)"
                                        Dim tbcustomercategory56 = "INSERT INTO departments (Code) VALUES('056')"
                                        cmd = New MySqlCommand(tbcustomercategory56.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(57)"
                                        Dim tbcustomercategory57 = "INSERT INTO departments (Code) VALUES('057')"
                                        cmd = New MySqlCommand(tbcustomercategory57.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(58)"
                                        Dim tbcustomercategory58 = "INSERT INTO departments (Code) VALUES('058')"
                                        cmd = New MySqlCommand(tbcustomercategory58.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(59)"
                                        Dim tbcustomercategory59 = "INSERT INTO departments (Code) VALUES('059')"
                                        cmd = New MySqlCommand(tbcustomercategory59.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(60)"
                                        Dim tbcustomercategory60 = "INSERT INTO departments (Code) VALUES('060')"
                                        cmd = New MySqlCommand(tbcustomercategory60.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(61)"
                                        Dim tbcustomercategory61 = "INSERT INTO departments (Code) VALUES('061')"
                                        cmd = New MySqlCommand(tbcustomercategory61.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(62)"
                                        Dim tbcustomercategory62 = "INSERT INTO departments (Code) VALUES('062')"
                                        cmd = New MySqlCommand(tbcustomercategory62.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(63)"
                                        Dim tbcustomercategory63 = "INSERT INTO departments (Code) VALUES('063')"
                                        cmd = New MySqlCommand(tbcustomercategory63.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(64)"
                                        Dim tbcustomercategory64 = "INSERT INTO departments (Code) VALUES('064')"
                                        cmd = New MySqlCommand(tbcustomercategory64.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(65)"
                                        Dim tbcustomercategory65 = "INSERT INTO departments (Code) VALUES('065')"
                                        cmd = New MySqlCommand(tbcustomercategory65.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(66)"
                                        Dim tbcustomercategory66 = "INSERT INTO departments (Code) VALUES('066')"
                                        cmd = New MySqlCommand(tbcustomercategory66.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(67)"
                                        Dim tbcustomercategory67 = "INSERT INTO departments (Code) VALUES('067')"
                                        cmd = New MySqlCommand(tbcustomercategory67.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(68)"
                                        Dim tbcustomercategory68 = "INSERT INTO departments (Code) VALUES('068')"
                                        cmd = New MySqlCommand(tbcustomercategory68.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(69)"
                                        Dim tbcustomercategory69 = "INSERT INTO departments (Code) VALUES('069')"
                                        cmd = New MySqlCommand(tbcustomercategory69.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(70)"
                                        Dim tbcustomercategory70 = "INSERT INTO departments (Code) VALUES('070')"
                                        cmd = New MySqlCommand(tbcustomercategory70.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(71)"
                                        Dim tbcustomercategory71 = "INSERT INTO departments (Code) VALUES('071')"
                                        cmd = New MySqlCommand(tbcustomercategory71.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(72)"
                                        Dim tbcustomercategory72 = "INSERT INTO departments (Code) VALUES('072')"
                                        cmd = New MySqlCommand(tbcustomercategory72.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(73)"
                                        Dim tbcustomercategory73 = "INSERT INTO departments (Code) VALUES('073')"
                                        cmd = New MySqlCommand(tbcustomercategory73.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(74)"
                                        Dim tbcustomercategory74 = "INSERT INTO departments (Code) VALUES('074')"
                                        cmd = New MySqlCommand(tbcustomercategory74.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(75)"
                                        Dim tbcustomercategory75 = "INSERT INTO departments (Code) VALUES('075')"
                                        cmd = New MySqlCommand(tbcustomercategory75.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(76)"
                                        Dim tbcustomercategory76 = "INSERT INTO departments (Code) VALUES('076')"
                                        cmd = New MySqlCommand(tbcustomercategory76.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(77)"
                                        Dim tbcustomercategory77 = "INSERT INTO departments (Code) VALUES('077')"
                                        cmd = New MySqlCommand(tbcustomercategory77.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(78)"
                                        Dim tbcustomercategory78 = "INSERT INTO departments (Code) VALUES('078')"
                                        cmd = New MySqlCommand(tbcustomercategory78.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(79)"
                                        Dim tbcustomercategory79 = "INSERT INTO departments (Code) VALUES('079')"
                                        cmd = New MySqlCommand(tbcustomercategory79.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(80)"
                                        Dim tbcustomercategory80 = "INSERT INTO departments (Code) VALUES('080')"
                                        cmd = New MySqlCommand(tbcustomercategory80.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(81)"
                                        Dim tbcustomercategory81 = "INSERT INTO departments (Code) VALUES('081')"
                                        cmd = New MySqlCommand(tbcustomercategory81.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(82)"
                                        Dim tbcustomercategory82 = "INSERT INTO departments (Code) VALUES('082')"
                                        cmd = New MySqlCommand(tbcustomercategory82.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(83)"
                                        Dim tbcustomercategory83 = "INSERT INTO departments (Code) VALUES('083')"
                                        cmd = New MySqlCommand(tbcustomercategory83.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(84)"
                                        Dim tbcustomercategory84 = "INSERT INTO departments (Code) VALUES('084')"
                                        cmd = New MySqlCommand(tbcustomercategory84.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(85)"
                                        Dim tbcustomercategory85 = "INSERT INTO departments (Code) VALUES('085')"
                                        cmd = New MySqlCommand(tbcustomercategory85.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(86)"
                                        Dim tbcustomercategory86 = "INSERT INTO departments (Code) VALUES('086')"
                                        cmd = New MySqlCommand(tbcustomercategory86.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(87)"
                                        Dim tbcustomercategory87 = "INSERT INTO departments (Code) VALUES('087')"
                                        cmd = New MySqlCommand(tbcustomercategory87.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(88)"
                                        Dim tbcustomercategory88 = "INSERT INTO departments (Code) VALUES('088')"
                                        cmd = New MySqlCommand(tbcustomercategory88.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(89)"
                                        Dim tbcustomercategory89 = "INSERT INTO departments (Code) VALUES('089')"
                                        cmd = New MySqlCommand(tbcustomercategory89.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(90)"
                                        Dim tbcustomercategory90 = "INSERT INTO departments (Code) VALUES('090')"
                                        cmd = New MySqlCommand(tbcustomercategory90.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(91)"
                                        Dim tbcustomercategory91 = "INSERT INTO departments (Code) VALUES('091')"
                                        cmd = New MySqlCommand(tbcustomercategory91.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(92)"
                                        Dim tbcustomercategory92 = "INSERT INTO departments (Code) VALUES('092')"
                                        cmd = New MySqlCommand(tbcustomercategory92.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(93)"
                                        Dim tbcustomercategory93 = "INSERT INTO departments (Code) VALUES('093')"
                                        cmd = New MySqlCommand(tbcustomercategory93.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(94)"
                                        Dim tbcustomercategory94 = "INSERT INTO departments (Code) VALUES('094')"
                                        cmd = New MySqlCommand(tbcustomercategory94.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(95)"
                                        Dim tbcustomercategory95 = "INSERT INTO departments (Code) VALUES('095')"
                                        cmd = New MySqlCommand(tbcustomercategory95.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(96)"
                                        Dim tbcustomercategory96 = "INSERT INTO departments (Code) VALUES('096')"
                                        cmd = New MySqlCommand(tbcustomercategory96.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(97)"
                                        Dim tbcustomercategory97 = "INSERT INTO departments (Code) VALUES('097')"
                                        cmd = New MySqlCommand(tbcustomercategory97.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(98)"
                                        Dim tbcustomercategory98 = "INSERT INTO departments (Code) VALUES('098')"
                                        cmd = New MySqlCommand(tbcustomercategory98.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Departments...(99)"
                                        Dim tbcustomercategory99 = "INSERT INTO departments (Code) VALUES('099')"
                                        cmd = New MySqlCommand(tbcustomercategory99.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'INSERT INTO Designation++++++++++++++++++++++++++++++++++
                                        Label3.Text = "Inserting into table Designation...(1)"
                                        Dim designation1 = "INSERT INTO designation (Code, Description) VALUES('001','Management')"
                                        cmd = New MySqlCommand(designation1.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(2)"
                                        Dim designation2 = "INSERT INTO designation (Code) VALUES('002')"
                                        cmd = New MySqlCommand(designation2.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(3)"
                                        Dim designation3 = "INSERT INTO designation (Code) VALUES('003')"
                                        cmd = New MySqlCommand(designation3.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(4)"
                                        Dim designation4 = "INSERT INTO designation (Code) VALUES('004')"
                                        cmd = New MySqlCommand(designation4.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(5)"
                                        Dim designation5 = "INSERT INTO designation (Code) VALUES('005')"
                                        cmd = New MySqlCommand(designation5.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(6)"
                                        Dim designation6 = "INSERT INTO designation (Code) VALUES('006')"
                                        cmd = New MySqlCommand(designation6.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(7)"
                                        Dim designation7 = "INSERT INTO designation (Code) VALUES('007')"
                                        cmd = New MySqlCommand(designation7.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(8)"
                                        Dim designation8 = "INSERT INTO designation (Code) VALUES('008')"
                                        cmd = New MySqlCommand(designation8.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(9)"
                                        Dim designation9 = "INSERT INTO designation (Code) VALUES('009')"
                                        cmd = New MySqlCommand(designation9.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(10)"
                                        Dim designation10 = "INSERT INTO designation (Code) VALUES('010')"
                                        cmd = New MySqlCommand(designation10.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(11)"
                                        Dim designation11 = "INSERT INTO designation (Code) VALUES('011')"
                                        cmd = New MySqlCommand(designation11.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(12)"
                                        Dim designation12 = "INSERT INTO designation (Code) VALUES('012')"
                                        cmd = New MySqlCommand(designation12.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(13)"
                                        Dim designation13 = "INSERT INTO designation (Code) VALUES('013')"
                                        cmd = New MySqlCommand(designation13.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(14)"
                                        Dim designation14 = "INSERT INTO designation (Code) VALUES('014')"
                                        cmd = New MySqlCommand(designation14.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(15)"
                                        Dim designation15 = "INSERT INTO designation (Code) VALUES('015')"
                                        cmd = New MySqlCommand(designation15.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(16)"
                                        Dim designation16 = "INSERT INTO designation (Code) VALUES('016')"
                                        cmd = New MySqlCommand(designation16.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(17)"
                                        Dim designation17 = "INSERT INTO designation (Code) VALUES('017')"
                                        cmd = New MySqlCommand(designation17.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(18)"
                                        Dim designation18 = "INSERT INTO designation (Code) VALUES('018')"
                                        cmd = New MySqlCommand(designation18.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(19)"
                                        Dim designation19 = "INSERT INTO designation (Code) VALUES('019')"
                                        cmd = New MySqlCommand(designation19.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(20)"
                                        Dim designation20 = "INSERT INTO designation (Code) VALUES('020')"
                                        cmd = New MySqlCommand(designation20.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(21)"
                                        Dim designation21 = "INSERT INTO designation (Code) VALUES('021')"
                                        cmd = New MySqlCommand(designation21.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(22)"
                                        Dim designation22 = "INSERT INTO designation (Code) VALUES('022')"
                                        cmd = New MySqlCommand(designation22.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(23)"
                                        Dim designation23 = "INSERT INTO designation (Code) VALUES('023')"
                                        cmd = New MySqlCommand(designation23.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(24)"
                                        Dim designation24 = "INSERT INTO designation (Code) VALUES('024')"
                                        cmd = New MySqlCommand(designation24.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(25)"
                                        Dim designation25 = "INSERT INTO designation (Code) VALUES('025')"
                                        cmd = New MySqlCommand(designation25.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(26)"
                                        Dim designation26 = "INSERT INTO designation (Code) VALUES('026')"
                                        cmd = New MySqlCommand(designation26.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(27)"
                                        Dim designation27 = "INSERT INTO designation (Code) VALUES('027')"
                                        cmd = New MySqlCommand(designation27.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(28)"
                                        Dim designation28 = "INSERT INTO designation (Code) VALUES('028')"
                                        cmd = New MySqlCommand(designation28.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(29)"
                                        Dim designation29 = "INSERT INTO designation (Code) VALUES('029')"
                                        cmd = New MySqlCommand(designation29.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(30)"
                                        Dim designation30 = "INSERT INTO designation (Code) VALUES('030')"
                                        cmd = New MySqlCommand(designation30.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(31)"
                                        Dim designation31 = "INSERT INTO designation (Code) VALUES('031')"
                                        cmd = New MySqlCommand(designation31.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(32)"
                                        Dim designation32 = "INSERT INTO designation (Code) VALUES('032')"
                                        cmd = New MySqlCommand(designation32.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(33)"
                                        Dim designation33 = "INSERT INTO designation (Code) VALUES('033')"
                                        cmd = New MySqlCommand(designation33.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(34)"
                                        Dim designation34 = "INSERT INTO designation (Code) VALUES('034')"
                                        cmd = New MySqlCommand(designation34.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(35)"
                                        Dim designation35 = "INSERT INTO designation (Code) VALUES('035')"
                                        cmd = New MySqlCommand(designation35.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(36)"
                                        Dim designation36 = "INSERT INTO designation (Code) VALUES('036')"
                                        cmd = New MySqlCommand(designation36.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(37)"
                                        Dim designation37 = "INSERT INTO designation (Code) VALUES('037')"
                                        cmd = New MySqlCommand(designation37.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(38)"
                                        Dim designation38 = "INSERT INTO designation (Code) VALUES('038')"
                                        cmd = New MySqlCommand(designation38.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(39)"
                                        Dim designation39 = "INSERT INTO designation (Code) VALUES('039')"
                                        cmd = New MySqlCommand(designation39.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(40)"
                                        Dim designation40 = "INSERT INTO designation (Code) VALUES('040')"
                                        cmd = New MySqlCommand(designation40.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(41)"
                                        Dim designation41 = "INSERT INTO designation (Code) VALUES('041')"
                                        cmd = New MySqlCommand(designation41.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(42)"
                                        Dim designation42 = "INSERT INTO designation (Code) VALUES('042')"
                                        cmd = New MySqlCommand(designation42.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(43)"
                                        Dim designation43 = "INSERT INTO designation (Code) VALUES('043')"
                                        cmd = New MySqlCommand(designation43.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(44)"
                                        Dim designation44 = "INSERT INTO designation (Code) VALUES('044')"
                                        cmd = New MySqlCommand(designation44.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(45)"
                                        Dim designation45 = "INSERT INTO designation (Code) VALUES('045')"
                                        cmd = New MySqlCommand(designation45.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(46)"
                                        Dim designation46 = "INSERT INTO designation (Code) VALUES('046')"
                                        cmd = New MySqlCommand(designation46.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(47)"
                                        Dim designation47 = "INSERT INTO designation (Code) VALUES('047')"
                                        cmd = New MySqlCommand(designation47.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(48)"
                                        Dim designation48 = "INSERT INTO designation (Code) VALUES('048')"
                                        cmd = New MySqlCommand(designation48.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(49)"
                                        Dim designation49 = "INSERT INTO designation (Code) VALUES('049')"
                                        cmd = New MySqlCommand(designation49.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(50)"
                                        Dim designation50 = "INSERT INTO designation (Code) VALUES('050')"
                                        cmd = New MySqlCommand(designation50.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(51)"
                                        Dim designation51 = "INSERT INTO designation (Code) VALUES('051')"
                                        cmd = New MySqlCommand(designation51.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(52)"
                                        Dim designation52 = "INSERT INTO designation (Code) VALUES('052')"
                                        cmd = New MySqlCommand(designation52.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(53)"
                                        Dim designation53 = "INSERT INTO designation (Code) VALUES('053')"
                                        cmd = New MySqlCommand(designation53.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(54)"
                                        Dim designation54 = "INSERT INTO designation (Code) VALUES('054')"
                                        cmd = New MySqlCommand(designation54.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(55)"
                                        Dim designation55 = "INSERT INTO designation (Code) VALUES('055')"
                                        cmd = New MySqlCommand(designation55.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(56)"
                                        Dim designation56 = "INSERT INTO designation (Code) VALUES('056')"
                                        cmd = New MySqlCommand(designation56.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(57)"
                                        Dim designation57 = "INSERT INTO designation (Code) VALUES('057')"
                                        cmd = New MySqlCommand(designation57.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(58)"
                                        Dim designation58 = "INSERT INTO designation (Code) VALUES('058')"
                                        cmd = New MySqlCommand(designation58.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(59)"
                                        Dim designation59 = "INSERT INTO designation (Code) VALUES('059')"
                                        cmd = New MySqlCommand(designation59.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(60)"
                                        Dim designation60 = "INSERT INTO designation (Code) VALUES('060')"
                                        cmd = New MySqlCommand(designation60.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(61)"
                                        Dim designation61 = "INSERT INTO designation (Code) VALUES('061')"
                                        cmd = New MySqlCommand(designation61.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(62)"
                                        Dim designation62 = "INSERT INTO designation (Code) VALUES('062')"
                                        cmd = New MySqlCommand(designation62.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(63)"
                                        Dim designation63 = "INSERT INTO designation (Code) VALUES('063')"
                                        cmd = New MySqlCommand(designation63.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(64)"
                                        Dim designation64 = "INSERT INTO designation (Code) VALUES('064')"
                                        cmd = New MySqlCommand(designation64.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(65)"
                                        Dim designation65 = "INSERT INTO designation (Code) VALUES('065')"
                                        cmd = New MySqlCommand(designation65.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(66)"
                                        Dim designation66 = "INSERT INTO designation (Code) VALUES('066')"
                                        cmd = New MySqlCommand(designation66.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(67)"
                                        Dim designation67 = "INSERT INTO designation (Code) VALUES('067')"
                                        cmd = New MySqlCommand(designation67.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(68)"
                                        Dim designation68 = "INSERT INTO designation (Code) VALUES('068')"
                                        cmd = New MySqlCommand(designation68.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(69)"
                                        Dim designation69 = "INSERT INTO designation (Code) VALUES('069')"
                                        cmd = New MySqlCommand(designation69.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(70)"
                                        Dim designation70 = "INSERT INTO designation (Code) VALUES('070')"
                                        cmd = New MySqlCommand(designation70.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(71)"
                                        Dim designation71 = "INSERT INTO designation (Code) VALUES('071')"
                                        cmd = New MySqlCommand(designation71.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(72)"
                                        Dim designation72 = "INSERT INTO designation (Code) VALUES('072')"
                                        cmd = New MySqlCommand(designation72.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(73)"
                                        Dim designation73 = "INSERT INTO designation (Code) VALUES('073')"
                                        cmd = New MySqlCommand(designation73.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(74)"
                                        Dim designation74 = "INSERT INTO designation (Code) VALUES('074')"
                                        cmd = New MySqlCommand(designation74.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(75)"
                                        Dim designation75 = "INSERT INTO designation (Code) VALUES('075')"
                                        cmd = New MySqlCommand(designation75.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(76)"
                                        Dim designation76 = "INSERT INTO designation (Code) VALUES('076')"
                                        cmd = New MySqlCommand(designation76.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(77)"
                                        Dim designation77 = "INSERT INTO designation (Code) VALUES('077')"
                                        cmd = New MySqlCommand(designation77.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(78)"
                                        Dim designation78 = "INSERT INTO designation (Code) VALUES('078')"
                                        cmd = New MySqlCommand(designation78.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(79)"
                                        Dim designation79 = "INSERT INTO designation (Code) VALUES('079')"
                                        cmd = New MySqlCommand(designation79.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(80)"
                                        Dim designation80 = "INSERT INTO designation (Code) VALUES('080')"
                                        cmd = New MySqlCommand(designation80.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(81)"
                                        Dim designation81 = "INSERT INTO designation (Code) VALUES('081')"
                                        cmd = New MySqlCommand(designation81.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(82)"
                                        Dim designation82 = "INSERT INTO designation (Code) VALUES('082')"
                                        cmd = New MySqlCommand(designation82.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(83)"
                                        Dim designation83 = "INSERT INTO designation (Code) VALUES('083')"
                                        cmd = New MySqlCommand(designation83.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(84)"
                                        Dim designation84 = "INSERT INTO designation (Code) VALUES('084')"
                                        cmd = New MySqlCommand(designation84.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(85)"
                                        Dim designation85 = "INSERT INTO designation (Code) VALUES('085')"
                                        cmd = New MySqlCommand(designation85.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(86)"
                                        Dim designation86 = "INSERT INTO designation (Code) VALUES('086')"
                                        cmd = New MySqlCommand(designation86.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(87)"
                                        Dim designation87 = "INSERT INTO designation (Code) VALUES('087')"
                                        cmd = New MySqlCommand(designation87.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(88)"
                                        Dim designation88 = "INSERT INTO designation (Code) VALUES('088')"
                                        cmd = New MySqlCommand(designation88.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(89)"
                                        Dim designation89 = "INSERT INTO designation (Code) VALUES('089')"
                                        cmd = New MySqlCommand(designation89.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(90)"
                                        Dim designation90 = "INSERT INTO designation (Code) VALUES('090')"
                                        cmd = New MySqlCommand(designation90.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(91)"
                                        Dim designation91 = "INSERT INTO designation (Code) VALUES('091')"
                                        cmd = New MySqlCommand(designation91.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(92)"
                                        Dim designation92 = "INSERT INTO designation (Code) VALUES('092')"
                                        cmd = New MySqlCommand(designation92.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(93)"
                                        Dim designation93 = "INSERT INTO designation (Code) VALUES('093')"
                                        cmd = New MySqlCommand(designation93.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(94)"
                                        Dim designation94 = "INSERT INTO designation (Code) VALUES('094')"
                                        cmd = New MySqlCommand(designation94.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(95)"
                                        Dim designation95 = "INSERT INTO designation (Code) VALUES('095')"
                                        cmd = New MySqlCommand(designation95.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(96)"
                                        Dim designation96 = "INSERT INTO designation (Code) VALUES('096')"
                                        cmd = New MySqlCommand(designation96.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(97)"
                                        Dim designation97 = "INSERT INTO designation (Code) VALUES('097')"
                                        cmd = New MySqlCommand(designation97.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(98)"
                                        Dim designation98 = "INSERT INTO designation (Code) VALUES('098')"
                                        cmd = New MySqlCommand(designation98.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(99)"
                                        Dim designation99 = "INSERT INTO designation (Code) VALUES('099')"
                                        cmd = New MySqlCommand(designation99.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'INSERT INTO employeeleave++++++++++++++++++++++++++++++++++
                                        Label3.Text = "Inserting into table employeeleave...(1)"
                                        Dim employeeleave1 = "INSERT INTO employeeleave (Code, Description) VALUES('001','Annual Leave')"
                                        cmd = New MySqlCommand(employeeleave1.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(2)"
                                        Dim employeeleave2 = "INSERT INTO employeeleave (Code, Description) VALUES('002','Sick Leave')"
                                        cmd = New MySqlCommand(employeeleave2.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(3)"
                                        Dim employeeleave3 = "INSERT INTO employeeleave (Code, Description) VALUES('003','Family Leave')"
                                        cmd = New MySqlCommand(employeeleave3.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(4)"
                                        Dim employeeleave4 = "INSERT INTO employeeleave (Code, Description) VALUES('004','Maternity Leave')"
                                        cmd = New MySqlCommand(employeeleave4.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(5)"
                                        Dim employeeleave5 = "INSERT INTO employeeleave (Code) VALUES('005')"
                                        cmd = New MySqlCommand(employeeleave5.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(6)"
                                        Dim employeeleave6 = "INSERT INTO employeeleave (Code) VALUES('006')"
                                        cmd = New MySqlCommand(employeeleave6.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(7)"
                                        Dim employeeleave7 = "INSERT INTO employeeleave (Code) VALUES('007')"
                                        cmd = New MySqlCommand(employeeleave7.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(8)"
                                        Dim employeeleave8 = "INSERT INTO employeeleave (Code) VALUES('008')"
                                        cmd = New MySqlCommand(employeeleave8.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(9)"
                                        Dim employeeleave9 = "INSERT INTO employeeleave (Code) VALUES('009')"
                                        cmd = New MySqlCommand(employeeleave9.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(10)"
                                        Dim employeeleave10 = "INSERT INTO employeeleave (Code) VALUES('010')"
                                        cmd = New MySqlCommand(employeeleave10.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(11)"
                                        Dim employeeleave11 = "INSERT INTO employeeleave (Code) VALUES('011')"
                                        cmd = New MySqlCommand(employeeleave11.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(12)"
                                        Dim employeeleave12 = "INSERT INTO employeeleave (Code) VALUES('012')"
                                        cmd = New MySqlCommand(employeeleave12.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(13)"
                                        Dim employeeleave13 = "INSERT INTO employeeleave (Code) VALUES('013')"
                                        cmd = New MySqlCommand(employeeleave13.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(14)"
                                        Dim employeeleave14 = "INSERT INTO employeeleave (Code) VALUES('014')"
                                        cmd = New MySqlCommand(employeeleave14.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(15)"
                                        Dim employeeleave15 = "INSERT INTO employeeleave (Code) VALUES('015')"
                                        cmd = New MySqlCommand(employeeleave15.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(16)"
                                        Dim employeeleave16 = "INSERT INTO employeeleave (Code) VALUES('016')"
                                        cmd = New MySqlCommand(employeeleave16.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(17)"
                                        Dim employeeleave17 = "INSERT INTO employeeleave (Code) VALUES('017')"
                                        cmd = New MySqlCommand(employeeleave17.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(18)"
                                        Dim employeeleave18 = "INSERT INTO employeeleave (Code) VALUES('018')"
                                        cmd = New MySqlCommand(employeeleave18.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(19)"
                                        Dim employeeleave19 = "INSERT INTO employeeleave (Code) VALUES('019')"
                                        cmd = New MySqlCommand(employeeleave19.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(20)"
                                        Dim employeeleave20 = "INSERT INTO employeeleave (Code) VALUES('020')"
                                        cmd = New MySqlCommand(employeeleave20.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(21)"
                                        Dim employeeleave21 = "INSERT INTO employeeleave (Code) VALUES('021')"
                                        cmd = New MySqlCommand(employeeleave21.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(22)"
                                        Dim employeeleave22 = "INSERT INTO employeeleave (Code) VALUES('022')"
                                        cmd = New MySqlCommand(employeeleave22.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(23)"
                                        Dim employeeleave23 = "INSERT INTO employeeleave (Code) VALUES('023')"
                                        cmd = New MySqlCommand(employeeleave23.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(24)"
                                        Dim employeeleave24 = "INSERT INTO employeeleave (Code) VALUES('024')"
                                        cmd = New MySqlCommand(employeeleave24.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(25)"
                                        Dim employeeleave25 = "INSERT INTO employeeleave (Code) VALUES('025')"
                                        cmd = New MySqlCommand(employeeleave25.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(26)"
                                        Dim employeeleave26 = "INSERT INTO employeeleave (Code) VALUES('026')"
                                        cmd = New MySqlCommand(employeeleave26.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(27)"
                                        Dim employeeleave27 = "INSERT INTO employeeleave (Code) VALUES('027')"
                                        cmd = New MySqlCommand(employeeleave27.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(28)"
                                        Dim employeeleave28 = "INSERT INTO employeeleave (Code) VALUES('028')"
                                        cmd = New MySqlCommand(employeeleave28.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(29)"
                                        Dim employeeleave29 = "INSERT INTO employeeleave (Code) VALUES('029')"
                                        cmd = New MySqlCommand(employeeleave29.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(30)"
                                        Dim employeeleave30 = "INSERT INTO employeeleave (Code) VALUES('030')"
                                        cmd = New MySqlCommand(employeeleave30.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(31)"
                                        Dim employeeleave31 = "INSERT INTO employeeleave (Code) VALUES('031')"
                                        cmd = New MySqlCommand(employeeleave31.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(32)"
                                        Dim employeeleave32 = "INSERT INTO employeeleave (Code) VALUES('032')"
                                        cmd = New MySqlCommand(employeeleave32.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table Designation...(33)"
                                        Dim employeeleave33 = "INSERT INTO employeeleave (Code) VALUES('033')"
                                        cmd = New MySqlCommand(employeeleave33.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(34)"
                                        Dim employeeleave34 = "INSERT INTO employeeleave (Code) VALUES('034')"
                                        cmd = New MySqlCommand(employeeleave34.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(35)"
                                        Dim employeeleave35 = "INSERT INTO employeeleave (Code) VALUES('035')"
                                        cmd = New MySqlCommand(employeeleave35.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(36)"
                                        Dim employeeleave36 = "INSERT INTO employeeleave (Code) VALUES('036')"
                                        cmd = New MySqlCommand(employeeleave36.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(37)"
                                        Dim employeeleave37 = "INSERT INTO employeeleave (Code) VALUES('037')"
                                        cmd = New MySqlCommand(employeeleave37.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(38)"
                                        Dim employeeleave38 = "INSERT INTO employeeleave (Code) VALUES('038')"
                                        cmd = New MySqlCommand(employeeleave38.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(39)"
                                        Dim employeeleave39 = "INSERT INTO employeeleave (Code) VALUES('039')"
                                        cmd = New MySqlCommand(employeeleave39.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(40)"
                                        Dim employeeleave40 = "INSERT INTO employeeleave (Code) VALUES('040')"
                                        cmd = New MySqlCommand(employeeleave40.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(41)"
                                        Dim employeeleave41 = "INSERT INTO employeeleave (Code) VALUES('041')"
                                        cmd = New MySqlCommand(employeeleave41.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(42)"
                                        Dim employeeleave42 = "INSERT INTO employeeleave (Code) VALUES('042')"
                                        cmd = New MySqlCommand(employeeleave42.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(43)"
                                        Dim employeeleave43 = "INSERT INTO employeeleave (Code) VALUES('043')"
                                        cmd = New MySqlCommand(employeeleave43.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(44)"
                                        Dim employeeleave44 = "INSERT INTO employeeleave (Code) VALUES('044')"
                                        cmd = New MySqlCommand(employeeleave44.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(45)"
                                        Dim employeeleave45 = "INSERT INTO employeeleave (Code) VALUES('045')"
                                        cmd = New MySqlCommand(employeeleave45.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(46)"
                                        Dim employeeleave46 = "INSERT INTO employeeleave (Code) VALUES('046')"
                                        cmd = New MySqlCommand(employeeleave46.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(47)"
                                        Dim employeeleave47 = "INSERT INTO employeeleave (Code) VALUES('047')"
                                        cmd = New MySqlCommand(employeeleave47.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(48)"
                                        Dim employeeleave48 = "INSERT INTO employeeleave (Code) VALUES('048')"
                                        cmd = New MySqlCommand(employeeleave48.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(49)"
                                        Dim employeeleave49 = "INSERT INTO employeeleave (Code) VALUES('049')"
                                        cmd = New MySqlCommand(employeeleave49.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table employeeleave...(50)"
                                        Dim employeeleave50 = "INSERT INTO employeeleave (Code) VALUES('050')"
                                        cmd = New MySqlCommand(employeeleave50.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'INSERT INTO bank++++++++++++++++++++++++++++++++++
                                        Label3.Text = "Inserting into table bank...(1)"
                                        Dim bank1 = "INSERT INTO bank (Code) VALUES('001')"
                                        cmd = New MySqlCommand(bank1.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(2)"
                                        Dim bank2 = "INSERT INTO bank (Code) VALUES('002')"
                                        cmd = New MySqlCommand(bank2.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(3)"
                                        Dim bank3 = "INSERT INTO bank (Code) VALUES('003')"
                                        cmd = New MySqlCommand(bank3.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(4)"
                                        Dim bank4 = "INSERT INTO bank (Code) VALUES('004')"
                                        cmd = New MySqlCommand(bank4.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(5)"
                                        Dim bank5 = "INSERT INTO bank (Code) VALUES('005')"
                                        cmd = New MySqlCommand(bank5.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(6)"
                                        Dim bank6 = "INSERT INTO bank (Code) VALUES('006')"
                                        cmd = New MySqlCommand(bank6.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(7)"
                                        Dim bank7 = "INSERT INTO bank (Code) VALUES('007')"
                                        cmd = New MySqlCommand(bank7.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(8)"
                                        Dim bank8 = "INSERT INTO bank (Code) VALUES('008')"
                                        cmd = New MySqlCommand(bank8.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(9)"
                                        Dim bank9 = "INSERT INTO bank (Code) VALUES('009')"
                                        cmd = New MySqlCommand(bank9.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(10)"
                                        Dim bank10 = "INSERT INTO bank (Code) VALUES('010')"
                                        cmd = New MySqlCommand(bank10.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(11)"
                                        Dim bank11 = "INSERT INTO bank (Code) VALUES('011')"
                                        cmd = New MySqlCommand(bank11.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(12)"
                                        Dim bank12 = "INSERT INTO bank (Code) VALUES('012')"
                                        cmd = New MySqlCommand(bank12.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(13)"
                                        Dim bank13 = "INSERT INTO bank (Code) VALUES('013')"
                                        cmd = New MySqlCommand(bank13.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(14)"
                                        Dim bank14 = "INSERT INTO bank (Code) VALUES('014')"
                                        cmd = New MySqlCommand(bank14.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(15)"
                                        Dim bank15 = "INSERT INTO bank (Code) VALUES('015')"
                                        cmd = New MySqlCommand(bank15.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(16)"
                                        Dim bank16 = "INSERT INTO bank (Code) VALUES('016')"
                                        cmd = New MySqlCommand(bank16.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(17)"
                                        Dim bank17 = "INSERT INTO bank (Code) VALUES('017')"
                                        cmd = New MySqlCommand(bank17.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(18)"
                                        Dim bank18 = "INSERT INTO bank (Code) VALUES('018')"
                                        cmd = New MySqlCommand(bank18.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(19)"
                                        Dim bank19 = "INSERT INTO bank (Code) VALUES('019')"
                                        cmd = New MySqlCommand(bank19.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table bank...(20)"
                                        Dim bank20 = "INSERT INTO bank (Code) VALUES('020')"
                                        cmd = New MySqlCommand(bank20.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'INSERT INTO parameters++++++++++++++++++++++++++++++++++
                                        Label3.Text = "Inserting into parameters...(1)"
                                        Dim insertparameters1 = "INSERT INTO parameters (ID) VALUES('1')"
                                        cmd = New MySqlCommand(insertparameters1.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        'INSERT INTO hrdocuments++++++++++++++++++++++++++++++++++
                                        Label3.Text = "Inserting into table HR Documents...(1)"
                                        Dim hrdocuments1 = "INSERT INTO hrdocuments (Code,description,docname) VALUES('001','Leave','Leave')"
                                        cmd = New MySqlCommand(hrdocuments1.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(2)"
                                        Dim hrdocuments2 = "INSERT INTO hrdocuments (Code,description,docname) VALUES('002','Written Warning','WrittenWarning')"
                                        cmd = New MySqlCommand(hrdocuments2.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(3)"
                                        Dim hrdocuments3 = "INSERT INTO hrdocuments (Code,description,docname) VALUES('003','Resignation','Resignation')"
                                        cmd = New MySqlCommand(hrdocuments3.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(4)"
                                        Dim hrdocuments4 = "INSERT INTO hrdocuments (Code,description,docname) VALUES('004','Overtime','Overtime')"
                                        cmd = New MySqlCommand(hrdocuments4.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(5)"
                                        Dim hrdocuments5 = "INSERT INTO hrdocuments (Code,description,docname) VALUES('005','Contract','Contract')"
                                        cmd = New MySqlCommand(hrdocuments5.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(6)"
                                        Dim hrdocuments6 = "INSERT INTO hrdocuments (Code) VALUES('006')"
                                        cmd = New MySqlCommand(hrdocuments6.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(7)"
                                        Dim hrdocuments7 = "INSERT INTO hrdocuments (Code) VALUES('007')"
                                        cmd = New MySqlCommand(hrdocuments7.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(8)"
                                        Dim hrdocuments8 = "INSERT INTO hrdocuments (Code) VALUES('008')"
                                        cmd = New MySqlCommand(hrdocuments8.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(9)"
                                        Dim hrdocuments9 = "INSERT INTO hrdocuments (Code) VALUES('009')"
                                        cmd = New MySqlCommand(hrdocuments9.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(10)"
                                        Dim hrdocuments10 = "INSERT INTO hrdocuments (Code) VALUES('010')"
                                        cmd = New MySqlCommand(hrdocuments10.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(11)"
                                        Dim hrdocuments11 = "INSERT INTO hrdocuments (Code) VALUES('011')"
                                        cmd = New MySqlCommand(hrdocuments11.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(12)"
                                        Dim hrdocuments12 = "INSERT INTO hrdocuments (Code) VALUES('012')"
                                        cmd = New MySqlCommand(hrdocuments12.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(13)"
                                        Dim hrdocuments13 = "INSERT INTO hrdocuments (Code) VALUES('013')"
                                        cmd = New MySqlCommand(hrdocuments13.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(14)"
                                        Dim hrdocuments14 = "INSERT INTO hrdocuments (Code) VALUES('014')"
                                        cmd = New MySqlCommand(hrdocuments14.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(15)"
                                        Dim hrdocuments15 = "INSERT INTO hrdocuments (Code) VALUES('015')"
                                        cmd = New MySqlCommand(hrdocuments15.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(16)"
                                        Dim hrdocuments16 = "INSERT INTO hrdocuments (Code) VALUES('016')"
                                        cmd = New MySqlCommand(hrdocuments16.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(17)"
                                        Dim hrdocuments17 = "INSERT INTO hrdocuments (Code) VALUES('017')"
                                        cmd = New MySqlCommand(hrdocuments17.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(18)"
                                        Dim hrdocuments18 = "INSERT INTO hrdocuments (Code) VALUES('018')"
                                        cmd = New MySqlCommand(hrdocuments18.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(19)"
                                        Dim hrdocuments19 = "INSERT INTO hrdocuments (Code) VALUES('019')"
                                        cmd = New MySqlCommand(hrdocuments19.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Inserting into table HR Documents...(20)"
                                        Dim hrdocuments20 = "INSERT INTO hrdocuments (Code) VALUES('020')"
                                        cmd = New MySqlCommand(hrdocuments20.ToString, conn)
                                        cmd.ExecuteNonQuery()


                                        Label3.Text = "Creating table Views(1)..."
                                        Dim view1 = "CREATE VIEW employees AS 
                                                                SELECT e.*, m.description As 'Designation Description', d.Description As 'Department Description'
                                                                FROM employee e
                                                                LEFT JOIN departments d
                                                                ON e.department = d.Code
                                                                LEFT JOIN designation m
                                                                ON e.designation = m.Code;"
                                        cmd = New MySqlCommand(view1.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Creating table Views(2)..."
                                        Dim view2 = "CREATE VIEW companydetails AS 
                                                                SELECT *
                                                                FROM company;"
                                        cmd = New MySqlCommand(view2.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Creating table Views(3)..."
                                        Dim view3 = "CREATE VIEW department AS 
                                                    SELECT *
                                                    FROM departments WHERE Description <> '';"
                                        cmd = New MySqlCommand(view3.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Creating table Views(4)..."
                                        Dim view4 = "CREATE VIEW designations AS 
                                                    SELECT *
                                                    FROM designation WHERE Description <> '';"
                                        cmd = New MySqlCommand(view4.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Creating table Views(5)..."
                                        Dim view5 = "CREATE VIEW empleave AS 
                                                        SELECT *
                                                        FROM employeeleave WHERE Description <> '';"
                                        cmd = New MySqlCommand(view5.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Creating table Views(6)..."
                                        Dim view6 = "CREATE VIEW empleavehistory AS 
                                                        SELECT concat(e.first_name,' ', e.last_name) As 'Name', l.employee_code, l.leave_type, 
                                                        l.start_date, l.end_date, l.num_days
                                                        FROM leavehistory l, employee e 
                                                        where e.code = l.employee_code order by l.employee_code;"
                                        cmd = New MySqlCommand(view6.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Creating table Views(7)..."
                                        Dim view7 = "CREATE VIEW payslip AS
                                                        Select t.emp_code,t.datepaid,t.totaltax,t.uif,t.subsalary,t.otherincome,t.otherdedicincome,t.capitalgain,t.loans,
                                                        t.naspa,t.pension,t.riskbenefitper,t.riskbenefitamnt,t.retirementper,t.retirementamnt,t.medicalaid as 'emp_medical',t.pocketexpen,
                                                        t.overtimeamnt,t.leaveamnt,t.netsalary,t.period,e.code,e.title,e.first_name,e.last_name,e.id_number,e.passport_num,
                                                        e.initial,e.second_name,e.know_name,e.date_of_birth,e.passport_country,e.asylum_seeker,e.refugee,e.unit_num,
                                                        e.complex_name,e.street_num,e.street_farm_name,e.suburb_district,e.city_town,e.post_code,e.country,
                                                        e.default_phy_res_address,e.post_unit_num,e.post_complex_name,e.post_street_num,e.post_street_farm_name,
                                                        e.post_suburb_district,e.post_city_town,e.post_postal_code As 'postal_code',e.post_country,e.start_date,e.department,e.working_h_day,
                                                        e.working_d_week,e.avrg_working_h_month,e.avrg_working_d_month,e.annual_salary,e.fixed_salary,e.rate_per_day,
                                                        e.rate_per_hour,e.ImageFile,e.designation,e.email,e.phonenum,e.taxnum,e.riskbenefits,e.retirementfund,
                                                        e.medicalaid,e.pocketexpense,e.pension as 'email_pension',e.dependantnum,c.com_num,c.com_name,c.com_reg_num,
                                                        c.sdl_pay_ref_num,c.phy_unit_num,c.phy_complex_num,c.phy_street_num,c.phy_street_farm_name,c.phy_suburb_district,c.phy_city_town,
                                                        c.phy_postal_code,c.phy_country,c.ImageFile as 'com_image',c.revenue_autho
                                                        From employee e
                                                        Join transactions t
                                                        On e.code = t.emp_code
                                                        Join company c;"
                                        cmd = New MySqlCommand(view7.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Creating table Views(8)..."
                                        Dim view8 = "CREATE VIEW emploans AS 
                                                     SELECT code,title,first_name,last_name,loan
                                                     FROM employee WHERE loan <> 0.00;"
                                        cmd = New MySqlCommand(view8.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Creating table Views(9)..."
                                        Dim view9 = "CREATE VIEW emppensions AS 
                                                        SELECT e.code,e.title,e.first_name,e.last_name,(p.napsaper/100) * e.fixed_salary As 'Penson',p.napsa,p.napsaper,
                                                        CASE
                                                            WHEN (p.napsaper/100) * e.fixed_salary < napsa THEN (p.napsaper/100) * e.fixed_salary
                                                            ELSE napsa
                                                        END As 'PensonPaind'
                                                        FROM employee e, parameters p;"
                                        cmd = New MySqlCommand(view9.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        Label3.Text = "Creating table Views(10)..."
                                        Dim view10 = "CREATE VIEW vwpayslip AS 
                                                        SELECT t.emp_code AS emp_code, e.title AS title, e.first_name AS first_name, e.last_name AS last_name, e.id_number AS id_number,
                                                        e.passport_num AS passport_num, t.datepaid AS datepaid, t.totaltax AS totaltax, t.uif AS uif, t.subsalary AS subsalary,
                                                        t.otherincome AS otherincome, t.otherdedicincome AS otherdedicincome, t.capitalgain AS capitalgain, t.loans AS loans,
                                                        t.naspa AS naspa, t.pension AS pension, t.riskbenefitper AS riskbenefitper, t.riskbenefitamnt AS riskbenefitamnt, t.retirementper AS retirementper,
                                                        t.retirementamnt AS retirementamnt, t.medicalaid AS medicalaid, t.pocketexpen AS pocketexpen, t.overtimeamnt AS overtimeamnt,
                                                        t.leaveamnt AS leaveamnt, t.netsalary AS netsalary, t.period AS period, e.unit_num AS unit_num, e.complex_name AS complex_name,
                                                        e.street_num AS street_num, e.street_farm_name AS street_farm_name, e.suburb_district AS suburb_district, e.city_town AS city_town,
                                                        e.post_code AS post_code, e.country AS country, d.Description AS Department, ds.description AS Designation, e.email AS email,
                                                        e.phonenum AS phonenum, e.start_date AS start_date, c.com_name AS com_name, c.com_num AS com_num, c.com_reg_num AS com_reg_num,
                                                        c.ImageFile AS ImageFile, c.phy_city_town AS phy_city_town, c.phy_complex_num AS phy_complex_num, c.phy_country AS phy_country,
                                                        c.phy_postal_code AS phy_postal_code, c.phy_street_farm_name AS phy_street_farm_name, c.phy_street_num AS phy_street_num,
                                                        c.phy_suburb_district AS phy_suburb_district, c.phy_unit_num AS phy_unit_num
                                                        FROM transactions t
                                                        JOIN employee e ON t.emp_code = e.code
                                                        JOIN departments d ON e.department = d.Code
                                                        JOIN designation ds ON e.designation = ds.code
                                                        JOIN company c;"
                                        cmd = New MySqlCommand(view10.ToString, conn)
                                        cmd.ExecuteNonQuery()

                                        cmd.Dispose()
                                        conn.Close()
                                        Button2.Enabled = False
                                        Button1.Enabled = True
                                        Label3.Text = "Database created. Please Click Next to continue."
                                    Catch ex As Exception
                                        MessageBox.Show(ex.Message, "Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Close()
                                    Finally
                                        conn.Dispose()
                                    End Try
                                End If
                            End With
                        Catch ex As Exception
                            MessageBox.Show(ex.Message, "Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Finally
                            conn.Dispose()
                        End Try
                        'End If
                    End If
                Else
                    MsgBox("Unable to connect!", MsgBoxStyle.Critical, "Mysql connection")
                End If
            End With
        ElseIf RadioButtonCopy.Checked = True Then
            MsgBox("Copy Another Company. Not yet written!")
        Else
            MsgBox("Use Setup Assistant. Not yet written!")
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim data As New Database
        With data
            'Assing the object property values
            .ServerName = txt_server.Text
            .UserID = txt_uid.Text
            .Password = txt_pwd.Text
            .Port = txt_port.Text

            If .Connection Then
                MsgBox("Database Successfully Conneted.", MsgBoxStyle.Information, "Database connection")
            Else
                MsgBox("Unable to connect!", MsgBoxStyle.Critical, "Database connection")
            End If
        End With
        txtName.Select()
    End Sub

    Dim path1 As String
    Private Sub NewCompany_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MainInterface.Activate()
        'STARTUP.Hide()

        txtName.Enabled = True
        txt_pwd.Enabled = True
        txt_server.Enabled = True
        txt_uid.Enabled = True
        txtName.Clear()
        Try
            Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
            Using sr As New System.IO.StreamReader(appPath + "\Settings.ini")
                Dim Line As String = sr.ReadLine
                Dim Line1 As String = sr.ReadLine
                Dim Line2 As String = sr.ReadLine
                Dim Line3 As String = sr.ReadLine
                Dim Line4 As String = sr.ReadLine

                txt_server.Text = Line.Substring(11)
                txt_uid.Text = Line1.Substring(11)
                'txt_pwd.Text = Line2.Substring(11)
                Dim wrapper As New Simple3Des("12345")
                Dim plainText As String = wrapper.DecryptData(Line2.Substring(11))
                txt_pwd.Text = plainText
                txt_port.Text = Line4.Substring(11)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Control.CheckForIllegalCrossThreadCalls = False

        Button1.Enabled = False
        txtName.Focus()
        txtName.Select()
    End Sub

    Private Sub MyWorkMethod()
        DataBase()

        Threading.Thread.Sleep(10000)
        Dim t As New Threading.Thread(AddressOf MyWorkMethod)
        t.Abort()
    End Sub

    Private Delegate Sub ConsumeData_Delegate(ByVal msg As String)

    Private Sub ConsumeData(ByVal msg As String)
        Try
            If InvokeRequired Then
                Dim delg As New ConsumeData_Delegate(AddressOf ConsumeData)
                Dim obj(0) As String
                obj(0) = msg
                Me.Invoke(delg, obj)
            Else
                Me.Text = msg
                MsgBox(msg)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Button1.Enabled = True
        End Try
    End Sub

    Public server1 As String
    Public uid1 As String
    Public pass1 As String
    Public database1 As String
    Public port As String

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        Label1.Select()
        Button2.Enabled = False
        server1 = txt_server.Text
        uid1 = txt_uid.Text
        pass1 = txt_pwd.Text
        database1 = txtName.Text
        port = txt_port.Text

        Try
            'Label6.Visible = True
            'Label6.Image = My.Resources.loader
            'If txtName.Text <> "" Then
            txtName.Enabled = False
            txt_pwd.Enabled = False
            txt_server.Enabled = False
            txt_uid.Enabled = False
            txt_port.Enabled = False
            Me.Button1.Enabled = False
            'GroupBox1.Enabled = False
            Button4.Enabled = False
            Dim t As New Threading.Thread(AddressOf MyWorkMethod)
            t.IsBackground = True
            t.SetApartmentState(ApartmentState.STA)
            t.Start()
        Catch ex As Exception
            MsgBox(ex.Message)
            Me.Button1.Enabled = True
        End Try
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Select()
        server1 = txt_server.Text
        uid1 = txt_uid.Text
        pass1 = txt_pwd.Text
        database1 = txtName.Text
        port = txt_port.Text

        setupinterview.ShowDialog()
    End Sub

    Private Sub Button3_Click_2(sender As Object, e As EventArgs) Handles Button3.Click
        Label1.Select()
        Close()
        Panel2.Select()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Close()
    End Sub

    Private Sub RadioButtonNew_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonNew.CheckedChanged
        ComboBox1.Text = ""
        ComboBox1.Enabled = False
    End Sub

    Private Sub RadioButtonCopy_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonCopy.CheckedChanged
        ComboBox1.Enabled = True
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)

    End Sub
End Class