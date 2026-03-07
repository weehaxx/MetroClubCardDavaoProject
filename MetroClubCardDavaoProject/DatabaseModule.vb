Imports System.Data.SQLite
Imports System.IO

Module DatabaseModule
    Private appDataPath As String = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "MetroCardClubDavao"
    )
    Private dbPath As String = Path.Combine(appDataPath, "metrocarddavaodb.db")
    Private conn As SQLiteConnection

    ' ✅ Initialize and create database if not exists
    Public Sub InitializeDatabase()
        Try
            If Not Directory.Exists(appDataPath) Then
                Directory.CreateDirectory(appDataPath)
            End If

            If Not File.Exists(dbPath) Then
                SQLiteConnection.CreateFile(dbPath)
            End If

            Using conn As New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
                conn.Open()

                ' Enable WAL
                Using walCmd As New SQLiteCommand("PRAGMA journal_mode = WAL;", conn)
                    walCmd.ExecuteNonQuery()
                End Using

                Using syncCmd As New SQLiteCommand("PRAGMA synchronous = NORMAL;", conn)
                    syncCmd.ExecuteNonQuery()
                End Using

                Using fkCmd As New SQLiteCommand("PRAGMA foreign_keys = ON;", conn)
                    fkCmd.ExecuteNonQuery()
                End Using

                ' ================= USERS TABLE =================
                Dim sql As String =
            "CREATE TABLE IF NOT EXISTS users (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                username TEXT NOT NULL UNIQUE,
                password TEXT NOT NULL
            );"

                Using createCmd As New SQLiteCommand(sql, conn)
                    createCmd.ExecuteNonQuery()
                End Using

                ' Default admin
                sql = "
            INSERT INTO users (username, password)
            SELECT 'MCCDADMIN', 'MCCD2026'
            WHERE NOT EXISTS (
                SELECT 1 FROM users WHERE username='MCCDADMIN'
            );"

                Using insertCmd As New SQLiteCommand(sql, conn)
                    insertCmd.ExecuteNonQuery()
                End Using

                ' ================= REGISTRATIONS TABLE =================
                sql =
            "CREATE TABLE IF NOT EXISTS registrations (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                registration_id TEXT,
                name TEXT NOT NULL,
                birthday TEXT,
                birthplace TEXT,
                presentaddress TEXT,
                permanentaddress TEXT,
                nationality TEXT,
                mobilenumber TEXT,
                blinds TEXT,
                sourceoffund TEXT,
                worknature TEXT,
                presentedid TEXT,
                identification_number TEXT,
                front_id BLOB,
                back_id BLOB,
                photo BLOB,
                signature BLOB
            );"

                Using createRegCmd As New SQLiteCommand(sql, conn)
                    createRegCmd.ExecuteNonQuery()
                End Using

                ' ================= CASHFLOWS TABLE =================
                sql =
            "CREATE TABLE IF NOT EXISTS cashflows (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                registration_id INTEGER NOT NULL,
                type TEXT NOT NULL,
                amount DECIMAL(18,2) NOT NULL,
                payment_mode TEXT,
                date_created TEXT,
                time_created TEXT,
                session_date TEXT NOT NULL,
                created_by TEXT,
                created_at TEXT DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (registration_id)
                    REFERENCES registrations(id)
                    ON DELETE CASCADE
            );"

                Using createCashflowCmd As New SQLiteCommand(sql, conn)
                    createCashflowCmd.ExecuteNonQuery()
                End Using

                ' ================= RAFFLE TABLE =================
                sql =
                    "CREATE TABLE IF NOT EXISTS raffle (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        registration_id INTEGER NOT NULL,
                        raffle_number INTEGER,
                        full_name TEXT NOT NULL,
                        raffle_date TEXT,
                        raffle_time TEXT,
                        session_raffle_date TEXT,
                        created_at TEXT DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (registration_id)
                            REFERENCES registrations(id) ON DELETE CASCADE
                    );"

                Using createRaffleCmd As New SQLiteCommand(sql, conn)
                    createRaffleCmd.ExecuteNonQuery()
                End Using

            End Using ' ✅ THIS WAS MISSING

        Catch ex As Exception
            MessageBox.Show("Error initializing database: " & ex.Message,
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error)
        End Try
    End Sub


    ' ✅ Centralized SQLite connection (lazy-loaded)
    Public Function GetConnection() As SQLiteConnection
        If conn Is Nothing Then
            conn = New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
        End If
        Return conn
    End Function

    ' ✅ Validate login credentials
    Public Function ValidateUser(username As String, password As String) As Boolean
        Try
            Using connection = New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
                connection.Open()
                Dim query As String = "SELECT 1 FROM users WHERE username=@uname AND password=@pword"
                Using cmd As New SQLiteCommand(query, connection)
                    cmd.Parameters.AddWithValue("@uname", username)
                    cmd.Parameters.AddWithValue("@pword", password)
                    Using reader = cmd.ExecuteReader()
                        Return reader.HasRows
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error validating user: " & ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' ✅ Safe save for registration data
    Public Sub SaveRegistration(
        lastName As String, firstName As String, middleName As String, alternativeName As String,
        presentAddress As String, permanentAddress As String, birthday As Date, birthPlace As String,
        civilStatus As String, nationality As String, email As String, mobileNumber As String,
        employmentStatus As String, businessName As String, employerName As String, businessNature As String,
        workName As String, presentedId As String, polMember As String, relationshipPol As String,
        nameEmergency As String, relationshipEmergency As String, contactEmergency As String
    )
        Try
            ' Use centralized db path
            Using conn As New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
                conn.Open()

                Dim sql As String =
                    "INSERT INTO registrations " &
                    "(lastname, firstname, middlename, alternativename, presentaddress, permanentaddress, birthday, birthplace, civilstatus, nationality, email, mobilenumber, employmentstatus, businessname, employername, businessnature, workname, presentedid, polmember, relationshippol, nameemergency, relationshipemergency, contactemergency) " &
                    "VALUES (@lastname, @firstname, @middlename, @alternativename, @presentaddress, @permanentaddress, @birthday, @birthplace, @civilstatus, @nationality, @email, @mobilenumber, @employmentstatus, @businessname, @employername, @businessnature, @workname, @presentedid, @polmember, @relationshippol, @nameemergency, @relationshipemergency, @contactemergency)"

                Using cmd As New SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@lastname", lastName)
                    cmd.Parameters.AddWithValue("@firstname", firstName)
                    cmd.Parameters.AddWithValue("@middlename", middleName)
                    cmd.Parameters.AddWithValue("@alternativename", alternativeName)
                    cmd.Parameters.AddWithValue("@presentaddress", presentAddress)
                    cmd.Parameters.AddWithValue("@permanentaddress", permanentAddress)
                    cmd.Parameters.AddWithValue("@birthday", birthday.ToString("yyyy-MM-dd"))
                    cmd.Parameters.AddWithValue("@birthplace", birthPlace)
                    cmd.Parameters.AddWithValue("@civilstatus", civilStatus)
                    cmd.Parameters.AddWithValue("@nationality", nationality)
                    cmd.Parameters.AddWithValue("@email", email)
                    cmd.Parameters.AddWithValue("@mobilenumber", mobileNumber)
                    cmd.Parameters.AddWithValue("@employmentstatus", employmentStatus)
                    cmd.Parameters.AddWithValue("@businessname", businessName)
                    cmd.Parameters.AddWithValue("@employername", employerName)
                    cmd.Parameters.AddWithValue("@businessnature", businessNature)
                    cmd.Parameters.AddWithValue("@workname", workName)
                    cmd.Parameters.AddWithValue("@presentedid", presentedId)
                    cmd.Parameters.AddWithValue("@polmember", polMember)
                    cmd.Parameters.AddWithValue("@relationshippol", relationshipPol)
                    cmd.Parameters.AddWithValue("@nameemergency", nameEmergency)
                    cmd.Parameters.AddWithValue("@relationshipemergency", relationshipEmergency)
                    cmd.Parameters.AddWithValue("@contactemergency", contactEmergency)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error saving registration: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Module
