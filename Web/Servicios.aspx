<%@ Page Title="Servicios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Servicios.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.Servicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Content/servicios.css") %>?v=<%= System.IO.File.GetLastWriteTimeUtc(Server.MapPath("~/Content/servicios.css")).Ticks %>"
        rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">

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


        <a href="ReservarTurno.aspx" class="btn btn-dark mt-2">Reservar turno
        </a>

    </div>
</asp:Content>
