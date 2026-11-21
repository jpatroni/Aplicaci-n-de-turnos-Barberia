<%@ Page Title="Reservar Turno" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReservarTurno.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.ReservarTurno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-4">
        <h1>Reservar turno   </h1>

        <p>Selecciona tu barbero y turno de preferencia </p>

        <div class="mb-3">
            <asp:Label ID="lblServicio" runat="server" AssociatedControlID="ddlServicio"
                Text="Servicio"
                CssClass="form-label" />
            <asp:DropDownList ID="ddlServicio" runat="server" CssClass="form-select">
            </asp:DropDownList>



        </div>

        <div class="mb-3">
            <asp:Label ID="lBarbero" runat="server" AssociatedControlID="ddlBarbero"
                Text="Barbero"
                CssClass="form-label" />

            <asp:DropDownList
                ID="ddlBarbero"
                runat="server"
                CssClass="form-select">
            </asp:DropDownList>

            <div class="mb-3">
                <asp:Label
                    ID="lblFecha"
                    runat="server"
                    AssociatedControlID="txtFecha"
                    Text="Fecha"
                    CssClass="form-label" />

                <asp:TextBox
                    ID="txtFecha"
                    runat="server"
                    TextMode="Date"
                    CssClass="form-control" />
            </div>
            <div class="mb-3">
                <asp:Label
                    ID="lblHorario"
                    runat="server"
                    AssociatedControlID="ddlHorario"
                    Text="Horario"
                    CssClass="form-label" />

                <asp:DropDownList
                    ID="ddlHorario"
                    runat="server"
                    CssClass="form-select">

                    <asp:ListItem Value="">Seleccioná un horario</asp:ListItem>
                    <asp:ListItem Value="09:00">09:00</asp:ListItem>
                    <asp:ListItem Value="09:30">09:30</asp:ListItem>
                    <asp:ListItem Value="10:00">10:00</asp:ListItem>
                    <asp:ListItem Value="10:30">10:30</asp:ListItem>
                    <asp:ListItem Value="11:00">11:00</asp:ListItem>
                    <asp:ListItem Value="11:30">11:30</asp:ListItem>
                    <asp:ListItem Value="12:00">12:00</asp:ListItem>
                    <asp:ListItem Value="12:30">12:30</asp:ListItem>
                    <asp:ListItem Value="13:00">13:00</asp:ListItem>
                    <asp:ListItem Value="13:30">13:30</asp:ListItem>
                    <asp:ListItem Value="14:00">14:00</asp:ListItem>
                    <asp:ListItem Value="14:30">14:30</asp:ListItem>
                    <asp:ListItem Value="15:00">15:00</asp:ListItem>
                    <asp:ListItem Value="15:30">15:30</asp:ListItem>
                    <asp:ListItem Value="16:00">16:00</asp:ListItem>
                    <asp:ListItem Value="16:30">16:30</asp:ListItem>
                    <asp:ListItem Value="17:00">17:00</asp:ListItem>
                    <asp:ListItem Value="17:30">17:30</asp:ListItem>
                    <asp:ListItem Value="18:00">18:00</asp:ListItem>
                    <asp:ListItem Value="18:30">18:30</asp:ListItem>
                    <asp:ListItem Value="19:00">19:00</asp:ListItem>
                    <asp:ListItem Value="19:30">19:30</asp:ListItem>
                    <asp:ListItem Value="20:00">20:00</asp:ListItem>
                    <asp:ListItem Value="20:30">20:30</asp:ListItem>
                </asp:DropDownList>
            </div>

            <%-- Botón de confirmar turno --%>
            <div class="mt-4">
                <div class="mt-4">
                    <asp:Button
                        runat="server"
                        ID="btnConfirmarTurno"
                        Text="Confirmar turno"
                        CssClass="btn btn-dark"
                        OnClick="btnConfirmarTurno_Click" />
                </div>

                <a href="Servicios.aspx" class="btn btn-outline-dark">Volver a servicios
                </a>
            </div>

            <p class="text-muted mt-2">
                La confirmación de reservas estará disponible próximamente.
            </p>
        </div>

    </div>


</asp:Content>
