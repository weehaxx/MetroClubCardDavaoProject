Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports ZXing

Public Class IDPrinting

    Public Property MemberName As String
    Public Property MemberID As String
    Public Property MemberPhoto As System.Drawing.Image

    Private lblNameRightEdge As Integer

    Private Sub IDPrinting_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Save the original right edge position
        lblNameRightEdge = lblName.Left + lblName.Width

        ' Make label auto-size
        lblName.AutoSize = True

        ' Set member name
        lblName.Text = MemberName

        ' Move label so its right edge stays fixed
        lblName.Left = lblNameRightEdge - lblName.Width

        lblMemberID.Text = MemberID

        If MemberPhoto IsNot Nothing Then
            pbIDphoto.Image = MemberPhoto
        Else
            pbIDphoto.Image = Nothing
        End If

        GenerateBarcode(MemberID)

    End Sub

    ' =========================
    ' BARCODE
    ' =========================
    Private Sub GenerateBarcode(value As String)

        Try
            Dim writer As New ZXing.BarcodeWriterPixelData() With {
                .Format = ZXing.BarcodeFormat.CODE_128,
                .Options = New ZXing.Common.EncodingOptions With {
                    .Width = Math.Max(1, pbBarcode.Width),
                    .Height = Math.Max(1, pbBarcode.Height),
                    .Margin = 2,
                    .PureBarcode = True
                }
            }

            Dim pixelData = writer.Write(value)

            Dim bmp As New System.Drawing.Bitmap(pixelData.Width, pixelData.Height,
                                                 System.Drawing.Imaging.PixelFormat.Format32bppArgb)

            Dim bmpData = bmp.LockBits(
                New System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                bmp.PixelFormat
            )

            Try
                Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bmpData.Scan0, pixelData.Pixels.Length)
            Finally
                bmp.UnlockBits(bmpData)
            End Try

            Dim finalBmp As New System.Drawing.Bitmap(bmp, pbBarcode.Width, pbBarcode.Height)
            pbBarcode.Image = finalBmp

            bmp.Dispose()

        Catch ex As Exception
            MessageBox.Show("Barcode Error: " & ex.Message)
        End Try

    End Sub

    ' =========================
    ' PRINT TO PDF (FIXED WHITE PIXEL ISSUE)
    ' =========================
    Private Sub PrintToC80PDF()

        Try

            Dim bmp As New Bitmap(Me.Width, Me.Height)

            Using g As Graphics = Graphics.FromImage(bmp)

                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

                ' Draw background
                If Me.BackgroundImage IsNot Nothing Then

                    g.DrawImage(
                    Me.BackgroundImage,
                    New System.Drawing.Rectangle(
                        0,
                        0,
                        Me.Width,
                        Me.Height))

                Else

                    g.Clear(Me.BackColor)

                End If

                ' Draw photo
                If pbIDphoto.Image IsNot Nothing Then

                    g.DrawImage(
                    pbIDphoto.Image,
                    pbIDphoto.Left,
                    pbIDphoto.Top,
                    pbIDphoto.Width,
                    pbIDphoto.Height)

                End If

                ' Draw barcode
                If pbBarcode.Image IsNot Nothing Then

                    g.DrawImage(
                    pbBarcode.Image,
                    pbBarcode.Left,
                    pbBarcode.Top,
                    pbBarcode.Width,
                    pbBarcode.Height)

                End If

                ' Draw Name
                Using br As New SolidBrush(lblName.ForeColor)

                    g.DrawString(
                    lblName.Text,
                    lblName.Font,
                    br,
                    lblName.Left,
                    lblName.Top)

                End Using

                ' Draw Member ID
                Using br As New SolidBrush(lblMemberID.ForeColor)

                    g.DrawString(
                    lblMemberID.Text,
                    lblMemberID.Font,
                    br,
                    lblMemberID.Left,
                    lblMemberID.Top)

                End Using

            End Using

            ' Optional test image
            Dim pngPath As String =
            Application.StartupPath & "\TestID.png"

            If File.Exists(pngPath) Then
                File.Delete(pngPath)
            End If

            bmp.Save(
            pngPath,
            System.Drawing.Imaging.ImageFormat.Png)

            ' CR80 Card Size
            Dim cardWidth As Single = 85.6F * 2.83465F
            Dim cardHeight As Single = 54.0F * 2.83465F

            Using sfd As New SaveFileDialog()

                sfd.Filter = "PDF Files|*.pdf"
                sfd.FileName = "IDCard.pdf"

                If sfd.ShowDialog() <> DialogResult.OK Then Exit Sub

                ' Overwrite existing file
                If File.Exists(sfd.FileName) Then

                    Try
                        File.Delete(sfd.FileName)

                    Catch ex As Exception

                        MessageBox.Show(
                        "Please close the existing PDF first.",
                        "File In Use",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)

                        Exit Sub

                    End Try

                End If

                Using fs As New FileStream(
                sfd.FileName,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None)

                    Dim doc As New iTextSharp.text.Document(
                    New iTextSharp.text.Rectangle(
                        cardWidth,
                        cardHeight),
                    0, 0, 0, 0)

                    PdfWriter.GetInstance(doc, fs)

                    doc.Open()

                    Using ms As New MemoryStream()

                        bmp.Save(
                        ms,
                        System.Drawing.Imaging.ImageFormat.Png)

                        Dim img As iTextSharp.text.Image =
                        iTextSharp.text.Image.GetInstance(
                            ms.ToArray())

                        img.ScaleAbsolute(
                        cardWidth,
                        cardHeight)

                        img.SetAbsolutePosition(0, 0)

                        doc.Add(img)

                    End Using

                    doc.Close()

                End Using

            End Using

            bmp.Dispose()

            MessageBox.Show(
            "✅ ID Saved Successfully",
            "Success",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

        Catch ex As Exception

            MessageBox.Show(
            ex.ToString(),
            "Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        PrintToC80PDF()
    End Sub

    Private Sub lblName_Click(sender As Object, e As EventArgs) Handles lblName.Click

    End Sub
End Class