Imports System.Data.SQLite
Imports System.IO

Public Class editCashflow

    ' 🔑 Cashflow primary key
    Public Property CashflowID As Long

    ' Display-only
    Public Property PlayerID As String
    Public Property FullName As String
    Public Property TimeValue As String
    Public Property BuyIn As String
    Public Property BuyInMode As String
    Public Property CashOut As String
    Public Property CashOutMode As String
    Public Property CreatedBy As String
    Public Property SessionDate As String

    Private ReadOnly dbPath As String = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "MetroCardClubDavao",
        "metrocarddavaodb.db"
    )

    Private Sub editCashflow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblFullname.Text = FullName

        ' Date
        dtpDate.Format = DateTimePickerFormat.Custom
        dtpDate.CustomFormat = "dddd, MMMM dd, yyyy"

        dtpSessionDate.Format = DateTimePickerFormat.Custom
        dtpSessionDate.CustomFormat = "dddd, MMMM dd, yyyy"

        ' Time
        dtpTIme.Format = DateTimePickerFormat.Custom
        dtpTime.CustomFormat = "hh:mm:ss tt"
        dtpTime.ShowUpDown = True

        ' Transaction types
        cbTransactionType.Items.Clear()
        cbTransactionType.Items.AddRange({"Buy-In", "Cash-Out"})

        ' Payment modes
        cbPaymentMode.Items.Clear()
        cbPaymentMode.Items.AddRange({"Cash", "GCash", "Bank Transfer", "Credit Card"})

        dtpDate.Enabled = True
        dtpTime.Enabled = True

        dtpDate.BringToFront()
        dtpTime.BringToFront()



        LoadCashflowDetails()
    End Sub

    Private Sub LoadCashflowDetails()
        Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
            conn.Open()

            Dim sql As String = "
            SELECT type, amount, payment_mode, date_created, time_created, created_by, session_date
            FROM cashflows
            WHERE id = @id
        "

            Using cmd As New SQLiteCommand(sql, conn)
                cmd.Parameters.AddWithValue("@id", CashflowID)

                Using reader = cmd.ExecuteReader()
                    If reader.Read() Then
                        cbTransactionType.Text = reader("type").ToString()
                        tbAmount.Text = reader("amount").ToString()
                        cbPaymentMode.Text = reader("payment_mode").ToString()
                        tbCashierName.Text = reader("created_by").ToString()

                        ' date_created
                        Dim parsedDate As DateTime
                        If DateTime.TryParseExact(reader("date_created").ToString(),
                            "dddd, MMMM dd, yyyy",
                            Globalization.CultureInfo.InvariantCulture,
                            Globalization.DateTimeStyles.None,
                            parsedDate) Then
                            dtpDate.Value = parsedDate
                        End If

                        ' time_created
                        Dim parsedTime As DateTime
                        If DateTime.TryParseExact(reader("time_created").ToString(),
                            "hh:mm:ss tt",
                            Globalization.CultureInfo.InvariantCulture,
                            Globalization.DateTimeStyles.None,
                            parsedTime) Then
                            dtpTIme.Value = parsedTime
                        End If

                        ' session_date  ✅ ADD THIS
                        Dim parsedSession As DateTime
                        If DateTime.TryParseExact(reader("session_date").ToString(),
                            "dddd, MMMM dd, yyyy",
                            Globalization.CultureInfo.InvariantCulture,
                            Globalization.DateTimeStyles.None,
                            parsedSession) Then
                            dtpSessionDate.Value = parsedSession
                        End If
                    End If
                End Using
            End Using
        End Using
    End Sub


    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Try
            ' Validation
            If String.IsNullOrWhiteSpace(tbAmount.Text) Then
                MessageBox.Show("Please enter an amount.")
                Return
            End If

            Dim amount As Decimal
            If Not Decimal.TryParse(tbAmount.Text, amount) Then
                MessageBox.Show("Invalid amount.")
                Return
            End If

            Dim confirm = MessageBox.Show(
                "Save changes to this transaction?",
                "Confirm Update",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )

            If confirm = DialogResult.No Then Return

            Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
                conn.Open()

                Dim sql As String = "
                    UPDATE cashflows SET
                        type=@type,
                        amount=@amount,
                        payment_mode=@mode,
                        date_created=@date,
                        time_created=@time,
                        created_by=@createdBy,
                        session_date = @session
                    WHERE id=@id
                "

                Using cmd As New SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@type", cbTransactionType.Text)
                    cmd.Parameters.AddWithValue("@amount", amount)
                    cmd.Parameters.AddWithValue("@mode", cbPaymentMode.Text)
                    cmd.Parameters.AddWithValue("@date", dtpDate.Value.ToString("dddd, MMMM dd, yyyy"))
                    cmd.Parameters.AddWithValue("@session", dtpSessionDate.Value.ToString("dddd, MMMM dd, yyyy"))
                    cmd.Parameters.AddWithValue("@time", dtpTime.Value.ToString("hh:mm:ss tt"))
                    cmd.Parameters.AddWithValue("@createdBy", tbCashierName.Text.Trim())
                    cmd.Parameters.AddWithValue("@id", CashflowID)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Transaction updated successfully!", "Success")
            Dim parentForm As Form = Me.FindForm()
            If parentForm IsNot Nothing Then
                parentForm.DialogResult = DialogResult.OK ' optional
                parentForm.Close()
            End If


        Catch ex As Exception
            MessageBox.Show("Error updating transaction: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim parentForm As Form = Me.FindForm()
        If parentForm IsNot Nothing Then
            parentForm.Close()
        End If
    End Sub
    Private Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
        Dim confirm = MessageBox.Show(
        "Are you sure you want to delete this transaction?",
        "Confirm Delete",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    )

        If confirm = DialogResult.No Then Return

        Try
            Using conn As New SQLiteConnection($"Data Source={dbPath};Version=3;")
                conn.Open()

                Dim sql As String = "DELETE FROM cashflows WHERE id=@id"

                Using cmd As New SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", CashflowID)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Transaction deleted successfully.", "Deleted")

            Dim parentForm As Form = Me.FindForm()
            If parentForm IsNot Nothing Then
                parentForm.DialogResult = DialogResult.OK
                parentForm.Close()
            End If

        Catch ex As Exception
            MessageBox.Show("Error deleting transaction: " & ex.Message)
        End Try
    End Sub




End Class

