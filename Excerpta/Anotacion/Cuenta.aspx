<%@ Page Title="Configuración de la cuenta" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cuenta.aspx.cs" Inherits="Excerpta.Cuenta" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdateNotificacion" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="Notificacion" runat="server" Visible="false" CssClass="notificacion" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Encabezado -->
    <div class="encabezado margin-top-80-mobile">
        <h2>Configurar cuenta</h2>
        <h3>Puede cambiar su clave de acceso, recuperarla o darse de baja</h3>
    </div>

    <hr />

    <!-- Pestañas -->
    <ul class="pestanas">
        <li class="active" data-formulario="cambiar-clave">
            <asp:HyperLink runat="server" NavigateUrl="#" Text="Cambiar clave" />
        </li>
        <li data-formulario="recordar-clave">
            <asp:HyperLink runat="server" NavigateUrl="#" Text="Recordar clave" />
        </li>
        <li data-formulario="eliminar-cuenta">
            <asp:HyperLink runat="server" NavigateUrl="#" Text="Darse de baja" />
        </li>
    </ul>

    <!--
    ╔═══════════════════════╗
    ║     CAMBIAR CLAVE     ║
    ╚═══════════════════════╝
    -->

    <!-- Formulario -->
    <div id="cambiar-clave">
        <asp:UpdatePanel ID="UpdateCambiarClave" runat="server">
            <ContentTemplate>

                <!-- Entradas -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                        <div class="entrada">
                            <asp:TextBox ID="InputEmailCambiarClave" runat="server" MaxLength="100" ValidationGroup="ValidationCambiarClave" autocomplete="off" ToolTip="Entrada de datos para el email"    />
                            <span class="caption">Email</span>
                        </div>
                    </div>
                    <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                        <div class="entrada">
                            <asp:TextBox ID="InputClaveActualCambiarClave" runat="server" MaxLength="100" ValidationGroup="ValidationCambiarClave" autocomplete="off" TextMode="Password" ToolTip="Entrada de datos para la contraseña" />
                            <span class="caption">Clave actual</span>
                        </div>
                    </div>
                    <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                        <div class="entrada">
                            <asp:TextBox ID="InputClaveNueva1CambiarClave" runat="server" MaxLength="100" ValidationGroup="ValidationCambiarClave" autocomplete="off" TextMode="Password" ToolTip="Entrada de datos para la contraseña" />
                            <span class="caption">Clave nueva</span>
                        </div>
                    </div>
                    <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                        <div class="entrada">
                            <asp:TextBox ID="InputClaveNueva2CambiarClave" runat="server" MaxLength="100" ValidationGroup="ValidationCambiarClave" autocomplete="off" TextMode="Password" ToolTip="Entrada de datos para la contraseña" />
                            <span class="caption">Repetir clave nueva</span>
                        </div>
                    </div>
                </div>

                <!-- Botón -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                        <asp:Button ID="ButtonCambiarClave" runat="server" Text="Cambiar" ValidationGroup="ValidationCambiarClave" OnClick="ButtonCambiarClave_Click" CssClass="boton" />
                    </div>
                </div>

                <!-- Campos obligatorios -->
                <asp:RequiredFieldValidator runat="server" ValidationGroup="ValidationCambiarClave" ControlToValidate="InputEmailCambiarClave"       Display="None" SetFocusOnError="true" />
                <asp:RequiredFieldValidator runat="server" ValidationGroup="ValidationCambiarClave" ControlToValidate="InputClaveActualCambiarClave" Display="None" SetFocusOnError="true" />
                <asp:RequiredFieldValidator runat="server" ValidationGroup="ValidationCambiarClave" ControlToValidate="InputClaveNueva1CambiarClave" Display="None" SetFocusOnError="true" />
                <asp:RequiredFieldValidator runat="server" ValidationGroup="ValidationCambiarClave" ControlToValidate="InputClaveNueva2CambiarClave" Display="None" SetFocusOnError="true" />

                <!-- Validaciones personalizadas -->
                <asp:CustomValidator runat="server" ValidationGroup="ValidationCambiarClave" OnServerValidate="ValidarEmail"           ControlToValidate="InputEmailCambiarClave"       />
                <asp:CustomValidator runat="server" ValidationGroup="ValidationCambiarClave" OnServerValidate="ValidarClavesDistintas" ControlToValidate="InputClaveActualCambiarClave" />
                <asp:CustomValidator runat="server" ValidationGroup="ValidationCambiarClave" OnServerValidate="ValidarClavesIguales"   ControlToValidate="InputClaveNueva1CambiarClave" />

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!--
    ╔════════════════════════╗
    ║     RECORDAR CLAVE     ║
    ╚════════════════════════╝
    -->

    <!-- Formulario -->
    <div id="recordar-clave">
        <asp:UpdatePanel ID="UpdateRecordarClave" runat="server">
            <ContentTemplate>

                <!-- Entradas -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                        <div class="entrada">
                            <asp:TextBox ID="InputEmailRecordarClave" runat="server" MaxLength="100" ValidationGroup="ValidationRecordarClave" autocomplete="off" ToolTip="Entrada de datos para la contraseña" />
                            <span class="caption">Email</span>
                        </div>
                    </div>
                </div>

                <!-- Botón -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                        <asp:Button runat="server" Text="Recordar" ValidationGroup="ValidationRecordarClave" OnClick="ButtonRecordarClave_Click" CssClass="boton" />
                    </div>
                </div>

                <!-- Campos obligatorios -->
                <asp:RequiredFieldValidator runat="server" ValidationGroup="ValidationRecordarClave" ControlToValidate="InputEmailRecordarClave" Display="None" SetFocusOnError="true" />

                <!-- Validaciones personalizadas -->
                <asp:CustomValidator runat="server" ValidationGroup="ValidationRecordarClave" OnServerValidate="ValidarEmail" ControlToValidate="InputEmailRecordarClave" />

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!--
    ╔═════════════════════════╗
    ║     ELIMINAR CUENTA     ║
    ╚═════════════════════════╝
    -->

    <!-- Formulario -->
    <div id="eliminar-cuenta">
        <asp:UpdatePanel ID="UpdateEliminarCuenta" runat="server">
            <ContentTemplate>

                <!-- Entradas -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                        <div class="entrada">
                            <asp:TextBox ID="InputEmailEliminarCuenta" runat="server" MaxLength="100" ValidationGroup="ValidationEliminarCuenta" autocomplete="off" ToolTip="Entrada de datos para el email" />
                            <span class="caption">Email</span>
                        </div>
                    </div>
                    <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                        <div class="entrada">
                            <asp:TextBox ID="InputClaveEliminarCuenta" runat="server" MaxLength="100" ValidationGroup="ValidationEliminarCuenta" autocomplete="off" ToolTip="Entrada de datos para la contraseña" TextMode="Password" />
                            <span class="caption">Contraseña</span>
                        </div>
                    </div>
                </div>

                <!-- Botón -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                        <asp:Button runat="server" Text="Eliminar" ValidationGroup="ValidationEliminarCuenta" OnClick="ButtonEliminarCuenta_Click" CssClass="boton" />
                    </div>
                </div>

                <!-- Campos obligatorios -->
                <asp:RequiredFieldValidator runat="server" ValidationGroup="ValidationEliminarCuenta" ControlToValidate="InputEmailEliminarCuenta" Display="None" SetFocusOnError="true" />
                <asp:RequiredFieldValidator runat="server" ValidationGroup="ValidationEliminarCuenta" ControlToValidate="InputClaveEliminarCuenta" Display="None" SetFocusOnError="true" />

                <!-- Validaciones personalizadas -->
                <asp:CustomValidator runat="server" ValidationGroup="ValidationEliminarCuenta" OnServerValidate="ValidarAdministrador" ControlToValidate="InputEmailEliminarCuenta"/>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!--
    ╔═════════════════╗
    ║     SPINNER     ║
    ╚═════════════════╝
    -->

    <!-- Cambiar de clave -->
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateCambiarClave">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <!-- Recordar clave -->
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateRecordarClave">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <!-- Eliminar cuenta -->
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateEliminarCuenta">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>