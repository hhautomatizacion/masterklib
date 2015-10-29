
Public Class hhBooleanLabel

    Inherits System.Windows.Forms.Label
    Dim sId As String
    Dim bValor As Boolean
    Dim bAutoActualizar As Boolean
    Dim sTextoVerdadero As String
    Dim cColorTextoVerdadero As System.Drawing.Color
    Dim cColorFondoVerdadero As System.Drawing.Color
    Dim sTextoFalso As String
    Dim cColorTextoFalso As System.Drawing.Color
    Dim cColorFondoFalso As System.Drawing.Color


    Dim sDireccionLectura As String
    Dim mMasterk As MasterKlib.MasterK
    Sub New()
        MyBase.New()
        Me.AutoSize = False
        Me.TextAlign = Drawing.ContentAlignment.MiddleCenter
        Me.Cursor = Windows.Forms.Cursors.Cross
    End Sub
    Public Sub Actualizar()
        If Not IsNothing(sDireccionLectura) Then
            bValor = mMasterk.ObtenerBoolean(sDireccionLectura)
            If bValor Then
                Me.Text = sTextoVerdadero
                Me.ForeColor = cColorTextoVerdadero
                Me.BackColor = cColorFondoVerdadero
            Else
                Me.Text = sTextoFalso
                Me.ForeColor = cColorTextoFalso
                Me.BackColor = cColorFondoFalso
            End If
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
    Public Property ColorTextoVerdadero() As System.Drawing.Color
        Get
            Return cColorTextoVerdadero
        End Get
        Set(ByVal value As System.Drawing.Color)
            cColorTextoVerdadero = value
        End Set
    End Property
    Public Property ColorFondoVerdadero() As System.Drawing.Color
        Get
            Return cColorfondoVerdadero
        End Get
        Set(ByVal value As System.Drawing.Color)
            cColorfondoVerdadero = value
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
    Public Property ColorTextoFalso() As System.Drawing.Color
        Get
            Return cColorTextoFalso
        End Get
        Set(ByVal value As System.Drawing.Color)
            cColorTextoFalso = value
        End Set
    End Property
    Public Property ColorFondoFalso() As System.Drawing.Color
        Get
            Return cColorFondoFalso
        End Get
        Set(ByVal value As System.Drawing.Color)
            cColorFondoFalso = value
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
