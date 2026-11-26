<%@ Page Title="Reservar Turno" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReservarTurno.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.ReservarTurno" UnobtrusiveValidationMode="None" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Content/reservarTurno.css") %>?v=<%= System.IO.File.GetLastWriteTimeUtc(Server.MapPath("~/Content/reservarTurno.css")).Ticks %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="reservar-pagina">
        <asp:Panel runat="server" CssClass="reservar-formulario" DefaultButton="btnConsultarHorarios">
            <h1>Reservar turno</h1>
            <p class="reservar-descripcion">
                Seleccioná tu servicio, barbero y horario de preferencia.
            </p>

            <div class="reservar-campo">
                <asp:Label ID="lblServicio" runat="server"
                    AssociatedControlID="ddlServicio" Text="Servicio" />
                <asp:DropDownList ID="ddlServicio" runat="server"
                    CssClass="form-control" AutoPostBack="true"
                    OnSelectedIndexChanged="SeleccionReserva_Changed" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlServicio"
                    ValidationGroup="Reserva" InitialValue="" Display="Dynamic"
                    ErrorMessage="Seleccioná un servicio." CssClass="reservar-error" />
            </div>

            <div class="reservar-campo">
                <asp:Label ID="lBarbero" runat="server"
                    AssociatedControlID="ddlBarbero" Text="Barbero" />
                <asp:DropDownList ID="ddlBarbero" runat="server"
                    CssClass="form-control" AutoPostBack="true"
                    OnSelectedIndexChanged="SeleccionReserva_Changed" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlBarbero"
                    ValidationGroup="Reserva" InitialValue="" Display="Dynamic"
                    ErrorMessage="Seleccioná un barbero." CssClass="reservar-error" />
            </div>

            <div class="reservar-campo">
                <asp:Label ID="lblFecha" runat="server"
                    AssociatedControlID="txtFecha" Text="Fecha" />
                <asp:TextBox ID="txtFecha" runat="server" TextMode="Date"
                    CssClass="form-control" AutoPostBack="true"
                    OnTextChanged="SeleccionReserva_Changed" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFecha"
                    ValidationGroup="Reserva" Display="Dynamic"
                    ErrorMessage="Ingresá una fecha." CssClass="reservar-error" />
                <small class="reservar-ayuda">Martes a sábado, de 09:00 a 21:00. Turnos cada media hora.</small>
            </div>

            <div class="reservar-campo">
                <asp:Label ID="lblHorario" runat="server"
                    AssociatedControlID="ddlHorario" Text="Horario" />
                <asp:DropDownList ID="ddlHorario" runat="server"
                    CssClass="form-control" Enabled="false">
                    <asp:ListItem Value="">Seleccioná un horario disponible</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlHorario"
                    ValidationGroup="Reserva" InitialValue="" Display="Dynamic"
                    ErrorMessage="Seleccioná un horario disponible." CssClass="reservar-error" />
                <asp:Button ID="btnConsultarHorarios" runat="server"
                    Text="Ver horarios disponibles" CssClass="btn btn-volver mt-3"
                    CausesValidation="false" OnClick="btnConsultarHorarios_Click" />
            </div>

            <asp:Label ID="lblMensaje" runat="server"
                CssClass="reservar-mensaje" role="status" aria-live="polite" />

            <div class="reservar-botones">
                <asp:Button ID="btnConfirmarTurno" runat="server"
                    Text="Confirmar turno" CssClass="btn btn-confirmar"
                    Enabled="false" ValidationGroup="Reserva"
                    OnClick="btnConfirmarTurno_Click" />
                <a href="Servicios.aspx" class="btn btn-volver">Volver a servicios</a>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
