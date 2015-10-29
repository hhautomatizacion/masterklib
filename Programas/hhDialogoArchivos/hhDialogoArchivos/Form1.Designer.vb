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
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.CheckedListBox1 = New System.Windows.Forms.CheckedListBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.Button5 = New System.Windows.Forms.Button
        Me.Button6 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button7 = New System.Windows.Forms.Button
        Me.HhGridDisplay1 = New hhGridDisplay.hhGridDisplay
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.HhCharacterEntry1 = New hhCharacterEntry.hhCharacterEntry
        CType(Me.HhGridDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CheckedListBox1
        '
        Me.CheckedListBox1.Cursor = System.Windows.Forms.Cursors.Cross
        Me.CheckedListBox1.FormattingEnabled = True
        Me.CheckedListBox1.IntegralHeight = False
        Me.CheckedListBox1.Location = New System.Drawing.Point(13, 105)
        Me.CheckedListBox1.Name = "CheckedListBox1"
        Me.CheckedListBox1.Size = New System.Drawing.Size(417, 270)
        Me.CheckedListBox1.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Cursor = System.Windows.Forms.Cursors.Cross
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Location = New System.Drawing.Point(554, 480)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(110, 90)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Ok"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Cursor = System.Windows.Forms.Cursors.Cross
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(670, 480)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(110, 90)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Cancelar"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Cursor = System.Windows.Forms.Cursors.Cross
        Me.Button3.Location = New System.Drawing.Point(156, 480)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(110, 90)
        Me.Button3.TabIndex = 3
        Me.Button3.Text = "Borrar"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Cursor = System.Windows.Forms.Cursors.Cross
        Me.Button4.Location = New System.Drawing.Point(272, 480)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(110, 90)
        Me.Button4.TabIndex = 4
        Me.Button4.Text = "Copiar"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Cursor = System.Windows.Forms.Cursors.Cross
        Me.Button5.Location = New System.Drawing.Point(388, 480)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(110, 90)
        Me.Button5.TabIndex = 5
        Me.Button5.Text = "Mover"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Cursor = System.Windows.Forms.Cursors.Cross
        Me.Button6.Location = New System.Drawing.Point(12, 480)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(110, 90)
        Me.Button6.TabIndex = 7
        Me.Button6.Text = "Cambiar"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Cross
        Me.Label1.Location = New System.Drawing.Point(13, 379)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(416, 40)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Label1"
        '
        'Button7
        '
        Me.Button7.Cursor = System.Windows.Forms.Cursors.Cross
        Me.Button7.Location = New System.Drawing.Point(13, 9)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(416, 90)
        Me.Button7.TabIndex = 9
        Me.Button7.Text = "Seleccionar todos"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'HhGridDisplay1
        '
        Me.HhGridDisplay1.AllowUserToAddRows = False
        Me.HhGridDisplay1.AllowUserToDeleteRows = False
        Me.HhGridDisplay1.AllowUserToResizeRows = False
        Me.HhGridDisplay1.AutoActualizar = False
        Me.HhGridDisplay1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.HhGridDisplay1.Cursor = System.Windows.Forms.Cursors.Cross
        Me.HhGridDisplay1.DireccionEscritura = Nothing
        Me.HhGridDisplay1.DireccionLectura = Nothing
        Me.HhGridDisplay1.DireccionPaso = Nothing
        Me.HhGridDisplay1.EscribirPaso = False
        Me.HhGridDisplay1.Font = New System.Drawing.Font("Verdana", 14.0!)
        Me.HhGridDisplay1.Link = Nothing
        Me.HhGridDisplay1.Location = New System.Drawing.Point(435, 9)
        Me.HhGridDisplay1.LongitudPaso = 0
        Me.HhGridDisplay1.LongitudTexto = 0
        Me.HhGridDisplay1.MostrarSeleccion = False
        Me.HhGridDisplay1.Name = "HhGridDisplay1"
        Me.HhGridDisplay1.PasoActual = 0
        Me.HhGridDisplay1.ReadOnly = True
        Me.HhGridDisplay1.Receta = Nothing
        Me.HhGridDisplay1.RowHeadersVisible = False
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 14.0!)
        Me.HhGridDisplay1.RowsDefaultCellStyle = DataGridViewCellStyle2
        Me.HhGridDisplay1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.HhGridDisplay1.ShowCellToolTips = False
        Me.HhGridDisplay1.Size = New System.Drawing.Size(345, 453)
        Me.HhGridDisplay1.TabIndex = 11
        '
        'Timer1
        '
        '
        'HhCharacterEntry1
        '
        Me.HhCharacterEntry1.AutoActualizar = False
        Me.HhCharacterEntry1.BackColor = System.Drawing.SystemColors.Window
        Me.HhCharacterEntry1.Cursor = System.Windows.Forms.Cursors.Cross
        Me.HhCharacterEntry1.DireccionEscritura = Nothing
        Me.HhCharacterEntry1.DireccionLectura = Nothing
        Me.HhCharacterEntry1.Etiqueta = Nothing
        Me.HhCharacterEntry1.Font = New System.Drawing.Font("Verdana", 14.0!)
        Me.HhCharacterEntry1.Link = Nothing
        Me.HhCharacterEntry1.Location = New System.Drawing.Point(13, 422)
        Me.HhCharacterEntry1.LongitudTexto = 0
        Me.HhCharacterEntry1.Name = "HhCharacterEntry1"
        Me.HhCharacterEntry1.Size = New System.Drawing.Size(416, 40)
        Me.HhCharacterEntry1.TabIndex = 14
        Me.HhCharacterEntry1.Texto = Nothing
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 580)
        Me.Controls.Add(Me.HhGridDisplay1)
        Me.Controls.Add(Me.HhCharacterEntry1)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.CheckedListBox1)
        Me.Cursor = System.Windows.Forms.Cursors.Cross
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Form1"
        CType(Me.HhGridDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CheckedListBox1 As System.Windows.Forms.CheckedListBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents HhGridDisplay1 As hhGridDisplay.hhGridDisplay
    Friend WithEvents HhCharacterEntry1 As hhCharacterEntry.hhCharacterEntry
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
