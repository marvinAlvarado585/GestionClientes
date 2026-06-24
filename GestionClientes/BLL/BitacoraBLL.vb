Imports System.Collections.Generic

''' <summary>
''' Lógica de negocio de la bitácora. Por ahora expone la consulta del historial;
''' el registro de acciones lo orquesta ClienteBLL al modificar clientes.
''' </summary>
Public Class BitacoraBLL

    ''' <summary>
    ''' Devuelve el historial de la bitácora (más reciente primero).
    ''' </summary>
    Public Shared Function Listar() As List(Of RegistroBitacora)
        Return BitacoraDAL.Listar()
    End Function

End Class
