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
        Dim CustomizableEdges11 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges12 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Label25 = New Label()
        dgvRaffleEntries = New Guna.UI2.WinForms.Guna2DataGridView()
        btnSubmit = New Guna.UI2.WinForms.Guna2Button()
        Panel2 = New Panel()
        lblTotalRaffleEntries = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Guna2HtmlLabel13 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Guna2HtmlLabel4 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Guna2HtmlLabel3 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        tbMiddleName = New Guna.UI2.WinForms.Guna2TextBox()
        tbFIrstName = New Guna.UI2.WinForms.Guna2TextBox()
        tbLastName = New Guna.UI2.WinForms.Guna2TextBox()
        Guna2HtmlLabel2 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Guna2PictureBox1 = New Guna.UI2.WinForms.Guna2PictureBox()
        btnReset = New Guna.UI2.WinForms.Guna2Button()
        CType(dgvRaffleEntries, ComponentModel.ISupportInitialize).BeginInit()
        Panel2.SuspendLayout()
        CType(Guna2PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label25
        ' 
        Label25.AutoSize = True
        Label25.BackColor = Color.Transparent
        Label25.Font = New Font("Arial Rounded MT Bold", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
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
        DataGridViewCellStyle2.Font = New Font("Arial Rounded MT Bold", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle2.ForeColor = Color.Black
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.ActiveCaption
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.True
        dgvRaffleEntries.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        dgvRaffleEntries.ColumnHeadersHeight = 40
        dgvRaffleEntries.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(249))
        DataGridViewCellStyle3.Font = New Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
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
        dgvRaffleEntries.Size = New Size(545, 595)
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
        dgvRaffleEntries.ThemeStyle.HeaderStyle.Font = New Font("Arial Rounded MT Bold", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        dgvRaffleEntries.ThemeStyle.HeaderStyle.ForeColor = Color.Black
        dgvRaffleEntries.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        dgvRaffleEntries.ThemeStyle.HeaderStyle.Height = 40
        dgvRaffleEntries.ThemeStyle.ReadOnly = True
        dgvRaffleEntries.ThemeStyle.RowsStyle.BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(249))
        dgvRaffleEntries.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgvRaffleEntries.ThemeStyle.RowsStyle.Font = New Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        dgvRaffleEntries.ThemeStyle.RowsStyle.ForeColor = Color.Black
        dgvRaffleEntries.ThemeStyle.RowsStyle.Height = 29
        dgvRaffleEntries.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(CByte(239), CByte(241), CByte(243))
        dgvRaffleEntries.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black
        ' 
        ' btnSubmit
        ' 
        btnSubmit.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnSubmit.BorderRadius = 10
        btnSubmit.CustomizableEdges = CustomizableEdges1
        btnSubmit.DisabledState.BorderColor = Color.DarkGray
        btnSubmit.DisabledState.CustomBorderColor = Color.DarkGray
        btnSubmit.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnSubmit.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnSubmit.FillColor = Color.Black
        btnSubmit.Font = New Font("Arial Rounded MT Bold", 10.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnSubmit.ForeColor = Color.White
        btnSubmit.Location = New Point(1191, 797)
        btnSubmit.Margin = New Padding(3, 2, 3, 2)
        btnSubmit.Name = "btnSubmit"
        btnSubmit.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        btnSubmit.Size = New Size(144, 32)
        btnSubmit.TabIndex = 71
        btnSubmit.Text = "PRINT"
        ' 
        ' Panel2
        ' 
        Panel2.BackColor = Color.DeepSkyBlue
        Panel2.Controls.Add(lblTotalRaffleEntries)
        Panel2.Controls.Add(Guna2HtmlLabel13)
        Panel2.Location = New Point(21, 76)
        Panel2.Margin = New Padding(3, 2, 3, 2)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(545, 86)
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
        ' Guna2HtmlLabel4
        ' 
        Guna2HtmlLabel4.BackColor = Color.Transparent
        Guna2HtmlLabel4.Font = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Guna2HtmlLabel4.ForeColor = Color.Black
        Guna2HtmlLabel4.Location = New Point(801, 202)
        Guna2HtmlLabel4.Name = "Guna2HtmlLabel4"
        Guna2HtmlLabel4.Size = New Size(109, 20)
        Guna2HtmlLabel4.TabIndex = 87
        Guna2HtmlLabel4.Text = "Middle Name:"
        ' 
        ' Guna2HtmlLabel3
        ' 
        Guna2HtmlLabel3.BackColor = Color.Transparent
        Guna2HtmlLabel3.Font = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Guna2HtmlLabel3.ForeColor = Color.Black
        Guna2HtmlLabel3.Location = New Point(801, 157)
        Guna2HtmlLabel3.Name = "Guna2HtmlLabel3"
        Guna2HtmlLabel3.Size = New Size(94, 20)
        Guna2HtmlLabel3.TabIndex = 86
        Guna2HtmlLabel3.Text = "First Name:"
        ' 
        ' tbMiddleName
        ' 
        tbMiddleName.CustomizableEdges = CustomizableEdges3
        tbMiddleName.DefaultText = ""
        tbMiddleName.DisabledState.BorderColor = Color.FromArgb(CByte(208), CByte(208), CByte(208))
        tbMiddleName.DisabledState.FillColor = Color.FromArgb(CByte(226), CByte(226), CByte(226))
        tbMiddleName.DisabledState.ForeColor = Color.FromArgb(CByte(138), CByte(138), CByte(138))
        tbMiddleName.DisabledState.PlaceholderForeColor = Color.FromArgb(CByte(138), CByte(138), CByte(138))
        tbMiddleName.FocusedState.BorderColor = Color.FromArgb(CByte(94), CByte(148), CByte(255))
        tbMiddleName.Font = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tbMiddleName.ForeColor = Color.Black
        tbMiddleName.HoverState.BorderColor = Color.FromArgb(CByte(94), CByte(148), CByte(255))
        tbMiddleName.Location = New Point(986, 185)
        tbMiddleName.Margin = New Padding(4, 5, 4, 5)
        tbMiddleName.Name = "tbMiddleName"
        tbMiddleName.PlaceholderText = ""
        tbMiddleName.ReadOnly = True
        tbMiddleName.SelectedText = ""
        tbMiddleName.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        tbMiddleName.Size = New Size(219, 36)
        tbMiddleName.TabIndex = 85
        ' 
        ' tbFIrstName
        ' 
        tbFIrstName.CustomizableEdges = CustomizableEdges5
        tbFIrstName.DefaultText = ""
        tbFIrstName.DisabledState.BorderColor = Color.FromArgb(CByte(208), CByte(208), CByte(208))
        tbFIrstName.DisabledState.FillColor = Color.FromArgb(CByte(226), CByte(226), CByte(226))
        tbFIrstName.DisabledState.ForeColor = Color.FromArgb(CByte(138), CByte(138), CByte(138))
        tbFIrstName.DisabledState.PlaceholderForeColor = Color.FromArgb(CByte(138), CByte(138), CByte(138))
        tbFIrstName.FocusedState.BorderColor = Color.FromArgb(CByte(94), CByte(148), CByte(255))
        tbFIrstName.Font = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tbFIrstName.ForeColor = Color.Black
        tbFIrstName.HoverState.BorderColor = Color.FromArgb(CByte(94), CByte(148), CByte(255))
        tbFIrstName.Location = New Point(986, 139)
        tbFIrstName.Margin = New Padding(4, 5, 4, 5)
        tbFIrstName.Name = "tbFIrstName"
        tbFIrstName.PlaceholderText = ""
        tbFIrstName.ReadOnly = True
        tbFIrstName.SelectedText = ""
        tbFIrstName.ShadowDecoration.CustomizableEdges = CustomizableEdges6
        tbFIrstName.Size = New Size(219, 36)
        tbFIrstName.TabIndex = 84
        ' 
        ' tbLastName
        ' 
        tbLastName.AutoValidate = AutoValidate.Disable
        tbLastName.CustomizableEdges = CustomizableEdges7
        tbLastName.DefaultText = ""
        tbLastName.DisabledState.BorderColor = Color.FromArgb(CByte(208), CByte(208), CByte(208))
        tbLastName.DisabledState.FillColor = Color.FromArgb(CByte(226), CByte(226), CByte(226))
        tbLastName.DisabledState.ForeColor = Color.FromArgb(CByte(138), CByte(138), CByte(138))
        tbLastName.DisabledState.PlaceholderForeColor = Color.FromArgb(CByte(138), CByte(138), CByte(138))
        tbLastName.FocusedState.BorderColor = Color.FromArgb(CByte(94), CByte(148), CByte(255))
        tbLastName.Font = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tbLastName.ForeColor = Color.Black
        tbLastName.HideSelection = False
        tbLastName.HoverState.BorderColor = Color.FromArgb(CByte(94), CByte(148), CByte(255))
        tbLastName.Location = New Point(986, 91)
        tbLastName.Margin = New Padding(4, 5, 4, 5)
        tbLastName.Name = "tbLastName"
        tbLastName.PlaceholderText = ""
        tbLastName.ReadOnly = True
        tbLastName.SelectedText = ""
        tbLastName.ShadowDecoration.CustomizableEdges = CustomizableEdges8
        tbLastName.Size = New Size(219, 36)
        tbLastName.TabIndex = 83
        ' 
        ' Guna2HtmlLabel2
        ' 
        Guna2HtmlLabel2.BackColor = Color.Transparent
        Guna2HtmlLabel2.Font = New Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Guna2HtmlLabel2.ForeColor = Color.Black
        Guna2HtmlLabel2.Location = New Point(801, 108)
        Guna2HtmlLabel2.Name = "Guna2HtmlLabel2"
        Guna2HtmlLabel2.Size = New Size(93, 20)
        Guna2HtmlLabel2.TabIndex = 82
        Guna2HtmlLabel2.Text = "Last Name:"
        ' 
        ' Guna2PictureBox1
        ' 
        Guna2PictureBox1.CustomizableEdges = CustomizableEdges9
        Guna2PictureBox1.ImageRotate = 0F
        Guna2PictureBox1.Location = New Point(590, 76)
        Guna2PictureBox1.Margin = New Padding(3, 2, 3, 2)
        Guna2PictureBox1.Name = "Guna2PictureBox1"
        Guna2PictureBox1.ShadowDecoration.CustomizableEdges = CustomizableEdges10
        Guna2PictureBox1.Size = New Size(190, 174)
        Guna2PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        Guna2PictureBox1.TabIndex = 88
        Guna2PictureBox1.TabStop = False
        ' 
        ' btnReset
        ' 
        btnReset.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnReset.CustomizableEdges = CustomizableEdges11
        btnReset.DisabledState.BorderColor = Color.DarkGray
        btnReset.DisabledState.CustomBorderColor = Color.DarkGray
        btnReset.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnReset.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnReset.FillColor = Color.Red
        btnReset.Font = New Font("Arial Rounded MT Bold", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnReset.ForeColor = Color.White
        btnReset.Location = New Point(21, 783)
        btnReset.Margin = New Padding(3, 2, 3, 2)
        btnReset.Name = "btnReset"
        btnReset.ShadowDecoration.CustomizableEdges = CustomizableEdges12
        btnReset.Size = New Size(173, 32)
        btnReset.TabIndex = 89
        btnReset.Text = "RESET RAFFLE"
        ' 
        ' RaffleEntry
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(btnReset)
        Controls.Add(Guna2PictureBox1)
        Controls.Add(Guna2HtmlLabel4)
        Controls.Add(Guna2HtmlLabel3)
        Controls.Add(tbMiddleName)
        Controls.Add(tbFIrstName)
        Controls.Add(tbLastName)
        Controls.Add(Guna2HtmlLabel2)
        Controls.Add(Panel2)
        Controls.Add(btnSubmit)
        Controls.Add(dgvRaffleEntries)
        Controls.Add(Label25)
        Name = "RaffleEntry"
        Size = New Size(1351, 849)
        CType(dgvRaffleEntries, ComponentModel.ISupportInitialize).EndInit()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        CType(Guna2PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label25 As Label
    Friend WithEvents dgvRaffleEntries As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents btnSubmit As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblTotalRaffleEntries As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel13 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel4 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel3 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents tbMiddleName As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents tbFIrstName As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents tbLastName As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Guna2HtmlLabel2 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2PictureBox1 As Guna.UI2.WinForms.Guna2PictureBox
    Friend WithEvents btnReset As Guna.UI2.WinForms.Guna2Button

End Class
