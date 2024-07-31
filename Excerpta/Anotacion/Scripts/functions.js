$(window).on("load", function () {

    // Notifications
    $(document).on("click", ".notificacion", function () {
        $(".notificacion").fadeOut();
    });

    // Disabled link
    $(document).on("click", ".boton.disabled", function () {
        return false;
    });

    // Smooth Scrolling Links
    $(".smooth-scrolling").click(function(event) {
        event.preventDefault();
        var target = $(this).attr("href");
        $("html, body").animate({ scrollTop: $(target).offset().top }, 800, function () {
            window.location.hash = target;
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
    ╔═══════════════════════════╗
    ║     SMOOTH NAVIGATION     ║
    ╚═══════════════════════════╝
    */

    hideSpinner();

    function hideSpinner() {
        $('.preloader .logo').fadeOut(200, function () {
            $('.preloader').fadeOut(200);
        });
    }

    /*
    ╔════════════════╗
    ║     NAVBAR     ║
    ╚════════════════╝
    */

    // Burger Button
    $(document).on("click", "nav .mobile-button", function () {        
        if ($("nav .navbar-desktop").hasClass("opened")) {
            $(this).removeClass("opened");
            $("nav .navbar-desktop").removeClass("opened");
        }
        else {
            $(this).addClass("opened");
            $("nav .navbar-desktop").addClass("opened");
        }
    });

    /*
    ╔══════════════════════╗
    ║     OWL CAROUSEL     ║
    ╚══════════════════════╝
    */

    // Postback
    createCarousel();

    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () { createCarousel() });

    // Estas tres instrucciones de arriba son para que el carrusel, si está dentro de un UpdatePanel, no desaparezca al hacer postback

    // Configuración
    function createCarousel() {
        $('.loop').owlCarousel({
            autoplay: false,
            center: false,
            dots: false,
            items: 3,
            loop: false,
            mouseDrag: false,
            nav: true,
            navText: ["<", ">"],
            responsive: {
                0: {
                    items: 1,
                },
                768: {
                    items: 2,
                }
            }
        });
    }

    /*
    ╔════════════════╗
    ║     CUENTA     ║
    ╚════════════════╝
    */

    // Cambio de formulario
    $(document).on("click", ".pestanas li", function (e) {

        if ($(this).attr("data-formulario") != null) {

            e.preventDefault();
            var currentForm = "#" + $(".pestanas .active").attr("data-formulario");
            var targetForm  = "#" + $(this).attr("data-formulario");

            // Detenemos la ejecución si hemos seleccionado el mismo formulario
            if (currentForm == targetForm)
                return;

            // Detenemos la ejecución si hay animaciones en ejecución
            if ($(currentForm).is(":animated") || $(targetForm).is(":animated"))
                return;

            // Ocultamos el formulario actual
            $(currentForm).fadeOut(500);
            $(".pestanas .active").removeClass("active");

            // Mostramos el formulario seleccionado
            $(targetForm).delay(500).fadeIn(500);
            $(this).addClass("active");
        }
    });

    /*
    ╔═════════════════╗
    ║     BACKUPS     ║
    ╚═════════════════╝
    */

    // FileUpload
    $(document).on("click", "#subir-backup", function (e) {
        e.preventDefault();
        $("#MainContent_BotonImportarBackup").prop("disabled", false);
        $("#MainContent_BotonOcultoBackup").trigger("click");
    });

    /*
    ╔════════════════╗
    ║     TABLAS     ║
    ╚════════════════╝
    */

    // Extractos » Row Command (Select)
    $(document).on("click", "#MainContent_TablaExtractos tr", function () {

        var indiceFila = $(this).index();

        javascript: __doPostBack('ctl00$MainContent$TablaExtractos', 'Select$' + (indiceFila - 1));

    });

    // Usuarios » Row Command (Select)
    $(document).on("click", "#MainContent_TablaUsuarios tr", function () {

        var indiceFila = $(this).index();

        javascript: __doPostBack('ctl00$MainContent$TablaUsuarios', 'Select$' + (indiceFila - 1));

    });

    // Florilegios (tabla de florilegios) » Row Command (Select)
    $(document).on("click", "#MainContent_TablaFlorilegios tr", function () {

        var indiceFila = $(this).index();

        javascript: __doPostBack('ctl00$MainContent$TablaFlorilegios', 'Select$' + indiceFila);

    });

    // Florilegios (tabla de estructuras) » Row Command (Select)
    $(document).on("click", "#MainContent_TablaRegistros tr", function () {

        var indiceFila = $(this).index();

        javascript: __doPostBack('ctl00$MainContent$TablaRegistros', 'Select$' + (indiceFila -1));

    });

    // Bibliografía » Row Command (Select)
    $(document).on("click", "#MainContent_TablaBibliografia tr", function () {

        var indiceFila = $(this).index();

        javascript: __doPostBack('ctl00$MainContent$TablaBibliografia', 'Select$' + (indiceFila - 1));

    });

});