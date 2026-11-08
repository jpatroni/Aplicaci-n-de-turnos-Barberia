<%@ Page Title="Servicios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Servicios.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.Servicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">

        <h1 class="mb-1">Inicio</h1>

        <p class="lead">
            Bienvenido al sistema de gestión de turnos de la barbería.
        </p>

        <hr />

        <h2 class="mb-3">Nuestros servicios</h2>

        <div class="table-responsive">


            <asp:GridView ID="dgvServicios" runat="server" AutoGenerateColumns="false" CssClass="table table-striped-columns table-bordered table-hover align-middle" Width="70%">
                <Columns>
                    <asp:BoundField HeaderText="Servicio" DataField="Nombre" />
                    <asp:BoundField
                        HeaderText="Precio" DataField="Precio" DataFormatString="{0:C2}"
                        ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-CssClass="text-end" />
                    <asp:BoundField HeaderText="Duración" DataField="DuracionMinutos" DataFormatString="{0} min"
                        ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-CssClass="text-end" />
                </Columns>
            </asp:GridView>
        </div>

        <a href="ReservarTurno.aspx" class="btn btn-dark mt-2">Reservar turno
        </a>

    </div>
</asp:Content>
