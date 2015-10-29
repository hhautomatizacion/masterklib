Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing
Public Class hhTimeCounterDisplay
    Inherits System.Windows.Forms.Label
    Dim sId As String
    Dim bAutoActualizar As Boolean
    Dim bAlerta As Boolean
    Dim cColorAlerta As Color
    Dim cColorNormal As Color
    Dim cEtiquetaBackcolor As Color
    Dim cEtiquetaForecolor As Color
    Dim iIntervaloAlerta As Integer
    Dim iTamanioFuente As Integer
    Dim iValor As Integer
    Dim iValorMaximo As Integer
    Dim lEtiqueta As Label
    Dim iAltoRenglonTooltip As Integer
    Dim sDireccionLectura As String
    Dim sEtiqueta As String
    Dim sNombreFuente As String
    Dim sTooltip As String
    Dim mMasterk As MasterKlib.MasterK
    Dim tHint As ToolTip
    Dim WithEvents tAlerta As Timer
    Sub New()
        MyBase.New()
        CargarOpciones()
        Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        Me.Font = New Font(sNombreFuente, iTamanioFuente)
        Me.TextAlign = ContentAlignment.MiddleCenter
        Me.Cursor = Cursors.Cross
        CrearEtiqueta()
        If Me.DesignMode Then
            EmparentarEtiqueta()
        Else
            CrearTemporizadores()
        End If
    End Sub
    Private Sub CargarOpciones()
        cEtiquetaBackcolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelBackColor", System.Drawing.SystemColors.Highlight.ToArgb.ToString))
        cEtiquetaForecolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelForeColor", System.Drawing.SystemColors.HighlightText.ToArgb.ToString))
        cColorAlerta = Color.FromArgb(GetSetting("hhControls", "Colors", "AlertBackColor", System.Drawing.Color.Red.ToArgb.ToString))
        cColorNormal = Color.FromArgb(GetSetting("hhControls", "Colors", "NormalBackColor", System.Drawing.SystemColors.Window.ToArgb.ToString))
        iIntervaloAlerta = Val(GetSetting("hhcontrols", "refresh", "alertinterval", "1000"))
        iTamanioFuente = Val(GetSetting("hhControls", "Font", "Size", "14"))
        sNombreFuente = GetSetting("hhControls", "Font", "Name", "Verdana")
    End Sub
    Private Sub CrearTemporizadores()
        tAlerta = New Timer
        tAlerta.Interval = ForzarRango(iIntervaloAlerta, 100, 60000)
        tAlerta.Enabled = False
    End Sub
    Function ForzarRango(ByVal Valor As Integer, ByVal ValorMinimo As Integer, ByVal ValorMaximo As Integer) As Integer
        If Valor < ValorMinimo Or Valor > ValorMaximo Then
            Valor = ValorMinimo
        End If
        Return Valor
    End Function
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
            If Me.DesignMode Then
                CrearEtiqueta()
            End If
            EmparentarEtiqueta()
            lEtiqueta.Text = sEtiqueta
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
                tHint.SetToolTip(lEtiqueta, sTooltip.Replace("|", vbCrLf))
                tHint.SetToolTip(Me, sTooltip.Replace("|", vbCrLf))
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
        sTempTooltip = sTooltip & "|Maximo:" & iValorMaximo
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
        EmparentarEtiqueta()
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
        DarFormato()
    End Sub
    Private Sub DarFormato()
        Me.Text = (iValor \ 60).ToString & "'" & (iValor Mod 60).ToString.PadLeft(2, "0") & Chr(34)
        If Not IsNothing(tAlerta) Then
            If EnRango(iValor, iValorMaximo, 0) Then
                tAlerta.Enabled = False
                Me.BackColor = cColorNormal
            Else
                tAlerta.Enabled = True
            End If
        End If
    End Sub
    Private Function EnRango(ByVal Valor As Integer, ByVal ValorMaximo As Integer, ByVal ValorMinimo As Integer) As Boolean
        Return ((Valor <= ValorMaximo) And (Valor >= ValorMinimo))
    End Function
    Sub Alerta(ByVal s As Object, ByVal e As System.EventArgs) Handles tAlerta.Tick
        bAlerta = Not bAlerta
        If bAlerta Then
            Me.BackColor = cColorAlerta
        Else
            Me.BackColor = cColorNormal
        End If
    End Sub
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not IsNothing(tAlerta) Then
                tAlerta.Enabled = False
                tAlerta = Nothing
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
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        If Not IsNothing(lEtiqueta) Then
            If Not IsNothing(Me.Parent) Then
                Me.Parent.Controls.Remove(lEtiqueta)
            End If
            lEtiqueta = Nothing
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
End Class

