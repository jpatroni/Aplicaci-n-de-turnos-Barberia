
<%@ Page Title="Gestión de Barberos" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="GestionBarberos.aspx.cs"
    Inherits="Aplicación_de_turnos_Barberia.GestionBarberos" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="head" runat="server">
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
                                <asp:Image ID="imgBarbero"
                                    runat="server"
                                    ImageUrl='<%# Eval("FotoUrl") %>'
                                    Width="80px"
                                    Height="80px"
                                    CssClass="rounded-circle"
                                    Style="object-fit:cover;" />
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
                            <label class="form-label">URL de foto</label>
                            <asp:TextBox ID="txtFotoUrl"
                                runat="server"
                                CssClass="form-control" />
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
                            OnClick="btnAlta_Click" />

                        <asp:Button ID="btnModificacion"
                            runat="server"
                            Text="Guardar cambios"
                            CssClass="btn btn-primary"
                            OnClick="btnModificacion_Click"
                            Visible="false" />

                        <asp:Button ID="btnBaja"
                            runat="server"
                            Text="Dar de baja"
                            CssClass="btn btn-danger"
                            OnClick="btnBaja_Click"
                            OnClientClick="return confirm('¿Dar de baja este barbero?');"
                            Visible="false" />

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


