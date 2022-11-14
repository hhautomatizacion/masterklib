Public Class Receta
    Public Structure Receta
        Public Nombre As String
        Public Descripcion As String
        Public Adjunto As String
        Public Pasos As Collection
    End Structure
    Public Structure Paso
        Public NombrePaso As String
        Public IdPaso As Integer
        Public Minutos As Integer
        Public Segundos As Integer
        Public Litros As Integer
        Public Centigrados As Integer
        Public RPM As Integer
        Public ParametroAuxiliar As Integer
        Public Argumentos As Integer
        Public Emergente As String
    End Structure
End Class
