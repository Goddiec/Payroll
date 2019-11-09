Public Class RunScripts
    Dim second As Integer
    Public updatedata As New scripts

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        second = second + 1
        If second >= 10 Then
            Timer1.Stop()
            updatedata.DataBase()
        End If
    End Sub

    Private Sub RunScripts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Interval = 10
        Timer1.Start()
    End Sub
End Class