Imports System.Configuration
Imports System.Data.SqlClient

''' <summary>
''' Punto único de acceso a la cadena de conexión definida en Web.config.
''' Centralizar la conexión evita repetir la cadena y facilita el mantenimiento.
''' </summary>
Public Class ConexionBD

    Private Const NombreConexion As String = "GestionClientesDB"

    ''' <summary>
    ''' Devuelve la cadena de conexión configurada en Web.config.
    ''' </summary>
    Public Shared ReadOnly Property CadenaConexion As String
        Get
            Return ConfigurationManager.ConnectionStrings(NombreConexion).ConnectionString
        End Get
    End Property

    ''' <summary>
    ''' Crea una nueva conexión SQL (sin abrir). El llamador es responsable de
    ''' abrirla y cerrarla, idealmente con un bloque Using.
    ''' </summary>
    Public Shared Function ObtenerConexion() As SqlConnection
        Return New SqlConnection(CadenaConexion)
    End Function

End Class
