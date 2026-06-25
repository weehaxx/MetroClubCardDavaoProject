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
            btnPrint.Visible = False
            Me.Refresh()

            ' 🔥 CLEAN CAPTURE
            Dim bmp As New System.Drawing.Bitmap(Me.Width, Me.Height,
                                                 System.Drawing.Imaging.PixelFormat.Format32bppArgb)

            Me.DrawToBitmap(bmp, New System.Drawing.Rectangle(0, 0, Me.Width, Me.Height))

            ' 🔥 REMOVE WHITE PIXEL HALO (IMPORTANT FIX)
            Dim bmpFlat As New System.Drawing.Bitmap(bmp.Width, bmp.Height,
                                                     System.Drawing.Imaging.PixelFormat.Format24bppRgb)

            Using g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(bmpFlat)
                g.Clear(System.Drawing.Color.White)
                g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height)
            End Using

            bmp.Dispose()

            btnPrint.Visible = True

            ' C80 / CR80 size
            Dim cardWidth As Single = 85.6F * 2.83465F
            Dim cardHeight As Single = 54.0F * 2.83465F

            Using sfd As New SaveFileDialog()
                sfd.Filter = "PDF Files|*.pdf"
                sfd.FileName = "IDCard.pdf"

                If sfd.ShowDialog() <> DialogResult.OK Then Exit Sub

                Using fs As New FileStream(sfd.FileName, FileMode.Create)

                    Dim doc As New iTextSharp.text.Document(
                        New iTextSharp.text.Rectangle(cardWidth, cardHeight),
                        0, 0, 0, 0
                    )

                    Dim writer = PdfWriter.GetInstance(doc, fs)
                    doc.Open()

                    Using ms As New MemoryStream()

                        ' 🔥 USE JPEG (removes edge artifacts)
                        bmpFlat.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)

                        Dim img As iTextSharp.text.Image =
                            iTextSharp.text.Image.GetInstance(ms.ToArray())

                        ' 🔥 NO SCALETOFIT (causes pixel halo)
                        img.ScaleAbsolute(cardWidth, cardHeight)
                        img.SetAbsolutePosition(0, 0)

                        doc.Add(img)

                    End Using

                    doc.Close()
                    writer.Close()

                End Using
            End Using

            MessageBox.Show("✅ ID Saved Successfully")

            Me.FindForm().Close()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        PrintToC80PDF()
    End Sub

    Private Sub lblName_Click(sender As Object, e As EventArgs) Handles lblName.Click

    End Sub
End Class