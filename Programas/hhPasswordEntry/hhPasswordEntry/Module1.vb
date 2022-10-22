Imports System.Drawing

Module Module1
    'Public mMasterk As MasterKlib.MasterK
    'Public sDireccionOk As String
    'Public sDireccionCancel As String
    'Public sTextoOk As String
    'Public iImagenOk As System.Drawing.Image
    'Public sTextoCancel As String
    'Public iImagenCancel As System.Drawing.Image
    'Public iImagen As System.Drawing.Image
    'Public iTamanio As Integer
    'Public sMensaje As String
    Public iAnchoBoton As Integer
    Public iAltoBoton As Integer
    Public cColorAlerta As System.Drawing.Color
    Public cColorNormal As System.Drawing.Color
    Public fFuente As Font
    Public iNivelAutorizacion As Integer
    Public sSeccion As String
    Public sAplicacion As String
    Public sPassword As String
    Public dResultado As System.Windows.Forms.DialogResult
    Public f As Form1

    Sub CargarOpciones()
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
End Module
