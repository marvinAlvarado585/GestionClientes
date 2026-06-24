Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' Acceso a datos de la tabla Clientes (CRUD completo).
''' Toda la lógica SQL está en procedimientos almacenados; aquí solo se invocan.
''' Devuelve objetos del modelo Cliente.
''' </summary>
Public Class ClienteDAL

    ''' <summary>
    ''' Devuelve todos los clientes ordenados por Id.
    ''' </summary>
    Public Shared Function Listar() As List(Of Cliente)
        Dim lista As New List(Of Cliente)()
        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand("usp_Clientes_Listar", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(MapearCliente(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    ''' <summary>
    ''' Obtiene un cliente por su Id, o Nothing si no existe.
    ''' </summary>
    Public Shared Function ObtenerPorId(ByVal id As Integer) As Cliente
        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand("usp_Clientes_ObtenerPorId", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Id", id)
                cn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Return MapearCliente(reader)
                    End If
                End Using
            End Using
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Inserta un nuevo cliente y devuelve el Id generado.
    ''' </summary>
    Public Shared Function Insertar(ByVal c As Cliente) As Integer
        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand("usp_Clientes_Insertar", cn)
                cmd.CommandType = CommandType.StoredProcedure
                AgregarParametros(cmd, c)
                cn.Open()
                Return Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Actualiza los datos de un cliente existente.
    ''' </summary>
    Public Shared Sub Actualizar(ByVal c As Cliente)
        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand("usp_Clientes_Actualizar", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Id", c.Id)
                AgregarParametros(cmd, c)
                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Elimina un cliente por su Id.
    ''' </summary>
    Public Shared Sub Eliminar(ByVal id As Integer)
        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand("usp_Clientes_Eliminar", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Id", id)
                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ' ----------------------- Helpers privados -----------------------

    Private Shared Sub AgregarParametros(ByVal cmd As SqlCommand, ByVal c As Cliente)
        cmd.Parameters.AddWithValue("@Nombre", c.Nombre)
        cmd.Parameters.AddWithValue("@Apellido", ValorONulo(c.Apellido))
        cmd.Parameters.AddWithValue("@Identificacion", ValorONulo(c.Identificacion))
        cmd.Parameters.AddWithValue("@Email", ValorONulo(c.Email))
        cmd.Parameters.AddWithValue("@Telefono", ValorONulo(c.Telefono))
        cmd.Parameters.AddWithValue("@Direccion", ValorONulo(c.Direccion))
    End Sub

    Private Shared Function ValorONulo(ByVal valor As String) As Object
        If String.IsNullOrWhiteSpace(valor) Then
            Return DBNull.Value
        End If
        Return valor.Trim()
    End Function

    Private Shared Function MapearCliente(ByVal reader As SqlDataReader) As Cliente
        Return New Cliente With {
            .Id = Convert.ToInt32(reader("Id")),
            .Nombre = reader("Nombre").ToString(),
            .Apellido = reader("Apellido").ToString(),
            .Identificacion = reader("Identificacion").ToString(),
            .Email = reader("Email").ToString(),
            .Telefono = reader("Telefono").ToString(),
            .Direccion = reader("Direccion").ToString(),
            .FechaRegistro = Convert.ToDateTime(reader("FechaRegistro"))
        }
    End Function

End Class
