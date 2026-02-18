Imports System.Data.SQLite
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO

Public Class Reports
    Private conn As SQLiteConnection
    Private rawValues As New Dictionary(Of String, Decimal) ' 🔹 Store real signed values
    Private totalCashIn As Decimal = 0
    Private totalCashOut As Decimal = 0
    Private daysInMonth As Integer = 31



    Private Function GetDatabasePath() As String
        Dim appDataPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MetroCardClubDavao")
        If Not Directory.Exists(appDataPath) Then
            Directory.CreateDirectory(appDataPath)
        End If

        Dim dbPath As String = Path.Combine(appDataPath, "metrocarddavaodb.db")
        If Not File.Exists(dbPath) Then
            ' 🔹 Create an empty SQLite file if it doesn’t exist yet
            SQLiteConnection.CreateFile(dbPath)
        End If

        Return dbPath
    End Function

    Private Sub Reports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Setup DB connection
        Dim dbPath As String = GetDatabasePath()
        conn = New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")

        ' ✅ DateTimePicker setup (Month-Year only)
        dtpMonthYear.Format = DateTimePickerFormat.Custom
        dtpMonthYear.CustomFormat = "MMMM yyyy"
        dtpMonthYear.ShowUpDown = True
        dtpMonthYear.Value = New DateTime(Date.Today.Year, Date.Today.Month, 1)

        ' Initial load
        LoadMonthlyReport()
    End Sub

    Private Sub LoadMonthlyReport()
        Try
            conn.Open()


            Dim selectedMonth As Integer = dtpMonthYear.Value.Month
            Dim selectedYear As Integer = dtpMonthYear.Value.Year
            Dim daysInMonth As Integer = 31 ' always 31 columns

            rawValues.Clear()
            dgvReports.Columns.Clear()
            dgvReports.Rows.Clear()
            dgvReports.AllowUserToAddRows = False
            dgvReports.RowHeadersVisible = False
            dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            dgvReports.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            dgvReports.ColumnHeadersHeight = 40
            dgvReports.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvReports.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvReports.Font = New System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Regular)
            dgvReports.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray

            ' Columns
            dgvReports.Columns.Add("PlayerName", "FULLNAME")
            dgvReports.Columns(0).Width = 110
            dgvReports.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            For i As Integer = 1 To daysInMonth
                dgvReports.Columns.Add($"Day{i}", i.ToString())
                dgvReports.Columns($"Day{i}").Width = 80
                dgvReports.Columns($"Day{i}").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            Next

            dgvReports.Columns.Add("Total", "TOTAL")
            dgvReports.Columns("Total").Width = 100
            dgvReports.Columns("Total").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Query transactions
            Dim sql As String = "
            SELECT r.id AS ID,
                   r.firstname, r.middlename, r.lastname,
                   c.session_date,
                   c.date_created,
                   c.type,
                   c.amount
            FROM registrations r
            LEFT JOIN cashflows c ON r.id = c.registration_id
            ORDER BY r.lastname, r.firstname"
            Dim cmd As New SQLiteCommand(sql, conn)
            Dim reader As SQLiteDataReader = cmd.ExecuteReader()

            Dim playerData As New Dictionary(Of Long, (FullName As String, Days As Decimal(), HasActivity As Boolean))
            Dim totalCashIn As Decimal = 0
            Dim totalCashOut As Decimal = 0

            ' Build player dictionary
            While reader.Read()
                Dim regID As Long = CLng(reader("ID"))
                Dim fullname As String = $"{reader("firstname")} {reader("middlename")} {reader("lastname")}".Replace("  ", " ").Trim()

                If Not playerData.ContainsKey(regID) Then
                    playerData(regID) = (fullname, New Decimal(daysInMonth - 1) {}, False)
                End If

                Dim dateStr As String
                If Not IsDBNull(reader("session_date")) AndAlso reader("session_date").ToString().Trim() <> "" Then
                    dateStr = reader("session_date").ToString()
                Else
                    dateStr = reader("date_created").ToString()
                End If

                Dim txDate As Date
                If Date.TryParse(dateStr, txDate) Then
                    If txDate.Year = selectedYear AndAlso txDate.Month = selectedMonth Then
                        Dim dayIndex As Integer = txDate.Day - 1
                        Dim amount As Decimal = Convert.ToDecimal(reader("amount"))
                        Dim txType As String = reader("type").ToString()

                        playerData(regID) = (playerData(regID).FullName,
                     playerData(regID).Days,
                     True)


                        If txType = "Buy-In" Then
                            playerData(regID).Days(dayIndex) -= amount
                            totalCashIn += amount
                        ElseIf txType = "Cash-Out" Then
                            playerData(regID).Days(dayIndex) += amount
                            totalCashOut += amount
                        End If
                    End If
                End If
            End While
            reader.Close()

            ' Add players to DataGridView (skip zero transactions)
            For Each kvp In playerData

                ' ✅ Show only players who had at least one transaction in the selected month
                If Not kvp.Value.HasActivity Then Continue For

                Dim days() As Decimal = kvp.Value.Days

                Dim rowIndex As Integer = dgvReports.Rows.Add()

                dgvReports.Rows(rowIndex).Cells(0).Value = kvp.Value.FullName

                Dim playerTotal As Decimal = 0
                For i As Integer = 0 To days.Length - 1
                    Dim val As Decimal = days(i)

                    dgvReports.Rows(rowIndex).Cells(i + 1).Value = Math.Abs(val).ToString("N0")
                    dgvReports.Rows(rowIndex).Cells(i + 1).Style.ForeColor = If(val < 0, Color.Red, Color.Black)
                    dgvReports.Rows(rowIndex).Cells(i + 1).Style.Alignment = DataGridViewContentAlignment.MiddleCenter

                    rawValues($"{rowIndex}_{i + 1}") = val
                    playerTotal += val
                Next

                dgvReports.Rows(rowIndex).Cells(daysInMonth + 1).Value = Math.Abs(playerTotal).ToString("N0")
                dgvReports.Rows(rowIndex).Cells(daysInMonth + 1).Style.ForeColor = If(playerTotal < 0, Color.Red, Color.Black)
                dgvReports.Rows(rowIndex).Cells(daysInMonth + 1).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                rawValues($"{rowIndex}_{daysInMonth + 1}") = playerTotal
            Next

            dgvReports.ClearSelection()
            dgvReports.CurrentCell = Nothing

            ' Update labels
            lblCashIn.Text = totalCashIn.ToString("N0")
            lblCashOut.Text = totalCashOut.ToString("N0")

        Catch ex As Exception
            MessageBox.Show("Error loading monthly report: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub


    ' ✅ Reload on date change
    Private Sub dtpMonthYear_ValueChanged(sender As Object, e As EventArgs) Handles dtpMonthYear.ValueChanged
        LoadMonthlyReport()
    End Sub

    Private Sub dtpMonthYear_CloseUp(sender As Object, e As EventArgs) Handles dtpMonthYear.CloseUp
        LoadMonthlyReport()
    End Sub

    Private Sub btnPrintMonthly_Click(sender As Object, e As EventArgs) Handles btnPrintMonthly.Click
        Try
            Dim saveDialog As New SaveFileDialog()
            saveDialog.Filter = "PDF Files|*.pdf"
            saveDialog.Title = "Save Monthly Report"
            saveDialog.FileName = $"MonthlyReport_{dtpMonthYear.Value.ToString("MMMM_yyyy")}.pdf"

            If saveDialog.ShowDialog() = DialogResult.OK Then
                Dim doc As New Document(PageSize.LEGAL.Rotate(), 20, 20, 20, 20)
                PdfWriter.GetInstance(doc, New FileStream(saveDialog.FileName, FileMode.Create))
                doc.Open()

                ' Title
                Dim titleFont As iTextSharp.text.Font = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD)
                Dim para As New Paragraph($"Monthly Report - {dtpMonthYear.Value.ToString("MMMM yyyy")}", titleFont)
                para.Alignment = Element.ALIGN_CENTER
                para.SpacingAfter = 20
                doc.Add(para)

                ' Table
                Dim pdfTable As New PdfPTable(dgvReports.Columns.Count)
                pdfTable.WidthPercentage = 100
                pdfTable.HeaderRows = 1

                Dim widths(dgvReports.Columns.Count - 1) As Single
                widths(0) = 110
                For i As Integer = 1 To dgvReports.Columns.Count - 2
                    widths(i) = 80
                Next
                widths(dgvReports.Columns.Count - 1) = 100
                pdfTable.SetWidths(widths)

                ' Column headers
                For Each col As DataGridViewColumn In dgvReports.Columns
                    Dim cell As New PdfPCell(New Phrase(col.HeaderText, FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)))
                    cell.HorizontalAlignment = Element.ALIGN_CENTER
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY
                    pdfTable.AddCell(cell)
                Next

                ' Rows
                For Each row As DataGridViewRow In dgvReports.Rows
                    For colIndex As Integer = 0 To dgvReports.Columns.Count - 1
                        Dim text As String = If(row.Cells(colIndex).Value IsNot Nothing, row.Cells(colIndex).Value.ToString(), "")
                        Dim font As iTextSharp.text.Font = FontFactory.GetFont("Arial", 7)
                        Dim key As String = $"{row.Index}_{colIndex}"

                        If rawValues.ContainsKey(key) Then
                            Dim signedValue As Decimal = rawValues(key)
                            text = Math.Abs(signedValue).ToString("N0")
                            If signedValue < 0 Then font.Color = BaseColor.RED
                        End If

                        Dim pdfCell As New PdfPCell(New Phrase(text, font))
                        pdfCell.HorizontalAlignment = If(colIndex = 0, Element.ALIGN_LEFT, Element.ALIGN_CENTER)
                        pdfTable.AddCell(pdfCell)
                    Next
                Next

                doc.Add(pdfTable)

                ' ✅ Use lblCashIn and lblCashOut values
                Dim totalsFont As iTextSharp.text.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)
                Dim buyInValue As Decimal = 0
                Dim cashOutValue As Decimal = 0

                Decimal.TryParse(lblCashIn.Text.Replace(",", ""), buyInValue)
                Decimal.TryParse(lblCashOut.Text.Replace(",", ""), cashOutValue)

                doc.Add(New Paragraph($"TOTAL BUY-IN: {buyInValue:N0}", totalsFont))
                doc.Add(New Paragraph($"TOTAL CASH-OUT: {cashOutValue:N0}", totalsFont))

                doc.Close()
                MessageBox.Show("Monthly Report exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error exporting report: " & ex.Message)
        End Try
    End Sub

    Private Sub lblCashIn_Click(sender As Object, e As EventArgs) Handles lblCashIn.Click

    End Sub

    Private Sub lblCashOut_Click(sender As Object, e As EventArgs) Handles lblCashOut.Click

    End Sub
End Class
