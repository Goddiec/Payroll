Imports System.IO
#Disable Warning BC40056 ' Namespace or type specified in the Imports 'System.Web.Mail' doesn't contain any public member or cannot be found. Make sure the namespace or the type is defined and contains at least one public member. Make sure the imported element name doesn't use any aliases.
Imports System.Web.Mail
#Enable Warning BC40056 ' Namespace or type specified in the Imports 'System.Web.Mail' doesn't contain any public member or cannot be found. Make sure the namespace or the type is defined and contains at least one public member. Make sure the imported element name doesn't use any aliases.
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports MySql.Data.MySqlClient
Public Class PrintEmail
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim ds As DataSet = New DataSet
    Dim dv As DataView
    Dim cm As CurrencyManager

    Dim company As String
    Dim Info11 As String
    Dim Info21 As String
    Dim Info31 As String
    Dim Info41 As String
    Dim Info51 As String

    Dim AddressIn1 As String
    Dim AddressIn2 As String
    Dim AddressIn3 As String
    Dim AddressIn4 As String
    Dim AddressIn5 As String

    Dim Adin1 As String
    Dim Adin2 As String
    Dim Adin3 As String
    Dim Adin4 As String
    Dim Adin5 As String
    Dim ID As Integer = 0
    Dim DocNumber As String
    Sub LOADID()
        Try
            cn.Open()
            With cmd
                .Connection = cn
                .CommandText = "SELECT ID FROM tbsalesheader ORDER BY ID DESC"
            End With
            dr = cmd.ExecuteReader
            dr.Read()
            If dr.HasRows Then
                ID = Val(dr.Item(0)) + 1
            Else
                ID = 1
            End If
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Sub LoadDocNum()
        Try
            cn.Open()
            Dim invoicenum As String = "SELECT DocNumber FROM tbsalesheader ORDER BY ID" 'DESC
            cmd = New MySqlCommand(invoicenum, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                DocNumber = dr.GetString("DocNumber")
            End While
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Loading Report", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub PrintEmail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'LoadDocNum()
        'txtDocNum.Text = DocNumber
        ComboBox1.SelectedIndex = 0
        Label2.Select()

        Dim p As New System.Drawing.Printing.PrinterSettings()

        Dim defaultPrinterName As String

        defaultPrinterName = p.PrinterName
        PrinterToPrint.Text = defaultPrinterName

        For i As Integer = 0 To PayrollSchedule.DataGridView1.Rows.Count - 1
            cn.Open()
            Dim Namequery As String = "SELECT email,title,first_name,last_name FROM employee WHERE code = '" & PayrollSchedule.DataGridView1.Rows(i).Cells(0).Value & "'"
            cmd = New MySqlCommand(Namequery, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtTo.Text = dr("email").ToString()
            End While
            cn.Close()
        Next
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

    Dim second As Integer
    Dim phy_country As String
    Dim valueDb As String
    Sub loaddb()
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
                valueDb = "SSFR"
            ElseIf phy_country = "Zambia" Then
                valueDb = "NASPA"
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Loading Report", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim selectionDate1 As String
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Label2.Select()
        'For i = 1 To CInt(txtnum.Text)
        '    'TaxInvoice()
        'Next
        loaddb()

        For i As Integer = 0 To PayrollSchedule.DataGridView1.Rows.Count - 1
            Try
                cn.Open()
                Dim Namequery As String = "SELECT email,title,first_name,last_name FROM employee WHERE code = '" & PayrollSchedule.DataGridView1.Rows(i).Cells(0).Value & "'"
                cmd = New MySqlCommand(Namequery, cn)
                dr = cmd.ExecuteReader
                While dr.Read
                    txtTo.Text = dr("email").ToString()
                End While
                cn.Close()

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
                                WHERE emp_code = '" & PayrollSchedule.DataGridView1.Rows(i).Cells(0).Value & "' ORDER BY datepaid DESC LIMIT 1" ' AND datepaid = '" & Today.Date & "'"
                daT1 = New MySqlDataAdapter(strSQL, objConn)
                activecomp = New DataSet
                daT1.Fill(activecomp, "vwpayslip")

                Dim rpt As New ReportDocument

                Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
                rpt.Load(appPath + "\Reports\paysliprpt.rpt")
                selectionDate1 = valueDb

                With rpt
                    .SetDataSource(activecomp)
                    .SetParameterValue("DocType", selectionDate1)
                End With

                rpt.PrintOptions.PrinterName = PrinterToPrint.Text
                rpt.PrintToPrinter(1, False, 0, 0)
                objConn.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error Loading Report", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        Next

        RunPayroll.Close()
        Close()
    End Sub

    Public PrintEm As Char
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PrintEm = "Y"
        Label2.Select()
        PrinterList.ShowDialog()
    End Sub
End Class