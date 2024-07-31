<%@ Page Title="Excerpta" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Busqueda._Default" Culture="auto:es-ES" UICulture="auto" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdateGeneral" runat="server">
        <ContentTemplate>

            <asp:Panel ID="Notificacion" runat="server" Visible="false" CssClass="notificacion" />

            <!--
            ╔═════════════════╗
            ║     PORTADA     ║
            ╚═════════════════╝
            -->

            <header id="portada">

                <asp:Image runat="server" ImageUrl="~/Images/logo.svg" CssClass="logo" AlternateText="Logotipo del Proyecto Excerpta" />

                <h1 class="titulo">
                    <asp:Label runat="server" meta:resourcekey="Portada" />
                </h1>

                <a class="flecha smooth-scrolling" href="#bienvenido" title="Saber más">
                    <i class="fa fa-angle-down" aria-hidden="true"></i>
                </a>

                <div class="overlay"></div>

            </header>

            <!--
            ╔════════════════════╗
            ║     BIENVENIDO     ║
            ╚════════════════════╝
            -->

            <section id="bienvenido" class="padding-bottom-0">
                <div class="container">

                    <!-- Encabezado -->
                    <h2><asp:Label runat="server" meta:resourcekey="BienvenidoTitulo" /></h2>
                    <hr />
                    <h3><asp:Label runat="server" meta:resourcekey="BienvenidoSubtitulo" /></h3>

                    <!-- Texto -->
                    <div class="row">
                        <div class="col-xs-10 col-xs-offset-1">
                            <p><asp:Label runat="server" meta:resourcekey="BienvenidoParrafo1" /></p>
                            <p><asp:Label runat="server" meta:resourcekey="BienvenidoParrafo2" /></p>
                        </div>
                    </div>
                </div>
            </section>

            <!--
            ╔═══════════════════╗
            ║     SERVICIOS     ║
            ╚═══════════════════╝
            -->

            <section id="servicios" class="padding-top-0">
                <div class="container">
                    <div class="row">

                        <!-- Encabezado -->
                        <h3 class="no-margin"><asp:Label runat="server" meta:resourcekey="ServiciosTitulo" /></h3>
                        <hr style="margin: 25px 0 75px" />

                        <!-- Ítems -->
                        <div class="col-xs-10 col-xs-offset-1 col-sm-4 col-sm-offset-0">
                            <div class="servicio hojear">
                                <div class="overlay"></div>
                                <h4 class="titulo"><asp:Label runat="server" meta:resourcekey="ServiciosItem1" /></h4>
                                <i class="icono fa fa-fw fa-plus" aria-hidden="true"></i>
                            </div>
                            <p class="caption"><asp:Label runat="server" meta:resourcekey="ServiciosParrafo1" /></p>
                        </div>
                        <div class="col-xs-10 col-xs-offset-1 col-sm-4 col-sm-offset-0">
                            <div class="servicio buscar">
                                <div class="overlay"></div>
                                <h4 class="titulo"><asp:Label runat="server" meta:resourcekey="ServiciosItem2" /></h4>
                                <i class="icono fa fa-fw fa-plus" aria-hidden="true"></i>
                            </div>
                            <p class="caption"><asp:Label runat="server" meta:resourcekey="ServiciosParrafo2" /></p>
                        </div>
                        <div class="col-xs-10 col-xs-offset-1 col-sm-4 col-sm-offset-0">
                            <div class="servicio biblioteca">
                                <div class="overlay"></div>
                                <h4 class="titulo"><asp:Label runat="server" meta:resourcekey="ServiciosItem3" /></h4>
                                <i class="icono fa fa-fw fa-plus" aria-hidden="true"></i>
                            </div>
                            <p class="caption"><asp:Label runat="server" meta:resourcekey="ServiciosParrafo3" /></p>
                        </div>
                    </div>
                </div>
            </section>

            <!--
            ╔═════════════════╗
            ║     CALLOUT     ║
            ╚═════════════════╝
            -->

            <aside id="callout">
                <div class="overlay"></div>
                <div class="container">
                    <i class="comillas fa fa-quote-left" aria-hidden="true"></i>
                    <p class="texto"><asp:Label runat="server" meta:resourcekey="Callout" /></p>
                    <i class="comillas fa fa-quote-right" aria-hidden="true"></i>
                </div>
            </aside>

            <!--
            ╔══════════════════╗
            ║     PROYECTO     ║
            ╚══════════════════╝
            -->

            <section id="proyecto">
                <div class="container">
                    <div class="row">
                        <div class="col-xs-10 col-xs-offset-1">

                            <h2><asp:Label runat="server" meta:resourcekey="ProyectoTitulo" /></h2>
                            <hr />
                            <h3><asp:Label runat="server" meta:resourcekey="ProyectoSubtitulo" /></h3>
                    
                            <!-- Texto -->
                            <p><asp:Label runat="server" meta:resourcekey="ProyectoParrafo1" /></p>
                            <p><asp:Label runat="server" meta:resourcekey="ProyectoParrafo2" /></p>

                            <!-- Cabecera -->
                            <h3 style="margin: 0"><asp:Label runat="server" meta:resourcekey="CreditosTitulo" /></h3>
                            <hr style="margin: 25px 0 75px" />

                            <p><asp:Label runat="server" meta:resourcekey="CreditosParrafo1" /></p>
                            <p><asp:Label runat="server" meta:resourcekey="CreditosParrafo2" /></p>
                    
                            <!-- Acordeón -->
                            <div class="acordeon">

                                <!-- MURMELLIUS -->
                                <div class="acordeon-item">
                                    <div class="acordeon-header">
                                        <span class="titulo">Ex Elegiis Tibulli, Propertii et Ouidii ab Ioanne Murmellio Selecti uersus</span>
                                        <span class="ubicacion">París, 1533 (=Breda, 1504)</span>
                                        <i class="flecha"></i>
                                    </div>
                                    <div class="acordeon-body">
                                        <ul>
                                            <li><b>Gregorio Rodríguez Herrera</b></li>
                                        </ul>
                                    </div>
                                </div>

                                <!-- SYLVA -->
                                <div class="acordeon-item">
                                    <div class="acordeon-header">
                                        <span class="titulo">Sylva sententiarum ex Ouidio, non librorum sed rerum ac titulorum ordine seruato, delectarum</span>
                                        <span class="ubicacion">Leipzig, 1515</span>
                                        <i class="flecha"></i>
                                    </div>
                                    <div class="acordeon-body">
                                        <ul>
                                            <li><b>Gregorio Rodríguez Herrera</b> » En proceso</li>
                                        </ul>
                                    </div>
                                </div>

                                <!-- STEPHANUS -->
                                <div class="acordeon-item">
                                    <div class="acordeon-header">
                                        <span class="titulo">Sententiae et prouerbia ex poetis latinis (ab Roberto Stephano) – His adiecimus Leosthenis Coluandri sententias prophanas</span>
                                        <span class="ubicacion">Venecia, 1547 (=París 1534)</span>
                                        <i class="flecha"></i>
                                    </div>
                                    <div class="acordeon-body">
                                        <ul>
                                            <li><b>Trinidad Arcos Pereira</b> » Plauto y Terencio</li>
                                            <li><b>Francisco Bravo de Laguna Romero</b> » Juvenal, Marcial y Séneca</li>
                                            <li><b>Mª Elena Curbelo Tavío</b> » Boecio y Lucano</li>
                                            <li><b>Mª Elisa Cuyás de Torres</b> » Claudiano, Estacio, Horacio, Ovidio (Metamorfosis, Fastos, Arte de amar, Remedios, Heroidas, Pónticas, Tristes, Consolación y Cosmética) y Virgilio</li>
                                            <li><b>Mª Dolores García de Paso</b> Carrasco » Enio, Persio, Silio Itálico y Valerio Flaco</li>
                                            <li><b>Gregorio Rodríguez Herrera</b> » Catulo, Lucrecio, Ovidio (Amores), Propercio y Tibulo</li>
                                        </ul>
                                    </div>
                                </div>

                                <!-- MAIOR -->
                                <div class="acordeon-item">
                                    <div class="acordeon-header">
                                        <span class="titulo">Sententiae ueterum poetarum, per locos communes digestae. Collectore Georgio Maiore</span>
                                        <span class="ubicacion">Amberes 1541(= Magdeburgo 1534)</span>
                                        <i class="flecha"></i>
                                    </div>
                                    <div class="acordeon-body">
                                        <ul>
                                            <li><b>Trinidad Arcos Pereira</b> » Estacio, Ovidio (Fastos, Pónticas y Tristes) Plauto y Terencio</li>
                                            <li><b>Francisco Bravo de Laguna Romero</b> » Juvenal, Marcial, Ovidio (Metamorfosis) y Publilio Siro</li>
                                            <li><b>Mª Elena Curbelo Tavío</b> » Ausonio, Boecio y Lucano</li>
                                            <li><b>Mª Dolores García de Paso Carrasco</b> » Enio, Horacio, Persio, Silio Itálico, Valerio Flaco, Virgilio, Appendix Vergiliana y Appendix Epigrammata</li>
                                            <li><b>Gregorio Rodríguez Herrera</b> » Catulo, Lucrecio, Manilio, Maximiano Estrusco (≈ Cornelio Galo) Ovidio (Amores, Arte de amar, Remedios, Heroidas y Cosmética), Propercio y Tibulo</li>
                                        </ul>
                                    </div>
                                </div>

                                <!-- MIRÁNDULA -->
                                <div class="acordeon-item">
                                    <div class="acordeon-header">
                                        <span class="titulo">Illustrium poetarum flores per Octauianum Mirandulam collecti</span>
                                        <span class="ubicacion">Estrasburgo, 1538</span>
                                        <i class="flecha"></i>
                                    </div>
                                    <div class="acordeon-body">
                                        <ul>
                                            <li><b>Trinidad Arcos Pereira</b> » Estacio, Petronio, Plauto, Nemesiano y Terencio</li>
                                            <li><b>Francisco Bravo de Laguna Romero</b> » Juvenal, Marcial, Ovidio (Metamorfosis, Fastos, Pónticas, Tristes y Consolación) y Séneca</li>
                                            <li><b>Mª Elena Curbelo Tavío</b> » Ausonio, Boecio y Lucano</li>
                                            <li><b>Mª Dolores García de Paso Carrasco</b> » Enio, Horacio, Persio, Valerio Flaco, Silio Itálico, Virgilio, Appendix Vergiliana y Appendix Epigrammata</li>
                                            <li><b>Gregorio Rodríguez Herrera</b> » Catulo, Claudiano, Lucrecio, Manilio, Maximiano Estrusco (≈ Cornelio Galo) Ovidio (Amores, Arte de amar, Remedios, Heroidas, Ibis y Cosmética), Propercio y Tibulo</li>
                                        </ul>
                                    </div>
                                </div>
                                
                                <!-- LAGNERIUS -->
                                <div class="acordeon-item">
                                    <div class="acordeon-header">
                                        <span class="titulo">Illustres quaedam sententiae ex optimis quibusque aliis autoribus selectae, per eundem Petrum Lagnerium (Intra “Ex M. T. Cicerone insignium sententiarum elegans et perutile compendium. Autore Petro Lagnerio compendiensi”)</span>
                                        <span class="ubicacion">París, 1553 (=Beauvais, 1550)</span>
                                        <i class="flecha"></i>
                                    </div>
                                    <div class="acordeon-body">
                                        <ul>
                                            <li><b>Gregorio Rodríguez Herrera</b> » En proceso</li>
                                        </ul>
                                    </div>
                                </div>

                                <!-- SCHONBORN -->
                                <div class="acordeon-item">
                                    <div class="acordeon-header">
                                        <span class="titulo">Versus Sententiosi et eximii, iuxta litterarum ordinem e ueteribus poetis consignati a Bartolemeo Schonborn Witebergensi</span>
                                        <span class="ubicacion">Witenberg, 1565</span>
                                        <i class="flecha"></i>
                                    </div>
                                    <div class="acordeon-body">
                                        <ul>
                                            <li><b>María Elena Curbelo Tavío</b></li>
                                        </ul>
                                    </div>
                                </div>

                                <!-- IESU -->
                                <div class="acordeon-item">
                                    <div class="acordeon-header">
                                        <span class="titulo">Flores poetarum hieme et aetate fragantes siue sentensiosi uersus ex ueterum praecipue poetarum topiariis. In collegio Soc. Jesu ad s. Clementem</span>
                                        <span class="ubicacion">Praga, 1684</span>
                                        <i class="flecha"></i>
                                    </div>
                                    <div class="acordeon-body">
                                        <ul>
                                            <li><b>María Elena Curbelo Tavío</b> » En proceso</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>

                            <!-- Texto -->
                            <p><asp:Label runat="server" meta:resourcekey="CreditosParrafo3" /></p>
                            <p><asp:Label runat="server" meta:resourcekey="CreditosParrafo4" /></p>

                            <!-- Gráfico -->
                            <asp:Panel runat="server" Wrap="true" CssClass="grafico margin-bottom-50">
                                <asp:Chart ID="ChartFlorilegios" runat="server" EnableViewState="true" Height="240" Width="450">
                                    <Series>
                                        <asp:Series Name="Serie" ChartType="Doughnut" />
                                    </Series>
                                    <Legends>
                                        <asp:Legend Alignment="Center" />
                                    </Legends>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea" />
                                    </ChartAreas>
                                </asp:Chart>
                            </asp:Panel>

                            <!-- Tabla -->
                            <div class="tabla">
                                <asp:GridView ID="TablaRecuento" runat="server" DataSourceID="DataRecuento" ShowHeader="true" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" GridLines="None" EmptyDataText="No se han encontrado datos">
                                    <Columns>
                                        <asp:BoundField DataField="Florilegio" SortExpression="Florilegio" HeaderText="<%$ Resources:General, Florilegio %>" />
                                        <asp:BoundField DataField="Recuento"   SortExpression="Recuento"   HeaderText="<%$ Resources:General, Recuento   %>"   />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <!--
            ╔══════════════════╗
            ║     CONTACTO     ║
            ╚══════════════════╝
            -->

            <section id="contacto">
                <div class="container">

                    <div class="overlay"></div>

                    <!-- Encabezado -->
                    <h2><asp:Label runat="server" meta:resourcekey="ContactoTitulo" /></h2>
                    <hr />
                    <h3><asp:Label runat="server" meta:resourcekey="ContactoSubtitulo" /></h3>

                    <!-- Entradas -->
                    <div class="row margin-top-25">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-3">
                            <div class="entrada">
                                <asp:TextBox ID="InputNombre" runat="server" AutoCompleteType="Disabled" MaxLength="100" ToolTip="Entrada de datos para el nombre" spellcheck="false" />
                                <span class="caption"><asp:Label runat="server" meta:resourcekey="ContactoPlaceholder1" /></span>
                            </div>
                        </div>
                    </div>
                    <div class="row margin-top-25">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-3">
                            <div class="entrada">
                                <asp:TextBox ID="InputEmail" runat="server" AutoCompleteType="Disabled" MaxLength="100" ToolTip="Entrada de datos para el email" spellcheck="false" TextMode="Email" />
                                <span class="caption"><asp:Label runat="server" meta:resourcekey="ContactoPlaceholder2" /></span>
                            </div>
                        </div>
                    </div>
                    <div class="row margin-top-25">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-3">
                            <div class="entrada">
                                <asp:TextBox ID="InputAsunto" runat="server" AutoCompleteType="Disabled" MaxLength="250" ToolTip="Entrada de datos para el asunto" spellcheck="false" />
                                <span class="caption"><asp:Label runat="server" meta:resourcekey="ContactoPlaceholder3" /></span>
                            </div>
                        </div>
                    </div>
                    <div class="row margin-top-25">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-3">
                            <div class="entrada">
                                <asp:TextBox ID="InputMensaje" runat="server" AutoCompleteType="Disabled" MaxLength="500" ToolTip="Entrada de datos para el mensaje" spellcheck="false" />
                                <span class="caption"><asp:Label runat="server" meta:resourcekey="ContactoPlaceholder4" /></span>
                            </div>
                        </div>
                    </div>

                    <!-- Botón -->
                    <div class="row margin-top-50">
                        <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-3">
                            <asp:LinkButton ID="ButtonEnviar" runat="server" OnClick="ButtonEnviar_Click" CssClass="boton contacto">
                                <asp:Label runat="server" meta:resourcekey="ContactoBoton" CssClass="texto" />
                                <i class="icono fa fa-plane"></i>
                            </asp:LinkButton>
                        </div>
                    </div>

                    <!-- Campos obligatorios -->
                    <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true" ControlToValidate="InputNombre"  />
                    <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true" ControlToValidate="InputEmail"   />
                    <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true" ControlToValidate="InputAsunto"  />
                    <asp:RequiredFieldValidator runat="server" Display="None" SetFocusOnError="true" ControlToValidate="InputMensaje" />

                </div>
            </section>

            <!--
            ╔═══════════════════╗
            ║     DIRECCIÓN     ║
            ╚═══════════════════╝
            -->

            <section id="location">
                <div class="container">
                    <i class="icono fa fa-map-marker" aria-hidden="true"></i>
                    <hr />
                    <p>Instituto Universitario de Análisis y Aplicaciones Textuates<br />Universidad de Las Palmas de Gran Canaria<br />35002 Las Palmas de Gran Canaria, Las Palmas</p>
                    <p><asp:HyperLink runat="server" NavigateUrl="http://iatext.ulpgc.es/es/investigador_grh" Target="_blank" Text="Dr. Gregorio Rodríguez Herrera" /></p>
                </div>
            </section>

        </ContentTemplate>
    </asp:UpdatePanel>

    <!--
    ╔═════════════════════════╗
    ║     ORIGEN DE DATOS     ║
    ╚═════════════════════════╝
    -->

    <!-- Recuento -->
    <asp:AccessDataSource ID="DataRecuento" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" SelectCommand="SELECT Florilegio, COUNT(Florilegio) AS Recuento FROM Extractos WHERE Alta=True AND Florilegio IS NOT NULL GROUP BY Florilegio" />

    <!--
    ╔═════════════════╗
    ║     SPINNER     ║
    ╚═════════════════╝
    -->

    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateGeneral">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>