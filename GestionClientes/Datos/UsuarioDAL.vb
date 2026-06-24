Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' Acceso a datos de la tabla Usuarios. Valida las credenciales de login
''' comparando la contraseña ingresada contra el hash/salt almacenados.
''' </summary>
Public Class UsuarioDAL

    ''' <summary>
    ''' Valida usuario y contraseña contra la base de datos.
    ''' Devuelve True si las credenciales son correctas y el usuario está activo.
    ''' Consulta parametrizada -> inmune a inyección SQL.
    ''' </summary>
    Public Shared Function ValidarUsuario(ByVal nombreUsuario As String, ByVal password As String) As Boolean
        If String.IsNullOrWhiteSpace(nombreUsuario) OrElse String.IsNullOrWhiteSpace(password) Then
            Return False
        End If

        Const sql As String =
            "SELECT PasswordHash, PasswordSalt FROM Usuarios " &
            "WHERE NombreUsuario = @NombreUsuario AND Activo = 1;"

        Dim hashAlmacenado As String = Nothing
        Dim saltAlmacenado As String = Nothing

        Using cn As SqlConnection = ConexionBD.ObtenerConexion()
            Using cmd As New SqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario.Trim())
                cn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        hashAlmacenado = reader("PasswordHash").ToString()
                        saltAlmacenado = reader("PasswordSalt").ToString()
                    End If
                End Using
            End Using
        End Using

        ' Usuario inexistente o inactivo.
        If hashAlmacenado Is Nothing Then
            Return False
        End If

        Return SeguridadHelper.VerificarPassword(password, hashAlmacenado, saltAlmacenado)
    End Function

End Class
