<%@ Page Title="Excerpta" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Biblioteca.aspx.cs" Inherits="Busqueda.Biblioteca" Culture="auto:es-ES" UICulture="auto" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <section id="biblioteca" class="padding-top-0">

        <div class="torn-page-heading">
            <div class="overlay"></div>
        </div>

        <div class="container relative">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>

                    <!-- Encabezado -->
                    <h2><asp:Label runat="server" meta:resourcekey="Titulo" /></h2>
                    <hr />
                    <h3><asp:Label runat="server" meta:resourcekey="Subtitulo" /></h3>

                    <!-- Filtro -->
                    <div class="row">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-4 col-md-offset-2">
                            <div class="selector">
                                <asp:DropDownList ID="DropLetras" runat="server" OnSelectedIndexChanged="DropLetras_SelectedIndexChanged" AutoPostBack="true" ToolTip="Selector de letra inicial">
                                    <asp:ListItem Selected="True" meta:resourcekey="InicialItem" />
                                    <asp:ListItem>A</asp:ListItem>
                                    <asp:ListItem>B</asp:ListItem>
                                    <asp:ListItem>C</asp:ListItem>
                                    <asp:ListItem>D</asp:ListItem>
                                    <asp:ListItem>E</asp:ListItem>
                                    <asp:ListItem>F</asp:ListItem>
                                    <asp:ListItem>G</asp:ListItem>
                                    <asp:ListItem>H</asp:ListItem>
                                    <asp:ListItem>I</asp:ListItem>
                                    <asp:ListItem>J</asp:ListItem>
                                    <asp:ListItem>K</asp:ListItem>
                                    <asp:ListItem>L</asp:ListItem>
                                    <asp:ListItem>M</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                    <asp:ListItem>O</asp:ListItem>
                                    <asp:ListItem>P</asp:ListItem>
                                    <asp:ListItem>Q</asp:ListItem>
                                    <asp:ListItem>R</asp:ListItem>
                                    <asp:ListItem>S</asp:ListItem>
                                    <asp:ListItem>T</asp:ListItem>
                                    <asp:ListItem>U</asp:ListItem>
                                    <asp:ListItem>V</asp:ListItem>
                                    <asp:ListItem>W</asp:ListItem>
                                    <asp:ListItem>V</asp:ListItem>
                                    <asp:ListItem>X</asp:ListItem>
                                    <asp:ListItem>Z</asp:ListItem>
                                </asp:DropDownList>
                                <span class="caption"><asp:Label runat="server" meta:resourcekey="InicialCaption" /></span>
                                <i class="flecha fa fa-fw fa-chevron-down" aria-hidden="true"></i>
                            </div>
                        </div>
                        <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-4 col-md-offset-0">
                            <div class="entrada">
                                <asp:TextBox ID="InputBusqueda" runat="server" AutoCompleteType="Disabled" MaxLength="100" CssClass="buscador" spellcheck="false" ToolTip="Entrada de datos para la búsqueda" />
                                <span class="caption"><asp:Label runat="server" meta:resourcekey="Buscar" /></span>
                            </div>
                        </div>
                    </div>

                    <!-- Recuento y tabla -->
                    <div class="row margin-top-25">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-12 col-sm-offset-0">
                            <div class="recuento right">
                                <div>
                                    <asp:Label ID="Recuento" runat="server" />
                                    <asp:Label runat="server" Text="<%$ Resources:General, Resultados %>" />
                                </div>
                            </div>
                            <div class="lista">
                                <asp:GridView ID="TablaBiblioteca" runat="server" AllowSorting="false" AllowPaging="false" DataSourceID="DataBiblioteca" AutoGenerateColumns="False" GridLines="None" OnRowDataBound="TablaBiblioteca_RowDataBound" ShowHeader="false">
                                    <Columns>
                                        <asp:BoundField DataField="ID"           />
                                        <asp:BoundField DataField="Bibliografía" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <!-- PDF -->
                    <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-3 col-md-4 col-md-offset-4">
                        <a ID="LinkDescargarPDF" runat="server" href="#" target="_blank" class="boton descarga margin-top-50">
                            <asp:Label runat="server" Text="<%$ Resources:General, Descargar %>" CssClass="texto" />
                            <i class="icono fa fa-angle-down"></i>
                        </a>
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
                                    <p>Este servicio ofrece un listado de fuentes bibliográficas (figura 1). Encima podemos ver un selector y una de entrada de texto (un buscador). El primero sirve para filtrar las fuentes bibliográficas por la letra inicial del primer apellido del autor. El segundo sirve para buscar una fuente bibliográfica que contenga el texto que hayamos escrito.</i></p>
                                    <asp:Image runat="server" ImageUrl="~/Images/Ayuda/biblioteca1.png" CssClass="img-responsive" />
                                    <span class="caption"><b>Figura 1.</b> Listado de fuentes bibliográficas.</span>
                                    <p>El buscador permite establecer un filtro de búsqueda más allá de la letra inicial del primer apellido del autor. Por ejemplo, puede interesarnos buscar aquellas fuentes bibliográficas en las que aparezca la palabra <span class="blue">Madrid</span>. Cuando hayamos terminado de escribir, hacemos clic en ENTER y el listado se refrescará mostrando únicamente aquellos resultados que satisfagan el filtro de búsqueda. Como vemos en la figura 2, no estamos restringidos a escribir una sola palabra.</p>
                                    <asp:Image runat="server" ImageUrl="~/Images/Ayuda/biblioteca2.png" CssClass="img-responsive" />
                                    <span class="caption"><b>Figura 2.</b> Resultados de búsqueda.</span>
                                    <p>Debajo del listado tenemos un botón para descargar un fichero PDF con el listado completo de fuentes biblográficas. No obstante, si hemos aplicado algún filtro, el PDF generado contendrá solo aquellas fuentes bibliográficas que satisfagan dicho filtro.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </section>

    <!-- ───────────────────── -->
    <!--     DB CONNECTION     -->
    <!-- ───────────────────── -->

    <!-- Biblioteca -->
    <asp:AccessDataSource ID="DataBiblioteca" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" OnSelected="DataBiblioteca_Selected" SelectCommand="SELECT [ID], [Bibliografía] FROM [Biblioteca] ORDER BY [Bibliografía] ASC" />

</asp:Content>