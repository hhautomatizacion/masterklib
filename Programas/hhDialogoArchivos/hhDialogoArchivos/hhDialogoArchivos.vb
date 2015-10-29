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

        iTamanioFuente = Val(GetSetting("hhcontrols", "font", "size", "14"))
        sNombreFuente = GetSetting("hhcontrols", "font", "name", "Verdana")

    End Sub

    Overrides Sub reset()

    End Sub
    Protected Overrides Function rundialog(ByVal hwndOwner As System.IntPtr) As Boolean
        Mostrar()

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
            If Len(value) = 0 Then
                sExtension = "*.*"
            Else
                sExtension = value
            End If
            sExtension = sExtension.ToUpper
        End Set
    End Property
    Public ReadOnly Property Archivos() As Collection
        Get
            Return cSeleccion
        End Get

    End Property
    Sub Mostrar()
        'Dim f As New Form1
        ' make a reference to a directory
        'Dim di As New IO.DirectoryInfo(sRuta)
        'Dim diar1 As IO.FileInfo() = di.GetFiles(sExtension)
        'Dim dra As IO.FileInfo
        fForm = New Form1
        'list the names of all files in the specified directory

        
        If Len(sExtension) = 0 Then
            sExtension = "*.*"
        End If
        sNombreArchivo = ""
        fForm.CheckedListBox1.Font = New Font(sNombreFuente, iTamanioFuente)
        fForm.CheckedListBox1.Items.Clear()
        fForm.Button1.Font = New Font(sNombreFuente, iTamanioFuente)
        fForm.Button2.Font = New Font(sNombreFuente, iTamanioFuente)
        fForm.Button3.Font = New Font(sNombreFuente, iTamanioFuente)
        fForm.Button4.Font = New Font(sNombreFuente, iTamanioFuente)
        fForm.Button5.Font = New Font(sNombreFuente, iTamanioFuente)
        fForm.Button6.Font = New Font(sNombreFuente, iTamanioFuente)
        fForm.Button7.Font = New Font(sNombreFuente, iTamanioFuente)
        fForm.Label1.Font = New Font(sNombreFuente, iTamanioFuente)
        fForm.HhCharacterEntry1.LongitudTexto = iLongitud
        fForm.Label1.Text = RutaCompleta().Replace("&", "&&")

        fForm.Inicializar()
        cSeleccion = New Collection

        If fForm.ShowDialog() = DialogResult.OK Then
            cSeleccion = fForm.c
        Else
            cSeleccion.Clear()
        End If

    End Sub

End Class
