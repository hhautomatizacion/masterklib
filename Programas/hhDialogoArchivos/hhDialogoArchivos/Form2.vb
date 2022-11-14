
Imports System.IO
Public Class Form2
    Dim fFuente As System.Drawing.Font
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles Me.Load
        CargarOpciones()
        CheckedListBox1.Font = ffuente
    End Sub
    Private Sub CargarOpciones()
        Try
            fFuente = New System.Drawing.Font(GetSetting("hhControls", "Font", "FontName", "Verdana"), Val(GetSetting("hhControls", "Font", "FontSize", "18")))
        Catch ex As Exception
            fFuente = New System.Drawing.Font("Verdana", 18)
        End Try
    End Sub
End Class