Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' Acceso a datos de la tabla Bitacora. Registra y consulta las acciones
''' realizadas sobre los clientes (agregar, editar, eliminar).
''' Todas las consultas son parametrizadas para evitar inyección SQL.
''' </summary>
Public Class BitacoraDAL

    ' Constantes de acción para evitar errores de tipeo.
    Public Const AccionAgregar As String = "AGREGAR"
    Public Const AccionEditar As String = "EDITAR"
    Public Const AccionEliminar As String = "ELIMINAR"

    ''' <summary>
    ''' Registra una acción en la bitácora (sobrecarga práctica con parámetros sueltos).
    ''' </summary>
    Public Shared Sub RegistrarAccion(ByVal accion As String,
                                      ByVal clienteId As Integer,
                                      ByVal nombreUsuario As String,
                                      ByVal detalle As String)
        Insertar(New RegistroBitacora With {
            .Accion = accion,
            .ClienteId = clienteId,
            .NombreUsuario = nombreUsuario,
            .Detalle = detalle
        })
    End Sub

    ''' <summary>
    ''' Inserta un registro de bitácora a partir del modelo.
    ''' </summary>
    Public Shared Sub Insertar(ByVal registro As RegistroBitacora)
        Const sql As String =
            "INSERT INTO Bitacora (Accion, ClienteId, NombreUsuario, Detalle) " &
            "VALUES (@Accion, @ClienteId, @NombreUsuario, @Detalle);"

        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("@Accion", registro.Accion)
                cmd.Parameters.AddWithValue("@ClienteId", registro.ClienteId)
                cmd.Parameters.AddWithValue("@NombreUsuario", registro.NombreUsuario)
                cmd.Parameters.AddWithValue("@Detalle", If(String.IsNullOrEmpty(registro.Detalle), CObj(DBNull.Value), registro.Detalle))
                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Devuelve todos los registros de la bitácora, del más reciente al más antiguo.
    ''' </summary>
    Public Shared Function Listar() As List(Of RegistroBitacora)
        Const sql As String =
            "SELECT Id, Accion, ClienteId, NombreUsuario, FechaHora, Detalle " &
            "FROM Bitacora ORDER BY FechaHora DESC, Id DESC;"

        Dim lista As New List(Of RegistroBitacora)()
        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand(sql, cn)
                cn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(New RegistroBitacora With {
                            .Id = Convert.ToInt32(reader("Id")),
                            .Accion = reader("Accion").ToString(),
                            .ClienteId = If(IsDBNull(reader("ClienteId")), 0, Convert.ToInt32(reader("ClienteId"))),
                            .NombreUsuario = reader("NombreUsuario").ToString(),
                            .FechaHora = Convert.ToDateTime(reader("FechaHora")),
                            .Detalle = reader("Detalle").ToString()
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

End Class
