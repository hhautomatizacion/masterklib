Imports System.IO
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing
Public Class hhNumericEntry
    Inherits TextBox
    Dim sId As String
    Dim fTecladoEnPantalla As Form1
    Dim iAnchoBoton As Integer
    Dim iAltoBoton As Integer
    Dim sNombreFuente As String
    Dim iTamanioFuente As Integer
    Dim iValorMaximo As Integer
    Dim iValorMinimo As Integer
    Dim iValor As Integer
    Dim iValorEdicion As Integer
    Dim iAutoOcultar As Integer
    Dim cColorAlerta As Color
    Dim cColorNormal As Color
    Dim cEtiquetaBackcolor As Color
    Dim cEtiquetaForecolor As Color
    Dim sUnidades As String
    Dim sEtiqueta As String
    Dim lEtiqueta As Label
    Dim sDireccionLectura As String
    Dim sDireccionEscritura As String
    Dim sFactor As Single
    Dim iDecimales As Integer
    Dim sTooltip As String
    Dim sNumeros() As String = New String() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "-", "0", "+"}
    Dim tHint As ToolTip
    Dim iAltoRenglonTooltip As Integer
    Dim mMasterk As MasterKlib.MasterK
    Dim bAutoActualizar As Boolean
    Dim bBackupAutoActualizar As Boolean
    Dim WithEvents tTeclado As Timer
    <Runtime.InteropServices.DllImport("user32")> Private Shared Function HideCaret(ByVal hWnd As IntPtr) As Integer
    End Function
    Sub New()
        MyBase.New()
        Dim b As Button
        CargarOpciones()
        tTeclado = New Timer
        Me.AutoSize = False
        Me.Font = New Font(sNombreFuente, iTamanioFuente)
        Me.TextAlign = HorizontalAlignment.Center
        Me.Cursor = Cursors.Cross
        CrearEtiqueta()
        If Me.DesignMode Then
            EmparentarEtiqueta()
        End If
        fTecladoEnPantalla = New Form1
        fTecladoEnPantalla.TextBox1.Font = New Font(sNombreFuente, iTamanioFuente)
        fTecladoEnPantalla.Height = iAltoBoton * 4 + fTecladoEnPantalla.TextBox1.Height
        fTecladoEnPantalla.Width = iAnchoBoton * 4
        fTecladoEnPantalla.Top = Screen.PrimaryScreen.WorkingArea.Height - fTecladoEnPantalla.Height
        fTecladoEnPantalla.Left = Screen.PrimaryScreen.WorkingArea.Width - fTecladoEnPantalla.Width
        fTecladoEnPantalla.Timer1.Interval = iAutoOcultar
        fTecladoEnPantalla.Timer1.Enabled = False
        tTeclado.Interval = iAutoOcultar
        tTeclado.Enabled = False
        For i As Integer = 0 To sNumeros.Length - 1
            b = New Button
            b.Font = New Font(sNombreFuente, iTamanioFuente)
            b.Text = sNumeros(i)
            b.Top = fTecladoEnPantalla.TextBox1.Height + (i \ 3) * iAltoBoton
            b.Left = (i Mod 3) * iAnchoBoton
            b.Width = iAnchoBoton
            b.Height = iAltoBoton
            AddHandler b.MouseDown, AddressOf presiona
            AddHandler b.MouseUp, AddressOf levanta
            fTecladoEnPantalla.Controls.Add(b)
        Next
        b = New Button
        With b
            .Font = New Font(sNombreFuente, iTamanioFuente)
            .Width = iAnchoBoton
            .Height = iAltoBoton * 2
            .Top = fTecladoEnPantalla.Height - .Height
            .Left = fTecladoEnPantalla.Width - .Width
            .Text = ""
            .Image = My.Resources.circle_with_check_symbol
            AddHandler .MouseUp, AddressOf botonok
        End With
        fTecladoEnPantalla.Controls.Add(b)
        b = New Button
        With b
            .Font = New Font(sNombreFuente, iTamanioFuente)
            .Width = iAnchoBoton
            .Height = iAltoBoton
            .Top = fTecladoEnPantalla.Height - 3 * .Height
            .Left = fTecladoEnPantalla.Width - .Width
            .Text = ""
            .Image = My.Resources.backspace_arrow
            AddHandler .MouseUp, AddressOf botonbackspace
        End With
        fTecladoEnPantalla.Controls.Add(b)
        b = New Button
        With b
            .Font = New Font(sNombreFuente, iTamanioFuente)
            .Width = iAnchoBoton
            .Height = iAltoBoton
            .Top = fTecladoEnPantalla.Height - 4 * .Height
            .Left = fTecladoEnPantalla.Width - .Width
            .Text = ""
            .Image = My.Resources.cancel_button
            AddHandler .MouseUp, AddressOf botoncancel
        End With
        fTecladoEnPantalla.Controls.Add(b)
        fTecladoEnPantalla.Visible = False
    End Sub
    Private Sub CargarOpciones()
        cEtiquetaBackcolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelBackColor", System.Drawing.SystemColors.Highlight.ToArgb.ToString))
        cEtiquetaForecolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelForeColor", System.Drawing.SystemColors.HighlightText.ToArgb.ToString))
        cColorAlerta = Color.FromArgb(GetSetting("hhControls", "Colors", "AlertBackColor", System.Drawing.Color.Red.ToArgb.ToString))
        cColorNormal = Color.FromArgb(GetSetting("hhControls", "Colors", "NormalBackColor", System.Drawing.SystemColors.Window.ToArgb.ToString))
        iAltoBoton = Val(GetSetting("hhcontrols", "size", "buttonheight", "70"))
        iAnchoBoton = Val(GetSetting("hhcontrols", "size", "buttonwidth", "70"))
        iAutoOcultar = Val(GetSetting("hhcontrols", "refresh", "autohide", "10000"))
        iTamanioFuente = Val(GetSetting("hhControls", "Font", "Size", "14"))
        sNombreFuente = GetSetting("hhControls", "Font", "Name", "Verdana")
    End Sub
    Private Sub Verificar()
        If EnRango(iValor, iValorMaximo, iValorMinimo) Then
            Me.BackColor = cColorNormal
        Else
            Me.BackColor = cColorAlerta
        End If
    End Sub
    Private Sub levanta(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If EnRango(iValorEdicion, iValorMaximo, iValorMinimo) Then
            fTecladoEnPantalla.TextBox1.BackColor = cColorNormal
        Else
            fTecladoEnPantalla.TextBox1.BackColor = cColorAlerta
        End If
    End Sub
    Private Sub presiona(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        'Ya no se ocupa el valor del textbox, sino que se edita iValorEdicion.
        If fTecladoEnPantalla.TextBox1.SelectedText.Length Then
            iValorEdicion = Val(sender.text)
        Else
            iValorEdicion = iValorEdicion * 10 + Val(sender.text)
        End If
        fTecladoEnPantalla.TextBox1.Text = DarFormato(iValorEdicion)
        fTecladoEnPantalla.Timer1.Enabled = False
        fTecladoEnPantalla.Timer1.Enabled = True
    End Sub
    Private Sub botonbackspace(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        'Ya no se ocupa el valor del textbox, sino que se edita una iValorEdicion.
        If fTecladoEnPantalla.TextBox1.SelectedText.Length Then
            iValorEdicion = 0
        Else
            iValorEdicion = iValorEdicion \ 10
        End If
        fTecladoEnPantalla.TextBox1.Text = DarFormato(iValorEdicion)
        If EnRango(iValorEdicion, iValorMaximo, iValorMinimo) Then
            fTecladoEnPantalla.TextBox1.BackColor = cColorNormal
        Else
            fTecladoEnPantalla.TextBox1.BackColor = cColorAlerta
        End If
        fTecladoEnPantalla.Timer1.Enabled = False
        fTecladoEnPantalla.Timer1.Enabled = True
    End Sub
    Private Function EnRango(ByVal Valor As Integer, ByVal ValorMaximo As Integer, ByVal ValorMinimo As Integer) As Boolean
        Return ((Valor <= ValorMaximo) And (Valor >= ValorMinimo))
    End Function
    Private Sub botoncancel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        fTecladoEnPantalla.Visible = False
        fTecladoEnPantalla.TextBox1.Text = ""
        fTecladoEnPantalla.Timer1.Enabled = False
    End Sub
    Private Sub botonok(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If EnRango(iValorEdicion, iValorMaximo, iValorMinimo) Then
            fTecladoEnPantalla.Visible = False
            Me.Text = DarFormato(iValorEdicion)
            fTecladoEnPantalla.TextBox1.Text = ""
            fTecladoEnPantalla.Timer1.Enabled = False
            iValor = iValorEdicion
            If Not IsNothing(mMasterk) Then
                mMasterk.EstablecerEntero(sDireccionEscritura, iValor)
            End If
        Else
            fTecladoEnPantalla.Timer1.Enabled = False
            fTecladoEnPantalla.Timer1.Enabled = True
        End If
    End Sub
    Private Sub touchscreen_focus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        HideCaret(Me.Handle)
    End Sub
    Private Sub touchscreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click
        'Ya no se ocupa el valor del textbox sino que se edita iValorEdicion
        iValorEdicion = iValor
        fTecladoEnPantalla.TextBox1.Text = DarFormato(iValorEdicion)
        fTecladoEnPantalla.Visible = True
        fTecladoEnPantalla.TextBox1.Focus()
        fTecladoEnPantalla.TextBox1.SelectAll()
        If EnRango(iValorEdicion, iValorMaximo, iValorMinimo) Then
            fTecladoEnPantalla.TextBox1.BackColor = cColorNormal
        Else
            fTecladoEnPantalla.TextBox1.BackColor = cColorAlerta
        End If
        fTecladoEnPantalla.Top = Screen.PrimaryScreen.WorkingArea.Height - fTecladoEnPantalla.Height
        fTecladoEnPantalla.Left = Screen.PrimaryScreen.WorkingArea.Width - fTecladoEnPantalla.Width
        fTecladoEnPantalla.Timer1.Enabled = True
    End Sub
    Property ValorMinimo() As Integer
        Get
            Return iValorMinimo
        End Get
        Set(ByVal value As Integer)
            iValorMinimo = value
        End Set
    End Property
    Property ValorMaximo() As Integer
        Get
            Return iValorMaximo
        End Get
        Set(ByVal value As Integer)
            iValorMaximo = value
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
    Property Unidades() As String
        Get
            Return sUnidades
        End Get
        Set(ByVal value As String)
            sUnidades = value
        End Set
    End Property
    Property Valor() As Integer
        Get
            If Not bAutoActualizar Then
                Actualizar()
            End If
            Return iValor
        End Get
        Set(ByVal value As Integer)
            If Not IsNothing(mMasterk) Then
                mMasterk.EstablecerEntero(sDireccionEscritura, value)
            End If
            iValor = value
        End Set
    End Property
    Property Factor() As Single
        Get
            Return sFactor
        End Get
        Set(ByVal value As Single)
            sFactor = value
        End Set
    End Property
    Property Decimales() As Integer
        Get
            Return iDecimales
        End Get
        Set(ByVal value As Integer)
            iDecimales = value
        End Set
    End Property
    Property Tooltip() As String
        Get
            Return sTooltip
        End Get
        Set(ByVal value As String)
            If Not isnothing(value) Then
                sTooltip = value
                tHint = New ToolTip
                tHint.Active = True
                tHint.AutomaticDelay = 1000
                tHint.AutoPopDelay = 5000
                tHint.OwnerDraw = True
                tHint.SetToolTip(lEtiqueta, sTooltip)
                tHint.SetToolTip(Me, sTooltip)
                AddHandler tHint.Draw, AddressOf Draw
                AddHandler tHint.Popup, AddressOf Popup
            End If
        End Set
    End Property
    Private Sub Draw(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawToolTipEventArgs)
        Dim sRenglon As String
        Dim sColumna As String
        Dim sTempTooltip As String
        Dim iRenglon As Integer
        Dim iColumna As Integer
        sTempTooltip = sTooltip & "|Maximo:" & DarFormato(iValorMaximo) & "|Minimo:" & DarFormato(iValorMinimo)
        Try
            e.Graphics.FillRectangle(SystemBrushes.ActiveCaption, e.Bounds)
            iRenglon = 0
            For Each sRenglon In Split(sTempTooltip, "|")
                If InStr(sRenglon, ":") Then
                    iColumna = 0
                    For Each sColumna In Split(sRenglon, ":")
                        If TextRenderer.MeasureText(sColumna, New Font(sNombreFuente, iTamanioFuente)).Width > e.Bounds.Width / 2 Then
                            Do
                                If Len(sColumna) < 3 Then Exit Do
                                sColumna = sColumna.Substring(0, sColumna.Length - 1)
                            Loop Until TextRenderer.MeasureText(sColumna & "...", New Font(sNombreFuente, iTamanioFuente)).Width < e.Bounds.Width / 2
                            sColumna = sColumna & "..."
                        End If
                        e.Graphics.DrawString(sColumna, New Font(sNombreFuente, iTamanioFuente), SystemBrushes.ActiveCaptionText, iColumna * (e.Bounds.Width / 2), iRenglon * iAltoRenglonTooltip)
                        iColumna = iColumna + 1
                    Next sColumna
                Else
                    If TextRenderer.MeasureText(sRenglon, New Font(sNombreFuente, iTamanioFuente)).Width > e.Bounds.Width Then
                        Do
                            If Len(sRenglon) < 3 Then Exit Do
                            sRenglon = sRenglon.Substring(0, sRenglon.Length - 1)
                        Loop Until TextRenderer.MeasureText(sRenglon & "...", New Font(sNombreFuente, iTamanioFuente)).Width < e.Bounds.Width
                        sRenglon = sRenglon & "..."
                    End If
                    e.Graphics.DrawString(sRenglon, New Font(sNombreFuente, iTamanioFuente), SystemBrushes.ActiveCaptionText, 0, iRenglon * iAltoRenglonTooltip)
                End If
                iRenglon = iRenglon + 1
            Next sRenglon
        Finally
        End Try
    End Sub
    Private Sub Popup(ByVal sender As Object, ByVal e As System.Windows.Forms.PopupEventArgs)
        ialtorenglontooltip = TextRenderer.MeasureText("Receta", New Font(sNombreFuente, iTamanioFuente)).Height
        e.ToolTipSize = New System.Drawing.Size(lEtiqueta.Width, iAltoRenglonTooltip * 5)

    End Sub
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
            AddHandler lEtiqueta.Click, AddressOf MostrarTooltip
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
        CrearEtiqueta()

        EmparentarEtiqueta()

        Me.Font = New Font(sNombreFuente, iTamanioFuente)
    End Sub
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Dim iAnchoUnidades As Integer
        Dim iAnchoTexto As Integer
        Dim sUnidadesAjuste As String
        MyBase.WndProc(m)
        If m.Msg = &HF Then
            If Len(sUnidades) Then
                sUnidadesAjuste = sUnidades
                Using g As Graphics = Me.CreateGraphics
                    iAnchoUnidades = g.MeasureString(sUnidadesAjuste, Me.Font).Width
                    iAnchoTexto = g.MeasureString(Me.Text, Me.Font).Width
                    While iAnchoUnidades >= ((Me.Width / 2) - (iAnchoTexto / 2))
                        sUnidadesAjuste = sUnidadesAjuste.Replace(Chr(26), "")
                        If sUnidadesAjuste.Length <= 1 Then
                            Exit Sub
                        End If
                        sUnidadesAjuste = sUnidadesAjuste.Substring(0, sUnidadesAjuste.Length - 1)
                        sUnidadesAjuste = sUnidadesAjuste & Chr(26)
                        iAnchoUnidades = g.MeasureString(sUnidadesAjuste, Me.Font).Width

                    End While
                    If iAnchoUnidades < ((Me.Width / 2) - (iAnchoTexto / 2)) Then
                        g.DrawString(sUnidadesAjuste, Me.Font, New SolidBrush(System.Drawing.SystemColors.GrayText), Me.Width - iAnchoUnidades, 0)
                    End If
                End Using
            End If
        End If

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
            If Not IsNothing(tHint) Then
                tHint.Dispose()
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
            iValor = mMasterk.ObtenerEntero(sDireccionLectura)
        End If
        Me.Text = DarFormato(iValor)
        Verificar()
    End Sub
    Private Function DarFormato(ByVal i As Integer) As String
        Dim sFormato As String
        If sFactor = 0 Then sFactor = 1
        If sFactor = 1 Then
            sFormato = "0"
        Else
            If iDecimales > 0 Then
                sFormato = "0." & StrDup(iDecimales, "0")
            Else
                sFormato = "0\."
            End If
        End If
        Return (i * sFactor).ToString(sFormato)
    End Function
    Private Sub hhNumericEntry_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.TextChanged
        Verificar()
    End Sub
    Private Sub MostrarTooltip(ByVal s As Object, ByVal e As System.EventArgs)
        If Not IsNothing(tHint) Then
            Try
                tHint.Show(sTooltip, Me, 5000)
            Catch
            End Try
        End If
    End Sub
    Private Sub tTeclado_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tTeclado.Tick
        bAutoActualizar = bBackupAutoActualizar
        tTeclado.Enabled = False
    End Sub
End Class
