Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' La página de inicio redirige según el estado de la sesión.
        If SesionHelper.HaySesion(Session) Then
            Response.Redirect("~/Clientes.aspx")
        Else
            Response.Redirect("~/Login.aspx")
        End If
    End Sub
End Class