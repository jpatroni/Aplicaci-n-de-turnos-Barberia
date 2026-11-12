<%@ Page Title="Inicio" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="Aplicación_de_turnos_Barberia.WebForm1" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="head"
    runat="server">
</asp:Content>

<asp:Content ID="Content2"
    ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">

    <!-- PORTADA -->
    <section class="portada">
        <div class="container">

            <p class="text-uppercase">
                Barbería · Estilo y cuidado
            </p>

            <h1>
                Tu estilo, en
                <span>buenas manos</span>
            </h1>

            <p>
                Cortes clásicos, estilos actuales y atención personalizada.
                Conocé nuestro equipo y encontrá tu próximo estilo.
            </p>

            <div class="d-flex flex-column flex-sm-row gap-3 justify-content-center">
                <a href="ReservarTurno.aspx" class="btn btn-dorado">
                    Reservar turno
                </a>

                <a href="Servicios.aspx" class="btn btn-borde">
                    Ver servicios
                </a>
            </div>

        </div>
    </section>

    <!-- PRESENTACIÓN -->
    <section class="seccion">
        <div class="container text-center">

            <p class="etiqueta">Nuestra barbería</p>

            <h2 class="seccion-titulo">
                Un espacio para renovar tu estilo
            </h2>

            <p class="mx-auto" style="max-width: 700px;">
                Combinamos el oficio de la barbería tradicional con
                técnicas actuales. Te acompañamos para elegir un corte
                que se adapte a vos.
            </p>

        </div>
    </section>

    <!-- EQUIPO -->
    <section class="seccion equipo" id="equipo">
        <div class="container">

            <div class="text-center mb-5">
                <p class="etiqueta">El equipo</p>

                <h2 class="seccion-titulo">
                    Elegí a tu especialista
                </h2>

                <p class="equipo-descripcion">
                    Conocé a nuestros profesionales.
                    Encontrá el estilo que va con vos.
                </p>
            </div>
                       
                 <!-- Esta en html por el momento pero tiene que salir de la BD -->
                
<div class="row g-4">

    <asp:Repeater ID="rptBarberos" runat="server">
        <ItemTemplate>

            <div class="col-12 col-md-6 col-lg-3">

                <article class="tarjeta-barbero">

                    <img
                        src='<%# ResolveUrl(Eval("FotoUrl").ToString()) %>'
                        alt="Foto del barbero"
                        loading="lazy"
                        onerror="this.src='https://placehold.co/400x500?text=Sin+foto';" />

                    <h3>
                        <%# Eval("Nombre") %>
                        <%# Eval("Apellido") %>
                    </h3>

                    <p class="especialidad">
                        <%# Eval("Especialidad") %>
                    </p>

                    <p>
                        <%# Eval("Descripcion") %>
                    </p>

                    <a href='<%# "ReservarTurno.aspx?idBarbero=" + Eval("Id") %>'
                       class="btn btn-borde">
                        Reservar turno
                    </a>

                </article>

            </div>

        </ItemTemplate>
    </asp:Repeater>

</div>

                
                </div>

            </div>
        </div>
    </section>

</asp:Content>