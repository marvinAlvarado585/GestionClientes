Public Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Cierra la sesión y vuelve al login.
        Session.Clear()
        Session.Abandon()
        Response.Redirect("~/Login.aspx")
    End Sub

End Class
