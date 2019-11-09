Option Explicit On
Imports System.Runtime.InteropServices
Imports System.IO
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.Drawing.Drawing2D
Public Class Employee
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager
    Dim dataadapter As New MySqlDataAdapter("select * from employee", cn)

    Const WM_CAP As Short = &H400S
    Const WM_CAP_DRIVER_CONNECT As Integer = WM_CAP + 10
    Const WM_CAP_DRIVER_DISCONNECT As Integer = WM_CAP + 11
    Const WM_CAP_EDIT_COPY As Integer = WM_CAP + 30
    Public Const WM_CAP_GET_STATUS As Integer = WM_CAP + 54
    Public Const WM_CAP_DLG_VIDEOFORMAT As Integer = WM_CAP + 41
    Const WM_CAP_SET_PREVIEW As Integer = WM_CAP + 50
    Const WM_CAP_SET_PREVIEWRATE As Integer = WM_CAP + 52
    Const WM_CAP_SET_SCALE As Integer = WM_CAP + 53
    Const WS_CHILD As Integer = &H40000000
    Const WS_VISIBLE As Integer = &H10000000
    Const SWP_NOMOVE As Short = &H2S
    Const SWP_NOSIZE As Short = 1
    Const SWP_NOZORDER As Short = &H4S
    Const HWND_BOTTOM As Short = 1
    Private DeviceID As Integer = 0 ' Current device ID
    Private hHwnd As Integer ' Handle to preview window

    Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
        (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer,
        ByRef lParam As CAPSTATUS) As Boolean


    Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
       (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Boolean,
       ByRef lParam As Integer) As Boolean


    Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
         (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer,
         ByRef lParam As Integer) As Boolean


    Declare Function SetWindowPos Lib "user32" Alias "SetWindowPos" (ByVal hwnd As Integer,
        ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer,
        ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer

    Declare Function DestroyWindow Lib "user32" (ByVal hndw As Integer) As Boolean
    Structure POINTAPI
        Dim x As Integer
        Dim y As Integer
    End Structure
    Public Structure CAPSTATUS
        Dim uiImageWidth As Integer                    '// Width of the image
        Dim uiImageHeight As Integer                   '// Height of the image
        Dim fLiveWindow As Integer                     '// Now Previewing video?
        Dim fOverlayWindow As Integer                  '// Now Overlaying video?
        Dim fScale As Integer                          '// Scale image to client?
        Dim ptScroll As POINTAPI                    '// Scroll position
        Dim fUsingDefaultPalette As Integer            '// Using default driver palette?
        Dim fAudioHardware As Integer                  '// Audio hardware present?
        Dim fCapFileExists As Integer                  '// Does capture file exist?
        Dim dwCurrentVideoFrame As Integer             '// # of video frames cap'td
        Dim dwCurrentVideoFramesDropped As Integer     '// # of video frames dropped
        Dim dwCurrentWaveSamples As Integer            '// # of wave samples cap'td
        Dim dwCurrentTimeElapsedMS As Integer          '// Elapsed capture duration
        Dim hPalCurrent As Integer                     '// Current palette in use
        Dim fCapturingNow As Integer                   '// Capture in progress?
        Dim dwReturn As Integer                        '// Error value after any operation
        Dim wNumVideoAllocated As Integer              '// Actual number of video buffers
        Dim wNumAudioAllocated As Integer              '// Actual number of audio buffers
    End Structure
    Declare Function capCreateCaptureWindowA Lib "avicap32.dll" _
         (ByVal lpszWindowName As String, ByVal dwStyle As Integer,
         ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer,
         ByVal nHeight As Short, ByVal hWndParent As Integer,
         ByVal nID As Integer) As Integer
    Declare Function capGetDriverDescriptionA Lib "avicap32.dll" (ByVal wDriver As Short,
        ByVal lpszName As String, ByVal cbName As Integer, ByVal lpszVer As String,
        ByVal cbVer As Integer) As Boolean

    Private Sub LoadDeviceList()
        Dim strName As String = Space(100)
        Dim strVer As String = Space(100)
        Dim bReturn As Boolean
        Dim x As Short = 0
        ' 
        ' Load name of all avialable devices into the lstDevices
        '
        Do
            '
            '   Get Driver name and version
            '
            bReturn = capGetDriverDescriptionA(x, strName, 100, strVer, 100)
            '
            ' If there was a device add device name to the list
            '
            If bReturn Then lstDevices.Items.Add(strName.Trim)
            x += CType(1, Short)
        Loop Until bReturn = False
    End Sub

    Private Sub OpenPreviewWindow()
        Dim iHeight As Integer = PictureBox1.Height
        Dim iWidth As Integer = PictureBox1.Width
        '
        ' Open Preview window in picturebox
        '
        hHwnd = capCreateCaptureWindowA(DeviceID.ToString, WS_VISIBLE Or WS_CHILD, 0, 0, 1280,
            1024, PictureBox1.Handle.ToInt32, 0)
        '
        ' Connect to device
        '
        If SendMessage(hHwnd, WM_CAP_DRIVER_CONNECT, DeviceID, 0) Then
            '
            'Set the preview scale
            '
            SendMessage(hHwnd, WM_CAP_SET_SCALE, True, 0)

            '
            'Set the preview rate in milliseconds
            '
            SendMessage(hHwnd, WM_CAP_SET_PREVIEWRATE, 66, 0)

            '
            'Start previewing the image from the camera
            '
            SendMessage(hHwnd, WM_CAP_SET_PREVIEW, True, 0)

            '
            ' Resize window to fit in picturebox
            '
            SetWindowPos(hHwnd, HWND_BOTTOM, 0, 0, PictureBox1.Width, PictureBox1.Height,
                    SWP_NOMOVE Or SWP_NOZORDER)

            'btnSave.Enabled = True
            'btnStop.Enabled = True
            'btnStart.Enabled = False
            'btnInfo.Enabled = True
        Else
            '
            ' Error connecting to device close window
            ' 
            DestroyWindow(hHwnd)
            'btnSave.Enabled = False
        End If
    End Sub

    Private Sub ClosePreviewWindow()
        '
        ' Disconnect from device
        '
        SendMessage(hHwnd, WM_CAP_DRIVER_DISCONNECT, DeviceID, 0)
        '
        ' close window
        '
        DestroyWindow(hHwnd)
    End Sub

    <DllImport("Gdi32.dll", EntryPoint:="CreateRoundRectRgn")>
    Private Shared Function CreateRoundRectRgn(ByVal iLeft As Integer, ByVal iTop As Integer, ByVal iRight As Integer, ByVal iBottom As Integer, ByVal iWidth As Integer, ByVal iHeight As Integer) As IntPtr
    End Function

    Public Sub filldatasetandview()
        Try
            ds = New DataSet
            dataadapter.Fill(ds, "employee")
            dv = New DataView(ds.Tables("employee"))
            cm = CType(Me.BindingContext(dv), CurrencyManager)
            cn.Close()
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Employee", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Public Sub bindfields()
        code.DataBindings.Clear()
        title.DataBindings.Clear()
        first_name.DataBindings.Clear()
        last_name.DataBindings.Clear()
        id_number.DataBindings.Clear()
        passport_num.DataBindings.Clear()
        initial.DataBindings.Clear()
        second_name.DataBindings.Clear()
        know_name.DataBindings.Clear()
        date_of_birth.DataBindings.Clear()
        Combo_passport_country.DataBindings.Clear()
        unit_num.DataBindings.Clear()
        complex_name.DataBindings.Clear()
        street_num.DataBindings.Clear()
        street_farm_name.DataBindings.Clear()
        suburb_district.DataBindings.Clear()
        city_town.DataBindings.Clear()
        post_code.DataBindings.Clear()
        Combo_country.DataBindings.Clear()
        post_unit_num.DataBindings.Clear()
        post_complex_name.DataBindings.Clear()
        post_street_num.DataBindings.Clear()
        post_street_farm_name.DataBindings.Clear()
        post_suburb_district.DataBindings.Clear()
        post_city_town.DataBindings.Clear()
        post_postal_code.DataBindings.Clear()
        Combo_post_country.DataBindings.Clear()
        Combo_pay_with.DataBindings.Clear()
        Combo_acc_type.DataBindings.Clear()
        Combo_bank.DataBindings.Clear()
        branch_code.DataBindings.Clear()
        acc_num.DataBindings.Clear()
        acc_holder_name.DataBindings.Clear()
        other_bank.DataBindings.Clear()
        branch_name.DataBindings.Clear()
        Combo_account_holder_rel.DataBindings.Clear()
        start_date.DataBindings.Clear()
        start_as.DataBindings.Clear()
        Combo_department.DataBindings.Clear()
        working_h_day.DataBindings.Clear()
        working_d_week.DataBindings.Clear()
        avrg_working_h_month.DataBindings.Clear()
        avrg_working_d_month.DataBindings.Clear()
        annual_salary.DataBindings.Clear()
        fixed_salary.DataBindings.Clear()
        rate_per_day.DataBindings.Clear()
        rate_per_hour.DataBindings.Clear()
        txtEmail.DataBindings.Clear()
        txtPhone.DataBindings.Clear()
        txtTaxNum.DataBindings.Clear()

        Check_asylum_seeker.Checked = False
        Check_refugee.Checked = False
        Check_default_phy_res_address.Checked = False

        Try
            code.DataBindings.Add("text", dv, "code").ToString()
            title.DataBindings.Add("text", dv, "title").ToString()
            first_name.DataBindings.Add("text", dv, "first_name").ToString()
            last_name.DataBindings.Add("text", dv, "last_name").ToString()
            id_number.DataBindings.Add("text", dv, "id_number").ToString()
            passport_num.DataBindings.Add("text", dv, "passport_num").ToString()
            initial.DataBindings.Add("text", dv, "initial").ToString()
            second_name.DataBindings.Add("text", dv, "second_name").ToString()
            know_name.DataBindings.Add("text", dv, "know_name").ToString()
            date_of_birth.DataBindings.Add("text", dv, "date_of_birth").ToString()
            Combo_passport_country.DataBindings.Add("text", dv, "passport_country").ToString()
            unit_num.DataBindings.Add("text", dv, "unit_num").ToString()
            complex_name.DataBindings.Add("text", dv, "complex_name").ToString()
            street_num.DataBindings.Add("text", dv, "street_num").ToString()
            street_farm_name.DataBindings.Add("text", dv, "street_farm_name").ToString()
            suburb_district.DataBindings.Add("text", dv, "suburb_district").ToString()
            city_town.DataBindings.Add("text", dv, "city_town").ToString()
            post_code.DataBindings.Add("text", dv, "post_code").ToString()
            Combo_country.DataBindings.Add("text", dv, "country").ToString()
            post_unit_num.DataBindings.Add("text", dv, "post_unit_num").ToString()
            post_complex_name.DataBindings.Add("text", dv, "post_complex_name").ToString()
            post_street_num.DataBindings.Add("text", dv, "post_street_num").ToString()
            post_street_farm_name.DataBindings.Add("text", dv, "post_street_farm_name").ToString()
            post_suburb_district.DataBindings.Add("text", dv, "post_suburb_district").ToString()
            post_city_town.DataBindings.Add("text", dv, "post_city_town").ToString()
            post_postal_code.DataBindings.Add("text", dv, "post_postal_code").ToString()
            Combo_post_country.DataBindings.Add("text", dv, "post_country").ToString()
            Combo_pay_with.DataBindings.Add("text", dv, "pay_with").ToString()
            Combo_acc_type.DataBindings.Add("text", dv, "acc_type").ToString()
            Combo_bank.DataBindings.Add("text", dv, "bank").ToString()
            branch_code.DataBindings.Add("text", dv, "branch_code").ToString()
            acc_num.DataBindings.Add("text", dv, "acc_num").ToString()
            acc_holder_name.DataBindings.Add("text", dv, "acc_holder_name").ToString()
            other_bank.DataBindings.Add("text", dv, "other_bank").ToString()
            branch_name.DataBindings.Add("text", dv, "branch_name").ToString()
            Combo_account_holder_rel.DataBindings.Add("text", dv, "account_holder_rel").ToString()
            start_date.DataBindings.Add("text", dv, "start_date")
            start_as.DataBindings.Add("text", dv, "start_as")
            Combo_department.DataBindings.Add("text", dv, "department")
            working_h_day.DataBindings.Add("text", dv, "working_h_day")
            working_d_week.DataBindings.Add("text", dv, "working_d_week")
            avrg_working_h_month.DataBindings.Add("text", dv, "avrg_working_h_month")
            avrg_working_d_month.DataBindings.Add("text", dv, "avrg_working_d_month")
            'annual_salary.DataBindings.Add("text", dv, "annual_salary")
            'fixed_salary.DataBindings.Add("text", dv, "fixed_salary")
            'rate_per_day.DataBindings.Add("text", dv, "rate_per_day")
            'rate_per_hour.DataBindings.Add("text", dv, "rate_per_hour")
            txtEmail.DataBindings.Add("text", dv, "email")
            txtPhone.DataBindings.Add("text", dv, "phonenum")
            txtTaxNum.DataBindings.Add("text", dv, "taxnum")
            ComboPayType.DataBindings.Add("text", dv, "paybasis")
            txtPensionCont.DataBindings.Add("text", dv, "pension")
            txtRiskPer.DataBindings.Add("text", dv, "riskbenefits")
            txtRetirementPer.DataBindings.Add("text", dv, "retirementfund")
            txtMedicalAid.DataBindings.Add("text", dv, "medicalaid")
            txtOutofPocket.DataBindings.Add("text", dv, "pocketexpense")
            txtNoofDependants.DataBindings.Add("text", dv, "dependantnum")
            cn.Close()
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Employee", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim Calc As Char
    Dim desfile As String = "002"
    Private Sub OtherData()
        Try
            PictureBox1.Image = Nothing
            Dim arrImage() As Byte
            cn.Open()
            Combo_department.Items.Clear()
            Dim Namequery As String = "SELECT asylum_seeker, refugee, default_phy_res_address, Caption, ImageFile FROM employee WHERE code = '" & code.Text & "'"
            cmd = New MySqlCommand(Namequery, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                asylum_seeker_val = dr("asylum_seeker").ToString()
                refugee_val = dr("refugee").ToString()
                default_phy_res_address_val = dr("default_phy_res_address").ToString()
            End While
            cn.Close()

            da.SelectCommand = cmd
            da.Fill(dt)

            'txtCaption.Text = dt.Rows(0).Item(0)
            arrImage = dt.Rows(0).Item(4)
            Dim mstream As New System.IO.MemoryStream(arrImage)
            PictureBox1.Image = Image.FromStream(mstream)

            If asylum_seeker_val = "Y" Then
                Check_asylum_seeker.Checked = True
            Else
                Check_asylum_seeker.Checked = False
            End If

            If refugee_val = "Y" Then
                Check_refugee.Checked = True
            Else
                Check_refugee.Checked = False
            End If

            If default_phy_res_address_val = "Y" Then
                Check_default_phy_res_address.Checked = True
            Else
                Check_default_phy_res_address.Checked = False
            End If
            cn.Close()

            cn.Open()
            Dim paramsCheck As String = "SELECT Calc FROM parameters"
            cmd = New MySqlCommand(paramsCheck, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Calc = dr("Calc").ToString()
            End While

            If Calc = "A" Then
                annual_salary.Enabled = True
                fixed_salary.Enabled = False
                rate_per_day.Enabled = False
                rate_per_hour.Enabled = False
            ElseIf Calc = "M" Then
                annual_salary.Enabled = False
                fixed_salary.Enabled = True
                rate_per_day.Enabled = False
                rate_per_hour.Enabled = False
            ElseIf Calc = "D" Then
                annual_salary.Enabled = False
                fixed_salary.Enabled = False
                rate_per_day.Enabled = True
                rate_per_hour.Enabled = False
            ElseIf Calc = "H" Then
                annual_salary.Enabled = False
                fixed_salary.Enabled = False
                rate_per_day.Enabled = False
                rate_per_hour.Enabled = True
            End If
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employee6", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub department()
        Try
            cn.Open()
            Combo_department.Items.Clear()
            Dim Namequery As String = "SELECT d.Description As 'Description' FROM departments d, employee e WHERE d.Code = e.Code AND e.code = '" & code.Text & "'"
            cmd = New MySqlCommand(Namequery, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Combo_department.Text = dr("Description").ToString()
            End While
            cn.Close()

            cn.Open()
            Combodesignation.Items.Clear()
            Dim Namequery1 As String = "SELECT d.description As 'Description' FROM designation d, employee e WHERE d.code = e.designation AND e.code = '" & code.Text & "'"
            cmd = New MySqlCommand(Namequery1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Combodesignation.Text = dr("Description").ToString()
            End While
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employee5", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub LoadData()
        'Try
        '    cn.Close()
        '    cn.Open()
        '    Combo_bank.Items.Clear()
        '    Dim bankquery As String = "SELECT * FROM bank WHERE Description != ''"
        '    cmd = New MySqlCommand(bankquery, cn)
        '    dr = cmd.ExecuteReader
        '    While dr.Read
        '        Dim sInvet = dr.GetString("Description").ToString
        '        Combo_bank.Items.Add(sInvet)
        '    End While
        '    cn.Close()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "Department", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'Finally
        '    cn.Dispose()
        'End Try
    End Sub

    Sub transactionFile()
        Try
            Dim dt1 As New DataTable
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT t.emp_code As 'Code', concat(e.first_name, ' ' ,e.last_name) As 'Full Name',t.datepaid As 'Date', t.subsalary As 'Salary', t.totaltax As 'Tax',t.uif As 'UIF',
                                (t.otherincome + t.otherdedicincome) As 'Other Income',
                                t.naspa 'NASPA',t.pension As 'Pension',t.riskbenefitamnt As 'Risk Benefit',
                                t.retirementamnt As 'Retirement' 
                                FROM transactions t, employee e WHERE t.emp_code = e.code AND t.emp_code =  '" & code.Text & "'"
            End With
            da.SelectCommand = cmd
            dt1.Clear()
            da.Fill(dt1)
            DataGridView2.DataSource = dt1
            cn.Close()

            DataGridView2.Columns(0).Width = 100
            DataGridView2.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            DataGridView2.Columns(2).Width = 110
            DataGridView2.Columns(3).Width = 110
            DataGridView2.Columns(4).Width = 110
            DataGridView2.Columns(5).Width = 110
            DataGridView2.Columns(6).Width = 130
            DataGridView2.Columns(7).Width = 110
            DataGridView2.Columns(8).Width = 140
            DataGridView2.Columns(9).Width = 120
            'DataGridView2.Columns(9).Width = 100
            DataGridView2.Columns(10).Width = 100

            Dim currencyCellStyle As New DataGridViewCellStyle
            currencyCellStyle.Format = "C2"

            With Me.DataGridView2
                .Columns(3).DefaultCellStyle = currencyCellStyle
                .Columns(4).DefaultCellStyle = currencyCellStyle
                .Columns(5).DefaultCellStyle = currencyCellStyle
                .Columns(6).DefaultCellStyle = currencyCellStyle
                .Columns(7).DefaultCellStyle = currencyCellStyle
                .Columns(8).DefaultCellStyle = currencyCellStyle
                .Columns(9).DefaultCellStyle = currencyCellStyle
                .Columns(10).DefaultCellStyle = currencyCellStyle
            End With
            DataGridView2.RowTemplate.Height = 35
            DataGridView2.Columns(0).DefaultCellStyle.Format = "yyyy-MM-dd"
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employee", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub leaveHistory()
        Try
            Dim dt1 As New DataTable
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT leave_type As 'Description', start_date As 'From', end_date As 'To', num_days As 'Days' FROM leavehistory WHERE employee_code = '" & code.Text & "'"
            End With
            da.SelectCommand = cmd
            dt1.Clear()
            da.Fill(dt1)
            DataGridView1.DataSource = dt1
            cn.Close()

            DataGridView1.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            DataGridView1.Columns(1).Width = 200
            DataGridView1.Columns(2).Width = 200
            DataGridView1.Columns(3).Width = 200

            DataGridView1.RowTemplate.Height = 35
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employee", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub Employeess()
        Try
            Dim dt1 As New DataTable
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT code As 'Code', first_name As 'Name' FROM employee"
            End With
            da.SelectCommand = cmd
            dt1.Clear()
            da.Fill(dt1)
            'DataGridView3.DataSource = dt1
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employee", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub Depart()
        Try
            cn.Open()
            Combo_department.Items.Clear()
            Dim Namequery As String = "SELECT Code,Description FROM departments WHERE Description != ''"
            cmd = New MySqlCommand(Namequery, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim sInvet = dr.GetString("Description").ToString
                Combo_department.Items.Add(sInvet)
            End While
            cn.Close()

            cn.Open()
            Combodesignation.Items.Clear()
            Dim Namequery1 As String = "SELECT code,description FROM designation WHERE description != ''"
            cmd = New MySqlCommand(Namequery1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim sInvet = dr.GetString("Description").ToString
                Combodesignation.Items.Add(sInvet)
            End While
            cn.Close()

            cn.Open()
            Dim Namequery2 As String = "SELECT Description FROM departments d, employee e WHERE d.Code = e.department AND e.code = '" & code.Text & "'"
            cmd = New MySqlCommand(Namequery2, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Combo_department.Text = dr.GetString("Description").ToString
            End While
            cn.Close()

            cn.Open()
            Dim Namequery3 As String = "SELECT Description FROM designation d, employee e WHERE d.Code = e.designation AND e.code = '" & code.Text & "'"
            cmd = New MySqlCommand(Namequery3, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Combodesignation.Text = dr.GetString("Description").ToString
            End While
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employee", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub ChangeFormat()
        Label31.AutoSize = False
        Label31.Height = 30
        Label31.Width = 140
        Label31.Padding = New Padding(1, 1, 1, 1)
        Label31.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label31.Width - 2, Label31.Height - 2, 5, 1))

        Label66.AutoSize = False
        Label66.Padding = New Padding(1, 1, 1, 1)
        Label66.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label66.Width - 2, Label66.Height - 2, 5, 1))

        Label12.AutoSize = False
        Label12.Padding = New Padding(1, 1, 1, 1)
        Label12.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label12.Width - 2, Label12.Height - 2, 5, 1))

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

        Label77.AutoSize = False
        Label77.Padding = New Padding(1, 1, 1, 1)
        Label77.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label77.Width - 2, Label77.Height - 2, 5, 1))

        Label58.AutoSize = False
        Label58.Padding = New Padding(1, 1, 1, 1)
        Label58.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label58.Width - 2, Label58.Height - 2, 5, 1))

        Label75.AutoSize = False
        Label75.Padding = New Padding(1, 1, 1, 1)
        Label75.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label75.Width - 2, Label75.Height - 2, 5, 1))

        Label70.AutoSize = False
        Label70.Padding = New Padding(1, 1, 1, 1)
        Label70.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label70.Width - 2, Label70.Height - 2, 5, 1))

        Label40.AutoSize = False
        Label40.Padding = New Padding(1, 1, 1, 1)
        Label40.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 2, Label40.Width - 2, Label40.Height - 2, 5, 1))

        Panel3.Visible = True
        Panel7.Visible = False
        Panel10.Visible = False
        Panel5.Visible = False
        Panel4.Visible = False
        Panel9.Visible = False

        Panel3.Location = New Point(0, 41)
        Panel3.Dock = DockStyle.Fill
    End Sub

    Sub FileLoad()
        Try
            Depart()
            Panel2.AutoScroll = True
            code.Select()
            filldatasetandview()
            bindfields()
            Label9.Select()
            imageRet1()
            code.ReadOnly = True
            PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
            Dim gp As New Drawing.Drawing2D.GraphicsPath
            gp.AddEllipse(0, 0, PictureBox1.Width, PictureBox1.Height)
            PictureBox1.Region = New Region(gp)

            leaveHistory()
            department()
            Depart()
            leaveHistory()
            TotalSearch()
            formatNumbers()
            transactionFile()
            SalaryLoad()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employee", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub formatNumbers()
        working_h_day.Text = FormatNumber(working_h_day.Text, 0)
        working_d_week.Text = FormatNumber(working_d_week.Text, 0)
        avrg_working_h_month.Text = FormatNumber(avrg_working_h_month.Text, 0)
        avrg_working_d_month.Text = FormatNumber(avrg_working_d_month.Text, 0)

        txtPensionCont.Text = FormatCurrency(txtPensionCont.Text, 2)
        txtRiskPer.Text = FormatNumber(txtRiskPer.Text, 2)
        txtRetirementPer.Text = FormatNumber(txtRetirementPer.Text, 2)
        txtMedicalAid.Text = FormatCurrency(txtMedicalAid.Text, 2)
        txtOutofPocket.Text = FormatCurrency(txtOutofPocket.Text, 2)
        txtNoofDependants.Text = FormatNumber(txtNoofDependants.Text, 0)

        ClosePreviewWindow()
    End Sub

    Public Sub SalaryLoad()
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT * FROM employee WHERE code = '" & code.Text & "'"
            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                annual_salary.Text = FormatCurrency(dr("annual_salary").ToString(), 2)
                fixed_salary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                rate_per_day.Text = FormatCurrency(dr("rate_per_day").ToString(), 2)
                rate_per_hour.Text = FormatCurrency(dr("rate_per_hour").ToString(), 2)
            End While
            cn.Close()
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim second As Integer
    Private Sub Employee_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Panel3.AutoScroll = True
        Panel4.AutoScroll = True
        Panel5.AutoScroll = True
        Panel7.AutoScroll = True
        Panel10.AutoScroll = True
        Panel9.AutoScroll = True
        Label70.Text = "Start Camera"
        Timer1.Interval = 10
        Timer1.Start() 'Timer starts functioning

        LoadDeviceList()
        If lstDevices.Items.Count > 0 Then
            lstDevices.SelectedIndex = 0
        Else
            lstDevices.Items.Add("No Capture Device")
        End If
        'Me.AutoScrollMinSize = New Size(100, 100)
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        Dim gp As New Drawing.Drawing2D.GraphicsPath
        gp.AddEllipse(0, 0, PictureBox1.Width, PictureBox1.Height)
        PictureBox1.Region = New Region(gp)
        'PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        Formats()
        formatNumbers()

        Panel3.Visible = True
        Panel4.Visible = False
        Panel5.Visible = False
        Panel7.Visible = False
        Panel10.Visible = False
        Panel9.Visible = False
        Panel3.Dock = DockStyle.Fill

        Label69.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label65.BackColor = Color.Red
        Label68.BackColor = Color.Lime
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime
        Button4.Visible = False
        ChangeFormat()
    End Sub

    Private Sub MakeRoundedImage(ByVal Img As Image, ByVal PicBox As PictureBox)
        Using bm As New Bitmap(Img.Width, Img.Height)
            Using grx2 As Graphics = Graphics.FromImage(bm)
                grx2.SmoothingMode = SmoothingMode.AntiAlias
                Using tb As New TextureBrush(Img)
                    tb.TranslateTransform(0, 0)
                    Using gp As New GraphicsPath
                        gp.AddEllipse(0, 0, Img.Width, Img.Height)
                        grx2.FillPath(tb, gp)
                    End Using
                End Using
            End Using
            If PicBox.Image IsNot Nothing Then PicBox.Image.Dispose()
            PicBox.Image = New Bitmap(bm)
        End Using
    End Sub

    Dim Itemexist As Char
    Dim TotalNumber As String
    Private Sub CheckItem()
        Try
            cn.Open()
            Dim Query As String
            Query = "SELECT code FROM employee WHERE code = '" & Trim(code.Text) & "'"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                Itemexist = "Y"
            Else
                Itemexist = "N"
            End If
            cn.Close()

            If Itemexist = "Y" Then
                EmployeeUpdate()
            ElseIf Itemexist = "N" Then
                EmployeeSave()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private forward As Boolean
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Label1.Select()

        'If code.Text = String.Empty Then
        '    MessageBox.Show("Please enter your employee code!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '    code.Select()
        'ElseIf title.SelectedIndex = -1 Then
        '    MessageBox.Show("Please select the title of the employee!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '    title.Select()
        'ElseIf first_name.Text = String.Empty Then
        '    MessageBox.Show("Please enter the name of the employee!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '    first_name.Select()
        'ElseIf last_name.Text = String.Empty Then
        '    MessageBox.Show("Please enter the last name of the employee!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '    last_name.Select()
        'ElseIf txtPhone.Text = String.Empty Then
        '    MessageBox.Show("Please enter the phone number of the employee!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '    txtPhone.Select()
        'ElseIf Combo_passport_country.SelectedIndex = -1 And passport_num.Text <> String.Empty Then

        'ElseIf Combo_country.SelectedIndex = -1 Then
        '    MessageBox.Show("Please Select the country!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '    Combo_country.Select()
        'ElseIf Combo_post_country.SelectedIndex = -1 Then
        '    MessageBox.Show("Please Select the postal country!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        '    Combo_post_country.Select()
        'ElseIf String.IsNullOrEmpty(id_number.Text) And String.IsNullOrEmpty(passport_num.Text) Then
        '    MessageBox.Show("Please enter Employee ID Or Passport!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        '    id_number.Select()
        'ElseIf id_number.Text <> String.Empty And passport_num.Text <> String.Empty Then
        '    MessageBox.Show("Employee cannot use ID And Passport at the sametime.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        '    id_number.SelectAll()
        '    passport_num.Clear()
        '    'If Combo_passport_country.SelectedIndex = -1 Then
        '    '    MessageBox.Show("Please Select the country!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '    'End If
        'ElseIf code.Text = String.Empty Then
        '    MessageBox.Show("Please enter employee code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        '    code.Select()
        'ElseIf title.Text = String.Empty Then
        '    MessageBox.Show("Please enter employee code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        '    title.Select()
        'ElseIf first_name.Text = String.Empty Then
        '    MessageBox.Show("Please enter employee first name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        '    first_name.Select()
        'ElseIf last_name.Text = String.Empty Then
        '    MessageBox.Show("Please enter employee code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        '    last_name.Select()
        'ElseIf code.Text = String.Empty Then
        '    MessageBox.Show("Please enter employee code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        '    code.Select()

        'Else

        'End If
        CheckItem()
    End Sub

    Private Sub clear()
        code.Enabled = True
        code.ReadOnly = False
        PictureBox1.Image = My.Resources.pic
        code.Clear()
        title.SelectedIndex = -1
        first_name.Clear()
        last_name.Clear()
        id_number.Clear()
        passport_num.Clear()
        initial.Clear()
        second_name.Clear()
        know_name.Clear()
        date_of_birth.Text = Today.Date
        Combo_passport_country.SelectedIndex = -1
        Combodesignation.SelectedIndex = -1
        Check_asylum_seeker.Checked = False
        Check_refugee.Checked = False
        unit_num.Clear()
        complex_name.Clear()
        street_num.Clear()
        street_farm_name.Clear()
        suburb_district.Clear()
        city_town.Clear()
        post_code.Clear()
        Combo_country.SelectedIndex = -1
        Check_default_phy_res_address.Checked = False
        post_unit_num.Clear()
        post_complex_name.Clear()
        post_street_num.Clear()
        post_street_farm_name.Clear()
        post_suburb_district.Clear()
        post_city_town.Clear()
        post_postal_code.Clear()
        Combo_post_country.SelectedIndex = -1
        Combo_pay_with.SelectedIndex = 0
        Combo_acc_type.SelectedIndex = -1
        Combo_bank.SelectedIndex = -1
        branch_code.Clear()
        acc_num.Clear()
        acc_holder_name.Clear()
        other_bank.Clear()
        branch_name.Clear()
        Combo_account_holder_rel.SelectedIndex = -1
        start_date.Text = Today.Date
        start_as.Clear()
        txtEmail.Clear()
        txtPhone.Clear()
        txtTaxNum.Clear()
        start_date.Value = Today.Date
        Combo_department.SelectedIndex = -1
        working_h_day.Text = FormatNumber(0, 0)
        working_d_week.Text = FormatNumber(0, 0)
        avrg_working_h_month.Text = FormatNumber(0, 0)
        avrg_working_d_month.Text = FormatNumber(0, 0)
        annual_salary.Text = FormatCurrency(0, 2)
        fixed_salary.Text = FormatCurrency(0, 2)
        rate_per_day.Text = FormatCurrency(0, 2)
        rate_per_hour.Text = FormatCurrency(0, 2)
    End Sub

    Dim designation1 As String
    Dim result As Integer
    Dim sql As String
    Dim caption As String
    Dim arrImage() As Byte
    Dim mstream As New System.IO.MemoryStream()
    Dim asylum_seeker As Char
    Dim refugee As Char
    Dim default_phy_res_address As Char
    Private Sub EmployeeSave()
        CategoryItemSub()

        If Check_asylum_seeker.Checked = True Then
            asylum_seeker = "Y"
        Else
            asylum_seeker = "N"
        End If

        If Check_refugee.Checked = True Then
            refugee = "Y"
        Else
            refugee = "N"
        End If

        If Check_default_phy_res_address.Checked = True Then
            default_phy_res_address = "Y"
        Else
            default_phy_res_address = "N"
        End If

        Try
            If code.Text = String.Empty Then
                MessageBox.Show("Please enter employee code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                code.Select()
            ElseIf title.Text = String.Empty Then
                MessageBox.Show("Please enter employee title.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                title.Select()
            ElseIf first_name.Text = String.Empty Then
                MessageBox.Show("Please enter employee first name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                first_name.Select()
            ElseIf last_name.Text = String.Empty Then
                MessageBox.Show("Please enter employee last name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                last_name.Select()
            ElseIf id_number.Text = String.Empty And passport_num.Text = String.Empty Then
                MessageBox.Show("Please enter employee ID Number/Passport Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                id_number.Select()
            Else
                caption = System.IO.Path.GetFileName(OpenFileDialog1.FileName)
                PictureBox1.Image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg)

                arrImage = mstream.GetBuffer()
                Dim FileSize As UInt32
                FileSize = mstream.Length
                mstream.Close()

                cn.Open()
                sql = "INSERT INTO employee (code, title, first_name, last_name, id_number, passport_num, initial, second_name, know_name, date_of_birth, passport_country, asylum_seeker, refugee, unit_num, complex_name, street_num, street_farm_name, suburb_district, city_town, post_code, country, default_phy_res_address, post_unit_num, post_complex_name, post_street_num, post_street_farm_name, post_suburb_district, post_city_town, post_postal_code, post_country, pay_with, acc_type, bank, branch_code, acc_num, acc_holder_name, other_bank, branch_name, account_holder_rel, start_date, start_as, department, working_h_day, working_d_week, avrg_working_h_month, avrg_working_d_month, annual_salary, fixed_salary, rate_per_day, rate_per_hour, Caption, employed, paybasis, designation, email,phonenum,taxnum,pension,riskbenefits,retirementfund,medicalaid,pocketexpense,dependantnum)
                                VALUES (@code, @title, @first_name, @last_name, @id_number, @passport_num, @initial, @second_name, @know_name, @date_of_birth, @passport_country, @asylum_seeker, @refugee, @unit_num, @complex_name, @street_num, @street_farm_name, @suburb_district, @city_town, @post_code, @country, @default_phy_res_address, @post_unit_num, @post_complex_name, @post_street_num, @post_street_farm_name, @post_suburb_district, @post_city_town, @post_postal_code, @post_country, @pay_with, @acc_type, @bank, @branch_code, @acc_num, @acc_holder_name, @other_bank, @branch_name, @account_holder_rel, @start_date, @start_as, @department, @working_h_day, @working_d_week, @avrg_working_h_month, @avrg_working_d_month, @annual_salary, @fixed_salary, @rate_per_day, @rate_per_hour, @Caption, @employed, @paybasis, @designation, @email,@phonenum,@taxnum,@pension,@riskbenefits,@retirementfund,@medicalaid,@pocketexpense,@dependantnum)"
                cmd = New MySqlCommand
                With cmd
                    .Connection = cn
                    .CommandText = sql
                    .Parameters.AddWithValue("@code", code.Text)
                    .Parameters.AddWithValue("@title", title.Text)
                    .Parameters.AddWithValue("@first_name", first_name.Text)
                    .Parameters.AddWithValue("@last_name", last_name.Text)
                    .Parameters.AddWithValue("@id_number", id_number.Text)
                    .Parameters.AddWithValue("@passport_num", passport_num.Text)
                    .Parameters.AddWithValue("@initial", initial.Text)
                    .Parameters.AddWithValue("@second_name", second_name.Text)
                    .Parameters.AddWithValue("@know_name", know_name.Text)
                    .Parameters.AddWithValue("@date_of_birth", date_of_birth.Value)
                    .Parameters.AddWithValue("@passport_country", Combo_passport_country.Text)
                    .Parameters.AddWithValue("@asylum_seeker", asylum_seeker)
                    .Parameters.AddWithValue("@refugee", refugee)
                    .Parameters.AddWithValue("@unit_num", unit_num.Text)
                    .Parameters.AddWithValue("@complex_name", complex_name.Text)
                    .Parameters.AddWithValue("@street_num", street_num.Text)
                    .Parameters.AddWithValue("@street_farm_name", street_farm_name.Text)
                    .Parameters.AddWithValue("@suburb_district", suburb_district.Text)
                    .Parameters.AddWithValue("@city_town", city_town.Text)
                    .Parameters.AddWithValue("@post_code", post_code.Text)
                    .Parameters.AddWithValue("@country", Combo_country.Text)
                    .Parameters.AddWithValue("@default_phy_res_address", default_phy_res_address)
                    .Parameters.AddWithValue("@post_unit_num", post_unit_num.Text)
                    .Parameters.AddWithValue("@post_complex_name", post_complex_name.Text)
                    .Parameters.AddWithValue("@post_street_num", post_street_num.Text)
                    .Parameters.AddWithValue("@post_street_farm_name", post_street_farm_name.Text)
                    .Parameters.AddWithValue("@post_suburb_district", post_suburb_district.Text)
                    .Parameters.AddWithValue("@post_city_town", post_city_town.Text)
                    .Parameters.AddWithValue("@post_postal_code", post_postal_code.Text)
                    .Parameters.AddWithValue("@post_country", Combo_post_country.Text)
                    .Parameters.AddWithValue("@pay_with", Combo_pay_with.Text)
                    .Parameters.AddWithValue("@acc_type", Combo_acc_type.Text)
                    .Parameters.AddWithValue("@bank", Combo_bank.Text)
                    .Parameters.AddWithValue("@branch_code", branch_code.Text)
                    .Parameters.AddWithValue("@acc_num", acc_num.Text)
                    .Parameters.AddWithValue("@acc_holder_name", acc_holder_name.Text)
                    .Parameters.AddWithValue("@other_bank", other_bank.Text)
                    .Parameters.AddWithValue("@branch_name", branch_name.Text)
                    .Parameters.AddWithValue("@account_holder_rel", Combo_account_holder_rel.Text)
                    .Parameters.AddWithValue("@start_date", start_date.Text)
                    .Parameters.AddWithValue("@start_as", start_as.Text)
                    .Parameters.AddWithValue("@department", deptart)
                    .Parameters.AddWithValue("@working_h_day", CDec(working_d_week.Text))
                    .Parameters.AddWithValue("@working_d_week", CDec(working_d_week.Text))
                    .Parameters.AddWithValue("@avrg_working_h_month", CDec(avrg_working_h_month.Text))
                    .Parameters.AddWithValue("@avrg_working_d_month", CDec(avrg_working_d_month.Text))
                    .Parameters.AddWithValue("@annual_salary", CDec(annual_salary.Text))
                    .Parameters.AddWithValue("@fixed_salary", CDec(fixed_salary.Text))
                    .Parameters.AddWithValue("@rate_per_day", CDec(rate_per_day.Text))
                    .Parameters.AddWithValue("@rate_per_hour", CDec(rate_per_hour.Text))
                    .Parameters.AddWithValue("@Caption", caption)
                    .Parameters.AddWithValue("@employed", "Y")
                    .Parameters.AddWithValue("@paybasis", ComboPayType.Text)
                    .Parameters.AddWithValue("@designation", desCode)
                    .Parameters.AddWithValue("@email", txtEmail.Text)
                    .Parameters.AddWithValue("@phonenum", txtPhone.Text)
                    .Parameters.AddWithValue("@taxnum", txtTaxNum.Text)
                    .Parameters.AddWithValue("@pension", CDec(txtPensionCont.Text))
                    .Parameters.AddWithValue("@riskbenefits", CDec(txtRiskPer.Text))
                    .Parameters.AddWithValue("@retirementfund", CDec(txtRetirementPer.Text))
                    .Parameters.AddWithValue("@medicalaid", CDec(txtMedicalAid.Text))
                    .Parameters.AddWithValue("@pocketexpense", CDec(txtOutofPocket.Text))
                    .Parameters.AddWithValue("@dependantnum", CDec(txtNoofDependants.Text))
                    result = .ExecuteNonQuery()
                End With
                cn.Close()

                Dim myAdapter As New MySqlDataAdapter
                Dim sqlquery = "SELECT * FROM employee WHERE code= '" + code.Text + "'"
                Dim myCommand As New MySqlCommand()
                myCommand.Connection = cn
                myCommand.CommandText = sqlquery
                myAdapter.SelectCommand = myCommand
                cn.Open()
                Dim ms As New MemoryStream

                Dim bm As Bitmap = New Bitmap(PictureBox1.Image)
                bm.Save(ms, PictureBox1.Image.RawFormat)

                Dim arrPic() As Byte = ms.GetBuffer()

                sqlquery = "UPDATE employee SET ImageFile=@ImageFile WHERE code = '" & code.Text & "'"

                myCommand = New MySqlCommand(sqlquery, cn)
                myCommand.Parameters.AddWithValue("@ImageFile", arrPic)
                myCommand.ExecuteNonQuery()
                cn.Close()
                MessageBox.Show("Employee successfully created.", "Employee", MessageBoxButtons.OK, MessageBoxIcon.Information)
                FileLoad()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim desCode As String
    Dim deptart As String
    Public Sub CategoryItemSub()
        Try
            cn.Open()
            Dim vat As String = "SELECT Code FROM departments WHERE Description = '" & Combo_department.Text & "'"
            cmd = New MySqlCommand(vat, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                deptart = dr.GetString("Code").ToString
            End While
            cn.Close()

            cn.Open()
            Dim desgn As String = "SELECT code FROM designation WHERE description = '" & Combodesignation.Text & "' ORDER BY code DESC"
            cmd = New MySqlCommand(desgn, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                desCode = dr.GetString("Code").ToString
            End While
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub EmployeeUpdate()
        CategoryItemSub()

        If Check_asylum_seeker.Checked = True Then
            asylum_seeker = "Y"
        Else
            asylum_seeker = "N"
        End If

        If Check_refugee.Checked = True Then
            refugee = "Y"
        Else
            refugee = "N"
        End If

        If Check_default_phy_res_address.Checked = True Then
            default_phy_res_address = "Y"
        Else
            default_phy_res_address = "N"
        End If

        Try
            If code.Text = String.Empty Then
                MessageBox.Show("Please enter employee code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                code.Select()
            ElseIf title.Text = String.Empty Then
                MessageBox.Show("Please enter employee title.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                title.Select()
            ElseIf first_name.Text = String.Empty Then
                MessageBox.Show("Please enter employee first name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                first_name.Select()
            ElseIf last_name.Text = String.Empty Then
                MessageBox.Show("Please enter employee last name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                last_name.Select()
            ElseIf id_number.Text = String.Empty And passport_num.Text = String.Empty Then
                MessageBox.Show("Please enter employee ID Number/Passport Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                id_number.Select()
            Else
                cn.Open()
                sql = "UPDATE employee SET pension=@pension,riskbenefits=@riskbenefits,retirementfund=@retirementfund,medicalaid=@medicalaid,pocketexpense=@pocketexpense,dependantnum=@dependantnum,phonenum=@phonenum,taxnum=@taxnum,paybasis=@paybasis,designation=@designation,email=@email,Caption=@Caption, title=@title, first_name=@first_name, last_name=@last_name, id_number=@id_number, passport_num=@passport_num, initial=@initial, second_name=@second_name, know_name=@know_name, date_of_birth=@date_of_birth, passport_country=@passport_country, asylum_seeker=@asylum_seeker, refugee=@refugee, unit_num=@unit_num, complex_name=@complex_name, street_num=@street_num, street_farm_name=@street_farm_name, suburb_district=@suburb_district, city_town=@city_town, post_code=@post_code, country=@country, default_phy_res_address=@default_phy_res_address, post_unit_num=@post_unit_num, post_complex_name=@post_complex_name, post_street_num=@post_street_num, post_street_farm_name=@post_street_farm_name, post_suburb_district=@post_suburb_district, post_city_town=@post_city_town, post_postal_code=@post_postal_code, post_country=@post_country, pay_with=@pay_with, acc_type=@acc_type, bank=@bank, branch_code=@branch_code, acc_num=@acc_num, acc_holder_name=@acc_holder_name, other_bank=@other_bank, branch_name=@branch_name, account_holder_rel=@account_holder_rel, start_date=@start_date, start_as=@start_as, department=@department, working_h_day=@working_h_day, working_d_week=@working_d_week, avrg_working_h_month=@avrg_working_h_month, avrg_working_d_month=@avrg_working_d_month, annual_salary=@annual_salary, fixed_salary=@fixed_salary, rate_per_day=@rate_per_day, rate_per_hour=@rate_per_hour WHERE code='" & code.Text & "'"

                With cmd
                    .Connection = cn
                    .CommandText = sql
                    .Parameters.AddWithValue("@title", title.Text)
                    .Parameters.AddWithValue("@first_name", first_name.Text)
                    .Parameters.AddWithValue("@last_name", last_name.Text)
                    .Parameters.AddWithValue("@id_number", id_number.Text)
                    .Parameters.AddWithValue("@passport_num", passport_num.Text)
                    .Parameters.AddWithValue("@initial", initial.Text)
                    .Parameters.AddWithValue("@second_name", second_name.Text)
                    .Parameters.AddWithValue("@know_name", know_name.Text)
                    .Parameters.AddWithValue("@date_of_birth", date_of_birth.Text)
                    .Parameters.AddWithValue("@passport_country", Combo_passport_country.Text)
                    .Parameters.AddWithValue("@asylum_seeker", asylum_seeker)
                    .Parameters.AddWithValue("@refugee", refugee)
                    .Parameters.AddWithValue("@unit_num", unit_num.Text)
                    .Parameters.AddWithValue("@complex_name", complex_name.Text)
                    .Parameters.AddWithValue("@street_num", street_num.Text)
                    .Parameters.AddWithValue("@street_farm_name", street_farm_name.Text)
                    .Parameters.AddWithValue("@suburb_district", suburb_district.Text)
                    .Parameters.AddWithValue("@city_town", city_town.Text)
                    .Parameters.AddWithValue("@post_code", post_code.Text)
                    .Parameters.AddWithValue("@country", Combo_country.Text)
                    .Parameters.AddWithValue("@default_phy_res_address", default_phy_res_address)
                    .Parameters.AddWithValue("@post_unit_num", post_unit_num.Text)
                    .Parameters.AddWithValue("@post_complex_name", post_complex_name.Text)
                    .Parameters.AddWithValue("@post_street_num", post_street_num.Text)
                    .Parameters.AddWithValue("@post_street_farm_name", post_street_farm_name.Text)
                    .Parameters.AddWithValue("@post_suburb_district", post_suburb_district.Text)
                    .Parameters.AddWithValue("@post_city_town", post_city_town.Text)
                    .Parameters.AddWithValue("@post_postal_code", post_postal_code.Text)
                    .Parameters.AddWithValue("@post_country", Combo_post_country.Text)
                    .Parameters.AddWithValue("@pay_with", Combo_pay_with.Text)
                    .Parameters.AddWithValue("@acc_type", Combo_acc_type.Text)
                    .Parameters.AddWithValue("@bank", Combo_bank.Text)
                    .Parameters.AddWithValue("@branch_code", branch_code.Text)
                    .Parameters.AddWithValue("@acc_num", acc_num.Text)
                    .Parameters.AddWithValue("@acc_holder_name", acc_holder_name.Text)
                    .Parameters.AddWithValue("@other_bank", other_bank.Text)
                    .Parameters.AddWithValue("@branch_name", branch_name.Text)
                    .Parameters.AddWithValue("@account_holder_rel", Combo_account_holder_rel.Text)
                    .Parameters.AddWithValue("@start_date", start_date.Text)
                    .Parameters.AddWithValue("@start_as", start_as.Text)
                    .Parameters.AddWithValue("@department", deptart)
                    .Parameters.AddWithValue("@working_h_day", CDec(working_h_day.Text))
                    .Parameters.AddWithValue("@working_d_week", CDec(working_d_week.Text))
                    .Parameters.AddWithValue("@avrg_working_h_month", CDec(avrg_working_h_month.Text))
                    .Parameters.AddWithValue("@avrg_working_d_month", CDec(avrg_working_d_month.Text))
                    .Parameters.AddWithValue("@annual_salary", CDec(annual_salary.Text))
                    .Parameters.AddWithValue("@fixed_salary", CDec(fixed_salary.Text))
                    .Parameters.AddWithValue("@rate_per_day", CDec(rate_per_day.Text))
                    .Parameters.AddWithValue("@rate_per_hour", CDec(rate_per_hour.Text))
                    .Parameters.AddWithValue("@Caption", caption)
                    .Parameters.AddWithValue("@paybasis", ComboPayType.Text)
                    .Parameters.AddWithValue("@designation", desCode)
                    .Parameters.AddWithValue("@email", txtEmail.Text)
                    .Parameters.AddWithValue("@phonenum", txtPhone.Text)
                    .Parameters.AddWithValue("@taxnum", txtTaxNum.Text)

                    .Parameters.AddWithValue("@pension", CDec(txtPensionCont.Text))
                    .Parameters.AddWithValue("@riskbenefits", CDec(txtRiskPer.Text))
                    .Parameters.AddWithValue("@retirementfund", CDec(txtRetirementPer.Text))
                    .Parameters.AddWithValue("@medicalaid", CDec(txtMedicalAid.Text))
                    .Parameters.AddWithValue("@pocketexpense", CDec(txtOutofPocket.Text))
                    .Parameters.AddWithValue("@dependantnum", CDec(txtNoofDependants.Text))
                    .ExecuteNonQuery()
                End With
                cn.Close()

                Dim myAdapter As New MySqlDataAdapter
                Dim sqlquery = "SELECT * FROM employee WHERE code= '" + code.Text + "'"
                Dim myCommand As New MySqlCommand()
                myCommand.Connection = cn
                myCommand.CommandText = sqlquery
                myAdapter.SelectCommand = myCommand
                cn.Open()
                Dim ms As New MemoryStream

                Dim bm As Bitmap = New Bitmap(PictureBox1.Image)
                bm.Save(ms, PictureBox1.Image.RawFormat)

                Dim arrPic() As Byte = ms.GetBuffer()

                sqlquery = "UPDATE employee SET ImageFile=@ImageFile WHERE code = '" & code.Text & "'"

                myCommand = New MySqlCommand(sqlquery, cn)
                myCommand.Parameters.AddWithValue("@ImageFile", arrPic)
                myCommand.ExecuteNonQuery()
                cn.Close()
                MessageBox.Show("Employee successfully updated.", "Employee", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        Catch ex As Exception
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
        cn.Dispose()
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label1.Select()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Select()
        Close()

        For Each ChildForm As Form In MainInterface.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub

    Private Sub TextBox17_TextChanged(sender As Object, e As EventArgs) Handles street_farm_name.TextChanged

    End Sub

    Private Sub Combo_pay_with_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Combo_pay_with.SelectedIndexChanged
        If Combo_pay_with.SelectedIndex = 3 Then
            Panel2.Visible = True
        Else
            Panel2.Visible = False
        End If
    End Sub

    Private Sub Check_default_phy_res_address_CheckedChanged(sender As Object, e As EventArgs) Handles Check_default_phy_res_address.CheckedChanged
        If Check_default_phy_res_address.Checked = True Then
            Combo_post_country.SelectedIndex = Combo_country.SelectedIndex
            post_unit_num.Text = unit_num.Text
            post_complex_name.Text = complex_name.Text
            post_street_num.Text = street_num.Text
            post_street_farm_name.Text = street_farm_name.Text
            post_suburb_district.Text = suburb_district.Text
            post_city_town.Text = city_town.Text
            post_postal_code.Text = post_code.Text
        ElseIf Check_default_phy_res_address.Checked = False Then
            post_unit_num.Text = ""
            post_complex_name.Text = ""
            post_street_num.Text = ""
            post_street_farm_name.Text = ""
            post_suburb_district.Text = ""
            post_city_town.Text = ""
            post_postal_code.Text = ""
            Combo_post_country.SelectedIndex = -1

        End If
    End Sub

    Private Sub first_name_Leave(sender As Object, e As EventArgs) Handles first_name.Leave
        If first_name.Text <> String.Empty Then
            know_name.Text = first_name.Text
            initial.Text = first_name.Text.Remove(1)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        code.Select()
        employeesearch.ShowDialog()
    End Sub

    Private Sub Employee_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            employeesearch.ShowDialog()
        End If
    End Sub

    Private Sub LoadDB()
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT * FROM employee WHERE code = '" & code.Text & "'"
            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                code.Text = dr("code").ToString()
                title.Text = dr("title").ToString()
                first_name.Text = dr("first_name").ToString()
                last_name.Text = dr("last_name").ToString()
                id_number.Text = dr("id_number").ToString()
                passport_num.Text = dr("passport_num").ToString()
                initial.Text = dr("initial").ToString()
                second_name.Text = dr("second_name").ToString()
                know_name.Text = dr("know_name").ToString()
                date_of_birth.Text = dr("date_of_birth").ToString()
                Combo_passport_country.Text = dr("passport_country").ToString()
                unit_num.Text = dr("unit_num").ToString()
                complex_name.Text = dr("complex_name").ToString()
                street_num.Text = dr("street_num").ToString()
                street_farm_name.Text = dr("street_farm_name").ToString()
                suburb_district.Text = dr("suburb_district").ToString()
                city_town.Text = dr("city_town").ToString()
                post_code.Text = dr("post_code").ToString()
                Combo_country.Text = dr("country").ToString()
                post_unit_num.Text = dr("post_unit_num").ToString()
                post_complex_name.Text = dr("post_complex_name").ToString()
                post_street_num.Text = dr("post_street_num").ToString()
                post_street_farm_name.Text = dr("post_street_farm_name").ToString()
                post_suburb_district.Text = dr("post_suburb_district").ToString()
                post_city_town.Text = dr("post_city_town").ToString()
                post_postal_code.Text = dr("post_postal_code").ToString()
                Combo_post_country.Text = dr("post_country").ToString()
                Combo_pay_with.Text = dr("pay_with").ToString()
                Combo_acc_type.Text = dr("acc_type").ToString()
                Combo_bank.Text = dr("bank").ToString()
                branch_code.Text = dr("branch_code").ToString()
                acc_num.Text = dr("acc_num").ToString()
                acc_holder_name.Text = dr("acc_holder_name").ToString()
                other_bank.Text = dr("other_bank").ToString()
                branch_name.Text = dr("branch_name").ToString()
                Combo_account_holder_rel.Text = dr("account_holder_rel").ToString()
                start_date.Text = dr("start_date").ToString()
                start_as.Text = dr("start_as").ToString()
                Combo_department.Text = dr("department").ToString()
                working_h_day.Text = FormatNumber(dr("working_h_day").ToString(), 2)
                working_d_week.Text = FormatNumber(dr("working_d_week").ToString(), 2)
                avrg_working_h_month.Text = FormatNumber(dr("avrg_working_h_month").ToString(), 2)
                avrg_working_d_month.Text = FormatNumber(dr("avrg_working_d_month").ToString(), 2)
                'annual_salary.Text = FormatCurrency(dr("annual_salary").ToString(), 2)
                'fixed_salary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                'rate_per_day.Text = FormatCurrency(dr("rate_per_day").ToString(), 2)
                'rate_per_hour.Text = FormatCurrency(dr("rate_per_hour").ToString(), 2)

                asylum_seeker_val = dr("asylum_seeker").ToString()
                refugee_val = dr("refugee").ToString()
                default_phy_res_address_val = dr("default_phy_res_address").ToString()

                If asylum_seeker_val = "Y" Then
                    Check_asylum_seeker.Checked = True
                Else
                    Check_asylum_seeker.Checked = False
                End If

                If refugee_val = "Y" Then
                    Check_refugee.Checked = True
                Else
                    Check_refugee.Checked = False
                End If

                If default_phy_res_address_val = "Y" Then
                    Check_default_phy_res_address.Checked = True
                Else
                    Check_default_phy_res_address.Checked = False
                End If
            End While
            cn.Close()
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Public Sub SaveLoadDB()
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT * FROM employee WHERE code = '" & NewEmployee.DataGridView1.CurrentRow.Cells(0).Value & "'"
            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                code.Text = dr("code").ToString()
                title.Text = dr("title").ToString()
                first_name.Text = dr("first_name").ToString()
                last_name.Text = dr("last_name").ToString()
                id_number.Text = dr("id_number").ToString()
                passport_num.Text = dr("passport_num").ToString()
                initial.Text = dr("initial").ToString()
                second_name.Text = dr("second_name").ToString()
                know_name.Text = dr("know_name").ToString()
                date_of_birth.Text = dr("date_of_birth").ToString()
                Combo_passport_country.Text = dr("passport_country").ToString()
                unit_num.Text = dr("unit_num").ToString()
                complex_name.Text = dr("complex_name").ToString()
                street_num.Text = dr("street_num").ToString()
                street_farm_name.Text = dr("street_farm_name").ToString()
                suburb_district.Text = dr("suburb_district").ToString()
                city_town.Text = dr("city_town").ToString()
                post_code.Text = dr("post_code").ToString()
                Combo_country.Text = dr("country").ToString()
                post_unit_num.Text = dr("post_unit_num").ToString()
                post_complex_name.Text = dr("post_complex_name").ToString()
                post_street_num.Text = dr("post_street_num").ToString()
                post_street_farm_name.Text = dr("post_street_farm_name").ToString()
                post_suburb_district.Text = dr("post_suburb_district").ToString()
                post_city_town.Text = dr("post_city_town").ToString()
                post_postal_code.Text = dr("post_postal_code").ToString()
                Combo_post_country.Text = dr("post_country").ToString()
                Combo_pay_with.Text = dr("pay_with").ToString()
                Combo_acc_type.Text = dr("acc_type").ToString()
                Combo_bank.Text = dr("bank").ToString()
                branch_code.Text = dr("branch_code").ToString()
                acc_num.Text = dr("acc_num").ToString()
                acc_holder_name.Text = dr("acc_holder_name").ToString()
                other_bank.Text = dr("other_bank").ToString()
                branch_name.Text = dr("branch_name").ToString()
                Combo_account_holder_rel.Text = dr("account_holder_rel").ToString()
                start_date.Text = dr("start_date").ToString()
                start_as.Text = dr("start_as").ToString()
                Combo_department.Text = dr("department").ToString()
                working_h_day.Text = FormatNumber(dr("working_h_day").ToString(), 2)
                working_d_week.Text = FormatNumber(dr("working_d_week").ToString(), 2)
                avrg_working_h_month.Text = FormatNumber(dr("avrg_working_h_month").ToString(), 2)
                avrg_working_d_month.Text = FormatNumber(dr("avrg_working_d_month").ToString(), 2)
                'annual_salary.Text = FormatCurrency(dr("annual_salary").ToString(), 2)
                'fixed_salary.Text = FormatCurrency(dr("fixed_salary").ToString(), 2)
                'rate_per_day.Text = FormatCurrency(dr("rate_per_day").ToString(), 2)
                'rate_per_hour.Text = FormatCurrency(dr("rate_per_hour").ToString(), 2)

                asylum_seeker_val = dr("asylum_seeker").ToString()
                refugee_val = dr("refugee").ToString()
                default_phy_res_address_val = dr("default_phy_res_address").ToString()

                If asylum_seeker_val = "Y" Then
                    Check_asylum_seeker.Checked = True
                Else
                    Check_asylum_seeker.Checked = False
                End If

                If refugee_val = "Y" Then
                    Check_refugee.Checked = True
                Else
                    Check_refugee.Checked = False
                End If

                If default_phy_res_address_val = "Y" Then
                    Check_default_phy_res_address.Checked = True
                Else
                    Check_default_phy_res_address.Checked = False
                End If
            End While
            cn.Close()
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim asylum_seeker_val As Char
    Dim refugee_val As Char
    Dim default_phy_res_address_val As Char
    Private Sub code_Leave(sender As Object, e As EventArgs) Handles code.Leave
        If code.Text = String.Empty Then

        Else
            Try
                cn.Open()
                Dim Query As String
                Query = "SELECT code FROM employee WHERE code = '" & Trim(code.Text) & "'"
                cmd = New MySqlCommand(Query, cn)
                dr = cmd.ExecuteReader
                If dr.HasRows = True Then
                    Itemexist = "Y"
                Else
                    Itemexist = "N"
                End If
                cn.Close()

                If Itemexist = "Y" Then
                    code.Enabled = False
                    LoadDB()
                    TotalSearch()
                    'ElseIf Itemexist = "N" Then
                    '    code.Enabled = True
                    '    code.Select()
                    '    clear()
                End If
            Catch ex As Exception
                'MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                cn.Dispose()
            End Try
        End If
    End Sub

    Sub TotalSearch()
        OtherData()
        imageRet()
        Depart()
        leaveHistory()

        leaveHistory()
        Panel3.Visible = False
        Panel4.Visible = True
        Panel7.Visible = False
        Panel5.Visible = False
        Panel10.Visible = False
        Panel9.Visible = False
        Panel4.Dock = DockStyle.Fill

        Label69.BackColor = Color.Lime
        Label67.BackColor = Color.Red
        Label65.BackColor = Color.Lime
        Label68.BackColor = Color.Lime
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime

        Panel4.Location = New Point(0, 41)
        OtherData()
        imageRet()
        Depart()
        leaveHistory()

        Panel3.Visible = False
        Panel4.Visible = False
        Panel7.Visible = False
        Panel5.Visible = True
        Panel10.Visible = False
        Panel9.Visible = False
        Panel5.Dock = DockStyle.Fill
        Label69.BackColor = Color.Red
        Label67.BackColor = Color.Lime
        Label65.BackColor = Color.Lime
        Label68.BackColor = Color.Lime
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime

        Panel5.Location = New Point(0, 41)
        OtherData()
        imageRet()
        Depart()
        leaveHistory()

        Panel3.Visible = False
        Panel4.Visible = False
        Panel5.Visible = False
        Panel7.Visible = True
        Panel10.Visible = False
        Panel9.Visible = False
        Panel7.Dock = DockStyle.Fill
        Label69.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label65.BackColor = Color.Lime
        Label68.BackColor = Color.Red
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime

        Panel7.Location = New Point(0, 41)
        OtherData()
        imageRet()
        Depart()
        leaveHistory()

        Panel3.Visible = True
        Panel7.Visible = False
        Panel10.Visible = False
        Panel5.Visible = False
        Panel4.Visible = False
        Panel9.Visible = False
        Label69.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label65.BackColor = Color.Red
        Label68.BackColor = Color.Lime
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime

        Panel3.Location = New Point(0, 41)
        Panel3.Dock = DockStyle.Fill
        OtherData()
        imageRet()
        Depart()
        leaveHistory()
        Values()
        Formats()
        Label70.Text = "Start Camera"
    End Sub

    Sub TotalSearch12()
        OtherData()
        imageRet()
        Depart()
        leaveHistory()

        leaveHistory()
        Panel3.Visible = False
        Panel4.Visible = True
        Panel7.Visible = False
        Panel5.Visible = False
        Panel10.Visible = False
        Panel9.Visible = False
        Panel4.Dock = DockStyle.Fill

        Label69.BackColor = Color.Lime
        Label67.BackColor = Color.Red
        Label65.BackColor = Color.Lime
        Label68.BackColor = Color.Lime
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime

        Panel4.Location = New Point(0, 41)
        OtherData()
        imageRet()
        Depart()
        leaveHistory()

        Panel3.Visible = False
        Panel4.Visible = False
        Panel7.Visible = False
        Panel5.Visible = True
        Panel10.Visible = False
        Panel9.Visible = False
        Panel5.Dock = DockStyle.Fill
        Label69.BackColor = Color.Red
        Label67.BackColor = Color.Lime
        Label65.BackColor = Color.Lime
        Label68.BackColor = Color.Lime
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime

        Panel5.Location = New Point(0, 41)
        OtherData()
        imageRet()
        Depart()
        leaveHistory()

        Panel3.Visible = False
        Panel4.Visible = False
        Panel5.Visible = False
        Panel7.Visible = True
        Panel10.Visible = False
        Panel9.Visible = False
        Panel7.Dock = DockStyle.Fill
        Label69.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label65.BackColor = Color.Lime
        Label68.BackColor = Color.Red
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime

        Panel7.Location = New Point(0, 41)
        OtherData()
        imageRet()
        Depart()
        leaveHistory()

        Panel3.Visible = True
        Panel7.Visible = False
        Panel10.Visible = False
        Panel5.Visible = False
        Panel4.Visible = False
        Panel9.Visible = False
        Label69.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label65.BackColor = Color.Red
        Label68.BackColor = Color.Lime
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime

        Panel3.Location = New Point(0, 41)
        Panel3.Dock = DockStyle.Fill
        OtherData()
        imageRet()
        Depart()
        leaveHistory()
        Values()
        Formats()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Label8.Select()
        cm.Position = 0
        TotalSearch()
        formatNumbers()
        transactionFile()
        Button4.Visible = False
        SalaryLoad()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Label8.Select()
        cm.Position = cm.Count - 1
        TotalSearch()
        formatNumbers()
        transactionFile()
        Button4.Visible = False
        SalaryLoad()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Label8.Select()
        cm.Position = cm.Position + 1
        TotalSearch()
        formatNumbers()
        transactionFile()
        Button4.Visible = False
        SalaryLoad()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Label8.Select()
        cm.Position = cm.Position - 1
        TotalSearch()
        formatNumbers()
        transactionFile()
        Button4.Visible = False
        SalaryLoad()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Try
            cn.Open()
            Dim empnum As String = "SELECT COUNT(ID) As 'TotalNumber' FROM employee"
            cmd = New MySqlCommand(empnum, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                TotalNumber = dr.GetString("TotalNumber").ToString
            End While
            cn.Close()

            Dim EmpNumber As Integer = CInt(My.Settings.EmpNum)
            If TotalNumber > EmpNumber Then
                MessageBox.Show("You have exceeded the number of employees registered for!. Number of employees registered = " & EmpNumber & " and number of employees in the database = " & TotalNumber & ". Please contact out administrator to get registration code for correct number of employees.", "Employees", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Label1.Select()
            Else
                ClearData()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        Label8.Select()
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

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub btnWorker_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Employee_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        'MainInterface.SplitContainer1.Visible = True
        ClosePreviewWindow()
    End Sub

    Private Sub Label31_Click(sender As Object, e As EventArgs) Handles Label31.Click
        Label1.Select()
        endemployement.ShowDialog()
    End Sub

    Private Sub DataGridView3_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub DataGridView3_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs)
        'Dim i As Integer

        'For i = 0 To DataGridView3.Columns.Count - 1

        '    DataGridView3.Columns.Item(i).SortMode = DataGridViewColumnSortMode.NotSortable

        'Next i
    End Sub

    Sub DataLoad()
        Try
            'cn.Open()
            'Dim Query1 As String
            'Query1 = "SELECT * FROM employee WHERE code =  '" & DataGridView3.CurrentRow.Cells(0).Value & "'"
            'cmd = New MySqlCommand(Query1, cn)
            'dr = cmd.ExecuteReader
            'While dr.Read
            '    code.Text = dr("code").ToString()
            '    title.Text = dr("title").ToString()
            '    first_name.Text = dr("first_name").ToString()
            '    last_name.Text = dr("last_name").ToString()
            '    id_number.Text = dr("id_number").ToString()
            '    passport_num.Text = dr("passport_num").ToString()
            '    initial.Text = dr("initial").ToString()
            '    second_name.Text = dr("second_name").ToString()
            '    know_name.Text = dr("know_name").ToString()
            '    date_of_birth.Text = dr("date_of_birth").ToString()
            '    Combo_passport_country.Text = dr("passport_country").ToString()
            '    unit_num.Text = dr("unit_num").ToString()
            '    complex_name.Text = dr("complex_name").ToString()
            '    street_num.Text = dr("street_num").ToString()
            '    street_farm_name.Text = dr("street_farm_name").ToString()
            '    suburb_district.Text = dr("suburb_district").ToString()
            '    city_town.Text = dr("city_town").ToString()
            '    post_code.Text = dr("post_code").ToString()
            '    Combo_country.Text = dr("country").ToString()
            '    post_unit_num.Text = dr("post_unit_num").ToString()
            '    post_complex_name.Text = dr("post_complex_name").ToString()
            '    post_street_num.Text = dr("post_street_num").ToString()
            '    post_street_farm_name.Text = dr("post_street_farm_name").ToString()
            '    post_suburb_district.Text = dr("post_suburb_district").ToString()
            '    post_city_town.Text = dr("post_city_town").ToString()
            '    post_postal_code.Text = dr("post_postal_code").ToString()
            '    Combo_post_country.Text = dr("post_country").ToString()
            '    Combo_pay_with.Text = dr("pay_with").ToString()
            '    Combo_acc_type.Text = dr("acc_type").ToString()
            '    Combo_bank.Text = dr("bank").ToString()
            '    branch_code.Text = dr("branch_code").ToString()
            '    acc_num.Text = dr("acc_num").ToString()
            '    acc_holder_name.Text = dr("acc_holder_name").ToString()
            '    other_bank.Text = dr("other_bank").ToString()
            '    branch_name.Text = dr("branch_name").ToString()
            '    Combo_account_holder_rel.Text = dr("account_holder_rel").ToString()
            '    start_date.Text = dr("start_date")
            '    start_as.Text = dr("start_as")
            '    Combo_department.Text = dr("department")
            '    working_h_day.Text = dr("working_h_day")
            '    working_d_week.Text = dr("working_d_week")
            '    avrg_working_h_month.Text = dr("avrg_working_h_month")
            '    avrg_working_d_month.Text = dr("avrg_working_d_month")
            '    annual_salary.Text = FormatCurrency(dr("annual_salary"), 2)
            '    fixed_salary.Text = FormatCurrency(dr("fixed_salary"), 2)
            '    rate_per_day.Text = FormatCurrency(dr("rate_per_day"), 2)
            '    rate_per_hour.Text = FormatCurrency(dr("rate_per_hour"), 2)
            'End While
            'cn.Close()
            'OtherData()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DataGridView3_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        DataLoad()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
        Try
            'Dim dt1 As New DataTable
            'cn.Open()
            'With cmd
            '    .Connection = cn
            '    '.CommandText = "SELECT i.Code AS 'CODE',i.Description AS 'DESCRIPTION',m.Qty As 'QTY',m.SellIncl01 As 'INCLUSIVE PRICE',m.SellExcl01 As 'EXCLUSIVE PRICE' FROM inventory As i INNER JOIN multistoretrn As m ON i.Code = m.ItemCode WHERE i.Blocked = 'N' AND m.StoreCode = '" & PROCESSSUPPLIER.ComboStore.Text & "' AND i.Blocked = 'N' AND i.Code LIKE '%" & txtFind.Text & "%' OR i.Description LIKE '%" & txtFind.Text & "%' ORDER BY i.Code"
            '    .CommandText = "SELECT code As 'Code', first_name As 'Name' FROM employee WHERE code LIKE '%" & txtSearch.Text & "%' OR first_name LIKE '%" & txtSearch.Text & "%' ORDER BY code"
            'End With
            'da.SelectCommand = cmd
            'dt1.Clear()
            'da.Fill(dt1)
            'DataGridView3.DataSource = dt1
            'cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
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

    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click
        code.Select()
        employeesearch.ShowDialog()
    End Sub

    Private Sub imageRet()
        Try
            cn.Open()
            Dim cmd1 As MySqlCommand
            cmd1 = New MySqlCommand("Select ImageFile from employee where code = '" & code.Text & "'", cn)
            Dim imageData As Byte() = DirectCast(cmd1.ExecuteScalar(), Byte())

            If Not imageData Is Nothing Then
                Using ms As New MemoryStream(imageData, 0, imageData.Length)
                    ms.Write(imageData, 0, imageData.Length)

                    PictureBox1.Image = Image.FromStream(ms, True)
                End Using
            End If
            cn.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub imageRet1()
        Try
            cn.Open()
            Dim cmd1 As MySqlCommand
            cmd1 = New MySqlCommand("Select ImageFile from employee", cn)
            Dim imageData As Byte() = DirectCast(cmd1.ExecuteScalar(), Byte())

            If Not imageData Is Nothing Then
                Using ms As New MemoryStream(imageData, 0, imageData.Length)
                    ms.Write(imageData, 0, imageData.Length)

                    PictureBox1.Image = Image.FromStream(ms, True)
                End Using
            End If
            cn.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim empCheck As Char
    Sub CheckEmployees()
        Try
            cn.Close()
            cn.Open()
            Combo_bank.Items.Clear()
            Dim bankquery As String = "SELECT * FROM bank WHERE Description != ''"
            cmd = New MySqlCommand(bankquery, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim sInvet = dr.GetString("Description").ToString
                Combo_bank.Items.Add(sInvet)
            End While
            cn.Close()

            cn.Open()
            Dim Query As String
            Query = "SELECT ID FROM employee"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                empCheck = "Y"
            Else
                empCheck = "N"
            End If
            cn.Close()

            If empCheck = "Y" Then
                FileLoad()
            ElseIf empCheck = "N" Then
                ClearData()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub ClearData()
        Try
            clear()
            code.Select()
            PictureBox1.Image = My.Resources.pic
            'MessageBox.Show("Details cleared", "Employee", MessageBoxButtons.OK, MessageBoxIcon.Information)

            leaveHistory()
            Panel3.Visible = False
            Panel4.Visible = True
            Panel7.Visible = False
            Panel5.Visible = False
            Panel10.Visible = False
            Panel9.Visible = False
            Panel4.Dock = DockStyle.Fill

            Label69.BackColor = Color.Lime
            Label67.BackColor = Color.Red
            Label65.BackColor = Color.Lime
            Label68.BackColor = Color.Lime
            Label77.BackColor = Color.Lime
            Label75.BackColor = Color.Lime

            Panel4.Location = New Point(0, 41)
            clear()

            Panel3.Visible = False
            Panel4.Visible = False
            Panel7.Visible = False
            Panel5.Visible = True
            Panel10.Visible = False
            Panel9.Visible = False
            Panel5.Dock = DockStyle.Fill
            Label69.BackColor = Color.Red
            Label67.BackColor = Color.Lime
            Label65.BackColor = Color.Lime
            Label68.BackColor = Color.Lime
            Label77.BackColor = Color.Lime
            Label75.BackColor = Color.Lime

            Panel5.Location = New Point(0, 41)
            clear()

            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel7.Visible = True
            Panel10.Visible = False
            Panel9.Visible = False
            Panel7.Dock = DockStyle.Fill
            Label69.BackColor = Color.Lime
            Label67.BackColor = Color.Lime
            Label65.BackColor = Color.Lime
            Label68.BackColor = Color.Red
            Label77.BackColor = Color.Lime
            Label75.BackColor = Color.Lime

            Panel7.Location = New Point(0, 41)
            clear()

            Panel3.Visible = True
            Panel7.Visible = False
            Panel10.Visible = False
            Panel5.Visible = False
            Panel4.Visible = False
            Panel9.Visible = False
            Label69.BackColor = Color.Lime
            Label67.BackColor = Color.Lime
            Label65.BackColor = Color.Red
            Label68.BackColor = Color.Lime
            Label77.BackColor = Color.Lime
            Label75.BackColor = Color.Lime

            Panel3.Location = New Point(0, 41)
            Panel3.Dock = DockStyle.Fill
            clear()
            code.ReadOnly = False

            title.SelectedIndex = 0

            cn.Open()
            Combo_department.Items.Clear()
            Dim Namequery As String = "SELECT Code,Description FROM departments WHERE Description != ''"
            cmd = New MySqlCommand(Namequery, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim sInvet = dr.GetString("Description").ToString
                Combo_department.Items.Add(sInvet)
            End While
            cn.Close()
            Combo_department.SelectedIndex = 0

            cn.Open()
            Combodesignation.Items.Clear()
            Dim Namequery1 As String = "SELECT code,description FROM designation WHERE description != ''"
            cmd = New MySqlCommand(Namequery1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim sInvet = dr.GetString("Description").ToString
                Combodesignation.Items.Add(sInvet)
            End While
            cn.Close()
            Combodesignation.SelectedIndex = 0

            cn.Open()
            Combo_bank.Items.Clear()
            Dim bankquery As String = "SELECT * FROM bank WHERE Description != ''"
            cmd = New MySqlCommand(bankquery, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim sInvet = dr.GetString("Description").ToString
                Combo_bank.Items.Add(sInvet)
            End While
            cn.Close()

            Dim currentRegion = System.Globalization.RegionInfo.CurrentRegion.DisplayName
            Combo_passport_country.Text = currentRegion
            Combo_country.Text = currentRegion
            Combo_post_country.Text = currentRegion
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        second = second + 1
        If second >= 10 Then
            Timer1.Stop()
            CheckEmployees()
        End If
    End Sub

    Private Sub Label65_Click(sender As Object, e As EventArgs) Handles Label65.Click
        Panel3.Visible = True
        Panel4.Visible = False
        Panel5.Visible = False
        Panel7.Visible = False
        Panel10.Visible = False
        Panel9.Visible = False
        Panel3.Dock = DockStyle.Fill

        Label69.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label65.BackColor = Color.Red
        Label68.BackColor = Color.Lime
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime
        Button4.Visible = False
    End Sub

    Private Sub Label67_Click(sender As Object, e As EventArgs) Handles Label67.Click
        Panel3.Visible = False
        Panel4.Visible = True
        Panel7.Visible = False
        Panel5.Visible = False
        Panel10.Visible = False
        Panel9.Visible = False
        Panel4.Dock = DockStyle.Fill

        Label69.BackColor = Color.Lime
        Label67.BackColor = Color.Red
        Label65.BackColor = Color.Lime
        Label68.BackColor = Color.Lime
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime

        Panel4.Location = New Point(0, 41)
        Button4.Visible = False
    End Sub

    Private Sub Label69_Click(sender As Object, e As EventArgs) Handles Label69.Click
        Panel3.Visible = False
        Panel4.Visible = False
        Panel7.Visible = False
        Panel5.Visible = True
        Panel10.Visible = False
        Panel9.Visible = False
        Panel5.Dock = DockStyle.Fill
        Label69.BackColor = Color.Red
        Label67.BackColor = Color.Lime
        Label65.BackColor = Color.Lime
        Label68.BackColor = Color.Lime
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime

        Panel5.Location = New Point(0, 41)
        Button4.Visible = False
    End Sub

    Private Sub Label68_Click(sender As Object, e As EventArgs) Handles Label68.Click
        Panel3.Visible = False
        Panel4.Visible = False
        Panel5.Visible = False
        Panel7.Visible = True
        Panel10.Visible = False
        Panel9.Visible = False
        Panel7.Dock = DockStyle.Fill
        Label69.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label65.BackColor = Color.Lime
        Label68.BackColor = Color.Red
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime

        Panel7.Location = New Point(0, 41)
        Button4.Visible = False
    End Sub

    Private Sub Label77_Click(sender As Object, e As EventArgs) Handles Label77.Click
        Panel3.Visible = False
        Panel4.Visible = False
        Panel5.Visible = False
        Panel7.Visible = False
        Panel10.Visible = True
        Panel9.Visible = False
        Panel10.Dock = DockStyle.Fill
        Label69.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label65.BackColor = Color.Lime
        Label68.BackColor = Color.Lime
        Label77.BackColor = Color.Red
        Label75.BackColor = Color.Lime

        Panel10.Location = New Point(0, 41)
        Button4.Visible = True
    End Sub

    Private Sub Label76_Click(sender As Object, e As EventArgs)
        Panel3.Visible = False
        Panel4.Visible = False
        Panel5.Visible = False
        Panel7.Visible = False
        Panel10.Visible = False
        Panel9.Visible = False
        Label69.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label65.BackColor = Color.Lime
        Label68.BackColor = Color.Lime
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Lime

    End Sub

    Private Sub Label75_Click(sender As Object, e As EventArgs) Handles Label75.Click
        Panel3.Visible = False
        Panel4.Visible = False
        Panel5.Visible = False
        Panel7.Visible = False
        Panel10.Visible = False
        Panel9.Visible = True
        Panel9.Dock = DockStyle.Fill
        Label69.BackColor = Color.Lime
        Label67.BackColor = Color.Lime
        Label65.BackColor = Color.Lime
        Label68.BackColor = Color.Lime
        Label77.BackColor = Color.Lime
        Label75.BackColor = Color.Red

        Panel9.Location = New Point(0, 41)
        Button4.Visible = False
    End Sub

    Private Sub Label40_Click(sender As Object, e As EventArgs) Handles Label40.Click
        Label1.Select()
        PictureBox1.Image = My.Resources.pic
    End Sub

    Private Sub Label57_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs)
        Try
            Dim myAdapter As New MySqlDataAdapter
            Dim sqlquery = "SELECT * FROM employee WHERE code= '" + code.Text + "'"
            Dim myCommand As New MySqlCommand()
            myCommand.Connection = cn
            myCommand.CommandText = sqlquery
            myAdapter.SelectCommand = myCommand
            cn.Open()
            Dim ms As New MemoryStream

            Dim bm As Bitmap = New Bitmap(PictureBox1.Image)
            bm.Save(ms, PictureBox1.Image.RawFormat)

            Dim arrPic() As Byte = ms.GetBuffer()



            sqlquery = "UPDATE employee SET ImageFile=@ImageFile WHERE code = '" & code.Text & "'"

            myCommand = New MySqlCommand(sqlquery, cn)
            myCommand.Parameters.AddWithValue("@ImageFile", arrPic)
            myCommand.ExecuteNonQuery()
            cn.Close()

            MsgBox("Update")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim paybasis As String
    Sub Values()
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT paybasis FROM employee WHERE code =  '" & Trim(code.Text) & "'"
            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                paybasis = dr("paybasis").ToString()
            End While
            cn.Close()

            cn.Open()
            Dim Query As String
            Query = "SELECT code FROM employee WHERE code = '" & Trim(code.Text) & "'"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                Itemexist = "Y"
            Else
                Itemexist = "N"
            End If
            cn.Close()

            'If Itemexist = "Y" Then
            '    EmployeeUpdate()
            'ElseIf Itemexist = "N" Then
            '    EmployeeSave()
            'End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub Formats()
        'annual_salary.Text = FormatCurrency(annual_salary.Text, 2)
        'fixed_salary.Text = FormatCurrency(fixed_salary.Text, 2)
        'rate_per_day.Text = FormatCurrency(rate_per_day.Text, 2)
        'rate_per_hour.Text = FormatCurrency(rate_per_hour.Text, 2)
    End Sub

    Sub Hours()
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT * FROM employee WHERE code =  '" & Trim(code.Text) & "'"
            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                working_h_day1 = dr("working_h_day").ToString()
                working_d_week1 = dr("working_d_week").ToString()
            End While
            cn.Close()
        Catch ex As Exception

        End Try
    End Sub

    Dim working_h_day1 As String
    Dim working_d_week1 As String
    Private Sub annual_salary_Leave(sender As Object, e As EventArgs) Handles annual_salary.Leave
        If working_h_day.Text = 0 Then
            MessageBox.Show("Attempted to divide by zero. Please enter working hours per day.", "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
            working_h_day.SelectAll()
            annual_salary.Text = FormatCurrency(0, 2)
        Else
            Hours()

            annual_salary.Text = FormatCurrency(annual_salary.Text, 2)
            fixed_salary.Text = FormatCurrency(CDec(annual_salary.Text) / 12, 2)
            rate_per_day.Text = FormatCurrency(CDec(fixed_salary.Text) / (CInt(working_h_day.Text) * 4), 2)
            rate_per_hour.Text = FormatCurrency(CDec(rate_per_day.Text) / CInt(working_h_day.Text), 2)
        End If
    End Sub

    Private Sub Combo_bank_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Combo_bank.SelectedIndexChanged
        Try
            cn.Open()
            Dim Query1 As String
            Query1 = "SELECT * FROM bank WHERE Description =  '" & Trim(Combo_bank.Text) & "'"
            cmd = New MySqlCommand(Query1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                branch_code.Text = dr("BranchCode").ToString()
                acc_num.Text = ""
            End While
            cn.Close()
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
    Private Sub Label70_Click(sender As Object, e As EventArgs) Handles Label70.Click
        If Label70.Text = "Start Camera" Then
            DeviceID = lstDevices.SelectedIndex
            OpenPreviewWindow()
            Dim bReturn As Boolean
            Dim s As CAPSTATUS
            bReturn = SendMessage(hHwnd, WM_CAP_GET_STATUS, Marshal.SizeOf(s), s)
            Debug.WriteLine(String.Format("Video Size {0} x {1}", s.uiImageWidth, s.uiImageHeight))
            Label70.Text = "Take Image"
        ElseIf Label70.Text = "Take Image" Then
            Dim data As IDataObject
            Dim bmap As Bitmap

            '
            ' Copy image to clipboard
            '
            SendMessage(hHwnd, WM_CAP_EDIT_COPY, 0, 0)
            '
            ' Get image from clipboard and convert it to a bitmap
            '
            data = Clipboard.GetDataObject()
            If data.GetDataPresent(GetType(System.Drawing.Bitmap)) Then
                bmap = CType(data.GetData(GetType(System.Drawing.Bitmap)), Bitmap)
                PictureBox1.Image = bmap
                ClosePreviewWindow()
                Trace.Assert(Not (bmap Is Nothing))
                PictureBox1.Image.Save(appPath & "\IMAGES\" & code.Text & ".jpg")
                PictureBox1.Image = Image.FromFile(appPath & "\IMAGES\" & code.Text & ".jpg")
                'caption = "Image.pic"
                'If sfdImage.ShowDialog = DialogResult.OK Then
                'bmap.Save(sfdImage.FileName, Imaging.ImageFormat.Bmp)
                'End If
            End If
            Label70.Text = "Start Camera"
        End If
    End Sub

    Private Sub Label58_Click(sender As Object, e As EventArgs) Handles Label58.Click
        If Label58.Text = "Activate Camera" Then
            DeviceID = lstDevices.SelectedIndex
            OpenPreviewWindow()
            Dim bReturn As Boolean
            Dim s As CAPSTATUS
            bReturn = SendMessage(hHwnd, WM_CAP_GET_STATUS, Marshal.SizeOf(s), s)
            Debug.WriteLine(String.Format("Video Size {0} x {1}", s.uiImageWidth, s.uiImageHeight))
            Label58.Text = "Deactivate Camera"
        ElseIf Label58.Text = "Deactivate Camera" Then
            ClosePreviewWindow()
            Label58.Text = "Activate Camera"
        End If
    End Sub

    Private Sub Button4_Click_2(sender As Object, e As EventArgs) Handles Button4.Click
        Label9.Select()
        PrintOrEmailPayslip.ShowDialog()
    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs)
        PleaseWait.Show()
    End Sub

    Private Sub fixed_salary_TextChanged(sender As Object, e As EventArgs) Handles fixed_salary.TextChanged

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If code.Text = String.Empty Then
            Button7.Enabled = False
            Button6.Enabled = False
            Button9.Enabled = False
            Button8.Enabled = False
        Else
            Button7.Enabled = True
            Button6.Enabled = True
            Button9.Enabled = True
            Button8.Enabled = True
        End If
    End Sub

    Private Sub fixed_salary_Leave(sender As Object, e As EventArgs) Handles fixed_salary.Leave
        If working_h_day.Text = 0 Then
            MessageBox.Show("Attempted to divide by zero. Please enter working hours per day.", "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
            working_h_day.SelectAll()
            fixed_salary.Text = FormatCurrency(0, 2)
        Else
            Hours()

            annual_salary.Text = FormatCurrency(CDec(fixed_salary.Text) * 12, 2)
            fixed_salary.Text = FormatCurrency(fixed_salary.Text, 2)
            rate_per_day.Text = FormatCurrency(CDec(fixed_salary.Text) / (CInt(working_h_day.Text) * 4), 2)
            rate_per_hour.Text = FormatCurrency(CDec(rate_per_day.Text) / CInt(working_h_day.Text), 2)
        End If
    End Sub

    Private Sub rate_per_day_Leave(sender As Object, e As EventArgs) Handles rate_per_day.Leave
        If working_h_day.Text = 0 Then
            MessageBox.Show("Attempted to divide by zero. Please enter working hours per day.", "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
            working_h_day.SelectAll()
            rate_per_day.Text = FormatCurrency(0, 2)
        Else
            Hours()

            rate_per_day.Text = FormatCurrency(CDec(rate_per_day.Text), 2)
            fixed_salary.Text = FormatCurrency(CDec(rate_per_day.Text) * (CInt(working_h_day.Text) * 4), 2)
            annual_salary.Text = FormatCurrency(fixed_salary.Text * 12, 2)
            rate_per_hour.Text = FormatCurrency(CDec(rate_per_day.Text) / CInt(working_h_day.Text), 2)
        End If
    End Sub

    Private Sub rate_per_hour_Leave(sender As Object, e As EventArgs) Handles rate_per_hour.Leave
        If working_h_day.Text = 0 Then
            MessageBox.Show("Attempted to divide by zero. Please enter working hours per day.", "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
            working_h_day.SelectAll()
            rate_per_hour.Text = FormatCurrency(0, 2)
        Else
            Hours()

            rate_per_hour.Text = FormatCurrency(rate_per_hour.Text, 2)
            rate_per_day.Text = FormatCurrency(CDec(rate_per_hour.Text) * CInt(working_h_day.Text), 2)
            fixed_salary.Text = FormatCurrency(CDec(rate_per_day.Text) * (CInt(working_h_day.Text) * 4), 2)
            annual_salary.Text = FormatCurrency(fixed_salary.Text * 12, 2)
        End If
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs)

    End Sub
End Class