Imports System.Data.SQLite
Imports System.IO
Imports AForge.Video
Imports AForge.Video.DirectShow

Public Class Registration
    Dim conn As SQLiteConnection
    Dim cmd As SQLiteCommand

    ' Webcam variables
    Private videoDevices As FilterInfoCollection
    Private videoSource As VideoCaptureDevice
    Private isCaptured As Boolean = False ' ✅ track if capture was pressed

    Private Sub Registration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' ✅ Safe database path (AppData folder)
            Dim dbFolder As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MetroCardClubDavao")
            Directory.CreateDirectory(dbFolder)
            Dim dbPath As String = Path.Combine(dbFolder, "metrocarddavaodb.db")

            conn = New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
            conn.Open()

            ' ✅ Enable Write-Ahead Logging for safety
            Using pragmaCmd As New SQLiteCommand("PRAGMA journal_mode = WAL;", conn)
                pragmaCmd.ExecuteNonQuery()
            End Using

            ' ✅ Auto-create table if it doesn’t exist
            Dim createTableSQL As String = "
                CREATE TABLE IF NOT EXISTS registrations (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    registration_id TEXT,
                    lastname TEXT,
                    firstname TEXT,
                    middlename TEXT,
                    alternativename TEXT,
                    presentaddress TEXT,
                    permanentaddress TEXT,
                    birthday TEXT,
                    birthplace TEXT,
                    civilstatus TEXT,
                    nationality TEXT,
                    email TEXT,
                    mobilenumber TEXT,
                    employmentstatus TEXT,
                    businessname TEXT,
                    employername TEXT,
                    businessnature TEXT,
                    workname TEXT,
                    presentedid TEXT,
                    polmember TEXT,
                    relationshippol TEXT,
                    nameemergency TEXT,
                    relationshipemergency TEXT,
                    contactemergency TEXT,
                    idimage BLOB,
                    photo BLOB
                );"
            Using cmd As New SQLiteCommand(createTableSQL, conn)
                cmd.ExecuteNonQuery()
            End Using

            conn.Close()

            ' UI setup
            tbRelationshipPol.Enabled = False
            btnSave.Enabled = False
            tnBusinessName.Enabled = False
            tbBusinessNature.Enabled = False
            tbEmployerName.Enabled = False
            tnWorkName.Enabled = False

            ' Load webcams
            videoDevices = New FilterInfoCollection(FilterCategory.VideoInputDevice)
            cbCamera.Items.Clear()
            For Each cam As FilterInfo In videoDevices
                cbCamera.Items.Add(cam.Name)
            Next
            If cbCamera.Items.Count > 0 Then
                cbCamera.SelectedIndex = 0
            Else
                cbCamera.Items.Add("No Camera Found")
                cbCamera.SelectedIndex = 0
                btnWebcam.Enabled = False
            End If

            ' Load dropdowns
            cbIDPresented.Items.AddRange(New String() {
                "Philippine Passport", "Driver’s License", "SSS ID", "Postal ID",
                "Voter’s ID", "PRC ID", "National ID", "Company ID", "Senior Citizen ID", "Other..."
            })

            cbCivilStatus.Items.AddRange(New String() {
                "Single", "Married", "Widowed", "Divorced", "Separated", "Annulled"
            })
        Catch ex As Exception
            MessageBox.Show("Error initializing: " & ex.Message)
        End Try
    End Sub
    Private Sub cbIDPresented_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbIDPresented.SelectedIndexChanged
        If cbIDPresented.SelectedItem IsNot Nothing AndAlso cbIDPresented.SelectedItem.ToString() = "Other..." Then
            Dim customID As String = InputBox("Please specify the type of ID:", "Other ID Type")

            If Not String.IsNullOrWhiteSpace(customID) Then
                ' Add new ID type dynamically if not already in the list
                If Not cbIDPresented.Items.Contains(customID) Then
                    cbIDPresented.Items.Insert(cbIDPresented.Items.Count - 1, customID)
                End If

                cbIDPresented.SelectedItem = customID
            Else
                ' If user cancels or leaves blank, reset selection
                cbIDPresented.SelectedIndex = -1
            End If
        End If
    End Sub


    ' -------------------- VALIDATION --------------------
    Private Sub ValidateForm()
        Dim mobileValid As Boolean = IsNumeric(tbMobileNumber.Text) AndAlso tbMobileNumber.Text.Trim() <> ""
        Dim contactValid As Boolean = tbContactEmergency.Text.Trim() <> "" ' ✅ allow any number length

        Dim allFieldsFilled As Boolean =
            Not String.IsNullOrWhiteSpace(tbLastName.Text) AndAlso
            Not String.IsNullOrWhiteSpace(tbFirstName.Text) AndAlso
            Not String.IsNullOrWhiteSpace(tbMiddleName.Text) AndAlso
            Not String.IsNullOrWhiteSpace(tbAlternativeName.Text) AndAlso
            Not String.IsNullOrWhiteSpace(tbPresentAddress.Text) AndAlso
            Not String.IsNullOrWhiteSpace(tbPermanentAddress.Text) AndAlso
            Not String.IsNullOrWhiteSpace(tbBirthPlace.Text) AndAlso
            Not String.IsNullOrWhiteSpace(cbCivilStatus.Text) AndAlso
            Not String.IsNullOrWhiteSpace(tbNationality.Text) AndAlso
            Not String.IsNullOrWhiteSpace(tbEmail.Text) AndAlso
            mobileValid AndAlso
            (rbSelfEmployed.Checked OrElse rbEmployed.Checked) AndAlso
            (tbYes.Checked OrElse tbNo.Checked) AndAlso
            (tbYes.Checked = False OrElse Not String.IsNullOrWhiteSpace(tbRelationshipPol.Text)) AndAlso
            Not String.IsNullOrWhiteSpace(tbNameEmergency.Text) AndAlso
            Not String.IsNullOrWhiteSpace(tbRelationShipEmergency.Text) AndAlso
            contactValid AndAlso
            pbCameraDisplay.Image IsNot Nothing AndAlso
            pbIDpresented.Image IsNot Nothing AndAlso
            cbIDPresented.SelectedIndex <> -1

        If rbSelfEmployed.Checked Then
            allFieldsFilled = allFieldsFilled AndAlso
                Not String.IsNullOrWhiteSpace(tnBusinessName.Text) AndAlso
                Not String.IsNullOrWhiteSpace(tbBusinessNature.Text)
        ElseIf rbEmployed.Checked Then
            allFieldsFilled = allFieldsFilled AndAlso
                Not String.IsNullOrWhiteSpace(tbEmployerName.Text) AndAlso
                Not String.IsNullOrWhiteSpace(tnWorkName.Text)
        End If

        If tbMobileNumber.Text.Length <> 11 Then allFieldsFilled = False

        btnSave.Enabled = allFieldsFilled
    End Sub

    Private Sub AnyFieldChanged(sender As Object, e As EventArgs) _
        Handles tbLastName.TextChanged, tbFirstName.TextChanged, tbMiddleName.TextChanged, tbAlternativeName.TextChanged, tbPresentAddress.TextChanged, tbPermanentAddress.TextChanged, tbBirthPlace.TextChanged, tbNationality.TextChanged, tbEmail.TextChanged, tbMobileNumber.TextChanged, tnBusinessName.TextChanged, tbEmployerName.TextChanged, tbBusinessNature.TextChanged, tnWorkName.TextChanged, tbRelationshipPol.TextChanged, tbNameEmergency.TextChanged, tbRelationShipEmergency.TextChanged, tbContactEmergency.TextChanged, rbSelfEmployed.CheckedChanged, rbEmployed.CheckedChanged, tbYes.CheckedChanged, tbNo.CheckedChanged, cbIDPresented.SelectedIndexChanged

        ValidateForm()
    End Sub

    ' -------------------- EMPLOYMENT LOGIC --------------------
    Private Sub rbSelfEmployed_CheckedChanged(sender As Object, e As EventArgs) Handles rbSelfEmployed.CheckedChanged
        If rbSelfEmployed.Checked Then
            tnBusinessName.Enabled = True
            tbBusinessNature.Enabled = True
            tbEmployerName.Enabled = False
            tnWorkName.Enabled = False
            tbEmployerName.Clear()
            tnWorkName.Clear()
        Else
            tnBusinessName.Clear()
            tbBusinessNature.Clear()
            tnBusinessName.Enabled = False
            tbBusinessNature.Enabled = False
        End If
        ValidateForm()
    End Sub

    Private Sub rbEmployed_CheckedChanged(sender As Object, e As EventArgs) Handles rbEmployed.CheckedChanged
        If rbEmployed.Checked Then
            tbEmployerName.Enabled = True
            tnWorkName.Enabled = True
            tnBusinessName.Enabled = False
            tbBusinessNature.Enabled = False
            tnBusinessName.Clear()
            tbBusinessNature.Clear()
        Else
            tbEmployerName.Clear()
            tnWorkName.Clear()
            tbEmployerName.Enabled = False
            tnWorkName.Enabled = False
        End If
        ValidateForm()
    End Sub

    ' -------------------- POLITICALLY EXPOSED FAMILY --------------------
    Private Sub tbYes_CheckedChanged(sender As Object, e As EventArgs) Handles tbYes.CheckedChanged
        If tbYes.Checked Then tbRelationshipPol.Enabled = True
        ValidateForm()
    End Sub

    Private Sub tbNo_CheckedChanged(sender As Object, e As EventArgs) Handles tbNo.CheckedChanged
        If tbNo.Checked Then
            tbRelationshipPol.Enabled = False
            tbRelationshipPol.Clear()
        End If
        ValidateForm()
    End Sub

    ' -------------------- SAVE --------------------
    ' 📌 SAVE BUTTON — Only allows save if webcam capture is done
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            ' 🧱 Validate
            If pbCameraDisplay.Image Is Nothing Then
                MessageBox.Show("Please capture or upload a photo before saving.")
                Return
            End If

            ' ✅ Safe DB path
            Dim dbPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MetroCardClubDavao", "metrocarddavaodb.db")

            ' ✅ Create backup before writing
            Dim backupPath As String = dbPath & ".bak"
            If File.Exists(dbPath) Then
                File.Copy(dbPath, backupPath, True)
            End If

            Using conn As New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
                conn.Open()

                ' Enable WAL again (redundant but safe)
                Using pragmaCmd As New SQLiteCommand("PRAGMA journal_mode = WAL;", conn)
                    pragmaCmd.ExecuteNonQuery()
                End Using

                Using trans = conn.BeginTransaction()
                    Try
                        ' Insert data
                        Dim sql As String = "
                            INSERT INTO registrations (
                                lastname, firstname, middlename, alternativename,
                                presentaddress, permanentaddress, birthday, birthplace,
                                civilstatus, nationality, email, mobilenumber, employmentstatus,
                                businessname, employername, businessnature, workname,
                                presentedid, polmember, relationshippol,
                                nameemergency, relationshipemergency, contactemergency,
                                idimage, photo
                            ) VALUES (
                                @lastname, @firstname, @middlename, @alternativename,
                                @presentaddress, @permanentaddress, @birthday, @birthplace,
                                @civilstatus, @nationality, @email, @mobilenumber, @employmentstatus,
                                @businessname, @employername, @businessnature, @workname,
                                @presentedid, @polmember, @relationshippol,
                                @nameemergency, @relationshipemergency, @contactemergency,
                                @idimage, @photo
                            );
                            SELECT last_insert_rowid();"

                        Dim newId As Long
                        Using cmd As New SQLiteCommand(sql, conn, trans)
                            cmd.Parameters.AddWithValue("@lastname", tbLastName.Text)
                            cmd.Parameters.AddWithValue("@firstname", tbFirstName.Text)
                            cmd.Parameters.AddWithValue("@middlename", tbMiddleName.Text)
                            cmd.Parameters.AddWithValue("@alternativename", tbAlternativeName.Text)
                            cmd.Parameters.AddWithValue("@presentaddress", tbPresentAddress.Text)
                            cmd.Parameters.AddWithValue("@permanentaddress", tbPermanentAddress.Text)
                            cmd.Parameters.AddWithValue("@birthday", dtpBirthday.Value.ToString("yyyy-MM-dd"))
                            cmd.Parameters.AddWithValue("@birthplace", tbBirthPlace.Text)
                            cmd.Parameters.AddWithValue("@civilstatus", cbCivilStatus.Text)
                            cmd.Parameters.AddWithValue("@nationality", tbNationality.Text)
                            cmd.Parameters.AddWithValue("@email", tbEmail.Text)
                            cmd.Parameters.AddWithValue("@mobilenumber", tbMobileNumber.Text)
                            cmd.Parameters.AddWithValue("@employmentstatus", If(rbSelfEmployed.Checked, "Self-Employed", If(rbEmployed.Checked, "Employed", "")))
                            cmd.Parameters.AddWithValue("@businessname", tnBusinessName.Text)
                            cmd.Parameters.AddWithValue("@employername", tbEmployerName.Text)
                            cmd.Parameters.AddWithValue("@businessnature", tbBusinessNature.Text)
                            cmd.Parameters.AddWithValue("@workname", tnWorkName.Text)
                            cmd.Parameters.AddWithValue("@presentedid", cbIDPresented.Text)
                            cmd.Parameters.AddWithValue("@polmember", If(tbYes.Checked, "Yes", "No"))
                            cmd.Parameters.AddWithValue("@relationshippol", tbRelationshipPol.Text)
                            cmd.Parameters.AddWithValue("@nameemergency", tbNameEmergency.Text)
                            cmd.Parameters.AddWithValue("@relationshipemergency", tbRelationShipEmergency.Text)
                            cmd.Parameters.AddWithValue("@contactemergency", tbContactEmergency.Text)

                            ' Convert images to bytes
                            Dim idImgBytes As Byte() = Nothing
                            If pbIDpresented.Image IsNot Nothing Then
                                Using ms As New MemoryStream()
                                    pbIDpresented.Image.Save(ms, Imaging.ImageFormat.Jpeg)
                                    idImgBytes = ms.ToArray()
                                End Using
                            End If
                            cmd.Parameters.AddWithValue("@idimage", idImgBytes)

                            Dim photoBytes As Byte() = Nothing
                            If pbCameraDisplay.Image IsNot Nothing Then
                                Using ms As New MemoryStream()
                                    pbCameraDisplay.Image.Save(ms, Imaging.ImageFormat.Jpeg)
                                    photoBytes = ms.ToArray()
                                End Using
                            End If
                            cmd.Parameters.AddWithValue("@photo", photoBytes)

                            newId = CLng(cmd.ExecuteScalar())
                        End Using

                        ' Generate registration_id
                        Dim regId As String = DateTime.Now.ToString("yyyyMMdd") & newId.ToString()
                        Using updateCmd As New SQLiteCommand("UPDATE registrations SET registration_id=@r WHERE id=@i", conn, trans)
                            updateCmd.Parameters.AddWithValue("@r", regId)
                            updateCmd.Parameters.AddWithValue("@i", newId)
                            updateCmd.ExecuteNonQuery()
                        End Using

                        trans.Commit()

                        MessageBox.Show("✅ Registration saved successfully!" & vbCrLf &
                                        "Registration ID: " & regId,
                                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ClearForm()
                    Catch ex As Exception
                        trans.Rollback()
                        MessageBox.Show("Error saving data: " & ex.Message)
                    End Try
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message)
        End Try
    End Sub
    Private Sub ClearForm()
        ' Recursively clear all input controls (including nested)
        ClearAllControls(Me)

        ' Reset other flags or buttons
        btnSave.Enabled = False
        isCaptured = False

        ' Optional: reset DateTimePickers to today
        For Each dtp As DateTimePicker In Me.Controls.OfType(Of DateTimePicker)()
            dtp.Value = DateTime.Now
        Next
    End Sub

    ' 🧩 Recursive helper — handles nested containers (Panels, GroupBoxes, Guna2GroupBoxes, etc.)
    Private Sub ClearAllControls(parent As Control)
        For Each ctrl As Control In parent.Controls
            Select Case True
                Case TypeOf ctrl Is TextBox
                    DirectCast(ctrl, TextBox).Clear()

                Case TypeOf ctrl Is ComboBox
                    DirectCast(ctrl, ComboBox).SelectedIndex = -1

                Case TypeOf ctrl Is PictureBox
                    DirectCast(ctrl, PictureBox).Image = Nothing

                Case TypeOf ctrl Is RadioButton
                    DirectCast(ctrl, RadioButton).Checked = False

                Case TypeOf ctrl Is CheckBox
                    DirectCast(ctrl, CheckBox).Checked = False

                Case TypeOf ctrl Is DateTimePicker
                    DirectCast(ctrl, DateTimePicker).Value = DateTime.Now

                Case Else
                    ' Recursively process child containers
                    If ctrl.HasChildren Then
                        ClearAllControls(ctrl)
                    End If
            End Select
        Next
    End Sub


    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ' 🧼 Clear all controls recursively starting from the form
        ClearControls(Me)

        ' Reset webcam capture state
        isCaptured = False
        btnWebcam.Text = "USE WEBCAM"

        ' Re-validate to disable Save button again after clearing
        ValidateForm()
    End Sub

    ' 🔁 Recursive method to clear controls inside nested containers
    Private Sub ClearControls(parent As Control)
        For Each ctrl As Control In parent.Controls
            If TypeOf ctrl Is TextBox Then
                DirectCast(ctrl, TextBox).Clear()

            ElseIf TypeOf ctrl Is ComboBox Then
                Dim cb As ComboBox = DirectCast(ctrl, ComboBox)
                If cb.Name <> "cbCamera" Then ' Don't reset camera selection
                    cb.SelectedIndex = -1
                    cb.Text = ""
                End If

            ElseIf TypeOf ctrl Is PictureBox Then
                Dim pb As PictureBox = DirectCast(ctrl, PictureBox)
                If pb.Image IsNot Nothing Then
                    pb.Image.Dispose()
                    pb.Image = Nothing
                End If

            ElseIf TypeOf ctrl Is RadioButton Then
                DirectCast(ctrl, RadioButton).Checked = False

            ElseIf TypeOf ctrl Is CheckBox Then
                DirectCast(ctrl, CheckBox).Checked = False

            ElseIf ctrl.HasChildren Then
                ' ✅ Recurse into GroupBox, Panel, TabPage, etc.
                ClearControls(ctrl)
            End If
        Next
    End Sub


    Private Sub btnAddPhoto_Click(sender As Object, e As EventArgs) Handles btnAddPhoto.Click
        Using ofd As New OpenFileDialog
            ofd.Title = "Select a Photo"
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            If ofd.ShowDialog = DialogResult.OK Then
                pbCameraDisplay.Image = Image.FromFile(ofd.FileName)
                pbCameraDisplay.SizeMode = PictureBoxSizeMode.StretchImage
                ValidateForm()
            End If
        End Using
    End Sub

    Private Sub btnUploadID_Click(sender As Object, e As EventArgs) Handles btnUploadID.Click
        Using ofd As New OpenFileDialog()
            ofd.Title = "Select an ID Image"
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            If ofd.ShowDialog() = DialogResult.OK Then
                pbIDpresented.Image = Image.FromFile(ofd.FileName)
                pbIDpresented.SizeMode = PictureBoxSizeMode.StretchImage
                ValidateForm()
            End If
        End Using
    End Sub

    ' -------------------- WEBCAM --------------------
    Private Sub StartWebcam()
        Try
            If cbCamera.SelectedIndex < 0 OrElse videoDevices.Count = 0 Then
                MessageBox.Show("No webcam selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            videoSource = New VideoCaptureDevice(videoDevices(cbCamera.SelectedIndex).MonikerString)
            AddHandler videoSource.NewFrame, AddressOf VideoSource_NewFrame
            videoSource.Start()
            isCaptured = False
        Catch ex As Exception
            MessageBox.Show("Error starting webcam: " & ex.Message)
        End Try
    End Sub

    Private Sub StopWebcam()
        Try
            If videoSource IsNot Nothing AndAlso videoSource.IsRunning Then
                RemoveHandler videoSource.NewFrame, AddressOf VideoSource_NewFrame
                videoSource.SignalToStop()
                videoSource.WaitForStop()
                videoSource = Nothing
            End If
        Catch
        End Try
    End Sub

    Private Sub VideoSource_NewFrame(sender As Object, eventArgs As NewFrameEventArgs)
        If isCaptured Then Return
        Dim bmp As Bitmap = CType(eventArgs.Frame.Clone(), Bitmap)
        If pbCameraDisplay.InvokeRequired Then
            pbCameraDisplay.BeginInvoke(New MethodInvoker(Sub()
                                                              If pbCameraDisplay.Image IsNot Nothing Then
                                                                  pbCameraDisplay.Image.Dispose()
                                                              End If
                                                              pbCameraDisplay.Image = bmp
                                                              pbCameraDisplay.SizeMode = PictureBoxSizeMode.StretchImage
                                                          End Sub))
        Else
            If pbCameraDisplay.Image IsNot Nothing Then
                pbCameraDisplay.Image.Dispose()
            End If
            pbCameraDisplay.Image = bmp
            pbCameraDisplay.SizeMode = PictureBoxSizeMode.StretchImage
        End If
    End Sub


    Private Sub btnCapture_Click(sender As Object, e As EventArgs)
        If pbCameraDisplay.Image IsNot Nothing AndAlso Not isCaptured Then
            isCaptured = True
            btnWebcam.Text = "STOP WEBCAM"
            MessageBox.Show("Photo captured!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ValidateForm
        End If
    End Sub


    Private Sub btnWebcam_Click(sender As Object, e As EventArgs) Handles btnWebcam.Click
        If btnWebcam.Text = "USE WEBCAM" Then
            ' 🟢 Start webcam
            StartWebcam()
            btnWebcam.Text = "CAPTURE"

        ElseIf btnWebcam.Text = "CAPTURE" Then
            ' 📸 Capture current frame (freeze)
            If pbCameraDisplay.Image IsNot Nothing Then
                isCaptured = True
                btnWebcam.Text = "STOP WEBCAM"
                MessageBox.Show("Photo captured!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ValidateForm()
            End If

        ElseIf btnWebcam.Text = "STOP WEBCAM" Then
            ' 🔴 Stop webcam and reset
            StopWebcam()
            pbCameraDisplay.Image = Nothing
            isCaptured = False
            btnWebcam.Text = "USE WEBCAM"
        End If
    End Sub

    Private Sub Registration_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If Not Me.Visible Then
            StopWebcam()
        End If
    End Sub

    Private Sub cbCamera_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbCamera.SelectedIndexChanged
        If videoSource IsNot Nothing AndAlso videoSource.IsRunning Then
            StopWebcam()
            StartWebcam()
        End If
    End Sub

    Private Sub cbCivilStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbCivilStatus.SelectedIndexChanged

    End Sub

    ' ✅ Mobile Number: Max 11 digits
    Private Sub tbMobileNumber_TextChanged(sender As Object, e As EventArgs) Handles tbMobileNumber.TextChanged
        If tbMobileNumber.Text.Length > 11 Then
            tbMobileNumber.Text = tbMobileNumber.Text.Substring(0, 11)
            tbMobileNumber.SelectionStart = tbMobileNumber.Text.Length
        End If
    End Sub

    Private Sub tbMobileNumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tbMobileNumber.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' ✅ Allow only digits
    Private Sub tbContactEmergency_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tbContactEmergency.KeyPress
        ' Allow: digits and control keys (backspace, delete, etc.)
        If Not (Char.IsDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar)) Then
            e.Handled = True
        End If
    End Sub

    Private Sub tbContactEmergency_TextChanged(sender As Object, e As EventArgs) Handles tbContactEmergency.TextChanged
        ' Keep only digits
        Dim filtered As String = ""
        For Each ch As Char In tbContactEmergency.Text
            If Char.IsDigit(ch) Then
                filtered &= ch
            End If
        Next

        ' If any invalid characters were removed, update the text
        If tbContactEmergency.Text <> filtered Then
            Dim selStart As Integer = tbContactEmergency.SelectionStart
            tbContactEmergency.Text = filtered
            tbContactEmergency.SelectionStart = Math.Min(selStart, tbContactEmergency.Text.Length)
        End If
    End Sub


End Class
