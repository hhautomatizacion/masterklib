Imports System.IO
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing

<DefaultEvent("Click")> Public Class hhMomentaryButton
    Inherits System.Windows.Forms.CheckBox
    Dim sId As String
    Dim bAutoActualizar As Boolean
    Dim sEtiqueta As String
    Dim sDireccionEscritura As String
    Dim sDireccionLectura As String
    Dim mMasterk As MasterKlib.MasterK
    Public Sub New()
        Me.Appearance = Windows.Forms.Appearance.Button
        Me.TextAlign = ContentAlignment.MiddleCenter
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
                iTamanioFuente = Val(GetSetting("hhControls", "Font", "ButtonFontSize", "8"))
                sNombreFuente = GetSetting("hhControls", "Font", "ButtonFontName", "Verdana")

                MyBase.Font = New Font(sNombreFuente, iTamanioFuente)
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
    Property Etiqueta() As String
        Get
            Return sEtiqueta
        End Get
        Set(ByVal value As String)
            sEtiqueta = value
            AcomodarEtiqueta()
        End Set
    End Property
    Private Sub AcomodarEtiqueta()
        Dim sTemp As String
        sTemp = sEtiqueta
        If TextRenderer.MeasureText(sTemp, MyBase.Font).Width >= (MyBase.ClientSize.Width - TextRenderer.MeasureText(".", MyBase.Font).Width) Then
            Do
                If Len(sTemp) Then
                    sTemp = sTemp.Substring(0, sTemp.Length - 1)
                Else
                    sTemp = sEtiqueta
                    Exit Do
                End If
            Loop Until TextRenderer.MeasureText(sTemp & "...", MyBase.Font).Width < (MyBase.ClientSize.Width - TextRenderer.MeasureText(".", MyBase.Font).Width)
            sTemp = sTemp & "..."
        End If
        MyBase.Text = sTemp
        If IsNothing(MyBase.Image) Then
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
