<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RaffleEntry
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
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges5 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges6 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges7 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges8 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges9 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges10 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Label25 = New Label()
        dgvRaffleEntries = New Guna.UI2.WinForms.Guna2DataGridView()
        btnprint = New Guna.UI2.WinForms.Guna2Button()
        Panel2 = New Panel()
        lblTotalRaffleEntries = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Guna2HtmlLabel13 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        btnReset = New Guna.UI2.WinForms.Guna2Button()
        tbSearch = New Guna.UI2.WinForms.Guna2TextBox()
        btnPrintPlayer = New Guna.UI2.WinForms.Guna2Button()
        BtnPrintByDate = New Guna.UI2.WinForms.Guna2Button()
        CType(dgvRaffleEntries, ComponentModel.ISupportInitialize).BeginInit()
        Panel2.SuspendLayout()
        SuspendLayout()
        ' 
        ' Label25
        ' 
        Label25.AutoSize = True
        Label25.BackColor = Color.Transparent
        Label25.Font = New Font("Arial Rounded MT Bold", 20.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label25.ForeColor = Color.Black
        Label25.Location = New Point(21, 21)
        Label25.Name = "Label25"
        Label25.Size = New Size(244, 32)
        Label25.TabIndex = 52
        Label25.Text = "RAFFLE ENTRIES"
        ' 
        ' dgvRaffleEntries
        ' 
        DataGridViewCellStyle1.BackColor = Color.White
        dgvRaffleEntries.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        dgvRaffleEntries.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        dgvRaffleEntries.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = Color.White
        DataGridViewCellStyle2.Font = New Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle2.ForeColor = Color.Black
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.ActiveCaption
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.True
        dgvRaffleEntries.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        dgvRaffleEntries.ColumnHeadersHeight = 40
        dgvRaffleEntries.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(249))
        DataGridViewCellStyle3.Font = New Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle3.ForeColor = Color.Black
        DataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(CByte(239), CByte(241), CByte(243))
        DataGridViewCellStyle3.SelectionForeColor = Color.Black
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.False
        dgvRaffleEntries.DefaultCellStyle = DataGridViewCellStyle3
        dgvRaffleEntries.GridColor = Color.FromArgb(CByte(247), CByte(248), CByte(249))
        dgvRaffleEntries.Location = New Point(21, 175)
        dgvRaffleEntries.Margin = New Padding(3, 2, 3, 2)
        dgvRaffleEntries.Name = "dgvRaffleEntries"
        dgvRaffleEntries.ReadOnly = True
        dgvRaffleEntries.RowHeadersVisible = False
        dgvRaffleEntries.RowHeadersWidth = 51
        dgvRaffleEntries.RowTemplate.Height = 29
        dgvRaffleEntries.Size = New Size(1314, 595)
        dgvRaffleEntries.TabIndex = 70
        dgvRaffleEntries.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.White
        dgvRaffleEntries.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White
        dgvRaffleEntries.ThemeStyle.AlternatingRowsStyle.Font = Nothing
        dgvRaffleEntries.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty
        dgvRaffleEntries.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty
        dgvRaffleEntries.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty
        dgvRaffleEntries.ThemeStyle.BackColor = Color.White
        dgvRaffleEntries.ThemeStyle.GridColor = Color.FromArgb(CByte(247), CByte(248), CByte(249))
        dgvRaffleEntries.ThemeStyle.HeaderStyle.BackColor = Color.White
        dgvRaffleEntries.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None
        dgvRaffleEntries.ThemeStyle.HeaderStyle.Font = New Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        dgvRaffleEntries.ThemeStyle.HeaderStyle.ForeColor = Color.Black
        dgvRaffleEntries.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        dgvRaffleEntries.ThemeStyle.HeaderStyle.Height = 40
        dgvRaffleEntries.ThemeStyle.ReadOnly = True
        dgvRaffleEntries.ThemeStyle.RowsStyle.BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(249))
        dgvRaffleEntries.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgvRaffleEntries.ThemeStyle.RowsStyle.Font = New Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        dgvRaffleEntries.ThemeStyle.RowsStyle.ForeColor = Color.Black
        dgvRaffleEntries.ThemeStyle.RowsStyle.Height = 29
        dgvRaffleEntries.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(CByte(239), CByte(241), CByte(243))
        dgvRaffleEntries.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black
        ' 
        ' btnprint
        ' 
        btnprint.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnprint.CustomizableEdges = CustomizableEdges1
        btnprint.DisabledState.BorderColor = Color.DarkGray
        btnprint.DisabledState.CustomBorderColor = Color.DarkGray
        btnprint.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnprint.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnprint.FillColor = Color.Black
        btnprint.Font = New Font("Microsoft Sans Serif", 10.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnprint.ForeColor = Color.White
        btnprint.Location = New Point(1162, 800)
        btnprint.Margin = New Padding(3, 2, 3, 2)
        btnprint.Name = "btnprint"
        btnprint.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        btnprint.Size = New Size(173, 32)
        btnprint.TabIndex = 71
        btnprint.Text = "PRINT ALL"
        ' 
        ' Panel2
        ' 
        Panel2.BackColor = Color.DeepSkyBlue
        Panel2.Controls.Add(lblTotalRaffleEntries)
        Panel2.Controls.Add(Guna2HtmlLabel13)
        Panel2.Location = New Point(21, 76)
        Panel2.Margin = New Padding(3, 2, 3, 2)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(477, 86)
        Panel2.TabIndex = 81
        ' 
        ' lblTotalRaffleEntries
        ' 
        lblTotalRaffleEntries.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblTotalRaffleEntries.BackColor = Color.Transparent
        lblTotalRaffleEntries.Font = New Font("Microsoft Sans Serif", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblTotalRaffleEntries.ForeColor = Color.White
        lblTotalRaffleEntries.Location = New Point(14, 34)
        lblTotalRaffleEntries.Name = "lblTotalRaffleEntries"
        lblTotalRaffleEntries.Size = New Size(17, 32)
        lblTotalRaffleEntries.TabIndex = 76
        lblTotalRaffleEntries.Text = "0"
        ' 
        ' Guna2HtmlLabel13
        ' 
        Guna2HtmlLabel13.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Guna2HtmlLabel13.BackColor = Color.Transparent
        Guna2HtmlLabel13.Font = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Guna2HtmlLabel13.ForeColor = Color.White
        Guna2HtmlLabel13.Location = New Point(14, 10)
        Guna2HtmlLabel13.Name = "Guna2HtmlLabel13"
        Guna2HtmlLabel13.Size = New Size(139, 20)
        Guna2HtmlLabel13.TabIndex = 75
        Guna2HtmlLabel13.Text = "TOTAL ENTRIES:"
        ' 
        ' btnReset
        ' 
        btnReset.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnReset.CustomizableEdges = CustomizableEdges3
        btnReset.DisabledState.BorderColor = Color.DarkGray
        btnReset.DisabledState.CustomBorderColor = Color.DarkGray
        btnReset.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnReset.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnReset.FillColor = Color.Red
        btnReset.Font = New Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnReset.ForeColor = Color.White
        btnReset.Location = New Point(21, 800)
        btnReset.Margin = New Padding(3, 2, 3, 2)
        btnReset.Name = "btnReset"
        btnReset.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        btnReset.Size = New Size(173, 32)
        btnReset.TabIndex = 89
        btnReset.Text = "RESET RAFFLE"
        ' 
        ' tbSearch
        ' 
        tbSearch.CustomizableEdges = CustomizableEdges5
        tbSearch.DefaultText = ""
        tbSearch.DisabledState.BorderColor = Color.FromArgb(CByte(208), CByte(208), CByte(208))
        tbSearch.DisabledState.FillColor = Color.FromArgb(CByte(226), CByte(226), CByte(226))
        tbSearch.DisabledState.ForeColor = Color.FromArgb(CByte(138), CByte(138), CByte(138))
        tbSearch.DisabledState.PlaceholderForeColor = Color.FromArgb(CByte(138), CByte(138), CByte(138))
        tbSearch.FocusedState.BorderColor = Color.FromArgb(CByte(94), CByte(148), CByte(255))
        tbSearch.Font = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tbSearch.ForeColor = Color.Black
        tbSearch.HoverState.BorderColor = Color.FromArgb(CByte(94), CByte(148), CByte(255))
        tbSearch.IconLeft = My.Resources.Resources.find
        tbSearch.Location = New Point(997, 127)
        tbSearch.Margin = New Padding(4, 5, 4, 5)
        tbSearch.Name = "tbSearch"
        tbSearch.PlaceholderForeColor = Color.Silver
        tbSearch.PlaceholderText = "Find Member"
        tbSearch.SelectedText = ""
        tbSearch.ShadowDecoration.CustomizableEdges = CustomizableEdges6
        tbSearch.Size = New Size(338, 35)
        tbSearch.TabIndex = 90
        ' 
        ' btnPrintPlayer
        ' 
        btnPrintPlayer.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnPrintPlayer.CustomizableEdges = CustomizableEdges7
        btnPrintPlayer.DisabledState.BorderColor = Color.DarkGray
        btnPrintPlayer.DisabledState.CustomBorderColor = Color.DarkGray
        btnPrintPlayer.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnPrintPlayer.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnPrintPlayer.FillColor = Color.Black
        btnPrintPlayer.Font = New Font("Microsoft Sans Serif", 10.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnPrintPlayer.ForeColor = Color.White
        btnPrintPlayer.Location = New Point(956, 800)
        btnPrintPlayer.Margin = New Padding(3, 2, 3, 2)
        btnPrintPlayer.Name = "btnPrintPlayer"
        btnPrintPlayer.ShadowDecoration.CustomizableEdges = CustomizableEdges8
        btnPrintPlayer.Size = New Size(173, 32)
        btnPrintPlayer.TabIndex = 91
        btnPrintPlayer.Text = "PRINT PLAYER"
        ' 
        ' BtnPrintByDate
        ' 
        BtnPrintByDate.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        BtnPrintByDate.CustomizableEdges = CustomizableEdges9
        BtnPrintByDate.DisabledState.BorderColor = Color.DarkGray
        BtnPrintByDate.DisabledState.CustomBorderColor = Color.DarkGray
        BtnPrintByDate.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        BtnPrintByDate.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        BtnPrintByDate.FillColor = Color.Black
        BtnPrintByDate.Font = New Font("Microsoft Sans Serif", 10.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnPrintByDate.ForeColor = Color.White
        BtnPrintByDate.Location = New Point(751, 800)
        BtnPrintByDate.Margin = New Padding(3, 2, 3, 2)
        BtnPrintByDate.Name = "BtnPrintByDate"
        BtnPrintByDate.ShadowDecoration.CustomizableEdges = CustomizableEdges10
        BtnPrintByDate.Size = New Size(173, 32)
        BtnPrintByDate.TabIndex = 92
        BtnPrintByDate.Text = "PRINT BY DATE"
        ' 
        ' RaffleEntry
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(BtnPrintByDate)
        Controls.Add(btnPrintPlayer)
        Controls.Add(tbSearch)
        Controls.Add(btnReset)
        Controls.Add(Panel2)
        Controls.Add(btnprint)
        Controls.Add(dgvRaffleEntries)
        Controls.Add(Label25)
        Name = "RaffleEntry"
        Size = New Size(1351, 849)
        CType(dgvRaffleEntries, ComponentModel.ISupportInitialize).EndInit()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label25 As Label
    Friend WithEvents dgvRaffleEntries As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents btnprint As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblTotalRaffleEntries As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel13 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents btnReset As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents tbSearch As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents btnPrintPlayer As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents BtnPrintByDate As Guna.UI2.WinForms.Guna2Button

End Class
