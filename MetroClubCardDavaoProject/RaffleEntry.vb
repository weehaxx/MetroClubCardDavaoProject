Imports System.Data.SQLite
Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing.Printing

Public Class RaffleEntry

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
                        Dim dt As New DataTable()
                        adapter.Fill(dt)
                        dgvRaffleEntries.DataSource = dt
                    End Using
                    Using adapter As New SQLiteDataAdapter(cmd)
                        Dim dt As New DataTable()
                        adapter.Fill(dt)
                        dgvRaffleEntries.DataSource = dt

                        ' <- Add this line here to remove the extra blank row
                        dgvRaffleEntries.AllowUserToAddRows = False
                    End Using
                End Using
            End Using
            dgvRaffleEntries.Columns("RaffleID").Visible = False
            ' Set column headers and format
            With dgvRaffleEntries
                .Columns("RaffleNumber").HeaderText = "Raffle #"
                .Columns("MemberName").HeaderText = "Member Name"
                .Columns("RegistrationID").HeaderText = "Reg ID"
                .Columns("RaffleDate").HeaderText = "Date"
                .Columns("RaffleTime").HeaderText = "Time"

                .Columns("RaffleDate").DefaultCellStyle.Format = "yyyy-MM-dd"
                .Columns("RaffleTime").DefaultCellStyle.Format = "hh:mm tt"
                .AutoResizeColumns()
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

                ' Remove old Edit/Delete columns if they exist
                If .Columns.Contains("Edit") Then .Columns.Remove("Edit")
                If .Columns.Contains("Delete") Then .Columns.Remove("Delete")

                ' Add Edit button
                ' Add Edit button once
                If dgvRaffleEntries.Columns("Edit") Is Nothing Then
                    Dim editBtn As New DataGridViewButtonColumn()
                    editBtn.Name = "Edit"
                    editBtn.HeaderText = "Edit"
                    editBtn.Text = "Edit"
                    editBtn.UseColumnTextForButtonValue = True
                    editBtn.Width = 60
                    editBtn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                    dgvRaffleEntries.Columns.Add(editBtn)
                End If

                ' Add Delete button once
                If dgvRaffleEntries.Columns("Delete") Is Nothing Then
                    Dim delBtn As New DataGridViewButtonColumn()
                    delBtn.Name = "Delete"
                    delBtn.HeaderText = "Delete"
                    delBtn.Text = "Delete"
                    delBtn.UseColumnTextForButtonValue = True
                    delBtn.Width = 60
                    delBtn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                    dgvRaffleEntries.Columns.Add(delBtn)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show("Error loading raffle entries: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
        ' ===== Get members with raffle entries =====
        Dim dbPath As String = GetDatabasePath()
        Dim dtMembers As New DataTable()
        Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
            conn.Open()
            Dim sql As String = "SELECT DISTINCT full_name FROM raffle ORDER BY full_name ASC"
            Using cmd As New SQLiteCommand(sql, conn)
                Using adapter As New SQLiteDataAdapter(cmd)
                    adapter.Fill(dtMembers)
                End Using
            End Using
        End Using

        If dtMembers.Rows.Count = 0 Then
            MessageBox.Show("No members found in raffle entries.", "No Members", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' ===== Let user select a member =====
        Dim memberList As New List(Of String)
        For Each row As DataRow In dtMembers.Rows
            memberList.Add(row("full_name").ToString())
        Next

        Dim selectForm As New Form() With {
        .Text = "Select Member to Print",
        .Size = New Size(300, 150),
        .StartPosition = FormStartPosition.CenterParent,
        .FormBorderStyle = FormBorderStyle.FixedDialog,
        .MaximizeBox = False,
        .MinimizeBox = False
    }

        Dim combo As New ComboBox() With {
        .Location = New Point(20, 20),
        .Width = 240,
        .DropDownStyle = ComboBoxStyle.DropDownList
    }
        combo.Items.AddRange(memberList.ToArray())
        combo.SelectedIndex = 0

        Dim btnOK As New Button() With {
        .Text = "OK",
        .DialogResult = DialogResult.OK,
        .Location = New Point(50, 60)
    }
        Dim btnCancel As New Button() With {
        .Text = "Cancel",
        .DialogResult = DialogResult.Cancel,
        .Location = New Point(150, 60)
    }

        selectForm.Controls.Add(combo)
        selectForm.Controls.Add(btnOK)
        selectForm.Controls.Add(btnCancel)
        selectForm.AcceptButton = btnOK
        selectForm.CancelButton = btnCancel

        If selectForm.ShowDialog() <> DialogResult.OK Then Return
        Dim selectedMember As String = combo.SelectedItem.ToString()

        ' ===== Get raffle entries for that member =====
        Dim dtTickets As New DataTable()
        Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
            conn.Open()
            Dim sql As String = "SELECT raffle_number FROM raffle WHERE full_name = @name ORDER BY raffle_number ASC"
            Using cmd As New SQLiteCommand(sql, conn)
                cmd.Parameters.AddWithValue("@name", selectedMember)
                Using adapter As New SQLiteDataAdapter(cmd)
                    adapter.Fill(dtTickets)
                End Using
            End Using
        End Using

        If dtTickets.Rows.Count = 0 Then
            MessageBox.Show("No raffle entries found for this member.", "No Tickets", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' ===== Prepare PrintDocument =====
        Dim ticketIndex As Integer = 0
        Dim pd As New PrintDocument()
        pd.DefaultPageSettings.Landscape = False
        pd.DefaultPageSettings.Margins = New Margins(20, 20, 20, 20)

        ' Ticket size and padding
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

                                         ' Calculate position
                                         Dim rowNum As Integer = Math.Floor(i / ticketsPerRow)
                                         Dim colNum As Integer = i Mod ticketsPerRow
                                         Dim x As Integer = e2.MarginBounds.Left + colNum * (ticketWidth + padding)
                                         Dim y As Integer = e2.MarginBounds.Top + rowNum * (ticketHeight + padding)

                                         ' Draw ticket rectangle
                                         g.DrawRectangle(Pens.Black, x, y, ticketWidth, ticketHeight)

                                         ' Draw ticket text
                                         Dim raffleNumber As String = dtTickets.Rows(ticketIndex)("raffle_number").ToString()

                                         ' Title
                                         g.DrawString("RAFFLE TICKET", fontTitle, Brushes.Black, x + 10, y + 5)

                                         ' ===== Member name with word wrap and dynamic font =====
                                         Dim nameRect As New RectangleF(x + 10, y + 30, ticketWidth - 20, 40)
                                         Dim nameFont As Font = baseFontText
                                         Dim nameText As String = "Member: " & selectedMember

                                         ' StringFormat for wrapping and trimming
                                         Dim sf As New StringFormat()
                                         sf.Alignment = StringAlignment.Near
                                         sf.LineAlignment = StringAlignment.Near
                                         sf.Trimming = StringTrimming.EllipsisCharacter
                                         sf.FormatFlags = StringFormatFlags.LineLimit

                                         ' Reduce font size until it fits height
                                         Do While g.MeasureString(nameText, nameFont, nameRect.Size, sf).Height > nameRect.Height AndAlso nameFont.Size > 6
                                             nameFont = New Font(nameFont.FontFamily, nameFont.Size - 0.5F, nameFont.Style)
                                         Loop

                                         g.DrawString(nameText, nameFont, Brushes.Black, nameRect, sf)

                                         ' Raffle number
                                         Dim raffleRect As New RectangleF(x + 10, y + 70, ticketWidth - 20, 20)
                                         g.DrawString("Raffle #: " & raffleNumber, baseFontText, Brushes.Black, raffleRect, sf)

                                         ticketIndex += 1
                                     Next

                                     ' More pages?
                                     e2.HasMorePages = ticketIndex < dtTickets.Rows.Count
                                 End Sub

        ' ===== Show Print Dialog =====
        Dim printDlg As New PrintDialog()
        printDlg.Document = pd
        If printDlg.ShowDialog() = DialogResult.OK Then
            pd.Print()
        End If
    End Sub
End Class