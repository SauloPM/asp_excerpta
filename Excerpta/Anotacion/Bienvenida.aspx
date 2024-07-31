<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Bienvenida.aspx.cs" Inherits="Excerpta.Bienvenida" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">

    <!-- Encabezado -->
    <div class="encabezado">
        <h2>Bienvenido</h2>
        <h3>¿En qué consiste el Proyecto Excerpta?</h3>
    </div>

    <hr />

    <!-- Texto -->
    <div id="bienvenida">
        <div class="row">
            <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2">
                <p>
                    Aunque los florilegios han tenido una gran relevancia para la configuración del pensamiento europeo, hoy día siguen siendo estudiados en su mayoría con un método pretecnológico que impide obtener resultados significativos y con rapidez.
                </p>
                <p>
                    El <strong>Proyecto Excerpta</strong> ofrece un sistema de acceso digital online y sencillo a florilegios poéticos latinos etiquetados para la investigación comparada y por segmentos, a través de dos aplicativos: uno para la transcripción y etiquetado de los textos que permite además incorporar nuevos florilegios en el futuro; y otro para la consulta simple o combinada de los florilegios que permite obtener resultados de investigación significativos.
                </p>
                <p>
                    El proyecto supone una innovación incremental en el ámbito de los estudios de corpus, pero disruptiva para el estudio de la educación y la sociedad del Renacimiento, que puede contribuir a la cantidad y calidad de los trabajos al no depender los investigadores de la lectura directa de los florilegios para obtener datos para el análisis.
                </p>
                <p>    
                    El <strong>Proyecto Excerpta</strong> se desarrolla en el seno del Instituto Universitario de Análisis y Aplicaciones Textuales (IATEXT) de la Universidad de Las Palmas de Gran Canaria.
                </p>
            </div>
        </div>
    </div>
</asp:Content>
