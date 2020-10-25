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
            //limpio base de datos 
            var metodo = new JugadorController();
            metodo.LimpiarBaseDeDatos();
            //termino de limpiar partidas que no contengan un ganador
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
                return RedirectToAction("Perfil", "Jugador");
            }
            else
            {
                return RedirectToAction("Login", "Home", new { message = "El usuario no existe o la contraseña es incorrecta" });
            }

        }


        
        // GET: Jugador/Create
        [HttpGet]
        public ActionResult Registro(string message = "")
        {
            if (message == "exito")
            {
                ViewBag.Message2 = "El usuario fue creado con Exito";
            }
            else if (message == "fracaso")
            {
                ViewBag.Message = "Error, el Usuario ya existe o dejo un campo vacio";
            }
            else
            {
                ViewBag.Message = message;
            }

            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre");
            return View();
        }

        [HttpPost]
        public ActionResult Registro([Bind(Include = "id_usuario,nombres,apellidos,contrasenia,fecha_nacimiento,correo,id_pais")] Jugador jugador)
        {
            var new_jugador = db.Jugador.FirstOrDefault(e => e.id_usuario == jugador.id_usuario);
            if (new_jugador == null && ModelState.IsValid && jugador.id_pais != 0 && jugador.nombres != null
                && jugador.apellidos != null && jugador.id_usuario != null
                && jugador.contrasenia != null && jugador.correo != null)
            {// significa que los datos son correctos porque no hay registro de ese usuario
                db.Jugador.Add(jugador);
                db.SaveChanges();
                return RedirectToAction("Registro", "Home", new { message = "exito" });
            }
            else
            { // significa que el usario ya existe o hay error en los datos
                return RedirectToAction("Registro", "Home", new { message = "fracaso" });
            }
        }
        //cerrar sesión
        [Authorize]
        public ActionResult CerrarSesion()
        {
            //limpio base de datos 
            var metodo = new JugadorController();
            metodo.LimpiarBaseDeDatos();
            //termino de limpiar partidas que no contengan un ganador
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}