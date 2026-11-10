<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Recepcion.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.Recepcion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="container mt-4">
     <h1>Recepcion</h1>
     <p>Listado de clientes</p>

       <%-- Más adelante, esta grilla recibirá la lista de ClienteDatos. --%>

     <asp:GridView ID="dgvClientes" runat="server" AutoGenerateColumns="false"
         CssClass="table table-striped"
         EmptyDataText="No hay clientes para mostrar." DataKeyNames="Id" OnSelectedIndexChanged="dgvClientes_SelectedIndexChanged">
         <Columns>
             <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
             <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
             <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
             <asp:BoundField DataField="Email" HeaderText="Email" />
             <asp:CommandField ShowSelectButton="True" SelectText="Editar" />
         </Columns>
         </asp:GridView>


 </div>
</asp:Content>
