<%@ Page Title="Bitácora" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bitacora.aspx.vb" Inherits="GestionClientes.Bitacora" %>

<asp:Content ID="ContenidoBitacora" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="mt-3">Bitácora de acciones</h2>
    <p class="text-muted">Registro de todos los cambios realizados sobre los clientes.</p>

    <asp:GridView ID="gvBitacora" runat="server" AutoGenerateColumns="false"
        CssClass="table table-striped table-bordered table-hover"
        EmptyDataText="No hay registros en la bitácora.">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" />
            <asp:BoundField DataField="Accion" HeaderText="Acción" />
            <asp:BoundField DataField="ClienteId" HeaderText="Id Cliente" />
            <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
            <asp:BoundField DataField="FechaHora" HeaderText="Fecha y hora" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
            <asp:BoundField DataField="Detalle" HeaderText="Detalle" />
        </Columns>
    </asp:GridView>

</asp:Content>
