<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class ="container mt-4" >
        <h1>Bienvenido a nuestra barbería</h1>
        <p> 
            Consulta nuestros servicios y reservá tu proximo turno.

        </p>

        <a href="Servicios.aspx" class ="btn btn-dark">
            
            Ver servicios </a>
           <a href="ReservarTurno.aspx" class="btn btn-outline-dark">
        Reservar turno
    </a>
      


    </div>



</asp:Content>
