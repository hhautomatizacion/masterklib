Public Class Form1

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Visible = False
        Timer1.Enabled = False
        TextBox1.Text = ""
    End Sub
End Class