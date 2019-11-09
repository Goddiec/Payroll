Public Class about
    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub


    Private Sub about_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label2.Select()
        Dim days As Integer = DateDiff(DateInterval.Day, System.DateTime.Today.Date, My.Settings.ExpiryDate.Date)
        Label1.Text = "Expiry Date: " & My.Settings.ExpiryDate.ToString("yyyy-MM-dd")
        Label6.Text = "Number of Days Left: " & days
        Label5.Text = "Number of Employees Registered: " & My.Settings.EmpNum
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)

    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label2.Select()
        Close()
    End Sub

    Private Sub LinkLabel1_LinkClicked_1(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim webAddress As String = "http://www.eposconsulting.co.za/"
        Process.Start(webAddress)
    End Sub

    Private Sub LinkLabel2_LinkClicked_1(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Process.Start("mailto:info@eposconsulting.co.za")
    End Sub

    Private Sub about_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub
End Class