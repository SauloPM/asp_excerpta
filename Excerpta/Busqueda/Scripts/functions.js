$(window).on('load', function () {

    // Notifications
    $(document).on('click', '.notificacion', function () {
        $('.notificacion').fadeOut();
    });

    // Smooth Scrolling Links
    $(document).on('click', '.smooth-scrolling', function (event) {

        event.preventDefault();

        var target = $(this).attr('href');

        $('html, body').animate({ scrollTop: $(target).offset().top }, 800, function () {
            window.location.hash = target;
        });

    });

    /*
    ╔══════════════════╗
    ║     PARALLAX     ║
    ╚══════════════════╝
    */

    $(document).scroll(function () {
        parallax();
    });

    function parallax() {
        $('header').css('background-position', 'center ' + ($(document).scrollTop() * .5) + 'px');
    }

    /*
    ╔════════════════╗
    ║     NAVBAR     ║
    ╚════════════════╝
    */

    resizeNavbar();

    $(document).scroll(function () {
        resizeNavbar();
    });

    function resizeNavbar() {
        if ( $(this).scrollTop() > 100 && $( window ).width() > 768 )
            $('nav').addClass('blanco');
        else
            $('nav').removeClass('blanco');
    }

    $(document).on('click', '.burger-button', function () {

        var navbar = $( '#navbar' )

        if (navbar.hasClass('abierto'))
            navbar.removeClass( 'abierto' ).css( 'transform', 'scaleY(0)' )

        else
            navbar.addClass( 'abierto' ).css( 'transform', 'scaleY(1)' )

    })

    /*
    ╔══════════════════════════════╗
    ║     NAVEGACIÓN SUAVIZADA     ║
    ╚══════════════════════════════╝
    */

    smoothNavigation();

    function smoothNavigation() {

        // Ocultamos el fondo blanco con efecto fade y mostramos el contenido de la página
        $('.fondo-blanco').fadeOut(500, function () {
            setTimeout(function () {
                $('body').css('overflow', 'visible');
            }, 500);
        });
    }

    $(document).on('click', 'nav .brand, nav .items li a', function (e) {

        e.preventDefault();

        var url    = window.location.pathname;
        var target = $(this).attr('href');

        // Si hacemos clic en 'Créditos' y nos encontramos en la página de inicio, movemos el scroll hasta el apartado 'Proyecto'
        if ( $(this).parent().is(':nth-child(4)') ) {

            if (url == '/' || url.indexOf('Default') >= 0) {
                $('html, body').animate({
                    scrollTop: ($('#proyecto').offset().top)
                }, 500);
                window.location.hash = 'creditos';
                return;
            }
        }

        // Si hacemos clic en el ítem actual, no hacemos nada
        //if ($(this).parent().hasClass('active'))
        //    return;

        // Si hacemos clic en el brand y nos encontramos en la página de inicio, no hacemos nada
        //if ( $(this).hasClass('brand') && ( url == '/' || url.indexOf('Default') != -1 ))
        //    return;

        // Ocultamos la barra de scroll
        $('body').css('overflow', 'hidden');

        // Mostramos un fondo blanco con efecto fade y navegamos a otra URL
        $('.fondo-blanco').fadeIn(500, function () {
            setTimeout(function () {
                window.location.replace(target);
            }, 500);
        });

    });

    /*
    ╔════════════════════╗
    ║     TOP BUTTON     ║
    ╚════════════════════╝
    */

    showTopButton();

    $(document).scroll(function () {
        showTopButton();
    });

    function showTopButton() {
        if (document.body.scrollTop > 150 || document.documentElement.scrollTop > 150)
            $(".top-button").css("opacity", "1");
        else
            $(".top-button").css("opacity", "0");
    }

    /*
    ╔════════════════╗
    ║     INICIO     ║
    ╚════════════════╝
    */

    // Servicios
    $(document).on('click', '#servicios .servicio', function () {

        if ($(this).hasClass('activo')) {

            $(this).removeClass('activo')
            $(this).next().animate({ opacity: '0' }, { duration: 250, queue: false })

            setTimeout(() => {
                $(this).next().animate({ marginTop: '-275px' }, { duration: 500, queue: false })
            }, 100)

        } else {

            $(this).addClass("activo")
            $(this).next().animate({ marginTop: '0' }, { duration: 500, queue: false })

            setTimeout(() => {
                $(this).next().animate({ opacity: '1' }, { duration: 250, queue: false })
            }, 350)
        }
    })

    // Acordeón
    $(document).on('click', '.acordeon .acordeon-header', function() {

        var itemSeleccionado = $(this).parent()
        var cabecera         = $(this)
        var cuerpo           = itemSeleccionado.find('.acordeon-body')
        var altura           = cuerpo.css('height')
        var contenido        = cuerpo.find('ul')

        if ( itemSeleccionado.hasClass("activo") ) {

            contenido.animate( { opacity: '0' }, 250, function () {
                itemSeleccionado.delay(250).animate({ paddingBottom: '0' }, 500 )
            })

            setTimeout( () => {
                itemSeleccionado.removeClass("activo")
            }, 1000)

        } else {

            itemSeleccionado.addClass("activo")

            itemSeleccionado.animate({ paddingBottom: altura }, 500, function () {
                contenido.delay(250).animate({ opacity: '1' }, 250)
            })
        }
    })

    /*
    ╔════════════════╗
    ║     HOJEAR     ║
    ╚════════════════╝
    */

    // Tabla de florilegios » Row Command (Abrir)
    $(document).on("click", "#MainContent_TablaFlorilegios tr:not(:first-child)", function () {

        var indiceFila = $(this).index();

        javascript: __doPostBack('ctl00$MainContent$TablaFlorilegios', 'Abrir$' + (indiceFila - 1));

    });

    // Tabla de autores » Row Command (Abrir)
    $(document).on("click", "#MainContent_TablaAutores tr:not(:first-child)", function () {

        var indiceFila = $(this).index();

        javascript: __doPostBack('ctl00$MainContent$TablaAutores', 'Abrir$' + (indiceFila - 1));

    });

    // Tabla de obras » Row Command (Abrir)
    $(document).on("click", "#MainContent_TablaObras tr:not(:first-child)", function () {

        var indiceFila = $(this).index();

        javascript: __doPostBack('ctl00$MainContent$TablaObras', 'Abrir$' + (indiceFila - 1));

    });

    // Tabla de capítulos » Row Command (Abrir)
    $(document).on("click", "#MainContent_TablaCapitulos tr:not(:first-child)", function () {

        var indiceFila = $(this).index();

        javascript: __doPostBack('ctl00$MainContent$TablaCapitulos', 'Abrir$' + (indiceFila - 1));

    });

    /*
    ╔════════════════╗
    ║     BUSCAR     ║
    ╚════════════════╝
    */

    // Abrir / cerrar filtro de búsqueda
    $(document).on('click', '#busqueda .abrir-filtro .icono', function () {

        var filtroAbierto = $(this).parent().hasClass('activo');

        // Abrir
        if ( !filtroAbierto ) {

            $('#busqueda .filtro-formulario').fadeIn()
            $('.abrir-filtro' ).addClass('activo')

        }

        // Cerrar
        else {

            $('#busqueda .filtro-formulario').fadeOut()
            $('.abrir-filtro').removeClass('activo')
            
        }
    })

    // Cambio de pestaña (de búsqueda simple a avanzada o viceversa)
    $(document).on("click", "#busqueda .filtro-formulario .pestanas span", function () {
        if (!$(this).hasClass("active")) {

            // Cambiamos de pestaña
            $("#busqueda .pestanas .active").removeClass("active");
            $(this).addClass("active");

            // Cambiamos de formulario
            var target   = "." + $(this).attr("data-target");
            var noTarget = target == ".filtro-simple" ? ".filtro-avanzado" : ".filtro-simple";

            $("#busqueda .filtro-simple, #busqueda .filtro-avanzado").css("opacity", "0");

            setTimeout(() => {
                $("#busqueda " + noTarget).css("display", "none" );
                $("#busqueda " + target  ).css("display", "block");
            }, 250);

            setTimeout(() => {
                $("#busqueda " + target).css("opacity", "1");
            }, 500);
        }
    });

    // Abrir extracto en una nueva pestaña
    $(document).on("click", "#busqueda .tabla > div > table > tbody > tr:not(.paginador):not(:first-child)", function () {

        var id   = $(this).find("td:nth-child(8)").html();
        var ruta = "Extracto.aspx?id=" + id;

        try {
            var ok = window.open(ruta, '_blank');

            // Browser has allowed it to be opened
            if (ok)
                ok.focus();
        }
        catch (exception) {
            // ...
        }

    });

    // Abrir la guía de ayuda desde el glosario de comandos
    $(document).on("click", "#operadores .boton", function () {

        $("#operadores .close").trigger("click");

        setTimeout(() => {
            $(".boton-ayuda").trigger("click");
        }, 500);

    });

    /*
    ╔════════════════════╗
    ║     BIBLIOTECA     ║
    ╚════════════════════╝
    */

    // Buscador
    $(document).on('input', '#biblioteca .buscador', function() {

        var referencia = ''
        var secuencia = $(this).val().toLowerCase().trim()

        $('#biblioteca .lista table tr td:nth-child(2)').each(function() {

            referencia = $(this).html().toLowerCase()

            // Ha habido coincidencias o no se ha escrito nada
            if ((referencia.indexOf(secuencia) > -1) || (secuencia.length == 0)) {
                $(this).parent().css('display', '');
            }

            // No ha habido coincidencia
            else {
                $(this).parent().css('display', 'none')
            }
        })
    })
})