Public Class Form1
    <Runtime.InteropServices.DllImport("user32")> Private Shared Function HideCaret(ByVal hWnd As IntPtr) As Integer
    End Function
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Visible = False
        Timer1.Enabled = False
        TextBox1.Text = ""
    End Sub
    Private Sub TextBox1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.GotFocus
        HideCaret(TextBox1.Handle)
    End Sub
End Class