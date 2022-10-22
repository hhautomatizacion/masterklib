Imports System.Drawing

Module Module1
    Public mMasterk As MasterKlib.MasterK
    Public sDireccionOk As String
    Public sDireccionCancel As String
    Public sTextoOk As String
    Public iImagenOk As System.Drawing.Image
    Public sTextoCancel As String
    Public iImagenCancel As System.Drawing.Image
    Public fFuente As Font
    Public iImagen As System.Drawing.Image
    Public iTamanio As Integer
    Public sMensaje As String
    Public dResultado As System.Windows.Forms.DialogResult
    Public f As Form1

    Sub CargarOpciones()
        Try
            fFuente = New System.Drawing.Font(GetSetting("hhControls", "Font", "FontName", "Verdana"), Val(GetSetting("hhControls", "Font", "FontSize", "18")))
        Catch ex As Exception
            fFuente = New System.Drawing.Font("Verdana", 18)
        End Try
    End Sub
End Module
