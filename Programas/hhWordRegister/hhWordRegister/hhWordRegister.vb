Imports System.Windows.Forms
Public Class hhWordRegister
    Dim sId As String
    Dim sNombre As String
    Dim bAutoActualizar As Boolean
    'Dim bAlerta As Boolean
    Dim iValor As Integer
    Dim iValorAnterior As Integer
    Dim iValorMaximo As Integer
    Dim sDireccionLectura As String
    Dim sDireccionEscritura As String
    Dim mMasterk As MasterKlib.MasterK
    Public Event Cambio As System.EventHandler
    Sub New()
        MyBase.New()
    End Sub
    Private Function EnRango(ByVal Valor As Integer, ByVal ValorMaximo As Integer, ByVal ValorMinimo As Integer) As Boolean
        Return ((Valor <= ValorMaximo) And (Valor >= ValorMinimo))
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
    Property Name() As String
        Get
            Return sNombre
        End Get
        Set(ByVal value As String)
            sNombre = value
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
            If EnRango(value, iValorMaximo, 0) Then
                iValor = value
                If Not IsNothing(sDireccionEscritura) Then
                    mMasterk.EstablecerEntero(sDireccionEscritura, iValor)
                End If
            End If
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
        If Not IsNothing(sDireccionLectura) Then
            iValor = mMasterk.ObtenerEntero(sDireccionLectura)
            If mMasterk.Failed Or mMasterk.TimedOut Then
                iValor = 0
            Else
                If iValor <> iValorAnterior Then
                    iValorAnterior = iValor
                    RaiseEvent Cambio(Me, New System.EventArgs)
                End If
            End If
        End If
    End Sub
    Protected Overrides Sub Finalize()
        If Not IsNothing(mMasterk) Then
            mMasterk.Quitar(sId)
        End If
        MyBase.Finalize()
    End Sub
End Class
