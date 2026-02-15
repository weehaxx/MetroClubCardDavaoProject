<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ViewID
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
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        PictureBox1 = New PictureBox()
        btnClose = New Guna.UI2.WinForms.Guna2Button()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        PictureBox1.Location = New Point(26, 40)
        PictureBox1.Margin = New Padding(3, 2, 3, 2)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(826, 315)
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox1.TabIndex = 0
        PictureBox1.TabStop = False
        ' 
        ' btnClose
        ' 
        btnClose.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnClose.BackColor = Color.Transparent
        btnClose.CustomizableEdges = CustomizableEdges1
        btnClose.DisabledState.BorderColor = Color.DarkGray
        btnClose.DisabledState.CustomBorderColor = Color.DarkGray
        btnClose.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnClose.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnClose.FillColor = Color.Red
        btnClose.Font = New Font("Arial Rounded MT Bold", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnClose.ForeColor = SystemColors.Window
        btnClose.Location = New Point(728, 373)
        btnClose.Margin = New Padding(3, 2, 3, 2)
        btnClose.Name = "btnClose"
        btnClose.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        btnClose.Size = New Size(124, 28)
        btnClose.TabIndex = 83
        btnClose.Text = "Close"
        ' 
        ' ViewID
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(btnClose)
        Controls.Add(PictureBox1)
        Margin = New Padding(3, 2, 3, 2)
        Name = "ViewID"
        Size = New Size(876, 423)
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnClose As Guna.UI2.WinForms.Guna2Button

End Class
