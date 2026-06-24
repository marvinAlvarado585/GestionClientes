Public Class Bitacora
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Página protegida: exige sesión activa.
        SesionHelper.RequerirSesion(Context)

        If Not IsPostBack Then
            gvBitacora.DataSource = BitacoraBLL.Listar()
            gvBitacora.DataBind()
            ' El GridView debe emitir <thead> para que DataTables funcione.
            If gvBitacora.HeaderRow IsNot Nothing Then
                gvBitacora.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        End If
    End Sub

End Class
