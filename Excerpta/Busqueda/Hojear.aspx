<%@ Page Title="Excerpta" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Hojear.aspx.cs" Inherits="Busqueda.Hojear" Culture="auto:es-ES" UICulture="auto" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <section id="hojear" class="padding-top-0">

        <div class="torn-page-heading">
            <div class="overlay"></div>
        </div>

        <div class="container relative">
            <asp:UpdatePanel ID="UpdateHojear" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>

                    <!-- Encabezado -->
                    <h2><asp:Label runat="server" meta:resourcekey="Titulo" /></h2>
                    <hr />
                    <h3><asp:Label runat="server" meta:resourcekey="Subtitulo" /></h3>

                    <!--
                    ╔════════════════╗
                    ║     FILTRO     ║
                    ╚════════════════╝
                    -->

                    <div runat="server" id="Filtro" class="row">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-3 col-md-4 col-md-offset-4">
                            <div class="selector">
                                <asp:DropDownList ID="DropFiltro" runat="server" OnSelectedIndexChanged="DropFiltro_SelectedIndexChanged" AutoPostBack="true" ToolTip="Selector de filtros">
                                    <asp:ListItem meta:resourcekey="FiltrarFlorilegio" Value="Florilegio" Selected="True" />
                                    <asp:ListItem meta:resourcekey="FiltrarAutor"      Value="Autor" />
                                    <asp:ListItem meta:resourcekey="FiltrarObra"       Value="Obra"  />
                                </asp:DropDownList>
                                <span class="caption"><asp:Label runat="server" Text="<%$ Resources:General, FiltroBusqueda %>" /></span>
                                <i class="flecha fa fa-fw fa-chevron-down" aria-hidden="true"></i>
                            </div>
                        </div>
                    </div>

                    <!--
                    ╔═════════════════════╗
                    ║     FLORILEGIOS     ║
                    ╚═════════════════════╝
                    -->

                    <div id="Florilegios" runat="server" class="row margin-top-50">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">

                            <!-- Recuento -->
                            <div class="recuento right">
                                <div>
                                    <asp:Label ID="RecuentoFlorilegios" runat="server" />
                                    <asp:Label runat="server" Text="<%$ Resources:General, Resultados %>" />
                                </div>
                            </div>

                            <!-- Tabla -->
                            <div class="tabla">
                                <asp:GridView ID="TablaFlorilegios" runat="server" AllowSorting="true" AllowPaging="false" AutoGenerateColumns="False" DataSourceID="DataFlorilegios" GridLines="None" OnRowCommand="TablaFlorilegios_RowCommand" ShowHeader="true">
                                    <Columns>
                                        <asp:BoundField  DataField="Florilegio" SortExpression="Florilegio" HeaderText="<%$ Resources:General, Florilegio %>" />
                                        <asp:BoundField  DataField="Recuento"   SortExpression="Recuento"   HeaderText="<%$ Resources:General, Recuento   %>" />
                                        <asp:ButtonField ButtonType="Link"      CommandName="Abrir"         Text="#" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <!--
                    ╔═════════════════╗
                    ║     AUTORES     ║
                    ╚═════════════════╝
                    -->

                    <div id="Autores" runat="server" class="row margin-top-50" visible="false">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">

                            <!-- Recuento -->
                            <div class="recuento right">
                                <div>
                                    <asp:Label ID="RecuentoAutores" runat="server" />
                                    <asp:Label runat="server" Text="<%$ Resources:General, Resultados %>" />
                                </div>
                            </div>

                            <!-- Tabla -->
                            <div class="tabla">
                                <asp:GridView ID="TablaAutores" runat="server" AllowSorting="true" AllowPaging="false" AutoGenerateColumns="False" DataSourceID="DataAutores" GridLines="None" OnRowCommand="TablaAutores_RowCommand" ShowHeader="true">
                                    <Columns>
                                        <asp:BoundField  DataField="Autor"    SortExpression="Autor"    HeaderText="<%$ Resources:General, Autor    %>" />
                                        <asp:BoundField  DataField="Recuento" SortExpression="Recuento" HeaderText="<%$ Resources:General, Recuento %>" />
                                        <asp:ButtonField ButtonType="Link"    CommandName="Abrir"       Text="#" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <!--
                    ╔═══════════════╗
                    ║     OBRAS     ║
                    ╚═══════════════╝
                    -->

                    <div id="Obras" runat="server" class="row margin-top-50" visible="false">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">

                            <!-- Recuento -->
                            <div class="recuento right">
                                <div>
                                    <asp:Label ID="RecuentoObras" runat="server" />
                                    <asp:Label runat="server" Text="<%$ Resources:General, Resultados %>" />
                                </div>
                            </div>

                            <!-- Tabla -->
                            <div class="tabla tabla-obras">
                                <asp:GridView ID="TablaObras" runat="server" AllowSorting="true" AllowPaging="false" AutoGenerateColumns="False" DataSourceID="DataObras" GridLines="None" OnRowCommand="TablaObras_RowCommand" OnRowDataBound="TablaObras_RowDataBound" ShowHeader="true">
                                    <Columns>
                                        <asp:BoundField  DataField="Obra"     SortExpression="Obra"      HeaderText="<%$ Resources:General, Obra     %>" />
                                        <asp:BoundField  DataField="Autor"    SortExpression="Autor"     HeaderText="<%$ Resources:General, Autor    %>" />
                                        <asp:BoundField  DataField="Recuento" SortExpression="Recuento"  HeaderText="<%$ Resources:General, Recuento %>" />
                                        <asp:ButtonField ButtonType="Link"    CommandName="Abrir"        Text="#" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <!--
                    ╔═══════════════════╗
                    ║     CAPÍTULOS     ║
                    ╚═══════════════════╝
                    -->

                    <div id="Capitulos" runat="server" class="row margin-top-50" visible="false">

                        <!-- Volver -->
                        <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-3 col-md-4 col-md-offset-4">
                            <asp:LinkButton ID="ButtonCapitulosVolver" runat="server" OnClick="ButtonCapitulosVolver_Click" CssClass="boton volver margin-bottom-50">
                                <asp:Label runat="server" Text="<%$ Resources:General, Volver %>" CssClass="texto" />
                                <i class="icono fa fa-angle-left" aria-hidden="true"></i>
                            </asp:LinkButton>
                        </div>

                        <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">

                            <!-- Recuento -->
                            <div class="recuento">
                                <div>
                                    <asp:Label ID="ItemSeleccionado" runat="server" />
                                </div>
                                <div>
                                    <asp:Label ID="RecuentoCapitulos" runat="server" />
                                    <asp:Label runat="server" Text="<%$ Resources:General, Resultados %>" />
                                </div>
                            </div>

                            <!-- Tabla -->
                            <div class="tabla">
                                <asp:GridView ID="TablaCapitulos" runat="server" AllowSorting="true" AllowPaging="false" AutoGenerateColumns="False" DataSourceID="DataCapitulos" GridLines="None" OnRowCommand="TablaCaptiulos_RowCommand" ShowHeader="true">
                                    <Columns>
                                        <asp:BoundField DataField="Capítulo" SortExpression="Capítulo" HeaderText="<%$ Resources:General, Capitulo %>" />
                                        <asp:BoundField DataField="Recuento" SortExpression="Recuento" HeaderText="<%$ Resources:General, Recuento %>" />
                                        <asp:ButtonField ButtonType="Link"   CommandName="Abrir"       Text="#" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <!--
                    ╔═══════════════════╗
                    ║     EXTRACTOS     ║
                    ╚═══════════════════╝
                    -->

                    <div id="Extractos" runat="server" class="row margin-top-50" visible="false">

                        <!-- Retorno y navegación -->
                        <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-3 col-md-4 col-md-offset-4">
                            <asp:LinkButton ID="ButtonExtractosVolver" runat="server" OnClick="ButtonExtractosVolver_Click" CssClass="boton volver margin-bottom-50">
                                <asp:Label runat="server" Text="<%$ Resources:General, Volver %>" CssClass="texto" />
                                <i class="icono fa fa-angle-left" aria-hidden="true"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="ButonAnteriorExtracto" runat="server" CssClass="navegacion izquierda" OnClick="ButonAnteriorExtracto_Click">
                                <i class="icono fa fa-angle-left"  aria-hidden="true"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="ButonSiguienteExtracto" runat="server" CssClass="navegacion derecha" OnClick="ButonSiguienteExtracto_Click">
                                <i class="icono fa fa-angle-right"  aria-hidden="true"></i>
                            </asp:LinkButton>
                        </div>

                        <!-- Recuento -->
                        <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0 ">
                            <div class="recuento right no-padding">
                                <asp:Label runat="server" Text="<%$ Resources:General, Mostrando %>" />&nbsp;
                                <asp:Label ID="ExtractoActual"    runat="server" />&nbsp;
                                <asp:Label runat="server" Text="<%$ Resources:General, De %>" />&nbsp;
                                <asp:Label ID="RecuentoExtractos" runat="server" />&nbsp;
                                <asp:Label runat="server" Text="<%$ Resources:General, Extractos %>" />
                            </div>
                        </div>

                        <!-- Fichas -->
                        <div class="extracto col-xs-12">
                            <div class="row">
                                <asp:Label ID="Identificadores" CssClass="hidden" runat="server" />

                                <!-- Miniatura -->
                                <div class="col-xs-10 col-xs-offset-1 col-sm-10 col-sm-offset-1 col-md-4 col-md-offset-0">
                                    <asp:UpdatePanel runat="server" class="miniatura">
                                        <ContentTemplate>
                                            <asp:Image ID="Miniatura" runat="server" />
                                            <asp:LinkButton ID="ButtonAnteriorMiniatura"  runat="server" OnClick="ButtonAnteriorMiniatura_Click"  CssClass="flecha izquierda" >
                                                <i class="icono fa fa-angle-left" aria-hidden="true"></i>
                                            </asp:LinkButton>
                                            <asp:HyperLink ID="LinkMiniaturaPDF" runat="server" Target="_blank" CssClass="lupa" NavigateUrl="#">
                                                <i class="icono fa fa-search" aria-hidden="true"></i>
                                            </asp:HyperLink>
                                            <asp:LinkButton ID="ButtonSiguienteMiniatura" runat="server" OnClick="ButtonSiguienteMiniatura_Click" CssClass="flecha derecha" >
                                                <i class="icono fa fa-angle-right" aria-hidden="true"></i>
                                            </asp:LinkButton>
                                            <asp:Label ID="MiniaturaActual" runat="server" CssClass="indice" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                                <!-- Tabla -->
                                <div class="col-xs-10 col-xs-offset-1 col-sm-10 col-sm-offset-1 col-md-8 col-md-offset-0">
                                    <asp:Table ID="Ficha" runat="server" CssClass="ficha" />
                                </div>
                            </div>
                        </div>

                        <!-- PDF -->
                        <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-3 col-md-4 col-md-offset-4">
                            <a ID="LinkDescargarPDF" runat="server" href="#" target="_blank" class="boton descarga margin-top-50">
                                <asp:Label runat="server" Text="<%$ Resources:General, Descargar %>" CssClass="texto" />
                                <i class="icono fa fa-angle-down" aria-hidden="true"></i>
                            </a>
                        </div>
                    </div>
                
                    <!--
                    ╔═══════════════╗
                    ║     AYUDA     ║
                    ╚═══════════════╝
                    -->

                    <span class="boton-ayuda" data-toggle="modal" data-target="#guia">
                        <span class="interrogante">?</span>
                    </span>

                    <div id="guia" class="ayuda modal fade" role="dialog">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <h2>Guía</h2>
                                    <hr />
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <p>Este servicio permite establecer un recorrido en profundidad para navegar entre los diferentes capítulos de los florilegios, autores u obras. Inicialmente veremos un listado de florilegios (figura 1). A la derecha aparece el número de extractos anotados actualmente para cada uno de ellos a modo de recuento.</p>
                                    <asp:Image runat="server" ImageUrl="~/Images/Ayuda/hojear1.png" CssClass="img-responsive" />
                                    <span class="caption"><b>Figura 1.</b> Listado de florilegios.</span>
                                    <p>Encima de dicho listado de florilegios (figura 1) aparece un selector que permite cambiar este por un listado de autores u obras, según el modo en que deseemos realizar la navegación. Por ejemplo, es posible que no nos interese hojear los extractos de ningún florilegio, sino los de un autor en concreto, en cuyo caso tendríamos que ajustar el filtro para que aparezca el listado de autores.</p>
                                    <p>Sea cual sea el filtro que apliques, una vez hayamos seleccionado un florilegio, autor u obra haciendo clic encima, se abrirá un nuevo listado, esta vez de capítulos (figura 2). Análogamente, a la derecha aparece el número de extractos anotados para cada capítulo a modo de recuento.</p>
                                    <asp:Image runat="server" ImageUrl="~/Images/Ayuda/hojear2.png" CssClass="img-responsive" />
                                    <span class="caption"><b>Figura 2.</b> Listado de capítulo.</span>
                                    <p>A partir de la situación de la figura 2 podemos volver al listado anterior (figura 1) o seleccionar un capítulo haciendo clic sobre él para hojear los extractos que contiene (figura 3).</p>
                                    <asp:Image runat="server" ImageUrl="~/Images/Ayuda/hojear3.png" CssClass="img-responsive" />
                                    <span class="caption"><b>Figura 3.</b> Ficha de un extracto.</span>
                                    <p>A partir de la situación de la figura 3 podemos volver al listado anterior (figura 2) o navegar entre las fichas de extractos utilizando los botones direccionales que se encuentran al extremo del botón de retorno (en caso de haber más de uno). En cada ficha podemos ver todos los datos del extracto cargado. Debajo tenemos un botón para descargar un fichero PDF con las fichas de todos los extractos del capítulo seleccionado.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </section>

    <!--
    ╔═════════════════════════╗
    ║     ORIGEN DE DATOS     ║
    ╚═════════════════════════╝
    -->

    <!-- Florilegios, autores, obras, capítulos y extractos -->
    <asp:AccessDataSource ID="DataFlorilegios" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" OnSelected="DataFlorilegios_Selected" SelectCommand="SELECT DISTINCT Florilegio,  COUNT(Florilegio) AS Recuento FROM Extractos WHERE Alta=True GROUP BY Florilegio  HAVING COUNT(Florilegio) > 0 ORDER BY Florilegio" />
    <asp:AccessDataSource ID="DataAutores"     runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" OnSelected="DataAutores_Selected"     SelectCommand="SELECT DISTINCT Autor,       COUNT(Autor)      AS Recuento FROM Extractos WHERE Alta=True GROUP BY Autor       HAVING COUNT(Autor)      > 0 ORDER BY Autor     " />
    <asp:AccessDataSource ID="DataObras"       runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" OnSelected="DataObras_Selected"       SelectCommand="SELECT DISTINCT Autor, Obra, COUNT(Obra)       AS Recuento FROM Extractos WHERE Alta=True GROUP BY Autor, Obra HAVING COUNT(Obra)       > 0 ORDER BY Obra      " />
    <asp:AccessDataSource ID="DataCapitulos"   runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" OnSelected="DataCapitulos_Selected" />

    <!--
    ╔═════════════════╗
    ║     SPINNER     ║
    ╚═════════════════╝
    -->

    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateHojear">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>