<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MisExtractos.aspx.cs" Inherits="Excerpta.MisExtractos" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <!-- Pestañas -->
    <ul class="pestanas">
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/AnotarExtracto.aspx" Text="Anotar extracto" />
        </li>
        <li class="active">
            <asp:HyperLink runat="server" NavigateUrl="~/MisExtractos.aspx" Text="Mis extractos" />
        </li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/Ayuda.aspx" Text="Ayuda" />
        </li>
    </ul>

    <!-- Encabezado -->
    <div class="encabezado margin-top-80-mobile">
        <h2>Mis extractos</h2>
        <h3>Modifique o elimine sus extractos que no hayan sido publicados</h3>
    </div>

    <hr />

    <!--
    ╔════════════════╗
    ║     FILTRO     ║
    ╚════════════════╝
    -->

    <div class="separador">
        <h5>Filtro de búsqueda</h5>
    </div>

    <asp:UpdatePanel ID="UpdateFiltro" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <!-- Selector de usuarios -->
            <asp:Panel ID="MostrarFiltroUsuarios" runat="server" CssClass="row" Visible="false">
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-3">
                    <div class="selector">
                        <asp:DropDownList ID="DropFiltroUsuarios" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataFiltroUsuarios" DataTextField="Usuario" DataValueField="Usuario" OnSelectedIndexChanged="DropFiltroUsuarios_SelectedIndexChanged" ToolTip="Selector de usuarios">
                            <asp:ListItem Text="Todos los usuarios" Value="%" Selected="True"  />
                        </asp:DropDownList>
                        <span class="caption">Usuario&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>
            </asp:Panel>
            <div class="row">

                <!-- Selector de florilegios -->
                <div class="col-xs-10 col-xs-offset-1 col-sm-3 col-sm-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropFiltroFlorilegios" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataFiltroFlorilegios" DataTextField="Florilegio" DataValueField="Florilegio" OnSelectedIndexChanged="DropFiltroFlorilegios_SelectedIndexChanged" ToolTip="Selector de florilegios" />
                        <span class="caption">Florilegio&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>

                <!-- Selector de autores -->
                <div class="col-xs-10 col-xs-offset-1 col-sm-3 col-sm-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropFiltroAutores" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataFiltroAutores" DataTextField="Autor" DataValueField="Autor" OnSelectedIndexChanged="DropFiltroAutores_SelectedIndexChanged" ToolTip="Selector de autores" />
                        <span class="caption">Autor&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>

                <!-- Selector de obras -->
                <div class="col-xs-10 col-xs-offset-1 col-sm-3 col-sm-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropFiltroObras" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataFiltroObras" DataTextField="Obra" DataValueField="Obra" OnSelectedIndexChanged="DropFiltroObras_SelectedIndexChanged" ToolTip="Selector de obras" />
                        <span class="caption">Obra&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>

                <!-- Selector de estados -->
                <div class="col-xs-10 col-xs-offset-1 col-sm-3 col-sm-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropFiltroEstados" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="DropFiltroEstados_SelectedIndexChanged" ToolTip="Selector de estados">
                            <asp:ListItem Text="Todos los extractos"     Value="%" Selected="True" />
                            <asp:ListItem Text="Extractos publicados"    Value="-1" />
                            <asp:ListItem Text="Extractos despublicados" Value="0"  />
                        </asp:DropDownList>
                        <span class="caption">Estado&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!--
    ╔═══════════════╗
    ║     TABLA     ║
    ╚═══════════════╝
    -->

    <!-- Recuento -->
    <div class="row">
        <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
            <asp:UpdatePanel ID="UpdateRecuento" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="RecuentoExtractos" runat="server" CssClass="recuento right" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Tabla -->
    <div class="row">
        <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
            <asp:UpdatePanel ID="UpdateTabla" runat="server" UpdateMode="Conditional" class="tabla hide-last-column max-height margin-bottom-35">
                <ContentTemplate>
                    <asp:GridView ID="TablaExtractos" runat="server" DataSourceID="DataExtractos" AllowPaging="false" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="None" EmptyDataText="No se han encontrado datos" OnRowCommand="TablaExtractos_RowCommand" OnRowDataBound="TablaExtractos_RowDataBound" PageSize="50" PagerSettings-PageButtonCount="5" PagerStyle-CssClass="paginador" ShowHeader="true">
                        <Columns>
                            <asp:BoundField DataField="ID"           SortExpression="ID"           HeaderText="ID"            />
                            <asp:BoundField DataField="Alta"         SortExpression="Alta"         HeaderText="Publicado"     />
                            <asp:BoundField DataField="Florilegio"   SortExpression="Florilegio"   HeaderText="Florilegio"    />
                            <asp:BoundField DataField="Autor"        SortExpression="Autor"        HeaderText="Autor"         />
                            <asp:BoundField DataField="Obra"         SortExpression="Obra"         HeaderText="Obra"          />
                            <asp:BoundField DataField="Libro"        SortExpression="Libro"        HeaderText="Libro"         />
                            <asp:BoundField DataField="Poema"        SortExpression="Poema"        HeaderText="Poema"         />
                            <asp:BoundField DataField="Capítulo"     SortExpression="Capítulo"     HeaderText="Capítulo"      />
                            <asp:BoundField DataField="Subcapítulo"  SortExpression="Subcapítulo"  HeaderText="Subcapítulo"   />
                            <asp:BoundField DataField="VersoInicial" SortExpression="VersoInicial" HeaderText="Verso Inicial" />
                            <asp:BoundField DataField="VersoFinal"   SortExpression="VersoFinal"   HeaderText="Verso Final"   />
                            <asp:BoundField DataField="Extracto"     SortExpression="Extracto"     HeaderText="Extracto"      />
                            <asp:BoundField DataField="TLL"          SortExpression="TLL"          HeaderText="TLL"           />
                            <asp:BoundField DataField="Vernácula"    SortExpression="Vernácula"    HeaderText="Vernácula"     />
                            <asp:BoundField DataField="Página"       SortExpression="Página"       HeaderText="Página"        />
                            <asp:CommandField ButtonType="Link" ShowSelectButton="true" SelectText="Seleccionar" ControlStyle-CssClass="seleccionar" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>

            <!-- PDF -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                    <a ID="BotonPDF" runat="server" href="#" target="_blank" class="boton margin-bottom-35">Generar PDF</a>
                </div>
            </div>
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

            <div class="separador">
                <h5>Edición de extractos</h5>
            </div>

            <!-- Entrada oculta -->
            <asp:TextBox ID="ExtractoSeleccionado" runat="server" Enabled="false" CssClass="hidden" ToolTip="Entrada de datos oculta" />

            <!-- Florilegio, autor, obra y página -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropFlorilegios" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataFlorilegios" DataTextField="Florilegio" DataValueField="Florilegio" Enabled="false" OnSelectedIndexChanged="DropFlorilegios_SelectedIndexChanged" ToolTip="Selector de florilegios" />
                        <span class="caption">Florilegio&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropAutores" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataAutores" DataTextField="Autor" DataValueField="Autor" Enabled="false" OnSelectedIndexChanged="DropAutores_SelectedIndexChanged" ToolTip="Selector de autores" />
                        <span class="caption">Autores&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropObras" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataObras" DataTextField="Obra" DataValueField="Obra" Enabled="false" ToolTip="Selector de obras" />
                        <span class="caption">Obras&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputPagina" runat="server" Enabled="false" MaxLength="10" autocomplete="off" ToolTip="Entrada de datos para la página" />
                        <span class="caption">Página</span>
                    </div>
                </div>
            </div>

            <!-- Libro, poema y versos -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputLibro" runat="server" Enabled="false" MaxLength="6" autocomplete="off" ToolTip="Entrada de datos para el libro" />
                        <span class="caption">Libro</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputPoema" runat="server" Enabled="false" MaxLength="6" autocomplete="off" ToolTip="Entrada de datos para el poema" />
                        <span class="caption">Poema</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputVersoInicial" runat="server" Enabled="false" MaxLength="6" autocomplete="off" ToolTip="Entrada de datos para el verso inicial" />
                        <span class="caption">Verso inicial</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputVersoFinal" runat="server" Enabled="false" MaxLength="6" autocomplete="off" ToolTip="Entrada de datos para el verso final" />
                        <span class="caption">Verso final</span>
                    </div>
                </div>
            </div>

            <!-- Capítulo y subcapítulo -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-6 col-md-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputCapitulo" runat="server" MaxLength="250" Enabled="false" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para ek capítulo" />
                        <span class="caption">Capítulo</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-6 col-md-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputSubcapitulo" runat="server" MaxLength="250" Enabled="false" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el subcapítulo" />
                        <span class="caption">Subcapítulo</span>
                    </div>
                </div>
            </div>

            <!-- Extracto, TLL y vernácula -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
                    <div class="owl-carousel owl-theme loop">
                        <div class="item">
                            <div class="entrada">
                                <asp:TextBox ID="InputExtracto" runat="server" TextMode="MultiLine" Enabled="false" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el extracto" />
                                <span class="caption">Extracto</span>
                            </div>
                        </div>
                        <div class="item">
                            <div class="entrada">
                                <asp:TextBox ID="InputTLL" runat="server" TextMode="MultiLine" Enabled="false" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el TLL" />
                                <span class="caption">TLL</span>
                            </div>
                        </div>
                        <div class="item">
                            <div class="entrada">
                                <asp:TextBox ID="InputVernacula" runat="server" TextMode="MultiLine" Enabled="false" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para la lengua vernácula" />
                                <span class="caption">Vernácula</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Botones -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-2">
                    <asp:Button ID="BotonGuardar"  runat="server" Text="Editar"  Enabled="false" CssClass="boton margin-bottom-35" OnClick="BotonEditarExtracto_Click" />
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-0">
                    <asp:Button ID="BotonEliminar" runat="server" Text="Eliminar" Enabled="false" CssClass="boton margin-bottom-35" OnClick="BotonEliminarExtracto_Click" OnClientClick = "return confirm('¿Está seguro? Esta acción no puede deshacerse')" />
                </div>
            </div>

            <!-- Campos obligatorios -->
            <asp:RequiredFieldValidator     runat="server" ControlToValidate="inputLibro"        Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator     runat="server" ControlToValidate="inputPoema"        Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator     runat="server" ControlToValidate="inputVersoInicial" Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator     runat="server" ControlToValidate="inputVersoFinal"   Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator     runat="server" ControlToValidate="inputExtracto"     Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator     runat="server" ControlToValidate="inputTLL"          Display="None" SetFocusOnError="true" />

            <!-- Apaño de MaxLenght -->
            <%--<asp:RegularExpressionValidator runat="server" ControlToValidate="inputExtracto"     Display="None" SetFocusOnError="true" ValidationExpression="^[\s\S]{0,5000}$" />            
            <asp:RegularExpressionValidator runat="server" ControlToValidate="inputTLL"          Display="None" SetFocusOnError="true" ValidationExpression="^[\s\S]{0,5000}$" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="InputVernacula"    Display="None" SetFocusOnError="true" ValidationExpression="^[\s\S]{0,5000}$" />--%>

            <!-- Caracteres permitidos -->
            <asp:FilteredTextBoxExtender    runat="server" TargetControlID="InputLibro"          ValidChars="0123456789" />
            <asp:FilteredTextBoxExtender    runat="server" TargetControlID="InputPoema"          ValidChars="0123456789" />
            <asp:FilteredTextBoxExtender    runat="server" TargetControlID="InputVersoInicial"   ValidChars="0123456789" />
            <asp:FilteredTextBoxExtender    runat="server" TargetControlID="InputVersoFinal"     ValidChars="0123456789" />
            <asp:FilteredTextBoxExtender    runat="server" TargetControlID="InputPagina"         ValidChars="0123456789" />

            <!-- Validaciones personalizadas -->
            <asp:CustomValidator runat="server" OnServerValidate="ValidarVersos"    />
            <asp:CustomValidator runat="server" OnServerValidate="ValidarPublicado" />

        </ContentTemplate>
    </asp:UpdatePanel>

    <!--
    ╔═════════════════╗
    ║     GRÁFICO     ║
    ╚═════════════════╝
    -->
    
    <div class="separador">
        <h5>Representación gráfica</h5>
    </div>
    
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
                    <div class="tabla recuento-florilegios margin-bottom-35">
                        <asp:GridView ID="TablaRecuento" runat="server" DataSourceID="DataRecuento" ShowHeader="true" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" GridLines="None" EmptyDataText="No se han encontrado datos">
                            <Columns>
                                <asp:BoundField DataField="Florilegio" SortExpression="Florilegio" HeaderText="Florilegio" />
                                <asp:BoundField DataField="Recuento"   SortExpression="Recuento"   HeaderText="Recuento"   />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
                    <div style="overflow-x: auto">
                        <asp:Chart ID="ChartFlorilegios" runat="server" Width="700" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!--
    ╔═════════════════════════╗
    ║     ORIGEN DE DATOS     ║
    ╚═════════════════════════╝
    -->

    <!-- Tabla de extractos -->
    <asp:AccessDataSource ID="DataExtractos" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" OnSelected="DataExtractos_Selected" />

    <!-- Desplegables del formulario -->
    <asp:AccessDataSource ID="DataFlorilegios" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" />
    <asp:AccessDataSource ID="DataAutores"     runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" />
    <asp:AccessDataSource ID="DataObras"       runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" />

    <!-- Filtro de búsqueda -->
    <asp:AccessDataSource ID="DataFiltroFlorilegios" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" />
    <asp:AccessDataSource ID="DataFiltroAutores"     runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" />
    <asp:AccessDataSource ID="DataFiltroObras"       runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" />
    <asp:AccessDataSource ID="DataFiltroUsuarios"    runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" SelectCommand="Select DISTINCT Usuario FROM Extractos WHERE Usuario IS NOT NULL ORDER BY Usuario" />

    <!-- Gráfico -->
    <asp:AccessDataSource ID="DataRecuento" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" />

    <!--
    ╔═════════════════╗
    ║     SPINNER     ║
    ╚═════════════════╝
    -->

    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateFiltro">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateTabla">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateFormulario">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
