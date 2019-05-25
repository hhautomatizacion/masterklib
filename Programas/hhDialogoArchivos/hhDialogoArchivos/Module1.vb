
Module Module1
    Public sFolder As String
    Public sUnidad As String
    Public sExtension As String
    Public sNombreArchivo As String
    Public sNombreCompleto As String
    'Public sNombreFuente As String
    'Public iTamanioFuente As Integer
    Public dResultado As Windows.Forms.DialogResult
    Public f As Form1

    Public Function RutaCompleta(Optional ByVal Unidad As String = "", Optional ByVal Folder As String = "") As String
        If Len(Unidad) = 0 Then Unidad = sUnidad
        If Len(Folder) = 0 Then Folder = sFolder
        Return Unidad & "\" & Folder & "\"
    End Function
    Public Function ExtensionCompleta(ByVal Extension As String)
        If Extension.ToUpper.StartsWith("*.") Then
            Return Extension.ToUpper.Replace("*", "")
        Else
            Return ""
        End If
    End Function
End Module
