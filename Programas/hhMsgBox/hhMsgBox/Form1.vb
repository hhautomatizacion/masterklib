Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CargarOpciones()
        Label1.BackColor = System.Drawing.SystemColors.Control
        Label1.Font = fFuente
    End Sub


    Private Sub HhMomentaryButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HhMomentaryButton1.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Private Sub HhMomentaryButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HhMomentaryButton2.Click
        DialogResult = Windows.Forms.DialogResult.OK
    End Sub
End Class