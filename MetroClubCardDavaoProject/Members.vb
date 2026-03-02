Imports System.Data.SQLite
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.IO
Imports Guna.UI2.WinForms
Imports Zen.Barcode
Imports MessagingToolkit.Barcode



Public Class Members
    Dim conn As SQLiteConnection
    Dim dt As DataTable ' Keep DataTable global for filtering
    Private Function GetDatabasePath() As String
        Dim appDataPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MetroCardClubDavao")
        If Not Directory.Exists(appDataPath) Then
            Directory.CreateDirectory(appDataPath)
        End If
        Return Path.Combine(appDataPath, "metrocarddavaodb.db")
    End Function
    Private Sub ClearMemberDetails()
        ' Clear all textboxes
        tbLastName.Clear()
        tbFIrstName.Clear()
        tbMiddleName.Clear()
        tbAlternativeName.Clear()
        tbPresentAddress.Clear()
        tbPermanentAddress.Clear()
        tbBirthday.Clear()
        tbBirthPlace.Clear()
        tbCivilStatus.Clear()
        tbNationality.Clear()
        tbEmail.Clear()
        tbMobileNum.Clear()
        tbBusinessName.Clear()
        tbBusinessNature.Clear()
        tbEmployerName.Clear()
        tbWorkNature.Clear()
        tbRelationshipPol.Clear()
        tbNameEmergency.Clear()
        tbRelationShipEmergency.Clear()
        tbContactEmergency.Clear()

        ' Clear radio buttons
        rbSelfEmployed.Checked = False
        rbEmployed.Checked = False
        rbYes.Checked = False
        rbNo.Checked = False

        ' Clear photo
        Guna2PictureBox1.Image = Nothing

        ' Clear selection in DataGridView
        dgvRegistrations.ClearSelection()
        dgvRegistrations.CurrentCell = Nothing
    End Sub

    Private Sub Members_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvRegistrations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Dim dbPath As String = GetDatabasePath()
        conn = New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")


        ' Disable changing of radio buttons (view-only mode)
        rbSelfEmployed.AutoCheck = False
        rbEmployed.AutoCheck = False
        rbYes.AutoCheck = False
        rbNo.AutoCheck = False

        LoadRegistrations()
        UpdateTotalMembers()
        tbSearch.Focus()


    End Sub

    Private Sub LoadRegistrations()
        dgvRegistrations.ClearSelection()
        dgvRegistrations.CurrentCell = Nothing

        Try
            conn.Open()

            ' ✅ Include registration_id in the SELECT
            Dim sql As String =
        "SELECT id, registration_id, lastname, firstname, middlename, alternativename, presentaddress, permanentaddress, birthday, birthplace, civilstatus, nationality, email, mobilenumber, employmentstatus, businessname, businessnature, employername, workname, polmember, relationshippol, nameemergency, relationshipemergency, contactemergency, photo " &
        "FROM registrations"

            Dim da As New SQLiteDataAdapter(sql, conn)
            dt = New DataTable()
            da.Fill(dt)

            ' ✅ Add computed column for fullname only if missing
            If Not dt.Columns.Contains("fullname") Then
                dt.Columns.Add("fullname", GetType(String))
            End If

            ' ✅ Fill computed fullname based on existing columns
            For Each row As DataRow In dt.Rows
                Dim lname As String = If(IsDBNull(row("lastname")), "", row("lastname").ToString())
                Dim fname As String = If(IsDBNull(row("firstname")), "", row("firstname").ToString())
                Dim mname As String = If(IsDBNull(row("middlename")), "", row("middlename").ToString())
                row("fullname") = $"{lname}, {fname} {mname}".ToUpper().Trim()
            Next

            ' ✅ Bind DataTable to DataGridView
            dgvRegistrations.DataSource = dt

            ' ✅ Hide all columns first
            For Each col As DataGridViewColumn In dgvRegistrations.Columns
                col.Visible = False
            Next

            ' ✅ Show only registration_id and fullname
            dgvRegistrations.Columns("registration_id").Visible = True
            dgvRegistrations.Columns("fullname").Visible = True

            ' ✅ Set column headers
            dgvRegistrations.Columns("registration_id").HeaderText = "ID"
            dgvRegistrations.Columns("fullname").HeaderText = "FULL NAME"

            dgvRegistrations.Refresh()
            dgvRegistrations.AutoResizeColumns()
            dgvRegistrations.AutoResizeColumnHeadersHeight()

            ' ✅ Clear selection settings
            dgvRegistrations.ClearSelection()
            dgvRegistrations.CurrentCell = Nothing
            dgvRegistrations.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvRegistrations.MultiSelect = False

            conn.Close()
        Catch ex As Exception
            MessageBox.Show("Error loading registrations: " & ex.Message)
        End Try
    End Sub




    ' Live Search by firstname/lastname/middlename only
    Private Sub tbSearch_TextChanged(sender As Object, e As EventArgs) Handles tbSearch.TextChanged
        tbSearch.Select()

        Try
            If dt IsNot Nothing Then
                Dim dv As New DataView(dt)
                Dim keyword As String = tbSearch.Text.Replace("'", "''")

                dv.RowFilter =
                $"Convert(registration_id, 'System.String') LIKE '%{keyword}%' OR " &
                $"lastname LIKE '%{keyword}%' OR " &
                $"firstname LIKE '%{keyword}%' OR " &
                $"middlename LIKE '%{keyword}%'"

                dgvRegistrations.DataSource = dv
            End If
        Catch ex As Exception
            MessageBox.Show("Error searching: " & ex.Message)
        End Try
    End Sub


    Private Sub dgvRegistrations_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRegistrations.CellClick
        Try
            ' Prevent header/empty-row crash
            If e.RowIndex < 0 OrElse e.RowIndex >= dgvRegistrations.Rows.Count Then Exit Sub

            Dim selectedRowView = TryCast(dgvRegistrations.Rows(e.RowIndex).DataBoundItem, DataRowView)
            If selectedRowView Is Nothing Then Exit Sub

            Dim selectedRow = selectedRowView.Row

            ' Fill detail fields (these columns are present in the DataTable even if hidden in grid)
            tbLastName.Text = If(IsDBNull(selectedRow("lastname")), "", selectedRow("lastname").ToString())
            tbFIrstName.Text = If(IsDBNull(selectedRow("firstname")), "", selectedRow("firstname").ToString())
            tbMiddleName.Text = If(IsDBNull(selectedRow("middlename")), "", selectedRow("middlename").ToString())
            tbAlternativeName.Text = If(IsDBNull(selectedRow("alternativename")), "", selectedRow("alternativename").ToString())
            tbPresentAddress.Text = If(IsDBNull(selectedRow("presentaddress")), "", selectedRow("presentaddress").ToString())
            tbPermanentAddress.Text = If(IsDBNull(selectedRow("permanentaddress")), "", selectedRow("permanentaddress").ToString())
            tbBirthday.Text = If(IsDBNull(selectedRow("birthday")), "", selectedRow("birthday").ToString())
            tbBirthPlace.Text = If(IsDBNull(selectedRow("birthplace")), "", selectedRow("birthplace").ToString())
            tbCivilStatus.Text = If(IsDBNull(selectedRow("civilstatus")), "", selectedRow("civilstatus").ToString())
            tbNationality.Text = If(IsDBNull(selectedRow("nationality")), "", selectedRow("nationality").ToString())
            tbEmail.Text = If(IsDBNull(selectedRow("email")), "", selectedRow("email").ToString())
            tbMobileNum.Text = If(IsDBNull(selectedRow("mobilenumber")), "", selectedRow("mobilenumber").ToString())

            ' Employment Status Handling
            Dim empStatus As String = If(IsDBNull(selectedRow("employmentstatus")), "", selectedRow("employmentstatus").ToString())

            If empStatus = "Self-Employed" Then
                rbSelfEmployed.Checked = True
                rbEmployed.Checked = False

                tbBusinessName.Text = If(IsDBNull(selectedRow("businessname")), "", selectedRow("businessname").ToString())
                tbBusinessNature.Text = If(IsDBNull(selectedRow("businessnature")), "", selectedRow("businessnature").ToString())

                tbEmployerName.Clear()
                tbWorkNature.Clear()
            ElseIf empStatus = "Employed" Then
                rbEmployed.Checked = True
                rbSelfEmployed.Checked = False

                tbEmployerName.Text = If(IsDBNull(selectedRow("employername")), "", selectedRow("employername").ToString())
                tbWorkNature.Text = If(IsDBNull(selectedRow("workname")), "", selectedRow("workname").ToString())

                tbBusinessName.Clear()
                tbBusinessNature.Clear()
            Else
                rbSelfEmployed.Checked = False
                rbEmployed.Checked = False
                tbBusinessName.Clear()
                tbBusinessNature.Clear()
                tbEmployerName.Clear()
                tbWorkNature.Clear()
            End If

            ' Load Photo if exists
            If Not IsDBNull(selectedRow("photo")) Then
                Dim photoData = DirectCast(selectedRow("photo"), Byte())
                Using ms As New MemoryStream(photoData)
                    Guna2PictureBox1.Image = Image.FromStream(ms)
                End Using
            Else
                Guna2PictureBox1.Image = Nothing
            End If

            ' POLITICAL MEMBER HANDLING
            Dim polMember As String = If(IsDBNull(selectedRow("polmember")), "", selectedRow("polmember").ToString())
            Dim relPol As String = If(IsDBNull(selectedRow("relationshippol")), "", selectedRow("relationshippol").ToString())

            If polMember = "Yes" Then
                rbYes.Checked = True
                rbNo.Checked = False
                tbRelationshipPol.Text = relPol
            Else
                rbNo.Checked = True
                rbYes.Checked = False
                tbRelationshipPol.Clear()
            End If

            ' EMERGENCY CONTACT INFO
            tbNameEmergency.Text = If(IsDBNull(selectedRow("nameemergency")), "", selectedRow("nameemergency").ToString())
            tbRelationShipEmergency.Text = If(IsDBNull(selectedRow("relationshipemergency")), "", selectedRow("relationshipemergency").ToString())
            tbContactEmergency.Text = If(IsDBNull(selectedRow("contactemergency")), "", selectedRow("contactemergency").ToString())
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        End Try
    End Sub

    ' Single Players Ledger (Buy-In / Cash-Out) button
    Private Sub gbCashout_Click(sender As Object, e As EventArgs) Handles gbPlayersLedger.Click
        Try
            ' ✅ Check if user has selected a player
            If dgvRegistrations.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a player first before opening the ledger.", "No Player Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim selectedRowView = TryCast(dgvRegistrations.SelectedRows(0).DataBoundItem, DataRowView)
            If selectedRowView Is Nothing Then
                MessageBox.Show("Invalid row selected.")
                Exit Sub
            End If

            Dim selectedRow = selectedRowView.Row
            Dim regID = selectedRow("id") ' ⚠ Use actual DB id
            Dim fullName = $"{selectedRow("lastname")} {selectedRow("firstname")} {selectedRow("middlename")}"

            ' ✅ Show overlay with blur
            Dim overlay As New OverlayForm(Me.FindForm)
            overlay.Show()
            overlay.Refresh() ' Ensure it draws immediately

            ' Open PlayerLedger as modal
            Dim dialog As New PlayerLedger
            dialog.RegistrationID = Convert.ToInt64(regID)
            dialog.FullName = fullName

            If dialog.ShowDialog() = DialogResult.OK Then
                MessageBox.Show("Buy-In/Cash-Out recorded.")
            End If

            ' ✅ Close overlay after dialog closes
            overlay.Close()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub



    Public Class OverlayForm
        Inherits Form

        Public Sub New(owner As Form)
            Me.FormBorderStyle = FormBorderStyle.None
            Me.BackColor = Color.Black
            Me.Opacity = 0.5 ' Dim effect (50%)
            Me.ShowInTaskbar = False
            Me.StartPosition = FormStartPosition.Manual
            Me.Bounds = owner.Bounds
            Me.TopMost = False ' ✅ Make sure dialog can appear on top
        End Sub
    End Class

    Private Sub btnViewAccount_Click(sender As Object, e As EventArgs) Handles btnViewAccount.Click
        Try
            ' ✅ Make sure a player is selected
            If dgvRegistrations.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a player first before viewing the ledger.", "No Player Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim selectedRowView = TryCast(dgvRegistrations.SelectedRows(0).DataBoundItem, DataRowView)
            If selectedRowView Is Nothing Then
                MessageBox.Show("Invalid row selected.")
                Exit Sub
            End If

            Dim selectedRow = selectedRowView.Row
            Dim regID As Long = Convert.ToInt64(selectedRow("id"))
            Dim regCode As String = selectedRow("registration_id").ToString()
            Dim fullName As String = $"{selectedRow("firstname")} {selectedRow("middlename")} {selectedRow("lastname")}"

            ' ✅ Show overlay
            Dim overlay As New OverlayForm(Me.FindForm)
            overlay.Show()
            overlay.Refresh()

            ' ✅ Create popup window (size 1107x738)
            Dim popup As New Form With {
                .FormBorderStyle = FormBorderStyle.None,
                .StartPosition = FormStartPosition.CenterParent,
                .Size = New Size(1107, 738),
                .BackColor = Color.White
            }

            ' ✅ Add Ledger UserControl
            Dim ledgerControl As New Ledger()
            ledgerControl.Dock = DockStyle.Fill

            ' ✅ Pass player data to Ledger (you need Public Properties in Ledger for these)
            ledgerControl.SelectedRegistrationID = regID
            ledgerControl.SelectedRegistrationCode = regCode
            ledgerControl.SelectedFullName = fullName

            popup.Controls.Add(ledgerControl)

            ' ✅ Show popup
            popup.ShowDialog()
            overlay.Close()

        Catch ex As Exception
            MessageBox.Show("Error opening Ledger: " & ex.Message)
        End Try
    End Sub

    Private Sub btnViewID_Click(sender As Object, e As EventArgs) Handles btnViewID.Click
        Try
            ' ✅ Ensure a member is selected
            If dgvRegistrations.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a member first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim selectedRowView = TryCast(dgvRegistrations.SelectedRows(0).DataBoundItem, DataRowView)
            If selectedRowView Is Nothing Then Exit Sub

            Dim selectedRow = selectedRowView.Row
            Dim memberID As Integer = selectedRow("id")
            Dim memberName As String = $"{selectedRow("lastname")} {selectedRow("firstname")} {selectedRow("middlename")}"
            Dim registrationID As String = selectedRow("registration_id").ToString()

            ' ✅ Get idimage from DB
            Dim idImage As Image = Nothing
            Dim dbPath As String = GetDatabasePath()
            Using conn As New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
                conn.Open()

                Dim query As String = "SELECT idimage FROM registrations WHERE id=@id"
                Using cmd As New SQLiteCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", memberID)
                    Dim result = cmd.ExecuteScalar()

                    If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                        Dim photoData As Byte() = DirectCast(result, Byte())
                        Using ms As New MemoryStream(photoData)
                            idImage = Image.FromStream(ms)
                        End Using
                    End If
                End Using
            End Using

            ' ✅ Show overlay
            Dim overlay As New OverlayForm(Me.FindForm)
            overlay.Show()
            overlay.Refresh()

            ' ✅ Popup Form (1001 x 564)
            Dim popup As New Form With {
                .FormBorderStyle = FormBorderStyle.None,
                .StartPosition = FormStartPosition.CenterScreen,
                .Size = New Size(1001, 564),
                .BackColor = Color.White,
                .TopMost = True
            }

            ' ✅ Load ViewID user control
            Dim viewIDControl As New ViewID()
            viewIDControl.Dock = DockStyle.Fill
            viewIDControl.IDImage = idImage

            popup.Controls.Add(viewIDControl)
            popup.ShowDialog()

            overlay.Close()

        Catch ex As Exception
            MessageBox.Show("Error displaying ID: " & ex.Message)
        End Try
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Try
            If dgvRegistrations.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a member first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' ✅ Overlay
            Dim overlay As New OverlayForm(FindForm)
            overlay.Show
            overlay.Refresh

            ' ✅ Authentication popup (145 x 47)
            Dim authForm As New Form With {
            .FormBorderStyle = FormBorderStyle.None,
            .StartPosition = FormStartPosition.CenterParent,
            .Size = New Size(527, 309),
            .BackColor = Color.White
        }

            Dim authControl As New Authentication
            authControl.Dock = DockStyle.Fill

            ' ✅ Handle events
            AddHandler authControl.AuthSuccess,
            Sub()
                authForm.DialogResult = DialogResult.OK
                authForm.Close
            End Sub

            AddHandler authControl.AuthCancelled,
            Sub()
                authForm.DialogResult = DialogResult.Cancel
                authForm.Close
            End Sub

            authForm.Controls.Add(authControl)

            ' ✅ Show authentication dialog
            If authForm.ShowDialog = DialogResult.OK Then
                ' 🔹 Only continue if authentication passed
                Dim selectedRowView = TryCast(dgvRegistrations.SelectedRows(0).DataBoundItem, DataRowView)
                If selectedRowView Is Nothing Then Exit Sub

                Dim selectedRow = selectedRowView.Row
                Dim memberID As Integer = selectedRow("id")
                Dim fullName = $"{selectedRow("lastname")} {selectedRow("firstname")} {selectedRow("middlename")}"

                ' ✅ Now show EditInfo
                Dim popup As New Form With {
                .FormBorderStyle = FormBorderStyle.None,
                .StartPosition = FormStartPosition.CenterParent,
                .Size = New Size(1351, 849),
                .BackColor = Color.White
            }

                Dim editControl As New editInfo
                editControl.Dock = DockStyle.Fill
                editControl.SelectedMemberID = memberID
                editControl.SelectedFullName = fullName

                popup.Controls.Add(editControl)
                popup.ShowDialog

                ' ✅ Refresh grid
                LoadRegistrations
            End If

            overlay.Close

        Catch ex As Exception
            MessageBox.Show("Error opening edit form: " & ex.Message)
        End Try
    End Sub


    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            ' ✅ Ensure a member is selected
            If dgvRegistrations.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a member to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' ✅ Show overlay
            Dim overlay As New OverlayForm(Me.FindForm)
            overlay.Show()
            overlay.Refresh()

            ' ✅ Authentication popup
            Dim authForm As New Form With {
            .FormBorderStyle = FormBorderStyle.None,
            .StartPosition = FormStartPosition.CenterParent,
            .Size = New Size(527, 309),
            .BackColor = Color.White
        }

            Dim authControl As New Authentication()
            authControl.Dock = DockStyle.Fill

            AddHandler authControl.AuthSuccess,
        Sub()
            authForm.DialogResult = DialogResult.OK
            authForm.Close()
        End Sub

            AddHandler authControl.AuthCancelled,
        Sub()
            authForm.DialogResult = DialogResult.Cancel
            authForm.Close()
        End Sub

            authForm.Controls.Add(authControl)

            ' ✅ Show authentication dialog
            If authForm.ShowDialog() <> DialogResult.OK Then
                overlay.Close()
                MessageBox.Show("Deletion cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' ✅ Confirm deletion
            If MessageBox.Show("Are you sure you want to permanently delete this member and all their cashflows?",
                           "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                overlay.Close()
                Exit Sub
            End If

            ' ✅ Get selected member ID
            Dim selectedRowView = TryCast(dgvRegistrations.SelectedRows(0).DataBoundItem, DataRowView)
            If selectedRowView Is Nothing Then
                overlay.Close()
                Exit Sub
            End If

            Dim memberID As Integer = selectedRowView.Row("id")

            ' ✅ Delete from database (cashflows first, then member)
            Dim dbPath As String = GetDatabasePath()
            Using conn As New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
                conn.Open()

                ' 🔸 Enable foreign key support (good practice)
                Using pragmaCmd As New SQLiteCommand("PRAGMA foreign_keys = ON;", conn)
                    pragmaCmd.ExecuteNonQuery()
                End Using

                ' 🗑 1. Delete related cashflows first
                Using deleteCashflowsCmd As New SQLiteCommand("DELETE FROM cashflows WHERE registration_id = @id", conn)
                    deleteCashflowsCmd.Parameters.AddWithValue("@id", memberID)
                    deleteCashflowsCmd.ExecuteNonQuery()
                End Using

                ' 🗑 2. Then delete the member
                Using deleteMemberCmd As New SQLiteCommand("DELETE FROM registrations WHERE id = @id", conn)
                    deleteMemberCmd.Parameters.AddWithValue("@id", memberID)
                    deleteMemberCmd.ExecuteNonQuery()
                End Using
            End Using

            ' ✅ Refresh list after deletion
            LoadRegistrations()

            ' ✅ Clear all fields and photo
            ClearMemberDetails()

            MessageBox.Show("Member and all related cashflow records have been deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)

            overlay.Close()

        Catch ex As Exception
            MessageBox.Show("Error deleting member: " & ex.Message)
        End Try
    End Sub


    Private Sub btnPrintMember_Click(sender As Object, e As EventArgs) Handles btnPrintMember.Click
        Try
            ' ✅ Check if a row is selected
            If dgvRegistrations.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a member first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' ✅ Get selected data
            Dim selectedRowView = TryCast(dgvRegistrations.SelectedRows(0).DataBoundItem, DataRowView)
            If selectedRowView Is Nothing Then Exit Sub

            Dim selectedRow = selectedRowView.Row

            ' ✅ Format and convert name to ALL CAPS: LASTNAME, FIRSTNAME MIDDLENAME
            Dim memberName As String = $"{selectedRow("lastname")}".ToUpper()

            Dim registrationID As String = selectedRow("registration_id").ToString()

            ' ✅ Load photo from DB (or from grid if already loaded)
            Dim memberPhoto As Image = Nothing
            If Not IsDBNull(selectedRow("photo")) Then
                Dim photoData = DirectCast(selectedRow("photo"), Byte())
                Using ms As New MemoryStream(photoData)
                    memberPhoto = Image.FromStream(ms)
                End Using
            End If

            ' ✅ Show overlay
            Dim overlay As New OverlayForm(Me.FindForm)
            overlay.Show()
            overlay.Refresh()

            ' ✅ Create the UserControl first (so we can read its size)
            Dim idPrintingControl As New IDPrinting() With {
            .MemberName = memberName,
            .MemberID = registrationID,
            .MemberPhoto = memberPhoto
        }

            ' ✅ Force layout to ensure proper size is calculated
            idPrintingControl.PerformLayout()
            idPrintingControl.Refresh()

            ' ✅ Create popup form using the UserControl's preferred size
            Dim frm As New Form With {
            .Text = "ID Printing Preview",
            .StartPosition = FormStartPosition.CenterScreen,
            .FormBorderStyle = FormBorderStyle.FixedDialog,
            .MaximizeBox = False,
            .MinimizeBox = False,
            .ShowInTaskbar = False,
            .ClientSize = idPrintingControl.Size ' 👈 Match size of IDPrinting UserControl
        }

            ' ✅ Dock it and add control to form
            idPrintingControl.Dock = DockStyle.Fill
            frm.Controls.Add(idPrintingControl)

            ' ✅ Show modal dialog
            frm.ShowDialog()

            ' ✅ Close overlay
            overlay.Close()
            overlay.Dispose()

        Catch ex As Exception
            MessageBox.Show("Error opening ID Printing: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateTotalMembers()
        Try
            Dim dbPath As String = GetDatabasePath()
            Using conn As New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
                conn.Open()

                Dim cmd As New SQLiteCommand("SELECT COUNT(*) FROM registrations", conn)
                Dim total As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                lblTotalMembers.Text = total.ToString()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error counting members: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub lblTotalMembers_Click(sender As Object, e As EventArgs) Handles lblTotalMembers.Click
        UpdateTotalMembers()
    End Sub

    Private Sub btnAddRaffleEntry_Click(sender As Object, e As EventArgs) Handles btnAddRaffleEntry.Click
        Try
            ' Ensure a member is selected
            If dgvRegistrations.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a member first.", "No Member Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim selectedRowView = TryCast(dgvRegistrations.SelectedRows(0).DataBoundItem, DataRowView)
            If selectedRowView Is Nothing Then Exit Sub

            Dim memberID As Long = Convert.ToInt64(selectedRowView.Row("id"))
            Dim fullName As String = $"{selectedRowView.Row("lastname")} {selectedRowView.Row("firstname")} {selectedRowView.Row("middlename")}".Trim()

            ' Confirmation dialog
            Dim confirmResult = MessageBox.Show(
            $"Are you sure you want to add a raffle entry for {fullName}?",
            "Confirm Raffle Entry",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        )

            If confirmResult = DialogResult.No Then Exit Sub

            ' Determine next sequential raffle number for this member
            Dim dbPath As String = GetDatabasePath()
            Dim nextRaffleNumber As Integer = 1

            Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
                conn.Open()

                Dim sqlMax As String = "SELECT MAX(CAST(raffle_number AS INTEGER)) FROM raffle"
                Using cmdMax As New SQLiteCommand(sqlMax, conn)
                    Dim result = cmdMax.ExecuteScalar()
                    If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                        nextRaffleNumber = Convert.ToInt32(result) + 1
                    End If
                End Using
            End Using

            ' --- Inline dialog to edit date & time ---
            Dim promptForm As New Form() With {
            .Text = "Edit Raffle Date & Time",
            .Size = New Size(300, 180),
            .StartPosition = FormStartPosition.CenterParent,
            .FormBorderStyle = FormBorderStyle.FixedDialog,
            .MaximizeBox = False,
            .MinimizeBox = False
        }

            ' Name label
            Dim lblName As New Label() With {
            .Text = $"Member: {fullName}",
            .AutoSize = True,
            .Location = New Point(10, 10)
        }

            ' Date picker
            Dim dtpDate As New DateTimePicker() With {
            .Format = DateTimePickerFormat.Short,
            .Value = DateTime.Now.Date,
            .Location = New Point(10, 40)
        }

            ' Time picker
            Dim dtpTime As New DateTimePicker() With {
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True,
            .Value = DateTime.Now,
            .Location = New Point(10, 70)
        }

            ' OK button
            Dim btnOK As New Button() With {
            .Text = "OK",
            .DialogResult = DialogResult.OK,
            .Location = New Point(50, 110)
        }

            ' Cancel button
            Dim btnCancel As New Button() With {
            .Text = "Cancel",
            .DialogResult = DialogResult.Cancel,
            .Location = New Point(150, 110)
        }

            promptForm.Controls.Add(lblName)
            promptForm.Controls.Add(dtpDate)
            promptForm.Controls.Add(dtpTime)
            promptForm.Controls.Add(btnOK)
            promptForm.Controls.Add(btnCancel)
            promptForm.AcceptButton = btnOK
            promptForm.CancelButton = btnCancel

            ' Show dialog
            If promptForm.ShowDialog() = DialogResult.OK Then
                Dim selectedDate As DateTime = dtpDate.Value.Date
                Dim selectedTime As DateTime = dtpTime.Value
                Dim selectedDateTime As DateTime = selectedDate.Add(selectedTime.TimeOfDay)

                ' Insert into DB
                Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
                    conn.Open()
                    Dim sqlInsert As String = "
                    INSERT INTO raffle (raffle_number, registration_id, full_name, raffle_date, raffle_time)
                    VALUES (@num, @regid, @name, @date, @time)
                "
                    Using cmdInsert As New SQLiteCommand(sqlInsert, conn)
                        cmdInsert.Parameters.AddWithValue("@num", nextRaffleNumber)
                        cmdInsert.Parameters.AddWithValue("@regid", memberID)
                        cmdInsert.Parameters.AddWithValue("@name", fullName)
                        cmdInsert.Parameters.AddWithValue("@date", selectedDate)
                        cmdInsert.Parameters.AddWithValue("@time", selectedDateTime.ToString("HH:mm:ss"))
                        cmdInsert.ExecuteNonQuery()
                    End Using
                End Using

                MessageBox.Show($"Raffle entry #{nextRaffleNumber} added for {fullName}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Raffle entry cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error adding raffle entry: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


End Class
