<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="Excerpta.Registro" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="UpdateRegistro" runat="server">
        <ContentTemplate>

            <asp:Panel ID="Notificacion" runat="server" Visible="false" CssClass="notificacion" />

            <!-- Encabezado -->
            <div class="encabezado">
                <h2>Registro</h2>
                <h3>Cree una cuenta nueva</h3>
            </div>

            <hr />

            <!-- Entradas -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                    <div class="entrada">
                        <asp:TextBox ID="InputNombre" runat="server" MaxLength="100" autocomplete="off" ToolTip="Entrada de datos para el nombre" />
                        <span class="caption">Nombre</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                    <div class="entrada">
                        <asp:TextBox ID="InputApellidos" runat="server" MaxLength="100" autocomplete="off" ToolTip="Entrada de datos para los apellidos" />
                        <span class="caption">Apellidos</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                    <div class="entrada">
                        <asp:TextBox ID="InputCentro" runat="server" MaxLength="100" autocomplete="off" ToolTip="Entrada de datos para el centro" />
                        <span class="caption">Centro</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                    <div class="entrada">
                        <asp:TextBox ID="InputTelefono" runat="server" MaxLength="12" autocomplete="off" ToolTip="Entrada de datos para el teléfono" />
                        <span class="caption">Teléfono</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                    <div class="entrada">
                        <asp:TextBox ID="InputEmail" runat="server" MaxLength="100" autocomplete="off" ToolTip="Entrada de datos para el email" />
                        <span class="caption">Email</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                    <div class="entrada">
                        <asp:TextBox ID="InputClave" runat="server" MaxLength="100" autocomplete="off" ToolTip="Entrada de datos para la contraseña" TextMode="Password" />
                        <span class="caption">Contraseña</span>
                    </div>
                </div>
            </div>

            <!-- Botón -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                    <asp:Button ID="ButtonRegistro" runat="server" Text="Registrarse" OnClick="ButtonRegistro_Click" CssClass="boton" />
                </div>
            </div>

            <!-- Campos obligatorios -->
            <asp:RequiredFieldValidator runat="server" ControlToValidate="InputNombre"    Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="InputApellidos" Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="InputCentro"    Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="InputTelefono"  Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="InputEmail"     Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="InputClave"     Display="None" SetFocusOnError="true" />

            <!-- Validaciones personalizadas -->
            <asp:CustomValidator runat="server" OnServerValidate="ValidarEmail" ControlToValidate="InputEmail" />

        </ContentTemplate>
    </asp:UpdatePanel>

    <!--
    ╔═════════════════╗
    ║     SPINNER     ║
    ╚═════════════════╝
    -->

    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateRegistro">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
