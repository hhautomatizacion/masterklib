Imports System.Windows.Forms
Public Class Form1
    Public iAnchoBoton As Integer
    Public iAltoBoton As Integer
    Public iLongitudTexto As Integer
    Public bUpperCase As Boolean
    Public cColorNormal As System.Drawing.Color
    Public cColorAlerta As System.Drawing.Color

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Close()
    End Sub
    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        DialogResult = vbCancel
    End Sub
    Private Sub ButtonOk_Click(sender As Object, e As EventArgs) Handles ButtonOk.Click
        AceptarValor()
    End Sub
    Private Sub Verificar(ByVal t As Windows.Forms.TextBox)
        If Len(t.Text) > iLongitudTexto Then
            t.BackColor = cColorAlerta
        Else
            t.BackColor = cColorNormal
        End If
        Timer1.Enabled = False
        Timer1.Enabled = True
        t.Focus()
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Height = iAltoBoton * 4 + TextBox1.Height
        Me.Width = iAnchoBoton * 11
        Me.Top = Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - Me.Height
        Me.Left = Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - Me.Width
        CheckBox1.Checked = bUpperCase

        TextBox1.Focus()
        TextBox1.SelectAll()
        DarFormato(CheckBox1)

        Timer1.Enabled = True
    End Sub
    Private Sub Button40_Click(sender As Object, e As EventArgs) Handles ButtonBS.Click
        EliminarTexto()
    End Sub
    Private Sub EliminarTexto()
        If TextBox1.SelectedText.Length > 0 Then
            TextBox1.Text = TextBox1.Text.Substring(0, TextBox1.SelectionStart) & TextBox1.Text.Substring(TextBox1.SelectionStart + TextBox1.SelectionLength)
        Else
            If TextBox1.SelectionStart > 0 Then
                TextBox1.Text = TextBox1.Text.Substring(0, TextBox1.SelectionStart - 1) & TextBox1.Text.Substring(TextBox1.SelectionStart)
                TextBox1.SelectionStart = Len(TextBox1.Text)
                TextBox1.Focus()
            End If
        End If
    End Sub
    Private Sub Button15_MouseDown(sender As Object, e As MouseEventArgs) Handles Button1.MouseDown, Button2.MouseDown, Button3.MouseDown, Button4.MouseDown, Button5.MouseDown, Button6.MouseDown, Button7.MouseDown, Button8.MouseDown, Button9.MouseDown, Button10.MouseDown, Button11.MouseDown, Button12.MouseDown, Button13.MouseDown, Button14.MouseDown, Button15.MouseDown, Button16.MouseDown, Button17.MouseDown, Button18.MouseDown, Button19.MouseDown, Button20.MouseDown, Button21.MouseDown, Button22.MouseDown, Button23.MouseDown, Button24.MouseDown, Button25.MouseDown, Button26.MouseDown, Button27.MouseDown, Button28.MouseDown, Button29.MouseDown, Button30.MouseDown, Button31.MouseDown, Button32.MouseDown, Button33.MouseDown, Button34.MouseDown, Button35.MouseDown, Button36.MouseDown, Button37.MouseDown, Button38.MouseDown
        AgregarTexto(sender.text)
    End Sub
    Private Sub AgregarTexto(sTexto As String)
        TextBox1.Focus()
        If bUpperCase Then
            sTexto = sTexto.ToUpper
        Else
            sTexto = sTexto.ToLower
        End If
        If sTexto = "-" Then
            If bUpperCase Then sTexto = "_"
        End If
        If sTexto = "_" Then
            If Not bUpperCase Then sTexto = "-"
        End If
        If TextBox1.SelectedText.Length > 0 Then
            TextBox1.Text = TextBox1.Text.Substring(0, TextBox1.SelectionStart) & sTexto
        Else
            TextBox1.Text = TextBox1.Text & sTexto
        End If
        TextBox1.SelectionStart = Len(TextBox1.Text)
    End Sub
    Private Sub AceptarValor()
        If Len(TextBox1.Text) > iLongitudTexto Then
            Verificar(TextBox1)
        Else
            Timer1.Enabled = False
            DialogResult = vbOK
        End If
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
            Case "A" To "Z"
                AgregarTexto(e.KeyChar)
            Case "a" To "z"
                AgregarTexto(e.KeyChar)
            Case "0" To "9"
                AgregarTexto(e.KeyChar)
            Case "-", "_", " "
                AgregarTexto(e.KeyChar)
        End Select
    End Sub
    Private Sub CheckBox1_Click(sender As Object, e As EventArgs) Handles CheckBox1.Click
        Dim c As Control
        If bUpperCase Then
            bUpperCase = False
            For Each c In TableLayoutPanel1.Controls
                If TypeOf (c) Is Button Then
                    c.Text = c.Text.ToLower
                End If
            Next
            Button37.Text = "-"
        Else
            bUpperCase = True
            For Each c In TableLayoutPanel1.Controls
                If TypeOf (c) Is Button Then
                    c.Text = c.Text.ToUpper
                End If
            Next
            Button37.Text = "_"
        End If
        CheckBox1.Checked = bUpperCase
        Verificar(TextBox1)
        DarFormato(CheckBox1)
    End Sub
    Private Sub DarFormato(c As CheckBox)
        If c.Checked Then
            c.ForeColor = System.Drawing.SystemColors.HighlightText
            c.BackColor = System.Drawing.SystemColors.Highlight
        Else
            c.ForeColor = System.Drawing.SystemColors.ControlText
            c.BackColor = System.Drawing.SystemColors.Control
            c.UseVisualStyleBackColor = True
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Verificar(TextBox1)
    End Sub

    Private Sub TextBox1_GotFocus(sender As Object, e As EventArgs) Handles TextBox1.GotFocus
        TextBox1.SelectionLength = 0
    End Sub
End Class