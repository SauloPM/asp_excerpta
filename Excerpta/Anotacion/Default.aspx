<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Excerpta.Default" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <asp:Panel ID="Notificacion" runat="server" Visible="false" CssClass="notificacion" />

            <!-- Encabezado -->
            <div class="encabezado">
                <h2>Inicio de sesión</h2>
                <h3>Utilice una cuenta local para iniciar sesión</h3>
            </div>

            <hr />

            <!-- Entradas -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                    <div class="entrada">
                        <asp:TextBox ID="InputEmail" runat="server" MaxLength="50" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el email" />
                        <span class="caption">Email</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                    <div class="entrada">
                        <asp:TextBox ID="InputClave" runat="server" MaxLength="50" autocomplete="off" spellcheck="false" TextMode="Password" ToolTip="Entrada de datos para la contraseña" />
                        <span class="caption">Contraseña</span>
                    </div>
                </div>
            </div>

            <!-- Botón -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                    <asp:Button ID="ButtonLogin" runat="server" Text="Acceder" OnClick="ButtonLogin_Click" CssClass="boton" />
                </div>
            </div>

            <!-- Cuenta -->
            <a class="cuenta" href="Cuenta.aspx" title="Configuración de la cuenta">Configuración de la cuenta</a>

            <!-- Campos obligatorios -->
            <asp:RequiredFieldValidator runat="server" ControlToValidate="InputEmail" Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="InputClave" Display="None" SetFocusOnError="true" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
