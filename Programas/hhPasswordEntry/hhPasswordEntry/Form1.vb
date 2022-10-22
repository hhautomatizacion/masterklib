Imports System.Windows.Forms

Public Class Form1
    'Public iAnchoBoton As Integer
    'Public iAltoBoton As Integer
    'Public iValorMaximo As Integer
    'Public iValorMinimo As Integer
    'Public iValor As Integer
    'Public sFactor As Single
    'Public iDecimales As Integer
    'Public sUnidades As String
    'Public cColorAlerta As System.Drawing.Color
    'Public cColorNormal As System.Drawing.Color
    '<Runtime.InteropServices.DllImport("user32")> Private Shared Function HideCaret(ByVal hWnd As IntPtr) As Integer
    'End Function
    'Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
    'Me.Close()
    'End Sub
    'Private Function EnRango(ByVal Valor As Integer, ByVal ValorMaximo As Integer, ByVal ValorMinimo As Integer) As Boolean
    'Return ((Valor <= ValorMaximo) And (Valor >= ValorMinimo))
    'End Function
    'Private Sub AceptarValor()
    'If EnRango(iValor, iValorMaximo, iValorMinimo) Then
    '        Timer1.Enabled = False
    '        DialogResult = vbOK
    ' Else
    '         Verificar()
    ' End If
    ' End Sub
    Private Sub AceptarValor()
        Dim iIter As Integer
        For iIter = 10 To 0 Step -1
            If Len(sPassword) > 0 Then
                If sPassword = GetSetting(saplicacion, sSeccion, "Password" & iIter.ToString) Then
                    iNivelAutorizacion = iIter
                    Exit For
                End If
            End If
        Next iiter
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
    'Private Sub Verificar()
    '    If EnRango(iValor, iValorMaximo, iValorMinimo) Then
    '        TextBox1.BackColor = cColorNormal
    '    Else
    '        TextBox1.BackColor = cColorAlerta
    '    End If
    '    TextBox1.SelectionStart = TextBox1.Text.Length
    '    TextBox1.SelectionLength = 0
    '    Timer1.Enabled = False
    '    Timer1.Enabled = True
    'End Sub
    'Private Function DarFormato(ByVal i As Integer) As String
    '    Dim sFormato As String
    '    If sFactor = 0 Then sFactor = 1
    '    If sFactor = 1 Then
    '        sFormato = "0"
    '    Else
    '        If iDecimales > 0 Then
    '            sFormato = "0." & StrDup(iDecimales, "0")
    '        Else
    '            sFormato = "0\."
    '        End If
    '    End If
    '    Return (i * sFactor).ToString(sFormato)
    'End Function

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
        'TextBox1.Text = DarFormato(iValor)
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

    'Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
    '   Verificar()
    'End Sub
End Class