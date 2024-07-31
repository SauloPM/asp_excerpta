<%@ Page Title="Excerpta" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Extracto.aspx.cs" Inherits="Busqueda.Extracto" Culture="auto:es-ES" UICulture="auto" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <section id="extracto" class="padding-top-0">
        
        <div class="torn-page-heading">
            <div class="overlay"></div>
        </div>

        <div class="container">

            <!-- Ficha -->
            <div class="row">
                <div class="extracto">

                    <!-- Miniatura -->
                    <div class="col-xs-10 col-xs-offset-1 col-sm-10 col-sm-offset-1 col-md-4 col-md-offset-0">
                        <asp:UpdatePanel runat="server" class="miniatura">
                            <ContentTemplate>

                                <!-- Imagen -->
                                <asp:Image ID="Miniatura" runat="server" />

                                <!-- Navegación -->
                                <asp:LinkButton ID="ButtonAnteriorMiniatura"  runat="server" OnClick="ButtonAnteriorMiniatura_Click"  CssClass="flecha izquierda" >
                                    <i class="icono fa fa-angle-left" aria-hidden="true"></i>
                                </asp:LinkButton>
                                <asp:HyperLink ID="LinkMiniaturaPDF" runat="server" Target="_blank" CssClass="lupa" NavigateUrl="#">
                                    <i class="icono fa fa-search" aria-hidden="true"></i>
                                </asp:HyperLink>
                                <asp:LinkButton ID="ButtonSiguienteMiniatura" runat="server" OnClick="ButtonSiguienteMiniatura_Click" CssClass="flecha derecha" >
                                    <i class="icono fa fa-angle-right" aria-hidden="true"></i>
                                </asp:LinkButton>

                                <!-- Índice -->
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
    </section>
</asp:Content>
