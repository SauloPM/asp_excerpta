<%@ Page Title="Excerpta" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Buscar.aspx.cs" Inherits="Busqueda.Buscar" Culture="auto:es-ES" UICulture="auto" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <section id="busqueda" class="padding-top-0">

        <!-- Torn page -->
        <div class="torn-page-heading">
            <div class="overlay"></div>
        </div>

        <div class="container relative">

            <!-- Heading -->
            <h2><asp:Label runat="server" meta:resourcekey="Titulo" /></h2>
            <hr />
            <h3><asp:Label runat="server" meta:resourcekey="Subtitulo" /></h3>

            <!--
            ╔════════════════╗
            ║     FILTRO     ║
            ╚════════════════╝
            -->

            <asp:TextBox ID="InputBuffer" runat="server" Visible="false" ToolTip="Entrada de datos oculta" />

            <!-- Button -->
            <div class="abrir-filtro margin-top-50">
                <div class="icono">
                    <i class="lupa fa fa-search" aria-hidden="true"></i>
                </div>
                <asp:Label runat="server" Text="<%$ Resources:General, FiltroBusqueda %>" CssClass="caption" />
            </div>

            <!-- Forms -->
            <div class="filtro-formulario">

                <!-- Pestañas -->
                <div class="pestanas">
                    <span data-target="filtro-simple" class="active">
                        <asp:Label runat="server" meta:resourcekey="BusquedaSimple" />
                    </span>
                    <span data-target="filtro-avanzado">
                        <asp:Label runat="server" meta:resourcekey="BusquedaAvanzada" />
                    </span>
                </div>

                <!-- Búsqueda simple -->
                <div class="filtro-simple">
                    <asp:UpdatePanel ID="UpdateFiltroSimple" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-xs-10 col-xs-offset-1 col-sm-4 col-sm-offset-0">
                                    <div class="selector">
                                        <asp:DropDownList ID="DropFlorilegios" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataFlorilegios" DataTextField="Florilegio" DataValueField="Florilegio" OnSelectedIndexChanged="DropFlorilegios_SelectedIndexChanged" ToolTip="Selector de florilegios">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:General, SeleccionarTodos %>" Value="%" />
                                        </asp:DropDownList>
                                        <span class="caption"><asp:Label runat="server" Text="<%$ Resources:General, Florilegio %>" /></span>
                                        <i class="flecha fa fa-fw fa-chevron-down" aria-hidden="true"></i>
                                    </div>
                                </div>
                                <div class="col-xs-10 col-xs-offset-1 col-sm-4 col-sm-offset-0">
                                    <div class="selector">
                                        <asp:DropDownList ID="DropAutores" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataAutores" DataTextField="Autor" DataValueField="Autor" OnSelectedIndexChanged="DropAutores_SelectedIndexChanged" ToolTip="Selector de autores">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:General, SeleccionarTodos %>" Value="%" />
                                        </asp:DropDownList>
                                        <span class="caption"><asp:Label runat="server" Text="<%$ Resources:General, Autor %>" /></span>
                                        <i class="flecha fa fa-fw fa-chevron-down" aria-hidden="true"></i>
                                    </div>
                                </div>
                                <div class="col-xs-10 col-xs-offset-1 col-sm-4 col-sm-offset-0">
                                    <div class="selector">
                                        <asp:DropDownList ID="DropObras" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataObras" DataTextField="Obra" DataValueField="Obra" ToolTip="Selector de obras">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:General, SeleccionarTodos %>" Value="%" />
                                        </asp:DropDownList>
                                        <span class="caption"><asp:Label runat="server" Text="<%$ Resources:General, Obra %>" /></span>
                                        <i class="flecha fa fa-fw fa-chevron-down" aria-hidden="true"></i>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0">
                                    <div class="entrada">
                                        <asp:TextBox ID="InputFrases" runat="server" MaxLength="1000" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para la búsqueda por aproximación" />
                                        <span class="caption"><asp:Label runat="server" meta:resourcekey="BusquedaAproximacion" /></span>
                                    </div>
                                </div>
                                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0">
                                    <div class="entrada">
                                        <asp:TextBox ID="InputPalabras" runat="server" MaxLength="1000" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para la búsqueda por palabras" />
                                        <span class="caption"><asp:Label runat="server" meta:resourcekey="BusquedaPalabras" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0">
                                    <div class="check">
                                        <asp:RadioButtonList ID="CheckFrases" runat="server"  AutoPostBack="true"  RepeatDirection="Horizontal">
                                            <asp:ListItem Value="AND" meta:resourcekey="AND" Selected="True" />
                                            <asp:ListItem Value="OR"  meta:resourcekey="OR" />
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0">
                                    <div class="check">
                                        <asp:RadioButtonList ID="CheckPalabras" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="AND" meta:resourcekey="AND" Selected="True" />
                                            <asp:ListItem Value="OR"  meta:resourcekey="OR" />
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                            <div class="row margin-top-25">
                                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0">
                                    <asp:LinkButton runat="server" OnClick="ButtonLimpiarFiltroSimple_Click" CssClass="boton limpiar-filtro margin-bottom-25">
                                        <asp:Label runat="server" meta:resourcekey="LimpiarFiltro" CssClass="texto" />
                                        <i class="icono fa fa-trash" aria-hidden="true"></i>
                                    </asp:LinkButton>
                                </div>
                                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0">
                                    <asp:LinkButton runat="server" OnClick="ButtonBusquedaSimple_Click" CssClass="boton buscar margin-bottom-25" >
                                        <asp:Label runat="server" meta:resourcekey="Buscar" CssClass="texto" />
                                        <i class="icono fa fa-search" aria-hidden="true"></i>
                                    </asp:LinkButton>
                                    <span class="interrogante" data-toggle="modal" data-target="#operadores">?</span>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <!-- Búsqueda avanzada -->
                <div class="filtro-avanzado">
                    <asp:UpdatePanel ID="UpdateFiltroAvanzado" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-xs-4 col-sm-6">
                                    <div class="selector">
                                        <asp:DropDownList ID="ANDOR1" runat="server" ToolTip="Selector de operador">
                                            <Items>
                                                <asp:ListItem Value="" Selected="True" />
                                                <asp:ListItem Value="NOT" meta:resourcekey="dropNO" />
                                            </Items>
                                        </asp:DropDownList>
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, Operador %>" />
                                        </span>
                                        <i class="flecha fa fa-fw fa-chevron-down" aria-hidden="true"></i>
                                    </div>
                                </div>
                                <div class="col-xs-8 col-sm-6">
                                    <div class="entrada">
                                        <asp:TextBox ID="InputFlorilegio" runat="server" AutoPostBack="true" MaxLength="1000" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el florilegio" /><!-- OnTextChanged="InputFlorilegio_TextChanged"-->
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, Florilegio %>" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-4 col-sm-6">
                                    <div class="selector">
                                        <asp:DropDownList ID="ANDOR2" runat="server" ToolTip="Selector de operador">
                                            <Items>
                                                <asp:ListItem Value="AND" Selected="True" meta:resourcekey="dropAND" />
                                                <asp:ListItem Value="OR" meta:resourcekey="dropOR" />
                                                <asp:ListItem Value="AND NOT" meta:resourcekey="dropNO" />
                                            </Items>
                                        </asp:DropDownList>
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, Operador %>" />
                                        </span>
                                        <i class="flecha fa fa-fw fa-chevron-down" aria-hidden="true"></i>
                                    </div>
                                </div>
                                <div class="col-xs-8 col-sm-6">
                                    <div class="entrada">
                                        <asp:TextBox ID="InputAutor" runat="server" MaxLength="1000" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el autor" />
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, Autor %>" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-4 col-sm-6">
                                    <div class="selector">
                                        <asp:DropDownList ID="ANDOR3" runat="server" ToolTip="Selector de operador">
                                            <Items>
                                                <asp:ListItem Value="AND" Selected="True" meta:resourcekey="dropAND" />
                                                <asp:ListItem Value="OR" meta:resourcekey="dropOR" />
                                                <asp:ListItem Value="AND NOT" meta:resourcekey="dropNO" />
                                            </Items>
                                        </asp:DropDownList>
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, Operador %>" />
                                        </span>
                                        <i class="flecha fa fa-fw fa-chevron-down" aria-hidden="true"></i>
                                    </div>
                                </div>
                                <div class="col-xs-8 col-sm-6">
                                    <div class="entrada">
                                        <asp:TextBox ID="InputObra" runat="server" MaxLength="1000" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para la obra" />
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, Obra %>" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-4 col-sm-6">
                                    <div class="selector">
                                        <asp:DropDownList ID="ANDOR4" runat="server" ToolTip="Selector de operador">
                                            <Items>
                                                <asp:ListItem Value="AND" Selected="True" meta:resourcekey="dropAND" />
                                                <asp:ListItem Value="OR" meta:resourcekey="dropOR" />
                                                <asp:ListItem Value="AND NOT" meta:resourcekey="dropNO" />
                                            </Items>
                                        </asp:DropDownList>
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, Operador %>" />
                                        </span>
                                        <i class="flecha fa fa-fw fa-chevron-down" aria-hidden="true"></i>
                                    </div>
                                </div>
                                <div class="col-xs-8 col-sm-6">
                                    <div class="entrada">
                                        <asp:TextBox ID="InputCapitulo" runat="server" MaxLength="1000" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el capítulo" />
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, Capitulo %>" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-4 col-sm-6">
                                    <div class="selector">
                                        <asp:DropDownList ID="ANDOR5" runat="server" ToolTip="Selector de operador">
                                            <Items>
                                                <asp:ListItem Value="AND" Selected="True" meta:resourcekey="dropAND" />
                                                <asp:ListItem Value="OR" meta:resourcekey="dropOR" />
                                                <asp:ListItem Value="AND NOT" meta:resourcekey="dropNO" />
                                            </Items>
                                        </asp:DropDownList>
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, Operador %>" />
                                        </span>
                                        <i class="flecha fa fa-fw fa-chevron-down" aria-hidden="true"></i>
                                    </div>
                                </div>
                                <div class="col-xs-8 col-sm-6">
                                    <div class="entrada">
                                        <asp:TextBox ID="InputSubcapitulo" runat="server" MaxLength="1000" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el subcapítulo" />
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, Subcapitulo %>" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-4 col-sm-6">
                                    <div class="selector">
                                        <asp:DropDownList ID="ANDOR6" runat="server" ToolTip="Selector de operador">
                                            <Items>
                                                <asp:ListItem Value="AND" Selected="True" meta:resourcekey="dropAND" />
                                                <asp:ListItem Value="OR" meta:resourcekey="dropOR" />
                                                <asp:ListItem Value="AND NOT" meta:resourcekey="dropNO" />
                                            </Items>
                                        </asp:DropDownList>
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, Operador %>" />
                                        </span>
                                        <i class="flecha fa fa-fw fa-chevron-down" aria-hidden="true"></i>
                                    </div>
                                </div>
                                <div class="col-xs-8 col-sm-6">
                                    <div class="entrada">
                                        <asp:TextBox ID="InputExtracto" runat="server" MaxLength="1000" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el extracto" />
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, Extracto %>" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-4 col-sm-6">
                                    <div class="selector">
                                        <asp:DropDownList ID="ANDOR7" runat="server" ToolTip="Selector de operador">
                                            <Items>
                                                <asp:ListItem Value="AND" Selected="True" meta:resourcekey="dropAND" />
                                                <asp:ListItem Value="OR" meta:resourcekey="dropOR" />
                                                <asp:ListItem Value="AND NOT" meta:resourcekey="dropNO" />
                                            </Items>
                                        </asp:DropDownList>
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, Operador %>" />
                                        </span>
                                        <i class="flecha fa fa-fw fa-chevron-down" aria-hidden="true"></i>
                                    </div>
                                </div>
                                <div class="col-xs-8 col-sm-6">
                                    <div class="entrada">
                                        <asp:TextBox ID="InputTLL" runat="server" MaxLength="1000" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el la edición moderna" />
                                        <span class="caption">
                                            <asp:Label runat="server" Text="<%$ Resources:General, EdicionModerna %>" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row margin-top-25">
                                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0">
                                    <asp:LinkButton runat="server" OnClick="ButtonLimpiarFiltroAvanzado_Click" CssClass="boton limpiar-filtro margin-bottom-25">
                                        <asp:Label runat="server" meta:resourcekey="LimpiarFiltro" CssClass="texto" />
                                        <i class="icono fa fa-trash" aria-hidden="true"></i>
                                    </asp:LinkButton>
                                </div>
                                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0">
                                    <asp:LinkButton runat="server" OnClick="ButtonBusquedaAvanzada_Click" CssClass="boton buscar margin-bottom-25" >
                                        <asp:Label runat="server" meta:resourcekey="Buscar" CssClass="texto" />
                                        <i class="icono fa fa-search" aria-hidden="true"></i>
                                    </asp:LinkButton>
                                    <span class="interrogante" data-toggle="modal" data-target="#operadores">?</span>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

            <!--
            ╔════════════════════╗
            ║     RESULTADOS     ║
            ╚════════════════════╝
            -->

            <asp:UpdatePanel ID="UpdateResultados" runat="server">
                <ContentTemplate>
                    <div class="row margin-top-50">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0 ">
                            
                            <!-- Recuento -->
                            <div class="recuento">
                                <div>
                                    <asp:Label runat="server" Text="<%$ Resources:General, UltimaActualizacion  %>" />
                                    <asp:Label ID="UltimaActualizacion" runat="server" />
                                </div>
                                <div>
                                    <asp:Label ID="Recuento" runat="server" />
                                    <asp:Label runat="server" Text="<%$ Resources:General, Resultados %>" />
                                </div>
                            </div>

                            <!-- Tabla -->
                            <div class="tabla">
                                <asp:GridView ID="TablaExtractos" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="False" DataSourceID="DataExtractos" GridLines="None" OnRowCommand="TablaExtractos_RowCommand" OnRowDataBound="TablaExtractos_RowDataBound" PageSize="15" PagerSettings-PageButtonCount="5" ShowHeader="true">
                                    <Columns>
                                        <asp:BoundField DataField="Florilegio"  SortExpression="Florilegio"  HeaderText="<%$ Resources:General, Florilegio     %>" />
                                        <asp:BoundField DataField="Autor"       SortExpression="Autor"       HeaderText="<%$ Resources:General, Autor          %>" />
                                        <asp:BoundField DataField="Obra"        SortExpression="Obra"        HeaderText="<%$ Resources:General, Obra           %>" />
                                        <asp:BoundField DataField="Capítulo"    SortExpression="Capítulo"    HeaderText="<%$ Resources:General, Capitulo       %>" />
                                        <asp:BoundField DataField="Subcapítulo" SortExpression="Subcapítulo" HeaderText="<%$ Resources:General, Subcapitulo    %>" />
                                        <asp:BoundField DataField="Extracto"    SortExpression="Extracto"    HeaderText="<%$ Resources:General, Extracto       %>" />
                                        <asp:BoundField DataField="TLL"         SortExpression="TLL"         HeaderText="<%$ Resources:General, EdicionModerna %>" />
                                        <asp:BoundField DataField="ID"          SortExpression="ID"          HeaderText="ID"                                    />
                                        <asp:ButtonField ButtonType="Link"      CommandName="Abrir"                                                             />
                                    </Columns>
                                    <PagerStyle CssClass="paginador" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <!--
            ╔═══════════════╗
            ║     AYUDA     ║
            ╚═══════════════╝
            -->

            <!-- Guía de ayuda -->
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
                            <p>Este servicio permite realizar búsquedas de extractos aplicando, para ello, filtros de búsqueda. Inicialmente veremos un listado detallado de extractos (figura 1). Para ver todos los datos de un extracto, utiliza el scroll que se encuentra en la parte inferior del listado para desplazarte lateralmente. Debido a que hay demasiados extractos, el listado se encuentra paginado. Al hacer clic sobre un extracto, se abrirá una nueva pestaña en nuestro navegador web que mostrará su ficha.</p>
                            <asp:Image runat="server" ImageUrl="~/Images/Ayuda/buscar1.png" CssClass="img-responsive" />
                            <span class="caption"><b>Figura 1.</b> Listado de florilegios.</span>
                            <p>Encima de dicho listado de extractos (figura 1) tenemos un botón con un icono de una lupa dentro. Este botón sirve para desplegar el filtro de búsqueda (figura 2). Podemos realizar dos tipos de búsqueda: simple o avanzada. Tenemos la opción de alternar entre una y otra utilizando las pestañas que aparecen en la parte superior del filtro de búsqueda (figura 2). En ambos casos podemos limpiar el filtro de búsqueda, lo que conlleva que ambos formularios queden vacíos y que se vuelvan a mostrar todos los extractos.</p>
                            <h3>Filtro de búsqueda simple</h3>
                            <hr />
                            <p>La búsqueda simple cuenta con tres selectores y dos entradas de texto (figura 2). Todos los campos son opcionales, de modo que podemos buscar con el filtro de búsqueda vacío, en cuyo caso se mostrarían todos los extractos.</p>
                            <p>El primero de los tres selectores sirve para seleccionar un florilegio, el segundo para seleccionar un autor y el tercero para seleccionar una obra. Si por ejemplo seleccionáramos un florilegio, se refrescaría el selector de autores para mostrar únicamente los autores que correspondiesen con el florilegio seleccionado previamente. Lo mismo ocurriría con el selector de obras tras haber seleccionado una autor. Es decir, los selectores se encuentran sincronizados en cascada.</p>
                            <p>No obstante, no estamos obligados a seleccionar ningún florilegio para poder utilizar el selector de autores, aunque en este caso el selector de autores mostraría todos los autores en lugar de mostrar solo aquellos vinculados al florilegio seleccionado, ya que en este ejemplo en concreto hemos supuesto que se había seleccionado previamente ningún florilegio.</p>
                            <asp:Image runat="server" ImageUrl="~/Images/Ayuda/buscar2.png" CssClass="img-responsive" />
                            <span class="caption"><b>Figura 2.</b> Filtro de búsqueda simple.</span>
                            <p>La segunda de las dos entradas de texto sirve para buscar palabras completas. Si quisiéramos buscar más de una palabra, solo tendríamos que separarlas entre sí con un espacio en blanco. Por defecto, cuando buscamos más de una palabra se va a buscar extractos en los que aparezcan <b>todas y cada una</b> de las palabras que hayamos escrito. No obstante, si cambiamos el ítem del check que se encuentra debajo de esta entrada de texto de <span class="azul">Y</span> a <span class="azul">O</span>, se buscará extractos en los que aparezca <b>al menos una</b> de dichas palabras. Nótese que en este caso el orden no se tiene en cuenta. Es decir, que se produciría el mismo resultado si hubiéramos escrito las mismas palabras en diferente orden.</p>
                            <p>Por otro lado, la primera de las dos entradas de texto sirve para lo mismo que la primera, pero aplicado a frases. En este caso, en lugar de utilizar un espacio en blanco como separador, se utilizaría <span class="azul">/</span>. Por defecto, cuando buscamos más de una frase se va a buscar extractos en los que aparezcan todas y cada una de las frases que hayamos escrito. No obstante, si cambiamos el ítem del check que se encuentra debajo de esta entrada de texto de <span class="azul">Y</span> a <span class="azul">O</span>, se buscará extractos en los que aparezca al menos una de dichas frases. Nótese que en este caso el orden sí se tiene en cuenta, ya que se están buscando frases.</p>
                            <h3>Filtro de búsqueda avanzada</h3>
                            <hr />
                            <p>Aunque el filtro de búsqueda simple parezca potente, no permite, por ejemplo, buscar más de un florilegio, autor u obra, ya que solo podíamos seleccionar uno en cada selector. Por otra parte, la búsqueda de frases y palabras no nos permitía elegir en qué campos en concreto buscar dentro del extracto, sino que siempre efectuaba la búsqueda sobre el capítulo, el subcapítulo, el extracto y la edición moderna. Todos estas limitaciones las viene a resolver la búsqueda avanzada (figura 3).</p>
                            <asp:Image runat="server" ImageUrl="~/Images/Ayuda/buscar3.png" CssClass="img-responsive" />
                            <span class="caption"><b>Figura 3.</b> Filtro de búsqueda avanzada.</span>
                            <p>Para cada campo del extracto (exceptuando el libro, poema y versos) tenemos un selector y una entrada de texto. En cada entrada de texto se sigue la misma dinámica que la búsqueda de frases del filtro de búsqueda simple, salvo que esta vez podemos utilizar dos tipos de separadores según nuestras necesidades, a saber, el separador <span class="azul">/</span> para la disyuncuión y el separador <span class="azul">&</span> para la conjunción.</p>
                            <p>Por ejemplo, si quisiéramos buscar extractos pertenecientes a los autores Propercius, Ovidius o Ausonius, tendríamos que escribir <span class="azul">Propercius / Ovidius / Ausonius</span> en la entrada de texto correspondiente a los autores. Dentro de una misma entrada de texto estos dos tipos de separadores pueden coexistir sin ningún problema. Por ejemplo, si escribiéramos <span class="azul">Propercius & Ovidius / Ausonius</span> estaríamos buscando extractos pertenecientes a los autores Propercius y Ovidius (a la vez) o al autor Ausonius.</p>
                            <asp:Image runat="server" ImageUrl="~/Images/Ayuda/buscar4.png" CssClass="img-responsive" />
                            <span class="caption"><b>Figura 4.</b> Búsqueda de extractos no pertenecientes al florilegio G. MAIOR, pero sí al autor Catullus.</span>
                            <p>Los selectores nos permiten conectar la búsqueda de los campos del extracto como deseemos. Cada selector contiene tres operadores, a saber, la conjunción (<span class="azul">Y</span>), la disyunción (<span class="azul">O</span>) y la negación (<span class="azul">NO</span>). Por ejemplo, si no estuviéramos interesados buscar extractos pertenecientes al florilegio G. MAIOR pero que sí pertenecieran al autor Catullus, tendríamos que escribir <span class="azul">G. MAIOR</span> en la entrada de texto de los florilegios, seleccionar el operador <span class="azul">NO</span> en el selector de operadores que la precede, escribir <span class="azul">Catullus</span> en la entrada de texto de los autores y seleccionar el operador <span class="azul">Y</span> en el selector de operadores que la precede (figura 4).</p>
                            <h3>Operadores</h3>
                            <hr />
                            <p>En todas las entradas de texto, tanto en las del filtro de la búsqueda simple como en las del filtro de la búsqueda avanzada, podemos utilizar operadores. Estos operadores nos permiten omitir parte de la información a la hora de efectuar una búsqueda, ya que sin ellos tendríamos que escribir la secuencia completa para cualquier campo sobre el que deseáramos efectuar una búsqueda o el buscador no proporcionaba resultados.</p>
                            <p>Por ejemplo, para buscar los extractos del florilegio H. STEPHANUS, teníamos que escribir <span class="azul">H. STEPHANUS (per L. Colvandrum)</span> o el buscador no lo reconocía. Para no vernos obligados a escribir el título completo del florilegio, podemos utilizar el operador <span class="azul">*</span>. Situando este operador delante de la secuencia, podemos omitir parte del contenido de la misma si quisiéramos, igualmente si lo situáramos al final (o ambos casos).</p>
                            <p>Siguiendo con este ejemplo, si escribimos <span class="azul">*STEPHANUS*</span> en la entrada de texto del florilegio, buscará aquellos extractos que contengan la secuencia <span class="azul">STEPHANUS</span> en el campo del florilegio, sin importar si existen más caracteres al principio o al final de dicha secuencia. No obstante, de haber omitido, por ejemplo, el asterisco del final (quedando <span class="azul">*STEPHANUS</span>), estaríamos diciéndole al buscador que el título del florilegio finalizaría por dicha secuencia de caracteres, no produciendo ningún resultado (ya que el título del florilegio no finaliza en <span class="azul">STEPHANUS</span>, sino en <span class="azul">(per L. Colvandrum)</span>).</p>
                            <p>De modo que este operador es útil cuando deseamos omitir parte de la información, bien por comodidad (para evitar tener que escribir largas secuencias de caracteres), o bien porque solo estamos seguros de un fragmento de la secuencia.</p>
                            <p>Por último, tenemos el operador <span class="azul">?</span>. Este operador es útil cuando no estamos seguros de un carácter. Por ejemplo, supongamos que deseamos buscar extractos en los que aparezcan las palabras <span class="azul">amor</span> y <span class="azul">amar</span> en el capítulo. Una opción sería escribir <span class="azul">amor || amar</span> en la entrada de texto correspondiente al capítulo, pero también podemos escribir la secuencia <span class="azul">am?r</span>y el efecto sería el mismo. Este operador nos permite omitir un solo carácter. Para omitir más de uno, tenemos que escribirlo más de una vez, tantas como queramos (por ejemplo, <span class="azul">am??</span>).</p>
                            <p>La diferencia entre el operador <span class="azul">?</span> y el operador <span class="azul">*</span> estriba en que el primero permite omitir un solo carácter, mientras que el segundo permite omitir uno o muchos caracteres (incluso ninguno). Con el operador <span class="azul">?</span> podemos omitir más de un carácter, pero tendríamos que utilizarlo tantas veces como caracteres queramos omitir.</p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Operadores -->
            <div id="operadores" class="ayuda modal fade" role="dialog">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-body">
                            <h2>Operadores</h2>
                            <hr />
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h3>Búsqueda sin operadores</h3>
                            <hr />
                            <p>En el caso de búsqueda por aproximación, localizaría cualquier término introducido cuando aparece en comienzo de capítulo, subcapítulo o extracto</p>
                            <p>En el caso de la búsqueda de palabras, localizaría cualquier término en cualquier lugar de estos mismos campo.</p>
                            <h3>Búsqueda con operadores</h3>
                            <hr />
                            <p>Operadores válidos en ambos tipos de búsqueda:</p>
                            <ul>
                                <li><b>Operador <span class="azul">*</span></b> » Permite buscar palabras (<span class="rosa">*venus*</span>), combinación de palabras (<span class="rosa">*venus enervat*</span>) o parte de una palabra (<span class="rosa">*ven*</span>) en cualquier contexto. Esta es la búsqueda más versátil y que más resultados proporciona</li>
                                <li><b>Operador <span class="azul">?</span></b> » Permite omitir uno (am?r) o varios caracteres de una palabra (a??r)</li>
                            </ul>
                            <ul style="display: none">
                                <li><b>Operador <span class="azul">?</span></b> » Permite omitir un caracter dentro de una secuencia de caracteres. Podemos utilizarlo una o tantas veces como queramos. Por ejemplo, si escribiéramos <span class="rosa">am?r</span>, encontraría <span class="rosa">amor</span> o <span class="rosa">amar</span>, ya que hemos utilizado este operador en el tercer carácter.</li>
                                <li><b>Operador <span class="azul">*</span></b> » Permite omitir uno, ninguno o más caracteres dentro de una secuencia de caracteres. Es similar al operador anterior, pero es más potente. Podemos utilizarlo una o tantas veces como queramos. Por ejemplo, si escribiéramos <span class="rosa">A*</span>, estaríamos buscando secuencias de caracteres que empezaran por <span class="rosa">A</span>. Análogamente, si escribiéramos <span class="rosa">*A</span>, estaríamos buscando secuencias de caracteres que terminaran por <span class="rosa">A</span>. Por último, supongamos que deseamos buscar una secuencia de caracteres, pero no recordábamos cómo empezaba ni cómo terminaba, solo recordamos que en medio de la secuencia aparecía la palabra <span class="rosa">amor</span>. En tal caso, tendríamos que escribir <span class="rosa">*amor*</span>.</li>
                            </ul>
                            <p>Operadores exclusivos de la búsqueda simple:</p>
                            <ul>
                                <li><b>Operador <span class="azul">/</span></b> » Actúa como separador en la búsqueda por aproximación. Permite separar dos o más secuencias de caracteres entre sí. Por ejemplo, <span class="rosa">Alieno periculo sapere / De aetatibus hominum / Fides etiam hosti seruanda</span>.</li>
                                <li><b>Espacio en blanco</b> » Actúa como separador en en la búsqueda de palabras. Permite separar palabras entre sí. Por ejemplo, <span class="rosa">format natura amores</span>.</li>
                            </ul>
                            <p>Operadores exclusivos de la búsqueda avanzada:</p>
                            <ul>
                                <li><b>Operador <span class="azul">/</span></b> » Se utiliza en una entrada de texto para conectar secuencias de caracteres. Le indica al buscador que necesita que <u>al menos una</u> de las secuencias que conecta aparezca en el campo correspondiente para que la búsqueda proporcione resultados. Por ejemplo, si escribiéramos <span class="rosa">B. Schonborn / G. Maior</span> en la entrada de texto de los florilegios, estaríamos buscando extractos que pertenezcan al florilegio B. Schonborn o al florilegio G. Maior.</li>
                                <li><b>Operador <span class="azul">&</span></b> » Se utiliza en una entrada de texto para conectar secuencias de caracteres. Le indica al buscador que necesita que <u>todas</u> las secuencias que conecta aparezcan en el campo correspondiente para que la búsqueda proporcione resultados. Siguiendo con el ejemplo anterior, si escribiéramos <span class="rosa">B. Schonborn & G. Maior</span> en la entrada de texto de los florilegios, estaríamos buscando extractos que pertenezcan al florilegio B. Schonborn y al florilegio G. Maior.</li>
                            </ul>
                            <button type="button" class="boton buscar">
                                <span class="texto">Ver ayuda</span>
                                <i class="icono fa fa-search" aria-hidden="true"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!--
    ╔═════════════════════════╗
    ║     ORIGEN DE DATOS     ║
    ╚═════════════════════════╝
    -->

    <!-- Desplegables de florilegios, autores y obras -->
    <asp:AccessDataSource ID="DataFlorilegios" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" SelectCommand="SELECT DISTINCT Florilegio FROM Extractos WHERE Alta=True AND Florilegio IS NOT NULL ORDER BY Florilegio ASC" />
    <asp:AccessDataSource ID="DataAutores"     runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" SelectCommand="SELECT DISTINCT Autor      FROM Extractos WHERE Alta=True AND Autor      IS NOT NULL ORDER BY Autor      ASC" />
    <asp:AccessDataSource ID="DataObras"       runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" SelectCommand="SELECT DISTINCT Obra       FROM Extractos WHERE Alta=True AND Obra       IS NOT NULL ORDER BY Obra       ASC" />

    <!-- Extractos -->
    <asp:AccessDataSource ID="DataExtractos" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" OnSelected="DataExtractos_Selected" SelectCommand="SELECT ID, Florilegio, Autor, Obra, [Capítulo], [Subcapítulo], Extracto, TLL FROM Extractos WHERE Alta=True ORDER BY Florilegio, Autor, Obra" />

    <!--
    ╔═════════════════╗
    ║     SPINNER     ║
    ╚═════════════════╝
    -->

    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateFiltroSimple">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateFiltroAvanzado">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateResultados">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
