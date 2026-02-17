Public Class ViewID
    Public Property IDImage As Image

    Private Sub ViewID_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If IDImage IsNot Nothing Then
            PictureBox1.Image = IDImage
            PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        Else
            PictureBox1.Image = Nothing
        End If
    End Sub

    ' Optional: Close popup if user clicks the image
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.FindForm.Close()
    End Sub
End Class
