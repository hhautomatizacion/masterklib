Public Class Form1

  

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim iTamanioFuente As Integer
        Dim sNombreFuente As String

        iTamanioFuente = Val(GetSetting("hhcontrols", "font", "size", "14"))
        sNombreFuente = GetSetting("hhcontrols", "font", "name", "Verdana")

        Label1.BackColor = System.Drawing.SystemColors.Control
        label1.font = New System.Drawing.Font(sNombreFuente, iTamanioFuente)
        HhMomentaryButton1.Font = New System.Drawing.Font(sNombreFuente, iTamanioFuente)
        HhMomentaryButton2.Font = New System.Drawing.Font(sNombreFuente, iTamanioFuente)

    End Sub

 
    
    Private Sub HhMomentaryButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HhMomentaryButton1.Click
        'If IsNothing(mMasterk) Then
        dResultado = Windows.Forms.DialogResult.OK
        Me.Close()
        'End If
    End Sub

    Private Sub HhMomentaryButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HhMomentaryButton2.Click
        dResultado = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class