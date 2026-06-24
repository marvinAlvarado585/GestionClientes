Public Class Bitacora
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Página protegida: exige sesión activa.
        SesionHelper.RequerirSesion(Context)

        If Not IsPostBack Then
            gvBitacora.DataSource = BitacoraBLL.Listar()
            gvBitacora.DataBind()
        End If
    End Sub

    ''' <summary>
    ''' En cada render el GridView debe emitir &lt;thead&gt; para DataTables.
    ''' </summary>
    Protected Sub gvBitacora_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles gvBitacora.PreRender
        If gvBitacora.HeaderRow IsNot Nothing Then
            gvBitacora.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub

End Class
