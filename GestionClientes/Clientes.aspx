<%@ Page Title="Clientes" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.vb" Inherits="GestionClientes.Clientes" %>

<asp:Content ID="ContenidoClientes" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="mt-3">Gestión de Clientes</h2>

    <%-- Mensaje de resultado (éxito / error) --%>
    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <div class="alert" runat="server" id="divMensaje">
            <asp:Label ID="lblMensaje" runat="server" />
        </div>
    </asp:Panel>

    <%-- ----------------------- Formulario de alta/edición ----------------------- --%>
    <div class="card mb-4">
        <div class="card-header">
            <asp:Label ID="lblTituloForm" runat="server" Text="Agregar cliente" />
        </div>
        <div class="card-body">
            <asp:HiddenField ID="hfId" runat="server" Value="0" />
            <div class="row g-3">
                <div class="col-md-6">
                    <label class="form-label">Nombre *</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                        ErrorMessage="El nombre es obligatorio." CssClass="text-danger small" Display="Dynamic"
                        ValidationGroup="Cliente" />
                </div>
                <div class="col-md-6">
                    <label class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" MaxLength="100" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Identificación</label>
                    <asp:TextBox ID="txtIdentificacion" runat="server" CssClass="form-control" MaxLength="30" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" MaxLength="120" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Email no válido."
                        CssClass="text-danger small" Display="Dynamic" ValidationGroup="Cliente" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="30" />
                </div>
                <div class="col-md-12">
                    <label class="form-label">Dirección</label>
                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" MaxLength="250" />
                </div>
            </div>
            <div class="mt-3">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary"
                    ValidationGroup="Cliente" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary"
                    CausesValidation="false" OnClick="btnCancelar_Click" />
            </div>
        </div>
    </div>

    <%-- ----------------------- Listado de clientes ----------------------- --%>
    <asp:GridView ID="gvClientes" runat="server" AutoGenerateColumns="false"
        CssClass="table table-striped table-bordered table-hover" DataKeyNames="Id"
        OnRowCommand="gvClientes_RowCommand" EmptyDataText="No hay clientes registrados.">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
            <asp:BoundField DataField="Identificacion" HeaderText="Identificación" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
            <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-outline-primary"
                        CommandName="Editar" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Text="Editar" />
                    <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-outline-danger"
                        CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Text="Eliminar"
                        OnClientClick="return confirm('¿Está seguro de eliminar este cliente?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
