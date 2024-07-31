<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bibliografia.aspx.cs" Inherits="Excerpta.Bibliografia" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <!-- Pestañas -->
    <ul class="pestanas">
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/Extractos.aspx"    Text="Extractos"    />
        </li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/Florilegios.aspx"  Text="Florilegios"  />
        </li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/Usuarios.aspx"     Text="Usuarios"     />
        </li>
        <li class="active">
            <asp:HyperLink runat="server" NavigateUrl="~/Bibliografia.aspx" Text="Bibliografía" />
        </li>
    </ul>

    <!-- Encabezado -->
    <div class="encabezado margin-top-125-mobile">
        <h2>Bibliografía</h2>
        <h3>Administración de fuentes bibliográficas publicadas en la aplicación web de búsqueda</h3>
    </div>

    <hr />

    <!--
    ╔═══════════════╗
    ║     TABLA     ║
    ╚═══════════════╝
    -->

    <!-- Recuento -->
    <div class="row">
        <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
            <asp:Label ID="Recuento" runat="server" CssClass="recuento right" />
        </div>
    </div>

    <!-- Tabla -->
    <div class="row">
        <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
            <asp:UpdatePanel ID="UpdateTabla" runat="server" UpdateMode="Conditional" class="tabla hide-first-column hide-last-column max-height scroll wide margin-bottom-35">
                <ContentTemplate>
                    <asp:GridView ID="TablaBibliografia" runat="server" AllowSorting="true" AutoGenerateColumns="false" DataKeyNames="ID" DataSourceID="DataBibliografia" EmptyDataText="No se han encontrado datos" GridLines="None" OnRowCommand="TablaBibliografia_RowCommand" ShowHeader="true">
                        <Columns>
                            <asp:BoundField DataField="ID" />
                            <asp:BoundField DataField="Bibliografía" SortExpression="Bibliografía" HeaderText="Bibliografía" />
                            <asp:CommandField ButtonType="Link" ShowSelectButton="true" SelectText="Seleccionar" ControlStyle-CssClass="seleccionar" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!--
    ╔════════════════════╗
    ║     FORMULARIO     ║
    ╚════════════════════╝
    -->

    <asp:UpdatePanel ID="UpdateFormulario" runat="server">
        <ContentTemplate>

            <asp:Panel ID="Notificacion" runat="server" Visible="false" CssClass="notificacion" />

            <!-- Entradas -->
            <asp:TextBox ID="InputID" runat="server" Enabled="false" CssClass="hidden" ToolTip="Entrada de datos oculta" />
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2">
                    <div class="entrada margin-0">
                        <asp:TextBox ID="InputBibliografia" runat="server" MaxLength="1000" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para las fuentes bibliográficas" />
                        <span class="caption">Fuente bibliográfica</span>
                    </div>
                </div>
            </div>

            <!-- Botones -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-2">
                    <asp:Button ID="ButtonGuardar" runat="server" Text="Guardar" ValidationGroup="ValidationGuardar" CssClass="boton margin-top-35" OnClick="BotonGuardarBibliografia_Click" />
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-0">
                    <asp:Button ID="ButtonEliminar" runat="server" Text="Eliminar" ValidationGroup="ValidationGuardar" CssClass="boton margin-top-35" OnClick="BotonEliminarBibliografia_Click" Enabled="false" OnClientClick = "return confirm('¿Está seguro? Esta acción no puede deshacerse')" />
                </div>
            </div>

            <!-- Campos obligatorios -->
            <asp:RequiredFieldValidator  runat="server" ControlToValidate="InputBibliografia" ValidationGroup="ValidationGuardar" Display="None" SetFocusOnError="true" />
                
            <!-- Caracteres permitidos -->
            <asp:FilteredTextBoxExtender runat="server" TargetControlID="InputID"             ValidChars="0123456789" />
            <asp:FilteredTextBoxExtender runat="server" TargetControlID="InputBibliografia"   ValidChars=" abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚàèìòùÀÈÌÒÙäëïöüÄËÏÖÜ0123456789.,:;¿?¡!/()[]'-ºª" />

        </ContentTemplate>
    </asp:UpdatePanel>

    <!--
    ╔═════════════════════════╗
    ║     ORIGEN DE DATOS     ║
    ╚═════════════════════════╝
    -->

    <!-- Tabla de bibliografía -->
    <asp:AccessDataSource ID="DataBibliografia" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" SelectCommand="SELECT ID, [Bibliografía] FROM Biblioteca ORDER BY [Bibliografía]" OnSelected="DataBiliografia_Selected" />

    <!--
    ╔═════════════════╗
    ║     SPINNER     ║
    ╚═════════════════╝
    -->

    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateFormulario">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
