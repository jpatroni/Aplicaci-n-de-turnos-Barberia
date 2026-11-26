<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Ingresar.aspx.cs"
    Inherits="Aplicación_de_turnos_Barberia.Ingresar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Content/login.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="login-pagina">
      <div class="login-distribucion">
          <aside class="login-presentacion">
    <h2>Tu lugar, cuando lo necesitás</h2>
    <p>Elegí tu servicio, reservá tu turno y disfrutá tu próxima visita.</p>
</aside>

  <div class="login-formulario">

                <h2 class="text-center mb-4">Ingresar con Usuario</h2>

                <!-- Correo electrónico -->
                <div class="mb-3">
                    <asp:Label
                        ID="lblEmail"
                        runat="server"
                        Text="Correo electrónico"
                        AssociatedControlID="txtEmail" />

                    <asp:TextBox
                        ID="txtEmail"
                        runat="server"
                        TextMode="Email"
                        CssClass="form-control" />
                </div>

                <!-- Contraseña -->
                <div class="mb-3">
                    <asp:Label
                        ID="lblPassword"
                        runat="server"
                        Text="Contraseña"
                        AssociatedControlID="txtPassword" />

                    <asp:TextBox
                        ID="txtPassword"
                        runat="server"
                        TextMode="Password"
                        CssClass="form-control" />

                    <small class="form-text text-muted">
                        Usá la contraseña que elegiste al registrarte.
                    </small>
                </div>

                <!-- Botones -->
                <div class="d-flex gap-2">
                    <asp:Button
                        ID="btnIngresar"
                        runat="server"
                        Text="Ingresar"
                        CssClass="btn btn-primary"
                        OnClick="btnIngresar_Click" />

                    <asp:Label
                        ID="lblErrorIngreso"
                        runat="server"
                        CssClass="text-danger" />

                    <asp:Button
                        ID="btnCancelar"
                        runat="server"
                        Text="Cancelar"
                        OnClick="btnVolver_Click"
                        CssClass="btn btn-secondary"
                        CausesValidation="false" />

                    <asp:Button
                        ID="btnAtras"
                        runat="server"
                        Text="Atrás"
                        OnClick="btnVolver_Click"
                        CssClass="btn btn-outline-dark"
                        CausesValidation="false" />
                </div>

            </div>
        </div>
    </div>
</asp:Content>
