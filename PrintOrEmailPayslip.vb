Imports System.IO
#Disable Warning BC40056 ' Namespace or type specified in the Imports 'System.Web.Mail' doesn't contain any public member or cannot be found. Make sure the namespace or the type is defined and contains at least one public member. Make sure the imported element name doesn't use any aliases.
#Enable Warning BC40056 ' Namespace or type specified in the Imports 'System.Web.Mail' doesn't contain any public member or cannot be found. Make sure the namespace or the type is defined and contains at least one public member. Make sure the imported element name doesn't use any aliases.
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports MySql.Data.MySqlClient
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Security.Cryptography
Imports System.Text
Public Class PrintOrEmailPayslip
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager
    Dim email_sender As String
    Dim email_password As String
    Dim email_port As String

    Private Sub sendMail()
        Try
            cn.Open()
            Dim Namequery As String = "SELECT email,emailpassword,port FROM parameters"
            cmd = New MySqlCommand(Namequery, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                email_sender = dr("email").ToString()
                email_password = dr("emailpassword").ToString()
                email_port = dr("port").ToString()
            End While
            cn.Close()

            Dim Smtp_Server As New SmtpClient
            Dim e_mail As New MailMessage()
            Smtp_Server.UseDefaultCredentials = False
            Smtp_Server.Credentials = New Net.NetworkCredential(email_sender, Decrypt(email_password))
            Smtp_Server.Port = 587
            Smtp_Server.EnableSsl = True
            Smtp_Server.Host = "smtp.gmail.com"

            e_mail = New MailMessage()
            e_mail.From = New MailAddress(email_sender)
            e_mail.To.Add(Trim(txtTo.Text))
            e_mail.Subject = txtSubject.Text
            e_mail.IsBodyHtml = False
            e_mail.Body = txtBody.Text
            e_mail.Attachments.Add(New Attachment(appPathDoc))
            Smtp_Server.Send(e_mail)
            MessageBox.Show("Email Sent", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch error_t As Exception
            MessageBox.Show(error_t.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Dim com_name As String
    Private Sub PrintEmail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        Label2.Select()

        'Dim dateValue As String = Employee.DataGridView2.CurrentRow.Cells(0).Value()
        'MsgBox(dateValue)

        Dim p As New System.Drawing.Printing.PrinterSettings()

        Dim defaultPrinterName As String

        defaultPrinterName = p.PrinterName
        PrinterToPrint.Text = defaultPrinterName

        cn.Open()
        Dim Namequery1 As String = "SELECT com_name FROM company"
        cmd = New MySqlCommand(Namequery1, cn)
        dr = cmd.ExecuteReader
        While dr.Read
            com_name = dr("com_name").ToString()
        End While
        cn.Close()

        cn.Open()
        Dim Namequery As String = "SELECT email,title,first_name,last_name FROM employee WHERE code = '" & Employee.code.Text & "'"
        cmd = New MySqlCommand(Namequery, cn)
        dr = cmd.ExecuteReader
        While dr.Read
            txtTo.Text = dr("email").ToString()
            txtSubject.Text = "Payslip"
            txtBody.Text = "Hi" & " " & dr("title").ToString() & " " & dr("first_name").ToString() & " " & dr("last_name").ToString() & vbNewLine & vbNewLine & com_name & " has just processed your pay and a new pay slip is available." & vbNewLine & vbNewLine & "Regards, " & vbNewLine & com_name
        End While
        cn.Close()
    End Sub

    Sub EmailDeatils()
        Try
            'cn.Open()
            'Dim Namequery As String = "SELECT email,title,first_name,last_name FROM employee WHERE code = '" & PayrollSchedule.DataGridView1.Rows(i).Cells(0).Value & "'"
            'cmd = New MySqlCommand(Namequery, cn)
            'dr = cmd.ExecuteReader
            'While dr.Read
            '    txtTo.Text = dr("email").ToString()
            'End While
            'cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.Text = "Print Document" Then
            Me.Size = New System.Drawing.Size(518, 139)
        ElseIf ComboBox1.Text = "Print and Email Document" Then
            Me.Size = New System.Drawing.Size(518, 379)
            Me.StartPosition = FormStartPosition.CenterScreen
            EmailDeatils()
        ElseIf ComboBox1.Text = "Email Document" Then
            Me.Size = New System.Drawing.Size(518, 379)
            Me.StartPosition = FormStartPosition.CenterScreen
            EmailDeatils()
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Label2.Select()
        Close()
        RunPayroll.Close()
    End Sub

    'Dim cryRpt As New ReportDocument
    'Dim pdfFile As String = "c:\ProductReport1.pdf"
    Dim appPathDoc As String = "C:\Data\DOCUMENTS\" & Employee.code.Text & ".pdf" '= Path.GetDirectoryName(Application.ExecutablePath)
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Label2.Select()
        If ComboBox1.SelectedIndex = 0 Then
            Try
                Dim objConn As MySqlConnection
                Dim daT1 As MySqlDataAdapter
                Dim activecomp As DataSet
                Dim strConnection As String
                Dim strSQL As String

                strConnection = "server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";"
                objConn = New MySqlConnection(strConnection)
                objConn.Open()

                strSQL = "SELECT *
                          FROM vwpayslip
                          WHERE emp_code = '" & Employee.code.Text & "' AND datepaid = '" & Employee.DataGridView2.CurrentRow.Cells(0).Value & "'"
                daT1 = New MySqlDataAdapter(strSQL, objConn)
                activecomp = New DataSet
                daT1.Fill(activecomp, "vwpayslip")

                Dim rpt As New ReportDocument

                Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
                rpt.Load(appPath + "\Reports\paysliprpt.rpt")
                rpt.SetDataSource(activecomp)

                rpt.PrintOptions.PrinterName = PrinterToPrint.Text
                rpt.PrintToPrinter(1, False, 0, 0)
                objConn.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error Loading Report", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try

            RunPayroll.Close()
            Close()
        ElseIf ComboBox1.SelectedIndex = 1 Then
            Try
                Dim objConn As MySqlConnection
                Dim daT1 As MySqlDataAdapter
                Dim activecomp As DataSet
                Dim strConnection As String
                Dim strSQL As String

                strConnection = "server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";"
                objConn = New MySqlConnection(strConnection)
                objConn.Open()

                strSQL = "SELECT *
                          FROM vwpayslip
                          WHERE emp_code = '" & Employee.code.Text & "' AND datepaid = '" & Employee.DataGridView2.CurrentRow.Cells(0).Value & "'"
                daT1 = New MySqlDataAdapter(strSQL, objConn)
                activecomp = New DataSet
                daT1.Fill(activecomp, "vwpayslip")

                Dim rpt As New ReportDocument

                Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
                rpt.Load(appPath + "\Reports\paysliprpt.rpt")
                rpt.SetDataSource(activecomp)

                Dim CrExportOptions As ExportOptions
                Dim CrDiskFileDestinationOptions As New _
                DiskFileDestinationOptions()
                Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions
                CrDiskFileDestinationOptions.DiskFileName = appPathDoc 'appPath + "\DOCUMENTS\" & Employee.code.Text & ".pdf"
                CrExportOptions = rpt.ExportOptions
                With CrExportOptions
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                    .DestinationOptions = CrDiskFileDestinationOptions
                    .FormatOptions = CrFormatTypeOptions
                End With
                rpt.Export()
                objConn.Close()

            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error Loading Report", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            sendMail()
            RunPayroll.Close()
            Close()
        ElseIf ComboBox1.SelectedIndex = 2 Then
            Try
                Dim objConn As MySqlConnection
                Dim daT1 As MySqlDataAdapter
                Dim activecomp As DataSet
                Dim strConnection As String
                Dim strSQL As String

                strConnection = "server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";"
                objConn = New MySqlConnection(strConnection)
                objConn.Open()

                strSQL = "SELECT *
                          FROM vwpayslip
                          WHERE emp_code = '" & Employee.code.Text & "' AND datepaid = '" & Employee.DataGridView2.CurrentRow.Cells(0).Value & "'"
                daT1 = New MySqlDataAdapter(strSQL, objConn)
                activecomp = New DataSet
                daT1.Fill(activecomp, "vwpayslip")

                Dim rpt As New ReportDocument

                Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
                rpt.Load(appPath + "\Reports\paysliprpt.rpt")
                rpt.SetDataSource(activecomp)

                rpt.PrintOptions.PrinterName = PrinterToPrint.Text
                rpt.PrintToPrinter(1, False, 0, 0)
                objConn.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error Loading Report", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try

            Try
                Dim objConn As MySqlConnection
                Dim daT1 As MySqlDataAdapter
                Dim activecomp As DataSet
                Dim strConnection As String
                Dim strSQL As String

                strConnection = "server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";"
                objConn = New MySqlConnection(strConnection)
                objConn.Open()

                strSQL = "SELECT *
                          FROM vwpayslip
                          WHERE emp_code = '" & Employee.code.Text & "' AND datepaid = '" & Employee.DataGridView2.CurrentRow.Cells(0).Value & "'"
                daT1 = New MySqlDataAdapter(strSQL, objConn)
                activecomp = New DataSet
                daT1.Fill(activecomp, "vwpayslip")

                Dim rpt As New ReportDocument

                Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
                rpt.Load(appPath + "\Reports\paysliprpt.rpt")
                rpt.SetDataSource(activecomp)

                Dim CrExportOptions As ExportOptions
                Dim CrDiskFileDestinationOptions As New _
                DiskFileDestinationOptions()
                Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions
                CrDiskFileDestinationOptions.DiskFileName = appPathDoc 'appPath + "\DOCUMENTS\" & Employee.code.Text & ".pdf"
                CrExportOptions = rpt.ExportOptions
                With CrExportOptions
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                    .DestinationOptions = CrDiskFileDestinationOptions
                    .FormatOptions = CrFormatTypeOptions
                End With
                rpt.Export()
                objConn.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error Loading Report", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            sendMail()
            RunPayroll.Close()
            Close()
        End If
    End Sub

    Public PrintEmPay As Char
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PrintEmPay = "Y"
        Label2.Select()
        PrinterList.ShowDialog()
    End Sub

    Private Function Decrypt(cipherText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
         &H65, &H64, &H76, &H65, &H64, &H65,
         &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function

End Class