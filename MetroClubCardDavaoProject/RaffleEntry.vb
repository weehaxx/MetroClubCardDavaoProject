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

                                     Dim ticketWidth As Integer = 200
                                     Dim ticketHeight As Integer = 100

                                     Dim x As Integer = e2.MarginBounds.Left
                                     Dim y As Integer = e2.MarginBounds.Top

                                     Dim fontTitle As New Font("Arial", 12, FontStyle.Bold)
                                     Dim fontText As New Font("Arial", 10)

                                     g.DrawRectangle(Pens.Black, x, y, ticketWidth, ticketHeight)

                                     g.DrawString("RAFFLE TICKET", fontTitle, Brushes.Black, x + 10, y + 5)

                                     Dim baseFontText As New Font("Arial", 10)
                                     Dim nameRect As New RectangleF(x + 10, y + 30, ticketWidth - 20, 35)

                                     Dim nameFont As Font = baseFontText
                                     Dim nameText As String = "Member: " & memberName

                                     Dim sf As New StringFormat() With {
    .Alignment = StringAlignment.Near,
    .LineAlignment = StringAlignment.Near,
    .Trimming = StringTrimming.EllipsisCharacter,
    .FormatFlags = StringFormatFlags.LineLimit
}

                                     ' Auto shrink font if too big
                                     Do While g.MeasureString(nameText, nameFont, nameRect.Size, sf).Height > nameRect.Height AndAlso nameFont.Size > 6
                                         nameFont = New Font(nameFont.FontFamily, nameFont.Size - 0.5F, nameFont.Style)
                                     Loop

                                     g.DrawString(nameText, nameFont, Brushes.Black, nameRect, sf)

                                     ' Raffle number (safe position)
                                     g.DrawString("Raffle #: " & raffleNumber, baseFontText, Brushes.Black, x + 10, y + 70, sf)

                                     e2.HasMorePages = False
                                 End Sub

        Dim printDlg As New PrintDialog() With {.Document = pd}
        If printDlg.ShowDialog() = DialogResult.OK Then
            pd.Print()
        End If

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
                       r.raffle_time AS RaffleTime
                FROM raffle r
                INNER JOIN registrations reg ON r.registration_id = reg.id
                ORDER BY CAST(r.raffle_number AS INTEGER) ASC
            "

                Using cmd As New SQLiteCommand(sql, conn)
                    Using adapter As New SQLiteDataAdapter(cmd)
                        dtRaffleEntries = New DataTable()
                        adapter.Fill(dtRaffleEntries)
                        ' ✅ Force proper time formatting
                        For Each row As DataRow In dtRaffleEntries.Rows
                            If Not IsDBNull(row("RaffleTime")) Then
                                Dim parsedTime As DateTime
                                If DateTime.TryParse(row("RaffleTime").ToString(), parsedTime) Then
                                    row("RaffleTime") = parsedTime.ToString("hh:mm tt")
                                End If
                            End If
                        Next
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
            dgvRaffleEntries.Columns("RaffleTime").Width = 90

            dgvRaffleEntries.Columns("RaffleID").Visible = False

            ' Set column headers and formatting
            With dgvRaffleEntries
                .Columns("RaffleNumber").HeaderText = "Raffle #"
                .Columns("MemberName").HeaderText = "Member Name"
                .Columns("RegistrationID").HeaderText = "Reg ID"
                .Columns("RaffleDate").HeaderText = "Date"
                .Columns("RaffleTime").HeaderText = "Time"

                .Columns("RaffleDate").DefaultCellStyle.Format = "yyyy-MM-dd"
                .AutoResizeColumns()
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells


                dgvRaffleEntries.DataSource = dtRaffleEntries
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
            Dim sql As String = "SELECT raffle_number FROM raffle WHERE full_name=@name ORDER BY raffle_number ASC"
            Using cmd As New SQLiteCommand(sql, conn)
                cmd.Parameters.AddWithValue("@name", memberName)
                Using adapter As New SQLiteDataAdapter(cmd)
                    adapter.Fill(dtTickets)
                End Using
            End Using
        End Using

        If dtTickets.Rows.Count = 0 Then
            MessageBox.Show("No raffle entries found for this member.", "No Tickets", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim ticketIndex As Integer = 0
        Dim pd As New PrintDocument()
        pd.DefaultPageSettings.Landscape = False
        pd.DefaultPageSettings.Margins = New Margins(20, 20, 20, 20)

        ' Ticket size
        Dim ticketWidth As Integer = 200
        Dim ticketHeight As Integer = 100
        Dim padding As Integer = 10

        AddHandler pd.PrintPage, Sub(sender2, e2)
                                     Dim g As Graphics = e2.Graphics
                                     g.Clear(Color.White)

                                     Dim ticketsPerRow As Integer = Math.Floor((e2.MarginBounds.Width + padding) / (ticketWidth + padding))
                                     Dim ticketsPerColumn As Integer = Math.Floor((e2.MarginBounds.Height + padding) / (ticketHeight + padding))
                                     Dim ticketsPerPage As Integer = ticketsPerRow * ticketsPerColumn

                                     Dim fontTitle As New Font("Arial", 12, FontStyle.Bold)
                                     Dim fontText As New Font("Arial", 10)

                                     For i As Integer = 0 To ticketsPerPage - 1
                                         If ticketIndex >= dtTickets.Rows.Count Then Exit For

                                         Dim rowNum As Integer = Math.Floor(i / ticketsPerRow)
                                         Dim colNum As Integer = i Mod ticketsPerRow
                                         Dim x As Integer = e2.MarginBounds.Left + colNum * (ticketWidth + padding)
                                         Dim y As Integer = e2.MarginBounds.Top + rowNum * (ticketHeight + padding)

                                         g.DrawRectangle(Pens.Black, x, y, ticketWidth, ticketHeight)

                                         Dim raffleNumber As String = dtTickets.Rows(ticketIndex)("raffle_number").ToString()

                                         g.DrawString("RAFFLE TICKET", fontTitle, Brushes.Black, x + 10, y + 5)
                                         g.DrawString("Member: " & memberName, fontText, Brushes.Black, New RectangleF(x + 10, y + 30, ticketWidth - 20, 40))
                                         g.DrawString("Raffle #: " & raffleNumber, fontText, Brushes.Black, New RectangleF(x + 10, y + 70, ticketWidth - 20, 20))

                                         ticketIndex += 1
                                     Next

                                     e2.HasMorePages = ticketIndex < dtTickets.Rows.Count
                                 End Sub

        Dim printDlg As New PrintDialog()
        printDlg.Document = pd
        If printDlg.ShowDialog() = DialogResult.OK Then
            pd.Print()
        End If
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
    Private Sub dgvRaffleEntries_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRaffleEntries.CellContentClick

        If e.RowIndex < 0 Then Exit Sub

        Dim raffleID As Integer = Convert.ToInt32(dgvRaffleEntries.Rows(e.RowIndex).Cells("RaffleID").Value)

        ' ================= EDIT =================
        If dgvRaffleEntries.Columns(e.ColumnIndex).Name = "Edit" Then

            Dim currentDate As String = dgvRaffleEntries.Rows(e.RowIndex).Cells("RaffleDate").Value.ToString()
            Dim currentTime As String = dgvRaffleEntries.Rows(e.RowIndex).Cells("RaffleTime").Value.ToString()

            ' ===== Create dialog =====
            Dim editForm As New Form() With {
                .Text = "Edit Raffle Entry",
                .Size = New Size(300, 220),
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

            DateTime.TryParse(currentDate, dtpDate.Value)

            Dim lblTime As New Label() With {.Text = "Raffle Time:", .Location = New Point(20, 80)}
            Dim dtpTime As New DateTimePicker() With {
                .Location = New Point(20, 105),
                .Width = 240,
                .Format = DateTimePickerFormat.Time,
                .ShowUpDown = True
            }

            DateTime.TryParse(currentTime, dtpTime.Value)

            Dim btnSave As New Button() With {
                .Text = "Save",
                .Location = New Point(50, 145),
                .DialogResult = DialogResult.OK
            }

            Dim btnCancel As New Button() With {
                .Text = "Cancel",
                .Location = New Point(150, 145),
                .DialogResult = DialogResult.Cancel
            }

            editForm.Controls.AddRange({lblDate, dtpDate, lblTime, dtpTime, btnSave, btnCancel})
            editForm.AcceptButton = btnSave
            editForm.CancelButton = btnCancel

            If editForm.ShowDialog() = DialogResult.OK Then

                Dim newDate As String = dtpDate.Value.ToString("yyyy-MM-dd")
                Dim newTime As String = dtpTime.Value.ToString("hh:mm tt")
                ' ===== Update database =====
                Try
                    Dim dbPath As String = GetDatabasePath()

                    Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
                        conn.Open()

                        Dim sql As String = "
                        UPDATE raffle 
                        SET raffle_date = @date,
                            raffle_time = @time
                        WHERE id = @id
                    "

                        Using cmd As New SQLiteCommand(sql, conn)
                            cmd.Parameters.AddWithValue("@date", newDate)
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
        End If

        ' ================= DELETE =================
        If dgvRaffleEntries.Columns(e.ColumnIndex).Name = "Delete" Then

            Dim confirm = MessageBox.Show("Delete this raffle entry?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If confirm = DialogResult.No Then Exit Sub

            Dim dbPath As String = GetDatabasePath()

            Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
                conn.Open()

                Dim sql As String = "DELETE FROM raffle WHERE id = @id"
                Using cmd As New SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", raffleID)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            RenumberRaffleNumbers()
            LoadRaffleEntries()
            UpdateTotalRaffleEntries()
        End If

    End Sub

    Private Sub btnprint_Click(sender As Object, e As EventArgs) Handles btnprint.Click
        Dim dbPath As String = GetDatabasePath()

        ' Get all raffle entries ordered by member name then raffle number
        Dim dtTickets As New DataTable()
        Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
            conn.Open()
            Dim sql As String = "
                            SELECT full_name, raffle_number 
                            FROM raffle 
                            ORDER BY CAST(raffle_number AS INTEGER) ASC"
            Using cmd As New SQLiteCommand(sql, conn)
                Using adapter As New SQLiteDataAdapter(cmd)
                    adapter.Fill(dtTickets)
                End Using
            End Using
        End Using

        If dtTickets.Rows.Count = 0 Then
            MessageBox.Show("No raffle entries found.", "No Tickets", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Prepare PrintDocument
        Dim ticketIndex As Integer = 0 ' Persist across pages
        Dim pd As New PrintDocument()
        pd.DefaultPageSettings.Landscape = False
        pd.DefaultPageSettings.Margins = New Margins(20, 20, 20, 20)

        Dim ticketWidth As Integer = 200
        Dim ticketHeight As Integer = 100
        Dim padding As Integer = 10

        AddHandler pd.PrintPage, Sub(sender2, e2)
                                     Dim g As Graphics = e2.Graphics
                                     g.Clear(Color.White)

                                     ' Tickets per row/column
                                     Dim ticketsPerRow As Integer = Math.Floor((e2.MarginBounds.Width + padding) / (ticketWidth + padding))
                                     Dim ticketsPerColumn As Integer = Math.Floor((e2.MarginBounds.Height + padding) / (ticketHeight + padding))
                                     Dim ticketsPerPage As Integer = ticketsPerRow * ticketsPerColumn

                                     Dim fontTitle As New Font("Arial", 12, FontStyle.Bold)
                                     Dim baseFontText As New Font("Arial", 10)

                                     For i As Integer = 0 To ticketsPerPage - 1
                                         If ticketIndex >= dtTickets.Rows.Count Then Exit For

                                         Dim rowNum As Integer = Math.Floor(i / ticketsPerRow)
                                         Dim colNum As Integer = i Mod ticketsPerRow
                                         Dim x As Integer = e2.MarginBounds.Left + colNum * (ticketWidth + padding)
                                         Dim y As Integer = e2.MarginBounds.Top + rowNum * (ticketHeight + padding)

                                         ' Draw ticket rectangle
                                         g.DrawRectangle(Pens.Black, x, y, ticketWidth, ticketHeight)

                                         ' Draw text
                                         Dim raffleNumber As String = dtTickets.Rows(ticketIndex)("raffle_number").ToString()
                                         Dim memberName As String = dtTickets.Rows(ticketIndex)("full_name").ToString()

                                         g.DrawString("RAFFLE TICKET", fontTitle, Brushes.Black, x + 10, y + 5)

                                         ' Member name
                                         Dim nameRect As New RectangleF(x + 10, y + 30, ticketWidth - 20, 40)
                                         Dim nameFont As Font = baseFontText
                                         Dim nameText As String = "Member: " & memberName
                                         Dim sf As New StringFormat() With {
                                         .Alignment = StringAlignment.Near,
                                         .LineAlignment = StringAlignment.Near,
                                         .Trimming = StringTrimming.EllipsisCharacter,
                                         .FormatFlags = StringFormatFlags.LineLimit
                                     }

                                         ' Auto shrink font if needed
                                         Do While g.MeasureString(nameText, nameFont, nameRect.Size, sf).Height > nameRect.Height AndAlso nameFont.Size > 6
                                             nameFont = New Font(nameFont.FontFamily, nameFont.Size - 0.5F, nameFont.Style)
                                         Loop

                                         g.DrawString(nameText, nameFont, Brushes.Black, nameRect, sf)

                                         ' Raffle number
                                         g.DrawString("Raffle #: " & raffleNumber, baseFontText, Brushes.Black, x + 10, y + 70, sf)

                                         ticketIndex += 1
                                     Next

                                     ' More pages?
                                     e2.HasMorePages = ticketIndex < dtTickets.Rows.Count
                                 End Sub

        ' Show Print Dialog
        Dim printDlg As New PrintDialog() With {.Document = pd}
        If printDlg.ShowDialog() = DialogResult.OK Then
            pd.Print()
        End If
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

        ' ===== Create dialog =====
        Dim editForm As New Form() With {
            .Text = "Edit Raffle Entry",
            .Size = New Size(300, 220),
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
        DateTime.TryParse(currentDate, dtpDate.Value)

        Dim lblTime As New Label() With {.Text = "Raffle Time:", .Location = New Point(20, 80)}
        Dim dtpTime As New DateTimePicker() With {
            .Location = New Point(20, 105),
            .Width = 240,
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True
        }
        DateTime.TryParse(currentTime, dtpTime.Value)

        Dim btnSave As New Button() With {.Text = "Save", .Location = New Point(50, 145), .DialogResult = DialogResult.OK}
        Dim btnCancel As New Button() With {.Text = "Cancel", .Location = New Point(150, 145), .DialogResult = DialogResult.Cancel}

        editForm.Controls.AddRange({lblDate, dtpDate, lblTime, dtpTime, btnSave, btnCancel})
        editForm.AcceptButton = btnSave
        editForm.CancelButton = btnCancel

        If editForm.ShowDialog() = DialogResult.OK Then
            Dim newDate As String = dtpDate.Value.ToString("yyyy-MM-dd")
            Dim newTime As String = dtpTime.Value.ToString("HH:mm:ss")

            ' Update DB
            Try
                Dim dbPath As String = GetDatabasePath()
                Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
                    conn.Open()
                    Dim sql As String = "UPDATE raffle SET raffle_date=@date, raffle_time=@time WHERE id=@id"
                    Using cmd As New SQLiteCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@date", newDate)
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

        RenumberRaffleNumbers()
        LoadRaffleEntries()
        UpdateTotalRaffleEntries()
    End Sub

    Private Sub btnPrintPlayer_Click(sender As Object, e As EventArgs) Handles btnPrintPlayer.Click

        ' ===== Create Dialog Form =====
        Dim pickForm As New Form() With {
        .Text = "Select Player to Print",
        .Size = New Size(400, 400),
        .StartPosition = FormStartPosition.CenterParent,
        .FormBorderStyle = FormBorderStyle.FixedDialog,
        .MaximizeBox = False,
        .MinimizeBox = False
    }

        Dim txtSearch As New TextBox() With {
        .PlaceholderText = "Search player...",
        .Location = New Point(20, 20),
        .Width = 340
    }

        Dim lstPlayers As New ListBox() With {
        .Location = New Point(20, 55),
        .Size = New Size(340, 220)
    }

        Dim btnPrint As New Button() With {
        .Text = "Print",
        .Location = New Point(80, 300),
        .Width = 100
    }

        Dim btnCancel As New Button() With {
        .Text = "Cancel",
        .Location = New Point(200, 300),
        .Width = 100,
        .DialogResult = DialogResult.Cancel
    }

        pickForm.Controls.AddRange({txtSearch, lstPlayers, btnPrint, btnCancel})
        pickForm.AcceptButton = btnPrint
        pickForm.CancelButton = btnCancel

        ' ===== Load Unique Players =====
        Dim dbPath As String = GetDatabasePath()
        Dim dtPlayers As New DataTable()

        Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
            conn.Open()
            Dim sql As String = "SELECT DISTINCT full_name FROM raffle ORDER BY full_name ASC"
            Using cmd As New SQLiteCommand(sql, conn)
                Using adapter As New SQLiteDataAdapter(cmd)
                    adapter.Fill(dtPlayers)
                End Using
            End Using
        End Using

        For Each row As DataRow In dtPlayers.Rows
            lstPlayers.Items.Add(row("full_name").ToString())
        Next

        ' ===== Search Filter =====
        AddHandler txtSearch.TextChanged, Sub()
                                              Dim filter = txtSearch.Text.ToLower()
                                              lstPlayers.Items.Clear()

                                              For Each row As DataRow In dtPlayers.Rows
                                                  Dim name = row("full_name").ToString()
                                                  If name.ToLower().Contains(filter) Then
                                                      lstPlayers.Items.Add(name)
                                                  End If
                                              Next
                                          End Sub

        ' ===== Print Button Click =====
        AddHandler btnPrint.Click, Sub()
                                       If lstPlayers.SelectedItem Is Nothing Then
                                           MessageBox.Show("Please select a player first.")
                                           Return
                                       End If

                                       Dim selectedPlayer As String = lstPlayers.SelectedItem.ToString()

                                       pickForm.Close()
                                       PrintRaffleForMember(selectedPlayer)
                                   End Sub

        pickForm.ShowDialog()

    End Sub
End Class