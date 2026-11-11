<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrateForm.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.RegistrateForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row" style="display: flex; justify-content: center;">

        <div class="col-4">
            <h1>Registrate</h1>

            <div class="form-field">
                <label for="<%= txtNombre.ClientID %>" class="form-label">Nombre</label>

                <asp:TextBox
                    runat="server" ID="txtNombre" CssClass="form-control" placeholder="Ingrese su nombre">
                </asp:TextBox>

                <div class="form-text">
                    <asp:RequiredFieldValidator
                        runat="server"
                        ID="rfvNombre"
                        ControlToValidate="txtNombre"
                        ErrorMessage="El nombre es obligatorio."
                        CssClass="text-danger"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="form-field">
                <label for="<%= txtApellido.ClientID %>" class="form-label">Apellido</label>

                <asp:TextBox
                    runat="server" ID="txtApellido" CssClass="form-control" placeholder="Ingrese su apellido">
                </asp:TextBox>

                <div class="form-text">
                    <asp:RequiredFieldValidator
                        runat="server"
                        ID="rfvApellido"
                        ControlToValidate="txtApellido"
                        ErrorMessage="El apellido es obligatorio."
                        CssClass="text-danger"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="form-field">
                <label for="<%= txtTelefono.ClientID %>" class="form-label">Teléfono</label>

                <asp:TextBox
                    runat="server" ID="txtTelefono" CssClass="form-control" TextMode="Phone" placeholder="Ingrese su teléfono">
                            
                   
                </asp:TextBox>

                <div class="form-text">
                    <asp:RequiredFieldValidator
                        runat="server"
                        ID="rfvTelefono"
                        ControlToValidate="txtTelefono"
                        ErrorMessage="El teléfono es obligatorio."
                        CssClass="text-danger"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="form-field">
                <label for="<%= txtEmail.ClientID %>" class="form-label">Correo electrónico</label>

                <asp:TextBox
                    runat="server"
                    ID="txtEmail"
                    CssClass="form-control"
                    TextMode="Email"
                    placeholder="ejemplo@correo.com">
                </asp:TextBox>

                <div class="form-text">
                    <asp:RequiredFieldValidator
                        runat="server"
                        ID="rfvEmail"
                        ControlToValidate="txtEmail"
                        ErrorMessage="El correo electrónico es obligatorio."
                        CssClass="text-danger"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                    <asp:RegularExpressionValidator
                        runat="server"
                        ID="revEmail"
                        ControlToValidate="txtEmail"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                        ErrorMessage="Ingrese un correo electrónico válido."
                        CssClass="text-danger"
                        Display="Dynamic">
                    </asp:RegularExpressionValidator>
                </div>
            </div>

            <div class="form-field">
                <label for="<%= txtPassword.ClientID %>" class="form-label">Contraseña</label>

                <asp:TextBox
                    runat="server"
                    ID="txtPassword"
                    TextMode="Password"
                    CssClass="form-control"
                    placeholder="Ingrese su contraseña">
    </asp:TextBox>

                <div class="form-text">
                    <asp:RequiredFieldValidator
                        runat="server"
                        ID="rfvPassword"
                        ControlToValidate="txtPassword"
                        ErrorMessage="La contraseña es obligatoria."
                        CssClass="text-danger"
                        Display="Dynamic">
        </asp:RequiredFieldValidator>
                </div>
            </div>


            <div class="d-flex gap-2 justify-content-start">
                <asp:Button
                    ID="btnRegistrar"
                    runat="server"
                    Text="Registrar"
                    CssClass="btn btn-primary"
                    OnClick="btnRegistrar_Click" />

                <asp:Button
                    ID="btnCancelar"
                    runat="server"
                    Text="Cancelar"
                    CssClass="btn btn-secondary"
                    CausesValidation="false"
                    OnClick="btnCancelar_Click" />

            </div>

        </div>
    </div>
</asp:Content>
