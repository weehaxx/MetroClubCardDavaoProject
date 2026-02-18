Imports System.Data.SQLite
Imports System.Globalization
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO

Public Class Ledger
    Public Property SelectedRegistrationID As Long
    Public Property SelectedRegistrationCode As String
    Public Property SelectedFullName As String

    Private conn As SQLiteConnection

    Private Sub Ledger_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tbMemeberID.Text = SelectedRegistrationCode
        tbPlayerName.Text = SelectedFullName

        Dim dbPath As String = GetDatabasePath()
        conn = New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")


        LoadLedger()
    End Sub
    Private Function GetDatabasePath() As String
        Dim appDataPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MetroCardClubDavao")
        If Not Directory.Exists(appDataPath) Then
            Directory.CreateDirectory(appDataPath)
        End If
        Return Path.Combine(appDataPath, "metrocarddavaodb.db")
    End Function

    Private Sub LoadLedger()
        Try
            Dim dbPath As String = GetDatabasePath()
            Using conn As New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
                conn.Open()

                Dim sql As String = "
                    SELECT 
                        session_date AS SessionDate,
                        date_created AS ActualDate,
                        MIN(CASE WHEN type='Buy-In' THEN time_created END) AS FirstIn,
                        SUM(CASE WHEN type='Buy-In' THEN amount ELSE 0 END) AS TotalBuyIn,
                        SUM(CASE WHEN type='Buy-In' THEN 1 ELSE 0 END) AS BuyInFreq,
                        SUM(CASE WHEN type='Cash-Out' THEN amount ELSE 0 END) AS TotalCashOut,
                        MAX(CASE WHEN type='Cash-Out' THEN time_created END) AS LastOut,
                        SUM(CASE WHEN type='Cash-Out' THEN 1 ELSE 0 END) AS CashOutFreq
                    FROM cashflows
                    WHERE registration_id = @regID
                    GROUP BY session_date
                    ORDER BY session_date;
                "


                Dim finalTable As New DataTable()
                finalTable.Columns.Add("SESSION DATE")
                finalTable.Columns.Add("ACTUAL DATE")
                finalTable.Columns.Add("FIRST TIME IN")
                finalTable.Columns.Add("TOTAL BUY-IN")
                finalTable.Columns.Add("BUY-IN FREQ")
                finalTable.Columns.Add("TOTAL CASH-OUT")
                finalTable.Columns.Add("LAST TIME OUT")
                finalTable.Columns.Add("CASH-OUT FREQ")
                finalTable.Columns.Add("WIN/LOSS")
                finalTable.Columns.Add("CASHIER'S SIGNATURE")
                finalTable.Columns.Add("REMARKS")

                Using cmd As New SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@regID", SelectedRegistrationID)

                    Using reader As SQLiteDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim sessionDate As String =
                                DateTime.Parse(reader("SessionDate").ToString()).ToString("MMM dd, yyyy")

                            Dim actualDate As String =
                                DateTime.Parse(reader("ActualDate").ToString()).ToString("MMM dd, yyyy")

                            Dim firstIn As String = ""
                            If Not IsDBNull(reader("FirstIn")) Then
                                firstIn = DateTime.Parse(reader("FirstIn").ToString()).ToString("hh:mm tt")
                            End If

                            Dim totalBuyIn As Decimal = Convert.ToDecimal(reader("TotalBuyIn"))
                            Dim buyInFreq As Integer = Convert.ToInt32(reader("BuyInFreq"))
                            Dim totalCashOut As Decimal = Convert.ToDecimal(reader("TotalCashOut"))

                            Dim lastOut As String = ""
                            If Not IsDBNull(reader("LastOut")) Then
                                lastOut = DateTime.Parse(reader("LastOut").ToString()).ToString("hh:mm tt")
                            End If

                            Dim cashOutFreq As Integer = Convert.ToInt32(reader("CashOutFreq"))

                            Dim winLoss As Decimal = totalCashOut - totalBuyIn
                            Dim winLossStr As String = "₱" & winLoss.ToString("N2")

                            Dim newRow = finalTable.NewRow()
                            newRow("SESSION DATE") = sessionDate
                            newRow("ACTUAL DATE") = actualDate
                            newRow("FIRST TIME IN") = firstIn
                            newRow("TOTAL BUY-IN") = "₱" & totalBuyIn.ToString("N2")
                            newRow("BUY-IN FREQ") = buyInFreq
                            newRow("TOTAL CASH-OUT") = "₱" & totalCashOut.ToString("N2")
                            newRow("LAST TIME OUT") = lastOut
                            newRow("CASH-OUT FREQ") = cashOutFreq
                            newRow("WIN/LOSS") = winLossStr
                            newRow("CASHIER'S SIGNATURE") = ""  ' leave blank for signature
                            newRow("REMARKS") = ""
                            finalTable.Rows.Add(newRow)
                        End While
                    End Using
                End Using

                dgvLedger.DataSource = finalTable
            End Using

            StyleLedgerGrid()

            ' Coloring WIN/LOSS
            For Each row As DataGridViewRow In dgvLedger.Rows
                If row.Cells("WIN/LOSS").Value IsNot Nothing Then
                    Dim valStr As String = row.Cells("WIN/LOSS").Value.ToString().Replace("₱", "").Trim()
                    Dim val As Decimal
                    If Decimal.TryParse(valStr, val) Then
                        If val < 0 Then
                            row.Cells("WIN/LOSS").Style.ForeColor = Color.Red
                            row.Cells("WIN/LOSS").Style.Font = New Drawing.Font("Segoe UI", 10, Drawing.FontStyle.Bold)
                        Else
                            row.Cells("WIN/LOSS").Style.ForeColor = Color.Black
                            row.Cells("WIN/LOSS").Style.Font = New Drawing.Font("Segoe UI", 10, Drawing.FontStyle.Bold)
                        End If
                    End If
                End If
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading ledger: " & ex.Message)
        End Try
    End Sub

    Private Sub StyleLedgerGrid()
        With dgvLedger
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .ColumnHeadersHeight = 40
            .EnableHeadersVisualStyles = False
            .ColumnHeadersDefaultCellStyle.BackColor = Color.MidnightBlue
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Segoe UI", 10, Drawing.FontStyle.Bold)
            .DefaultCellStyle.Font = New Drawing.Font("Segoe UI", 10)
            .DefaultCellStyle.ForeColor = Color.Black
            .AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue
            .DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue
            .DefaultCellStyle.SelectionForeColor = Color.Black
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End With
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Dim parentForm As Form = Me.FindForm()
        If parentForm IsNot Nothing Then
            parentForm.Close()
        End If
    End Sub

    ' 🔹 PRINT LEDGER TO PDF (Landscape + space for signature)
    Private Sub btnLedgerPrint_Click(sender As Object, e As EventArgs) Handles btnLedgerPrint.Click
        Try
            Dim fileNameBase As String = SelectedFullName.Replace(" ", "_")
            Dim safeName As String = New String(fileNameBase.Where(Function(c) Char.IsLetterOrDigit(c) OrElse c = "_"c).ToArray())
            If String.IsNullOrWhiteSpace(safeName) Then safeName = "Member"

            Dim defaultFileName As String = safeName & "_Ledger.pdf"

            Dim sfd As New SaveFileDialog()
            sfd.Filter = "PDF Files (*.pdf)|*.pdf"
            sfd.FileName = defaultFileName

            If sfd.ShowDialog() <> DialogResult.OK Then
                Return
            End If

            Dim savePath As String = sfd.FileName

            ' Landscape A4
            Dim doc As New Document(PageSize.A4.Rotate(), 40, 40, 40, 40)
            PdfWriter.GetInstance(doc, New FileStream(savePath, FileMode.Create))
            doc.Open()

            ' Title
            Dim titleFont As iTextSharp.text.Font = FontFactory.GetFont("Helvetica", 14, iTextSharp.text.Font.BOLD)
            doc.Add(New Paragraph("Daily Ledger Report", titleFont))
            doc.Add(New Paragraph("Member: " & SelectedFullName & " (" & SelectedRegistrationCode & ")", FontFactory.GetFont("Arial", 10)))
            doc.Add(New Paragraph("Generated On: " & DateTime.Now.ToString("MMM dd, yyyy hh:mm tt"), FontFactory.GetFont("Arial", 9)))
            doc.Add(New Paragraph(" "))

            ' Table
            Dim pdfTable As New PdfPTable(dgvLedger.Columns.Count)
            pdfTable.WidthPercentage = 100

            ' Custom widths (more room for signature column)
            Dim widths(dgvLedger.Columns.Count - 1) As Single
            For i As Integer = 0 To dgvLedger.Columns.Count - 1
                If dgvLedger.Columns(i).HeaderText = "CASHIER'S SIGNATURE" Then
                    widths(i) = 150
                Else
                    widths(i) = 80
                End If
            Next
            pdfTable.SetWidths(widths)

            ' Add headers
            For Each col As DataGridViewColumn In dgvLedger.Columns
                Dim cell As New PdfPCell(New Phrase(col.HeaderText, FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)))
                cell.BackgroundColor = BaseColor.LIGHT_GRAY
                cell.HorizontalAlignment = Element.ALIGN_CENTER
                pdfTable.AddCell(cell)
            Next

            ' Add rows
            For Each row As DataGridViewRow In dgvLedger.Rows
                If Not row.IsNewRow Then
                    For Each cell As DataGridViewCell In row.Cells
                        Dim cellValue As String = If(cell.Value, "").ToString()
                        Dim phrase As New Phrase(cellValue, FontFactory.GetFont("Arial", 9))

                        ' Color WIN/LOSS
                        If dgvLedger.Columns(cell.ColumnIndex).HeaderText = "WIN/LOSS" Then
                            Dim valStr As String = cellValue.Replace("₱", "").Trim()
                            Dim val As Decimal
                            If Decimal.TryParse(valStr, val) Then
                                If val < 0 Then
                                    phrase.Font.Color = BaseColor.RED
                                Else
                                    phrase.Font.Color = BaseColor.BLACK
                                End If
                                phrase.Font.SetStyle(iTextSharp.text.Font.BOLD)
                            End If
                        End If

                        pdfTable.AddCell(phrase)
                    Next
                End If
            Next

            doc.Add(pdfTable)
            doc.Close()

            MessageBox.Show("Ledger PDF saved to: " & savePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error printing ledger: " & ex.Message)
        End Try
    End Sub

End Class
