Imports System.Data.SQLite
Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing.Printing

Public Class RaffleEntry

    Private dtRaffleEntries As DataTable ' Stores all raffle entries

    Private Sub RaffleEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadRaffleEntries()
        UpdateTotalRaffleEntries()
    End Sub

    Private Function GetDatabasePath() As String
        Dim appDataPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MetroCardClubDavao")
        If Not Directory.Exists(appDataPath) Then
            Directory.CreateDirectory(appDataPath)
        End If
        Return Path.Combine(appDataPath, "metrocarddavaodb.db")

    End Function
    Private Function IsTicketPrinted(raffleID As Integer) As Boolean

        Dim dbPath = GetDatabasePath()

        Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
            conn.Open()

            Dim sql As String =
            "SELECT is_printed FROM raffle WHERE id=@id"

            Using cmd As New SQLiteCommand(sql, conn)

                cmd.Parameters.AddWithValue("@id", raffleID)

                Dim result = cmd.ExecuteScalar()

                If result IsNot Nothing Then
                    Return Convert.ToInt32(result) = 1
                End If

            End Using

        End Using

        Return False

    End Function
    Private Sub MarkTicketPrinted(raffleID As Integer)

        Dim dbPath = GetDatabasePath()

        Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
            conn.Open()

            Dim sql As String =
            "UPDATE raffle SET is_printed=1 WHERE id=@id"

            Using cmd As New SQLiteCommand(sql, conn)

                cmd.Parameters.AddWithValue("@id", raffleID)
                cmd.ExecuteNonQuery()

            End Using

        End Using

    End Sub
    Private Sub SendCutCommand()
        Try
            Dim printerName As String = "Your Printer Name"

            Dim cutCommand() As Byte = {29, 86, 65, 0} ' ESC/POS Full Cut

            Dim printerPath As String = "\\.\\" & printerName

            Using fs As New FileStream(printerPath, FileMode.Open, FileAccess.Write)
                fs.Write(cutCommand, 0, cutCommand.Length)
            End Using

        Catch ex As Exception
            ' Printer might not support cut
        End Try
    End Sub
    Private Sub PrintSingleRaffle(raffleID As Integer)

        Dim dbPath As String = GetDatabasePath()
        Dim dtTicket As New DataTable()

        Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
            conn.Open()

            Dim sql As String = "
            SELECT full_name, raffle_number 
            FROM raffle 
            WHERE id = @id
        "

            Using cmd As New SQLiteCommand(sql, conn)
                cmd.Parameters.AddWithValue("@id", raffleID)
                Using adapter As New SQLiteDataAdapter(cmd)
                    adapter.Fill(dtTicket)
                End Using
            End Using
        End Using

        If dtTicket.Rows.Count = 0 Then
            If IsTicketPrinted(raffleID) Then

                If MessageBox.Show(
        "This ticket was already printed." &
        vbCrLf &
        "Do you want to print it again?",
        "Already Printed",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    ) = DialogResult.No Then

                    Return

                End If

            End If
            MessageBox.Show("Raffle entry not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim memberName As String = dtTicket.Rows(0)("full_name").ToString()
        Dim raffleNumber As String = dtTicket.Rows(0)("raffle_number").ToString()

        Dim pd As New PrintDocument()
        pd.DefaultPageSettings.Margins = New Margins(20, 20, 20, 20)

        AddHandler pd.PrintPage, Sub(sender2, e2)

                                     Dim g As Graphics = e2.Graphics
                                     g.Clear(Color.White)

                                     Dim fontTitle As New Font("Arial", 14, FontStyle.Bold)
                                     Dim fontHeader As New Font("Arial", 11, FontStyle.Bold)
                                     Dim fontText As New Font("Arial", 10)
                                     Dim fontBig As New Font("Arial", 20, FontStyle.Bold)

                                     Dim center As New StringFormat()
                                     center.Alignment = StringAlignment.Center

                                     Dim y As Integer = 10
                                     Dim centerX As Single = e2.PageBounds.Width / 2

                                     g.DrawString("METRO CARD CLUB DAVAO", fontHeader, Brushes.Black, centerX, y, center)
                                     y += 25

                                     g.DrawString("RAFFLE TICKET", fontTitle, Brushes.Black, centerX, y, center)
                                     y += 25

                                     g.DrawString("--------------------------------", fontText, Brushes.Black, centerX, y, center)
                                     y += 25

                                     g.DrawString("Member:", fontHeader, Brushes.Black, centerX, y, center)
                                     y += 20

                                     g.DrawString(memberName, fontText, Brushes.Black, centerX, y, center)
                                     y += 30

                                     g.DrawString("Ticket Number", fontHeader, Brushes.Black, centerX, y, center)
                                     y += 25

                                     g.DrawString("# " & raffleNumber, fontBig, Brushes.Black, centerX, y, center)
                                     y += 40

                                     g.DrawString("--------------------------------", fontText, Brushes.Black, centerX, y, center)
                                     y += 40 ' extra space for cutter

                                     e2.HasMorePages = False

                                 End Sub

        Dim printDlg As New PrintDialog() With {.Document = pd}
        If printDlg.ShowDialog() = DialogResult.OK Then
            Try
                pd.Print()
                SendCutCommand()
                MarkTicketPrinted(raffleID)
                LoadRaffleEntries()
            Catch ex As System.ComponentModel.Win32Exception
                MessageBox.Show("Cannot print because the file is in use. Close it and try again.", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Catch ex As Exception
                MessageBox.Show("An error occurred while printing: " & ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

    End Sub

    Private Sub dgvRaffleEntries_DataBindingComplete(
    sender As Object,
    e As DataGridViewBindingCompleteEventArgs
) Handles dgvRaffleEntries.DataBindingComplete

        For Each row As DataGridViewRow In dgvRaffleEntries.Rows

            Dim status As String = ""

            If row.Cells("PrintStatus").Value IsNot Nothing Then
                status = row.Cells("PrintStatus").Value.ToString()
            End If

            If status = "PRINTED" Then
                row.Cells("PrintStatus").Style.ForeColor = Color.Red
                row.Cells("PrintStatus").Style.Font =
        New Font(dgvRaffleEntries.Font, FontStyle.Bold)
            End If

        Next

    End Sub
    ' ================= LOAD RAFFLE ENTRIES =================
    Private Sub LoadRaffleEntries()
        Try
            Dim dbPath As String = GetDatabasePath()
            If Not File.Exists(dbPath) Then
                MessageBox.Show("Database not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
                conn.Open()
                Dim sql As String = "
                    SELECT r.id AS RaffleID,
                           r.raffle_number AS RaffleNumber,
                           r.full_name AS MemberName,
                           reg.registration_id AS RegistrationID,
                           r.raffle_date AS RaffleDate,
                           r.session_raffle_date AS SessionDate,
                           strftime('%I:%M %p', r.raffle_time) AS RaffleTime,
                           CASE
                               WHEN r.is_printed = 1 THEN 'PRINTED'
                               ELSE 'NOT PRINTED'
                           END AS PrintStatus
                    FROM raffle r
                    INNER JOIN registrations reg ON r.registration_id = reg.id
                    ORDER BY CAST(r.raffle_number AS INTEGER) ASC
                    "

                Using cmd As New SQLiteCommand(sql, conn)
                    Using adapter As New SQLiteDataAdapter(cmd)
                        dtRaffleEntries = New DataTable()
                        adapter.Fill(dtRaffleEntries)
                        ' ✅ Force proper time formatting
                        dgvRaffleEntries.DataSource = dtRaffleEntries
                        dgvRaffleEntries.AllowUserToAddRows = False
                    End Using
                End Using
            End Using
            dgvRaffleEntries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            dgvRaffleEntries.Columns("RaffleNumber").Width = 60
            dgvRaffleEntries.Columns("MemberName").Width = 200
            dgvRaffleEntries.Columns("RegistrationID").Width = 120
            dgvRaffleEntries.Columns("RaffleDate").Width = 110
            dgvRaffleEntries.Columns("SessionDate").Width = 110
            dgvRaffleEntries.Columns("RaffleTime").Width = 90

            dgvRaffleEntries.Columns("RaffleID").Visible = False

            ' Set column headers and formatting
            With dgvRaffleEntries
                .Columns("RaffleNumber").HeaderText = "Raffle #"
                .Columns("MemberName").HeaderText = "Member Name"
                .Columns("RegistrationID").HeaderText = "Reg ID"
                .Columns("RaffleDate").HeaderText = "Date"
                .Columns("SessionDate").HeaderText = "Session Date"
                .Columns("RaffleTime").HeaderText = "Time"
                .Columns("PrintStatus").HeaderText = "Status"
                .Columns("RaffleDate").DefaultCellStyle.Format = "yyyy-MM-dd"
                .Columns("SessionDate").DefaultCellStyle.Format = "yyyy-MM-dd"
                .AutoResizeColumns()
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

                dgvRaffleEntries.DataSource = dtRaffleEntries
                dgvRaffleEntries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                dgvRaffleEntries.Columns("RaffleNumber").FillWeight = 10
                dgvRaffleEntries.Columns("MemberName").FillWeight = 35
                dgvRaffleEntries.Columns("RegistrationID").FillWeight = 20
                dgvRaffleEntries.Columns("RaffleDate").FillWeight = 15
                dgvRaffleEntries.Columns("RaffleTime").FillWeight = 10
                dgvRaffleEntries.Columns("SessionDate").FillWeight = 15
                dgvRaffleEntries.AllowUserToAddRows = False

                ' ✅ Add Action column only if it does not exist
                If Not dgvRaffleEntries.Columns.Contains("Action") Then
                    Dim actionCol As New DataGridViewTextBoxColumn()
                    actionCol.Name = "Action"
                    actionCol.HeaderText = "Action"
                    actionCol.ReadOnly = True
                    dgvRaffleEntries.Columns.Add(actionCol)
                End If

                ' Force Action column to fixed width
                dgvRaffleEntries.Columns("Action").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                dgvRaffleEntries.Columns("Action").Width = 220



            End With

        Catch ex As Exception
            MessageBox.Show("Error loading raffle entries: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvRaffleEntries_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvRaffleEntries.CellMouseClick
        If e.RowIndex < 0 Then Exit Sub
        If e.ColumnIndex <> dgvRaffleEntries.Columns("Action").Index Then Exit Sub

        Dim cellRect As Rectangle = dgvRaffleEntries.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, True)
        Dim clickX As Integer = e.Location.X

        Dim btnWidth As Integer = (cellRect.Width - 12) \ 3

        Dim raffleID As Integer = Convert.ToInt32(dgvRaffleEntries.Rows(e.RowIndex).Cells("RaffleID").Value)
        Dim memberName As String = dgvRaffleEntries.Rows(e.RowIndex).Cells("MemberName").Value.ToString()

        If clickX <= btnWidth + 2 Then
            ' Edit clicked
            EditRaffleEntry(raffleID)
        ElseIf clickX <= (btnWidth * 2) + 6 Then
            ' Delete clicked
            DeleteRaffleEntry(raffleID)
        Else
            ' Print clicked → print raffle tickets for this member only
            ' Print only this specific entry
            PrintSingleRaffle(raffleID)
        End If
    End Sub

    Private Sub PrintRaffleForMember(memberName As String)

        Dim dbPath As String = GetDatabasePath()
        Dim dtTickets As New DataTable()

        Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
            conn.Open()

            Dim sql As String = "SELECT id,
                       raffle_number,
                       is_printed
                FROM raffle
                WHERE full_name=@name ORDER BY raffle_number ASC"

            Using cmd As New SQLiteCommand(sql, conn)
                cmd.Parameters.AddWithValue("@name", memberName)

                Using adapter As New SQLiteDataAdapter(cmd)
                    adapter.Fill(dtTickets)
                End Using
            End Using
        End Using

        If dtTickets.Rows.Count = 0 Then
            MessageBox.Show("No raffle entries found for this member.", "No Tickets")
            Return
        End If

        ' Count already printed tickets
        Dim alreadyPrinted As Integer =
    dtTickets.AsEnumerable().
    Count(Function(r)
              If IsDBNull(r("is_printed")) Then
                  Return False
              End If

              Return Convert.ToInt32(r("is_printed")) = 1
          End Function)

        If alreadyPrinted > 0 Then

            If MessageBox.Show(
        $"{alreadyPrinted} ticket(s) were already printed." &
        vbCrLf &
        "Print again?",
        "Already Printed",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    ) = DialogResult.No Then

                Return

            End If

        End If

        If MessageBox.Show($"Print {dtTickets.Rows.Count} raffle tickets for {memberName}?",
               "Print Confirmation",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        For Each row As DataRow In dtTickets.Rows

            Dim raffleID As Integer = Convert.ToInt32(row("id"))
            Dim raffleNumber As String = row("raffle_number").ToString()

            Try

                PrintSingleTicket(memberName, raffleNumber)

                MarkTicketPrinted(raffleID)

                Threading.Thread.Sleep(300)

                SendCutCommand()

            Catch ex As Exception

                MessageBox.Show(
            "Failed to print ticket #" & raffleNumber &
            vbCrLf &
            ex.Message,
            "Print Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)

            End Try

        Next

        LoadRaffleEntries()
    End Sub

    Private Sub PrintSingleTicket(memberName As String, raffleNumber As String)

        Dim pd As New PrintDocument()

        pd.DefaultPageSettings.Landscape = False
        pd.DefaultPageSettings.Margins = New Margins(20, 20, 20, 20)

        AddHandler pd.PrintPage, Sub(sender, e)

                                     Dim g As Graphics = e.Graphics
                                     g.Clear(Color.White)

                                     Dim fontTitle As New Font("Arial", 14, FontStyle.Bold)
                                     Dim fontHeader As New Font("Arial", 11, FontStyle.Bold)
                                     Dim fontText As New Font("Arial", 10)
                                     Dim fontBig As New Font("Arial", 20, FontStyle.Bold)

                                     Dim center As New StringFormat()
                                     center.Alignment = StringAlignment.Center

                                     Dim y As Integer = 10
                                     Dim centerX As Single = e.PageBounds.Width / 2

                                     g.DrawString("METRO CARD CLUB DAVAO", fontHeader, Brushes.Black, centerX, y, center)
                                     y += 25

                                     g.DrawString("RAFFLE TICKET", fontTitle, Brushes.Black, centerX, y, center)
                                     y += 25

                                     g.DrawString("--------------------------------", fontText, Brushes.Black, centerX, y, center)
                                     y += 25

                                     g.DrawString("Member:", fontHeader, Brushes.Black, centerX, y, center)
                                     y += 20

                                     g.DrawString(memberName, fontText, Brushes.Black, centerX, y, center)
                                     y += 30

                                     g.DrawString("Ticket Number", fontHeader, Brushes.Black, centerX, y, center)
                                     y += 25

                                     g.DrawString("# " & raffleNumber, fontBig, Brushes.Black, centerX, y, center)
                                     y += 40

                                     g.DrawString("--------------------------------", fontText, Brushes.Black, centerX, y, center)

                                 End Sub

        pd.Print()

    End Sub


    Private Sub dgvRaffleEntries_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvRaffleEntries.CellPainting
        If e.ColumnIndex = dgvRaffleEntries.Columns("Action").Index AndAlso e.RowIndex >= 0 Then
            e.PaintBackground(e.CellBounds, True)

            ' 3 buttons: Edit, Delete, Print
            Dim btnWidth As Integer = (e.CellBounds.Width - 12) \ 3
            Dim btnHeight As Integer = e.CellBounds.Height - 6

            Dim editRect As New Rectangle(e.CellBounds.Left + 2, e.CellBounds.Top + 3, btnWidth, btnHeight)
            Dim delRect As New Rectangle(editRect.Right + 4, e.CellBounds.Top + 3, btnWidth, btnHeight)
            Dim printRect As New Rectangle(delRect.Right + 4, e.CellBounds.Top + 3, btnWidth, btnHeight)

            ButtonRenderer.DrawButton(e.Graphics, editRect, "Edit", dgvRaffleEntries.Font, False, System.Windows.Forms.VisualStyles.PushButtonState.Default)
            ButtonRenderer.DrawButton(e.Graphics, delRect, "Delete", dgvRaffleEntries.Font, False, System.Windows.Forms.VisualStyles.PushButtonState.Default)
            ButtonRenderer.DrawButton(e.Graphics, printRect, "Print", dgvRaffleEntries.Font, False, System.Windows.Forms.VisualStyles.PushButtonState.Default)

            e.Handled = True
        End If
    End Sub

    ' ================= TOTAL RAFFLE ENTRIES =================
    Private Sub UpdateTotalRaffleEntries()
        Try
            Dim dbPath As String = GetDatabasePath()
            If Not File.Exists(dbPath) Then
                lblTotalRaffleEntries.Text = "0"
                Return
            End If

            Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
                conn.Open()
                Dim sql As String = "SELECT COUNT(*) FROM raffle"
                Using cmd As New SQLiteCommand(sql, conn)
                    Dim total As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    lblTotalRaffleEntries.Text = total.ToString()
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error counting raffle entries: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblTotalRaffleEntries.Text = "0"
        End Try
    End Sub



    ' ================= RESET RAFFLE ENTRIES =================
    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Try
            ' Confirm reset
            Dim confirmResult = MessageBox.Show(
                "Are you sure you want to delete ALL raffle entries? This cannot be undone.",
                "Confirm Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            )
            If confirmResult = DialogResult.No Then Exit Sub

            Dim dbPath As String = GetDatabasePath()
            Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
                conn.Open()

                ' Delete all raffle entries
                Dim sqlDelete As String = "DELETE FROM raffle"
                Using cmdDelete As New SQLiteCommand(sqlDelete, conn)
                    cmdDelete.ExecuteNonQuery()
                End Using

                ' Reset any autoincrement (if used)
                Dim sqlResetSeq As String = "DELETE FROM sqlite_sequence WHERE name='raffle'"
                Using cmdReset As New SQLiteCommand(sqlResetSeq, conn)
                    cmdReset.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("All raffle entries have been deleted and sequence reset.", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Refresh display
            LoadRaffleEntries()
            UpdateTotalRaffleEntries()

        Catch ex As Exception
            MessageBox.Show("Error resetting raffle entries: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub RenumberRaffleNumbers()
        Try
            Dim dbPath As String = GetDatabasePath()
            Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
                conn.Open()

                ' Get all raffle entries ordered by current raffle_date ASC (or DESC if you prefer)
                Dim sqlSelect As String = "SELECT id FROM raffle ORDER BY raffle_date ASC, id ASC"
                Dim raffleIDs As New List(Of Integer)

                Using cmd As New SQLiteCommand(sqlSelect, conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            raffleIDs.Add(Convert.ToInt32(reader("id")))
                        End While
                    End Using
                End Using

                ' Update raffle_number sequentially starting from 1
                For i As Integer = 0 To raffleIDs.Count - 1
                    Dim sqlUpdate As String = "UPDATE raffle SET raffle_number = @num WHERE id = @id"
                    Using cmd As New SQLiteCommand(sqlUpdate, conn)
                        cmd.Parameters.AddWithValue("@num", i + 1)
                        cmd.Parameters.AddWithValue("@id", raffleIDs(i))
                        cmd.ExecuteNonQuery()
                    End Using
                Next
            End Using
        Catch ex As Exception
            MessageBox.Show("Error renumbering raffle entries: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub btnprint_Click(sender As Object, e As EventArgs) Handles btnprint.Click

        Dim dbPath As String = GetDatabasePath()

        ' Get all raffle entries
        Dim dtTickets As New DataTable()

        Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
            conn.Open()

            Dim sql As String = "
        SELECT id,
           full_name,
           raffle_number,
           is_printed 
        FROM raffle 
        ORDER BY CAST(raffle_number AS INTEGER) ASC"

            Using cmd As New SQLiteCommand(sql, conn)
                Using adapter As New SQLiteDataAdapter(cmd)
                    adapter.Fill(dtTickets)
                End Using
            End Using
        End Using

        ' Count already printed tickets
        Dim alreadyPrinted As Integer =
    dtTickets.AsEnumerable().
    Count(Function(r)
              If IsDBNull(r("is_printed")) Then
                  Return False
              End If

              Return Convert.ToInt32(r("is_printed")) = 1
          End Function)

        If alreadyPrinted > 0 Then

            If MessageBox.Show(
        $"{alreadyPrinted} ticket(s) were already printed." &
        vbCrLf &
        "Print again?",
        "Already Printed",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    ) = DialogResult.No Then

                Return

            End If

        End If

        ' CONFIRMATION BEFORE PRINT
        If MessageBox.Show("Are you sure you want to print ALL raffle tickets?",
                   "Print Confirmation",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If


        For Each row As DataRow In dtTickets.Rows

            Dim raffleID As Integer = Convert.ToInt32(row("id"))
            Dim memberName As String = row("full_name").ToString()
            Dim raffleNumber As String = row("raffle_number").ToString()

            Try

                PrintSingleTicket(memberName, raffleNumber)

                MarkTicketPrinted(raffleID)

                Threading.Thread.Sleep(300)

                SendCutCommand()

            Catch ex As Exception

                MessageBox.Show(
            "Failed to print ticket #" & raffleNumber &
            vbCrLf &
            ex.Message,
            "Print Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)

            End Try

        Next

        LoadRaffleEntries()
    End Sub

    Private Sub tbSearch_TextChanged(sender As Object, e As EventArgs) Handles tbSearch.TextChanged
        If dtRaffleEntries Is Nothing Then Exit Sub

        Dim filterText As String = tbSearch.Text.Trim().Replace("'", "''") ' Escape quotes

        Dim dv As New DataView(dtRaffleEntries)
        If String.IsNullOrEmpty(filterText) Then
            dv.RowFilter = ""
        Else
            dv.RowFilter = $"MemberName LIKE '%{filterText}%' OR RegistrationID LIKE '%{filterText}%'"
        End If

        dgvRaffleEntries.DataSource = dv
    End Sub

    ' ================= EDIT RAFFLE ENTRY =================
    Private Sub EditRaffleEntry(raffleID As Integer)
        ' Find the row for this raffleID
        Dim row As DataGridViewRow = dgvRaffleEntries.Rows.Cast(Of DataGridViewRow)() _
            .FirstOrDefault(Function(r) Convert.ToInt32(r.Cells("RaffleID").Value) = raffleID)
        If row Is Nothing Then Return

        Dim currentDate As String = row.Cells("RaffleDate").Value.ToString()
        Dim currentTime As String = row.Cells("RaffleTime").Value.ToString()
        Dim currentSessionDate As String = row.Cells("SessionDate").Value.ToString()

        ' ===== Create dialog =====
        Dim editForm As New Form() With {
            .Text = "Edit Raffle Entry",
            .Size = New Size(300, 300),
            .StartPosition = FormStartPosition.CenterParent,
            .FormBorderStyle = FormBorderStyle.FixedDialog,
            .MaximizeBox = False,
            .MinimizeBox = False
        }

        Dim lblDate As New Label() With {.Text = "Raffle Date:", .Location = New Point(20, 20)}
        Dim dtpDate As New DateTimePicker() With {
            .Location = New Point(20, 45),
            .Width = 240,
            .Format = DateTimePickerFormat.Custom,
            .CustomFormat = "yyyy-MM-dd"
        }
        Dim lblSessionDate As New Label() With {.Text = "Session Date:", .Location = New Point(20, 80)}

        Dim dtpSessionDate As New DateTimePicker() With {
    .Location = New Point(20, 105),
    .Width = 240,
    .Format = DateTimePickerFormat.Custom,
    .CustomFormat = "yyyy-MM-dd"
}
        Dim lblTime As New Label() With {.Text = "Raffle Time:", .Location = New Point(20, 140)}
        Dim dtpTime As New DateTimePicker() With {
            .Location = New Point(20, 165),
            .Width = 240,
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True
        }
        Dim parsedTime As DateTime
        If DateTime.TryParse(currentTime, parsedTime) Then
            dtpTime.Value = parsedTime
        End If

        Dim parsedDate As DateTime
        If DateTime.TryParse(currentDate, parsedDate) Then
            dtpDate.Value = parsedDate
        End If
        Dim parsedSessionDate As DateTime
        If DateTime.TryParse(currentSessionDate, parsedSessionDate) Then
            dtpSessionDate.Value = parsedSessionDate
        End If

        Dim btnSave As New Button() With {.Text = "Save", .Location = New Point(50, 205), .DialogResult = DialogResult.OK}
        Dim btnCancel As New Button() With {.Text = "Cancel", .Location = New Point(150, 205), .DialogResult = DialogResult.Cancel}

        editForm.Controls.AddRange({
            lblDate, dtpDate,
            lblSessionDate, dtpSessionDate,
            lblTime, dtpTime,
            btnSave, btnCancel
        })
        editForm.AcceptButton = btnSave
        editForm.CancelButton = btnCancel

        If editForm.ShowDialog() = DialogResult.OK Then
            Dim newDate As String = dtpDate.Value.ToString("yyyy-MM-dd")
            Dim newSessionDate As String = dtpSessionDate.Value.ToString("yyyy-MM-dd")
            Dim newTime As String = dtpTime.Value.ToString("HH:mm:ss")
            ' Update DB
            Try
                Dim dbPath As String = GetDatabasePath()
                Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
                    conn.Open()
                    Dim sql As String = "
                    UPDATE raffle 
                    SET raffle_date=@date,
                        session_raffle_date=@sessiondate,
                        raffle_time=@time
                    WHERE id=@id"
                    Using cmd As New SQLiteCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@date", newDate)
                        cmd.Parameters.AddWithValue("@sessiondate", newSessionDate)
                        cmd.Parameters.AddWithValue("@time", newTime)
                        cmd.Parameters.AddWithValue("@id", raffleID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                MessageBox.Show("Raffle entry updated successfully.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadRaffleEntries()
            Catch ex As Exception
                MessageBox.Show("Error updating raffle entry: " & ex.Message)
            End Try
        End If
    End Sub

    ' ================= DELETE RAFFLE ENTRY =================
    Private Sub DeleteRaffleEntry(raffleID As Integer)
        Dim confirm = MessageBox.Show("Delete this raffle entry?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If confirm = DialogResult.No Then Exit Sub

        Dim dbPath As String = GetDatabasePath()
        Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
            conn.Open()
            Dim sql As String = "DELETE FROM raffle WHERE id=@id"
            Using cmd As New SQLiteCommand(sql, conn)
                cmd.Parameters.AddWithValue("@id", raffleID)
                cmd.ExecuteNonQuery()
            End Using
        End Using

        LoadRaffleEntries()
        UpdateTotalRaffleEntries()
    End Sub

    Private Sub btnPrintPlayer_Click(sender As Object, e As EventArgs) Handles btnPrintPlayer.Click

        ' ===== Create Dialog Form =====
        Dim pickForm As New Form With {
        .Text = "Select Player to Print",
        .Size = New Size(400, 400),
        .StartPosition = FormStartPosition.CenterParent,
        .FormBorderStyle = FormBorderStyle.FixedDialog,
        .MaximizeBox = False,
        .MinimizeBox = False
    }

        Dim txtSearch As New TextBox With {
        .Location = New Point(20, 20),
        .Width = 340
    }

        Dim lstPlayers As New ListBox With {
        .Location = New Point(20, 55),
        .Size = New Size(340, 220)
    }

        Dim btnPrint As New Button With {
        .Text = "Print",
        .Location = New Point(80, 300),
        .Width = 100
    }

        Dim btnCancel As New Button With {
        .Text = "Cancel",
        .Location = New Point(200, 300),
        .Width = 100
    }

        pickForm.Controls.AddRange({txtSearch, lstPlayers, btnPrint, btnCancel})

        ' ===== Load players from database =====
        Dim dbPath = GetDatabasePath()
        Dim dtPlayers As New DataTable

        Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
            conn.Open()

            Dim sql = "
        SELECT DISTINCT full_name 
        FROM raffle 
        ORDER BY full_name ASC"

            Using cmd As New SQLiteCommand(sql, conn)
                Using adapter As New SQLiteDataAdapter(cmd)
                    adapter.Fill(dtPlayers)
                End Using
            End Using
        End Using

        ' Fill listbox
        For Each row As DataRow In dtPlayers.Rows
            lstPlayers.Items.Add(row("full_name").ToString())
        Next

        ' ===== Search filter =====
        AddHandler txtSearch.TextChanged,
    Sub()
        lstPlayers.Items.Clear()

        Dim filter = txtSearch.Text.ToLower()

        For Each row As DataRow In dtPlayers.Rows
            Dim name = row("full_name").ToString()

            If name.ToLower().Contains(filter) Then
                lstPlayers.Items.Add(name)
            End If
        Next
    End Sub

        ' ===== Print button =====
        AddHandler btnPrint.Click,
    Sub()
        If lstPlayers.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a player.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedPlayer = lstPlayers.SelectedItem.ToString()

        ' Don't close the popup yet
        Try
            PrintRaffleForMember(selectedPlayer)
            ' Only close if printing succeeds
            pickForm.Close()
        Catch ex As IOException
            MessageBox.Show("Cannot print now. Database is in use. Please try again.", "File In Use", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Catch ex As Exception
            MessageBox.Show("Error printing: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

        ' Cancel
        AddHandler btnCancel.Click,
    Sub()
        pickForm.Close()
    End Sub

        pickForm.ShowDialog()

    End Sub

    Private Sub BtnPrintByDate_Click(sender As Object, e As EventArgs) Handles BtnPrintByDate.Click

        ' ===== Create popup form =====
        Dim dateForm As New Form With {
        .Text = "Print by Date",
        .Size = New Size(300, 250),
        .StartPosition = FormStartPosition.CenterParent,
        .FormBorderStyle = FormBorderStyle.FixedDialog,
        .MaximizeBox = False,
        .MinimizeBox = False
    }

        Dim lblFrom As New Label With {
        .Text = "From Date:",
        .Location = New Point(20, 20)
    }

        Dim dtFrom As New DateTimePicker With {
        .Location = New Point(20, 45),
        .Width = 240,
        .Format = DateTimePickerFormat.Custom,
        .CustomFormat = "yyyy-MM-dd"
    }

        Dim lblTo As New Label With {
        .Text = "To Date:",
        .Location = New Point(20, 80)
    }

        Dim dtTo As New DateTimePicker With {
        .Location = New Point(20, 105),
        .Width = 240,
        .Format = DateTimePickerFormat.Custom,
        .CustomFormat = "yyyy-MM-dd"
    }

        Dim btnPrint As New Button With {
        .Text = "Print",
        .Location = New Point(50, 140),
        .Width = 80
    }

        Dim btnCancel As New Button With {
        .Text = "Cancel",
        .Location = New Point(150, 140),
        .Width = 80
    }

        dateForm.Controls.AddRange({lblFrom, dtFrom, lblTo, dtTo, btnPrint, btnCancel})

        ' ===== Print button =====
        AddHandler btnPrint.Click,
    Sub()

        Dim fromDate As String = dtFrom.Value.ToString("yyyy-MM-dd")
        Dim toDate As String = dtTo.Value.ToString("yyyy-MM-dd")

        dateForm.Close()

        PrintRaffleByDate(fromDate, toDate)

    End Sub

        AddHandler btnCancel.Click,
    Sub()
        dateForm.Close()
    End Sub

        dateForm.ShowDialog()

    End Sub

    Private Sub PrintRaffleByDate(fromDate As String, toDate As String)

        Dim dbPath As String = GetDatabasePath()
        Dim dtTickets As New DataTable()

        Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
            conn.Open()

            Dim sql As String = "
        SELECT id,
               full_name,
               raffle_number,
               is_printed
        FROM raffle
        WHERE DATE(session_raffle_date) BETWEEN DATE(@from) AND DATE(@to)
        ORDER BY CAST(raffle_number AS INTEGER) ASC"

            Using cmd As New SQLiteCommand(sql, conn)
                cmd.Parameters.AddWithValue("@from", fromDate)
                cmd.Parameters.AddWithValue("@to", toDate)

                Using adapter As New SQLiteDataAdapter(cmd)
                    adapter.Fill(dtTickets)
                End Using
            End Using
        End Using
        If dtTickets.Rows.Count = 0 Then
            MessageBox.Show("No raffle entries found.", "No Tickets", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' CONFIRMATION BEFORE PRINT
        If dtTickets.Rows.Count = 0 Then
            MessageBox.Show("No raffle entries found for this session date range.", "No Tickets", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        ' Count already printed tickets
        Dim alreadyPrinted As Integer =
    dtTickets.AsEnumerable().
    Count(Function(r)
              If IsDBNull(r("is_printed")) Then
                  Return False
              End If

              Return Convert.ToInt32(r("is_printed")) = 1
          End Function)

        If alreadyPrinted > 0 Then

            If MessageBox.Show(
        $"{alreadyPrinted} ticket(s) in this date range were already printed." &
        vbCrLf &
        "Do you want to print them again?",
        "Already Printed",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    ) = DialogResult.No Then

                Return

            End If

        End If
        If MessageBox.Show("Print raffle tickets for this date range?",
                   "Print Confirmation",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If


        For Each row As DataRow In dtTickets.Rows

            Dim raffleID As Integer = Convert.ToInt32(row("id"))
            Dim memberName As String = row("full_name").ToString()
            Dim raffleNumber As String = row("raffle_number").ToString()

            Try

                PrintSingleTicket(memberName, raffleNumber)

                MarkTicketPrinted(raffleID)

                Threading.Thread.Sleep(300)

                SendCutCommand()

            Catch ex As Exception

                MessageBox.Show(
                    "Failed to print ticket #" & raffleNumber &
                    vbCrLf &
                    ex.Message,
                    "Print Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error)

            End Try

        Next

        LoadRaffleEntries()
    End Sub
End Class