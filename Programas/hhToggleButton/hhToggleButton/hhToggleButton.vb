﻿Imports System.IO
Imports System.Drawing
Imports System.Windows.Forms
Public Class hhToggleButton
    Inherits System.Windows.Forms.CheckBox
    Dim sId As String
    Dim cEtiquetaForecolor As Color
    Dim cEtiquetaBackcolor As Color
    Dim mMasterk As MasterKlib.MasterK
    Dim fFuenteBoton As Font
    Dim fFuenteEtiqueta As Font
    Dim sDireccionLectura As String
    Dim sDireccionEscritura As String
    Dim bAutoActualizar As Boolean
    Dim sEtiqueta As String
    Dim sTooltip As String
    Dim iAltoRenglonTooltip As Integer
    Dim lEtiqueta As Label
    Dim tHint As ToolTip
    Sub New()
        MyBase.New()
        CargarOpciones()

        Me.Appearance = Windows.Forms.Appearance.Button
        Me.Cursor = Cursors.Cross
        Me.Font = ffuenteboton

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
        cEtiquetaBackcolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelBackColor", System.Drawing.SystemColors.Highlight.ToArgb.ToString))
        cEtiquetaForecolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelForeColor", System.Drawing.SystemColors.HighlightText.ToArgb.ToString))
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
    Property AutoActualizar() As Boolean
        Get
            Return bAutoActualizar
        End Get
        Set(ByVal value As Boolean)
            bAutoActualizar = value
        End Set
    End Property
    Property Etiqueta() As String
        Get
            Return sEtiqueta
        End Get
        Set(ByVal value As String)
            sEtiqueta = value
            CrearEtiqueta()
            EmparentarEtiqueta()
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
                tHint.SetToolTip(lEtiqueta, sTooltip)
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
                        If TextRenderer.MeasureText(sColumna, ffuenteetiqueta).Width > e.Bounds.Width / 2 Then
                            Do
                                If Len(sColumna) < 3 Then Exit Do
                                sColumna = sColumna.Substring(0, sColumna.Length - 1)
                            Loop Until TextRenderer.MeasureText(sColumna & "...", ffuenteetiqueta).Width < e.Bounds.Width / 2
                            sColumna = sColumna & "..."
                        End If
                        e.Graphics.DrawString(sColumna, ffuenteetiqueta, SystemBrushes.ActiveCaptionText, iColumna * (e.Bounds.Width / 2), iRenglon * iAltoRenglonTooltip)
                        iColumna = iColumna + 1
                    Next sColumna
                Else
                    If TextRenderer.MeasureText(sRenglon, ffuenteetiqueta).Width > e.Bounds.Width Then
                        Do
                            If Len(sRenglon) < 3 Then Exit Do
                            sRenglon = sRenglon.Substring(0, sRenglon.Length - 1)
                        Loop Until TextRenderer.MeasureText(sRenglon & "...", ffuenteetiqueta).Width < e.Bounds.Width
                        sRenglon = sRenglon & "..."
                    End If
                    e.Graphics.DrawString(sRenglon, ffuenteetiqueta, SystemBrushes.ActiveCaptionText, 0, iRenglon * iAltoRenglonTooltip)
                End If
                iRenglon = iRenglon + 1
            Next sRenglon
        Finally
        End Try
    End Sub
    Private Sub Popup(ByVal sender As Object, ByVal e As System.Windows.Forms.PopupEventArgs)
        iAltoRenglonTooltip = TextRenderer.MeasureText("Receta", ffuenteetiqueta).Height
        e.ToolTipSize = New System.Drawing.Size(lEtiqueta.Width, iAltoRenglonTooltip * 4)
    End Sub
    Private Sub CrearEtiqueta()
        If IsNothing(lEtiqueta) Then
            lEtiqueta = New Label
            lEtiqueta.Cursor = Cursors.Cross
            lEtiqueta.Font = ffuenteetiqueta
            lEtiqueta.TextAlign = ContentAlignment.MiddleCenter
            lEtiqueta.Text = sEtiqueta
            lEtiqueta.Height = Me.Height
            lEtiqueta.Width = 198
            lEtiqueta.BackColor = cEtiquetaBackcolor
            lEtiqueta.ForeColor = cEtiquetaForecolor
            lEtiqueta.Top = Me.Top
            lEtiqueta.Left = Me.Left - 200
            lEtiqueta.Visible = True
            AddHandler lEtiqueta.Click, AddressOf MostrarTooltip
        Else
            lEtiqueta.Text = sEtiqueta
        End If
    End Sub
    Private Sub EmparentarEtiqueta()
        If IsNothing(lEtiqueta.Parent) Then
            If Not IsNothing(Me.Parent) Then
                Me.Parent.Controls.Add(lEtiqueta)
            End If
        End If
    End Sub
    Protected Overrides Sub OnParentChanged(ByVal e As System.EventArgs)
        MyBase.OnParentChanged(e)
        EmparentarEtiqueta()
    End Sub
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not IsNothing(lEtiqueta) Then
                If Not IsNothing(Me.Parent) Then
                    Me.Parent.Controls.Remove(lEtiqueta)
                End If
                lEtiqueta = Nothing
            End If
            If Not IsNothing(tHint) Then
                tHint.Dispose()
            End If
            If Not IsNothing(mMasterk) Then
                mMasterk.Quitar(sId)
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
    Protected Overrides Sub OnMove(ByVal e As System.EventArgs)
        MyBase.OnMove(e)
        If IsNothing(lEtiqueta) Then
            CrearEtiqueta()
        Else
            lEtiqueta.Top = Me.Top
            lEtiqueta.Left = Me.Left - 200
        End If
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        If IsNothing(lEtiqueta) Then
            CrearEtiqueta()
        Else
            lEtiqueta.Height = Me.Height
        End If
    End Sub
    Private Sub hhToggleButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CheckedChanged
        DarFormato()
    End Sub
    Private Sub DarFormato()
        If Me.Checked Then
            Me.BackColor = SystemColors.Highlight
        Else
            Me.BackColor = SystemColors.Control
            Me.UseVisualStyleBackColor = True
        End If
    End Sub
    Private Sub hhToggleButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click
        If Not IsNothing(mMasterk) Then
            mMasterk.EstablecerBoolean(sDireccionEscritura, Not mMasterk.ObtenerBoolean(sDireccionLectura))
        End If
    End Sub
    Sub Actualizar()
        If Not IsNothing(mMasterk) Then
            Me.Checked = mMasterk.ObtenerBoolean(sDireccionLectura)
        End If
        DarFormato()
    End Sub
    Private Sub MostrarTooltip(ByVal s As Object, ByVal e As System.EventArgs)
        If Not IsNothing(tHint) Then
            Try
                tHint.Show(sTooltip, Me, 5000)
            Catch
            End Try
        End If
    End Sub
End Class
