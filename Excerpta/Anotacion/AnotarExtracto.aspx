<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnotarExtracto.aspx.cs" Inherits="Excerpta.AnotarExtracto" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <!-- Pestañas -->
    <ul class="pestanas">
        <li class="active">
            <asp:HyperLink runat="server" NavigateUrl="~/AnotarExtracto.aspx" Text="Anotar extracto" />
        </li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/MisExtractos.aspx" Text="Mis extractos" />
        </li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/Ayuda.aspx" Text="Ayuda" />
        </li>
    </ul>

    <!--
    ╔════════════════════╗
    ║     FORMULARIO     ║
    ╚════════════════════╝
    -->

    <asp:UpdatePanel ID="UpdateCrearExtracto" runat="server">
        <ContentTemplate>

            <asp:Panel ID="Notificacion" runat="server" Visible="false" CssClass="notificacion" />

            <!-- Encabezado -->
            <div class="encabezado margin-top-80-mobile">
                <h2>Anotar extracto</h2>
                <h3>Transcriba un nuevo extracto</h3>
            </div>

            <hr />

            <!-- Florilegio, autor, obra y página -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropFlorilegios" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataFlorilegios" DataTextField="Título" DataValueField="Título" OnSelectedIndexChanged="DropFlorilegios_SelectedIndexChanged" ToolTip="Selector de florilegios" >
                            <Items>
                                <asp:ListItem Text="Seleccionar florilegio" Value="%" Selected="True" />
                            </Items>
                        </asp:DropDownList>
                        <span class="caption">Florilegio&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropAutores" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataAutores" DataTextField="Autor" DataValueField="Autor" Enabled="false" OnSelectedIndexChanged="DropAutores_SelectedIndexChanged" ToolTip="Selector de autores" >
                            <Items>
                                <asp:ListItem Selected="True" Text="" />
                            </Items>
                        </asp:DropDownList>
                        <span class="caption">Autor&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropObras" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataObras" DataTextField="Obra" DataValueField="Obra" Enabled="false" OnSelectedIndexChanged="DropObras_SelectedIndexChanged" ToolTip="Selector de obras" >
                            <Items>
                                <asp:ListItem Selected="True" Text="" />
                            </Items>
                        </asp:DropDownList>
                        <span class="caption">Obra&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputPagina" runat="server" MaxLength="10" Enabled="false" autocomplete="off" ToolTip="Entrada de datos para la página" />
                        <span class="caption">Página</span>
                    </div>
                </div>
            </div>

            <!-- Libro, poema y versos -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputLibro" runat="server" MaxLength="6" Enabled="false" autocomplete="off" ToolTip="Entrada de datos para el libro" />
                        <span class="caption">Libro</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputPoema" runat="server" MaxLength="6" Enabled="false" autocomplete="off" ToolTip="Entrada de datos para el poema" />
                        <span class="caption">Poema</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputVersoInicial" runat="server" MaxLength="6" Enabled="false" autocomplete="off" ToolTip="Entrada de datos para el verso inicial" />
                        <span class="caption">Verso inicial</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-3 col-md-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputVersoFinal" runat="server" MaxLength="6" Enabled="false" autocomplete="off" ToolTip="Entrada de datos para el verso final" />
                        <span class="caption">Verso final</span>
                    </div>
                </div>
            </div>

            <!-- Capítulo y subcapítulo -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-6 col-sm-offset-0 col-md-6 col-md-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputCapitulo" runat="server" MaxLength="250" Enabled="false" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para capítulo" />
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
                                <asp:TextBox ID="InputExtracto" runat="server" Enabled="false" TextMode="MultiLine" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el extracto" />
                                <span class="caption">Extracto</span>
                            </div>
                        </div>
                        <div class="item">
                            <div class="entrada">
                                <asp:TextBox ID="InputTLL" runat="server" Enabled="false" TextMode="MultiLine" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para el TLL" />
                                <span class="caption">TLL</span>
                            </div>
                        </div>
                        <div class="item">
                            <div class="entrada">
                                <asp:TextBox ID="InputVernacula" runat="server" Enabled="false" TextMode="MultiLine" autocomplete="off" spellcheck="false" ToolTip="Entrada de datos para la lengua vernácula" />
                                <span class="caption">Vernácula</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Botón -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                    <asp:Button ID="BotonGuardar" runat="server" Text="Guardar" Enabled="false" OnClick="BotonCrearExtracto_Click" CssClass="boton" />
                </div>
            </div>

            <!-- Campos obligatorios -->
            <asp:RequiredFieldValidator     runat="server" ControlToValidate="InputPagina"       Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator     runat="server" ControlToValidate="InputLibro"        Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator     runat="server" ControlToValidate="InputPoema"        Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator     runat="server" ControlToValidate="InputVersoInicial" Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator     runat="server" ControlToValidate="InputVersoFinal"   Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator     runat="server" ControlToValidate="InputExtracto"     Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator     runat="server" ControlToValidate="InputTLL"          Display="None" SetFocusOnError="true" />

            <%--<asp:RegularExpressionValidator runat="server" ControlToValidate="InputExtracto"     Display="None" SetFocusOnError="true" ValidationExpression="^[\s\S]{0,500000}$" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="InputTLL"          Display="None" SetFocusOnError="true" ValidationExpression="^[\s\S]{0,500000}$" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="InputVernacula"    Display="None" SetFocusOnError="true" ValidationExpression="^[\s\S]{0,500000}$" />--%>

            <!-- Caracteres permitidos -->
            <asp:FilteredTextBoxExtender    runat="server" TargetControlID="InputLibro"          ValidChars="0123456789" />
            <asp:FilteredTextBoxExtender    runat="server" TargetControlID="InputPoema"          ValidChars="0123456789" />
            <asp:FilteredTextBoxExtender    runat="server" TargetControlID="InputVersoInicial"   ValidChars="0123456789" />
            <asp:FilteredTextBoxExtender    runat="server" TargetControlID="InputVersoFinal"     ValidChars="0123456789" />
            <asp:FilteredTextBoxExtender    runat="server" TargetControlID="InputPagina"         ValidChars="0123456789" />

            <!-- Validaciones personalizadas -->
            <asp:CustomValidator runat="server" OnServerValidate="ValidarVersos"           />
            <asp:CustomValidator runat="server" OnServerValidate="ValidarExtractoRepetido" />

        </ContentTemplate>
    </asp:UpdatePanel>

    <!--
    ╔═════════════════════════╗
    ║     ORIGEN DE DATOS     ║
    ╚═════════════════════════╝
    -->

    <!-- Desplegables -->
    <asp:AccessDataSource ID="DataFlorilegios" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" SelectCommand="SELECT          [Título] FROM Florilegios   ORDER BY [Título]" />
    <asp:AccessDataSource ID="DataAutores"     runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" SelectCommand="SELECT DISTINCT Autor    FROM Descripciones ORDER BY Autor"    />
    <asp:AccessDataSource ID="DataObras"       runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" SelectCommand="SELECT DISTINCT Obra     FROM Descripciones ORDER BY Obra"     />

    <!--
    ╔═════════════════╗
    ║     SPINNER     ║
    ╚═════════════════╝
    -->

    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateCrearExtracto">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
