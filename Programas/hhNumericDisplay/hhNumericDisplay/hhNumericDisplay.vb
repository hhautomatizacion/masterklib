Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing
Public Class hhNumericDisplay
    Inherits System.Windows.Forms.Label
    Dim sId As String
    Dim bAlerta As Boolean
    Dim bAutoActualizar As Boolean
    Dim cColorAlerta As Color
    Dim cColorNormal As Color
    Dim cColorAlertaTexto As Color
    Dim cColorNormalTexto As Color
    Dim fFuente As Font
    Dim fFuenteEtiqueta As Font
    Dim iIntervaloAlerta As Integer
    Dim iValor As Integer
    Dim iValorMaximo As Integer
    Dim iValorMinimo As Integer
    Dim mMasterk As MasterKlib.MasterK
    Dim sDireccionLectura As String
    Dim sTooltip As String
    Dim iAltoRenglonTooltip As Integer
    Dim iAnchoTooltip As Integer
    Dim tHint As ToolTip
    Dim WithEvents tAlerta As Timer
    Sub New()
        MyBase.New()
        CargarOpciones()
        Me.Font = fFuente
        Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        Me.TextAlign = ContentAlignment.MiddleCenter
        Me.Cursor = Cursors.Cross
        Me.Text = ""
        If Not Me.DesignMode Then
            CrearTemporizadores()
        End If
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
        cColorAlertaTexto = Color.FromArgb(GetSetting("hhControls", "Colors", "AlertTextColor", System.Drawing.Color.Black.ToArgb.ToString))
        cColorNormalTexto = Color.FromArgb(GetSetting("hhControls", "Colors", "NormalTextColor", System.Drawing.SystemColors.WindowText.ToArgb.ToString))
        iIntervaloAlerta = Val(GetSetting("hhcontrols", "refresh", "alertinterval", "1000"))
        iAnchoTooltip = Val(GetSetting("hhControls", "Tooltip", "TooltipWidth", "200"))
        GuardarOpciones()
    End Sub
    Private Sub GuardarOpciones()
        SaveSetting("hhControls", "Font", "FontName", fFuente.Name)
        SaveSetting("hhControls", "Font", "FontSize", fFuente.Size.ToString)
        SaveSetting("hhControls", "Font", "LabelFontName", fFuenteEtiqueta.Name)
        SaveSetting("hhControls", "Font", "LabelFontSize", fFuenteEtiqueta.Size.ToString)
        SaveSetting("hhControls", "Colors", "AlertBackColor", cColorAlerta.ToArgb.ToString)
        SaveSetting("hhControls", "Colors", "NormalBackColor", cColorNormal.ToArgb.ToString)
        SaveSetting("hhControls", "Colors", "AlertTextColor", cColorAlertatexto.ToArgb.ToString)
        SaveSetting("hhControls", "Colors", "NormalTextColor", cColorNormaltexto.ToArgb.ToString)
        SaveSetting("hhControls", "Refresh", "AlertInterval", iIntervaloAlerta.ToString)
        SaveSetting("hhControls", "Tooltip", "TooltipWidth", iAnchoTooltip.ToString)
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

    Private Sub CrearTemporizadores()
        tAlerta = New Timer
        tAlerta.Interval = ForzarRango(iIntervaloAlerta, 100, 60000)
        tAlerta.Enabled = False
    End Sub
    Private Function ForzarRango(ByVal Valor As Integer, ByVal ValorMinimo As Integer, ByVal ValorMaximo As Integer) As Integer
        If Valor < ValorMinimo Or Valor > ValorMaximo Then
            Valor = ValorMinimo
        End If
        Return Valor
    End Function
    Property ValorMinimo() As Integer
        Get
            Return iValorMinimo
        End Get
        Set(ByVal value As Integer)
            iValorMinimo = value
        End Set
    End Property
    <System.ComponentModel.DefaultValue(Integer.MaxValue)> Property ValorMaximo() As Integer
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
    Property Link() As MasterKlib.MasterK
        Get
            Return mMasterk
        End Get
        Set(ByVal value As MasterKlib.MasterK)
            If Not isnothing(value) Then
                mMasterk = value
                sId = mMasterk.Agregar(Me)
            End If
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
            DarFormato()
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
                tHint.AutomaticDelay = 0
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
        sTempTooltip = sTooltip & "|Maximo:" & iValorMaximo & "|Minimo:" & iValorMinimo
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
        e.ToolTipSize = New System.Drawing.Size(ianchotooltip, iAltoRenglonTooltip * 5)
    End Sub

    Property AutoActualizar() As Boolean
        Get
            Return bAutoActualizar
        End Get
        Set(ByVal value As Boolean)
            bAutoActualizar = value
        End Set
    End Property

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not IsNothing(tAlerta) Then
                tAlerta.Enabled = False
                tAlerta = Nothing
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

    Sub Actualizar()
        If Not IsNothing(mMasterk) Then
            iValor = mMasterk.ObtenerEntero(sDireccionLectura)
        End If
        DarFormato()
    End Sub
    Private Sub DarFormato()
        MyBase.Text = iValor.ToString
        If Not IsNothing(tAlerta) Then
            If EnRango(iValor, iValorMaximo, iValorMinimo) Then
                tAlerta.Enabled = False
                MyBase.ForeColor = ccolornormaltexto
                MyBase.BackColor = cColorNormal
            Else
                tAlerta.Enabled = True
            End If
        End If
    End Sub
    Private Function EnRango(ByVal Valor As Integer, ByVal ValorMaximo As Integer, ByVal ValorMinimo As Integer) As Boolean
        Return ((Valor <= ValorMaximo) And (Valor >= ValorMinimo))
    End Function
    Private Sub Alerta(ByVal s As Object, ByVal e As System.EventArgs) Handles tAlerta.Tick
        bAlerta = Not bAlerta
        If bAlerta Then
            MyBase.ForeColor = cColorAlertatexto
            MyBase.BackColor = cColorAlerta
        Else
            MyBase.ForeColor = cColorNormaltexto
            MyBase.BackColor = cColorNormal
        End If
    End Sub

    Private Sub MostrarTooltip(ByVal s As Object, ByVal e As System.EventArgs)
        If Not IsNothing(tHint) Then
            Try
                tHint.Show(sTooltip, Me, 5000)
            Catch
            End Try
        End If
    End Sub
    Private Sub hhNumericDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click
        If Not IsNothing(tHint) Then
            Try
                tHint.Show(sTooltip, Me, 5000)
            Catch
            End Try
        End If
    End Sub
End Class
