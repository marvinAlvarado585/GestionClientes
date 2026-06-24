''' <summary>
''' Representa un registro de la bitácora (tabla Bitacora): un cambio realizado
''' sobre un cliente. Se llama RegistroBitacora para no chocar con la página Bitacora.
''' </summary>
Public Class RegistroBitacora
    Public Property Id As Integer
    Public Property Accion As String        ' AGREGAR / EDITAR / ELIMINAR
    Public Property ClienteId As Integer
    Public Property NombreUsuario As String
    Public Property FechaHora As DateTime
    Public Property Detalle As String
End Class
