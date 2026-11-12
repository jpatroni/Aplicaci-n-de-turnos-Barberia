<%@ Page Title="Administración" Language="C#" MasterPageFile="~/Site.Master" Inherits="Aplicación_de_turnos_Barberia.Seguridad.PaginaAdministracion" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h1>Administración</h1>
        <p>Elegí qué querés gestionar.</p>
        <a class="btn btn-outline-dark" href="Recepcion.aspx">Clientes</a>
        <a class="btn btn-outline-dark" href="GestionBarberos.aspx">Barberos</a>
        <a class="btn btn-outline-dark" href="GestionServicios.aspx">Servicios</a>
    </div>
</asp:Content>
