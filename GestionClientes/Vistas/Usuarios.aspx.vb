Public Class Usuarios
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Página protegida: exige sesión activa.
        SesionHelper.RequerirSesion(Context)

        If Not IsPostBack Then
            CargarUsuarios()
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de usuarios en el GridView.
    ''' </summary>
    Private Sub CargarUsuarios()
        gvUsuarios.DataSource = UsuarioBLL.Listar()
        gvUsuarios.DataBind()
        ' El GridView debe emitir <thead> para que DataTables funcione.
        If gvUsuarios.HeaderRow IsNot Nothing Then
            gvUsuarios.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub

    ''' <summary>
    ''' Crea un nuevo usuario a través de la capa de negocio.
    ''' </summary>
    Protected Sub btnCrear_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Not Page.IsValid Then
            Return
        End If

        Try
            If UsuarioBLL.Registrar(txtUsuario.Text, txtPassword.Text) Then
                MostrarMensaje("Usuario creado correctamente.", True)
                LimpiarFormulario()
                CargarUsuarios()
            Else
                MostrarMensaje("Ya existe un usuario con ese nombre.", False)
            End If
        Catch ex As Exception
            MostrarMensaje("Ocurrió un error al crear el usuario.", False)
        End Try
    End Sub

    Private Sub LimpiarFormulario()
        txtUsuario.Text = String.Empty
        txtPassword.Text = String.Empty
        txtConfirmar.Text = String.Empty
    End Sub

    Private Sub MostrarMensaje(ByVal texto As String, ByVal exito As Boolean)
        lblMensaje.Text = texto
        divMensaje.Attributes("class") = If(exito, "alert alert-success", "alert alert-danger")
        pnlMensaje.Visible = True
    End Sub

End Class
