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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Guna2HtmlLabel1 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Guna2HtmlLabel2 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        dtpMonthYear = New DateTimePicker()
        dgvReports = New Guna.UI2.WinForms.Guna2DataGridView()
        btnPrintMonthly = New Guna.UI2.WinForms.Guna2Button()
        CType(dgvReports, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Guna2HtmlLabel1
        ' 
        Guna2HtmlLabel1.BackColor = Color.Transparent
        Guna2HtmlLabel1.Font = New Font("Arial Rounded MT Bold", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Guna2HtmlLabel1.Location = New Point(20, 14)
        Guna2HtmlLabel1.Margin = New Padding(3, 2, 3, 2)
        Guna2HtmlLabel1.Name = "Guna2HtmlLabel1"
        Guna2HtmlLabel1.Size = New Size(714, 32)
        Guna2HtmlLabel1.TabIndex = 1
        Guna2HtmlLabel1.Text = "MONTHLY SUMMARY OF PLAYER/ ACCOUNT LEDGDER"
        ' 
        ' Guna2HtmlLabel2
        ' 
        Guna2HtmlLabel2.BackColor = Color.Transparent
        Guna2HtmlLabel2.Font = New Font("Arial Rounded MT Bold", 16.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Guna2HtmlLabel2.ForeColor = Color.Black
        Guna2HtmlLabel2.Location = New Point(20, 82)
        Guna2HtmlLabel2.Name = "Guna2HtmlLabel2"
        Guna2HtmlLabel2.Size = New Size(93, 28)
        Guna2HtmlLabel2.TabIndex = 76
        Guna2HtmlLabel2.Text = "MONTH:"
        ' 
        ' dtpMonthYear
        ' 
        dtpMonthYear.CalendarFont = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        dtpMonthYear.Font = New Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        dtpMonthYear.Location = New Point(124, 82)
        dtpMonthYear.Margin = New Padding(3, 2, 3, 2)
        dtpMonthYear.Name = "dtpMonthYear"
        dtpMonthYear.Size = New Size(206, 32)
        dtpMonthYear.TabIndex = 77
        ' 
        ' dgvReports
        ' 
        DataGridViewCellStyle1.BackColor = Color.White
        dgvReports.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        dgvReports.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader
        dgvReports.BackgroundColor = Color.WhiteSmoke
        dgvReports.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = Color.FromArgb(CByte(100), CByte(88), CByte(255))
        DataGridViewCellStyle2.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle2.ForeColor = Color.White
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.True
        dgvReports.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        dgvReports.ColumnHeadersHeight = 4
        dgvReports.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = Color.White
        DataGridViewCellStyle3.Font = New Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle3.ForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        DataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        DataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.False
        dgvReports.DefaultCellStyle = DataGridViewCellStyle3
        dgvReports.GridColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        dgvReports.Location = New Point(20, 131)
        dgvReports.Margin = New Padding(3, 2, 3, 2)
        dgvReports.Name = "dgvReports"
        dgvReports.ReadOnly = True
        dgvReports.RowHeadersVisible = False
        dgvReports.RowHeadersWidth = 51
        dgvReports.RowTemplate.Height = 29
        dgvReports.Size = New Size(1004, 454)
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
        btnPrintMonthly.CustomizableEdges = CustomizableEdges1
        btnPrintMonthly.DisabledState.BorderColor = Color.DarkGray
        btnPrintMonthly.DisabledState.CustomBorderColor = Color.DarkGray
        btnPrintMonthly.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnPrintMonthly.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnPrintMonthly.FillColor = Color.Black
        btnPrintMonthly.Font = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnPrintMonthly.ForeColor = Color.White
        btnPrintMonthly.Location = New Point(346, 82)
        btnPrintMonthly.Name = "btnPrintMonthly"
        btnPrintMonthly.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        btnPrintMonthly.Size = New Size(168, 32)
        btnPrintMonthly.TabIndex = 79
        btnPrintMonthly.Text = "PRINT"
        ' 
        ' Reports
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.WhiteSmoke
        Controls.Add(btnPrintMonthly)
        Controls.Add(dgvReports)
        Controls.Add(dtpMonthYear)
        Controls.Add(Guna2HtmlLabel2)
        Controls.Add(Guna2HtmlLabel1)
        Margin = New Padding(3, 2, 3, 2)
        Name = "Reports"
        Size = New Size(1041, 635)
        CType(dgvReports, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Guna2HtmlLabel1 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel2 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents dtpMonthYear As DateTimePicker
    Friend WithEvents dgvReports As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents btnPrintMonthly As Guna.UI2.WinForms.Guna2Button

End Class
