Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports ZXing ' ✅ ZXing.Net for barcode generation
Imports ZXing.Rendering


Public Class IDPrinting
    ' 🔹 Public properties to receive data
    Public Property MemberName As String
    Public Property MemberID As String
    Public Property MemberPhoto As System.Drawing.Image


    Private Sub IDPrinting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Display the received data
        AdjustIDLabel()

        lblName.Text = MemberName
        lblMemberID.Text = MemberID

        ' ✅ Show photo if exists
        If MemberPhoto IsNot Nothing Then
            pbIDphoto.Image = MemberPhoto
        Else
            pbIDphoto.Image = Nothing
        End If

        ' ✅ Generate barcode based on MemberID
        GenerateBarcode(MemberID)
    End Sub

    ' 🔹 Automatically adjust alignment of the ID label
    Private Sub lblID_TextChanged(sender As Object, e As EventArgs) Handles lblMemberID.TextChanged
        AdjustIDLabel()
    End Sub

    Private Sub AdjustIDLabel()
        If lblMemberID Is Nothing OrElse lblMemberID.Parent Is Nothing Then Return

        Dim parentWidth As Integer = lblMemberID.Parent.ClientSize.Width
        Using g As Graphics = lblMemberID.CreateGraphics()
            Dim textSize As SizeF = g.MeasureString(lblMemberID.Text, lblMemberID.Font)
            lblMemberID.Width = CInt(textSize.Width)
            lblMemberID.Left = parentWidth - lblMemberID.Width - 40 ' adjust margin
        End Using
    End Sub


    ' 🔹 Generate barcode image using ZXing.Net (robust: uses BarcodeWriterPixelData -> Bitmap)
    Private Sub GenerateBarcode(value As String)
        Try
            ' create a pixel-data writer (no renderer dependency)
            Dim pw As New ZXing.BarcodeWriterPixelData() With {
            .Format = ZXing.BarcodeFormat.CODE_128,
            .Options = New ZXing.Common.EncodingOptions With {
                .Width = Math.Max(1, pbBarcode.Width),
                .Height = Math.Max(1, pbBarcode.Height),
                .Margin = 2,
                .PureBarcode = True
            }
        }

            ' get the raw pixel data
            Dim pixelData = pw.Write(value)

            ' create a bitmap from the raw pixel bytes (ZXing gives BGRA-ish bytes)
            Dim bmp As New Bitmap(pixelData.Width, pixelData.Height, Imaging.PixelFormat.Format32bppArgb)

            ' copy bytes into the bitmap
            Dim bmpData = bmp.LockBits(New System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), Imaging.ImageLockMode.WriteOnly, bmp.PixelFormat)

            Try
                ' pixelData.Pixels is a byte() in RGBA order in many ZXing builds — copying directly usually works for 32bpp ARGB bitmaps
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bmpData.Scan0, pixelData.Pixels.Length)
            Finally
                bmp.UnlockBits(bmpData)
            End Try

            ' optional: resize to fit PictureBox exactly (maintain aspect if you want)
            Dim finalBmp As Bitmap = New Bitmap(bmp, pbBarcode.Width, pbBarcode.Height)

            pbBarcode.Image = finalBmp

            ' cleanup
            bmp.Dispose()
            If Not finalBmp Is bmp Then
                ' finalBmp kept for display
            End If

        Catch ex As Exception
            MessageBox.Show("Error generating barcode: " & ex.Message, "Barcode Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub




    ' 🔹 Print form to PDF (C80 size)
    Private Sub PrintToC80PDF()
        Try
            btnPrint.Visible = False
            Me.Refresh()

            Dim bmp As New Bitmap(Me.Width, Me.Height)
            Using g As Graphics = Graphics.FromImage(bmp)
                Dim screenPoint As Point = Me.PointToScreen(Point.Empty)
                g.CopyFromScreen(screenPoint.X, screenPoint.Y, 0, 0, Me.Size)
            End Using

            btnPrint.Visible = True

            ' Define C80 card size (in points)
            Dim cardWidth As Single = 85.6F * 2.83465F
            Dim cardHeight As Single = 54.0F * 2.83465F

            Using sfd As New SaveFileDialog()
                sfd.Filter = "PDF Files|*.pdf"
                sfd.FileName = "IDCard.pdf"

                If sfd.ShowDialog() <> DialogResult.OK Then Exit Sub

                Using fs As New FileStream(sfd.FileName, FileMode.Create)
                    Dim doc As New Document(New Rectangle(cardWidth, cardHeight), 0, 0, 0, 0)
                    Dim writer = PdfWriter.GetInstance(doc, fs)
                    doc.Open()

                    Using ms As New MemoryStream()
                        bmp.Save(ms, Imaging.ImageFormat.Png)
                        Dim img As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(ms.ToArray())

                        ' Scale image to fill card
                        Dim scaleX As Single = cardWidth / img.Width
                        Dim scaleY As Single = cardHeight / img.Height
                        Dim scale As Single = Math.Max(scaleX, scaleY)
                        img.ScalePercent(scale * 100)

                        Dim offsetX As Single = (cardWidth - img.ScaledWidth) / 2
                        Dim offsetY As Single = (cardHeight - img.ScaledHeight) / 2
                        img.SetAbsolutePosition(offsetX, offsetY)
                        doc.Add(img)
                    End Using

                    doc.Close()
                    writer.Close()
                End Using
            End Using

            MessageBox.Show("✅ ID successfully saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.FindForm().Close()
        Catch ex As Exception
            MessageBox.Show("Error printing ID: " & ex.Message)
        End Try
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        PrintToC80PDF()
    End Sub

    Private Sub lblMemberID_Click(sender As Object, e As EventArgs) Handles lblMemberID.Click

    End Sub
End Class
