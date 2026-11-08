<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ingresar.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.Ingresar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row justify-content-center">
            <div class="col-md-5">

                <h2 class="text-center mb-4">Ingresar con Usuario</h2>

                <!-- Correo electrónico -->
                <div class="mb-3">
                    <asp:Label
                        ID="lblEmail"
                        runat="server"
                        Text="Correo electrónico"
                        AssociatedControlID="txtEmail">
                </asp:Label>

                    <asp:TextBox
                        ID="txtEmail"
                        runat="server"
                        TextMode="Email"
                        CssClass="form-control">
                </asp:TextBox>
                </div>

                <!-- Contraseña -->
                <div class="mb-3">
                    <asp:Label
                        ID="lblPassword"
                        runat="server"
                        Text="Contraseña"
                        AssociatedControlID="txtPassword">
                </asp:Label>

                    <asp:TextBox
                        ID="txtPassword"
                        runat="server"
                        TextMode="Password"
                        CssClass="form-control">
                </asp:TextBox>

                    <small class="form-text text-muted">La contraseña predeterminada es tu número de teléfono.
</small>
                </div>

                <!-- Botones -->
                <div class="d-flex gap-2">
                    <asp:Button
                        ID="btnIngresar"
                        runat="server"
                        Text="Ingresar"
                        CssClass="btn btn-primary" />

                    <asp:Button
                        ID="btnCancelar"
                        runat="server"
                        Text="Cancelar"
                        CssClass="btn btn-secondary"
                        CausesValidation="false" />

                    <asp:Button
                        ID="btnAtras"
                        runat="server"
                        Text="Atrás"
                        CssClass="btn btn-outline-dark"
                        CausesValidation="false" />
                </div>

            </div>
        </div>
    </div>
</asp:Content>
