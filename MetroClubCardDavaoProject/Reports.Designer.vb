<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Reports
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Guna2HtmlLabel1 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Guna2HtmlLabel2 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        dtpMonthYear = New DateTimePicker()
        dgvReports = New Guna.UI2.WinForms.Guna2DataGridView()
        btnPrintMonthly = New Guna.UI2.WinForms.Guna2Button()
        Panel2 = New Panel()
        lblCashIn = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Guna2HtmlLabel13 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Panel3 = New Panel()
        lblCashOut = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Guna2HtmlLabel4 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        CType(dgvReports, ComponentModel.ISupportInitialize).BeginInit()
        Panel2.SuspendLayout()
        Panel3.SuspendLayout()
        SuspendLayout()
        ' 
        ' Guna2HtmlLabel1
        ' 
        Guna2HtmlLabel1.BackColor = Color.Transparent
        Guna2HtmlLabel1.Font = New Font("Arial Rounded MT Bold", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Guna2HtmlLabel1.Location = New Point(23, 19)
        Guna2HtmlLabel1.Name = "Guna2HtmlLabel1"
        Guna2HtmlLabel1.Size = New Size(897, 40)
        Guna2HtmlLabel1.TabIndex = 1
        Guna2HtmlLabel1.Text = "MONTHLY SUMMARY OF PLAYER/ ACCOUNT LEDGDER"
        ' 
        ' Guna2HtmlLabel2
        ' 
        Guna2HtmlLabel2.BackColor = Color.Transparent
        Guna2HtmlLabel2.Font = New Font("Arial Rounded MT Bold", 16.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Guna2HtmlLabel2.ForeColor = Color.Black
        Guna2HtmlLabel2.Location = New Point(23, 89)
        Guna2HtmlLabel2.Margin = New Padding(3, 4, 3, 4)
        Guna2HtmlLabel2.Name = "Guna2HtmlLabel2"
        Guna2HtmlLabel2.Size = New Size(113, 34)
        Guna2HtmlLabel2.TabIndex = 76
        Guna2HtmlLabel2.Text = "MONTH:"
        ' 
        ' dtpMonthYear
        ' 
        dtpMonthYear.CalendarFont = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        dtpMonthYear.Font = New Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        dtpMonthYear.Location = New Point(142, 89)
        dtpMonthYear.Name = "dtpMonthYear"
        dtpMonthYear.Size = New Size(235, 38)
        dtpMonthYear.TabIndex = 77
        ' 
        ' dgvReports
        ' 
        DataGridViewCellStyle4.BackColor = Color.White
        dgvReports.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle4
        dgvReports.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader
        dgvReports.BackgroundColor = Color.WhiteSmoke
        dgvReports.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
        DataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = Color.FromArgb(CByte(100), CByte(88), CByte(255))
        DataGridViewCellStyle5.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle5.ForeColor = Color.White
        DataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = DataGridViewTriState.True
        dgvReports.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        dgvReports.ColumnHeadersHeight = 4
        dgvReports.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        DataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = Color.White
        DataGridViewCellStyle6.Font = New Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle6.ForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        DataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        DataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        DataGridViewCellStyle6.WrapMode = DataGridViewTriState.False
        dgvReports.DefaultCellStyle = DataGridViewCellStyle6
        dgvReports.GridColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        dgvReports.Location = New Point(23, 309)
        dgvReports.Name = "dgvReports"
        dgvReports.ReadOnly = True
        dgvReports.RowHeadersVisible = False
        dgvReports.RowHeadersWidth = 51
        dgvReports.Size = New Size(1147, 471)
        dgvReports.TabIndex = 78
        dgvReports.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White
        dgvReports.ThemeStyle.AlternatingRowsStyle.Font = Nothing
        dgvReports.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty
        dgvReports.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty
        dgvReports.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty
        dgvReports.ThemeStyle.BackColor = Color.WhiteSmoke
        dgvReports.ThemeStyle.GridColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        dgvReports.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(CByte(100), CByte(88), CByte(255))
        dgvReports.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None
        dgvReports.ThemeStyle.HeaderStyle.Font = New Font("Segoe UI", 9F)
        dgvReports.ThemeStyle.HeaderStyle.ForeColor = Color.White
        dgvReports.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        dgvReports.ThemeStyle.HeaderStyle.Height = 4
        dgvReports.ThemeStyle.ReadOnly = True
        dgvReports.ThemeStyle.RowsStyle.BackColor = Color.White
        dgvReports.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgvReports.ThemeStyle.RowsStyle.Font = New Font("Segoe UI", 9F)
        dgvReports.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        dgvReports.ThemeStyle.RowsStyle.Height = 29
        dgvReports.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        dgvReports.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        ' 
        ' btnPrintMonthly
        ' 
        btnPrintMonthly.CustomizableEdges = CustomizableEdges3
        btnPrintMonthly.DisabledState.BorderColor = Color.DarkGray
        btnPrintMonthly.DisabledState.CustomBorderColor = Color.DarkGray
        btnPrintMonthly.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnPrintMonthly.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnPrintMonthly.FillColor = Color.Black
        btnPrintMonthly.Font = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnPrintMonthly.ForeColor = Color.White
        btnPrintMonthly.Location = New Point(395, 89)
        btnPrintMonthly.Margin = New Padding(3, 4, 3, 4)
        btnPrintMonthly.Name = "btnPrintMonthly"
        btnPrintMonthly.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        btnPrintMonthly.Size = New Size(192, 43)
        btnPrintMonthly.TabIndex = 79
        btnPrintMonthly.Text = "PRINT"
        ' 
        ' Panel2
        ' 
        Panel2.BackColor = Color.Green
        Panel2.Controls.Add(lblCashIn)
        Panel2.Controls.Add(Guna2HtmlLabel13)
        Panel2.Location = New Point(23, 153)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(493, 115)
        Panel2.TabIndex = 80
        ' 
        ' lblCashIn
        ' 
        lblCashIn.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblCashIn.BackColor = Color.Transparent
        lblCashIn.Font = New Font("Microsoft Sans Serif", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblCashIn.ForeColor = Color.White
        lblCashIn.Location = New Point(16, 45)
        lblCashIn.Margin = New Padding(3, 4, 3, 4)
        lblCashIn.Name = "lblCashIn"
        lblCashIn.Size = New Size(21, 40)
        lblCashIn.TabIndex = 76
        lblCashIn.Text = "0"
        ' 
        ' Guna2HtmlLabel13
        ' 
        Guna2HtmlLabel13.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Guna2HtmlLabel13.BackColor = Color.Transparent
        Guna2HtmlLabel13.Font = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Guna2HtmlLabel13.ForeColor = Color.White
        Guna2HtmlLabel13.Location = New Point(16, 13)
        Guna2HtmlLabel13.Margin = New Padding(3, 4, 3, 4)
        Guna2HtmlLabel13.Name = "Guna2HtmlLabel13"
        Guna2HtmlLabel13.Size = New Size(152, 25)
        Guna2HtmlLabel13.TabIndex = 75
        Guna2HtmlLabel13.Text = "TOTAL BUY-IN:"
        ' 
        ' Panel3
        ' 
        Panel3.BackColor = Color.Red
        Panel3.Controls.Add(lblCashOut)
        Panel3.Controls.Add(Guna2HtmlLabel4)
        Panel3.Location = New Point(545, 153)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(519, 115)
        Panel3.TabIndex = 81
        ' 
        ' lblCashOut
        ' 
        lblCashOut.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblCashOut.BackColor = Color.Transparent
        lblCashOut.Font = New Font("Microsoft Sans Serif", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblCashOut.ForeColor = Color.White
        lblCashOut.Location = New Point(16, 45)
        lblCashOut.Margin = New Padding(3, 4, 3, 4)
        lblCashOut.Name = "lblCashOut"
        lblCashOut.Size = New Size(21, 40)
        lblCashOut.TabIndex = 76
        lblCashOut.Text = "0"
        ' 
        ' Guna2HtmlLabel4
        ' 
        Guna2HtmlLabel4.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Guna2HtmlLabel4.BackColor = Color.Transparent
        Guna2HtmlLabel4.Font = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Guna2HtmlLabel4.ForeColor = Color.White
        Guna2HtmlLabel4.Location = New Point(16, 13)
        Guna2HtmlLabel4.Margin = New Padding(3, 4, 3, 4)
        Guna2HtmlLabel4.Name = "Guna2HtmlLabel4"
        Guna2HtmlLabel4.Size = New Size(190, 25)
        Guna2HtmlLabel4.TabIndex = 75
        Guna2HtmlLabel4.Text = "TOTAL CASH-OUT:"
        ' 
        ' Reports
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.WhiteSmoke
        Controls.Add(Panel3)
        Controls.Add(Panel2)
        Controls.Add(btnPrintMonthly)
        Controls.Add(dgvReports)
        Controls.Add(dtpMonthYear)
        Controls.Add(Guna2HtmlLabel2)
        Controls.Add(Guna2HtmlLabel1)
        Name = "Reports"
        Size = New Size(1190, 847)
        CType(dgvReports, ComponentModel.ISupportInitialize).EndInit()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        Panel3.ResumeLayout(False)
        Panel3.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Guna2HtmlLabel1 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel2 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents dtpMonthYear As DateTimePicker
    Friend WithEvents dgvReports As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents btnPrintMonthly As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblCashIn As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel13 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents lblCashOut As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel4 As Guna.UI2.WinForms.Guna2HtmlLabel

End Class
