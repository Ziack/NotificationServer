using Nancy;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using NotificationServer.Service.Commands;
using NotificationServer.Service.Services;

namespace NotificationServer.Nancy.Modules
{
    public class LoginModule : NancyModule
    {
        private UserManagementService _users;

        public LoginModule(UserManagementService users)
            : base("/Notifications/Dashboard")
        {
            _users = users;

            Get["/Login"] = parameters =>
            {
                var mensajeError = "";
                var usuario = "";

                if (Session["Login/MensajeError"] != null)
                {
                    mensajeError = Session["Login/MensajeError"] as string;
                    usuario = Session["Login/Usuario"] as string;
                    Session["Login/Usuario"] = null;
                    Session["Login/MensajeError"] = null;
                }

                return View["Views/Login/Index.html", new
                {
                    MensajeError = mensajeError,
                    Username = usuario
                }];
            };

            Get["/Logout"] = parameters =>
            {
                return this.LogoutAndRedirect("/Notifications/Dashboard/Login");
            };

            Post["/Login"] = parameters =>
            {
                var loginData = this.Bind<CreateUserCommand>();

                var user = _users.Login(loginData.Username, loginData.UnencryptedPassword);

                if (user == null)
                {
                    Session["Login/MensajeError"] = "Usuario o clave incorrecto";
                    Session["Login/Usuario"] = loginData.Username;

                    return Response.AsRedirect("/Notifications/Dashboard/Login");
                }

                return this.LoginAndRedirect(user.Uid);
            };
        }
    }
}
