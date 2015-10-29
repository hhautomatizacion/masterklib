Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing
Public Class hhCharacterDisplay
    Inherits System.Windows.Forms.Label
    Dim sId As String
    Dim sNombreFuente As String
    Dim iTamanioFuente As Integer
    Dim bAutoActualizar As Boolean
    Dim sDireccionLectura As String
    Dim iLongitud As Integer
    Dim mMasterk As MasterKlib.MasterK


    Public Sub New()
        MyBase.New()
        CargarOpciones()

        Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        Me.Font = New Font(sNombreFuente, iTamanioFuente)
        Me.TextAlign = ContentAlignment.MiddleCenter
        Me.Cursor = Cursors.Cross
        Me.Text = ""
        'CrearEtiqueta()
        'If Me.DesignMode Then
        'EmparentarEtiqueta()
        'Else
        'CrearTemporizadores()
        'End If

    End Sub
    Private Sub CargarOpciones()
        'cEtiquetaBackcolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelBackColor", System.Drawing.SystemColors.Highlight.ToArgb.ToString))
        'cEtiquetaForecolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelForeColor", System.Drawing.SystemColors.HighlightText.ToArgb.ToString))
        'cColorAlerta = Color.FromArgb(GetSetting("hhControls", "Colors", "AlertBackColor", System.Drawing.Color.Red.ToArgb.ToString))
        'cColorNormal = Color.FromArgb(GetSetting("hhControls", "Colors", "NormalBackColor", System.Drawing.SystemColors.Window.ToArgb.ToString))
        'iIntervaloAlerta = Val(GetSetting("hhcontrols", "refresh", "alertinterval", "1000"))
        iTamanioFuente = Val(GetSetting("hhControls", "Font", "Size", "14"))
        sNombreFuente = GetSetting("hhControls", "Font", "Name", "Verdana")
    End Sub
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
    Property Longitud() As Integer
        Get
            Return iLongitud
        End Get
        Set(ByVal value As Integer)
            iLongitud = value
        End Set
    End Property
    Property AutoActualizar() As Boolean
        Get
            Return bAutoActualizar
        End Get
        Set(ByVal value As Boolean)
            bAutoActualizar = value

        End Set
    End Property



    Sub Actualizar()
        If Not IsNothing(mMasterk) Then
            Me.Text = mMasterk.ObtenerCadena(sDireccionLectura, iLongitud)
        Else
            Me.Text = ""
        End If
    End Sub
  
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Me.Font = New Font(sNombreFuente, iTamanioFuente)

    End Sub
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)

        If disposing Then
            'If Not IsNothing(tAlerta) Then
            'tAlerta.Enabled = False
            'tAlerta = Nothing
            'End If
            'If Not IsNothing(lEtiqueta) Then
            'If Not IsNothing(Me.Parent) Then
            'Me.Parent.Controls.Remove(lEtiqueta)
            'End If
            'lEtiqueta = Nothing
            'End If
            'If Not IsNothing(tHint) Then
            'tHint.Dispose()
            'End If
            If Not IsNothing(mMasterk) Then
                mMasterk.Quitar(sId)
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class
