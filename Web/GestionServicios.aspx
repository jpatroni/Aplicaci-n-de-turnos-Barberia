<%@ Page Title="Gestión de Servicios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionServicios.aspx.cs" Inherits="Aplicación_de_turnos_Barberia.GestionServicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .contenedor {
            width: 33.33%;
            margin: 30px auto;
        }

        .campo {
            margin-bottom: 12px;
        }

            .campo label {
                display: block;
                margin-bottom: 4px;
            }

            .campo input {
                width: 100%;
                box-sizing: border-box;
            }

        .botones {
            margin-top: 15px;
        }

        .listado {
            width: 90%;
            margin: 30px auto;
        }

            .listado table {
                width: 100%;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Gestión de Servicios</h1>

    <div class="contenedor">

        <h2>Datos del servicio</h2>

        <asp:HiddenField ID="hfIdServicio" runat="server" Value="0" />

        <div class="campo">
            <label>Nombre:</label>
            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
        </div>

        <div class="campo">
            <label>Precio:</label>
            <asp:TextBox ID="txtPrecio" runat="server"></asp:TextBox>
        </div>

        <div class="campo">
            <label>Duración en minutos:</label>
            <asp:TextBox ID="txtDuracion" runat="server"></asp:TextBox>
        </div>

        <div class="botones">
            <asp:Button ID="btnGuardar" runat="server"
                Text="Guardar servicio" OnClick="btnGuardar_Click" />

            <asp:Button ID="btnCancelar" runat="server"
                Text="Cancelar" OnClick="btnCancelar_Click" />
        </div>

        <br />

        <asp:Label ID="lblMensaje" runat="server"></asp:Label>

    </div>

    
    <h4>Listado de Servicios</h4>

    <asp:Panel ID="pnlListado" runat="server">

    <div class="table-responsive">

        <asp:GridView ID="dgvServicios"
            runat="server"
            AutoGenerateColumns="false"
            DataKeyNames="Id"
            OnSelectedIndexChanged="dgvServicios_SelectedIndexChanged">

            <Columns>

                <asp:BoundField DataField="Nombre"
                    HeaderText="Nombre" />

                <asp:BoundField DataField="Precio"
                    HeaderText="Precio" />

                <asp:BoundField DataField="DuracionMinutos"
                    HeaderText="Duración (minutos)" />

                <asp:CheckBoxField DataField="Activo"
                    HeaderText="Activo"
                    ReadOnly="true" />

                <asp:CommandField
                    ShowSelectButton="true"
                    SelectText="Modificar" />

            </Columns>

        </asp:GridView>

    </div>

    <div class="mt-3">
        <asp:Button ID="btnNuevo"
            runat="server"
            Text="Agregar servicio"
            OnClick="btnNuevo_Click" />
    </div>

</asp:Panel>

    

</asp:Content>
