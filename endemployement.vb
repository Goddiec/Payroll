Public Class endemployement
    Private Sub endemployement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label2.Select()
        comboReason.SelectedIndex = 0
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label2.Select()
        Close()
    End Sub
End Class