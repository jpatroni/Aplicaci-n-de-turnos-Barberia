<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Recepcion.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.Recepcion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="container mt-4">
     <h1>Recepcion</h1>
     <p>Listado de clientes</p>

       <%-- Más adelante, esta grilla recibirá la lista de ClienteDatos. --%>
         <div class="mb-3">
    <asp:Label runat="server" AssociatedControlID="txtBuscarCliente"
        Text="Buscar por nombre, apellido o teléfono" CssClass="form-label" />

    <div class="input-group">
        <asp:TextBox ID="txtBuscarCliente" runat="server"
            MaxLength="100" CssClass="form-control" />

        <asp:Button ID="btnBuscarCliente" runat="server"
            Text="Buscar" CssClass="btn btn-primary"
            CausesValidation="false" OnClick="btnBuscarCliente_Click" />
    </div>
</div>

     <asp:GridView ID="dgvClientes" runat="server" AutoGenerateColumns="false"
         CssClass="table table-striped"
         EmptyDataText="No hay clientes para mostrar." DataKeyNames="Id" OnSelectedIndexChanged="dgvClientes_SelectedIndexChanged">
         <Columns>
             <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
             <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
             <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
             <asp:BoundField DataField="Email" HeaderText="Email" />
             <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
             <asp:CommandField ShowSelectButton="True" SelectText="Editar" />
         </Columns>
         </asp:GridView>
         <asp:Label ID="lblMensajeRecepcion" runat="server" CssClass="text-danger" />
         <asp:Panel ID="pnlEdision" runat="server" Visible="false" CssClass="mt-4">
             <h2>  Editar Cliente</h2>
             <div class="mb-3">
                 <asp:Label runat="server" 
                     AssociatedControlID="txtEditarNombre" Text="Nombre"
                     CssClass="from-label" />

                 <asp:TextBox ID="txtEditarNombre" runat ="server" MaxLength="100" 
                     CssClass="from-control" />



             </div>
            
             <div class="mb-3">
                 <asp:Label runat="server" AssociatedControlID="txtEditarApellido"
                     Text="Apellido" CssClass="from-label" />
                   <asp:TextBox ID="txtEditarApellido" runat="server" MaxLength="100" CssClass="from-control" />
                 </div>

             <div class="mb-3">
                 <asp:Label runat="server" AssociatedControlID="txtEditarTelefono"
                     Text="Telefono" CssClass="from-label" />
                 <asp:TextBox ID="txtEditarTelefono" runat="server"
                     TextMode="Phone" MaxLength="30" CssClass="from-control" />

                </div>

             <div class ="mb-3">
                 <asp:Label runat="server" AssociatedControlID="txtEditarEmail"
                     Text="Email de contacto" CssClass="from-label" />
                 <asp:TextBox ID="txtEditarEmail" runat="server"
                     TextMode="Email" MaxLength="254" CssClass="from-control" />

             </div>

             <div class="mt-3">
                 <asp:Button ID="btnGuardarEdicion" runat ="server"
                     Text="Guardar" CssClass="btn btn-primary"
                     OnClick="btnGuardarEdicion_Click"/>
                 
                 <asp:Button ID="btnCancelarEdicion" runat="server" 
                     Text="Cancelar" CssClass="btn btn-secondary" CausesValidation="false"
                     OnClick="btnCancelarEdicion_Click" />
                 <asp:Button ID="btnCambiarEstado" runat="server" Visible="false"
                     CssClass="btn btn-outline-danger" CausesValidation="false"
                     OnClick="btnCambiarEstado_Click" />



             </div>

         </asp:Panel>

 </div>
</asp:Content>
