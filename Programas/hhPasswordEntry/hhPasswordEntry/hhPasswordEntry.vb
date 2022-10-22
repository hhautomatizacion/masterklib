Imports System.IO
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Net.Mime.MediaTypeNames

Public Class hhPasswordEntry
    Inherits System.Windows.Forms.CommonDialog

    Sub New()
        MyBase.New()
        f = New Form1
        CargarOpciones()
        f.TextBox1.Font = fFuente
        For Each b In f.TableLayoutPanel1.Controls
            If TypeOf (b) Is Button Then
                b.Font = fFuente
            End If
        Next
    End Sub

    Overrides Sub Reset()
        sPassword = ""
        iNivelAutorizacion = 0
        dResultado = Nothing
    End Sub

    Protected Overrides Function RunDialog(ByVal hwndOwner As System.IntPtr) As Boolean
        Reset()
        dResultado = f.ShowDialog()
        Return dResultado
    End Function

    Private Sub CargarOpciones()
        Try
            fFuente = New Font(GetSetting("hhControls", "Font", "FontName", "Verdana"), Val(GetSetting("hhControls", "Font", "FontSize", "18")))
        Catch ex As Exception
            fFuente = New Font("Verdana", 18)
        End Try
        cColorAlerta = Color.FromArgb(GetSetting("hhControls", "Colors", "AlertBackColor", System.Drawing.Color.Red.ToArgb.ToString))
        cColorNormal = Color.FromArgb(GetSetting("hhControls", "Colors", "NormalBackColor", System.Drawing.SystemColors.Window.ToArgb.ToString))
        iAltoBoton = Val(GetSetting("hhControls", "Size", "ButtonHeight", "70"))
        iAnchoBoton = Val(GetSetting("hhControls", "Size", "ButtonWidth", "70"))
    End Sub

    Property NivelDeAutorizacion() As Integer
        Get
            Return iNivelAutorizacion
        End Get
        Set(value As Integer)
            iNivelAutorizacion = value
        End Set
    End Property

    Property Aplicacion As String
        Get
            Return sAplicacion
        End Get
        Set(value As String)
            sAplicacion = value
        End Set
    End Property

    Property Seccion As String
        Get
            Return sSeccion
        End Get
        Set(value As String)
            sSeccion = value
        End Set
    End Property
End Class
