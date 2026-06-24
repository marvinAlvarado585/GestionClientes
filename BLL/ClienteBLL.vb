Imports System.Collections.Generic

''' <summary>
''' Capa de lógica de negocio para clientes.
''' Orquesta el acceso a datos y aplica la regla del negocio:
''' TODA alta, edición o eliminación de un cliente debe quedar registrada en la bitácora.
''' La presentación llama a esta capa, nunca al DAL directamente.
''' </summary>
Public Class ClienteBLL

    ''' <summary>
    ''' Lista todos los clientes.
    ''' </summary>
    Public Shared Function Listar() As List(Of Cliente)
        Return ClienteDAL.Listar()
    End Function

    ''' <summary>
    ''' Obtiene un cliente por su Id (Nothing si no existe).
    ''' </summary>
    Public Shared Function ObtenerPorId(ByVal id As Integer) As Cliente
        Return ClienteDAL.ObtenerPorId(id)
    End Function

    ''' <summary>
    ''' Guarda un cliente: inserta si es nuevo (Id = 0) o actualiza si ya existe,
    ''' y registra la acción correspondiente en la bitácora.
    ''' </summary>
    Public Shared Sub Guardar(ByVal c As Cliente, ByVal usuario As String)
        If c.Id = 0 Then
            Dim nuevoId As Integer = ClienteDAL.Insertar(c)
            BitacoraDAL.RegistrarAccion(BitacoraDAL.AccionAgregar, nuevoId, usuario,
                                        "Se agregó el cliente: " & c.Nombre & " " & c.Apellido)
        Else
            ClienteDAL.Actualizar(c)
            BitacoraDAL.RegistrarAccion(BitacoraDAL.AccionEditar, c.Id, usuario,
                                        "Se editó el cliente: " & c.Nombre & " " & c.Apellido)
        End If
    End Sub

    ''' <summary>
    ''' Elimina un cliente y registra la acción en la bitácora.
    ''' </summary>
    Public Shared Sub Eliminar(ByVal id As Integer, ByVal usuario As String)
        ClienteDAL.Eliminar(id)
        BitacoraDAL.RegistrarAccion(BitacoraDAL.AccionEliminar, id, usuario,
                                    "Se eliminó el cliente con Id " & id.ToString())
    End Sub

End Class
