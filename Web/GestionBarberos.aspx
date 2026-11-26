<%@ Page Title="Gestión de Barberos" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="GestionBarberos.aspx.cs"
    Inherits="Aplicación_de_turnos_Barberia.GestionBarberos" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="head" runat="server">
    <script>
        function validarPesoFotoBarbero() {
            var campo = document.getElementById('<%= fuFoto.ClientID %>');
            if (campo && campo.files && campo.files.length && campo.files[0].size > 15 * 1024 * 1024) {
                var mensaje = document.getElementById('<%= lblMensaje.ClientID %>');
                mensaje.textContent = 'La foto supera los 15 MB. Elegí una imagen más pequeña.';
                mensaje.className = 'd-block alert alert-info mb-3';
                return false;
            }
            return true;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2"
    ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-4">

        <h2>Gestión de Barberos</h2>

        <asp:Label ID="lblMensaje" runat="server"
            EnableViewState="false" />

        <!-- LISTADO DE BARBEROS -->

        <asp:Panel ID="pnlListado" runat="server">

            <h4>Listado de Barberos</h4>

            <div class="table-responsive">

                <asp:GridView ID="dgvBarberos"
                    runat="server"
                    AutoGenerateColumns="false"
                    DataKeyNames="Id"
                    CssClass="table table-striped table-bordered align-middle"
                    OnSelectedIndexChanged="dgvBarberos_SelectedIndexChanged">

                    <Columns>

                        <asp:BoundField DataField="Nombre"
                            HeaderText="Nombre" />

                        <asp:BoundField DataField="Apellido"
                            HeaderText="Apellido" />

                        <asp:BoundField DataField="Especialidad"
                            HeaderText="Especialidad" />


                        <asp:TemplateField HeaderText="Foto">
                            <ItemTemplate>
                                <asp:Image
                                    ID="imgBarbero"
                                    runat="server"
                                    ImageUrl='<%# ObtenerFotoListado(Eval("FotoUrl")) %>'
                                    Width="80px"
                                    Height="80px"
                                    CssClass="rounded-circle"
                                    Style="object-fit: cover;" />
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:CheckBoxField DataField="Activo"
                            HeaderText="Activo"
                            ReadOnly="true" />

                        <asp:CommandField
                            ShowSelectButton="true"
                            SelectText="Modificar" />

                    </Columns>

                </asp:GridView>

            </div>

            <div class="mt-3">
                <asp:Button ID="btnNuevo"
                    runat="server"
                    Text="Agregar barbero"
                    CssClass="btn btn-success"
                    OnClick="btnNuevo_Click" />
            </div>

        </asp:Panel>

        <!-- FORM ALTA Y MODIFICACION -->

        <asp:Panel ID="pnlFormulario"
            runat="server"
            Visible="false">

            <asp:HiddenField ID="hfIdBarbero"
                runat="server" />

            <div class="card mt-3">
                <div class="card-body">

                    <h4>
                        <asp:Label ID="lblTituloFormulario"
                            runat="server" />
                    </h4>

                    <div class="row g-3 mt-2">

                        <div class="col-md-6">
                            <label class="form-label">Nombre</label>
                            <asp:TextBox ID="txtNombre"
                                runat="server"
                                CssClass="form-control" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label">Apellido</label>
                            <asp:TextBox ID="txtApellido"
                                runat="server"
                                CssClass="form-control" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label">Especialidad</label>
                            <asp:TextBox ID="txtEspecialidad"
                                runat="server"
                                CssClass="form-control" />
                        </div>


                        <div class="col-md-6">
                            <label class="form-label">Foto del barbero</label>

                            <asp:FileUpload
                                ID="fuFoto"
                                runat="server"
                                CssClass="form-control"
                                accept=".jpg,.jpeg,.png" />

                            <asp:HiddenField
                                ID="hfFotoActual"
                                runat="server" />

                            <small class="text-muted">Seleccioná una imagen JPG o PNG (máximo 15 MB).
                            </small>

                            <div class="mt-2">
                                <asp:Image
                                    ID="imgFotoActual"
                                    runat="server"
                                    Width="120px"
                                    Height="120px"
                                    CssClass="rounded"
                                    AlternateText="Foto actual" />
                            </div>
                        </div>


                        <div class="col-12">
                            <label class="form-label">Descripción</label>
                            <asp:TextBox ID="txtDescripcion"
                                runat="server"
                                CssClass="form-control"
                                TextMode="MultiLine"
                                Rows="3" />
                        </div>

                    </div>

                    <div class="mt-3">

                        <asp:Button ID="btnAlta"
                            runat="server"
                            Text="Guardar barbero"
                            CssClass="btn btn-success"
                            OnClick="btnAlta_Click" OnClientClick="return validarPesoFotoBarbero();" />

                        <asp:Button ID="btnModificacion"
                            runat="server"
                            Text="Guardar cambios"
                            CssClass="btn btn-primary"
                            OnClick="btnModificacion_Click" OnClientClick="return validarPesoFotoBarbero();"
                            Visible="false" />

                        <asp:Button ID="btnBaja"
                            runat="server"
                            Text="Dar de baja"
                            CssClass="btn btn-danger"
                            OnClick="btnBaja_Click"
                            OnClientClick="return confirm('¿Dar de baja este barbero?');"
                            Visible="false" />

                        <asp:Button
                            ID="btnReactivar"
                            runat="server"
                            Text="Dar de alta"
                            CssClass="btn btn-success"
                            OnClick="btnReactivar_Click"
                            Visible="true" />

                        <asp:Button ID="btnVolver"
                            runat="server"
                            Text="Volver al listado"
                            CssClass="btn btn-secondary"
                            OnClick="btnVolver_Click" />

                    </div>

                </div>
            </div>

        </asp:Panel>

    </div>

</asp:Content>


