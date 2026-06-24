<%@ Page Title="Iniciar sesión" Language="VB" AutoEventWireup="true" CodeBehind="Login.aspx.vb" Inherits="GestionClientes.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Iniciar sesión - Gestión de Clientes</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" runat="server" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container">
            <div class="row justify-content-center" style="margin-top: 8%;">
                <div class="col-md-5 col-lg-4">
                    <div class="card shadow-sm">
                        <div class="card-body p-4">
                            <h3 class="card-title text-center mb-1">Gestión de Clientes</h3>
                            <p class="text-center text-muted mb-4">Inicie sesión para continuar</p>

                            <div class="mb-3">
                                <label class="form-label" for="<%= txtUsuario.ClientID %>">Usuario</label>
                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Usuario" />
                                <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario"
                                    ErrorMessage="Ingrese el usuario." CssClass="text-danger small" Display="Dynamic"
                                    ValidationGroup="Login" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label" for="<%= txtPassword.ClientID %>">Contraseña</label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Contraseña" />
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="Ingrese la contraseña." CssClass="text-danger small" Display="Dynamic"
                                    ValidationGroup="Login" />
                            </div>

                            <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger d-block mb-3" />

                            <div class="d-grid">
                                <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CssClass="btn btn-primary"
                                    ValidationGroup="Login" OnClick="btnIngresar_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
