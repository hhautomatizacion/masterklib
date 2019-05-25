Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing
Public Class hhDialogoArchivos
    Inherits CommonDialog

    Dim iAltoBoton As Integer
    Dim iAnchoBoton As Integer
    Dim cSeleccion As Collection
    Dim iLongitud As Integer
    Sub New()
        MyBase.New()

        f = New Form1
        cSeleccion = New Collection
        Reset()
    End Sub

    Overrides Sub Reset()

    End Sub
    Protected Overrides Function RunDialog(ByVal hwndOwner As System.IntPtr) As Boolean

        sNombreArchivo = ""

        f.CheckedListBox1.Items.Clear()

        f.HhCharacterEntry1.LongitudTexto = iLongitud
        f.Label1.Text = RutaCompleta().Replace("&", "&&")

        f.Inicializar()


        If f.ShowDialog() = DialogResult.OK Then
            cSeleccion = f.c
            dResultado = DialogResult.OK
        Else

            dResultado = DialogResult.Cancel
        End If


    End Function
    Public ReadOnly Property NombreArchivo() As String
        Get
            Return sNombreArchivo
        End Get
    End Property
    Public ReadOnly Property NombreCompleto() As String
        Get
            Return sNombreCompleto
        End Get
    End Property
    Public Property Longitud() As Integer
        Get
            Return iLongitud
        End Get
        Set(ByVal value As Integer)
            iLongitud = value
        End Set
    End Property
    Public Property Unidad() As String
        Get
            Return sUnidad
        End Get
        Set(ByVal value As String)
            If Len(value) = 0 Then value = "c:"
            If Len(value) >= 2 Then
                sUnidad = value.Substring(0, 2)
            End If
            sUnidad = sUnidad.ToUpper
        End Set
    End Property
    Public Property Folder() As String
        Get
            Return sFolder
        End Get
        Set(ByVal value As String)
            If Len(value) = 0 Then value = "h&h"
            sFolder = value
            sFolder = sFolder.ToUpper
        End Set
    End Property
    Public Property Extension() As String
        Get
            Return sExtension
        End Get
        Set(ByVal value As String)
            sExtension = value.ToUpper
        End Set
    End Property
    Public ReadOnly Property Archivos() As Collection
        Get
            Return cSeleccion
        End Get
    End Property
    Public ReadOnly Property Resultado() As Windows.Forms.DialogResult
        Get
            Return dResultado
        End Get
    End Property
End Class
