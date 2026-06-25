Imports System.Data.SQLite
Imports System.IO
Imports AForge.Video
Imports AForge.Video.DirectShow

Public Class EditInfo
    ' ✅ Public properties to receive selected member details
    Public Property SelectedMemberID As Integer
    Public Property SelectedFullName As String

    Private isDirty As Boolean = False ' Track changes
    Private selectedPhotoBytes As Byte() = Nothing
    Private videoDevices As FilterInfoCollection
    Private videoSource As VideoCaptureDevice
    Private capturedImage As Bitmap
    Private webcamRunning As Boolean = False
    Private photoCaptured As Boolean = False
    Private latestFrame As Bitmap

    ' ✅ Safe database path inside AppData
    Private ReadOnly dbPath As String = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "MetroCardClubDavao",
        "metrocarddavaodb.db"
    )

    Private Sub EditInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Ensure folder exists
            Dim dbFolder As String = Path.GetDirectoryName(dbPath)
            If Not Directory.Exists(dbFolder) Then
                Directory.CreateDirectory(dbFolder)
            End If

            If Not File.Exists(dbPath) Then
                MessageBox.Show("Database file not found at: " & dbPath,
                                "Database Missing", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' ✅ Load member data
            Using conn As New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
                conn.Open()

                Dim query As String = "SELECT * FROM registrations WHERE id=@id"
                Using cmd As New SQLiteCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", SelectedMemberID)

                    Using reader As SQLiteDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            tbLastName.Text = reader("lastname").ToString()
                            tbFirstName.Text = reader("firstname").ToString()
                            tbMiddleName.Text = reader("middlename").ToString()
                            tbPresentAddress.Text = reader("presentaddress").ToString()
                            tbPermanentAddress.Text = reader("permanentaddress").ToString()
                            tbEmail.Text = reader("email").ToString()
                            tbMobileNumber.Text = reader("mobilenumber").ToString()
                            dtpBirthday.Text = reader("birthday").ToString()
                            tbBirthPlace.Text = reader("birthplace").ToString()
                            tbCivilStatus.Text = reader("civilstatus").ToString()
                            tbNationality.Text = reader("nationality").ToString()

                            ' ✅ New fields
                            tbAlternativeName.Text = reader("alternativename").ToString()
                            tbPresentedID.Text = reader("presentedid").ToString()

                            ' Employment status
                            Dim empStatus As String = reader("employmentstatus").ToString()
                            If empStatus = "Self-Employed" Then
                                rbSelfEmployed.Checked = True
                                tnBusinessName.Text = reader("businessname").ToString()
                                tbBusinessNature.Text = reader("businessnature").ToString()
                            ElseIf empStatus = "Employed" Then
                                rbEmployed.Checked = True
                                tbEmployerName.Text = reader("employername").ToString()
                                tnWorkName.Text = reader("workname").ToString()
                            End If

                            ' Political membership
                            Dim polMember As String = reader("polmember").ToString()
                            If polMember = "Yes" Then
                                tbYes.Checked = True
                                tbRelationshipPol.Text = reader("relationshippol").ToString()
                            Else
                                tbNo.Checked = True
                            End If

                            ' Emergency contact
                            tbNameEmergency.Text = reader("nameemergency").ToString()
                            tbRelationShipEmergency.Text = reader("relationshipemergency").ToString()
                            tbContactEmergency.Text = reader("contactemergency").ToString()

                            ' Photo
                            If Not IsDBNull(reader("photo")) Then
                                Dim photoData As Byte() = DirectCast(reader("photo"), Byte())
                                selectedPhotoBytes = photoData
                                Using ms As New MemoryStream(photoData)
                                    pbCameraDisplay.Image = Image.FromStream(ms)
                                End Using
                            End If
                        End If
                    End Using
                End Using
            End Using

            btnSave.Enabled = False
            AddHandlerForAllInputs(Me)
            LoadCameras()
        Catch ex As Exception
            MessageBox.Show("Error loading member info for edit: " & ex.Message)
        End Try
    End Sub
    Private Sub StopCamera()

        Try

            If videoSource IsNot Nothing Then

                RemoveHandler videoSource.NewFrame, AddressOf Video_NewFrame

                If videoSource.IsRunning Then
                    videoSource.SignalToStop()
                End If

                videoSource = Nothing

            End If

        Catch
        End Try

    End Sub
    Private Sub Video_NewFrame(sender As Object, eventArgs As NewFrameEventArgs)

        Try

            Dim frame As Bitmap = DirectCast(eventArgs.Frame.Clone(), Bitmap)

            ' Store latest frame for capture
            If latestFrame IsNot Nothing Then
                latestFrame.Dispose()
            End If

            latestFrame = DirectCast(frame.Clone(), Bitmap)

            ' Form is closing/disposed
            If Me.IsDisposed OrElse pbCameraDisplay.IsDisposed Then
                frame.Dispose()
                Return
            End If

            If pbCameraDisplay.InvokeRequired Then

                Try

                    pbCameraDisplay.Invoke(
                    New MethodInvoker(
                        Sub()

                            If Me.IsDisposed OrElse pbCameraDisplay.IsDisposed Then
                                Return
                            End If

                            If pbCameraDisplay.Image IsNot Nothing Then
                                pbCameraDisplay.Image.Dispose()
                            End If

                            pbCameraDisplay.Image = DirectCast(frame.Clone(), Bitmap)

                        End Sub))

                Catch
                    ' Ignore if form is closing
                End Try

            Else

                If pbCameraDisplay.Image IsNot Nothing Then
                    pbCameraDisplay.Image.Dispose()
                End If

                pbCameraDisplay.Image = DirectCast(frame.Clone(), Bitmap)

            End If

            frame.Dispose()

        Catch
            ' Ignore camera frame errors during shutdown
        End Try

    End Sub
    Private Sub LoadCameras()

        cbCamera.Items.Clear()

        videoDevices = New FilterInfoCollection(FilterCategory.VideoInputDevice)

        For Each device As FilterInfo In videoDevices
            cbCamera.Items.Add(device.Name)
        Next

        If cbCamera.Items.Count > 0 Then
            cbCamera.SelectedIndex = 0
        Else
            MessageBox.Show("No webcam detected.")
        End If

    End Sub
    ' 🔄 Detect changes
    Private Sub Control_Changed(sender As Object, e As EventArgs)
        isDirty = True
        btnSave.Enabled = True
    End Sub

    Private Sub AddHandlerForAllInputs(ctrl As Control)
        For Each c As Control In ctrl.Controls
            If TypeOf c Is TextBox Then
                AddHandler DirectCast(c, TextBox).TextChanged, AddressOf Control_Changed
            ElseIf TypeOf c Is ComboBox Then
                AddHandler DirectCast(c, ComboBox).SelectedIndexChanged, AddressOf Control_Changed
            ElseIf TypeOf c Is RadioButton Then
                AddHandler DirectCast(c, RadioButton).CheckedChanged, AddressOf Control_Changed
            End If

            If c.HasChildren Then AddHandlerForAllInputs(c)
        Next
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If isDirty Then
            Dim result = MessageBox.Show("You have unsaved changes. Are you sure you want to cancel?",
                                         "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.No Then Return
        End If
        CloseEditInfo()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim confirmResult As DialogResult = MessageBox.Show(
            "Are you sure you want to save these changes?",
            "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmResult = DialogResult.No Then Exit Sub

        Try
            Using conn As New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
                conn.Open()

                Dim query As String =
                    "UPDATE registrations SET " &
                    "lastname=@lastname, firstname=@firstname, middlename=@middlename, " &
                    "presentaddress=@presentaddress, permanentaddress=@permanentaddress, " &
                    "email=@email, mobilenumber=@mobilenumber, birthday=@birthday, " &
                    "birthplace=@birthplace, civilstatus=@civilstatus, nationality=@nationality, " &
                    "alternativename=@alternativename, presentedid=@presentedid, " &
                    "employmentstatus=@employmentstatus, businessname=@businessname, businessnature=@businessnature, " &
                    "employername=@employername, workname=@workname, " &
                    "polmember=@polmember, relationshippol=@relationshippol, " &
                    "nameemergency=@nameemergency, relationshipemergency=@relationshipemergency, contactemergency=@contactemergency, photo=@photo " &
                    "WHERE id=@id"

                Using cmd As New SQLiteCommand(query, conn)
                    cmd.Parameters.AddWithValue("@lastname", tbLastName.Text)
                    cmd.Parameters.AddWithValue("@firstname", tbFirstName.Text)
                    cmd.Parameters.AddWithValue("@middlename", tbMiddleName.Text)
                    cmd.Parameters.AddWithValue("@presentaddress", tbPresentAddress.Text)
                    cmd.Parameters.AddWithValue("@permanentaddress", tbPermanentAddress.Text)
                    cmd.Parameters.AddWithValue("@email", tbEmail.Text)
                    cmd.Parameters.AddWithValue("@mobilenumber", tbMobileNumber.Text)
                    cmd.Parameters.AddWithValue("@birthday", dtpBirthday.Value.ToString("yyyy-MM-dd"))
                    cmd.Parameters.AddWithValue("@birthplace", tbBirthPlace.Text)
                    cmd.Parameters.AddWithValue("@civilstatus", tbCivilStatus.Text)
                    cmd.Parameters.AddWithValue("@nationality", tbNationality.Text)

                    cmd.Parameters.AddWithValue("@alternativename", tbAlternativeName.Text)
                    cmd.Parameters.AddWithValue("@presentedid", tbPresentedID.Text)

                    ' Employment status
                    Dim empStatus As String = If(rbSelfEmployed.Checked, "Self-Employed",
                                          If(rbEmployed.Checked, "Employed", ""))
                    cmd.Parameters.AddWithValue("@employmentstatus", empStatus)
                    cmd.Parameters.AddWithValue("@businessname", tnBusinessName.Text)
                    cmd.Parameters.AddWithValue("@businessnature", tbBusinessNature.Text)
                    cmd.Parameters.AddWithValue("@employername", tbEmployerName.Text)
                    cmd.Parameters.AddWithValue("@workname", tnWorkName.Text)

                    ' Political member
                    cmd.Parameters.AddWithValue("@polmember", If(tbYes.Checked, "Yes", "No"))
                    cmd.Parameters.AddWithValue("@relationshippol", tbRelationshipPol.Text)

                    ' Emergency contact
                    cmd.Parameters.AddWithValue("@nameemergency", tbNameEmergency.Text)
                    cmd.Parameters.AddWithValue("@relationshipemergency", tbRelationShipEmergency.Text)
                    cmd.Parameters.AddWithValue("@contactemergency", tbContactEmergency.Text)
                    If selectedPhotoBytes IsNot Nothing Then
                        cmd.Parameters.AddWithValue("@photo", selectedPhotoBytes)
                    Else
                        cmd.Parameters.AddWithValue("@photo", DBNull.Value)
                    End If

                    cmd.Parameters.AddWithValue("@id", SelectedMemberID)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Member information updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)

            isDirty = False
            CloseEditInfo()

        Catch ex As Exception
            MessageBox.Show("Error updating member info: " & ex.Message)
        End Try
    End Sub

    Private Sub CloseEditInfo()

        StopCamera()
        Application.DoEvents()
        If TypeOf Me.Parent Is Form Then
            Me.FindForm().Close()
        Else
            Dim parentPanel As Control = Me.Parent
            Me.Parent.Controls.Remove(Me)
            Me.Dispose()

            If TypeOf parentPanel Is Panel Then
                parentPanel.Visible = False
            End If
        End If

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
    End Sub

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
    End Sub

    Private Sub BtnAddPhoto_Click(sender As Object, e As EventArgs) Handles BtnAddPhoto.Click

        Using ofd As New OpenFileDialog()

            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"

            If ofd.ShowDialog() = DialogResult.OK Then

                ' Stop webcam if running
                StopCamera()

                webcamRunning = False
                photoCaptured = False

                Button2.Text = "Use Webcam"

                ' Replace current image
                If pbCameraDisplay.Image IsNot Nothing Then
                    pbCameraDisplay.Image.Dispose()
                    pbCameraDisplay.Image = Nothing
                End If

                pbCameraDisplay.Image = Image.FromFile(ofd.FileName)

                selectedPhotoBytes = File.ReadAllBytes(ofd.FileName)

                isDirty = True
                btnSave.Enabled = True

            End If

        End Using

    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Not webcamRunning Then

            If cbCamera.SelectedIndex < 0 Then
                MessageBox.Show("Select a camera first.")
                Return
            End If

            If pbCameraDisplay.Image IsNot Nothing Then
                pbCameraDisplay.Image.Dispose()
                pbCameraDisplay.Image = Nothing
            End If

            photoCaptured = False

            videoSource = New VideoCaptureDevice(
        videoDevices(cbCamera.SelectedIndex).MonikerString)

            AddHandler videoSource.NewFrame, AddressOf Video_NewFrame

            videoSource.Start()

            webcamRunning = True

            Button2.Text = "Capture Photo"

            Return

        End If

        If webcamRunning AndAlso Not photoCaptured Then

            MessageBox.Show("Capture button clicked.")

            If latestFrame Is Nothing Then
                MessageBox.Show("No camera frame available.")
                Return
            End If

            capturedImage = DirectCast(latestFrame.Clone(), Bitmap)

            Dim result As DialogResult =
            MessageBox.Show(
                "Do you want to use this picture?",
                "Confirm Picture",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

                If result = DialogResult.Yes Then

                    Using ms As New MemoryStream()

                        capturedImage.Save(ms, Imaging.ImageFormat.Jpeg)

                        selectedPhotoBytes = ms.ToArray()

                    End Using

                    photoCaptured = True

                    StopCamera()

                    webcamRunning = False

                    Button2.Text = "Use Webcam"

                    MessageBox.Show(
                "Photo saved successfully.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

                Else

                    capturedImage.Dispose()
                    capturedImage = Nothing

                    MessageBox.Show(
                "Retake your photo.",
                "Retake",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

                End If

            End If

        Return

        If webcamRunning AndAlso photoCaptured Then

            If videoSource IsNot Nothing AndAlso videoSource.IsRunning Then
                videoSource.SignalToStop()
            End If

            webcamRunning = False
            photoCaptured = False

            Button2.Text = "Use Webcam"

        End If

    End Sub

    Private Sub cbCamera_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbCamera.SelectedIndexChanged

        If webcamRunning Then

            videoSource.SignalToStop()

            videoSource = New VideoCaptureDevice(
            videoDevices(cbCamera.SelectedIndex).MonikerString)

            AddHandler videoSource.NewFrame, AddressOf Video_NewFrame

            videoSource.Start()

        End If

    End Sub
End Class
