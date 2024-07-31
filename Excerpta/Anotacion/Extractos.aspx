<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Extractos.aspx.cs" Inherits="Excerpta.Extractos" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <!-- Pestañas -->
    <ul class="pestanas">
        <li class="active">
            <asp:HyperLink runat="server" NavigateUrl="~/Extractos.aspx"    Text="Extractos"    />
        </li>
        <li>
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
        <h2>Extractos</h2>
        <h3>Administración de los extractos de todos los usuarios</h3>
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
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-4 col-sm-offset-0">
                    <div class="entrada">
                        <asp:TextBox ID="InputID" runat="server" AutoPostBack="true" OnTextChanged="InputID_TextChanged" CssClass="entrada" autocomplete="off" ToolTip="Entrada de datos para el ID" />
                        <span class="caption">ID</span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-4 col-sm-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropFiltroEstados" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="DropFiltroEstados_SelectedIndexChanged" ToolTip="Selector de estados">
                            <asp:ListItem Text="Todos los extractos"     Value="%" Selected="True" />
                            <asp:ListItem Text="Extractos publicados"    Value="-1" />
                            <asp:ListItem Text="Extractos despublicados" Value="0"  />
                        </asp:DropDownList>
                        <span class="caption">Estado&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-4 col-sm-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropFiltroUsuarios" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataFiltroUsuarios" DataTextField="Usuario" DataValueField="Usuario" OnSelectedIndexChanged="DropFiltroUsuarios_SelectedIndexChanged" ToolTip="Selector de usuarios">
                            <asp:ListItem Text="Todos los usuarios" Value="%" Selected="True"  />
                        </asp:DropDownList>
                        <span class="caption">Usuario&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-4 col-sm-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropFiltroFlorilegios" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataFiltroFlorilegios" DataTextField="Florilegio" DataValueField="Florilegio" OnSelectedIndexChanged="DropFiltroFlorilegios_SelectedIndexChanged" ToolTip="Selector de florilegios" />
                        <span class="caption">Florilegio&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-4 col-sm-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropFiltroAutores" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataFiltroAutores" DataTextField="Autor" DataValueField="Autor" OnSelectedIndexChanged="DropFiltroAutores_SelectedIndexChanged" ToolTip="Selector de autores" />
                        <span class="caption">Autor&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-4 col-sm-offset-0">
                    <div class="selector">
                        <asp:DropDownList ID="DropFiltroObras" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="DataFiltroObras" DataTextField="Obra" DataValueField="Obra" OnSelectedIndexChanged="DropFiltroObras_SelectedIndexChanged" ToolTip="Selector de obras" />
                        <span class="caption">Obra&nbsp;<i class="fa fa-fw fa-chevron-down" aria-hidden="true"></i></span>
                    </div>
                </div>
            </div>

            <!-- Caracteres permitidos -->
            <asp:FilteredTextBoxExtender runat="server" TargetControlID="InputID" ValidChars=" ,0123456789" />

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
                    <div class="recuento">
                        <div>
                            <asp:LinkButton ID="ButtonMarcar"    runat="server" Text="Marcar todo"    OnClick="ButtonMarcarTodos_Click"    />
                            &nbsp;|&nbsp;
                            <asp:LinkButton ID="ButtonDesmarcar" runat="server" Text="Desmarcar todo" OnClick="ButtonDesmarcarTodos_Click" />
                        </div>
                        <asp:Label ID="RecuentoExtractos" runat="server" CssClass="recuento" />
                    </div>
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
                            <asp:BoundField DataField="Alta"         SortExpression="Alta"         HeaderText="Alta"          />
                            <asp:BoundField DataField="ID"           SortExpression="ID"           HeaderText="ID"            />
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
                            <asp:BoundField DataField="Usuario"      SortExpression="Usuario"      HeaderText="Usuario"      />
                            <asp:CommandField ButtonType="Link" ShowSelectButton="true" SelectText="Seleccionar" ControlStyle-CssClass="seleccionar" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>

    <!-- PDF -->
    <div class="row">
        <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
            <a ID="BotonPDF" runat="server" href="#" target="_blank" class="boton margin-bottom-35">Generar PDF</a>
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

            <!-- Entrada oculta -->
            <asp:TextBox ID="ExtractosSeleccionados" runat="server" Enabled="false" CssClass="hidden" ToolTip="Entrada de datos oculta" />

            <!-- Botones -->
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-2">
                    <asp:Button ID="ButtonPublicar"    runat="server" Text="Publicar"    OnClick="ButtonPublicarExtracto_Click"    Enabled="false" CssClass="boton margin-bottom-35" />
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-0">
                    <asp:Button ID="ButtonDespublicar" runat="server" Text="Despublicar" OnClick="ButtonDespublicarExtracto_Click" Enabled="false" CssClass="boton margin-bottom-35" />
                </div>
            </div>

            <!-- Caracteres permitidos -->
            <asp:FilteredTextBoxExtender runat="server" TargetControlID="ExtractosSeleccionados" ValidChars="0123456789," />

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
                        <asp:Chart ID="ChartFlorilegios" runat="server" EnableViewState="true" Width="700" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!--
    ╔═════════════════╗
    ║     BACKUPS     ║
    ╚═════════════════╝
    -->

    <div class="separador">
        <h5>Copias de seguridad</h5>
    </div>

    <div class="row">

        <!-- Descargar backup -->
        <div class="col-xs-10 col-xs-offset-1 col-sm-5 col-sm-offset-1">
            <asp:UpdatePanel runat="server" ID="UpdateBackup">
                <ContentTemplate>
                    <asp:Image runat="server" ImageUrl="~/Images/exportarbackup.png" Width="175" CssClass="backup" AlternateText="Exportar copia de seguridad" />
                    <asp:Button runat="server" Text="Crear" CssClass="boton margin-bottom-35" OnClick="BotonDescargarBackup_Click" />
                    <a ID="LinkDescargarBackup" runat="server" href="#" target="_blank" class="boton disabled">Descargar</a>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <!-- Subir backup -->
        <div class="col-xs-10 col-xs-offset-1 col-sm-5 col-sm-offset-0">
            <asp:Image runat="server" ImageUrl="~/Images/importarbackup.png" Width="175" CssClass="backup margin-top-35-mobile" AlternateText="Importar copia de seguridad" />
            <a id="subir-backup" class="boton margin-bottom-35" href="#">Seleccionar archivo</a>
            <asp:FileUpload ID="BotonOcultoBackup" runat="server" CssClass="hidden" />
            <asp:Button ID="BotonImportarBackup" runat="server" CssClass="boton" Text="Importar" OnClick="ButtonImportarXML_Click" Enabled="false" />
        </div>
    </div>

    <!--
    ╔═════════════════════════╗
    ║     ORIGEN DE DATOS     ║
    ╚═════════════════════════╝
    -->

    <!-- Filtro de búsqueda -->
    <asp:AccessDataSource ID="DataFiltroFlorilegios" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" />
    <asp:AccessDataSource ID="DataFiltroAutores"     runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" />
    <asp:AccessDataSource ID="DataFiltroObras"       runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" />
    <asp:AccessDataSource ID="DataFiltroUsuarios"    runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" SelectCommand="Select DISTINCT Usuario FROM Extractos WHERE Usuario IS NOT NULL ORDER BY Usuario" />
    
    <!-- Tabla de extractos -->
    <asp:AccessDataSource ID="DataExtractos" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" OnSelected="DataExtractos_Selected" />

    <!-- Gráfico -->
    <asp:AccessDataSource ID="DataRecuento" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" />

    <!-- Tabla de palabras (?) -->
    <asp:AccessDataSource ID="DataPalabras" runat="server" DataFile="c:/inetpub/excerpta/excerpta.accdb" InsertCommand="INSERT INTO Palabras (Palabra, ExtractoID, Grupo) VALUES (@Palabra, @ExtractoID, @Grupo)">
        <InsertParameters>
            <asp:Parameter Name="Palabra"    Type="String" />
            <asp:Parameter Name="ExtractoID" Type="Int32"  />
            <asp:Parameter Name="Grupo"      Type="String" />
        </InsertParameters>
    </asp:AccessDataSource>

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

    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdateBackup">
        <ProgressTemplate>
            <div class="spinner">
                <asp:Image runat="server" CssClass="logo" ImageUrl="~/Images/logo.svg" AlternateText="Logotipo del Proyecto Excerpta" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
