Public Class SiteMaster
    Inherits MasterPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Muestra el usuario autenticado en la barra de navegación.
        Dim usuario As String = SesionHelper.UsuarioActual(Session)
        If usuario <> String.Empty Then
            lblUsuario.Text = "Usuario: " & usuario
        End If
    End Sub
End Class