Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing

Public Class hhSliderBar
    Inherits TrackBar
dim sId as string 
    Dim sDireccionLectura As String
    Dim sDireccionEscritura As String
    Dim cEtiquetaBackcolor As Color
    Dim cEtiquetaForecolor As Color
    Dim sEtiqueta As String
    Dim iTamanioFuente As Integer
dim iValor as integer 
    Dim sNombreFuente As String
    Dim bAutoActualizar As Boolean
    Dim lEtiqueta As Label
    Dim mMasterk As MasterKlib.MasterK
    'Dim WithEvents tTemporizador As Timer
    Sub New()
        'Dim iIntervalo As Integer
        'tTemporizador = New Timer
        iTamanioFuente = Val(GetSetting("hhcontrols", "font", "size", "20"))
        sNombreFuente = GetSetting("hhcontrols", "font", "name", "Verdana")
        'iIntervalo = Val(GetSetting("hhcontrols", "refresh", "interval", "100"))
        cEtiquetaForecolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelForeColor", System.Drawing.SystemColors.MenuText.ToArgb.ToString))
        cEtiquetaBackcolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelBackColor", System.Drawing.SystemColors.MenuHighlight.ToArgb.ToString))

        'If iIntervalo < 0 Or iIntervalo > 60000 Then iIntervalo = 100
        'tTemporizador.Interval = iIntervalo
        Me.Cursor = Cursors.Cross
    End Sub
    Property Link() As MasterKlib.MasterK
        Get
            Return mMasterk
        End Get
        Set(ByVal value As MasterKlib.MasterK)
		if not isnothing(value) then
            		mMasterk = value
			sid=mmasterk.controlid
			mmasterk.controles.add(me,sid)
		end if
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
            If IsNothing(lEtiqueta) Then
            Else
                lEtiqueta.Text = sEtiqueta
            End If
        End Set
    End Property
    Protected Overrides Sub OnParentChanged(ByVal e As System.EventArgs)
        MyBase.OnParentChanged(e)

        lEtiqueta = New Label
        lEtiqueta.Cursor = Cursors.Cross
        lEtiqueta.Font = New Font(sNombreFuente, iTamanioFuente)
        lEtiqueta.TextAlign = ContentAlignment.MiddleCenter
        lEtiqueta.Text = sEtiqueta
        lEtiqueta.Height = Me.Height
        lEtiqueta.Width = 198
        lEtiqueta.BackColor = cEtiquetaBackcolor
        lEtiqueta.ForeColor = cEtiquetaForecolor
        Me.Parent.Controls.Add(Me.lEtiqueta)
        lEtiqueta.Top = Me.Top
        lEtiqueta.Left = Me.Left - 200

        Me.Font = New Font(sNombreFuente, iTamanioFuente)
    End Sub
    Protected Overrides Sub OnMove(ByVal e As System.EventArgs)
        MyBase.OnMove(e)
        If Not IsNothing(lEtiqueta) Then
            lEtiqueta.Top = Me.Top
            lEtiqueta.Left = Me.Left - 200
        End If
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        If Not IsNothing(lEtiqueta) Then
            lEtiqueta.Height = Me.Height
        End If
    End Sub

    Property AutoActualizar() As Boolean
        Get
            Return bAutoActualizar
        End Get
        Set(ByVal value As Boolean)
            bAutoActualizar = value
            tTemporizador.Enabled = bAutoActualizar
        End Set
    End Property
    Sub Actualizar()
        Dim sRespuesta As String
        Dim iValor As Integer
     
        mMasterk.RSS(sDireccionLectura)
        sRespuesta = mMasterk.RespuestaDelPLC
        If Len(sRespuesta) > 14 Then
            iValor = CInt("&H" & sRespuesta.Substring(10, 4))
            Me.Value = iValor
        End If
    End Sub
    'Sub Tick(ByVal s As Object, ByVal e As System.EventArgs) Handles tTemporizador.Tick
    '    Actualizar()
    'End Sub

    Private Sub hhSliderBar_Scroll(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Scroll
        Dim iValor As Integer
'        Dim sRespuesta As String
        iValor = Me.Value
        If Not IsNothing(mMasterk) Then
            mMasterk.establecerentero(sDireccionEscritura, iValor)
'            sRespuesta = mMasterk.RespuestaDelPLC
        End If
    End Sub

End Class
