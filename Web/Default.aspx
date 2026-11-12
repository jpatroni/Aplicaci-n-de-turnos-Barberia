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

            <div class="row g-4">
                 <!-- Esta en html por el momento pero tiene que salir de la BD -->
                <!-- PROFESIONAL 1 -->
                <div class="col-12 col-md-6 col-lg-3">
                    <article class="tarjeta-barbero">

                        <img
                            src="https://lh3.googleusercontent.com/aida-public/AB6AXuB8s8tJtvAxRfjctVmwhnSd4Sk10L7yYPG1Et1z_24KvBo9knvt87YVhcfJdRU2V9BTz-5BshO5WizELk9MiiPYqqeGI_-kSYZOXbBX3jERSY8LBZZWp7QtANqrAzlFJHGU_iBjJU7sCZ-ln7cZmQErAhHMDUyrEAU7N_axIrT0g20MWccIT4hiSOMaWjmXFXb0YJ4VTdcyOJouxX8hCy0aX8KPJ4DNOcKxPos8DSQY"
                            alt="Retrato de Mateo Silva"
                            loading="lazy" />

                        <h3>Mateo Silva</h3>

                        <p class="especialidad">
                            Degradados y texturados
                        </p>

                        <p>
                            Cortes actuales con transiciones prolijas
                            y atención a cada detalle.
                        </p>

                        <a href="ReservarTurno.aspx" class="btn btn-borde">
                            Reservar turno
                        </a>

                    </article>
                </div>

                <!-- PROFESIONAL 2 -->
                <div class="col-12 col-md-6 col-lg-3">
                    <article class="tarjeta-barbero">

                        <img
                            src="https://lh3.googleusercontent.com/aida-public/AB6AXuAtaOMX9KpIjKZ_ReuGpZ84q0VwyPleDwaR972UGscIxlZ1CwyHqncVBJUDmRoc7TyPKzE6_Ul6lDCJc4U19SHRgGoVRWvsmvW16c7Rjkeob2QzKNyxEdglJAFV4GcYMxrFNPdXsoAJqu2omU81deAMbAKixUXztMOeIyIO3pgA-W-ZBu5jLGWKuXkQmsIkG-C2HvpaBgM-pL9DCSVdsgkMjS9gy-wH-FMt2zqIM-tD"
                            alt="Retrato de Lucas Benítez"
                            loading="lazy" />

                        <h3>Lucas Benítez</h3>

                        <p class="especialidad">
                            Cortes clásicos
                        </p>

                        <p>
                            Estilos tradicionales con terminaciones
                            cuidadas y un toque personal.
                        </p>

                        <a href="ReservarTurno.aspx" class="btn btn-borde">
                            Reservar turno
                        </a>

                    </article>
                </div>

                <!-- PROFESIONAL 3 -->
                <div class="col-12 col-md-6 col-lg-3">
                    <article class="tarjeta-barbero">

                        <img
                            src="https://lh3.googleusercontent.com/aida-public/AB6AXuDDhypWy5JhTIclWVnpMjXhXdUP1lUkCztXLEfk9iOA-VNOKS02aV4QazfunRpMF_R89EVvJ9k7lcbj2e-x5Jhgzwjlez10XBvsybmgJ3MyY2_Nn24PYXL3_dLIk0c7KCzbfoIX-eXd5qMrZQp4OZJb38MnB8EhHWNsNy_4DeYtCNruxlPk5ydcDtt1sEVolrGElxmTQd38zT7TQj4aRD1BAswDA-JD2eLXFLO9lHjR"
                            alt="Retrato de Santiago Rossi"
                            loading="lazy" />

                        <h3>Santiago Rossi</h3>

                        <p class="especialidad">
                            Barba y perfilado
                        </p>

                        <p>
                            Diseño y cuidado de barba para acompañar
                            tus facciones y completar tu estilo.
                        </p>

                        <a href="ReservarTurno.aspx" class="btn btn-borde">
                            Reservar turno
                        </a>

                    </article>
                </div>

                <!-- PROFESIONAL 4 -->
                <div class="col-12 col-md-6 col-lg-3">
                    <article class="tarjeta-barbero">

                        <img
                            src="https://lh3.googleusercontent.com/aida-public/AB6AXuCFjHKj1IdagMln5Ymkz4No9VxgffT8JaT1o9n0_NgiuTJ7mbqC69fy5pm4J0-yW2LJNW2XN1o0KdWBFv-04fDabwuT-53iDri4YYJces4dpv1Q2YnEGQthLciLc623fj1XV_FpchdEzduENOkGFKtYqc3gvUhfGk6xw5Lb_ScS4AZzY4PhNIGcIq3uZDfcA6P2oEJ80Bm_IvaXHokstoKs8mlFpaA8yiloVLdHeKok"
                            alt="Retrato de Joaquín Vega"
                            loading="lazy" />

                        <h3>Joaquín Vega</h3>

                        <p class="especialidad">
                            Corte personalizado
                        </p>

                        <p>
                            Asesoramiento para encontrar un corte
                            cómodo que se adapte a tu día a día.
                        </p>

                        <a href="ReservarTurno.aspx" class="btn btn-borde">
                            Reservar turno
                        </a>

                    </article>
                </div>

            </div>
        </div>
    </section>

</asp:Content>