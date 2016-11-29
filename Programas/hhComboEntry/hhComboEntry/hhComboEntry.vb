Imports system.windows.forms
Imports System.Drawing
Public Class hhComboEntry
    Inherits combobox
    Dim fTecladoEnPantalla As Form1
    Dim sId As String
    Dim iAnchoBoton As Integer
    Dim iAltoBoton As Integer
    Dim iAutoOcultar As Integer
    Dim sNombreFuente As String
    Dim iTamanioFuente As Integer
    Dim iLongitudTexto As Integer
    Dim cEtiquetaForecolor As Color
    Dim cEtiquetaBackcolor As Color
    Dim cColorNormal As Color
    Dim cColorAlerta As Color
    Dim sEtiqueta As String
    Dim lEtiqueta As Label
    Dim sDireccionLectura As String
    Dim sDireccionEscritura As String
    Dim sTexto As String
    Dim sLetras() As String = New String() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "[", "]", ".", "-", "_", " "}
    Dim sNumeros() As String = New String() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "-", "0", "+"}
    Dim mMasterk As MasterKlib.MasterK
    Dim bAutoActualizar As Boolean



    Sub New()
        MyBase.New()
        Dim b As Button

        CargarOpciones()


        Me.AutoSize = False
        Me.Font = New Font(sNombreFuente, iTamanioFuente)
        Me.Cursor = Cursors.Cross
        CrearEtiqueta()
        If Me.DesignMode Then


            EmparentarEtiqueta()

        End If

        ftecladoenpantalla = New Form1

        ftecladoenpantalla.TextBox1.Font = New System.Drawing.Font(sNombreFuente, iTamanioFuente)
        fTecladoEnPantalla.Height = iAltoBoton * 4 + fTecladoEnPantalla.TextBox1.Height
        ftecladoenpantalla.Width = iAnchoBoton * 13
        fTecladoEnPantalla.Top = Screen.PrimaryScreen.WorkingArea.Height - fTecladoEnPantalla.Height
        fTecladoEnPantalla.Left = Screen.PrimaryScreen.WorkingArea.Width - fTecladoEnPantalla.Width
        fTecladoEnPantalla.Timer1.Interval = iAutoOcultar
        fTecladoEnPantalla.Timer1.Enabled = False
        For i As Integer = 0 To sLetras.Length - 1
            b = New Button
            b.Font = New System.Drawing.Font(sNombreFuente, iTamanioFuente)
            b.Text = sLetras(i)
            b.Cursor = Cursors.Cross
            b.Top = fTecladoEnPantalla.TextBox1.Height + (i \ 8) * iAltoBoton
            b.Left = (i Mod 8) * iAnchoBoton
            b.Width = iAnchoBoton
            b.Height = iAltoBoton
            AddHandler b.MouseDown, AddressOf presiona
            AddHandler b.MouseUp, AddressOf levanta
            fTecladoEnPantalla.Controls.Add(b)
        Next i

        For i As Integer = 0 To sNumeros.Length - 1
            b = New Button
            b.Font = New System.Drawing.Font(sNombreFuente, iTamanioFuente)
            b.Text = sNumeros(i)
            b.Cursor = Cursors.Cross
            b.Top = fTecladoEnPantalla.TextBox1.Height + (i \ 3) * iAltoBoton
            b.Left = iAnchoBoton * 8.5 + (i Mod 3) * iAnchoBoton
            b.Width = iAnchoBoton
            b.Height = iAltoBoton
            AddHandler b.MouseDown, AddressOf presiona
            AddHandler b.MouseUp, AddressOf levanta
            fTecladoEnPantalla.Controls.Add(b)
        Next i

        b = New Button
        With b
            .Font = New System.Drawing.Font(sNombreFuente, iTamanioFuente)
            .Cursor = Cursors.Cross
            .Width = iAnchoBoton
            .Height = 2 * iAltoBoton
            .Top = fTecladoEnPantalla.Height - .Height
            .Left = fTecladoEnPantalla.Width - .Width
            .Text = ""
            .Image = My.Resources.circle_with_check_symbol
            AddHandler .Click, AddressOf botonok
        End With
        fTecladoEnPantalla.Controls.Add(b)

        b = New Button
        With b
            .Font = New System.Drawing.Font(sNombreFuente, iTamanioFuente)
            .Cursor = Cursors.Cross
            .Width = iAnchoBoton
            .Height = iAltoBoton
            .Top = fTecladoEnPantalla.Height - 3 * .Height
            .Left = ftecladoenpantalla.Width - .Width
            .Text = ""
            .Image = My.Resources.backspace_arrow
            AddHandler .Click, AddressOf botonbackspace
        End With
        ftecladoenpantalla.Controls.Add(b)


        b = New Button
        With b
            .Font = New System.Drawing.Font(sNombreFuente, iTamanioFuente)
            .Cursor = Cursors.Cross
            .Width = iAnchoBoton
            .Height = iAltoBoton
            .Top = fTecladoEnPantalla.Height - 4 * .Height
            .Left = fTecladoEnPantalla.Width - .Width
            .Text = ""
            .Image = My.Resources.cancel_button
            AddHandler .Click, AddressOf botoncancel
        End With
        fTecladoEnPantalla.Controls.Add(b)

        ftecladoenpantalla.Visible = False

    End Sub
    Private Sub CargarOpciones()
        cEtiquetaBackcolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelBackColor", System.Drawing.SystemColors.Highlight.ToArgb.ToString))
        cEtiquetaForecolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelForeColor", System.Drawing.SystemColors.HighlightText.ToArgb.ToString))
        cColorAlerta = Color.FromArgb(GetSetting("hhControls", "Colors", "AlertBackColor", System.Drawing.Color.Red.ToArgb.ToString))
        cColorNormal = Color.FromArgb(GetSetting("hhControls", "Colors", "NormalBackColor", System.Drawing.SystemColors.Window.ToArgb.ToString))
        iAltoBoton = Val(GetSetting("hhcontrols", "size", "SmallButtonHeight", "60"))
        iAnchoBoton = Val(GetSetting("hhcontrols", "size", "SmallButtonWidth", "60"))
        iAutoOcultar = Val(GetSetting("hhcontrols", "refresh", "autohide", "10000"))
        iTamanioFuente = Val(GetSetting("hhControls", "Font", "Size", "14"))
        sNombreFuente = GetSetting("hhControls", "Font", "Name", "Verdana")
    End Sub

    Private Sub Verificar(ByVal t As Control)
        If Len(t.Text) > iLongitudTexto Then
            t.BackColor = cColorAlerta
        Else
            t.BackColor = cColorNormal
        End If
    End Sub
    Private Sub levanta(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Verificar(fTecladoEnPantalla.TextBox1)
    End Sub
    Private Sub presiona(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        ftecladoenpantalla.TextBox1.Focus()
        If sender.text = "+" Then
            SendKeys.Send("{ADD}")
        Else

            SendKeys.Send(sender.text)
        End If

        ftecladoenpantalla.Timer1.Enabled = False
        ftecladoenpantalla.Timer1.Enabled = True
    End Sub
    Private Sub botonbackspace(ByVal sender As Object, ByVal e As System.EventArgs)

        If ftecladoenpantalla.TextBox1.SelectedText.Length > 0 Then
            fTecladoEnPantalla.TextBox1.Text = fTecladoEnPantalla.TextBox1.Text.Substring(0, fTecladoEnPantalla.TextBox1.SelectionStart) & fTecladoEnPantalla.TextBox1.Text.Substring(fTecladoEnPantalla.TextBox1.SelectionStart + fTecladoEnPantalla.TextBox1.SelectionLength)
        Else
            If ftecladoenpantalla.TextBox1.SelectionStart > 0 Then
                fTecladoEnPantalla.TextBox1.Text = fTecladoEnPantalla.TextBox1.Text.Substring(0, fTecladoEnPantalla.TextBox1.SelectionStart - 1) & fTecladoEnPantalla.TextBox1.Text.Substring(fTecladoEnPantalla.TextBox1.SelectionStart)
                ftecladoenpantalla.TextBox1.SelectionStart = Len(ftecladoenpantalla.TextBox1.Text)
                ftecladoenpantalla.TextBox1.Focus()
            End If
        End If
        Verificar(ftecladoenpantalla.TextBox1)
        ftecladoenpantalla.Timer1.Enabled = False
        ftecladoenpantalla.Timer1.Enabled = True

    End Sub

    Private Sub botonok(ByVal sender As Object, ByVal e As System.EventArgs)





        If Len(ftecladoenpantalla.TextBox1.Text) > iLongitudTexto Then
            Verificar(ftecladoenpantalla.TextBox1)
            ftecladoenpantalla.Timer1.Enabled = False
            ftecladoenpantalla.Timer1.Enabled = True

        Else
            ftecladoenpantalla.Visible = False
            Me.Text = ftecladoenpantalla.TextBox1.Text
            ftecladoenpantalla.TextBox1.Text = ""
            ftecladoenpantalla.Timer1.Enabled = False

            If Not IsNothing(mMasterk) Then
                If Len(mMasterk.EstablecerCadena(sDireccionEscritura, Me.Text)) Then

                End If
            End If

        End If



    End Sub

    Private Sub botoncancel(ByVal sender As Object, ByVal e As System.EventArgs)
        fTecladoEnPantalla.Visible = False
        fTecladoEnPantalla.TextBox1.Text = ""
        fTecladoEnPantalla.Timer1.Enabled = False
    End Sub

    Private Sub touchscreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click
        If CType(e, MouseEventArgs).X < Me.Width - SystemInformation.VerticalScrollBarWidth Then

            fTecladoEnPantalla.TextBox1.Text = Me.Text

            fTecladoEnPantalla.Visible = True
            fTecladoEnPantalla.TextBox1.Focus()
            fTecladoEnPantalla.TextBox1.SelectAll()

            fTecladoEnPantalla.Top = Screen.PrimaryScreen.WorkingArea.Height - fTecladoEnPantalla.Height
            fTecladoEnPantalla.Left = Screen.PrimaryScreen.WorkingArea.Width - fTecladoEnPantalla.Width

            Verificar(fTecladoEnPantalla.TextBox1)
            fTecladoEnPantalla.Timer1.Enabled = True
        End If
    End Sub

    Property Link() As MasterKlib.MasterK
        Get
            Return mMasterk
        End Get
        Set(ByVal value As MasterKlib.MasterK)
            If Not IsNothing(value) Then
                mMasterk = value
                sId = mMasterk.Agregar(Me)
            End If
        End Set
    End Property
    Property LongitudTexto() As Integer
        Get
            Return iLongitudTexto
        End Get
        Set(ByVal value As Integer)
            iLongitudTexto = value
        End Set
    End Property
    Property DireccionLectura() As String
        Get
            Return sDireccionLectura
        End Get
        Set(ByVal value As String)
            sDireccionLectura = value
        End Set
    End Property
    Property DireccionEscritura() As String
        Get
            Return sDireccionEscritura
        End Get
        Set(ByVal value As String)
            sDireccionEscritura = value
        End Set
    End Property


    Property Etiqueta() As String
        Get
            Return sEtiqueta
        End Get
        Set(ByVal value As String)
            sEtiqueta = value
            CrearEtiqueta()
            EmparentarEtiqueta()
            lEtiqueta.Text = sEtiqueta

        End Set
    End Property

    Property Texto() As String
        Get
            If Not bAutoActualizar Then
                Actualizar()
            End If
            Return sTexto
        End Get
        Set(ByVal value As String)
            If Not IsNothing(mMasterk) Then
                mMasterk.EstablecerCadena(sDireccionEscritura, value)

            End If
            sTexto = value
        End Set
    End Property
    Private Sub CrearEtiqueta()
        If IsNothing(lEtiqueta) Then
            lEtiqueta = New Label
            lEtiqueta.Cursor = Cursors.Cross
            lEtiqueta.Font = New Font(sNombreFuente, iTamanioFuente)
            lEtiqueta.TextAlign = ContentAlignment.MiddleCenter
            lEtiqueta.Text = sEtiqueta
            lEtiqueta.Height = Me.Height
            lEtiqueta.Width = 198
            lEtiqueta.BackColor = cEtiquetaBackcolor
            lEtiqueta.ForeColor = cEtiquetaForecolor
            lEtiqueta.Top = Me.Top
            lEtiqueta.Left = Me.Left - 200
            lEtiqueta.Visible = True
        End If
    End Sub
    Private Sub EmparentarEtiqueta()
        If IsNothing(lEtiqueta.Parent) Then
            If Not IsNothing(Me.Parent) Then
                Me.Parent.Controls.Add(lEtiqueta)
            End If
        End If
    End Sub
    Property AutoActualizar() As Boolean
        Get
            Return bAutoActualizar
        End Get
        Set(ByVal value As Boolean)
            bAutoActualizar = value

        End Set
    End Property
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Me.Font = New Font(sNombreFuente, iTamanioFuente)
    End Sub
    Protected Overrides Sub OnParentChanged(ByVal e As System.EventArgs)
        MyBase.OnParentChanged(e)
        CargarOpciones()
        crearetiqueta()
        emparentaretiqueta()


        Me.Font = New Font(sNombreFuente, iTamanioFuente)
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)

        If disposing Then
            If Not IsNothing(fTecladoEnPantalla) Then
                fTecladoEnPantalla.Dispose()
                fTecladoEnPantalla = Nothing
            End If
            If Not IsNothing(lEtiqueta) Then
                If Not IsNothing(Me.Parent) Then
                    Me.Parent.Controls.Remove(lEtiqueta)
                End If
                lEtiqueta = Nothing
            End If

            If Not IsNothing(mMasterk) Then
                mMasterk.Quitar(sId)
            End If

        End If
        MyBase.Dispose(disposing)
    End Sub

    Protected Overrides Sub OnMove(ByVal e As System.EventArgs)
        MyBase.OnMove(e)
        If IsNothing(lEtiqueta) Then
            CrearEtiqueta()
        Else
            lEtiqueta.Top = Me.Top
            lEtiqueta.Left = Me.Left - 200
        End If
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        If IsNothing(lEtiqueta) Then
            CrearEtiqueta()
        Else
            lEtiqueta.Height = Me.Height
        End If
    End Sub
    Sub Actualizar()
        If Not IsNothing(mMasterk) Then
            sTexto = mMasterk.ObtenerCadena(sDireccionLectura, iLongitudTexto)
        End If
        Me.Text = sTexto
    End Sub



    Private Sub hhCharacterEntry_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.TextChanged
        Verificar(Me)
    End Sub

End Class
