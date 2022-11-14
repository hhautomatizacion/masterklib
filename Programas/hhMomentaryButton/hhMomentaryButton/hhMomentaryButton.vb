Imports System.IO
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing
Imports MasterKlib
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

<DefaultEvent("Click")> Public Class hhMomentaryButton
    Inherits System.Windows.Forms.CheckBox
    Dim sId As String
    Dim bAutoActualizar As Boolean
    Dim bAutoSize As Boolean
    Dim sTexto As String
    Dim sDireccionEscritura As String
    Dim sDireccionLectura As String
    Dim cColorSeleccionTexto As Color
    Dim cColorSeleccion As Color
    Dim cColorBotonTexto As Color
    Dim cColorBoton As Color
    Dim fFuenteBoton As Font
    Dim sTooltip As String
    Dim iAnchoTooltip As Integer
    Dim iAltoRenglonTooltip As Integer
    Dim fFuenteEtiqueta As Font
    Dim mMasterk As MasterKlib.MasterK
    Dim tHint As System.Windows.Forms.ToolTip
    Sub New()
        MyBase.New()
        CargarOpciones()
        MyBase.Appearance = Windows.Forms.Appearance.Button

        MyBase.TextAlign = ContentAlignment.MiddleCenter
        MyBase.Cursor = Cursors.Cross
        MyBase.Font = fFuenteBoton

        MyBase.UseVisualStyleBackColor = False
        MyBase.BackColor = cColorBoton
        MyBase.ForeColor = cColorBotonTexto
        MyBase.Checked = False
        DarFormato()
    End Sub
    Private Sub CargarOpciones()
        Try
            fFuenteBoton = New Font(GetSetting("hhControls", "Font", "ButtonFontName", "Verdana"), Val(GetSetting("hhControls", "Font", "ButtonFontSize", "10")))
        Catch ex As Exception
            fFuenteBoton = New Font("Verdana", 10)
        End Try
        Try
            fFuenteEtiqueta = New Font(GetSetting("hhControls", "Font", "LabelFontName", "Verdana"), Val(GetSetting("hhControls", "Font", "LabelFontSize", "14")))
        Catch ex As Exception
            fFuenteEtiqueta = New Font("Verdana", 14)
        End Try
        cColorBoton = Color.FromArgb(GetSetting("hhControls", "Colors", "ButtonColor", SystemColors.Control.ToArgb.ToString))
        cColorBotonTexto = Color.FromArgb(GetSetting("hhControls", "Colors", "ButtonTextColor", System.Drawing.SystemColors.ControlText.ToArgb.ToString))
        cColorSeleccion = Color.FromArgb(GetSetting("hhControls", "Colors", "HighlightColor", SystemColors.Highlight.ToArgb.ToString))
        cColorSeleccionTexto = Color.FromArgb(GetSetting("hhControls", "Colors", "HighlightTextColor", System.Drawing.SystemColors.HighlightText.ToArgb.ToString))
        iAnchoTooltip = Val(GetSetting("hhControls", "Tooltip", "TooltipWidth", "200"))
        GuardarOpciones()
    End Sub
    Private Sub GuardarOpciones()
        SaveSetting("hhControls", "Font", "ButtonFontName", fFuenteBoton.Name)
        SaveSetting("hhControls", "Font", "ButtonFontSize", fFuenteBoton.Size.ToString)
        SaveSetting("hhControls", "Font", "LabelFontName", fFuenteEtiqueta.Name)
        SaveSetting("hhControls", "Font", "LabelFontSize", fFuenteEtiqueta.Size.ToString)
        SaveSetting("hhControls", "Colors", "HighlightColor", cColorSeleccion.ToArgb.ToString)
        SaveSetting("hhControls", "Colors", "HighlightTextColor", cColorSeleccionTexto.ToArgb.ToString)
        SaveSetting("hhControls", "Colors", "ButtonColor", cColorBoton.ToArgb.ToString)
        SaveSetting("hhControls", "Colors", "ButtonTextColor", cColorBotonTexto.ToArgb.ToString)
        SaveSetting("hhControls", "Tooltip", "TooltipWidth", iAnchoTooltip.ToString)
    End Sub
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
    Property Texto() As String
        Get
            Return sTexto
        End Get
        Set(ByVal value As String)
            sTexto = value
            AcomodarTexto()
        End Set
    End Property
    Private Sub AcomodarTexto()
        Dim sTemp As String
        sTemp = sTexto
        If TextRenderer.MeasureText(sTemp, fFuenteBoton).Width >= (MyBase.ClientSize.Width - TextRenderer.MeasureText(".", fFuenteBoton).Width) Then
            Do
                If Len(sTemp) Then
                    sTemp = sTemp.Substring(0, sTemp.Length - 1)
                Else
                    sTemp = sTexto
                    Exit Do
                End If
            Loop Until TextRenderer.MeasureText(sTemp & "...", fFuenteBoton).Width < (MyBase.ClientSize.Width - TextRenderer.MeasureText(".", fFuenteBoton).Width)
            sTemp = sTemp & "..."
        End If
        MyBase.Text = sTemp
        If IsNothing(MyBase.Image) Then
            MyBase.TextAlign = ContentAlignment.MiddleCenter
        Else
            MyBase.TextAlign = ContentAlignment.BottomCenter
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
    Property Tooltip() As String
        Get
            Return sTooltip
        End Get
        Set(ByVal value As String)
            If Not IsNothing(value) Then
                sTooltip = value
                tHint = New System.Windows.Forms.ToolTip
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
    Private Sub Draw(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawToolTipEventArgs)
        Dim sRenglon As String
        Dim sColumna As String
        Dim sTempTooltip As String
        Dim iRenglon As Integer
        Dim iColumna As Integer
        sTempTooltip = e.ToolTipText
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
        e.ToolTipSize = New System.Drawing.Size(ianchotooltip, iAltoRenglonTooltip * 4)
    End Sub
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not IsNothing(mMasterk) Then
                mMasterk.Quitar(sId)
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
    Private Sub hhMomentaryButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CheckedChanged
        If IsNothing(sDireccionLectura) Then
            Me.Checked = False
        End If
        DarFormato()
    End Sub
    Private Sub DarFormato()
        MyBase.UseVisualStyleBackColor = False
        If MyBase.Checked Then
            MyBase.ForeColor = cColorSeleccionTexto
            MyBase.BackColor = cColorSeleccion
        Else
            MyBase.ForeColor = cColorBotonTexto
            MyBase.BackColor = cColorBoton
        End If
    End Sub
    Sub Actualizar()
        If Not IsNothing(mMasterk) Then
            Me.Checked = mMasterk.ObtenerBoolean(sDireccionLectura)
        End If
        DarFormato()
    End Sub
    Private Sub hhMomentaryButton_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        If Not IsNothing(mMasterk) Then
            mMasterk.EstablecerBoolean(sDireccionEscritura, 1)
            Threading.Thread.Sleep(5)
            mMasterk.EstablecerBoolean(sDireccionEscritura, 0)
        End If
        Actualizar()
    End Sub
End Class
