Imports System.Data.SQLite
Imports Guna.UI2.WinForms
Imports System.IO

Public Class PlayerLedger
    ' IMPORTANT:
    ' RegistrationID here should be the REAL INTEGER PK (registrations.id), not the text registration_id
    Public Property RegistrationID As Long
    Public Property FullName As String

    Private Sub PlayerLedger_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblFullname.Text = FullName

        ' ✅ DateTimePicker for DATE selection
        dtpDate.Format = DateTimePickerFormat.Custom
        dtpDate.CustomFormat = "dddd, MMMM dd, yyyy"
        dtpDate.Value = DateTime.Now

        ' ✅ Setup DateTimePicker for TIME selection
        dtpTime.Format = DateTimePickerFormat.Custom
        dtpTime.CustomFormat = "hh:mm:ss tt"  ' 12-hour format + AM/PM
        dtpTime.ShowUpDown = True
        dtpTime.Value = DateTime.Now

        ' ✅ Transaction types
        cbTransactionType.Items.Clear()
        cbTransactionType.Items.AddRange({"Buy-In", "Cash-Out"})
        cbTransactionType.SelectedIndex = 0

        ' ✅ Payment modes
        cbPaymentMode.Items.Clear()
        cbPaymentMode.Items.AddRange({"Cash", "GCash", "Bank Transfer", "Credit Card"})
        cbPaymentMode.SelectedIndex = 0
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Try
            ' 🧩 Validate required fields
            If cbTransactionType.SelectedItem Is Nothing Then
                MessageBox.Show("Please select a transaction type.")
                Return
            End If

            If String.IsNullOrWhiteSpace(tbAmount.Text) Then
                MessageBox.Show("Please enter an amount.")
                Return
            End If

            If String.IsNullOrWhiteSpace(tbCashierName.Text) Then
                MessageBox.Show("Please enter the cashier's name.")
                Return
            End If

            ' ✅ Parse amount safely
            Dim transactionType = cbTransactionType.SelectedItem.ToString()
            Dim amount As Decimal
            If Not Decimal.TryParse(tbAmount.Text, amount) Then
                MessageBox.Show("Invalid amount entered.")
                Return
            End If

            ' ✅ Get chosen date and time
            Dim selectedDate As String = dtpDate.Value.ToString("dddd, MMMM dd, yyyy")
            Dim selectedTime As String = dtpTime.Value.ToString("hh:mm:ss tt")

            ' ✅ Define AppData path for DB
            Dim appDataPath As String = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MetroCardClubDavao"
            )

            ' ✅ Ensure folder exists
            If Not Directory.Exists(appDataPath) Then
                Directory.CreateDirectory(appDataPath)
            End If

            ' ✅ Full database path (use same DB for consistency)
            Dim dbPath As String = Path.Combine(appDataPath, "metrocarddavaodb.db")

            ' ✅ Connect to database
            Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
                conn.Open()

                ' ✅ Ensure player exists
                Dim checkSql As String = "SELECT COUNT(*) FROM registrations WHERE id = @id"
                Using checkCmd As New SQLiteCommand(checkSql, conn)
                    checkCmd.Parameters.AddWithValue("@id", RegistrationID)
                    Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                    If exists = 0 Then
                        MessageBox.Show("This player does not exist in registrations. Please register first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End Using

                ' ✅ Insert transaction safely
                Dim sql As String = "
                    INSERT INTO cashflows 
                    (registration_id, type, amount, payment_mode, date_created, time_created, created_by) 
                    VALUES (@regid, @type, @amount, @mode, @date, @time, @createdBy)
                "

                Using cmd As New SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@regid", RegistrationID)
                    cmd.Parameters.AddWithValue("@type", transactionType)
                    cmd.Parameters.AddWithValue("@amount", amount)
                    cmd.Parameters.AddWithValue("@mode", cbPaymentMode.Text)
                    cmd.Parameters.AddWithValue("@date", selectedDate)
                    cmd.Parameters.AddWithValue("@time", selectedTime)
                    cmd.Parameters.AddWithValue("@createdBy", tbCashierName.Text.Trim())
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show($"{transactionType} saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error saving transaction: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel ' ✅ Notify parent that user canceled
        Me.Close()
    End Sub

End Class
