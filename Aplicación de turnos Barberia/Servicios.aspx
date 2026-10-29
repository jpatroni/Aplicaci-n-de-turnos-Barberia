<%@ Page Title="Servicios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Servicios.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.Servicios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="container mt-4">

        <h1 class="display-4">Inicio</h1>

        <p class="lead">
            Bienvenido al sistema de gestión de turnos de la barbería.
        </p>

        <hr />

        <h2 class="mb-3">Nuestros servicios</h2>

        <div class="table-responsive">
            <table class="table table-striped table-bordered table-hover">
                <thead class="table-dark">
                    <tr>
                        <th>Nombre del servicio</th>
                        <th>Precio</th>
                        <th>Duración</th>
                    </tr>
                </thead>

                <tbody>
                    <tr>
                        <td>Corte de pelo</td>
                        <td>$12.000</td>
                        <td>30 minutos</td>
                    </tr>

                    <tr>
                        <td>Corte y barba</td>
                        <td>$15.000</td>
                        <td>45 minutos</td>
                    </tr>

                    <tr>
                        <td>Arreglo de barba</td>
                        <td>$6.000</td>
                        <td>20 minutos</td>
                    </tr>

                    <tr>
                        <td>Corte con degradado</td>
                        <td>$15.000</td>
                        <td>40 minutos</td>
                    </tr>
                </tbody>
            </table>
        </div>

        <a href="ReservarTurno.aspx" class="btn btn-dark mt-2">
            Reservar turno
        </a>

    </div>
</asp:Content>
