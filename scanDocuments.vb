Imports System.IO
Imports System.Runtime.InteropServices
Imports MySql.Data.MySqlClient
Public Class scanDocuments
    Dim cn As New MySqlConnection("server = " & DatabaseSetting.txt_server.Text & "; username = " & DatabaseSetting.txt_uid.Text & "; password = " & DatabaseSetting.txt_pwd.Text & "; database = " & DatabaseSetting.txt_database.Text & ";port = " & DatabaseSetting.txt_port.Text & ";")
    Dim cmd As New MySqlCommand
    Dim dt As New DataTable
    Dim dr As MySqlDataReader
    Dim da As New MySqlDataAdapter

    <DllImport("ScanSetting.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Public Shared Function GetImageDialog(hWndParent As IntPtr, pwsFolder As String, pwsFilename As String, ByRef plNumFiles As IntPtr, ByRef p2 As IntPtr, ByRef pWiaItem21 As IntPtr, pWiaItem22 As IntPtr, nDisplay As Integer) As Integer
    End Function

    Dim Itemexist As Char
    Private Sub CheckItem()
        Try
            cn.Open()
            Dim Query As String
            Query = "SELECT code FROM employee WHERE code = '" & Trim(txtEmp.Text) & "'"
            cmd = New MySqlCommand(Query, cn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then

            Else
                MessageBox.Show("Employee Code does not exist.", "Documents", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Button1.Enabled = False
                txtEmp.Clear()
                txtEmp.Select()
            End If
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Panel1.Select()
        Try
            If txtname.TextLength < 4 Then
                MessageBox.Show("Please enter at least five characters for document description!", "Documents", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtname.SelectAll()
            Else
                Button1.Enabled = True

                'user must have rights on folder
                Dim wsFolder As String = appPath & "\DOCUMENTS\"
                Dim wsFile As String = txtname.Text
                Dim plNumFiles As IntPtr = IntPtr.Zero
                Dim p2 As IntPtr = IntPtr.Zero
                Dim pWiaItem21 As IntPtr = IntPtr.Zero
                Dim pWiaItem22 As IntPtr = IntPtr.Zero
                'last parameter = 1 : scanner can be modified
                ' returns:
                'S_OK = 0
                'S_FALSE = 1 if cancel
                ' &H0021000B if modify scanner button
                Dim hr As Integer = GetImageDialog(Me.Handle, wsFolder, wsFile, plNumFiles, p2, pWiaItem21, pWiaItem22, 0)

                PictureBox1.Image = Image.FromFile(appPath & "\DOCUMENTS\" & txtname.Text & ".jpg")
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            PictureBox1.Image = Nothing
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Panel1.Select()
        Try
            Dim myAdapter As New MySqlDataAdapter
            Dim sqlquery = "SELECT * FROM employee WHERE code= '" + txtEmp.Text + "'"
            Dim myCommand As New MySqlCommand()
            myCommand.Connection = cn
            myCommand.CommandText = sqlquery
            myAdapter.SelectCommand = myCommand
            cn.Open()
            Dim ms As New MemoryStream

            Dim bm As Bitmap = New Bitmap(PictureBox1.Image)
            bm.Save(ms, PictureBox1.Image.RawFormat)

            Dim arrPic() As Byte = ms.GetBuffer()

            sqlquery = "INSERT INTO scandoc(code, Image, Name) VALUES (@code, @Image, @Name)"

            myCommand = New MySqlCommand(sqlquery, cn)
            myCommand.Parameters.AddWithValue("@code", txtEmp.Text)
            myCommand.Parameters.AddWithValue("@Image", arrPic)
            myCommand.Parameters.AddWithValue("@Name", txtname.Text)
            myCommand.ExecuteNonQuery()
            cn.Close()
            MessageBox.Show("Document successfully uploaded.", "Employee", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Document", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Public Sub Periods()
        cn.Open()
        ComboBox1.Items.Clear()
        Dim store As String = "SELECT * FROM employee"
        cmd = New MySqlCommand(store, cn)
        dr = cmd.ExecuteReader
        While dr.Read
            Dim Code = dr.GetString("code").ToString
            ComboBox1.Items.Add(Code)
        End While
        cn.Close()
    End Sub

    Private Sub scanDocuments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtEmp.Select()
        Button1.Enabled = False
        Periods()
    End Sub

    Private Sub txtEmp_Leave(sender As Object, e As EventArgs) Handles txtEmp.Leave
        If txtEmp.Text <> String.Empty Then
            CheckItem()
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

    End Sub

    Private Sub txtname_TextChanged(sender As Object, e As EventArgs) Handles txtname.TextChanged
        If txtname.TextLength > 4 Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If
    End Sub

    Dim WithEvents printDoc As New Printing.PrintDocument()

    Private Sub PrintImage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles printDoc.PrintPage
        e.Graphics.DrawImage(PictureBox1.Image, e.MarginBounds.Left, e.MarginBounds.Top)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Label1.Select()
        Close()

        For Each ChildForm As Form In MainInterface.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Label1.Select()
        Close()

        For Each ChildForm As Form In MainInterface.MdiChildren
            ChildForm.Close()
        Next

        StartWindow.WindowState = FormWindowState.Maximized
        StartWindow.Show()
        StartWindow.MdiParent = MainInterface
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Label1.Select()
        printDoc.Print()
    End Sub

    Public Sub DocumentsLoad()
        Try
            cn.Open()
            Dim store As String = "SELECT * FROM employee WHERE code = '" & ComboBox1.Text & "'"
            cmd = New MySqlCommand(store, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                txtEmpName.Text = dr.GetString("first_name").ToString & " " & dr.GetString("last_name").ToString
            End While
            cn.Close()

            cn.Open()
            PictureBox1.Image = Nothing
            ComboBox2.Items.Clear()
            Dim store1 As String = "SELECT * FROM scandoc where  code = '" & ComboBox1.Text & "'"
            cmd = New MySqlCommand(store1, cn)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim Code = dr.GetString("Name").ToString
                ComboBox2.Items.Add(Code)
            End While
            cn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Document", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        DocumentsLoad()
    End Sub

    Private Sub imageRet()
        Try
            cn.Open()
            Dim cmd1 As MySqlCommand
            cmd1 = New MySqlCommand("Select Image from scandoc where code = '" & ComboBox1.Text & "' and Name = '" & ComboBox2.Text & "'", cn)
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

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        imageRet()
    End Sub

    Dim musicPath As String = "C:\Data\Files\MUSIC\Fav\Windows Logon.wav"
    Private Sub Button3_Click_1(sender As Object, e As EventArgs)
        My.Computer.Audio.Play(musicPath, AudioPlayMode.WaitToComplete)
    End Sub

    Public scanDoc As Char
    Private Sub Button3_Click_2(sender As Object, e As EventArgs) Handles Button3.Click
        scanDoc = "Y"
        SearchEmployee.ShowDialog()
    End Sub
End Class