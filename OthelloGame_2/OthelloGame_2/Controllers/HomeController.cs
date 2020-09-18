using OthelloGame_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OthelloGame_2.Controllers
{
    public class HomeController : Controller
    {
        private DataBase db = new DataBase();
        public ActionResult Index()
        {
            return View();
        }

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
                FormsAuthentication.SetAuthCookie(jugador.id_usuario, true);
                return RedirectToAction("Detalles", "Jugador");
            }
            else
            {

                return RedirectToAction("Login", "Home", new { message = "El usuario no existe o la contraseña es incorrecta" });
            }

        }
        //cerrar sesión
        [Authorize]
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}