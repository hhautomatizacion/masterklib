Public Class Form1

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim iTamanioFuente As Integer
        Dim sNombreFuente As String

        iTamanioFuente = Val(GetSetting("hhcontrols", "font", "size", "14"))
        sNombreFuente = GetSetting("hhcontrols", "font", "name", "Verdana")

        Label1.BackColor = System.Drawing.SystemColors.Control
        Label1.Font = New System.Drawing.Font(sNombreFuente, iTamanioFuente)
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