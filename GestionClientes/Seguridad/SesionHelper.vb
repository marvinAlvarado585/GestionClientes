Imports System.Web
Imports System.Web.SessionState

''' <summary>
''' Centraliza el manejo de la sesión del usuario autenticado.
''' </summary>
Public Class SesionHelper

    Public Const ClaveUsuario As String = "Usuario"

    ''' <summary>
    ''' Indica si existe una sesión de usuario activa.
    ''' </summary>
    Public Shared Function HaySesion(ByVal session As HttpSessionState) As Boolean
        Return session IsNot Nothing AndAlso session(ClaveUsuario) IsNot Nothing
    End Function

    ''' <summary>
    ''' Devuelve el nombre del usuario en sesión, o cadena vacía si no hay sesión.
    ''' </summary>
    Public Shared Function UsuarioActual(ByVal session As HttpSessionState) As String
        If HaySesion(session) Then
            Return session(ClaveUsuario).ToString()
        End If
        Return String.Empty
    End Function

    ''' <summary>
    ''' Redirige al login si no hay sesión. Llamar al inicio de las páginas protegidas.
    ''' </summary>
    Public Shared Sub RequerirSesion(ByVal contexto As HttpContext)
        If Not HaySesion(contexto.Session) Then
            contexto.Response.Redirect("~/Vistas/Login.aspx")
        End If
    End Sub

End Class
