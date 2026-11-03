<%@ Page Title="Reservar Turno" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReservarTurno.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.ReservarTurno" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class ="container mt-4"> 
        <h1> Reservar turno   </h1>

        <p>Selecciona tu barbero y turno de preferencia </p>

        <div class ="mb-3">   
            <asp:Label ID="lblServicio" runat="server" AssociatedControlID="ddlServicio"
                Text="Servicio"
                 CssClass="form-label" />
            <asp:DropDownList ID="ddlServicio" runat="server"    CssClass="form-select">
                 <asp:ListItem Value="">Seleccioná un servicio</asp:ListItem>
            <asp:ListItem Value="1">Corte de pelo</asp:ListItem>
            <asp:ListItem Value="2">Arreglo de barba</asp:ListItem>
                </asp:DropDownList>



        </div>

        <div class ="mb-3">
            <asp:Label ID="Barbero" runat="server"  AssociatedControlID="ddlBarbero"
        Text="Barbero"
        CssClass="form-label" />

              <asp:DropDownList
        ID="ddlBarbero"
        runat="server"
        CssClass="form-select">


                    <asp:ListItem Value="">Seleccioná un barbero</asp:ListItem>
        <asp:ListItem Value="1">Martín</asp:ListItem>
        <asp:ListItem Value="2">Lucas</asp:ListItem>
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
    </asp:DropDownList>
</div>

<div class="mt-4">
    <button type="button" class="btn btn-dark" disabled>
        Confirmar turno
    </button>

    <a href="Servicios.aspx" class="btn btn-outline-dark">
        Volver a servicios
    </a>
</div>

<p class="text-muted mt-2">
    La confirmación de reservas estará disponible próximamente.
</p>
        </div>

    </div>


</asp:Content>
