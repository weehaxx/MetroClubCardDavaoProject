<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class IDPrinting
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        pbIDphoto = New Guna.UI2.WinForms.Guna2PictureBox()
        btnPrint = New Guna.UI2.WinForms.Guna2Button()
        lblMemberID = New Label()
        pbBarcode = New PictureBox()
        lblName = New Label()
        CType(pbIDphoto, ComponentModel.ISupportInitialize).BeginInit()
        CType(pbBarcode, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' pbIDphoto
        ' 
        pbIDphoto.CustomizableEdges = CustomizableEdges1
        pbIDphoto.ImageRotate = 0F
        pbIDphoto.Location = New Point(38, 142)
        pbIDphoto.Margin = New Padding(3, 4, 3, 4)
        pbIDphoto.Name = "pbIDphoto"
        pbIDphoto.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        pbIDphoto.Size = New Size(227, 236)
        pbIDphoto.SizeMode = PictureBoxSizeMode.StretchImage
        pbIDphoto.TabIndex = 1
        pbIDphoto.TabStop = False
        ' 
        ' btnPrint
        ' 
        btnPrint.CustomizableEdges = CustomizableEdges3
        btnPrint.DisabledState.BorderColor = Color.DarkGray
        btnPrint.DisabledState.CustomBorderColor = Color.DarkGray
        btnPrint.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnPrint.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnPrint.FillColor = Color.FromArgb(CByte(0), CByte(192), CByte(0))
        btnPrint.Font = New Font("Arial Rounded MT Bold", 12.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnPrint.ForeColor = Color.White
        btnPrint.Location = New Point(348, 521)
        btnPrint.Margin = New Padding(3, 4, 3, 4)
        btnPrint.Name = "btnPrint"
        btnPrint.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        btnPrint.Size = New Size(206, 42)
        btnPrint.TabIndex = 7
        btnPrint.Text = "PRINT"
        ' 
        ' lblMemberID
        ' 
        lblMemberID.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblMemberID.BackColor = Color.Transparent
        lblMemberID.Font = New Font("Arial", 13.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblMemberID.ForeColor = Color.White
        lblMemberID.Location = New Point(45, 378)
        lblMemberID.Name = "lblMemberID"
        lblMemberID.RightToLeft = RightToLeft.Yes
        lblMemberID.Size = New Size(220, 26)
        lblMemberID.TabIndex = 8
        lblMemberID.Text = "0000000000000000"
        lblMemberID.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' pbBarcode
        ' 
        pbBarcode.BackColor = Color.White
        pbBarcode.Location = New Point(246, 445)
        pbBarcode.Name = "pbBarcode"
        pbBarcode.Size = New Size(383, 54)
        pbBarcode.SizeMode = PictureBoxSizeMode.Zoom
        pbBarcode.TabIndex = 9
        pbBarcode.TabStop = False
        ' 
        ' lblName
        ' 
        lblName.AutoSize = True
        lblName.Font = New Font("Arial Narrow", 22.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblName.ForeColor = Color.White
        lblName.Location = New Point(733, 67)
        lblName.Name = "lblName"
        lblName.RightToLeft = RightToLeft.No
        lblName.Size = New Size(106, 43)
        lblName.TabIndex = 10
        lblName.Text = "NAME"
        lblName.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' IDPrinting
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.Transparent
        BackgroundImage = My.Resources.Resources.Membership_ID_Back_CR80_size___Blank_01
        BackgroundImageLayout = ImageLayout.Stretch
        Controls.Add(lblName)
        Controls.Add(pbBarcode)
        Controls.Add(pbIDphoto)
        Controls.Add(lblMemberID)
        Controls.Add(btnPrint)
        Margin = New Padding(0)
        Name = "IDPrinting"
        Size = New Size(874, 567)
        CType(pbIDphoto, ComponentModel.ISupportInitialize).EndInit()
        CType(pbBarcode, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents pbIDphoto As Guna.UI2.WinForms.Guna2PictureBox
    Friend WithEvents btnPrint As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents lblMemberID As Label
    Friend WithEvents pbBarcode As PictureBox
    Friend WithEvents lblName As Label

End Class
