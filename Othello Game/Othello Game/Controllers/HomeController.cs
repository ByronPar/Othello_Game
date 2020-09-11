using Othello_Game.Models;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Othello_Game.Controllers
{
    public class HomeController : Controller
    {
        private Othello_GameEntities db = new Othello_GameEntities();
        public ActionResult Index()
        {
            return View();
        }
        //
        // GET: /Home/Login
        [HttpGet]
        public ActionResult Login(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        //
        // POST: /Home/Login
        [HttpPost]
        public ActionResult Login(string username, string contrasenia)
        {
            var jugador = db.Jugador.FirstOrDefault(e => e.id_usuario == username && e.contrasenia == contrasenia);
            if (jugador != null)
            {
                FormsAuthentication.SetAuthCookie(jugador.id_usuario,true);
                return RedirectToAction("Details", "Jugador");
            }
            else
            {

                return RedirectToAction("Login", "Home", new { message = "El usuario no existe o la contraseña es incorrecta" });
            }

        }
        //
        // POST: /Account/LogOff
        [Authorize]
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}