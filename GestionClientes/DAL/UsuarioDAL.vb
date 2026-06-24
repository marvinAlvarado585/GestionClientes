Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' Acceso a datos de la tabla Usuarios. Valida las credenciales de login
''' comparando la contraseña ingresada contra el hash/salt almacenados.
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
    ''' Valida usuario y contraseña. Devuelve True si las credenciales son correctas.
    ''' </summary>
    Public Shared Function ValidarUsuario(ByVal nombreUsuario As String, ByVal password As String) As Boolean
        If String.IsNullOrWhiteSpace(nombreUsuario) OrElse String.IsNullOrWhiteSpace(password) Then
            Return False
        End If

        Dim usuario As Usuario = ObtenerPorNombre(nombreUsuario)
        If usuario Is Nothing Then
            Return False
        End If

        Return SeguridadHelper.VerificarPassword(password, usuario.PasswordHash, usuario.PasswordSalt)
    End Function

End Class
