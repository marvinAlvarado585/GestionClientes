Imports System.Security.Cryptography

''' <summary>
''' Utilidades de seguridad para el manejo de contraseñas.
''' Las contraseñas se protegen con PBKDF2 (HMAC-SHA256) y un salt aleatorio
''' único por usuario. Nunca se almacena ni se compara la contraseña en texto plano.
''' </summary>
Public Class SeguridadHelper

    ' Parámetros del algoritmo. Deben coincidir con los usados al sembrar el
    ' usuario admin en el script SQL.
    Private Const Iteraciones As Integer = 100000
    Private Const TamanoSalt As Integer = 16   ' bytes
    Private Const TamanoHash As Integer = 32   ' bytes

    ''' <summary>
    ''' Genera un salt aleatorio y devuelve su representación en Base64.
    ''' </summary>
    Public Shared Function GenerarSalt() As String
        Dim salt(TamanoSalt - 1) As Byte
        Using rng As RandomNumberGenerator = RandomNumberGenerator.Create()
            rng.GetBytes(salt)
        End Using
        Return Convert.ToBase64String(salt)
    End Function

    ''' <summary>
    ''' Calcula el hash PBKDF2 (Base64) de una contraseña usando el salt indicado (Base64).
    ''' </summary>
    Public Shared Function CalcularHash(ByVal password As String, ByVal saltBase64 As String) As String
        Dim salt As Byte() = Convert.FromBase64String(saltBase64)
        Using pbkdf2 As New Rfc2898DeriveBytes(password, salt, Iteraciones, HashAlgorithmName.SHA256)
            Dim hash As Byte() = pbkdf2.GetBytes(TamanoHash)
            Return Convert.ToBase64String(hash)
        End Using
    End Function

    ''' <summary>
    ''' Genera salt + hash para una contraseña nueva (alta de usuario).
    ''' </summary>
    Public Shared Sub GenerarCredenciales(ByVal password As String, ByRef hashBase64 As String, ByRef saltBase64 As String)
        saltBase64 = GenerarSalt()
        hashBase64 = CalcularHash(password, saltBase64)
    End Sub

    ''' <summary>
    ''' Verifica una contraseña contra el hash y salt almacenados.
    ''' Usa comparación en tiempo fijo para evitar ataques de temporización.
    ''' </summary>
    Public Shared Function VerificarPassword(ByVal password As String, ByVal hashBase64 As String, ByVal saltBase64 As String) As Boolean
        If String.IsNullOrEmpty(password) OrElse String.IsNullOrEmpty(hashBase64) OrElse String.IsNullOrEmpty(saltBase64) Then
            Return False
        End If

        Dim hashCalculado As String = CalcularHash(password, saltBase64)
        Return ComparacionSegura(hashCalculado, hashBase64)
    End Function

    ''' <summary>
    ''' Comparación de cadenas en tiempo constante.
    ''' </summary>
    Private Shared Function ComparacionSegura(ByVal a As String, ByVal b As String) As Boolean
        If a Is Nothing OrElse b Is Nothing OrElse a.Length <> b.Length Then
            Return False
        End If

        Dim diferencia As Integer = 0
        For i As Integer = 0 To a.Length - 1
            diferencia = diferencia Or (Asc(a(i)) Xor Asc(b(i)))
        Next
        Return diferencia = 0
    End Function

End Class
