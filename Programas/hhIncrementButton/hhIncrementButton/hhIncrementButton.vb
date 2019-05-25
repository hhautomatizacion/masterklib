Imports System.IO
Imports System.Drawing
Imports System.ComponentModel
Imports System.Windows.Forms
Public Class hhIncrementButton
    Inherits System.Windows.Forms.Button
    Dim fFuenteBoton As Font
    Dim sDireccionLectura As String
    Dim sDireccionEscritura As String
    Dim iIncremento As Integer
    Dim iValorMaximo As Integer
    Dim iValorMinimo As Integer
    Dim iValor As Integer
    Dim mMasterK As MasterKlib.MasterK
    Sub New()
        MyBase.New
        CargarOpciones()
        Me.Font = fFuenteBoton
    End Sub
    Private Sub CargarOpciones()
        Try
            fFuenteboton = New Font(GetSetting("hhControls", "Font", "ButtonFontName", "Verdana"), Val(GetSetting("hhControls", "Font", "FontSize", "10")))
        Catch ex As Exception
            fFuenteboton = New Font("Verdana", 10)
        End Try
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
    Property Link() As MasterKlib.MasterK
        Get
            Return mMasterk
        End Get
        Set(ByVal value As MasterKlib.MasterK)
            mMasterk = value
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
    <DefaultValue(1)> Property Incremento() As Integer
        Get
            Return iIncremento
        End Get
        Set(ByVal value As Integer)
            iIncremento = value
        End Set
    End Property
    <DefaultValue(65535)> Property ValorMaximo() As Integer
        Get
            Return iValorMaximo
        End Get
        Set(ByVal value As Integer)
            iValorMaximo = value
        End Set
    End Property
    <DefaultValue(0)> Property ValorMinimo() As Integer
        Get
            Return iValorMinimo
        End Get
        Set(ByVal value As Integer)
            iValorMinimo = value
        End Set
    End Property

    Property Valor() As Integer
        Get
            Return iValor
        End Get
        Set(ByVal value As Integer)
            iValor = value
        End Set
    End Property

    Private Sub hhIncrementButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click
        Dim i As Integer
        i = mMasterK.ObtenerEntero(sDireccionLectura) + iIncremento
        If i <= iValorMaximo Then
            mMasterK.EstablecerEntero(sDireccionEscritura, i)
            iValor = i
        End If
    End Sub
End Class
