Imports System.Collections.Generic

''' <summary>
''' Lógica de negocio para la gestión de usuarios.
''' Aplica las reglas: no permitir nombres duplicados y guardar la contraseña
''' siempre como hash + salt (nunca en texto plano).
''' </summary>
Public Class UsuarioBLL

    ''' <summary>
    ''' Lista todos los usuarios (sin datos sensibles).
    ''' </summary>
    Public Shared Function Listar() As List(Of Usuario)
        Return UsuarioDAL.Listar()
    End Function

    ''' <summary>
    ''' Registra un nuevo usuario. Devuelve True si se creó, o False si el
    ''' nombre de usuario ya existe.
    ''' </summary>
    Public Shared Function Registrar(ByVal nombreUsuario As String, ByVal password As String) As Boolean
        nombreUsuario = nombreUsuario.Trim()

        ' Regla: no se permiten nombres de usuario duplicados.
        If UsuarioDAL.ExisteNombre(nombreUsuario) Then
            Return False
        End If

        ' Regla: la contraseña se almacena hasheada (PBKDF2 + salt).
        Dim hash As String = Nothing
        Dim salt As String = Nothing
        SeguridadHelper.GenerarCredenciales(password, hash, salt)

        UsuarioDAL.Insertar(New Usuario With {
            .NombreUsuario = nombreUsuario,
            .PasswordHash = hash,
            .PasswordSalt = salt
        })
        Return True
    End Function

End Class
