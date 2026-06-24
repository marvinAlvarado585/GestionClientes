Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' Acceso a datos de la tabla Usuarios. Toda la lógica SQL está en
''' procedimientos almacenados; aquí solo se invocan.
''' </summary>
Public Class UsuarioDAL

    ''' <summary>
    ''' Obtiene un usuario activo por su nombre, o Nothing si no existe / está inactivo.
    ''' </summary>
    Public Shared Function ObtenerPorNombre(ByVal nombreUsuario As String) As Usuario
        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand("usp_Usuarios_ObtenerPorNombre", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario.Trim())
                cn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Return New Usuario With {
                            .Id = Convert.ToInt32(reader("Id")),
                            .NombreUsuario = reader("NombreUsuario").ToString(),
                            .PasswordHash = reader("PasswordHash").ToString(),
                            .PasswordSalt = reader("PasswordSalt").ToString(),
                            .FechaCreacion = Convert.ToDateTime(reader("FechaCreacion")),
                            .Activo = Convert.ToBoolean(reader("Activo"))
                        }
                    End If
                End Using
            End Using
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Indica si ya existe un usuario con ese nombre (sin importar si está activo).
    ''' </summary>
    Public Shared Function ExisteNombre(ByVal nombreUsuario As String) As Boolean
        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand("usp_Usuarios_ExisteNombre", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario.Trim())
                cn.Open()
                Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Inserta un nuevo usuario y devuelve el Id generado.
    ''' </summary>
    Public Shared Function Insertar(ByVal u As Usuario) As Integer
        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand("usp_Usuarios_Insertar", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@NombreUsuario", u.NombreUsuario.Trim())
                cmd.Parameters.AddWithValue("@PasswordHash", u.PasswordHash)
                cmd.Parameters.AddWithValue("@PasswordSalt", u.PasswordSalt)
                cn.Open()
                Return Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Lista los usuarios (sin exponer el hash ni el salt).
    ''' </summary>
    Public Shared Function Listar() As List(Of Usuario)
        Dim lista As New List(Of Usuario)()
        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand("usp_Usuarios_Listar", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(New Usuario With {
                            .Id = Convert.ToInt32(reader("Id")),
                            .NombreUsuario = reader("NombreUsuario").ToString(),
                            .FechaCreacion = Convert.ToDateTime(reader("FechaCreacion")),
                            .Activo = Convert.ToBoolean(reader("Activo"))
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

End Class
