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
    ''' Inserta un registro en la bitácora.
    ''' </summary>
    Public Shared Sub RegistrarAccion(ByVal accion As String,
                                      ByVal clienteId As Integer,
                                      ByVal nombreUsuario As String,
                                      ByVal detalle As String)
        Const sql As String =
            "INSERT INTO Bitacora (Accion, ClienteId, NombreUsuario, Detalle) " &
            "VALUES (@Accion, @ClienteId, @NombreUsuario, @Detalle);"

        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("@Accion", accion)
                cmd.Parameters.AddWithValue("@ClienteId", clienteId)
                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario)
                cmd.Parameters.AddWithValue("@Detalle", If(String.IsNullOrEmpty(detalle), CObj(DBNull.Value), detalle))
                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Devuelve todos los registros de la bitácora, del más reciente al más antiguo.
    ''' </summary>
    Public Shared Function Listar() As DataTable
        Const sql As String =
            "SELECT Id, Accion, ClienteId, NombreUsuario, FechaHora, Detalle " &
            "FROM Bitacora ORDER BY FechaHora DESC, Id DESC;"

        Dim tabla As New DataTable()
        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using da As New SqlDataAdapter(sql, cn)
                da.Fill(tabla)
            End Using
        End Using
        Return tabla
    End Function

End Class
