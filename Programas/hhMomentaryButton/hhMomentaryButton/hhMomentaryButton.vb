Imports System.IO
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing
Public Class hhMomentaryButton
    Inherits System.Windows.Forms.CheckBox
    Dim sId As String
    Dim bAutoActualizar As Boolean
    Dim sNombreFuente As String
    Dim iTamaniofuente As String
    Dim sDireccionEscritura As String
    Dim sDireccionLectura As String
    Dim mMasterk As MasterKlib.MasterK
    Public Sub New()
        CargarOpciones()
        Me.Font = New Font(sNombreFuente, iTamaniofuente)
        Me.Appearance = Windows.Forms.Appearance.Button
        Me.TextAlign = ContentAlignment.MiddleCenter
        Me.Cursor = Cursors.Cross
    End Sub
    Private Sub CargarOpciones()
        'cEtiquetaBackcolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelBackColor", System.Drawing.SystemColors.Highlight.ToArgb.ToString))
        'cEtiquetaForecolor = Color.FromArgb(GetSetting("hhControls", "Colors", "LabelForeColor", System.Drawing.SystemColors.HighlightText.ToArgb.ToString))
        iTamanioFuente = Val(GetSetting("hhControls", "Font", "Size", "14"))
        sNombreFuente = GetSetting("hhControls", "Font", "Name", "Verdana")
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

    Protected Overrides Sub OnParentChanged(ByVal e As System.EventArgs)
        MyBase.OnParentChanged(e)
        Me.Font = New Font(sNombreFuente, iTamaniofuente)
    End Sub
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)

        If disposing Then
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
    Private Sub hhMomentaryButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CheckedChanged
        If IsNothing(sDireccionLectura) Then
            Me.Checked = False
        End If
        DarFormato()
    End Sub
    Private Sub hhMomentaryButton_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        Dim iContador As Integer
        If Not IsNothing(mMasterk) Then
            iContador = 0
            Do
                iContador = iContador + 1
                mMasterk.EstablecerBoolean(sDireccionEscritura, 1)
                Application.DoEvents()

            Loop Until mMasterk.ObtenerBoolean(sDireccionEscritura) = True Or iContador >= 3
            iContador = 0
            Do
                iContador = iContador + 1
                mMasterk.EstablecerBoolean(sDireccionEscritura, 0)
                Application.DoEvents()

            Loop Until mMasterk.ObtenerBoolean(sDireccionEscritura) = False Or iContador >= 3
        End If
    End Sub

    Private Sub DarFormato()
        If Me.Checked Then
            Me.ForeColor = SystemColors.HighlightText
            Me.BackColor = SystemColors.Highlight
        Else
            Me.ForeColor = SystemColors.ControlText
            Me.BackColor = SystemColors.Control
            Me.UseVisualStyleBackColor = True
        End If
    End Sub
    Sub Actualizar()
        If Not IsNothing(mMasterk) Then
            Me.Checked = mMasterk.ObtenerBoolean(sDireccionLectura)
        End If
        DarFormato()
    End Sub

End Class
