using System;
using System.Web.UI;

namespace Excerpta
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /*
        ╔════════════════╗
        ║     NAVBAR     ║
        ╚════════════════╝
        */

        // Ítems visibles

        public bool navbarLogin_Visible
        {
            get { return navbarLogin.Visible;  }
            set { navbarLogin.Visible = value; }
        }
        public bool navbarRegistro_Visible
        {
            get { return navbarRegistro.Visible;  }
            set { navbarRegistro.Visible = value; }
        }
        public bool navbarLogout_Visible
        {
            get { return navbarLogout.Visible;  }
            set { navbarLogout.Visible = value; }
        }
        public bool navbarBienvenida_Visible
        {
            get { return navbarBienvenida.Visible;  }
            set { navbarBienvenida.Visible = value; }
        }
        public bool navbarExtractos_Visible
        {
            get { return navbarExtractos.Visible;  }
            set { navbarExtractos.Visible = value; }
        }
        public bool navbarAdministracion_Visible
        {
            get { return navbarAdministracion.Visible;  }
            set { navbarAdministracion.Visible = value; }
        }

        // Ítem activo

        public string navbarLogin_Active
        {
            get { return navbarLogin.Attributes["Class"];  }
            set { navbarLogin.Attributes["Class"] = value; }
        }
        public string navbarRegistro_Active
        {
            get { return navbarRegistro.Attributes["Class"];  }
            set { navbarRegistro.Attributes["Class"] = value; }
        }
        public string navbarBienvenida_Active
        {
            get { return navbarBienvenida.Attributes["Class"];  }
            set { navbarBienvenida.Attributes["Class"] = value; }
        }
        public string navbarExtractos_Active
        {
            get { return navbarExtractos.Attributes["Class"];  }
            set { navbarExtractos.Attributes["Class"] = value; }
        }
        public string navbarAdministracion_Active
        {
            get { return navbarAdministracion.Attributes["Class"];  }
            set { navbarAdministracion.Attributes["Class"] = value; }
        }
    }
}
