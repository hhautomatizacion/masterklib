Public Class Form1
    Dim fFuente As System.Drawing.Font
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CargarOpciones()
        Label1.BackColor = System.Drawing.SystemColors.Control
        Label1.Font = fFuente
    End Sub

    Private Sub CargarOpciones()
        Try
            fFuente = New System.Drawing.Font(GetSetting("hhControls", "Font", "FontName", "Verdana"), Val(GetSetting("hhControls", "Font", "FontSize", "18")))
        Catch ex As Exception
            fFuente = New System.Drawing.Font("Verdana", 18)
        End Try
    End Sub
    Private Sub HhMomentaryButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HhMomentaryButton1.Click
        dResultado = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub HhMomentaryButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HhMomentaryButton2.Click
        dResultado = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub SplitContainer3_Panel2_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer3.Panel2.SizeChanged
        HhNumericEntry5.Top = (SplitContainer3.Panel2.Height / 2) - (HhNumericEntry5.Height / 2)
        HhNumericEntry5.Left = SplitContainer3.Panel2.Width / 2
    End Sub
End Class