<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="Excerpta.Usuarios" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdateUsuarios" runat="server">
        <ContentTemplate>

            <asp:Panel ID="Notificacion" runat="server" Visible="false" CssClass="notificacion" />

            <!-- Pestañas -->
            <ul class="pestanas">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Extractos.aspx"    Text="Extractos"    />
                </li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Florilegios.aspx"  Text="Florilegios"  />
                </li>
                <li class="active">
                    <asp:HyperLink runat="server" NavigateUrl="~/Usuarios.aspx"     Text="Usuarios"     />
                </li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Bibliografia.aspx" Text="Bibliografía" />
                </li>
            </ul>

            <!-- Encabezado -->
            <div class="encabezado margin-top-125-mobile">
                <h2>Usuarios</h2>
                <h3>Administración de usuarios registrados y pendientes de alta</h3>
            </div>

            <hr />

            <!--
            ╔═══════════════╗
            ║     TABLA     ║
            ╚═══════════════╝
            -->

            <asp:UpdatePanel ID="UpdateTabla" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <!-- Filtro -->
                    <div class="row">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-6 col-md-offset-3">
                            <div class="selector">
                                <asp:DropDownList ID="DropUsuarios" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropUsuarios_SelectedIndexChanged" ToolTip="Selector de usuarios" >
                                    <Items>
                                        <asp:ListItem Text="Todos los usuarios"          Value="1" Selected="True" />
                                        <asp:ListItem Text="Usuarios dados de alta"      Value="2" />
                                        <asp:ListItem Text="Usuarios pendientes de alta" Value="3" />
                                    </Items>
                                </asp:DropDownList>
                                <span class="caption">Filtrar usuarios&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                            </div>
                        </div>
                    </div>

                    <!-- Recuento -->
                    <div class="row">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
                            <asp:Label ID="Recuento" runat="server" CssClass="recuento right" />
                        </div>
                    </div>

                    <!-- Tabla de usuarios -->
                    <div class="row">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
                            <div class="tabla hide-last-column margin-bottom-35">
                                <asp:GridView ID="TablaUsuarios" runat="server" AllowPaging="false" AutoGenerateColumns="false" DataKeyNames="Email" DataSourceID="DataUsuarios" EmptyDataText="No se han encontrado datos" GridLines="None" OnRowDataBound="TablaUsuarios_RowDataBound" OnRowCommand="TablaUsuarios_RowCommand" >
                                    <Columns>
                                        <asp:BoundField   DataField="Alta"      SortExpression="Alta"      HeaderText="Alta"      />
                                        <asp:BoundField   DataField="Nombre"    SortExpression="Nombre"    HeaderText="Nombre"    />
                                        <asp:BoundField   DataField="Apellidos" SortExpression="Apellidos" HeaderText="Apellidos" />
                                        <asp:BoundField   DataField="Email"     SortExpression="Email"     HeaderText="Email"     />
                                        <asp:BoundField   DataField="Centro"    SortExpression="Centro"    HeaderText="Centro"    />
                                        <asp:BoundField   DataField="Teléfono"  SortExpression="Teléfono"  HeaderText="Teléfono"  />
                                        <asp:CommandField ButtonType="Link" ShowSelectButton="true" SelectText="Seleccionar" ControlStyle-CssClass="seleccionar" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

            <!--
            ╔════════════════════╗
            ║     FORMULARIO     ║
            ╚════════════════════╝
            -->

            <!-- Entrada oculta -->
            <asp:TextBox ID="InputEmails" runat="server" Enabled="false" CssClass="hidden" ToolTip="Entrada oculta para los emails" />

            <!-- Botones -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-2">
                    <asp:Button ID="ButtonAlta" runat="server" Text="Dar de alta" OnClick="ButtonAlta_Click" Enabled="false" CssClass="boton margin-bottom-35" />
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-0">
                    <asp:Button ID="ButtonBaja" runat="server" Text="Dar de baja" OnClick="ButtonBaja_Click" Enabled="false" CssClass="boton margin-bottom-35" OnClientClick="return confirm('¿Estás seguro? Esta acción no puede deshacerse')" />
                </div>
            </div>

            <!--
            ╔═════════════════╗
            ║     GRÁFICO     ║
            ╚═════════════════╝
            -->

            <div class="separador">
                <h5>Representación gráfica</h5>
            </div>
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
                    <asp:Chart ID="ChartUsuarios" runat="server" Width="550">
                        <Legends>
                            <asp:Legend Alignment="Center" />
                        </Legends>
                        <ChartAreas>
                            <asp:ChartArea Name="AreaUsuarios" />
                        </ChartAreas>
                    </asp:Chart>
                </div>
            </div>

            <!--
            ╔═══════════════════╗
            ║     ACTIVIDAD     ║
            ╚═══════════════════╝
            -->

            <!-- Botón -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                    <asp:Button ID="ButtonVerActividad" runat="server" Text="Ver actividad" OnClick="ButtonVerActividad_Click" CssClass="boton margin-top-35"/>
                </div>
            </div>

            <!-- Tabla de actividad -->
            <asp:GridView ID="ActividadUser" runat="server" AutoGenerateColumns="True" AllowPaging="false" CssClass="margin-top-35" GridLines="None" Width="100%">
                <HeaderStyle BackColor="#84bbed" BorderColor="#DDDDDD" BorderStyle="Solid" BorderWidth="1" Height="40" HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="Black" />
                <RowStyle BackColor="Transparent" BorderColor="#DDDDDD" BorderStyle="Solid" BorderWidth="1" Height="40" HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="#666666" />
                <AlternatingRowStyle BackColor="#bad7fd" />
            </asp:GridView>
            <asp:Label ID="Resultados" runat="server" Text="" Visible="false" Width="100%" CssClass="label-warning"></asp:Label>

        </ContentTemplate>
    </asp:UpdatePanel>

    <!--
    ╔═════════════════════════╗
    ║     ORIGEN DE DATOS     ║
    ╚═════════════════════════╝
    -->

    <!-- Tabla de usuarios -->
    <asp:AccessDataSource ID="DataUsuarios" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" SelectCommand="SELECT * FROM Usuarios" OnSelected="DataUsuarios_Selected" />

    <!--
    ╔═════════════════╗
    ║     SPINNER     ║
    ╚═════════════════╝
    -->

    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateUsuarios">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
