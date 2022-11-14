Imports System.Windows.Forms
Public Class Form1
    Private Sub AceptarValor()
        Dim iIter As Integer
        For iIter = 10 To 0 Step -1
            If Len(sPassword) > 0 Then
                If sPassword = GetSetting(sAplicacion, sSeccion, "Password" & iIter.ToString) Then
                    iNivelAutorizacion = iIter
                    Exit For
                End If
            End If
        Next iIter
        DialogResult = vbOK
    End Sub
    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        DialogResult = vbCancel
    End Sub

    Private Sub ButtonBS_Click(sender As Object, e As EventArgs) Handles ButtonBS.Click
        EliminarTexto()
    End Sub
    Private Sub AgregarTexto(sTexto As String)
        sPassword = sPassword & sTexto
        TextBox1.Text = StrDup(Len(sPassword), "*")
    End Sub
    Private Sub EliminarTexto()
        If sPassword.Length > 0 Then
            sPassword = sPassword.Substring(0, sPassword.Length - 1)
        End If
        TextBox1.Text = StrDup(Len(sPassword), "*")
    End Sub
    Private Sub ButtonOK_Click(sender As Object, e As EventArgs) Handles ButtonOK.Click
        AceptarValor()
    End Sub
    Private Sub Button0_Click(sender As Object, e As EventArgs) Handles Button0.Click, Button1.Click, Button2.Click, Button3.Click, Button4.Click, Button5.Click, Button6.Click, Button7.Click, Button8.Click, Button9.Click
        AgregarTexto(sender.text)
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        CargarOpciones()
        Me.Height = iAltoBoton * 4 + TextBox1.Height
        Me.Width = iAnchoBoton * 4
        Me.Top = (Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height / 2) - (Me.Height / 2)
        Me.Left = (Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width / 2) - (Me.Width / 2)
        TextBox1.Focus()
        TextBox1.SelectAll()
        Timer1.Enabled = True
    End Sub
    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            AceptarValor()
        End If
        If e.KeyCode = Keys.Back Then
            EliminarTexto()
        End If
    End Sub
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        e.Handled = True
        Select Case e.KeyChar
            Case "0" To "9"
                AgregarTexto(e.KeyChar)
        End Select
    End Sub
End Class