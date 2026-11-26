<%@ Page Title="Servicios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Servicios.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.Servicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Content/servicios.css") %>?v=<%= System.IO.File.GetLastWriteTimeUtc(Server.MapPath("~/Content/servicios.css")).Ticks %>"
        rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">

        <h1 class="mb-1">Inicio</h1>

        <p class="lead">
            Bienvenido a nuestra barbería.
        </p>

        <hr />

        <h2 class="mb-3">Nuestros servicios</h2>

        <div class="servicios-lista">
  <asp:Repeater ID="rptServicios" runat="server"
    OnItemCommand="rptServicios_ItemCommand">
        <ItemTemplate>
            <article class="servicio-tarjeta">
                <asp:Image ID="imgServicio" runat="server"
                    CssClass="servicio-foto"
                    ImageUrl='<%# ObtenerImagenServicio(Convert.ToString(Eval("Nombre"))) %>'
                    AlternateText='<%# Convert.ToString(Eval("Nombre")) %>'
                    Visible='<%# !string.IsNullOrEmpty(ObtenerImagenServicio(Convert.ToString(Eval("Nombre")))) %>' />
                <span class="servicio-duracion">
                    Duración estimada: <%#: Eval("DuracionMinutos") %> minutos
                </span>

                <h2><%#: Eval("Nombre") %></h2>

                <p class="servicio-precio">
                    <%#: Eval("Precio", "{0:C0}") %>
                </p>
                <asp:Button ID="btnElegirServicio" runat="server"
                    Text="Elegir servicio"
                    CssClass="btn btn-dorado mt-3"
                    CausesValidation="false"
                    CommandName="SeleccionarServicio"
                    CommandArgument='<%# Eval("Id") %>' />
            </article>
        </ItemTemplate>
    </asp:Repeater>
</div>

        <div class="table-responsive">


            <asp:GridView ID="dgvServicios" runat="server" AutoGenerateColumns="false" OnRowCommand="dgvServicios_RowCommand"
                CssClass="table table-striped-columns table-bordered table-hover align-middle" Width="70%">
                <Columns>
                    <asp:BoundField HeaderText="Servicio" DataField="Nombre" />
                    <asp:BoundField
                        HeaderText="Precio" DataField="Precio" DataFormatString="{0:C2}"
                        ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-CssClass="text-end" />
                    <asp:BoundField HeaderText="Duración estimada" DataField="DuracionMinutos" DataFormatString="{0} min"
                        ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-CssClass="text-end" />
                    <asp:TemplateField HeaderText="Acción">
                <ItemTemplate>
                    <asp:Button
                        ID="btnReservar"
                        runat="server"
                        Text="Seleccionar"
                        CssClass="btn btn-dark btn-sm"
                        CommandName="SeleccionarServicio"
                        CommandArgument='<%# Eval("Id") %>' />
                </ItemTemplate>
            </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <a href="ReservarTurno.aspx" class="btn btn-dark mt-2">Reservar turno
        </a>

    </div>
</asp:Content>
