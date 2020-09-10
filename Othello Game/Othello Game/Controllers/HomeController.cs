using Othello_Game.Models;
using System.Linq;
using System.Web.Mvc;

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
            ViewBag.Paises = new SelectList(db.Pais, "id_pais", "nombre");
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
                return RedirectToAction("Index", "Jugador", jugador);
            }
            else
            {

                return RedirectToAction("Login", "Home", new { message = "El usuario no existe o la contraseña es incorrecta" });
            }

        }

        public ActionResult Registro()
        {

            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        public ActionResult LogOff()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}