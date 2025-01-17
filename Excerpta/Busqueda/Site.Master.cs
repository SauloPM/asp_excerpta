﻿using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Busqueda
{
    public partial class SiteMaster : MasterPage
    {
        private string       _antiXsrfTokenValue;
        private const string AntiXsrfTokenKey    = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";

        protected void Page_Init(object sender, EventArgs e)
        {
            // El código siguiente ayuda a proteger frente a ataques XSRF
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Utilizar el token Anti-XSRF de la cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generar un nuevo token Anti-XSRF y guardarlo en la cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Establecer token Anti-XSRF
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validar el token Anti-XSRF
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Error de validación del token Anti-XSRF.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // ────────────── //
        //     NAVBAR     //
        // ────────────── //

        // Ítems activos

        public string NavHojear_Active
        {
            get { return NavHojear.Attributes["Class"]; }
            set { NavHojear.Attributes["Class"] = value; }
        }

        public string NavBuscar_Active
        {
            get { return NavBuscar.Attributes["Class"]; }
            set { NavBuscar.Attributes["Class"] = value; }
        }

        public string NavBiblioteca_Active
        {
            get { return NavBiblioteca.Attributes["Class"]; }
            set { NavBiblioteca.Attributes["Class"] = value; }
        }

        public string NavCreditos_Active
        {
            get { return NavCreditos.Attributes["Class"]; }
            set { NavCreditos.Attributes["Class"] = value; }
        }
    }
}