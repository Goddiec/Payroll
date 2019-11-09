Public Class PrinterList
    Private Sub PrinterList()
        Dim sPrinters As String = ""
        ' POPULATE THE LIST BOX.
        ListBox1.Items.Clear()
        For Each sPrinters In System.Drawing.Printing.PrinterSettings.InstalledPrinters
            ListBox1.Items.Add(sPrinters)
        Next
    End Sub

    Private Sub PrinterList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PrinterList()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListBox1.Select()
        Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListBox1.Select()
        If PrintEmail.PrintEm = "Y" Then
            PrintEmail.PrinterToPrint.Text = ListBox1.SelectedItem
            Close()
            PrintEmail.PrintEm = "N"
        ElseIf PrintOrEmailPayslip.PrintEmpay = "Y" Then
            PrintOrEmailPayslip.PrinterToPrint.Text = ListBox1.SelectedItem
            Close()
            PrintOrEmailPayslip.PrintEmPay = "N"
        End If
    End Sub
End Class