<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.HhMomentaryButton1 = New hhMomentaryButton.hhMomentaryButton
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.HhMomentaryButton2 = New hhMomentaryButton.hhMomentaryButton
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer
        Me.Label1 = New System.Windows.Forms.Label
        Me.HhNumericEntry5 = New hhNumericEntry.hhNumericEntry
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        Me.SuspendLayout()
        '
        'HhMomentaryButton1
        '
        Me.HhMomentaryButton1.Appearance = System.Windows.Forms.Appearance.Button
        Me.HhMomentaryButton1.AutoActualizar = False
        Me.HhMomentaryButton1.Cursor = System.Windows.Forms.Cursors.Cross
        Me.HhMomentaryButton1.DireccionEscritura = Nothing
        Me.HhMomentaryButton1.DireccionLectura = Nothing
        Me.HhMomentaryButton1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HhMomentaryButton1.Font = New System.Drawing.Font("Papyrus", 18.0!)
        Me.HhMomentaryButton1.Link = Nothing
        Me.HhMomentaryButton1.Location = New System.Drawing.Point(0, 0)
        Me.HhMomentaryButton1.Name = "HhMomentaryButton1"
        Me.HhMomentaryButton1.Size = New System.Drawing.Size(215, 84)
        Me.HhMomentaryButton1.TabIndex = 0
        Me.HhMomentaryButton1.Text = "Ok"
        Me.HhMomentaryButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.HhMomentaryButton1.UseVisualStyleBackColor = True
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.HhMomentaryButton1)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.HhMomentaryButton2)
        Me.SplitContainer2.Size = New System.Drawing.Size(444, 84)
        Me.SplitContainer2.SplitterDistance = 215
        Me.SplitContainer2.TabIndex = 0
        '
        'HhMomentaryButton2
        '
        Me.HhMomentaryButton2.Appearance = System.Windows.Forms.Appearance.Button
        Me.HhMomentaryButton2.AutoActualizar = False
        Me.HhMomentaryButton2.Cursor = System.Windows.Forms.Cursors.Cross
        Me.HhMomentaryButton2.DireccionEscritura = Nothing
        Me.HhMomentaryButton2.DireccionLectura = Nothing
        Me.HhMomentaryButton2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HhMomentaryButton2.Font = New System.Drawing.Font("Papyrus", 18.0!)
        Me.HhMomentaryButton2.Link = Nothing
        Me.HhMomentaryButton2.Location = New System.Drawing.Point(0, 0)
        Me.HhMomentaryButton2.Name = "HhMomentaryButton2"
        Me.HhMomentaryButton2.Size = New System.Drawing.Size(225, 84)
        Me.HhMomentaryButton2.TabIndex = 0
        Me.HhMomentaryButton2.Text = "Cancel"
        Me.HhMomentaryButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.HhMomentaryButton2.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer3)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(444, 278)
        Me.SplitContainer1.SplitterDistance = 190
        Me.SplitContainer1.TabIndex = 1
        '
        'SplitContainer3
        '
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        Me.SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer3.Panel1.Padding = New System.Windows.Forms.Padding(10)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.HhNumericEntry5)
        Me.SplitContainer3.Panel2.Cursor = System.Windows.Forms.Cursors.Cross
        Me.SplitContainer3.Size = New System.Drawing.Size(444, 190)
        Me.SplitContainer3.SplitterDistance = 101
        Me.SplitContainer3.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Cross
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(10, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(424, 81)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Label1"
        '
        'HhNumericEntry5
        '
        Me.HhNumericEntry5.AutoActualizar = False
        Me.HhNumericEntry5.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.HhNumericEntry5.Cursor = System.Windows.Forms.Cursors.Cross
        Me.HhNumericEntry5.DireccionEscritura = Nothing
        Me.HhNumericEntry5.DireccionLectura = Nothing
        Me.HhNumericEntry5.Etiqueta = Nothing
        Me.HhNumericEntry5.Font = New System.Drawing.Font("Papyrus", 18.0!)
        Me.HhNumericEntry5.Link = Nothing
        Me.HhNumericEntry5.Location = New System.Drawing.Point(171, 25)
        Me.HhNumericEntry5.Name = "HhNumericEntry5"
        Me.HhNumericEntry5.Size = New System.Drawing.Size(180, 40)
        Me.HhNumericEntry5.TabIndex = 15
        Me.HhNumericEntry5.Text = "0"
        Me.HhNumericEntry5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.HhNumericEntry5.Tooltip = Nothing
        Me.HhNumericEntry5.Unidades = Nothing
        Me.HhNumericEntry5.Valor = 0
        Me.HhNumericEntry5.ValorMaximo = 0
        Me.HhNumericEntry5.ValorMinimo = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(444, 278)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Cursor = System.Windows.Forms.Cursors.Cross
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Lavadora"
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        Me.SplitContainer3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents HhMomentaryButton1 As hhMomentaryButton.hhMomentaryButton
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents HhMomentaryButton2 As hhMomentaryButton.hhMomentaryButton
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer3 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents HhNumericEntry5 As hhNumericEntry.hhNumericEntry
End Class
