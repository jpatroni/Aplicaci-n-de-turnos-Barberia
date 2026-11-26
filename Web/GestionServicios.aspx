<%@ Page Title="Gestión de Servicios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionServicios.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.GestionServicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Content/gestion-servicios.css") %>?v=<%= System.IO.File.GetLastWriteTimeUtc(Server.MapPath("~/Content/gestion-servicios.css")).Ticks %>"
        rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="container gestion-servicios">
        <header class="gestion-servicios-cabecera">
            <div>
                <h1>Gestión de Servicios</h1>
                <p>Agregá servicios, modificá sus datos o administrá su disponibilidad.</p>
            </div>
            <a class="btn btn-outline-dark" href="<%= ResolveUrl("~/Administracion.aspx") %>">Volver a Administración</a>
        </header>

        <asp:Label ID="lblMensaje" runat="server" Visible="false"
            role="status" aria-live="polite" />

        <asp:Panel runat="server" DefaultButton="btnGuardar"
            CssClass="gestion-servicios-formulario">
            <h2>Datos del servicio</h2>
            <asp:HiddenField ID="hfIdServicio" runat="server" Value="0" />

            <div class="row g-3">
                <div class="col-md-5">
                    <asp:Label runat="server" AssociatedControlID="txtNombre"
                        Text="Nombre" CssClass="form-label" />
                    <asp:TextBox ID="txtNombre" runat="server"
                        MaxLength="100" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <asp:Label runat="server" AssociatedControlID="txtPrecio"
                        Text="Precio" CssClass="form-label" />
                    <asp:TextBox ID="txtPrecio" runat="server"
                        MaxLength="14" inputmode="decimal" CssClass="form-control"
                        aria-describedby="ayudaPrecio" />
                    <small id="ayudaPrecio" class="form-text">
                        Sin separador de miles. Ejemplo: 10000 o 10000,50.
                    </small>
                </div>
                <div class="col-md-3">
                    <asp:Label runat="server" AssociatedControlID="txtDuracion"
                        Text="Duración estimada (minutos)" CssClass="form-label" />
                    <asp:TextBox ID="txtDuracion" runat="server"
                        ReadOnly="true" Text="30" CssClass="form-control" />
                    <small class="form-text">Todos los servicios usan bloques de 30 minutos.</small>
                </div>
            </div>

            <div class="d-flex gap-2 flex-wrap mt-4">
                <asp:Button ID="btnGuardar" runat="server"
                    Text="Guardar servicio" OnClick="btnGuardar_Click"
                    CssClass="btn btn-dorado" />
                <asp:Button ID="btnCancelar" runat="server"
                    Text="Cancelar edición" OnClick="btnCancelar_Click"
                    CausesValidation="false" CssClass="btn btn-outline-dark" />
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlListado" runat="server" CssClass="mt-4">
            <div class="gestion-servicios-cabecera">
                <h2>Listado de servicios</h2>
                <asp:Button ID="btnNuevo" runat="server"
                    Text="Agregar servicio" OnClick="btnNuevo_Click"
                    CausesValidation="false" CssClass="btn btn-outline-dark" />
            </div>
            <p>Dar de baja oculta el servicio del catálogo. Sus turnos quedan conservados.</p>
            <p>Para eliminar definitivamente un servicio, primero dale de baja. Solo se pueden borrar servicios sin turnos asociados.</p>
            <div class="table-responsive">
                <asp:GridView ID="dgvServicios" runat="server"
                    AutoGenerateColumns="false" DataKeyNames="Id"
                    CssClass="table table-striped align-middle"
                    EmptyDataText="Todavía no hay servicios registrados."
                    OnSelectedIndexChanged="dgvServicios_SelectedIndexChanged"
                    OnRowCommand="dgvServicios_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Precio" HeaderText="Precio"
                            DataFormatString="{0:C2}" />
                        <asp:BoundField DataField="DuracionMinutos"
                            HeaderText="Duración estimada (minutos)" />
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" ReadOnly="true" />
                        <asp:CommandField ShowSelectButton="true" SelectText="Modificar"
                            CausesValidation="false" />
                        <asp:TemplateField HeaderText="Disponibilidad">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEstadoServicio" runat="server"
                                    Text='<%# (bool)Eval("Activo") ? "Dar de baja" : "Reactivar" %>'
                                    CommandName='<%# (bool)Eval("Activo") ? "DesactivarServicio" : "ReactivarServicio" %>'
                                    CommandArgument='<%# Eval("Id") %>'
                                    CausesValidation="false"
                                    OnClientClick="return confirm('¿Querés cambiar la disponibilidad de este servicio?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Eliminar">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEliminarServicio" runat="server"
                                    Text="Eliminar definitivamente"
                                    CommandName="EliminarServicio"
                                    CommandArgument='<%# Eval("Id") %>'
                                    Visible='<%# !(bool)Eval("Activo") %>'
                                    CausesValidation="false"
                                    CssClass="servicio-eliminar"
                                    OnClientClick="return confirm('¿Eliminar definitivamente este servicio? Esta acción no se puede deshacer. Si tiene turnos asociados, no se eliminará.');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
    </section>
</asp:Content>
