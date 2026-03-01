Imports System.Data.SQLite
Imports System.IO
Imports System.Windows.Forms

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
                    SELECT r.raffle_number AS RaffleNumber,
                           r.full_name AS MemberName,
                           reg.registration_id AS RegistrationID,
                           r.raffle_date AS RaffleDate
                    FROM raffle r
                    INNER JOIN registrations reg ON r.registration_id = reg.id
                    ORDER BY r.raffle_date DESC, CAST(r.raffle_number AS INTEGER) ASC
                "

                Using cmd As New SQLiteCommand(sql, conn)
                    Using adapter As New SQLiteDataAdapter(cmd)
                        Dim dt As New DataTable()
                        adapter.Fill(dt)
                        dgvRaffleEntries.DataSource = dt
                    End Using
                End Using
            End Using

            dgvRaffleEntries.Columns("RaffleNumber").HeaderText = "Raffle #"
            dgvRaffleEntries.Columns("MemberName").HeaderText = "Member Name"
            dgvRaffleEntries.Columns("RegistrationID").HeaderText = "Reg ID"
            dgvRaffleEntries.Columns("RaffleDate").HeaderText = "Date"

            dgvRaffleEntries.Columns("RaffleDate").DefaultCellStyle.Format = "yyyy-MM-dd" ' Only date

            dgvRaffleEntries.AutoResizeColumns()
            dgvRaffleEntries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

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

    Private Sub dgvRaffleEntries_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRaffleEntries.CellContentClick
        ' Optional: handle clicks on cells
    End Sub

End Class