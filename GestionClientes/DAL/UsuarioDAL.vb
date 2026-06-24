Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' Acceso a datos de la tabla Usuarios (consultas parametrizadas).
''' </summary>
Public Class UsuarioDAL

    ''' <summary>
    ''' Obtiene un usuario activo por su nombre, o Nothing si no existe / está inactivo.
    ''' Consulta parametrizada -> inmune a inyección SQL.
    ''' </summary>
    Public Shared Function ObtenerPorNombre(ByVal nombreUsuario As String) As Usuario
        Const sql As String =
            "SELECT Id, NombreUsuario, PasswordHash, PasswordSalt, FechaCreacion, Activo " &
            "FROM Usuarios WHERE NombreUsuario = @NombreUsuario AND Activo = 1;"

        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand(sql, cn)
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
        Const sql As String = "SELECT COUNT(1) FROM Usuarios WHERE NombreUsuario = @NombreUsuario;"

        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand(sql, cn)
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
        Const sql As String =
            "INSERT INTO Usuarios (NombreUsuario, PasswordHash, PasswordSalt) " &
            "VALUES (@NombreUsuario, @PasswordHash, @PasswordSalt); " &
            "SELECT CAST(SCOPE_IDENTITY() AS INT);"

        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand(sql, cn)
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
        Const sql As String =
            "SELECT Id, NombreUsuario, FechaCreacion, Activo FROM Usuarios ORDER BY Id;"

        Dim lista As New List(Of Usuario)()
        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand(sql, cn)
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
