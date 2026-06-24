Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Si ya hay sesión activa, ir directo a la pantalla principal.
        If Not IsPostBack AndAlso SesionHelper.HaySesion(Session) Then
            Response.Redirect("~/Clientes.aspx")
        End If
    End Sub

    Protected Sub btnIngresar_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Not Page.IsValid Then
            Return
        End If

        Try
            Dim usuario As String = txtUsuario.Text.Trim()

            If UsuarioDAL.ValidarUsuario(usuario, txtPassword.Text) Then
                ' Credenciales válidas: se crea la sesión.
                Session("Usuario") = usuario
                Response.Redirect("~/Clientes.aspx")
            Else
                lblMensaje.Text = "Usuario o contraseña incorrectos."
            End If
        Catch ex As Exception
            lblMensaje.Text = "Ocurrió un error al iniciar sesión. Intente nuevamente."
        End Try
    End Sub

End Class
