using Othello_Game.Models;
using System.Data.Entity.Infrastructure;
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

        //
        // GET: /Home/Registro
        [HttpGet]
        public ActionResult Registro(string message = "")
        {
            ViewBag.Message = message;
            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre");
            return View();
        }

        //
        // POST: /Home/Login
        [HttpPost]
        public ActionResult Registro(string id_usuario,string nombres,string apellidos,string contrasenia, string fecha_nacimiento, string correo,int id_pais = 0)
        {
            if (id_usuario != null && nombres != null && apellidos != null && contrasenia != null
                && fecha_nacimiento != null && correo != null && id_pais != 0)
            {
                var new_jugador = db.Jugador.FirstOrDefault(e=>e.id_usuario == id_usuario);
                if (new_jugador == null) {// significa que los datos son correctos porque no hay registro de ese usuario

                    return RedirectToAction("Create", "Jugador", new
                    {
                        id_usuario,
                        nombres,
                        apellidos,
                        contrasenia,
                        fecha_nacimiento,
                        correo,
                        id_pais
                    });

                }
                else { // significa que el usario ya existe
                    return RedirectToAction("Registro", "Home", new { message = "Error, el Usuario ya existe" });
                }

            }
            else {
                return RedirectToAction("Registro", "Home", new { message = "Error en el ingreso de datos" });
            }

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