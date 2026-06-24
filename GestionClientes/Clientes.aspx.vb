Public Class Clientes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Página protegida: exige sesión activa.
        SesionHelper.RequerirSesion(Context)

        If Not IsPostBack Then
            CargarClientes()
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de clientes en el GridView.
    ''' </summary>
    Private Sub CargarClientes()
        gvClientes.DataSource = ClienteDAL.Listar()
        gvClientes.DataBind()
    End Sub

    ''' <summary>
    ''' Guarda (inserta o actualiza) el cliente y registra la acción en la bitácora.
    ''' </summary>
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Not Page.IsValid Then
            Return
        End If

        Try
            Dim c As New Cliente With {
                .Id = Convert.ToInt32(hfId.Value),
                .Nombre = txtNombre.Text.Trim(),
                .Apellido = txtApellido.Text.Trim(),
                .Identificacion = txtIdentificacion.Text.Trim(),
                .Email = txtEmail.Text.Trim(),
                .Telefono = txtTelefono.Text.Trim(),
                .Direccion = txtDireccion.Text.Trim()
            }

            Dim usuario As String = SesionHelper.UsuarioActual(Session)

            If c.Id = 0 Then
                ' ----- Alta -----
                Dim nuevoId As Integer = ClienteDAL.Insertar(c)
                BitacoraDAL.RegistrarAccion(BitacoraDAL.AccionAgregar, nuevoId, usuario,
                                            "Se agregó el cliente: " & c.Nombre & " " & c.Apellido)
                MostrarMensaje("Cliente agregado correctamente.", True)
            Else
                ' ----- Edición -----
                ClienteDAL.Actualizar(c)
                BitacoraDAL.RegistrarAccion(BitacoraDAL.AccionEditar, c.Id, usuario,
                                            "Se editó el cliente: " & c.Nombre & " " & c.Apellido)
                MostrarMensaje("Cliente actualizado correctamente.", True)
            End If

            LimpiarFormulario()
            CargarClientes()

        Catch ex As Exception
            MostrarMensaje("Ocurrió un error al guardar el cliente.", False)
        End Try
    End Sub

    ''' <summary>
    ''' Maneja los botones Editar / Eliminar de cada fila del GridView.
    ''' </summary>
    Protected Sub gvClientes_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Dim id As Integer = Convert.ToInt32(e.CommandArgument)

        Try
            If e.CommandName = "Editar" Then
                CargarFormulario(id)

            ElseIf e.CommandName = "Eliminar" Then
                Dim usuario As String = SesionHelper.UsuarioActual(Session)
                ClienteDAL.Eliminar(id)
                BitacoraDAL.RegistrarAccion(BitacoraDAL.AccionEliminar, id, usuario,
                                            "Se eliminó el cliente con Id " & id.ToString())
                LimpiarFormulario()
                CargarClientes()
                MostrarMensaje("Cliente eliminado correctamente.", True)
            End If

        Catch ex As Exception
            MostrarMensaje("Ocurrió un error al procesar la acción.", False)
        End Try
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs)
        LimpiarFormulario()
    End Sub

    ''' <summary>
    ''' Carga los datos de un cliente en el formulario para editarlo.
    ''' </summary>
    Private Sub CargarFormulario(ByVal id As Integer)
        Dim c As Cliente = ClienteDAL.ObtenerPorId(id)
        If c Is Nothing Then
            MostrarMensaje("El cliente ya no existe.", False)
            CargarClientes()
            Return
        End If

        hfId.Value = c.Id.ToString()
        txtNombre.Text = c.Nombre
        txtApellido.Text = c.Apellido
        txtIdentificacion.Text = c.Identificacion
        txtEmail.Text = c.Email
        txtTelefono.Text = c.Telefono
        txtDireccion.Text = c.Direccion
        lblTituloForm.Text = "Editar cliente (Id " & c.Id.ToString() & ")"
    End Sub

    ''' <summary>
    ''' Restaura el formulario a su estado inicial (modo "Agregar").
    ''' </summary>
    Private Sub LimpiarFormulario()
        hfId.Value = "0"
        txtNombre.Text = String.Empty
        txtApellido.Text = String.Empty
        txtIdentificacion.Text = String.Empty
        txtEmail.Text = String.Empty
        txtTelefono.Text = String.Empty
        txtDireccion.Text = String.Empty
        lblTituloForm.Text = "Agregar cliente"
    End Sub

    ''' <summary>
    ''' Muestra un mensaje de éxito o error con estilo Bootstrap.
    ''' </summary>
    Private Sub MostrarMensaje(ByVal texto As String, ByVal exito As Boolean)
        lblMensaje.Text = texto
        divMensaje.Attributes("class") = If(exito, "alert alert-success", "alert alert-danger")
        pnlMensaje.Visible = True
    End Sub

End Class
