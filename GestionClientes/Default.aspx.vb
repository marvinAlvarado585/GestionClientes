Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' La página de inicio redirige según el estado de la sesión.
        If SesionHelper.HaySesion(Session) Then
            Response.Redirect("~/Vistas/Clientes.aspx")
        Else
            Response.Redirect("~/Vistas/Login.aspx")
        End If
    End Sub
End Class