''' <summary>
''' Lógica de negocio de autenticación. El DAL solo trae el usuario;
''' aquí se aplica la regla de negocio: verificar la contraseña contra el hash + salt.
''' </summary>
Public Class SeguridadBLL

    ''' <summary>
    ''' Autentica a un usuario. Devuelve True si el usuario existe, está activo
    ''' y la contraseña coincide con el hash almacenado.
    ''' </summary>
    Public Shared Function Autenticar(ByVal nombreUsuario As String, ByVal password As String) As Boolean
        If String.IsNullOrWhiteSpace(nombreUsuario) OrElse String.IsNullOrWhiteSpace(password) Then
            Return False
        End If

        Dim usuario As Usuario = UsuarioDAL.ObtenerPorNombre(nombreUsuario)
        If usuario Is Nothing Then
            Return False
        End If

        Return SeguridadHelper.VerificarPassword(password, usuario.PasswordHash, usuario.PasswordSalt)
    End Function

End Class
