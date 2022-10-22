Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing
Public Class hhSliderBar
    Inherits TrackBar
    Dim sId As String
    Dim sDireccionLectura As String
    Dim sDireccionEscritura As String
    Dim sTooltip As String
    Dim tHint As ToolTip
    Dim sFactor As Single
    Dim iDecimales As Integer
    Dim iAltoRenglonTooltip As Integer
    Dim iValor As Integer
    Dim bAutoActualizar As Boolean
    Dim mMasterk As MasterKlib.MasterK
    Sub New()
        Me.Cursor = Cursors.Cross
    End Sub
    Public Overrides Property Font() As System.Drawing.Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As System.Drawing.Font)
            Dim sNombreFuente As String
            Dim iTamanioFuente As String
            Try
                iTamanioFuente = Val(GetSetting("hhControls", "Font", "FontSize", "12"))
                sNombreFuente = GetSetting("hhControls", "Font", "FontName", "Verdana")
                MyBase.Font = New Font(sNombreFuente, iTamanioFuente)
            Catch ex As Exception
                MyBase.Font = value
            End Try
        End Set
    End Property
    Property Tooltip() As String
        Get
            Return sTooltip
        End Get
        Set(ByVal value As String)
            If Not IsNothing(value) Then
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
    Property Factor() As Single
        Get
            Return sFactor
        End Get
        Set(ByVal value As Single)
            sFactor = value
        End Set
    End Property
    Property Decimales() As Integer
        Get
            Return iDecimales
        End Get
        Set(ByVal value As Integer)
            iDecimales = value
        End Set
    End Property
    Private Sub Draw(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawToolTipEventArgs)
        Dim sRenglon As String
        Dim sColumna As String
        Dim sTempTooltip As String
        Dim iRenglon As Integer
        Dim iColumna As Integer
        sTempTooltip = sTooltip & "|Maximo:" & DarFormato(Me.Maximum) & "|Minimo:" & DarFormato(Me.Minimum)
        Try
            e.Graphics.FillRectangle(SystemBrushes.ActiveCaption, e.Bounds)
            iRenglon = 0
            For Each sRenglon In Split(sTempTooltip, "|")
                If InStr(sRenglon, ":") Then
                    iColumna = 0
                    For Each sColumna In Split(sRenglon, ":")
                        If TextRenderer.MeasureText(sColumna, MyBase.Font).Width > e.Bounds.Width / 2 Then
                            Do
                                If Len(sColumna) < 3 Then Exit Do
                                sColumna = sColumna.Substring(0, sColumna.Length - 1)
                            Loop Until TextRenderer.MeasureText(sColumna & "...", MyBase.Font).Width < e.Bounds.Width / 2
                            sColumna = sColumna & "..."
                        End If
                        e.Graphics.DrawString(sColumna, MyBase.Font, SystemBrushes.ActiveCaptionText, iColumna * (e.Bounds.Width / 2), iRenglon * iAltoRenglonTooltip)
                        iColumna = iColumna + 1
                    Next sColumna
                Else
                    If TextRenderer.MeasureText(sRenglon, MyBase.Font).Width > e.Bounds.Width Then
                        Do
                            If Len(sRenglon) < 3 Then Exit Do
                            sRenglon = sRenglon.Substring(0, sRenglon.Length - 1)
                        Loop Until TextRenderer.MeasureText(sRenglon & "...", MyBase.Font).Width < e.Bounds.Width
                        sRenglon = sRenglon & "..."
                    End If
                    e.Graphics.DrawString(sRenglon, MyBase.Font, SystemBrushes.ActiveCaptionText, 0, iRenglon * iAltoRenglonTooltip)
                End If
                iRenglon = iRenglon + 1
            Next sRenglon
        Finally
        End Try
    End Sub
    Private Sub Popup(ByVal sender As Object, ByVal e As System.Windows.Forms.PopupEventArgs)
        iAltoRenglonTooltip = TextRenderer.MeasureText("Receta", MyBase.Font).Height
        e.ToolTipSize = New System.Drawing.Size(Me.Width, iAltoRenglonTooltip * 5)
    End Sub

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

    Property AutoActualizar() As Boolean
        Get
            Return bAutoActualizar
        End Get
        Set(ByVal value As Boolean)
            bAutoActualizar = value
        End Set
    End Property

    Sub Actualizar()
        Dim iValor As Integer
        iValor = mMasterk.ObtenerEntero(sDireccionLectura)
        Me.Value = iValor
    End Sub
    Private Sub hhSliderBar_Scroll(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Scroll
        Dim iValor As Integer
        iValor = Me.Value
        If Not IsNothing(mMasterk) Then
            mMasterk.EstablecerEntero(sDireccionEscritura, iValor)
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
End Class
