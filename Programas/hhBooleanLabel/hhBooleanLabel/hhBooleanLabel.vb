Imports System.Drawing

Public Class hhBooleanLabel
    Inherits System.Windows.Forms.Label
    Dim sId As String
    Dim bValor As Boolean
    Dim bAutoSize As Boolean
    Dim bAutoActualizar As Boolean
    Dim sTextoVerdadero As String
    Dim fFuenteBoton As System.Drawing.Font
    Dim cColorSeleccion As Color
    Dim cColorSeleccionTexto As Color
    Dim cColorNormal As Color
    Dim cColorNormalTexto As Color
    Dim sTextoFalso As String
    Dim sDireccionLectura As String
    Dim mMasterk As MasterKlib.MasterK
    Sub New()
        MyBase.New()
        CargarOpciones()
        Me.Font = fFuenteBoton
        Me.TextAlign = Drawing.ContentAlignment.MiddleCenter
        Me.Cursor = Windows.Forms.Cursors.Cross

    End Sub
    Private Sub CargarOpciones()
        Try
            fFuenteBoton = New Font(GetSetting("hhControls", "Font", "ButtonFontName", "Verdana"), Val(GetSetting("hhControls", "Font", "ButtonFontSize", "10")))
        Catch ex As Exception
            fFuenteBoton = New Font("Verdana", 10)
        End Try
        cColorNormal = Color.FromArgb(GetSetting("hhControls", "Colors", "NormalBackColor", System.Drawing.SystemColors.Window.ToArgb.ToString))
        cColorNormalTexto = Color.FromArgb(GetSetting("hhControls", "Colors", "NormalTextColor", System.Drawing.SystemColors.WindowText.ToArgb.ToString))
        cColorSeleccion = Color.FromArgb(GetSetting("hhControls", "Colors", "HighlightColor", SystemColors.Highlight.ToArgb.ToString))
        cColorSeleccionTexto = Color.FromArgb(GetSetting("hhControls", "Colors", "HighlightTextColor", System.Drawing.SystemColors.HighlightText.ToArgb.ToString))
        GuardarOpciones()
    End Sub
    Private Sub GuardarOpciones()
        SaveSetting("hhControls", "Font", "ButtonFontName", fFuenteBoton.Name)
        SaveSetting("hhControls", "Font", "ButtonFontSize", fFuenteBoton.Size.ToString)
        SaveSetting("hhControls", "Colors", "NormalBackColor", cColorNormal.ToArgb.ToString)
        SaveSetting("hhControls", "Colors", "NormalTextColor", cColorNormalTexto.ToArgb.ToString)
        SaveSetting("hhControls", "Colors", "HighlightColor", cColorSeleccion.ToArgb.ToString)
        SaveSetting("hhControls", "Colors", "HighlightTextColor", cColorSeleccionTexto.ToArgb.ToString)
    End Sub
    Public Overrides Property Font() As System.Drawing.Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As System.Drawing.Font)
            Try
                MyBase.Font = fFuenteBoton
            Catch ex As Exception
                MyBase.Font = value
            End Try
        End Set
    End Property

    <System.ComponentModel.DefaultValue(False)> Public Overrides Property AutoSize() As Boolean
        Get
            Return MyBase.AutoSize
        End Get
        Set(ByVal value As Boolean)
            If bAutoSize <> value And bAutoSize = False Then
                MyBase.AutoSize = False
                bAutoSize = value
            Else
                MyBase.AutoSize = value
            End If
        End Set
    End Property

    Public Sub Actualizar()
        If Not IsNothing(sDireccionLectura) Then
            bValor = mMasterk.ObtenerBoolean(sDireccionLectura)
        End If
        If bValor Then
            MyBase.Text = sTextoVerdadero
            MyBase.ForeColor = cColorSeleccionTexto
            MyBase.BackColor = cColorSeleccion
        Else
            MyBase.Text = sTextoFalso
            MyBase.ForeColor = cColorNormalTexto
            MyBase.BackColor = cColorNormal
        End If
    End Sub
    Public Property Valor() As Boolean
        Get
            If Not bAutoActualizar Then
                Actualizar()
            End If
            Return bValor
        End Get
        Set(ByVal value As Boolean)
            bValor = value
        End Set
    End Property
    Public Property Link() As MasterKlib.MasterK
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
    Public Property AutoActualizar() As Boolean
        Get
            Return bAutoActualizar
        End Get
        Set(ByVal value As Boolean)
            bAutoActualizar = value
        End Set
    End Property
    Public Property TextoVerdadero() As String
        Get
            Return sTextoVerdadero
        End Get
        Set(ByVal value As String)
            sTextoVerdadero = value
        End Set
    End Property
    Public Property TextoFalso() As String
        Get
            Return sTextoFalso
        End Get
        Set(ByVal value As String)
            sTextoFalso = value
        End Set
    End Property
    Public Property DireccionLectura() As String
        Get
            Return sDireccionLectura
        End Get
        Set(ByVal value As String)
            sDireccionLectura = value
        End Set
    End Property
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not IsNothing(mMasterk) Then
                mMasterk.Quitar(sId)
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class
