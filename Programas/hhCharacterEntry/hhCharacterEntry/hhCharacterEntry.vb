Imports System.Windows.Forms
Imports System.IO
Imports System.ComponentModel
Imports System.Drawing
Public Class hhCharacterEntry
    Inherits TextBox
    Dim sId As String
    Dim iAnchoBoton As Integer
    Dim iAltoBoton As Integer
    Dim iAutoOcultar As Integer
    Dim iAltoRenglonTooltip As Integer
    Dim iAnchoTooltip As Integer
    Dim iLongitudTexto As Integer
    Dim cColorNormal As Color
    Dim cColorAlerta As Color
    Dim cColorNormalTexto As Color
    Dim cColorAlertaTexto As Color
    Dim fFuente As Font
    Dim fFuenteEtiqueta As Font
    Dim sDireccionLectura As String
    Dim sDireccionEscritura As String
    Dim sTooltip As String
    Dim sTexto As String
    Dim tHint As ToolTip
    Dim mMasterk As MasterKlib.MasterK
    Dim bAutoActualizar As Boolean
    Dim bUpperCase As Boolean

    <Runtime.InteropServices.DllImport("user32")> Private Shared Function HideCaret(ByVal hWnd As IntPtr) As Integer
    End Function
    Sub New()
        MyBase.New()
        CargarOpciones()
        Me.AutoSize = False
        Me.Cursor = Cursors.Cross
        Me.BorderStyle = BorderStyle.FixedSingle

        Me.Font = fFuente
        Verificar(Me)
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
        bUpperCase = -Val(GetSetting("hhControls", "Text", "UpperCase", "1"))
        iAltoBoton = Val(GetSetting("hhControls", "Size", "SmallButtonHeight", "60"))
        iAnchoBoton = Val(GetSetting("hhControls", "Size", "SmallButtonWidth", "60"))
        iAutoOcultar = Val(GetSetting("hhControls", "Refresh", "AutoHide", "10000"))
        iAnchoTooltip = Val(GetSetting("hhControls", "Tooltip", "TooltipWidth", "200"))
        GuardarOpciones()
    End Sub
    Private Sub GuardarOpciones()
        SaveSetting("hhControls", "Font", "FontName", fFuente.Name)
        SaveSetting("hhControls", "Font", "FontSize", fFuente.Size)
        SaveSetting("hhControls", "Font", "LabelFontName", fFuenteEtiqueta.Name)
        SaveSetting("hhControls", "Font", "LabelFontSize", fFuenteEtiqueta.Size)
        SaveSetting("hhControls", "Colors", "AlertBackColor", cColorAlerta.ToArgb.ToString)
        SaveSetting("hhControls", "Colors", "NormalBackColor", cColorNormal.ToArgb.ToString)
        SaveSetting("hhControls", "Colors", "AlertTextColor", cColorAlertaTexto.ToArgb.ToString)
        SaveSetting("hhControls", "Colors", "NormalTextColor", cColorNormalTexto.ToArgb.ToString)
        SaveSetting("hhControls", "Text", "UpperCase", -bUpperCase)
        SaveSetting("hhControls", "Size", "SmallButtonHeight", iAltoBoton)
        SaveSetting("hhControls", "Size", "SmallButtonWidth", iAnchoBoton)
        SaveSetting("hhControls", "Refresh", "AutoHide", iAutoOcultar)
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
    Private Sub Verificar(ByVal t As TextBox)
        If Len(t.Text) > iLongitudTexto Then
            t.ForeColor = cColorAlertaTexto
            t.BackColor = cColorAlerta
        Else
            t.ForeColor = cColorNormalTexto
            t.BackColor = cColorNormal
        End If
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
    <System.ComponentModel.DefaultValue(Integer.MaxValue)> Property LongitudTexto() As Integer
        Get
            Return iLongitudTexto
        End Get
        Set(ByVal value As Integer)
            iLongitudTexto = value
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

    Property Texto() As String
        Get
            If Not bAutoActualizar Then
                Actualizar()
            End If
            Return sTexto
        End Get
        Set(ByVal value As String)
            If Not IsNothing(mMasterk) Then
                mMasterk.EstablecerCadena(sDireccionEscritura, value)
            End If
            sTexto = value
        End Set
    End Property
    Private Sub Draw(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawToolTipEventArgs)
        Dim sRenglon As String
        Dim sColumna As String
        Dim sTempTooltip As String
        Dim iRenglon As Integer
        Dim iColumna As Integer
        sTempTooltip = sTooltip & "|Maximo:" & iLongitudTexto.ToString & " caracteres "
        Try
            e.Graphics.FillRectangle(SystemBrushes.ActiveCaption, e.Bounds)
            iRenglon = 0
            For Each sRenglon In Split(sTempTooltip, "|")
                If InStr(sRenglon, ":") Then
                    iColumna = 0
                    For Each sColumna In Split(sRenglon, ":")
                        If TextRenderer.MeasureText(sColumna, fFuenteEtiqueta).Width > e.Bounds.Width / 2 Then
                            Do
                                If Len(sColumna) < 3 Then Exit Do
                                sColumna = sColumna.Substring(0, sColumna.Length - 1)
                            Loop Until TextRenderer.MeasureText(sColumna & "...", fFuenteEtiqueta).Width < e.Bounds.Width / 2
                            sColumna = sColumna & "..."
                        End If
                        e.Graphics.DrawString(sColumna, fFuenteEtiqueta, SystemBrushes.ActiveCaptionText, iColumna * (e.Bounds.Width / 2), iRenglon * iAltoRenglonTooltip)
                        iColumna = iColumna + 1
                    Next sColumna
                Else
                    If TextRenderer.MeasureText(sRenglon, fFuenteEtiqueta).Width > e.Bounds.Width Then
                        Do
                            If Len(sRenglon) < 3 Then Exit Do
                            sRenglon = sRenglon.Substring(0, sRenglon.Length - 1)
                        Loop Until TextRenderer.MeasureText(sRenglon & "...", fFuenteEtiqueta).Width < e.Bounds.Width
                        sRenglon = sRenglon & "..."
                    End If
                    e.Graphics.DrawString(sRenglon, fFuenteEtiqueta, SystemBrushes.ActiveCaptionText, 0, iRenglon * iAltoRenglonTooltip)
                End If
                iRenglon = iRenglon + 1
            Next sRenglon
        Finally
        End Try
    End Sub
    Private Sub Popup(ByVal sender As Object, ByVal e As System.Windows.Forms.PopupEventArgs)
        iAltoRenglonTooltip = TextRenderer.MeasureText("Receta", fFuenteEtiqueta).Height
        e.ToolTipSize = New System.Drawing.Size(iAnchoTooltip, iAltoRenglonTooltip * 5)
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
            If Not IsNothing(mMasterk) Then
                mMasterk.Quitar(sId)
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Sub Actualizar()
        If Not IsNothing(mMasterk) Then
            sTexto = mMasterk.ObtenerCadena(sDireccionLectura, iLongitudTexto)
        End If
        Verificar(Me)
        MyBase.Text = sTexto
    End Sub
    Private Sub MostrarTooltip(ByVal s As Object, ByVal e As System.EventArgs)
        If Not IsNothing(tHint) Then
            Try
                tHint.Show(sTooltip, Me, 5000)
            Catch
            End Try
        End If
    End Sub
    Private Sub hhCharacterEntry_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.TextChanged
        Verificar(Me)
    End Sub

    Private Sub hhCharacterEntry_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus
        Me.SelectionLength = 0
        HideCaret(Me.Handle)
    End Sub

    Private Sub hhCharacterEntry_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        e.Handled = True
        If e.KeyCode = Keys.Enter Then
            AbrirTecladoEnPantalla()
        End If
    End Sub
    Private Sub AbrirTecladoEnPantalla()
        Dim b As Control

        Using fTecladoEnPantalla As New Form1
            fTecladoEnPantalla.TextBox1.Font = fFuente
            For Each b In fTecladoEnPantalla.TableLayoutPanel1.Controls
                If TypeOf (b) Is Button Then
                    b.Font = fFuente
                End If
            Next
            fTecladoEnPantalla.iLongitudTexto = iLongitudTexto
            fTecladoEnPantalla.iAltoBoton = iAltoBoton
            fTecladoEnPantalla.iAnchoBoton = iAnchoBoton
            fTecladoEnPantalla.cColorAlerta = cColorAlerta
            fTecladoEnPantalla.cColorNormal = cColorNormal
            fTecladoEnPantalla.bUpperCase = bUpperCase
            fTecladoEnPantalla.Timer1.Interval = iAutoOcultar
            fTecladoEnPantalla.TextBox1.Text = sTexto
            If fTecladoEnPantalla.ShowDialog() = DialogResult.OK Then
                sTexto = fTecladoEnPantalla.TextBox1.Text
                If Not IsNothing(mMasterk) Then
                    If Len(mMasterk.EstablecerCadena(sDireccionEscritura, sTexto)) Then
                    End If
                End If
                Actualizar()
            End If
        End Using
        Verificar(Me)
    End Sub

    Private Sub hhCharacterEntry_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        e.Handled = True
    End Sub

    Private Sub hhCharacterEntry_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        AbrirTecladoEnPantalla()
    End Sub

    Private Sub hhCharacterEntry_LostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus
        MyBase.SelectionLength = 0
        HideCaret(MyBase.Handle)
    End Sub
End Class
