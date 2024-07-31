<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ayuda.aspx.cs" Inherits="Excerpta.Ayuda" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <!-- Pestañas -->
    <ul class="pestanas">
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/AnotarExtracto.aspx" Text="Anotar extracto" />
        </li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/MisExtractos.aspx" Text="Mis extractos" />
        </li>
        <li class="active">
            <asp:HyperLink runat="server" NavigateUrl="~/Ayuda.aspx" Text="Ayuda" />
        </li>
    </ul>

    <!-- Encabezado -->
    <div class="encabezado margin-top-80-mobile">
        <h2>Ayuda</h2>
        <h3>Para cualquier duda sobre la gestión de extractos</h3>
    </div>

    <hr />

    <!--
    ╔════════════════╗
    ║     ÍNDICE     ║
    ╚════════════════╝
    -->

    <div id="ayuda">
        <div class="row">
            <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2">
                <h4>Índice</h4>
                <ul>
                    <li>
                        <a class="smooth-scrolling" href="#guia-anotar-extracto">
                            Anotar extracto
                        </a>
                    </li>
                    <li>
                        <a class="smooth-scrolling" href="#guia-editar-extracto">
                            Editar extracto
                        </a>
                    </li>
                    <li>
                        <a class="smooth-scrolling" href="#guia-eliminar-extracto">
                            Eliminar extracto
                        </a>
                    </li>
                    <li>
                        <a class="smooth-scrolling" href="#guia-filtro">
                            Utilizar el filtro de búsqueda
                        </a>
                    </li>
                    <li>
                        <a class="smooth-scrolling" href="#guia-pdf">
                            Exportar extractos anotados en PDF
                        </a>
                    </li>
                    <li>
                        <a class="smooth-scrolling" href="#guia-centon">
                            Técnica del Centón
                        </a>
                    </li>
                    <li>
                        <a class="smooth-scrolling" href="#guia-canon">
                            Qué hacer si el extracto no pertenece a una obra recogida en el canon que alimenta el TLL
                        </a>
                    </li>
                </ul>

                <!--
                ╔═════════════════════════╗
                ║     ANOTAR EXTRACTO     ║
                ╚═════════════════════════╝
                -->

                <h4 id="guia-anotar-extracto" style="margin-top: 50px">Anotar extracto</h4>
                <p>
                    Debemos hacer clic en la pestaña <asp:HyperLink runat="server" NavigateUrl="~/AnotarExtracto.aspx" Text="Anotar extracto" />.
                </p>
                <p>
                    En la primera fila de campos aparecen los desplegables para seleccionar un florilegio, un autor y una obra, respectivamente (figura 1). Una vez hayamos elegido un valor para cada uno, se activa el resto de campos del formulario que inicialmente se encuentran desactivados. Es decir, es obligatorio elegir un valor para cada desplegable de izquierda a derecha para que el resto de campos del formulario se active.
                </p>
                <asp:Image runat="server" ImageUrl="~/Images/Ayuda/1.png" AlternateText="Figura 1" />
                <p class="image-caption"><b>Figura 1.</b>&nbsp;Primera fila del formulario.</p>
                <p>
                    Para aquellos autores que solo tengan una obra (por ejemplo, Propercio o Lucano) y que se citen habitualmente solo con el nombre del autor, se debe seleccionar el símbolo Ø, como se observa en la figura 2.
                </p>
                <asp:Image runat="server" ImageUrl="~/Images/Ayuda/2.png" AlternateText="Figura 2" />
                <p class="image-caption"><b>Figura 2.</b>&nbsp;Primera fila del formulario con un valor seleccionado para cada desplegable.</p>
                <p>
                    En la segunda fila de campos (figura 3) debemos escribir manualmente el índice numérico del libro, poema y versos (inicial y final) del extracto del autor clásico según su edición contemporánea. En el caso de que la obra no esté estructurada en libros o poemas, se debe escribir el índice numérico 0 según corresponda.
                </p>
                <asp:Image runat="server" ImageUrl="~/Images/Ayuda/3.png" AlternateText="Figura 3" />
                <p class="image-caption"><b>Figura 3.</b>&nbsp;Segunda fila del formulario.</p>
                <p>
                    Asimismo, se vincula el extracto transcrito con la página de la correspondiente edición renacentista digitalizada proporcionada por el gestor de la aplicación web (último campo de la primera fila del formulario, como ilustra la figura 1). En el caso de que un extracto ocupe más de dos páginas, debe anotarse siempre la página en la que aparezca su primera referencia, ya sea el nombre del poeta o el primer verso del extracto.
                </p>
                <p>
                    Por ejemplo, en la figura 4 se ilustra una página en la que se encuentra un extracto de Tibulo. En el campo correspondiente del formulario debemos escribir el número de página del documento PDF (página 133), no el que aparece en el del folio de la edición renacentista (página 50).
                </p>
                <asp:Image runat="server" ImageUrl="~/Images/Ayuda/4.png" AlternateText="Figura 4" />
                <p class="image-caption"><b>Figura 4.</b>&nbsp;Número de página.</p>
                <p>
                    En la tercera fila de campos debemos escribir manualmente el título, lema o mote bajo el que se figure cada extracto en su florilegio, si lo hubiera. Dado que en algunos florilegios, como por ejemplo el de O. Mirandula, además del título, lema o mote principal los extractos se clasifican en un segundo nivel por subtítulos o subcapítulos, se ha habilitado un segundo campo para escribirlos (figura 5). En el caso de que el título se reproduzca mediante clichés del tipo "in eadem sententiam" o similar, se debe anotar el título precedente. 
                </p>
                <asp:Image runat="server" ImageUrl="~/Images/Ayuda/5.png" AlternateText="Figura 5" />
                <p class="image-caption"><b>Figura 5.</b>&nbsp;Tercera fila del formulario.</p>
                <p>
                    Finalmente, en la cuarta fila de campos encontramos tres campos (figura 6). En el primero debemos escribir el extracto que se encuentra en el florilegio. La transcripción del texto latino debe respetar las grafías y la puntuación del florilegio, a excepción de "u" semivocal, que siempre se transcribirá "u" (<i>uox</i>, <i>no vox o fuluo</i>, <i>no fulvo</i>) y de "i" semivocal, que siempre se transcribirá "i" (<i>iam</i>, <i>no jam</i>). Igualmente se debe reproducir la estructura en las estrofas latinas mediante el uso del retorno manual al final de cada verso.
                </p>
                <p>
                    Por otro lado, en el segundo campo debemos escribir el texto de la edición contemporánea que estemos utilizando, para lo que se recomienda el PHI Work Place, una versión digital del Thesaurus Linguae Latina, de ahí que esté presidido por las siglas TLL. Este texto debe respetar las grafías de la edición elegida.
                </p>
                <asp:Image runat="server" ImageUrl="~/Images/Ayuda/6.png" AlternateText="Figura 6" />
                <p class="image-caption"><b>Figura 6.</b>&nbsp;Cuarta fila del formulario.</p>
                <p>
                    Una vez hayamos introducido la información en las cuatro filas de campos que componen el formulario, debemos hacer clic en "Guardar". Una vez los datos se hayan guardado, los campos de la primera y segunda fila mantienen los datos, con el objeto de ahorrar trabajo cuando estamos transcribiendo un mismo autor, obra, libro y poema.
                </p>
                <p>
                    En aquellos casos en los que el extracto reduzca el texto original (como se observaba en la figura 6), debe escribirse el texto completo en el campo TLL. Además, en la segunda fila del formulario, concretamente en los campos Verso inicial y Verso final, debemos escribir los índices numéricos del TLL, no del extracto, como ilustra la figura 7.
                </p>
                <asp:Image runat="server" ImageUrl="~/Images/Ayuda/7.png" AlternateText="Figura 7" />
                <p class="image-caption"><b>Figura 7.</b>&nbsp;Índices numéricos.</p>

                <!--
                ╔═════════════════════════╗
                ║     EDITAR EXTRACTO     ║
                ╚═════════════════════════╝
                -->

                <h4 id="guia-editar-extracto" style="margin-top: 50px">Editar extracto</h4>
                <p>
                    Debemos hacer clic en la pestaña <asp:HyperLink runat="server" NavigateUrl="~/MisExtractos.aspx" Text="Mis extractos" />.
                </p>
                <p>
                    Veremos una tabla con todos los extractos que hayamos anotado. Cada extracto corresponde con una fila de la tabla. Cuando hayamos encontrado el extracto que deseemos editar, lo seleccionamos haciendo clic sobre él. Verás que la fila aparece ahora resaltada en gris, como ilustra la figura 8 para el extracto de ID 1175.
                </p>
                <asp:Image runat="server" ImageUrl="~/Images/Ayuda/8.png" AlternateText="Figura 8" />
                <p class="image-caption"><b>Figura 8.</b>&nbsp;Selección de extracto.</p>
                <p>
                    Después de haber seleccionado el extracto, veremos que el formulario que se encuentra debajo de la tabla se ha rellenado automáticamente con los datos del extracto seleccionado, como se observa en la figura 9. También vemos que se han activado los dos botones de edición y eliminación que podemos encontrar debajo de dicho formulario. Para editar uno o más campos del extracto seleccionado, solo debemos realizar las modificaciones que deseemos en el formulario y hacer clic en el botón "Editar".
                </p>
                <asp:Image runat="server" ImageUrl="~/Images/Ayuda/9.png" AlternateText="Figura 9" />
                <p class="image-caption"><b>Figura 9.</b>&nbsp;Formulario de edición rellenado automáticamente.</p>
                <p>
                    Si el extracto seleccionado no se encontraba publicado, podremos comprobar en la tabla que efectivamente el extracto ha sido modificado y que aparece un mensaje de notificación informándote de que la operación se ha completado con éxito.
                </p>

                <!--
                ╔═══════════════════════════╗
                ║     ELIMINAR EXTRACTO     ║
                ╚═══════════════════════════╝
                -->

                <h4 id="guia-eliminar-extracto" style="margin-top: 50px">Eliminar extracto</h4>
                <p>
                    Debemos hacer clic en la pestaña <asp:HyperLink runat="server" NavigateUrl="~/MisExtractos.aspx" Text="Mis extractos" />.
                </p>
                <p>
                    Veremos una tabla con todos los extractos que hayamos anotado. Cada extracto corresponde con una fila de la tabla. Cuando hayamos encontrado el extracto que deseemos editar, lo seleccionamos haciendo clic sobre él. Verás que la fila aparece ahora resaltada en gris, como ilustra la figura 8 para el extracto de ID 1175.
                </p>
                <p>
                    Después de haber seleccionado el extracto, solo debemos hacer clic en el botón "Eliminar" que se encuentra al final del formulario que se encuentra debajo de la tabla (figura 9).
                </p>
                <p>
                    Si el extracto seleccionado no se encontraba publicado, podremos comprobar en la tabla que efectivamente el extracto ha sido eliminado y que aparece un mensaje de notificación informándote de que la operación se ha completado con éxito.
                </p>
        
                <!--
                ╔════════════════════════════╗
                ║     FILTRO DE BÚSQUEDA     ║
                ╚════════════════════════════╝
                -->

                <h4 id="guia-filtro" style="margin-top: 50px">Utilizar el filtro de búsqueda</h4>
                <p>
                    Debemos hacer clic en la pestaña <asp:HyperLink runat="server" NavigateUrl="~/MisExtractos.aspx" Text="Mis extractos" />.
                </p>
                <p>
                    Es posible localizar el extracto utilizando el filtro de búsqueda que se encuentra encima de la tabla de los extractos anotados, compuesto por tres desplegables, a saber, el primero de florilegios, el segundo de autores, el tercereo de obras y el cuarto de estados (publicados o despublicados), como se observa en la figura 10.
                </p>
                <p>
                    En cada uno de los tres primeros desplegables aparecerán solo aquellos ítems que correspondan a los extractos de la tabla. Es decir, no debemos alarmarnos si por ejemplo en el desplegable de florilegios no vemos todos los florilegios, ya que solo aparecerán aquellos para los que hayamos escrito algún extracto. Es decir, si no hemos escrito ningún extracto para un florilegio en concreto, este no aparecerá en el desplegable de florilegios del filtro, ya que carece de sentido filtrar por un florilegio que no va a mostrarnos ningún extracto. Aplíquese el mismo razonamiento para los desplegables de autor y obra.
                </p>
                <asp:Image runat="server" ImageUrl="~/Images/Ayuda/10.png" AlternateText="Figura 10" />
                <p class="image-caption"><b>Figura 10.</b>&nbsp;Filtro de búsqueda.</p>
                <p>
                    Podemos hacer uso de cualquiera de ellos para filtrar los extractos mostrados en la tabla. No obstante, los tres primeros desplegables (florilegio, autor y obra) se encuentran sincronizados. Por ejemplo, si seleccionamos un florilegio cualquiera, los ítems de los desplegables de autores y obras se actualizarán, mostrándose únicamente aquellos vinculados al florilegio seleccionado. Lo mismo ocurre con el desplegable de obras en caso de haber seleccionado previamente un florilegio y un autor, que los ítems corresponderán a obras vinculadas al florilegio y autor seleccionados.
                </p>
                <p>
                    No obstante, no estamos obligados a seleccionar un florilegio ni un autor para poder seleccionar una obra. Podemos utilizar el desplegable de obras directamente, pero aparecerán todas las obras para las que hayamos anotado al menos un extracto. Lo mismo ocurre con el desplegable de autores en caso de no haber seleccionado ningún florilegio.
                </p>
                <p>
                    Nótese que cuando seleccionamos un ítem del desplegable de florilegios, los desplegables de autor y obra se resetean. Lo mismo ocurre con el desplegable de obras si hemos seleccionado un ítem del desplegable de autores. Esto se debe a que al estar sincronizados, los ítems de los desplegables afectados se renuevan para adecuarse a dicha selección, como explicamos anteriormente.
                </p>

                <!--
                ╔═════════════╗
                ║     PDF     ║
                ╚═════════════╝
                -->

                <h4 id="guia-pdf" style="margin-top: 50px">Exportar extractos anotados en PDF</h4>
                <p>
                    Debemos hacer clic en la pestaña <asp:HyperLink runat="server" NavigateUrl="~/MisExtractos.aspx" Text="Mis extractos" />.
                </p>
                <p>
                    Debajo de la tabla podremos ver un botón que pone "Generar PDF" que nos permitirá generar un fichero PDF con todos los extractos que aparezcan en ese momento en la tabla. El fichero en cuestión se abrirá en una nueva pestaña, desde la que podremos descargarlo en caso de que así lo deseemos.
                </p>
                <p>
                    Podemos seleccionar un subconjunto de los extractos que hayamos anotado utilizando el filtro que se encuentra encima de la tabla (figura 10). Es decir, si solo deseamos que en el fichero PDF aparezcan los extractos pertenecientes a un florilegio en cuestión, solo debemos utilizar el desplegable de florilegios del filtro y seleccionar el ítem correspondiente, actualizándose la tabla de extractos. Tras esto, si generamos el fichero PDF veremos que solo aparecerán los extractos pertenecientes a dicho florilegio. Aplíquese el mismo procedimiento para el resto de desplegables del filtro.
                </p>

                <!--
                ╔════════════════╗
                ║     CENTÓN     ║
                ╚════════════════╝
                -->

                <h4 id="guia-centon" style="margin-top: 50px">Anotar extractos hechos madiente la técnica del Centón</h4>
                <p>
                    En algunos extractos es posible encontrar que el compilador haya realizado un nuevo texto a partir de extractos de partes diferentes de un mismo poema o de obras diferentes. En estos casos hay que anotar el extracto tantas veces como obras o partes se haya utilizado para que así en la aplicación web de búsqueda se pueda localizar por todas las obras extractadas. Por ejemplo, el extracto del florilegio de G. Maior, recogido en la figura 11, es un centón a partir de Propercio y de Ovidio <i>Ars</i>:
                </p>
                <asp:Image runat="server" ImageUrl="~/Images/Ayuda/11.png" AlternateText="Figura 11" />
                <p class="image-caption"><b>Figura 11.</b>&nbsp;Extracto del florilegio G. Maior.</p>
                <p>
                    Para este ejemplo en concreto, debemos anotar el extracto dos veces, una como extracto vinculado a Propercio (figura 12) y otra como extracto vinculado a Ovidio (figura 13).
                </p>
                <asp:Image runat="server" ImageUrl="~/Images/Ayuda/12.png" AlternateText="Figura 12" />
                <p class="image-caption"><b>Figura 12.</b>&nbsp;Extracto vinculado a Propercio.</p>
                <p>
                    En el campo TLL debemos escribir la referencia del extracto que no hemos seleccionado en las líneas 1 y 2, que además deberá aparecer en segundo lugar tras el extracto principal.
                </p>
                <asp:Image runat="server" ImageUrl="~/Images/Ayuda/13.png" AlternateText="Figura 13" />
                <p class="image-caption"><b>Figura 13.</b>&nbsp;Extracto vinculado a Ovidio.</p>

                <!--
                ╔═══════════════╗
                ║     CANON     ║
                ╚═══════════════╝
                -->

                <h4 id="guia-canon" style="margin-top: 50px">El extracto no pertenece a una obra recogida en el canon que alimenta el TLL</h4>
                <p>
                    Existe la posibilidad de que el extracto pertenezca a un autor que no esté recogido por Phi Latin Text, en cuyo caso debemos contactar con el coordinador del proyecto, Gregorio Rodríguez Herrera, que le proporcionará el texto que actuará como TLL.
                </p>
                <p style="margin-bottom: 0">
                    Otra posibilidad es que el extracto del florilegio, a pesar de que el compilador lo haya asignado a un autor, sea en realidad un texto anónimo. En este caso debemos identificar el autor junto con la referencia que el compilador haya anotado. No obstante, en el campo TLL debemos escribir una línea de diez guiones bajos sin espacios (__________).
                </p>
            </div>
        </div>
    </div>
</asp:Content>
