Imports System.IO
Imports System.Drawing
Imports System.ComponentModel
Imports System.Windows.Forms
Public Class hhIncrementButton
    Inherits System.Windows.Forms.Button
    'Dim sPuerto As System.IO.Ports.SerialPort
    Dim sNombreFuente As String
    Dim iTamanioFuente As Integer
    Dim sDireccionLectura As String
    Dim sDireccionEscritura As String
    Dim iIncremento As Integer = 1
    Dim iValorMaximo As Integer = 65535
    Dim iValorMinimo As Integer = 0
    'Dim bEstacion As Byte
    Dim mMasterK As MasterKlib.MasterK
    Sub New()
        sNombreFuente = GetSetting("hhcontrols", "font", "name", "Verdana")
        iTamanioFuente = Val(GetSetting("hhcontrols", "font", "size", "25"))
        Me.Font = New Font(sNombreFuente, iTamanioFuente)
    End Sub

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
    Property Incremento() As Integer
        Get
            Return iIncremento
        End Get
        Set(ByVal value As Integer)
            iIncremento = value
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
    Property ValorMinimo() As Integer
        Get
            Return iValorMinimo
        End Get
        Set(ByVal value As Integer)
            iValorMinimo = value
        End Set
    End Property


    Private Sub hhIncrementButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click
        Dim sRespuesta As String
        Dim iValor As Integer
        mMasterK.RSS(sDireccionLectura)
        sRespuesta = mMasterK.RespuestaDelPLC
        If Len(sRespuesta) > 14 Then
            iValor = CInt("&H" & sRespuesta.Substring(10, 4))
            iValor = iValor + iIncremento
            If iValor <= iValorMaximo Then
                mMasterK.WSS(sDireccionEscritura, iValor.ToString("X2").PadLeft(4, "0"))
                sRespuesta = mMasterK.RespuestaDelPLC

            End If
        End If
    End Sub
End Class
