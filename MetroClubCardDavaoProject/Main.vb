Public Class Main

    ' ✅ When the Main form loads, show the Members dashboard by default
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowDashboard()
    End Sub

    ' 🔹 Dashboard Button
    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        btnDashboard.Checked = True
        btnRegistration.Checked = False
        btnCashFlow.Checked = False
        btnReports.Checked = False
        btnRaffle.Checked = False
        Panel3.Controls.Clear()
        ShowDashboard()
    End Sub

    ' 🔹 Registration Button
    Private Sub btnRegistration_Click(sender As Object, e As EventArgs) Handles btnRegistration.Click
        btnDashboard.Checked = False
        btnRegistration.Checked = True
        btnCashFlow.Checked = False
        btnReports.Checked = False
        btnRaffle.Checked = False
        Panel3.Controls.Clear()

        Dim registrationUC As New Registration()
        registrationUC.Dock = DockStyle.Fill
        Panel3.Controls.Add(registrationUC)
    End Sub

    ' 🔹 Cash Flow Button
    Private Sub btnCashFlow_Click(sender As Object, e As EventArgs) Handles btnCashFlow.Click
        btnDashboard.Checked = False
        btnRegistration.Checked = False
        btnCashFlow.Checked = True
        btnReports.Checked = False
        btnRaffle.Checked = False
        Panel3.Controls.Clear()

        Dim cashFlowUC As New CashFlow()
        cashFlowUC.Dock = DockStyle.Fill
        Panel3.Controls.Add(cashFlowUC)
    End Sub

    ' 🔹 Reports Button
    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        btnDashboard.Checked = False
        btnRegistration.Checked = False
        btnCashFlow.Checked = False
        btnReports.Checked = True
        btnRaffle.Checked = False
        Panel3.Controls.Clear()

        Dim reportsUC As New Reports()
        reportsUC.Dock = DockStyle.Fill
        Panel3.Controls.Add(reportsUC)
    End Sub

    Private Sub btnRaffle_Click(sender As Object, e As EventArgs) Handles btnRaffle.Click
        btnDashboard.Checked = False
        btnRegistration.Checked = False
        btnCashFlow.Checked = False
        btnReports.Checked = False
        btnRaffle.Checked = True
        Panel3.Controls.Clear()

        Dim raffleUC As New RaffleEntry()
        raffleUC.Dock = DockStyle.Fill
        Panel3.Controls.Add(raffleUC)
    End Sub

    ' 🔹 Logout Button
    Private Sub btnLogout_Click(sender As Object, e As EventArgs)
        Dim result = MessageBox.Show("Are you sure you want to log out?",
                                     "Logout",
                                     MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            Dim loginForm As New Form1()
            loginForm.Show()
            Close()
        End If
    End Sub

    ' 🔹 Show Members Dashboard
    Private Sub ShowDashboard()
        Dim memberUC As New Members()
        memberUC.Dock = DockStyle.Fill
        Panel3.Controls.Add(memberUC)
    End Sub

    ' 🔹 Optional paint events — removed duplicates to avoid multiple loads
    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        ' Do nothing here — prevent duplicate loads
    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Panel3.Paint
        ' Do nothing here — handled by button events
    End Sub

    ' ✅ When closing the form, make sure the whole app shuts down properly
    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Application.Exit() ' Ensures the app fully closes (no background process)
    End Sub


End Class
