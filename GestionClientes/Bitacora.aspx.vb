Public Class Bitacora
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Página protegida: exige sesión activa.
        SesionHelper.RequerirSesion(Context)

        If Not IsPostBack Then
            gvBitacora.DataSource = BitacoraDAL.Listar()
            gvBitacora.DataBind()
        End If
    End Sub

End Class
