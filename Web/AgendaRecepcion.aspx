<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AgendaRecepcion.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.AgendaRecepcion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container m-t4">
    <h1>Agenda Recepción</h1>
    <p>Consulta los turnos de la barbería</p>

    <div class="mb-3">
       <asp:Calendar ID="calAgenda" runat="server"
           SelectionMode="Day"
           FirstDayOfWeek="Monday"
           DayNameFormat="Shortest"
           CssClass="mb-3" />


    </div>
    <asp:Button ID="btnVerAgenda" runat="server"
        Text="Ver Turnos"
        CssClass="btn btn-primary"
        OnClick="btnVerAgenda_Click"/>

    <asp:Label ID="lblFechaSeleccionada" runat="server"
    CssClass="d-block mt-3" />




  <asp:Panel runat="server"
    CssClass="input-group mt-3"
    DefaultButton="btnBuscarTurno">
        <asp:TextBox ID="txtBuscarTurno" runat="server"
            MaxLength="100"
            CssClass="from-control"
            placeholder="Buscar turno por nombre o apellido del cliente"
            aria-label="Buscar turno por cliente" />

        <asp:Button ID="btnBuscarTurno" runat="server"
            Text="Buscar"
            CssClass="btn btn-primary"
            OnClick="btnBuscarTurno_Click" />

</asp:Panel>


    <asp:GridView ID="dgvTurnos" runat="server"
        DataKeyNames="Id"
        OnSelectedIndexChanged="dgvTurnos_SelectedIndexChanged"
        AutoGenerateColumns ="False"
        CssClass="table table-striped mt-3"
        EmptyDataText="No hay turnos para esta fecha"
        OnRowCommand="dgvTurnos_RowCommand">
        
       <Columns>
            <asp:BoundField DataField="Hora" HeaderText="Hora" />
        <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
        <asp:BoundField DataField="Barbero" HeaderText="Barbero" />
        <asp:BoundField DataField="Servicio" HeaderText="Servicio" />
        <asp:BoundField DataField="Estado" HeaderText="Estado" />

           <asp:CommandField
               HeaderText="Reprogramar"
               ShowSelectButton="true"
               SelectText="Seleccionar" />

           <asp:TemplateField HeaderText="Cancelar">
    <ItemTemplate>
        <asp:LinkButton ID="btnCancelarTurno" runat="server"
            Text="Cancelar turno"
            CausesValidation="false"
            OnClientClick="return confirm('¿Querés cancelar esta reserva?');"
            CommandName="CancelarReserva"
CommandArgument='<%# Eval("Id") %>'/>
    </ItemTemplate>
</asp:TemplateField>

       </Columns>


    </asp:GridView>


    <asp:Label ID="lblMensajeAgenda" runat="server"
    CssClass="text-danger d-block mt-3" />

    <asp:Label ID="lblTurnoSeleccionado" runat="server"
    CssClass="d-block mt-3" />

    <asp:Panel ID="pnlReprogramacion" runat="server"
        Visible="false" CssClass ="mt-3">

        <h2>Reprogramar turno</h2>

        <asp:Label runat="server" 
            AssociatedControlID="txtNuevaFecha"
            Text="Nueva fecha" />

        <asp:TextBox ID="txtNuevaFecha" runat="server"
            TextMode="Date" CssClass ="from-control mb-3" />

        <asp:Label runat ="server"
            AssociatedControlID="ddlNuevoHorario"
            Text="Nuevo Horario" />

        <asp:DropDownList ID="ddlNuevoHorario" runat="server"
            CssClass="from-select mb-3">
            <asp:ListItem Text="Selecioná un horario" Value=""  />

        </asp:DropDownList>

        <asp:Button ID="btnRevisarReprogramacion" runat="server"
            Text="Confirmar reprogramación"
            CssClass ="btn btn-primary"
            OnClick="btnRevisarReprogramacion_Click" />


        <asp:Button ID="btnCancelarEdicion" runat="server"
            Text="Cancelar Edicion"
            CssClass="btn btn-secondary"
            CausesValidation="false"
            OnClick="btnCancelarEdicion_Click"/>

        <asp:Label ID="lblMensajeReprogramacion" runat="server"
            CssClass="d-block mt-3" />



    </asp:Panel>





</div>




</asp:Content>
