Imports System.IO
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing
Public Class hhNumericEntry
    Inherits TextBox
    Dim sId As String
    Dim iAnchoBoton As Integer
    Dim iAltoBoton As Integer
    Dim iValorMaximo As Integer
    Dim iValorMinimo As Integer
    Dim iValor As Integer
    Dim iAutoOcultar As Integer
    Dim cColorAlerta As Color
    Dim cColorNormal As Color
    Dim fFuente As Font
    Dim fFuenteEtiqueta As Font
    Dim sUnidades As String
    Dim sDireccionLectura As String
    Dim sDireccionEscritura As String
    Dim sFactor As Single
    Dim iDecimales As Integer
    Dim sTooltip As String
    Dim tHint As ToolTip
    Dim iAltoRenglonTooltip As Integer
    Dim mMasterk As MasterKlib.MasterK
    Dim bAutoActualizar As Boolean
    <Runtime.InteropServices.DllImport("user32")> Private Shared Function HideCaret(ByVal hWnd As IntPtr) As Integer
    End Function
    Sub New()
        MyBase.New()
        CargarOpciones()
        Me.AutoSize = False
        Me.TextAlign = HorizontalAlignment.Center
        Me.Cursor = Cursors.Cross
        Me.BorderStyle = BorderStyle.FixedSingle
        Me.Font = fFuente
    End Sub
    Private Sub CargarOpciones()
        Try
            fFuente = New Font(GetSetting("hhControls", "Font", "FontName", "Verdana"), Val(GetSetting("hhControls", "Font", "FontSize", "18")))
        Catch ex As Exception
            fFuente = New Font("Verdana", 18)
        End Try
        Try
            fFuenteEtiqueta = New Font(GetSetting("hhControls", "Font", "LabelFontName", "Verdana"), Val(GetSetting("hhControls", "Font", "LabelFontSize", "14")))
        Catch ex As Exception
            fFuenteEtiqueta = New Font("Verdana", 14)
        End Try
        cColorAlerta = Color.FromArgb(GetSetting("hhControls", "Colors", "AlertBackColor", System.Drawing.Color.Red.ToArgb.ToString))
        cColorNormal = Color.FromArgb(GetSetting("hhControls", "Colors", "NormalBackColor", System.Drawing.SystemColors.Window.ToArgb.ToString))
        iAltoBoton = Val(GetSetting("hhControls", "Size", "ButtonHeight", "70"))
        iAnchoBoton = Val(GetSetting("hhControls", "Size", "ButtonWidth", "70"))
        iAutoOcultar = Val(GetSetting("hhControls", "Refresh", "AutoHide", "10000"))
    End Sub
    Public Overrides Property Font() As System.Drawing.Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As System.Drawing.Font)
            Try
                MyBase.Font = fFuente
            Catch ex As Exception
                MyBase.Font = value
            End Try
        End Set
    End Property
    Private Sub Verificar()
        If EnRango(iValor, iValorMaximo, iValorMinimo) Then
            Me.BackColor = cColorNormal
        Else
            Me.BackColor = cColorAlerta
        End If

    End Sub
    Private Function EnRango(ByVal Valor As Integer, ByVal ValorMaximo As Integer, ByVal ValorMinimo As Integer) As Boolean
        Return ((Valor <= ValorMaximo) And (Valor >= ValorMinimo))
    End Function
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
            iValor = value
            If Not IsNothing(mMasterk) Then
                mMasterk.EstablecerEntero(sDireccionEscritura, iValor)
            End If
            Actualizar()
        End Set
    End Property
    Property Factor() As Single
        Get
            Return sFactor
        End Get
        Set(ByVal value As Single)
            sFactor = value
            Actualizar()
        End Set
    End Property
    Property Decimales() As Integer
        Get
            Return iDecimales
        End Get
        Set(ByVal value As Integer)
            iDecimales = value
            Actualizar()
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
                        If TextRenderer.MeasureText(sColumna, ffuenteetiqueta).Width > e.Bounds.Width / 2 Then
                            Do
                                If Len(sColumna) < 3 Then Exit Do
                                sColumna = sColumna.Substring(0, sColumna.Length - 1)
                            Loop Until TextRenderer.MeasureText(sColumna & "...", ffuenteetiqueta).Width < e.Bounds.Width / 2
                            sColumna = sColumna & "..."
                        End If
                        e.Graphics.DrawString(sColumna, ffuenteetiqueta, SystemBrushes.ActiveCaptionText, iColumna * (e.Bounds.Width / 2), iRenglon * iAltoRenglonTooltip)
                        iColumna = iColumna + 1
                    Next sColumna
                Else
                    If TextRenderer.MeasureText(sRenglon, ffuenteetiqueta).Width > e.Bounds.Width Then
                        Do
                            If Len(sRenglon) < 3 Then Exit Do
                            sRenglon = sRenglon.Substring(0, sRenglon.Length - 1)
                        Loop Until TextRenderer.MeasureText(sRenglon & "...", ffuenteetiqueta).Width < e.Bounds.Width
                        sRenglon = sRenglon & "..."
                    End If
                    e.Graphics.DrawString(sRenglon, ffuenteetiqueta, SystemBrushes.ActiveCaptionText, 0, iRenglon * iAltoRenglonTooltip)
                End If
                iRenglon = iRenglon + 1
            Next sRenglon
        Finally
        End Try
    End Sub
    Private Sub Popup(ByVal sender As Object, ByVal e As System.Windows.Forms.PopupEventArgs)
        iAltoRenglonTooltip = TextRenderer.MeasureText("Receta", ffuenteetiqueta).Height
        e.ToolTipSize = New System.Drawing.Size(Me.Width, iAltoRenglonTooltip * 5)
    End Sub

    Property AutoActualizar() As Boolean
        Get
            Return bAutoActualizar
        End Get
        Set(ByVal value As Boolean)
            bAutoActualizar = value
        End Set
    End Property

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Dim iAnchoUnidades As Integer
        Dim iAnchoTexto As Integer
        Dim sUnidadesAjuste As String
        MyBase.WndProc(m)
        If m.Msg = &HF Then
            If Len(sUnidades) Then
                sUnidadesAjuste = sUnidades
                Using g As Graphics = Me.CreateGraphics
                    iAnchoUnidades = g.MeasureString(sUnidadesAjuste, fFuenteEtiqueta).Width
                    iAnchoTexto = g.MeasureString(Me.Text, fFuenteEtiqueta).Width
                    While iAnchoUnidades >= ((Me.Width / 2) - (iAnchoTexto / 2))
                        sUnidadesAjuste = sUnidadesAjuste.Replace(Chr(26), "")
                        If sUnidadesAjuste.Length <= 1 Then
                            Exit Sub
                        End If
                        sUnidadesAjuste = sUnidadesAjuste.Substring(0, sUnidadesAjuste.Length - 1)
                        sUnidadesAjuste = sUnidadesAjuste & Chr(26)
                        iAnchoUnidades = g.MeasureString(sUnidadesAjuste, fFuenteEtiqueta).Width
                    End While
                    If iAnchoUnidades < ((Me.Width / 2) - (iAnchoTexto / 2)) Then
                        g.DrawString(sUnidadesAjuste, fFuenteEtiqueta, New SolidBrush(System.Drawing.SystemColors.GrayText), Me.Width - iAnchoUnidades, 0)
                    End If
                End Using
            End If
        End If
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not IsNothing(tHint) Then
                tHint.Dispose()
            End If
            If Not IsNothing(mMasterk) Then
                mMasterk.Quitar(sId)
            End If
        End If
        MyBase.Dispose(disposing)
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

    Private Sub hhNumericEntry_Click(sender As Object, e As EventArgs) Handles Me.Click
        Dim b As Control
        Using fTecladoEnPantalla As New Form1
            fTecladoEnPantalla.TextBox1.Font = fFuente
            For Each b In fTecladoEnPantalla.TableLayoutPanel1.Controls
                If TypeOf (b) Is Button Then
                    b.Font = fFuente
                End If
            Next
            fTecladoEnPantalla.iValorMinimo = iValorMinimo
            fTecladoEnPantalla.iValorMaximo = iValorMaximo
            fTecladoEnPantalla.iAltoBoton = iAltoBoton
            fTecladoEnPantalla.iAnchoBoton = iAnchoBoton
            fTecladoEnPantalla.cColorAlerta = cColorAlerta
            fTecladoEnPantalla.cColorNormal = cColorNormal
            fTecladoEnPantalla.sUnidades = sUnidades
            fTecladoEnPantalla.sFactor = sFactor
            fTecladoEnPantalla.iDecimales = iDecimales
            fTecladoEnPantalla.Timer1.Interval = iAutoOcultar
            fTecladoEnPantalla.iValor = iValor
            If fTecladoEnPantalla.ShowDialog() = DialogResult.OK Then
                iValor = fTecladoEnPantalla.iValor
                If Not IsNothing(mMasterk) Then
                    mMasterk.EstablecerEntero(sDireccionEscritura, iValor)
                End If
                Actualizar()
            End If
        End Using
        Verificar()
    End Sub
End Class
