''' <summary>
''' Representa un usuario del sistema (tabla Usuarios).
''' La contraseña se guarda como hash + salt, nunca en texto plano.
''' </summary>
Public Class Usuario
    Public Property Id As Integer
    Public Property NombreUsuario As String
    Public Property PasswordHash As String
    Public Property PasswordSalt As String
    Public Property FechaCreacion As DateTime
    Public Property Activo As Boolean
End Class
