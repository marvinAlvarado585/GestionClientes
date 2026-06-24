<%@ Page Title="Usuarios" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.vb" Inherits="GestionClientes.Usuarios" %>

<asp:Content ID="ContenidoUsuarios" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="mt-3">Gestión de Usuarios</h2>

    <%-- Mensaje de resultado --%>
    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <div class="alert" runat="server" id="divMensaje">
            <asp:Label ID="lblMensaje" runat="server" />
        </div>
    </asp:Panel>

    <%-- ----------------------- Formulario de alta ----------------------- --%>
    <div class="card mb-4">
        <div class="card-header">Nuevo usuario</div>
        <div class="card-body">
            <div class="row g-3">
                <div class="col-md-6">
                    <label class="form-label">Usuario *</label>
                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" MaxLength="50" />
                    <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario"
                        ErrorMessage="El usuario es obligatorio." CssClass="text-danger small" Display="Dynamic"
                        ValidationGroup="Usuario" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Contraseña *</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="La contraseña es obligatoria." CssClass="text-danger small" Display="Dynamic"
                        ValidationGroup="Usuario" />
                    <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword"
                        ValidationExpression=".{6,}" ErrorMessage="Mínimo 6 caracteres." CssClass="text-danger small"
                        Display="Dynamic" ValidationGroup="Usuario" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Confirmar contraseña *</label>
                    <asp:TextBox ID="txtConfirmar" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100" />
                    <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmar"
                        ControlToCompare="txtPassword" ErrorMessage="Las contraseñas no coinciden."
                        CssClass="text-danger small" Display="Dynamic" ValidationGroup="Usuario" />
                </div>
            </div>
            <div class="mt-3">
                <asp:Button ID="btnCrear" runat="server" Text="Crear usuario" CssClass="btn btn-primary"
                    ValidationGroup="Usuario" OnClick="btnCrear_Click" />
            </div>
        </div>
    </div>

    <%-- ----------------------- Listado de usuarios ----------------------- --%>
    <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="false"
        CssClass="table table-striped table-bordered table-hover"
        EmptyDataText="No hay usuarios registrados.">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" />
            <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
            <asp:BoundField DataField="FechaCreacion" HeaderText="Fecha de creación" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
        </Columns>
    </asp:GridView>

</asp:Content>
