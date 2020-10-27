using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OthelloGame_2.Models;

namespace OthelloGame_2.Controllers
{
    [Authorize] // significa que solo usuarios registrados entraran en esta modalidad
    public class JugadorController : Controller
    {
        private DataBase db = new DataBase();


        // GET: Jugador/Perfil
        [HttpGet]
        public ActionResult Perfil()
        {
            LimpiarBaseDeDatos();
            string id = User.Identity.Name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jugador jugador = db.Jugador.Find(id);
            if (jugador == null)
            {
                return HttpNotFound();
            }
            return View(jugador);
        }

        public void LimpiarBaseDeDatos()
        {
            List<Partida> PartidasEliminar = new List<Partida>();
            List<Ficha> FichasEliminar = new List<Ficha>();
            List<Jugador_Partida> J_P = new List<Jugador_Partida>();
            List<Jugador_P_C> J_P_C = new List<Jugador_P_C>();
            foreach (Partida partida in db.Partida.Where(e => e.id_ganador == ""))
            {
                PartidasEliminar.Add(partida);
                foreach (Ficha item in db.Ficha.Where(e => e.id_partida == partida.id_partida))
                {
                    FichasEliminar.Add(item);
                }
                foreach (Jugador_Partida item in db.Jugador_Partida.Where(e => e.id_Partida == partida.id_partida))
                {
                    J_P.Add(item);
                    foreach (Jugador_P_C item2 in db.Jugador_P_C.Where(e => e.id_J_P == item.id_J_P))
                    {
                        J_P_C.Add(item2);
                    }
                }
            }
            foreach (var item in FichasEliminar)
            {
                db.Ficha.Remove(item);
                db.SaveChanges();
            }
            foreach (var item in J_P_C)
            {
                db.Jugador_P_C.Remove(item);
                db.SaveChanges();
            }
            foreach (var item in J_P)
            {
                db.Jugador_Partida.Remove(item);
                db.SaveChanges();
            }
            foreach (var item in PartidasEliminar)
            {
                db.Partida.Remove(item);
                db.SaveChanges();
            }
        }

        // GET: Jugador/Create
        public ActionResult Create()
        {
            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre");
            return View();
        }

        // POST: Jugador/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_usuario,nombres,apellidos,contrasenia,fecha_nacimiento,correo,id_pais")] Jugador jugador)
        {
            if (ModelState.IsValid)
            {
                db.Jugador.Add(jugador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_pais = new SelectList(db.Pais, "id_pais", "nombre", jugador.id_pais);
            return View(jugador);
        }

    }
}
