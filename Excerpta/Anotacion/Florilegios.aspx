<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Florilegios.aspx.cs" Inherits="Excerpta.Florilegios" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="Notificacion" runat="server" Visible="false" CssClass="notificacion" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Pestañas -->
    <ul class="pestanas">
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/Extractos.aspx"    Text="Extractos"    />
        </li>
        <li class="active">
            <asp:HyperLink runat="server" NavigateUrl="~/Florilegios.aspx"  Text="Florilegios"  />
        </li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/Usuarios.aspx"     Text="Usuarios"     />
        </li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/Bibliografia.aspx" Text="Bibliografía" />
        </li>
    </ul>

    <!-- Encabezado -->
    <div class="encabezado margin-top-125-mobile">
        <h2>Florilegios</h2>
        <h3>Administración de la estructura interna de los florilegios</h3>
    </div>

    <hr />

    <!--
    ╔═════════════════════╗
    ║     FLORILEGIOS     ║
    ╚═════════════════════╝
    -->

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelFlorilegios" runat="server">

                <!-- Botón "Abrir" -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-3 col-md-4 col-md-offset-4">
                        <asp:Button ID="BotonAbrirFlorilegio" runat="server" CausesValidation="false" Text="Abrir" Enabled="false" OnClick="BotonAbrirFlorilegio_Click" CssClass="boton margin-bottom-35" />
                    </div>
                </div>

                <!-- Recuento -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
                        <asp:Label ID="RecuentoFlorilegios" runat="server" CssClass="recuento right" />
                    </div>
                </div>

                <!-- Tabla -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
                        <div class="tabla max-height not-header hide-last-column margin-bottom-35">
                            <asp:GridView ID="TablaFlorilegios" runat="server" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" DataKeyNames="Título" DataSourceID="DataFlorilegios" EmptyDataText="No se han encontrado datos" GridLines="None" OnRowCommand="TablaFlorilegios_RowCommand" ShowHeader="false" >
                                <Columns>
                                    <asp:BoundField DataField="Título" />
                                    <asp:CommandField ButtonType="Link" ShowSelectButton="true" SelectText="Seleccionar" ControlStyle-CssClass="seleccionar" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <!-- Entrada oculta -->
                <asp:TextBox ID="TituloFlorilegio" runat="server" Enabled="false" CssClass="hidden" ToolTip="Entrada de datos oculta" />

                <!-- Entrada -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-6 col-md-offset-3">
                        <div class="entrada margin-0">
                            <asp:TextBox ID="InputFlorilegio" runat="server" MaxLength="100" ValidationGroup="ValidationGuardarFlorilegio" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el florilegio" />
                            <span class="caption">Florilegio</span>
                        </div>
                    </div>
                </div>

                <!-- Botones -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-2">
                        <asp:Button ID="BotonGuardarFlorilegio" runat="server" Text="Guardar" OnClick="ButtonGuardarFlorilegio_Click" CssClass="boton margin-top-35" ValidationGroup="ValidationGuardarFlorilegio"  />
                    </div>
                    <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-0">
                        <asp:Button ID="BotonEliminarFlorilegio" runat="server" Text="Eliminar" OnClick="ButtonEliminarFlorilegio_Click" CssClass="boton margin-top-35" ValidationGroup="ValidationEliminarFlorilegio" Enabled="false" OnClientClick="return confirm('¿Estás seguro? Esta acción no puede deshacerse')" />
                    </div>
                </div>

                <!-- Campos obligatorios -->
                <asp:RequiredFieldValidator runat="server" ValidationGroup="ValidationGuardarFlorilegio" ControlToValidate="inputFlorilegio"  Display="None" SetFocusOnError="true" />

                <!-- Caracteres permitidos -->
                <asp:FilteredTextBoxExtender runat="server" TargetControlID="TituloFlorilegio" ValidChars=" abcdefghijklmnñopqrstuvwxzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚ0123456789.,:;?!/()[]" />

                <!-- Validaciones personalizadas -->
                <asp:CustomValidator runat="server" OnServerValidate="ValidarFlorilegioVacio"    ControlToValidate="InputFlorilegio" ValidationGroup="ValidationEliminarFlorilegio" />
                <asp:CustomValidator runat="server" OnServerValidate="ValidarFlorilegioRepetido" ControlToValidate="InputFlorilegio" ValidationGroup="ValidationGuardarFlorilegio"  />

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!--
    ╔═══════════════════╗
    ║     REGISTROS     ║
    ╚═══════════════════╝
    -->

    <asp:UpdatePanel ID="UpdatePanelRegistros" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <asp:Panel ID="PanelRegistros" runat="server" Visible="false">
                
                <!-- Botón "Volver" -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-3 col-md-4 col-md-offset-4">
                        <asp:Button ID="ButtonVolver" runat="server" Text="Volver" OnClick="ButtonVolver_Click" CssClass="boton margin-bottom-35" />
                    </div>
                </div>

                <!-- Florilegio seleccionado y recuento -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="recuento">
                                    <asp:Label ID="FlorilegioSeleccionado" runat="server" />
                                    <asp:Label ID="RecuentoRegistros"      runat="server" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <!-- Tabla de usuarios -->
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
                        <div class="">
                            <asp:UpdatePanel ID="UpdateTablaRegistros" runat="server" UpdateMode="Conditional" class="tabla max-height hide-first-column hide-last-column margin-bottom-35">
                                <ContentTemplate>
                                    <asp:GridView ID="TablaRegistros" runat="server" AllowSorting="true" AllowPaging="false" AutoGenerateColumns="false" DataKeyNames="ID" DataSourceID="DataRegistros" EmptyDataText="No se han encontrado datos" GridLines="None" OnRowCommand="TablaRegistros_RowCommand" ShowHeader="true" >
                                        <Columns>
                                            <asp:BoundField DataField="ID"    SortExpression="ID" />
                                            <asp:BoundField DataField="Autor" SortExpression="Autor" HeaderText="Autor" ItemStyle-Width="25%" />
                                            <asp:BoundField DataField="Obra"  SortExpression="Obra"  HeaderText="Obra"  ItemStyle-Width="25%" />
                                            <asp:BoundField DataField="Libro" SortExpression="Libro" HeaderText="Libro" ItemStyle-Width="25%" />
                                            <asp:BoundField DataField="Poema" SortExpression="Poema" HeaderText="Poema" ItemStyle-Width="25%" />
                                            <asp:CommandField ButtonType="Link" ShowSelectButton="true" SelectText="Seleccionar" ControlStyle-CssClass="seleccionar" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>

                <asp:UpdatePanel runat="server">
                    <ContentTemplate>

                        <!-- Entrada oculta -->
                        <asp:TextBox ID="RegistroSeleccionado" runat="server" Enabled="false" CssClass="hidden" ToolTip="Entrada de datos oculta" />

                        <!-- Entradas -->
                        <div class="row">
                            <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                                <div class="entrada">
                                    <asp:TextBox ID="InputAutor" runat="server" MaxLength="100" ValidationGroup="ValidationGuardarRegistro" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el autor" />
                                    <span class="caption">Autor</span>
                                </div>
                            </div>
                            <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                                <div class="entrada">
                                    <asp:TextBox ID="InputObra" runat="server" MaxLength="100" ValidationGroup="ValidationGuardarRegistro" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para la obra" />
                                    <span class="caption">Obra</span>
                                </div>
                            </div>
                            <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                                <div class="entrada">
                                    <asp:TextBox ID="InputLibro" runat="server" MaxLength="100" ValidationGroup="ValidationGuardarRegistro" autocomplete="off" spellcheck="false" placeholder="valor mínimo ; valor máximo" ToolTip="Entrada de datos para el libro" />
                                    <span class="caption">Libro</span>
                                </div>
                            </div>
                            <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                                <div class="entrada">
                                    <asp:TextBox ID="InputPoema" runat="server" MaxLength="100" ValidationGroup="ValidationGuardarRegistro" autocomplete="off" spellcheck="false" placeholder="valor mínimo ; valor máximo" ToolTip="Entrada de datos para el poema" />
                                    <span class="caption">Poema</span>
                                </div>
                            </div>
                        </div>

                        <!-- Botones -->
                        <div class="row">
                            <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-2">
                                <asp:Button ID="BotonGuardarRegistro"  runat="server" Text="Guardar"  ValidationGroup="ValidationGuardarRegistro"  OnClick="ButtonGuardarRegistro_Click"  CssClass="boton margin-bottom-35" />
                            </div>
                            <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-0">
                                <asp:Button ID="BotonEliminarRegistro" runat="server" Text="Eliminar" ValidationGroup="ValidationEliminarRegistro" OnClick="ButtonEliminarRegistro_Click" CssClass="boton" Enabled="false" OnClientClick="return confirm('¿Estás seguro? Esta acción no puede deshacerse')" />
                            </div>
                        </div>

                        <!-- Campos obligatorios -->
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="InputAutor" ValidationGroup="ValidationGuardarRegistro" Display="None" SetFocusOnError="true" />

                        <!-- Caracteres permitidos -->
                        <asp:FilteredTextBoxExtender runat="server" TargetControlID="InputLibro" ValidChars=";0123456789" />
                        <asp:FilteredTextBoxExtender runat="server" TargetControlID="InputPoema" ValidChars=";0123456789" />

                        <!-- Validaciones personalizadas -->
                        <asp:CustomValidator runat="server" OnServerValidate="ValidarEstructuraVacia"    ControlToValidate="InputAutor" ValidationGroup="ValidationEliminarRegistro" />
                        <asp:CustomValidator runat="server" OnServerValidate="ValidarEstructuraRepetida" ControlToValidate="InputAutor" ValidationGroup="ValidationGuardarRegistro"  />

                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!--
    ╔═════════════════════════╗
    ║     ORIGEN DE DATOS     ║
    ╚═════════════════════════╝
    -->

    <!-- Tabla de florilegios -->
    <asp:AccessDataSource ID="DataFlorilegios" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" SelectCommand="SELECT * FROM [Florilegios] ORDER BY [Título] ASC" OnSelected="DataFlorilegios_Selected" />

    <!-- Tabla de registros -->
    <asp:AccessDataSource ID="DataRegistros" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" OnSelected="DataRegistros_Selected" />

</asp:Content>
